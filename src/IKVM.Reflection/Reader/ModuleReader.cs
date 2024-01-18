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
using System.Text;

using IKVM.Reflection.Metadata;

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
                    return module.universe.GetMissingTypeOrThrow(module, module, null, typeName).SetCyclicTypeForwarder();
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
        readonly PEReader peFile = new PEReader();
        readonly CliHeader cliHeader = new CliHeader();
        string imageRuntimeVersion;
        int metadataStreamVersion;
        byte[] stringHeap;
        byte[] blobHeap;
        byte[] guidHeap;
        uint userStringHeapOffset;
        uint userStringHeapSize;
        byte[] lazyUserStringHeap;
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
            this.stream = universe != null && universe.MetadataOnly ? null : stream;
            this.location = location;
            Read(stream, mapped);

            if (assembly == null && AssemblyTable.records.Length != 0)
                assembly = new AssemblyReader(location, this);

            this.assembly = assembly;
        }

        void Read(Stream stream, bool mapped)
        {
            var br = new BinaryReader(stream);
            peFile.Read(br, mapped);
            stream.Seek(peFile.RvaToFileOffset(peFile.GetComDescriptorVirtualAddress()), SeekOrigin.Begin);
            cliHeader.Read(br);

            stream.Seek(peFile.RvaToFileOffset(cliHeader.MetaData.VirtualAddress), SeekOrigin.Begin);
            foreach (var sh in ReadStreamHeaders(br, out imageRuntimeVersion))
            {
                switch (sh.Name)
                {
                    case "#Strings":
                        stringHeap = ReadHeap(stream, sh.Offset, sh.Size);
                        break;
                    case "#Blob":
                        blobHeap = ReadHeap(stream, sh.Offset, sh.Size);
                        break;
                    case "#US":
                        userStringHeapOffset = sh.Offset;
                        userStringHeapSize = sh.Size;
                        break;
                    case "#GUID":
                        guidHeap = ReadHeap(stream, sh.Offset, sh.Size);
                        break;
                    case "#~":
                    case "#-":
                        stream.Seek(peFile.RvaToFileOffset(cliHeader.MetaData.VirtualAddress + sh.Offset), SeekOrigin.Begin);
                        ReadTables(br);
                        break;
                    default:
                        // we ignore unknown streams, because the CLR does so too
                        // (and some obfuscators add bogus streams)
                        break;
                }
            }
        }

        internal void SetAssembly(Assembly assembly)
        {
            this.assembly = assembly;
        }

        static StreamHeader[] ReadStreamHeaders(BinaryReader br, out string version)
        {
            var signature = br.ReadUInt32();
            if (signature != 0x424A5342)
                throw new BadImageFormatException("Invalid metadata signature");

            /*ushort MajorVersion =*/
            br.ReadUInt16();
            /*ushort MinorVersion =*/
            br.ReadUInt16();
            /*uint Reserved =*/
            br.ReadUInt32();
            var Length = br.ReadUInt32();
            var buf = br.ReadBytes((int)Length);
            version = Encoding.UTF8.GetString(buf).TrimEnd('\u0000');
            /*ushort Flags =*/
            br.ReadUInt16();

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

        byte[] ReadHeap(Stream stream, uint offset, uint size)
        {
            var buf = new byte[size];
            stream.Seek(peFile.RvaToFileOffset(cliHeader.MetaData.VirtualAddress + offset), SeekOrigin.Begin);

            for (var pos = 0; pos < buf.Length;)
            {
                var read = stream.Read(buf, pos, buf.Length - pos);
                if (read == 0)
                    throw new BadImageFormatException();

                pos += read;
            }

            return buf;
        }

        internal void SeekRVA(int rva)
        {
            GetStream().Seek(peFile.RvaToFileOffset((uint)rva), SeekOrigin.Begin);
        }

        internal Stream GetStream()
        {
            return stream ?? throw new InvalidOperationException("Operation not available when UniverseOptions.MetadataOnly is enabled.");
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

            if (!strings.TryGetValue(handle, out var str))
            {
                int len = 0;
                while (stringHeap[MetadataTokens.GetHeapOffset(handle) + len] != 0)
                    len++;

                str = Encoding.UTF8.GetString(stringHeap, MetadataTokens.GetHeapOffset(handle), len);
                strings.Add(handle, str);
            }

            return str;
        }

        static int ReadCompressedUInt(byte[] buffer, ref int offset)
        {
            var b1 = buffer[offset++];
            if (b1 <= 0x7F)
            {
                return b1;
            }
            else if ((b1 & 0xC0) == 0x80)
            {
                var b2 = buffer[offset++];
                return ((b1 & 0x3F) << 8) | b2;
            }
            else
            {
                var b2 = buffer[offset++];
                var b3 = buffer[offset++];
                var b4 = buffer[offset++];
                return ((b1 & 0x3F) << 24) + (b2 << 16) + (b3 << 8) + b4;
            }
        }

        internal byte[] GetBlobCopy(BlobHandle handle)
        {
            var idx = MetadataTokens.GetHeapOffset(handle);
            var len = ReadCompressedUInt(blobHeap, ref idx);
            var buf = new byte[len];
            Buffer.BlockCopy(blobHeap, idx, buf, 0, len);
            return buf;
        }

        internal override ByteReader GetBlobReader(BlobHandle handle)
        {
            return ByteReader.FromBlob(blobHeap, handle);
        }

        public override string ResolveString(int metadataToken)
        {
            if ((metadataToken >> 24) != 0x70)
                throw TokenOutOfRangeException(metadataToken);

            var h = MetadataTokens.StringHandle(metadataToken);

            if (strings.TryGetValue(h, out var str) == false)
            {
                lazyUserStringHeap ??= ReadHeap(GetStream(), userStringHeapOffset, userStringHeapSize);

                var index = metadataToken & 0xFFFFFF;
                var len = ReadCompressedUInt(lazyUserStringHeap, ref index) & ~1;
                var sb = new StringBuilder(len / 2);
                for (int i = 0; i < len; i += 2)
                {
                    var ch = (char)(lazyUserStringHeap[index + i] | lazyUserStringHeap[index + i + 1] << 8);
                    sb.Append(ch);
                }

                str = sb.ToString();
                strings.Add(h, str);
            }

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
                                typeRefs[index] = module.FindType(typeName) ?? module.universe.GetMissingTypeOrThrow(this, module, null, typeName);
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
                        type = Signature.ReadTypeSpec(this, ByteReader.FromBlob(blobHeap, TypeSpecTable.records[index]), tc);
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
                    if (universe.MissingMemberResolution)
                    {
                        return universe
                            .GetMissingTypeOrThrow(this, this, null, new TypeName(null, "Cyclic TypeSpec " + metadataToken.ToString("X")))
                            .SetCyclicTypeSpec()
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

            return universe.Load(name, this, true);
        }

        public override Guid ModuleVersionId => GuidFromSpan(guidHeap.AsSpan(16 * (MetadataTokens.GetHeapOffset(ModuleTable.records[0].Mvid) - 1), 16));

        /// <summary>
        /// Creates a new <see cref="Guid"/> from a span. Optimized for .NET.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        static Guid GuidFromSpan(ReadOnlySpan<byte> b)
        {
#if NETFRAMEWORK
            var _a = (b[3] << 24) | (b[2] << 16) | (b[1] << 8) | b[0];
            var _b = (short)((b[5] << 8) | b[4]);
            var _c = (short)((b[7] << 8) | b[6]);
            var _d = b[8];
            var _e = b[9];
            var _f = b[10];
            var _g = b[11];
            var _h = b[12];
            var _i = b[13];
            var _j = b[14];
            var _k = b[15];
            return new Guid(_a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k);
#else
            return new Guid(b);
#endif
        }

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
                var instantiation = ByteReader.FromBlob(blobHeap, MethodSpecTable.records[index].Instantiation);
                return method.MakeGenericMethod(Signature.ReadMethodSpec(this, instantiation, new GenericContext(genericTypeArguments, genericMethodArguments)));
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        public override Type[] __ResolveOptionalParameterTypes(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments, out CustomModifiers[] customModifiers)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
            {
                throw TokenOutOfRangeException(metadataToken);
            }
            else if ((metadataToken >> 24) == MemberRefTable.Index && index < MemberRefTable.RowCount)
            {
                var sig = MemberRefTable.records[index].Signature;
                return Signature.ReadOptionalParameterTypes(this, GetBlobReader(sig), new GenericContext(genericTypeArguments, genericMethodArguments), out customModifiers);
            }
            else if ((metadataToken >> 24) == MethodDefTable.Index && index < MethodDefTable.RowCount)
            {
                // for convenience, we support passing a MethodDef token as well, because in some places
                // it makes sense to have a vararg method that is referred to by its methoddef (e.g. ldftn).
                // Note that MethodSpec doesn't make sense, because generic methods cannot be vararg.
                customModifiers = Array.Empty<CustomModifiers>();
                return Type.EmptyTypes;
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        public override CustomModifiers __ResolveTypeSpecCustomModifiers(int typeSpecToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            int index = (typeSpecToken & 0xFFFFFF) - 1;
            if (typeSpecToken >> 24 != TypeSpecTable.Index || index < 0 || index >= TypeSpecTable.RowCount)
                throw TokenOutOfRangeException(typeSpecToken);

            return CustomModifiers.Read(this, ByteReader.FromBlob(blobHeap, TypeSpecTable.records[index]), new GenericContext(genericTypeArguments, genericMethodArguments));
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
                        memberRefs[index] = ResolveTypeMemberRef(ResolveModuleType(owner), name, ByteReader.FromBlob(blobHeap, sig));
                        break;
                    case TypeDefTable.Index:
                    case TypeRefTable.Index:
                        memberRefs[index] = ResolveTypeMemberRef(ResolveType(owner), name, ByteReader.FromBlob(blobHeap, sig));
                        break;
                    case TypeSpecTable.Index:
                        {
                            var type = ResolveType(owner, genericTypeArguments, genericMethodArguments);
                            if (type.IsArray)
                            {
                                var methodSig = MethodSignature.ReadSig(this, ByteReader.FromBlob(blobHeap, sig), new GenericContext(genericTypeArguments, genericMethodArguments));
                                return type.FindMethod(name, methodSig) ?? universe.GetMissingMethodOrThrow(this, type, name, methodSig);
                            }
                            else if (type.IsConstructedGenericType)
                            {
                                var member = ResolveTypeMemberRef(type.GetGenericTypeDefinition(), name, ByteReader.FromBlob(blobHeap, sig));
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
                                return ResolveTypeMemberRef(type, name, ByteReader.FromBlob(blobHeap, sig));
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
                if (field == null && universe.MissingMemberResolution)
                    return universe.GetMissingFieldOrThrow(this, type, name, fieldSig);

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
                if (method == null && universe.MissingMemberResolution)
                    return universe.GetMissingMethodOrThrow(this, type, name, methodSig);

                while (method == null && (type = type.BaseType) != null)
                    method = type.FindMethod(name, methodSig);

                if (method != null)
                    return method;

                throw new MissingMethodException(org.ToString(), name);
            }
        }

        internal ByteReader GetStandAloneSig(int index)
        {
            return ByteReader.FromBlob(blobHeap, StandAloneSigTable.records[index]);
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

        public override __StandAloneMethodSig __ResolveStandAloneMethodSig(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if ((metadataToken >> 24) == StandAloneSigTable.Index && index >= 0 && index < StandAloneSigTable.RowCount)
                return MethodSignature.ReadStandAloneMethodSig(this, GetStandAloneSig(index), new GenericContext(genericTypeArguments, genericMethodArguments));
            else
                throw TokenOutOfRangeException(metadataToken);
        }

        internal MethodInfo GetEntryPoint()
        {
            if (cliHeader.EntryPointToken != 0 && (cliHeader.Flags & CliHeader.COMIMAGE_FLAGS_NATIVE_ENTRYPOINT) == 0)
                return (MethodInfo)ResolveMethod((int)cliHeader.EntryPointToken);

            return null;
        }

        internal string[] GetManifestResourceNames()
        {
            var names = new string[ManifestResourceTable.records.Length];
            for (int i = 0; i < ManifestResourceTable.records.Length; i++)
                names[i] = GetString(ManifestResourceTable.records[i].Name);

            return names;
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
                    SeekRVA((int)cliHeader.Resources.VirtualAddress + ManifestResourceTable.records[i].Offset);
                    var br = new BinaryReader(stream);
                    var length = br.ReadInt32();
                    return new MemoryStream(br.ReadBytes(length));
                }
            }

            return null;
        }

        public override AssemblyName[] __GetReferencedAssemblies()
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

        public override void __ResolveReferencedAssemblies(Assembly[] assemblies)
        {
            assemblyRefs ??= new Assembly[AssemblyRefTable.RowCount];

            for (int i = 0; i < assemblies.Length; i++)
                if (assemblyRefs[i] == null)
                    assemblyRefs[i] = assemblies[i];
        }

        public override string[] __GetReferencedModules()
        {
            var arr = new string[this.ModuleRefTable.RowCount];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = GetString(ModuleRefTable.records[i]);

            return arr;
        }

        public override Type[] __GetReferencedTypes()
        {
            var arr = new Type[TypeRefTable.RowCount];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ResolveType((TypeRefTable.Index << 24) + i + 1);

            return arr;
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
                    return module.FindType(typeName) ?? module.universe.GetMissingTypeOrThrow(this, module, null, typeName).SetMetadataTokenForMissing(token, flags);
                default:
                    throw new BadImageFormatException();
            }
        }

        internal override Type GetModuleType()
        {
            PopulateTypeDef();
            return moduleType;
        }

        public override string __ImageRuntimeVersion
        {
            get { return imageRuntimeVersion; }
        }

        public override int MDStreamVersion
        {
            get { return metadataStreamVersion; }
        }

        public override void __GetDataDirectoryEntry(int index, out int rva, out int length)
        {
            peFile.GetDataDirectoryEntry(index, out rva, out length);
        }

        public override long __RelativeVirtualAddressToFileOffset(int rva)
        {
            return peFile.RvaToFileOffset((uint)rva);
        }

        public override bool __GetSectionInfo(int rva, out string name, out int characteristics, out int virtualAddress, out int virtualSize, out int pointerToRawData, out int sizeOfRawData)
        {
            return peFile.GetSectionInfo(rva, out name, out characteristics, out virtualAddress, out virtualSize, out pointerToRawData, out sizeOfRawData);
        }

        public override int __ReadDataFromRVA(int rva, byte[] data, int offset, int length)
        {
            SeekRVA(rva);

            var totalBytesRead = 0;
            while (length > 0)
            {
                var read = stream.Read(data, offset, length);
                if (read == 0)
                    break; // C++ assemblies can have fields that have an RVA that lies outside of the file

                offset += read;
                length -= read;
                totalBytesRead += read;
            }

            return totalBytesRead;
        }

        public override void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
        {
            peKind = 0;
            if ((cliHeader.Flags & CliHeader.COMIMAGE_FLAGS_ILONLY) != 0)
                peKind |= PortableExecutableKinds.ILOnly;

            switch (cliHeader.Flags & (CliHeader.COMIMAGE_FLAGS_32BITREQUIRED | CliHeader.COMIMAGE_FLAGS_32BITPREFERRED))
            {
                case CliHeader.COMIMAGE_FLAGS_32BITREQUIRED:
                    peKind |= PortableExecutableKinds.Required32Bit;
                    break;
                case CliHeader.COMIMAGE_FLAGS_32BITREQUIRED | CliHeader.COMIMAGE_FLAGS_32BITPREFERRED:
                    peKind |= PortableExecutableKinds.Preferred32Bit;
                    break;
                default:
                    // COMIMAGE_FLAGS_32BITPREFERRED by itself is illegal, so we ignore it
                    // (not setting any flag is ok)
                    break;
            }

            if (peFile.OptionalHeader.Magic == IMAGE_OPTIONAL_HEADER.IMAGE_NT_OPTIONAL_HDR64_MAGIC)
                peKind |= PortableExecutableKinds.PE32Plus;

            machine = (ImageFileMachine)peFile.FileHeader.Machine;
        }

        public override int __Subsystem
        {
            get { return peFile.OptionalHeader.Subsystem; }
        }

        public override IList<CustomAttributeData> __GetPlaceholderAssemblyCustomAttributes(bool multiple, bool security)
        {
            TypeName typeName;
            switch ((multiple ? 1 : 0) + (security ? 2 : 0))
            {
                case 0:
                    typeName = new TypeName("System.Runtime.CompilerServices", "AssemblyAttributesGoHere");
                    break;
                case 1:
                    typeName = new TypeName("System.Runtime.CompilerServices", "AssemblyAttributesGoHereM");
                    break;
                case 2:
                    typeName = new TypeName("System.Runtime.CompilerServices", "AssemblyAttributesGoHereS");
                    break;
                case 3:
                default:
                    typeName = new TypeName("System.Runtime.CompilerServices", "AssemblyAttributesGoHereSM");
                    break;
            }

            var list = new List<CustomAttributeData>();
            for (int i = 0; i < CustomAttributeTable.records.Length; i++)
            {
                if ((CustomAttributeTable.records[i].Parent >> 24) == TypeRefTable.Index)
                {
                    var index = (CustomAttributeTable.records[i].Parent & 0xFFFFFF) - 1;
                    if (typeName == GetTypeName(TypeRefTable.records[index].TypeNamespace, TypeRefTable.records[index].TypeName))
                        list.Add(new CustomAttributeData(this, i));
                }
            }

            return list;
        }

        internal override void Dispose()
        {
            stream?.Dispose();
        }

        internal override void ExportTypes(AssemblyFileHandle fileToken, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {
            PopulateTypeDef();
            manifestModule.ExportTypes(typeDefs, fileToken);
        }

        protected override ulong GetImageBaseImpl()
        {
            return peFile.OptionalHeader.ImageBase;
        }

        protected override ulong GetStackReserveImpl()
        {
            return peFile.OptionalHeader.SizeOfStackReserve;
        }

        protected override uint GetFileAlignmentImpl()
        {
            return peFile.OptionalHeader.FileAlignment;
        }

        protected override DllCharacteristics GetDllCharacteristicsImpl()
        {
            return (DllCharacteristics)peFile.OptionalHeader.DllCharacteristics;
        }

        public override int __EntryPointRVA
        {
            get { return (cliHeader.Flags & CliHeader.COMIMAGE_FLAGS_NATIVE_ENTRYPOINT) != 0 ? (int)cliHeader.EntryPointToken : 0; }
        }

        public override int __EntryPointToken
        {
            get { return (cliHeader.Flags & CliHeader.COMIMAGE_FLAGS_NATIVE_ENTRYPOINT) == 0 ? (int)cliHeader.EntryPointToken : 0; }
        }

        public override System.Security.Cryptography.X509Certificates.X509Certificate GetSignerCertificate()
        {
            return Authenticode.GetSignerCertificate(GetStream());
        }

    }

}
