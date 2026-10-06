/*
  Copyright (C) 2009-2013 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

using IKVM.Reflection.Metadata;

using SrmMetadataReader = System.Reflection.Metadata.MetadataReader;
using SrmPEReader = System.Reflection.PortableExecutable.PEReader;

namespace IKVM.Reflection.Reader
{

    sealed class ModuleReader : Module
    {

        sealed class LazyForwardedType
        {

            readonly int index;
            Type type;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="index"></param>
            internal LazyForwardedType(int index)
            {
                this.index = index;
            }

            internal Type GetType(ModuleReader module)
            {
                // guard against circular type forwarding
                if (type == MarkerType.LazyResolveInProgress)
                {
                    var typeName = module.GetTypeName(module.ExportedTypeTable.records[index].TypeNamespace, module.ExportedTypeTable.records[index].TypeName);
                    return module.Universe.GetMissingTypeOrThrow(module, module, null, typeName);
                }
                else if (type == null)
                {
                    type = MarkerType.LazyResolveInProgress;
                    type = module.ResolveExportedType(index);
                }

                return type;
            }

        }

        readonly Stream stream;
        readonly string location;
        Assembly assembly;
        readonly SrmPEReader pe;
        readonly byte[] metadataImage;
        GCHandle metadataImageHandle;
        readonly SrmMetadataReader metadata;
        readonly int blobHeapOffset;
        int metadataStreamVersion;
        readonly Dictionary<int, string> userStrings = new Dictionary<int, string>();
        TypeDefImpl[] typeDefs;
        TypeDefImpl moduleType;
        Assembly[] assemblyRefs;
        Type[] typeRefs;
        Type[] typeSpecs;
        FieldInfo[] fields;
        MethodBase[] methods;
        MemberInfo[] memberRefs;
        Dictionary<StringHandle, string> strings = new Dictionary<StringHandle, string>();
        Dictionary<TypeName, Type> types = new Dictionary<TypeName, Type>();
        Dictionary<TypeName, LazyForwardedType> forwardedTypes = new Dictionary<TypeName, LazyForwardedType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="universe"></param>
        /// <param name="stream"></param>
        /// <param name="location"></param>
        /// <param name="mapped"></param>
        internal ModuleReader(AssemblyReader assembly, Universe universe, Stream stream, string location, bool mapped) :
            base(universe)
        {
            this.stream = stream;
            this.location = location;

            // one copy of the metadata, pinned so that the metadata reader and the signature decoders share it
            pe = new SrmPEReader(stream, PEStreamOptions.LeaveOpen | (mapped ? PEStreamOptions.IsLoadedImage : PEStreamOptions.Default));
            if (pe.HasMetadata == false)
                throw new BadImageFormatException("The image has no metadata.");

            unsafe
            {
                var block = pe.GetMetadata();
                metadataImage = new byte[block.Length];
                Marshal.Copy((IntPtr)block.Pointer, metadataImage, 0, block.Length);
                metadataImageHandle = GCHandle.Alloc(metadataImage, GCHandleType.Pinned);
                metadata = new SrmMetadataReader((byte*)metadataImageHandle.AddrOfPinnedObject(), metadataImage.Length, MetadataReaderOptions.None);
            }

            blobHeapOffset = metadata.GetHeapMetadataOffset(HeapIndex.Blob);
            ReadTables();

            if (assembly == null && AssemblyTable.records.Length != 0)
                assembly = new AssemblyReader(location, this);

            this.assembly = assembly;
        }

        /// <summary>
        /// Reads the tables from the #~ or #- stream of the metadata image.
        /// </summary>
        void ReadTables()
        {
            var br = new BinaryReader(new MemoryStream(metadataImage, false));
            foreach (var sh in ReadStreamHeaders(br))
            {
                if (sh.Name is "#~" or "#-")
                {
                    br.BaseStream.Position = sh.Offset;
                    ReadTables(br);
                    return;
                }
            }
        }

        static StreamHeader[] ReadStreamHeaders(BinaryReader br)
        {
            var signature = br.ReadUInt32();
            if (signature != 0x424A5342)
                throw new BadImageFormatException("Invalid metadata signature");

            br.ReadUInt16(); // major version
            br.ReadUInt16(); // minor version
            br.ReadUInt32(); // reserved
            var length = br.ReadUInt32();
            br.ReadBytes((int)length); // version
            br.ReadUInt16(); // flags

            var streams = br.ReadUInt16();
            var streamHeaders = new StreamHeader[streams];
            for (int i = 0; i < streamHeaders.Length; i++)
            {
                streamHeaders[i] = new StreamHeader();
                streamHeaders[i].Read(br);
            }

            return streamHeaders;
        }

        void ReadTables(BinaryReader br)
        {
            var tables = GetTables();

            /*uint Reserved0 =*/
            br.ReadUInt32();
            var majorVersion = br.ReadByte();
            var minorVersion = br.ReadByte();
            metadataStreamVersion = majorVersion << 16 | minorVersion;
            var heapSizes = br.ReadByte();
            /*byte Reserved7 =*/
            br.ReadByte();

            ulong valid = br.ReadUInt64();
            ulong sorted = br.ReadUInt64();
            for (int i = 0; i < 64; i++)
            {
                if ((valid & (1UL << i)) != 0)
                {
                    tables[i].Sorted = (sorted & (1UL << i)) != 0;
                    tables[i].RowCount = br.ReadInt32();
                }
            }

            var mr = new MetadataReader(this, br.BaseStream, heapSizes);
            for (int i = 0; i < 64; i++)
                if ((valid & (1UL << i)) != 0)
                    tables[i].Read(mr);

            if (ParamPtrTable.RowCount != 0)
                throw new NotImplementedException("ParamPtr table support has not yet been implemented.");
        }

        internal override void GetTypesImpl(List<Type> list)
        {
            PopulateTypeDef();

            foreach (var type in typeDefs)
                if (type != moduleType)
                    list.Add(type);
        }

        void PopulateTypeDef()
        {
            if (typeDefs == null)
            {
                typeDefs = new TypeDefImpl[TypeDefTable.records.Length];
                for (int i = 0; i < typeDefs.Length; i++)
                {
                    var type = new TypeDefImpl(this, i);
                    typeDefs[i] = type;
                    if (type.IsModulePseudoType)
                        moduleType = type;
                    else if (!type.IsNestedByFlags)
                        types.Add(type.TypeName, type);
                }

                // add forwarded types to forwardedTypes dictionary (because Module.GetType(string) should return them)
                for (int i = 0; i < ExportedTypeTable.records.Length; i++)
                {
                    int implementation = ExportedTypeTable.records[i].Implementation;
                    if (implementation >> 24 == AssemblyRefTable.Index)
                    {
                        var typeName = GetTypeName(ExportedTypeTable.records[i].TypeNamespace, ExportedTypeTable.records[i].TypeName);
                        forwardedTypes.Add(typeName, new LazyForwardedType(i));
                    }
                }
            }
        }

        internal override string GetString(StringHandle handle)
        {
            if (handle.IsNil)
                return null;

            if (strings.TryGetValue(handle, out var str) == false)
                strings.Add(handle, str = metadata.GetString(handle));

            return str;
        }

        internal byte[] GetBlobCopy(BlobHandle handle) => metadata.GetBlobBytes(handle);

        internal override ByteReader GetBlobReader(BlobHandle handle) => ByteReader.FromBlob(metadataImage, blobHeapOffset, handle);

        public override string ResolveString(int metadataToken)
        {
            if ((metadataToken >> 24) != 0x70)
                throw TokenOutOfRangeException(metadataToken);

            if (userStrings.TryGetValue(metadataToken, out var str) == false)
                userStrings.Add(metadataToken, str = metadata.GetUserString(MetadataTokens.UserStringHandle(metadataToken & 0xFFFFFF)));

            return str;
        }

        internal override Type ResolveType(int metadataToken, IGenericContext context)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
                throw TokenOutOfRangeException(metadataToken);

            if ((metadataToken >> 24) == TypeDefTable.Index && index < TypeDefTable.RowCount)
            {
                PopulateTypeDef();
                return typeDefs[index];
            }

            if ((metadataToken >> 24) == TypeRefTable.Index && index < TypeRefTable.RowCount)
            {
                typeRefs ??= new Type[TypeRefTable.records.Length];

                if (typeRefs[index] == null)
                {
                    var scope = TypeRefTable.records[index].ResolutionScope;
                    switch (scope >> 24)
                    {
                        case AssemblyRefTable.Index:
                            {
                                var assembly = ResolveAssemblyRef((scope & 0xFFFFFF) - 1);
                                var typeName = GetTypeName(TypeRefTable.records[index].TypeNamespace, TypeRefTable.records[index].TypeName);
                                typeRefs[index] = assembly.ResolveType(this, typeName);
                                break;
                            }
                        case TypeRefTable.Index:
                            {
                                var outer = ResolveType(scope, null);
                                var typeName = GetTypeName(TypeRefTable.records[index].TypeNamespace, TypeRefTable.records[index].TypeName);
                                typeRefs[index] = outer.ResolveNestedType(this, typeName);
                                break;
                            }
                        case ModuleTable.Index:
                        case ModuleRefTable.Index:
                            {
                                Module module;

                                if (scope >> 24 == ModuleTable.Index)
                                {
                                    if (scope == 0 || scope == 1)
                                        module = this;
                                    else
                                        throw new NotImplementedException("self reference scope?");
                                }
                                else
                                {
                                    module = ResolveModuleRef(ModuleRefTable.records[(scope & 0xFFFFFF) - 1]);
                                }

                                var typeName = GetTypeName(TypeRefTable.records[index].TypeNamespace, TypeRefTable.records[index].TypeName);
                                typeRefs[index] = module.FindType(typeName) ?? module.Universe.GetMissingTypeOrThrow(this, module, null, typeName);
                                break;
                            }
                        default:
                            throw new NotImplementedException("ResolutionScope = " + scope.ToString("X"));
                    }
                }

                return typeRefs[index];
            }

            if ((metadataToken >> 24) == TypeSpecTable.Index && index < TypeSpecTable.RowCount)
            {
                typeSpecs ??= new Type[TypeSpecTable.records.Length];

                var type = typeSpecs[index];
                if (type == null)
                {
                    var tc = context == null ? null : new TrackingGenericContext(context);
                    typeSpecs[index] = MarkerType.LazyResolveInProgress;

                    try
                    {
                        type = Signature.ReadTypeSpec(this, GetBlobReader(TypeSpecTable.records[index]), tc);
                    }
                    finally
                    {
                        typeSpecs[index] = null;
                    }

                    if (tc == null || !tc.IsUsed)
                        typeSpecs[index] = type;
                }
                else if (type == MarkerType.LazyResolveInProgress)
                {
                    if (Universe.MissingMemberResolution)
                    {
                        return Universe
                            .GetMissingTypeOrThrow(this, this, null, new TypeName(null, "Cyclic TypeSpec " + metadataToken.ToString("X")))
                            .SetMetadataTokenForMissing(metadataToken, 0);
                    }

                    throw new BadImageFormatException("Cyclic TypeSpec " + metadataToken.ToString("X"));
                }

                return type;
            }

            throw TokenOutOfRangeException(metadataToken);
        }

        Module ResolveModuleRef(StringHandle moduleNameIndex)
        {
            var moduleName = GetString(moduleNameIndex);
            return assembly.GetModule(moduleName) ?? throw new FileNotFoundException(moduleName);
        }

        sealed class TrackingGenericContext : IGenericContext
        {

            readonly IGenericContext context;
            bool used;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            internal TrackingGenericContext(IGenericContext context)
            {
                this.context = context;
            }

            internal bool IsUsed
            {
                get { return used; }
            }

            public Type GetGenericTypeArgument(int index)
            {
                used = true;
                return context.GetGenericTypeArgument(index);
            }

            public Type GetGenericMethodArgument(int index)
            {
                used = true;
                return context.GetGenericMethodArgument(index);
            }

        }

        TypeName GetTypeName(StringHandle typeNamespace, StringHandle typeName)
        {
            return new TypeName(GetString(typeNamespace), GetString(typeName));
        }

        internal Assembly ResolveAssemblyRef(int index)
        {
            assemblyRefs ??= new Assembly[AssemblyRefTable.RowCount];
            assemblyRefs[index] ??= ResolveAssemblyRefImpl(ref AssemblyRefTable.records[index]);
            return assemblyRefs[index];
        }

        Assembly ResolveAssemblyRefImpl(ref AssemblyRefTable.Record rec)
        {
            const int PublicKey = 0x0001;

            var name = AssemblyName.GetFullName(
                GetString(rec.Name),
                rec.MajorVersion,
                rec.MinorVersion,
                rec.BuildNumber,
                rec.RevisionNumber,
                rec.Culture.IsNil ? "neutral" : GetString(rec.Culture),
                rec.PublicKeyOrToken.IsNil ? Array.Empty<byte>() : (rec.Flags & PublicKey) == 0 ? GetBlobCopy(rec.PublicKeyOrToken) : AssemblyName.ComputePublicKeyToken(GetBlobCopy(rec.PublicKeyOrToken)),
                rec.Flags);

            return Universe.Load(name, this, true);
        }

        public override Guid ModuleVersionId => metadata.GetGuid(metadata.GetModuleDefinition().Mvid);

        public override string FullyQualifiedName => location ?? "<Unknown>";

        public override string Name => location == null ? "<Unknown>" : Path.GetFileName(location);

        public override Assembly Assembly => assembly;

        internal override Type FindType(TypeName typeName)
        {
            PopulateTypeDef();

            if (!types.TryGetValue(typeName, out var type))
                if (forwardedTypes.TryGetValue(typeName, out var fw))
                    return fw.GetType(this);

            return type;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            PopulateTypeDef();

            foreach (var type in types.Values)
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            foreach (var name in forwardedTypes.Keys)
                if (name.ToLowerInvariant() == lowerCaseName)
                    return forwardedTypes[name].GetType(this);

            return null;
        }

        Exception TokenOutOfRangeException(int metadataToken)
        {
            return new ArgumentOutOfRangeException("metadataToken", String.Format("Token 0x{0:x8} is not valid in the scope of module {1}.", metadataToken, this.Name));
        }

        public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            switch (metadataToken >> 24)
            {
                case FieldTable.Index:
                    return ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
                case MemberRefTable.Index:
                    int index = (metadataToken & 0xFFFFFF) - 1;
                    if (index < 0 || index >= MemberRefTable.RowCount)
                        goto default;

                    return GetMemberRef(index, genericTypeArguments, genericMethodArguments);
                case MethodDefTable.Index:
                case MethodSpecTable.Index:
                    return ResolveMethod(metadataToken, genericTypeArguments, genericMethodArguments);
                case TypeRefTable.Index:
                case TypeDefTable.Index:
                case TypeSpecTable.Index:
                    return ResolveType(metadataToken, genericTypeArguments, genericMethodArguments);
                default:
                    throw TokenOutOfRangeException(metadataToken);
            }
        }

        internal FieldInfo GetFieldAt(TypeDefImpl owner, int index)
        {
            fields ??= new FieldInfo[FieldTable.records.Length];
            fields[index] ??= new FieldDefImpl(this, owner ?? FindFieldOwner(index), index);
            return fields[index];
        }

        public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
            {
                throw TokenOutOfRangeException(metadataToken);
            }
            else if ((metadataToken >> 24) == FieldTable.Index && index < FieldTable.RowCount)
            {
                return GetFieldAt(null, index);
            }
            else if ((metadataToken >> 24) == MemberRefTable.Index && index < MemberRefTable.RowCount)
            {
                var field = GetMemberRef(index, genericTypeArguments, genericMethodArguments) as FieldInfo;
                if (field != null)
                    return field;

                throw new ArgumentException(String.Format("Token 0x{0:x8} is not a valid FieldInfo token in the scope of module {1}.", metadataToken, this.Name), "metadataToken");
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        TypeDefImpl FindFieldOwner(int fieldIndex)
        {
            // TODO use binary search?
            for (int i = 0; i < TypeDefTable.records.Length; i++)
            {
                var field = TypeDefTable.records[i].FieldList - 1;
                var end = TypeDefTable.records.Length > i + 1 ? TypeDefTable.records[i + 1].FieldList - 1 : FieldTable.records.Length;
                if (field <= fieldIndex && fieldIndex < end)
                {
                    PopulateTypeDef();
                    return typeDefs[i];
                }
            }

            throw new InvalidOperationException();
        }

        internal MethodBase GetMethodAt(TypeDefImpl owner, int index)
        {
            methods ??= new MethodBase[MethodDefTable.records.Length];
            if (methods[index] == null)
            {
                var method = new MethodDefImpl(this, owner ?? FindMethodOwner(index), index);
                methods[index] = method.IsConstructor ? new ConstructorInfoImpl(method) : (MethodBase)method;
            }

            return methods[index];
        }

        public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
            {
                throw TokenOutOfRangeException(metadataToken);
            }
            else if ((metadataToken >> 24) == MethodDefTable.Index && index < MethodDefTable.RowCount)
            {
                return GetMethodAt(null, index);
            }
            else if ((metadataToken >> 24) == MemberRefTable.Index && index < MemberRefTable.RowCount)
            {
                var method = GetMemberRef(index, genericTypeArguments, genericMethodArguments) as MethodBase;
                if (method != null)
                    return method;

                throw new ArgumentException(String.Format("Token 0x{0:x8} is not a valid MethodBase token in the scope of module {1}.", metadataToken, this.Name), "metadataToken");
            }
            else if ((metadataToken >> 24) == MethodSpecTable.Index && index < MethodSpecTable.RowCount)
            {
                var method = (MethodInfo)ResolveMethod(MethodSpecTable.records[index].Method, genericTypeArguments, genericMethodArguments);
                var instantiation = GetBlobReader(MethodSpecTable.records[index].Instantiation);
                return method.MakeGenericMethod(Signature.ReadMethodSpec(this, instantiation, new GenericContext(genericTypeArguments, genericMethodArguments)));
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        public override string ScopeName
        {
            get { return GetString(ModuleTable.records[0].Name); }
        }

        TypeDefImpl FindMethodOwner(int methodIndex)
        {
            // TODO use binary search?
            for (int i = 0; i < TypeDefTable.records.Length; i++)
            {
                int method = TypeDefTable.records[i].MethodList - 1;
                int end = TypeDefTable.records.Length > i + 1 ? TypeDefTable.records[i + 1].MethodList - 1 : MethodDefTable.records.Length;
                if (method <= methodIndex && methodIndex < end)
                {
                    PopulateTypeDef();
                    return typeDefs[i];
                }
            }

            throw new InvalidOperationException();
        }

        MemberInfo GetMemberRef(int index, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            memberRefs ??= new MemberInfo[MemberRefTable.records.Length];

            if (memberRefs[index] == null)
            {
                var owner = MemberRefTable.records[index].Class;
                var sig = MemberRefTable.records[index].Signature;
                var name = GetString(MemberRefTable.records[index].Name);
                switch (owner >> 24)
                {
                    case MethodDefTable.Index:
                        return GetMethodAt(null, (owner & 0xFFFFFF) - 1);
                    case ModuleRefTable.Index:
                        memberRefs[index] = ResolveTypeMemberRef(ResolveModuleType(owner), name, GetBlobReader(sig));
                        break;
                    case TypeDefTable.Index:
                    case TypeRefTable.Index:
                        memberRefs[index] = ResolveTypeMemberRef(ResolveType(owner), name, GetBlobReader(sig));
                        break;
                    case TypeSpecTable.Index:
                        {
                            var type = ResolveType(owner, genericTypeArguments, genericMethodArguments);
                            if (type.IsArray)
                            {
                                var methodSig = MethodSignature.ReadSig(this, GetBlobReader(sig), new GenericContext(genericTypeArguments, genericMethodArguments));
                                return type.FindMethod(name, methodSig) ?? Universe.GetMissingMethodOrThrow(this, type, name, methodSig);
                            }
                            else if (type.IsConstructedGenericType)
                            {
                                var member = ResolveTypeMemberRef(type.GetGenericTypeDefinition(), name, GetBlobReader(sig));
                                var mb = member as MethodBase;
                                if (mb != null)
                                    member = mb.BindTypeParameters(type);

                                var fi = member as FieldInfo;
                                if (fi != null)
                                    member = fi.BindTypeParameters(type);

                                return member;
                            }
                            else
                            {
                                return ResolveTypeMemberRef(type, name, GetBlobReader(sig));
                            }
                        }
                    default:
                        throw new BadImageFormatException();
                }
            }

            return memberRefs[index];
        }

        Type ResolveModuleType(int token)
        {
            int index = (token & 0xFFFFFF) - 1;
            var name = GetString(ModuleRefTable.records[index]);
            var module = assembly.GetModule(name);
            if (module == null || module.IsResource())
                throw new BadImageFormatException();

            return module.GetModuleType();
        }

        MemberInfo ResolveTypeMemberRef(Type type, string name, ByteReader sig)
        {
            if (sig.PeekByte() == Signature.FIELD)
            {
                var org = type;
                var fieldSig = FieldSignature.ReadSig(this, sig, type);
                var field = type.FindField(name, fieldSig);
                if (field == null && Universe.MissingMemberResolution)
                    return Universe.GetMissingFieldOrThrow(this, type, name, fieldSig);

                while (field == null && (type = type.BaseType) != null)
                    field = type.FindField(name, fieldSig);

                if (field != null)
                    return field;

                throw new MissingFieldException(org.ToString(), name);
            }
            else
            {
                var org = type;
                var methodSig = MethodSignature.ReadSig(this, sig, type);
                var method = type.FindMethod(name, methodSig);
                if (method == null && Universe.MissingMemberResolution)
                    return Universe.GetMissingMethodOrThrow(this, type, name, methodSig);

                while (method == null && (type = type.BaseType) != null)
                    method = type.FindMethod(name, methodSig);

                if (method != null)
                    return method;

                throw new MissingMethodException(org.ToString(), name);
            }
        }

        internal ByteReader GetStandAloneSig(int index)
        {
            return GetBlobReader(StandAloneSigTable.records[index]);
        }

        public override byte[] ResolveSignature(int metadataToken)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if ((metadataToken >> 24) == StandAloneSigTable.Index && index >= 0 && index < StandAloneSigTable.RowCount)
            {
                var br = GetStandAloneSig(index);
                return br.ReadBytes(br.Length);
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        internal MethodInfo GetEntryPoint()
        {
            var cor = pe.PEHeaders.CorHeader;
            if (cor.EntryPointTokenOrRelativeVirtualAddress != 0 && (cor.Flags & CorFlags.NativeEntryPoint) == 0)
                return (MethodInfo)ResolveMethod(cor.EntryPointTokenOrRelativeVirtualAddress);

            return null;
        }

        internal ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            for (int i = 0; i < ManifestResourceTable.records.Length; i++)
            {
                if (resourceName == GetString(ManifestResourceTable.records[i].Name))
                {
                    var info = new ManifestResourceInfo(this, i);
                    var asm = info.ReferencedAssembly;
                    if (asm != null && !asm.__IsMissing && asm.GetManifestResourceInfo(resourceName) == null)
                        return null;

                    return info;
                }
            }

            return null;
        }

        internal Stream GetManifestResourceStream(string resourceName)
        {
            for (int i = 0; i < ManifestResourceTable.records.Length; i++)
            {
                if (resourceName == GetString(ManifestResourceTable.records[i].Name))
                {
                    if (ManifestResourceTable.records[i].Implementation != 0x26000000)
                    {
                        var info = new ManifestResourceInfo(this, i);
                        switch (ManifestResourceTable.records[i].Implementation >> 24)
                        {
                            case FileTable.Index:
                                var fileName = Path.Combine(Path.GetDirectoryName(location), info.FileName);
                                if (System.IO.File.Exists(fileName))
                                {
                                    // note that, like System.Reflection, we return null for zero length files and
                                    // ManifestResource.Offset is ignored
                                    var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
                                    if (fs.Length == 0)
                                    {
                                        fs.Dispose();
                                        return null;
                                    }

                                    return fs;
                                }

                                return null;
                            case AssemblyRefTable.Index:
                                var asm = info.ReferencedAssembly;
                                if (asm.__IsMissing)
                                    return null;

                                return asm.GetManifestResourceStream(resourceName);
                            default:
                                throw new BadImageFormatException();
                        }
                    }
                    // an embedded resource is its length followed by its content
                    var data = pe.GetSectionData(pe.PEHeaders.CorHeader.ResourcesDirectory.RelativeVirtualAddress + ManifestResourceTable.records[i].Offset).GetReader();
                    var length = data.ReadInt32();
                    return new MemoryStream(data.ReadBytes(length));
                }
            }

            return null;
        }

        public AssemblyName[] __GetReferencedAssemblies()
        {
            var list = new List<AssemblyName>();
            for (int i = 0; i < AssemblyRefTable.records.Length; i++)
            {
                var name = new AssemblyName();
                name.Name = GetString(AssemblyRefTable.records[i].Name);
                name.Version = new Version(
                    AssemblyRefTable.records[i].MajorVersion,
                    AssemblyRefTable.records[i].MinorVersion,
                    AssemblyRefTable.records[i].BuildNumber,
                    AssemblyRefTable.records[i].RevisionNumber);

                if (AssemblyRefTable.records[i].PublicKeyOrToken.IsNil == false)
                {
                    byte[] keyOrToken = GetBlobCopy(AssemblyRefTable.records[i].PublicKeyOrToken);
                    const int PublicKey = 0x0001;
                    if ((AssemblyRefTable.records[i].Flags & PublicKey) != 0)
                        name.SetPublicKey(keyOrToken);
                    else
                        name.SetPublicKeyToken(keyOrToken);
                }
                else
                {
                    name.SetPublicKeyToken(Array.Empty<byte>());
                }

                if (AssemblyRefTable.records[i].Culture.IsNil == false)
                    name.CultureName = GetString(AssemblyRefTable.records[i].Culture);
                else
                    name.CultureName = "";

                if (AssemblyRefTable.records[i].HashValue.IsNil == false)
                    name.hash = GetBlobCopy(AssemblyRefTable.records[i].HashValue);

                name.RawFlags = (AssemblyNameFlags)AssemblyRefTable.records[i].Flags;
                list.Add(name);
            }

            return list.ToArray();
        }

        public override Type[] __GetExportedTypes()
        {
            var arr = new Type[ExportedTypeTable.RowCount];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ResolveExportedType(i);

            return arr;
        }

        private Type ResolveExportedType(int index)
        {
            var typeName = GetTypeName(ExportedTypeTable.records[index].TypeNamespace, ExportedTypeTable.records[index].TypeName);
            var implementation = ExportedTypeTable.records[index].Implementation;
            var token = ExportedTypeTable.records[index].TypeDefId;
            var flags = ExportedTypeTable.records[index].Flags;
            switch (implementation >> 24)
            {
                case AssemblyRefTable.Index:
                    return ResolveAssemblyRef((implementation & 0xFFFFFF) - 1).ResolveType(this, typeName).SetMetadataTokenForMissing(token, flags);
                case ExportedTypeTable.Index:
                    return ResolveExportedType((implementation & 0xFFFFFF) - 1).ResolveNestedType(this, typeName).SetMetadataTokenForMissing(token, flags);
                case FileTable.Index:
                    Module module = assembly.GetModule(GetString(FileTable.records[(implementation & 0xFFFFFF) - 1].Name));
                    return module.FindType(typeName) ?? module.Universe.GetMissingTypeOrThrow(this, module, null, typeName).SetMetadataTokenForMissing(token, flags);
                default:
                    throw new BadImageFormatException();
            }
        }

        internal override Type GetModuleType()
        {
            PopulateTypeDef();
            return moduleType;
        }

        public string __ImageRuntimeVersion => metadata.MetadataVersion;

        public override int MDStreamVersion
        {
            get { return metadataStreamVersion; }
        }

        public void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
        {
            var headers = pe.PEHeaders;
            var flags = headers.CorHeader.Flags;

            peKind = 0;
            if ((flags & CorFlags.ILOnly) != 0)
                peKind |= PortableExecutableKinds.ILOnly;

            // 32BITPREFERRED by itself is illegal, so it is ignored
            switch (flags & (CorFlags.Requires32Bit | CorFlags.Prefers32Bit))
            {
                case CorFlags.Requires32Bit:
                    peKind |= PortableExecutableKinds.Required32Bit;
                    break;
                case CorFlags.Requires32Bit | CorFlags.Prefers32Bit:
                    peKind |= PortableExecutableKinds.Preferred32Bit;
                    break;
            }

            if (headers.PEHeader.Magic == PEMagic.PE32Plus)
                peKind |= PortableExecutableKinds.PE32Plus;

            machine = (ImageFileMachine)headers.CoffHeader.Machine;
        }

        internal override void Dispose()
        {
            pe.Dispose();
            if (metadataImageHandle.IsAllocated)
                metadataImageHandle.Free();

            stream?.Dispose();
        }

        internal override void ExportTypes(AssemblyFileHandle fileToken, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {
            PopulateTypeDef();
            manifestModule.ExportTypes(typeDefs, fileToken);
        }

    }

}
