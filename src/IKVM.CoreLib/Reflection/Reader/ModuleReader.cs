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

using SrmMetadataReader = System.Reflection.Metadata.MetadataReader;
using SrmPEReader = System.Reflection.PortableExecutable.PEReader;

namespace IKVM.Reflection.Reader
{

    sealed class ModuleReader : Module
    {

        sealed class LazyForwardedType
        {

            readonly int _index;
            Type _type;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="index"></param>
            internal LazyForwardedType(int index)
            {
                this._index = index;
            }

            internal Type GetType(ModuleReader module)
            {
                // guard against circular type forwarding
                if (_type == MarkerType.LazyResolveInProgress)
                {
                    var exported = module._metadata.GetExportedType(MetadataTokens.ExportedTypeHandle(_index + 1));
                    return module.Universe.GetMissingTypeOrThrow(module, module, null, module.GetTypeName(exported.Namespace, exported.Name));
                }
                else if (_type == null)
                {
                    _type = MarkerType.LazyResolveInProgress;
                    _type = module.ResolveExportedType(_index);
                }

                return _type;
            }

        }

        readonly Stream _stream;
        readonly string _location;
        Assembly _assembly;
        readonly SrmPEReader _pe;
        readonly SrmMetadataReader _metadata;
        readonly Dictionary<int, string> _userStrings = new Dictionary<int, string>();
        TypeDefImpl[] _typeDefs;
        TypeDefImpl _moduleType;
        Assembly[] _assemblyRefs;
        Type[] _typeRefs;
        Type[] _typeSpecs;
        FieldInfo[] _fields;
        MethodBase[] _methods;
        MemberInfo[] _memberRefs;
        Dictionary<StringHandle, string> _strings = new Dictionary<StringHandle, string>();
        Dictionary<TypeName, Type> _types = new Dictionary<TypeName, Type>();
        Dictionary<TypeName, LazyForwardedType> _forwardedTypes = new Dictionary<TypeName, LazyForwardedType>();

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
            this._stream = stream;
            this._location = location;

            _pe = new SrmPEReader(stream, PEStreamOptions.LeaveOpen | (mapped ? PEStreamOptions.IsLoadedImage : PEStreamOptions.Default));
            if (_pe.HasMetadata == false)
                throw new BadImageFormatException("The image has no metadata.");

            _metadata = _pe.GetMetadataReader(MetadataReaderOptions.None);

            if (assembly == null && _metadata.IsAssembly)
                assembly = new AssemblyReader(location, this);

            this._assembly = assembly;
        }

        internal override void GetTypesImpl(List<Type> list)
        {
            PopulateTypeDef();

            foreach (var type in _typeDefs)
                if (type != _moduleType)
                    list.Add(type);
        }

        void PopulateTypeDef()
        {
            if (_typeDefs == null)
            {
                _typeDefs = new TypeDefImpl[_metadata.TypeDefinitions.Count];
                for (int i = 0; i < _typeDefs.Length; i++)
                {
                    var type = new TypeDefImpl(this, MetadataTokens.TypeDefinitionHandle(i + 1));
                    _typeDefs[i] = type;
                    if (type.IsModulePseudoType)
                        _moduleType = type;
                    else if (!type.IsNestedByFlags)
                        _types.Add(type.TypeName, type);
                }

                // add forwarded types to forwardedTypes dictionary (because Module.GetType(string) should return them)
                foreach (var h in _metadata.ExportedTypes)
                {
                    var exported = _metadata.GetExportedType(h);
                    if (exported.Implementation.Kind == HandleKind.AssemblyReference)
                        _forwardedTypes.Add(GetTypeName(exported.Namespace, exported.Name), new LazyForwardedType(MetadataTokens.GetRowNumber(h) - 1));
                }
            }
        }

        internal override string GetString(StringHandle handle)
        {
            if (handle.IsNil)
                return null;

            if (_strings.TryGetValue(handle, out var str) == false)
                _strings.Add(handle, str = _metadata.GetString(handle));

            return str;
        }

        internal byte[] GetBlobCopy(BlobHandle handle) => _metadata.GetBlobBytes(handle);

        /// <summary>
        /// Gets the metadata reader over the module.
        /// </summary>
        internal SrmMetadataReader Metadata => _metadata;

        /// <summary>
        /// Gets the value of a constant row.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The member has no constant.</exception>
        internal object GetConstantValue(ConstantHandle handle)
        {
            if (handle.IsNil)
                throw new InvalidOperationException();

            var constant = _metadata.GetConstant(handle);
            var value = _metadata.GetBlobReader(constant.Value);
            return constant.TypeCode switch
            {
                ConstantTypeCode.Boolean => value.ReadBoolean(),
                ConstantTypeCode.Char => value.ReadChar(),
                ConstantTypeCode.SByte => value.ReadSByte(),
                ConstantTypeCode.Byte => value.ReadByte(),
                ConstantTypeCode.Int16 => value.ReadInt16(),
                ConstantTypeCode.UInt16 => value.ReadUInt16(),
                ConstantTypeCode.Int32 => value.ReadInt32(),
                ConstantTypeCode.UInt32 => value.ReadUInt32(),
                ConstantTypeCode.Int64 => value.ReadInt64(),
                ConstantTypeCode.UInt64 => value.ReadUInt64(),
                ConstantTypeCode.Single => value.ReadSingle(),
                ConstantTypeCode.Double => value.ReadDouble(),
                ConstantTypeCode.String => value.ReadUTF16(value.Length),
                ConstantTypeCode.NullReference => null,
                _ => throw new BadImageFormatException(),
            };
        }

        internal override BlobReader GetBlobReader(BlobHandle handle) => _metadata.GetBlobReader(handle);

        public override string ResolveString(int metadataToken)
        {
            if ((metadataToken >> 24) != 0x70)
                throw TokenOutOfRangeException(metadataToken);

            if (_userStrings.TryGetValue(metadataToken, out var str) == false)
                _userStrings.Add(metadataToken, str = _metadata.GetUserString(MetadataTokens.UserStringHandle(metadataToken & 0xFFFFFF)));

            return str;
        }

        internal override Type ResolveType(int metadataToken, IGenericContext context)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
                throw TokenOutOfRangeException(metadataToken);

            if ((metadataToken >> 24) == (int)TableIndex.TypeDef && index < _metadata.TypeDefinitions.Count)
            {
                PopulateTypeDef();
                return _typeDefs[index];
            }

            if ((metadataToken >> 24) == (int)TableIndex.TypeRef && index < _metadata.TypeReferences.Count)
            {
                _typeRefs ??= new Type[_metadata.TypeReferences.Count];
                return _typeRefs[index] ??= ResolveTypeRef(_metadata.GetTypeReference(MetadataTokens.TypeReferenceHandle(index + 1)));
            }

            if ((metadataToken >> 24) == (int)TableIndex.TypeSpec && index < _metadata.GetTableRowCount(TableIndex.TypeSpec))
            {
                _typeSpecs ??= new Type[_metadata.GetTableRowCount(TableIndex.TypeSpec)];

                var type = _typeSpecs[index];
                if (type == null)
                {
                    var tc = context == null ? null : new TrackingGenericContext(context);
                    _typeSpecs[index] = MarkerType.LazyResolveInProgress;

                    try
                    {
                        type = Signature.ReadTypeSpec(this, GetBlobReader(_metadata.GetTypeSpecification(MetadataTokens.TypeSpecificationHandle(index + 1)).Signature), tc);
                    }
                    finally
                    {
                        _typeSpecs[index] = null;
                    }

                    if (tc == null || !tc.IsUsed)
                        _typeSpecs[index] = type;
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

        Type ResolveTypeRef(TypeReference reference)
        {
            var typeName = GetTypeName(reference.Namespace, reference.Name);
            var scope = reference.ResolutionScope;

            // a nil scope is a reference to an exported type, which we find through this module
            if (scope.IsNil)
                return FindType(typeName) ?? Universe.GetMissingTypeOrThrow(this, this, null, typeName);

            switch (scope.Kind)
            {
                case HandleKind.AssemblyReference:
                    return ResolveAssemblyRef(MetadataTokens.GetRowNumber(scope) - 1).ResolveType(this, typeName);
                case HandleKind.TypeReference:
                    return ResolveType(MetadataTokens.GetToken(scope), null).ResolveNestedType(this, typeName);
                case HandleKind.ModuleDefinition:
                    return FindType(typeName) ?? Universe.GetMissingTypeOrThrow(this, this, null, typeName);
                case HandleKind.ModuleReference:
                    var module = ResolveModuleRef(_metadata.GetModuleReference((ModuleReferenceHandle)scope).Name);
                    return module.FindType(typeName) ?? module.Universe.GetMissingTypeOrThrow(this, module, null, typeName);
                default:
                    throw new BadImageFormatException("ResolutionScope = " + MetadataTokens.GetToken(scope).ToString("X"));
            }
        }

        Module ResolveModuleRef(StringHandle moduleNameIndex)
        {
            var moduleName = GetString(moduleNameIndex);
            return _assembly.GetModule(moduleName) ?? throw new FileNotFoundException(moduleName);
        }

        sealed class TrackingGenericContext : IGenericContext
        {

            readonly IGenericContext _context;
            bool _used;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            internal TrackingGenericContext(IGenericContext context)
            {
                this._context = context;
            }

            internal bool IsUsed
            {
                get { return _used; }
            }

            public Type GetGenericTypeArgument(int index)
            {
                _used = true;
                return _context.GetGenericTypeArgument(index);
            }

            public Type GetGenericMethodArgument(int index)
            {
                _used = true;
                return _context.GetGenericMethodArgument(index);
            }

        }

        TypeName GetTypeName(StringHandle typeNamespace, StringHandle typeName)
        {
            return new TypeName(GetString(typeNamespace), GetString(typeName));
        }

        internal Assembly ResolveAssemblyRef(int index)
        {
            _assemblyRefs ??= new Assembly[_metadata.AssemblyReferences.Count];
            return _assemblyRefs[index] ??= ResolveAssemblyRefImpl(_metadata.GetAssemblyReference(MetadataTokens.AssemblyReferenceHandle(index + 1)));
        }

        Assembly ResolveAssemblyRefImpl(AssemblyReference reference)
        {
            var flags = (int)reference.Flags;
            var version = reference.Version;
            var name = AssemblyName.GetFullName(
                GetString(reference.Name),
                (ushort)version.Major,
                (ushort)version.Minor,
                (ushort)version.Build,
                (ushort)version.Revision,
                reference.Culture.IsNil ? "neutral" : GetString(reference.Culture),
                reference.PublicKeyOrToken.IsNil ? Array.Empty<byte>() : (flags & PublicKeyFlag) == 0 ? GetBlobCopy(reference.PublicKeyOrToken) : AssemblyName.ComputePublicKeyToken(GetBlobCopy(reference.PublicKeyOrToken)),
                flags);

            return Universe.Load(name, this, true);
        }

        const int PublicKeyFlag = 0x0001;

        public override Guid ModuleVersionId => _metadata.GetGuid(_metadata.GetModuleDefinition().Mvid);

        public override string FullyQualifiedName => _location ?? "<Unknown>";

        public override string Name => _location == null ? "<Unknown>" : Path.GetFileName(_location);

        public override Assembly Assembly => _assembly;

        internal override Type FindType(TypeName typeName)
        {
            PopulateTypeDef();

            if (!_types.TryGetValue(typeName, out var type))
                if (_forwardedTypes.TryGetValue(typeName, out var fw))
                    return fw.GetType(this);

            return type;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            PopulateTypeDef();

            foreach (var type in _types.Values)
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            foreach (var name in _forwardedTypes.Keys)
                if (name.ToLowerInvariant() == lowerCaseName)
                    return _forwardedTypes[name].GetType(this);

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
                case (int)TableIndex.Field:
                    return ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
                case (int)TableIndex.MemberRef:
                    int index = (metadataToken & 0xFFFFFF) - 1;
                    if (index < 0 || index >= _metadata.MemberReferences.Count)
                        goto default;

                    return GetMemberRef(index, genericTypeArguments, genericMethodArguments);
                case (int)TableIndex.MethodDef:
                case (int)TableIndex.MethodSpec:
                    return ResolveMethod(metadataToken, genericTypeArguments, genericMethodArguments);
                case (int)TableIndex.TypeRef:
                case (int)TableIndex.TypeDef:
                case (int)TableIndex.TypeSpec:
                    return ResolveType(metadataToken, genericTypeArguments, genericMethodArguments);
                default:
                    throw TokenOutOfRangeException(metadataToken);
            }
        }

        internal FieldInfo GetFieldAt(TypeDefImpl owner, FieldDefinitionHandle handle)
        {
            var index = MetadataTokens.GetRowNumber(handle) - 1;
            _fields ??= new FieldInfo[_metadata.FieldDefinitions.Count];
            _fields[index] ??= new FieldDefImpl(this, owner ?? GetTypeDef(_metadata.GetFieldDefinition(handle).GetDeclaringType()), handle);
            return _fields[index];
        }

        TypeDefImpl GetTypeDef(TypeDefinitionHandle handle)
        {
            PopulateTypeDef();
            return _typeDefs[MetadataTokens.GetRowNumber(handle) - 1];
        }

        public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
            {
                throw TokenOutOfRangeException(metadataToken);
            }
            else if ((metadataToken >> 24) == (int)TableIndex.Field && index < _metadata.FieldDefinitions.Count)
            {
                return GetFieldAt(null, MetadataTokens.FieldDefinitionHandle(index + 1));
            }
            else if ((metadataToken >> 24) == (int)TableIndex.MemberRef && index < _metadata.MemberReferences.Count)
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

        internal MethodBase GetMethodAt(TypeDefImpl owner, MethodDefinitionHandle handle)
        {
            var index = MetadataTokens.GetRowNumber(handle) - 1;
            _methods ??= new MethodBase[_metadata.MethodDefinitions.Count];
            if (_methods[index] == null)
            {
                var method = new MethodDefImpl(this, owner ?? GetTypeDef(_metadata.GetMethodDefinition(handle).GetDeclaringType()), handle);
                _methods[index] = method.IsConstructor ? new ConstructorInfoImpl(method) : (MethodBase)method;
            }

            return _methods[index];
        }

        public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if (index < 0)
            {
                throw TokenOutOfRangeException(metadataToken);
            }
            else if ((metadataToken >> 24) == (int)TableIndex.MethodDef && index < _metadata.MethodDefinitions.Count)
            {
                return GetMethodAt(null, MetadataTokens.MethodDefinitionHandle(index + 1));
            }
            else if ((metadataToken >> 24) == (int)TableIndex.MemberRef && index < _metadata.MemberReferences.Count)
            {
                var method = GetMemberRef(index, genericTypeArguments, genericMethodArguments) as MethodBase;
                if (method != null)
                    return method;

                throw new ArgumentException(String.Format("Token 0x{0:x8} is not a valid MethodBase token in the scope of module {1}.", metadataToken, this.Name), "metadataToken");
            }
            else if ((metadataToken >> 24) == (int)TableIndex.MethodSpec && index < _metadata.GetTableRowCount(TableIndex.MethodSpec))
            {
                var spec = _metadata.GetMethodSpecification(MetadataTokens.MethodSpecificationHandle(index + 1));
                var method = (MethodInfo)ResolveMethod(MetadataTokens.GetToken(spec.Method), genericTypeArguments, genericMethodArguments);
                var instantiation = GetBlobReader(spec.Signature);
                return method.MakeGenericMethod(Signature.ReadMethodSpec(this, instantiation, new GenericContext(genericTypeArguments, genericMethodArguments)));
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        public override string ScopeName
        {
            get { return GetString(_metadata.GetModuleDefinition().Name); }
        }

        MemberInfo GetMemberRef(int index, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            _memberRefs ??= new MemberInfo[_metadata.MemberReferences.Count];

            if (_memberRefs[index] == null)
            {
                var reference = _metadata.GetMemberReference(MetadataTokens.MemberReferenceHandle(index + 1));
                var owner = MetadataTokens.GetToken(reference.Parent);
                var sig = reference.Signature;
                var name = GetString(reference.Name);
                switch (owner >> 24)
                {
                    case (int)TableIndex.MethodDef:
                        return GetMethodAt(null, MetadataTokens.MethodDefinitionHandle(owner & 0xFFFFFF));
                    case (int)TableIndex.ModuleRef:
                        _memberRefs[index] = ResolveTypeMemberRef(ResolveModuleType(owner), name, GetBlobReader(sig));
                        break;
                    case (int)TableIndex.TypeDef:
                    case (int)TableIndex.TypeRef:
                        _memberRefs[index] = ResolveTypeMemberRef(ResolveType(owner), name, GetBlobReader(sig));
                        break;
                    case (int)TableIndex.TypeSpec:
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

            return _memberRefs[index];
        }

        Type ResolveModuleType(int token)
        {
            var name = GetString(_metadata.GetModuleReference(MetadataTokens.ModuleReferenceHandle(token & 0xFFFFFF)).Name);
            var module = _assembly.GetModule(name);
            if (module == null || module.IsResource())
                throw new BadImageFormatException();

            return module.GetModuleType();
        }

        MemberInfo ResolveTypeMemberRef(Type type, string name, BlobReader sig)
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

        public override byte[] ResolveSignature(int metadataToken)
        {
            int index = (metadataToken & 0xFFFFFF) - 1;
            if ((metadataToken >> 24) == (int)TableIndex.StandAloneSig && index >= 0 && index < _metadata.GetTableRowCount(TableIndex.StandAloneSig))
                return GetBlobCopy(_metadata.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle(index + 1)).Signature);

            throw TokenOutOfRangeException(metadataToken);
        }

        internal MethodInfo GetEntryPoint()
        {
            var cor = _pe.PEHeaders.CorHeader;
            if (cor.EntryPointTokenOrRelativeVirtualAddress != 0 && (cor.Flags & CorFlags.NativeEntryPoint) == 0)
                return (MethodInfo)ResolveMethod(cor.EntryPointTokenOrRelativeVirtualAddress);

            return null;
        }

        ManifestResourceHandle FindManifestResource(string resourceName)
        {
            foreach (var h in _metadata.ManifestResources)
                if (resourceName == GetString(_metadata.GetManifestResource(h).Name))
                    return h;

            return default;
        }

        internal ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            var h = FindManifestResource(resourceName);
            if (h.IsNil)
                return null;

            var info = new ManifestResourceInfo(this, _metadata.GetManifestResource(h).Implementation);
            var asm = info.ReferencedAssembly;
            if (asm != null && !asm.__IsMissing && asm.GetManifestResourceInfo(resourceName) == null)
                return null;

            return info;
        }

        internal Stream GetManifestResourceStream(string resourceName)
        {
            var h = FindManifestResource(resourceName);
            if (h.IsNil)
                return null;

            var resource = _metadata.GetManifestResource(h);
            if (resource.Implementation.IsNil == false)
            {
                var info = new ManifestResourceInfo(this, resource.Implementation);
                switch (resource.Implementation.Kind)
                {
                    case HandleKind.AssemblyFile:
                        var fileName = Path.Combine(Path.GetDirectoryName(_location), info.FileName);
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
                    case HandleKind.AssemblyReference:
                        var asm = info.ReferencedAssembly;
                        if (asm.__IsMissing)
                            return null;

                        return asm.GetManifestResourceStream(resourceName);
                    default:
                        throw new BadImageFormatException();
                }
            }

            // an embedded resource is its length followed by its content
            var data = _pe.GetSectionData(_pe.PEHeaders.CorHeader.ResourcesDirectory.RelativeVirtualAddress + (int)resource.Offset).GetReader();
            var length = data.ReadInt32();
            return new MemoryStream(data.ReadBytes(length));
        }

        public AssemblyName[] __GetReferencedAssemblies()
        {
            var list = new List<AssemblyName>(_metadata.AssemblyReferences.Count);
            foreach (var h in _metadata.AssemblyReferences)
            {
                var reference = _metadata.GetAssemblyReference(h);
                var name = new AssemblyName();
                name.Name = GetString(reference.Name);
                name.Version = reference.Version;

                if (reference.PublicKeyOrToken.IsNil == false)
                {
                    var keyOrToken = GetBlobCopy(reference.PublicKeyOrToken);
                    if (((int)reference.Flags & PublicKeyFlag) != 0)
                        name.SetPublicKey(keyOrToken);
                    else
                        name.SetPublicKeyToken(keyOrToken);
                }
                else
                {
                    name.SetPublicKeyToken(Array.Empty<byte>());
                }

                name.CultureName = reference.Culture.IsNil == false ? GetString(reference.Culture) : "";

                if (reference.HashValue.IsNil == false)
                    name.Hash = GetBlobCopy(reference.HashValue);

                name.RawFlags = (AssemblyNameFlags)(int)reference.Flags;
                list.Add(name);
            }

            return list.ToArray();
        }

        public override Type[] __GetExportedTypes()
        {
            var arr = new Type[_metadata.ExportedTypes.Count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = ResolveExportedType(i);

            return arr;
        }

        Type ResolveExportedType(int index)
        {
            var exported = _metadata.GetExportedType(MetadataTokens.ExportedTypeHandle(index + 1));
            var typeName = GetTypeName(exported.Namespace, exported.Name);
            var implementation = exported.Implementation;
            var token = exported.GetTypeDefinitionId();
            var flags = (int)exported.Attributes;
            switch (implementation.Kind)
            {
                case HandleKind.AssemblyReference:
                    return ResolveAssemblyRef(MetadataTokens.GetRowNumber(implementation) - 1).ResolveType(this, typeName).SetMetadataTokenForMissing(token, flags);
                case HandleKind.ExportedType:
                    return ResolveExportedType(MetadataTokens.GetRowNumber(implementation) - 1).ResolveNestedType(this, typeName).SetMetadataTokenForMissing(token, flags);
                case HandleKind.AssemblyFile:
                    var module = _assembly.GetModule(GetString(_metadata.GetAssemblyFile((AssemblyFileHandle)implementation).Name));
                    return module.FindType(typeName) ?? module.Universe.GetMissingTypeOrThrow(this, module, null, typeName).SetMetadataTokenForMissing(token, flags);
                default:
                    throw new BadImageFormatException();
            }
        }

        /// <summary>
        /// Gets the number of rows in the File table.
        /// </summary>
        internal int FileCount => _metadata.AssemblyFiles.Count;

        /// <summary>
        /// Gets the name of the file at the specified index of the File table.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal string GetFileName(int index) => GetString(_metadata.GetAssemblyFile(MetadataTokens.AssemblyFileHandle(index + 1)).Name);

        /// <summary>
        /// Gets whether the file at the specified index of the File table contains metadata.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal bool FileContainsMetadata(int index) => _metadata.GetAssemblyFile(MetadataTokens.AssemblyFileHandle(index + 1)).ContainsMetadata;

        internal override Type GetModuleType()
        {
            PopulateTypeDef();
            return _moduleType;
        }

        public string __ImageRuntimeVersion => _metadata.MetadataVersion;

        public override int MDStreamVersion => GetTablesStreamVersion();

        /// <summary>
        /// Gets the version of the #~ or #- stream, which System.Reflection.Metadata does not expose, from the metadata root.
        /// </summary>
        /// <returns></returns>
        unsafe int GetTablesStreamVersion()
        {
            var root = new BlobReader(_metadata.MetadataPointer, _metadata.MetadataLength);
            root.Offset = 12;
            root.Offset += 4 + root.ReadInt32() + 2; // version string and flags

            for (int i = root.ReadUInt16(); i > 0; i--)
            {
                var offset = root.ReadInt32();
                root.ReadInt32(); // size
                var name = root.ReadUTF8(root.IndexOf(0));
                root.Offset = (root.Offset + 4) & ~3; // the terminator and the padding to four bytes

                if (name is "#~" or "#-")
                {
                    var stream = new BlobReader(_metadata.MetadataPointer + offset, _metadata.MetadataLength - offset);
                    stream.Offset = 4;
                    return stream.ReadByte() << 16 | stream.ReadByte();
                }
            }

            throw new BadImageFormatException("The metadata has no tables stream.");
        }

        public void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
        {
            var headers = _pe.PEHeaders;
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
            _pe.Dispose();

            _stream?.Dispose();
        }

        internal override void ExportTypes(AssemblyFileHandle fileToken, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {
            PopulateTypeDef();
            manifestModule.ExportTypes(_typeDefs, fileToken);
        }

    }

}
