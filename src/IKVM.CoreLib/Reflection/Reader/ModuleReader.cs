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

    /// <summary>
    /// A module read from a PE image, on top of System.Reflection.Metadata. Types, members and other metadata are
    /// resolved lazily and cached by row.
    /// </summary>
    sealed class ModuleReader : Module
    {

        /// <summary>
        /// A type forwarded to another assembly by an ExportedType row, resolved on first use.
        /// </summary>
        sealed class LazyForwardedType
        {

            readonly int _index;
            Type _type;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="index">The zero-based index of the ExportedType row.</param>
            internal LazyForwardedType(int index)
            {
                _index = index;
            }

            /// <summary>
            /// Resolves the forwarded type, caching the result. If the type is reached again while it is being resolved,
            /// because of circular forwarding, a missing type is returned instead, or an exception is thrown if missing types
            /// are not allowed.
            /// </summary>
            /// <param name="module">The module that contains the ExportedType row.</param>
            /// <returns>The forwarded type.</returns>
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
        readonly Assembly _assembly;
        readonly SrmPEReader _pe;
        readonly SrmMetadataReader _metadata;
        readonly Dictionary<int, string> _userStrings = [];
        TypeDefImpl[] _typeDefs;
        TypeDefImpl _moduleType;
        Assembly[] _assemblyRefs;
        Type[] _typeRefs;
        Type[] _typeSpecs;
        FieldInfo[] _fields;
        MethodBase[] _methods;
        MemberInfo[] _memberRefs;
        readonly Dictionary<StringHandle, string> _strings = [];
        readonly Dictionary<TypeName, Type> _types = [];
        readonly Dictionary<TypeName, LazyForwardedType> _forwardedTypes = [];

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly">
        /// The assembly the module belongs to, or <see langword="null"/> to create an <see cref="AssemblyReader"/> if
        /// the module has a manifest.
        /// </param>
        /// <param name="universe">The universe the module is loaded into.</param>
        /// <param name="stream">The stream containing the image. It is left open by the PE reader and disposed with the module.</param>
        /// <param name="location">The path the module was read from, or <see langword="null"/>.</param>
        /// <param name="mapped"><see langword="true"/> if the stream contains an image mapped by the loader rather than a file.</param>
        /// <exception cref="BadImageFormatException">The image has no metadata.</exception>
        internal ModuleReader(AssemblyReader assembly, Universe universe, Stream stream, string location, bool mapped) :
            base(universe)
        {
            _stream = stream;
            _location = location;

            _pe = new SrmPEReader(stream, PEStreamOptions.LeaveOpen | (mapped ? PEStreamOptions.IsLoadedImage : PEStreamOptions.Default));
            if (_pe.HasMetadata == false)
                throw new BadImageFormatException("The image has no metadata.");

            _metadata = _pe.GetMetadataReader(MetadataReaderOptions.None);

            if (assembly == null && _metadata.IsAssembly)
                assembly = new AssemblyReader(location, this);

            _assembly = assembly;
        }

        /// <summary>
        /// Adds the types defined in the module, except the module pseudo type, to the list.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        internal override void GetTypesImpl(List<Type> list)
        {
            PopulateTypeDef();

            foreach (var type in _typeDefs)
                if (type != _moduleType)
                    list.Add(type);
        }

        /// <summary>
        /// Creates the TypeDef objects and indexes the top-level and forwarded types by name, on first use.
        /// </summary>
        void PopulateTypeDef()
        {
            if (_typeDefs == null)
            {
                _typeDefs = new TypeDefImpl[_metadata.TypeDefinitions.Count];
                for (var i = 0; i < _typeDefs.Length; i++)
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

        /// <summary>
        /// Gets a string from the #Strings heap, caching it.
        /// </summary>
        /// <param name="handle">The handle of the string.</param>
        /// <returns>The string, or <see langword="null"/> if <paramref name="handle"/> is nil.</returns>
        internal override string GetString(StringHandle handle)
        {
            if (handle.IsNil)
                return null;

            if (_strings.TryGetValue(handle, out var str) == false)
                _strings.Add(handle, str = _metadata.GetString(handle));

            return str;
        }

        /// <summary>
        /// Gets a copy of a blob from the #Blob heap.
        /// </summary>
        /// <param name="handle">The handle of the blob.</param>
        /// <returns>A new array with the contents of the blob.</returns>
        internal byte[] GetBlobCopy(BlobHandle handle) => _metadata.GetBlobBytes(handle);

        /// <summary>
        /// Gets the metadata reader over the module.
        /// </summary>
        internal SrmMetadataReader Metadata => _metadata;

        /// <summary>
        /// Gets the value of a constant row.
        /// </summary>
        /// <param name="handle">The Constant row.</param>
        /// <returns>The value of the constant.</returns>
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

        /// <inheritdoc />
        internal override BlobReader GetBlobReader(BlobHandle handle) => _metadata.GetBlobReader(handle);

        /// <summary>
        /// Returns the string identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a string in the string heap of the module.</param>
        /// <returns>A string containing a string value from the metadata string heap.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="metadataToken"/> is not a token for a string.</exception>
        public override string ResolveString(int metadataToken)
        {
            if ((metadataToken >> 24) != 0x70)
                throw TokenOutOfRangeException(metadataToken);

            if (_userStrings.TryGetValue(metadataToken, out var str) == false)
                _userStrings.Add(metadataToken, str = _metadata.GetUserString(MetadataTokens.UserStringHandle(metadataToken & 0xFFFFFF)));

            return str;
        }

        /// <summary>
        /// Resolves a TypeDef, TypeRef or TypeSpec token. A TypeSpec is cached only when resolving it did not depend on the
        /// generic context.
        /// </summary>
        /// <param name="metadataToken">The token to resolve.</param>
        /// <param name="context">The generic context used to resolve a TypeSpec, or <see langword="null"/>.</param>
        /// <returns>The type.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="metadataToken"/> is not a valid type token in this module.</exception>
        /// <exception cref="BadImageFormatException">A TypeSpec refers to itself.</exception>
        internal override Type ResolveType(int metadataToken, IGenericContext context)
        {
            var index = (metadataToken & 0xFFFFFF) - 1;
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
                            .SetMetadataTokenForMissing(metadataToken);
                    }

                    throw new BadImageFormatException("Cyclic TypeSpec " + metadataToken.ToString("X"));
                }

                return type;
            }

            throw TokenOutOfRangeException(metadataToken);
        }

        /// <summary>
        /// Resolves a TypeRef row through its resolution scope.
        /// </summary>
        /// <param name="reference">The TypeRef row.</param>
        /// <returns>The referenced type, or a missing type if it cannot be found and missing types are allowed.</returns>
        /// <exception cref="BadImageFormatException">The resolution scope is not valid.</exception>
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

        /// <summary>
        /// Finds a module of this assembly by the name in a ModuleRef row.
        /// </summary>
        /// <param name="moduleNameIndex">The name of the module.</param>
        /// <returns>The module.</returns>
        /// <exception cref="FileNotFoundException">The assembly has no module with that name.</exception>
        Module ResolveModuleRef(StringHandle moduleNameIndex)
        {
            var moduleName = GetString(moduleNameIndex);
            return _assembly.GetModule(moduleName) ?? throw new FileNotFoundException(moduleName);
        }

        /// <summary>
        /// Wraps a generic context and records whether it was consulted, to decide whether a resolved TypeSpec is
        /// independent of the context and can be cached.
        /// </summary>
        sealed class TrackingGenericContext : IGenericContext
        {

            readonly IGenericContext _context;
            bool _used;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context">The context to wrap.</param>
            internal TrackingGenericContext(IGenericContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Gets whether a type argument has been requested from the context.
            /// </summary>
            internal bool IsUsed => _used;

            /// <inheritdoc />
            public Type GetGenericTypeArgument(int index)
            {
                _used = true;
                return _context.GetGenericTypeArgument(index);
            }

            /// <inheritdoc />
            public Type GetGenericMethodArgument(int index)
            {
                _used = true;
                return _context.GetGenericMethodArgument(index);
            }

        }

        /// <summary>
        /// Creates a type name from #Strings heap handles.
        /// </summary>
        /// <param name="typeNamespace">The namespace of the type.</param>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The type name.</returns>
        TypeName GetTypeName(StringHandle typeNamespace, StringHandle typeName) => new TypeName(GetString(typeNamespace), GetString(typeName));

        /// <summary>
        /// Resolves the assembly referenced by an AssemblyRef row, caching it.
        /// </summary>
        /// <param name="index">The zero-based index of the AssemblyRef row.</param>
        /// <returns>The referenced assembly.</returns>
        internal Assembly ResolveAssemblyRef(int index)
        {
            _assemblyRefs ??= new Assembly[_metadata.AssemblyReferences.Count];
            return _assemblyRefs[index] ??= ResolveAssemblyRefImpl(_metadata.GetAssemblyReference(MetadataTokens.AssemblyReferenceHandle(index + 1)));
        }

        /// <summary>
        /// Loads the assembly referenced by an AssemblyRef row into the universe.
        /// </summary>
        /// <param name="reference">The AssemblyRef row.</param>
        /// <returns>The referenced assembly.</returns>
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

        /// <summary>
        /// The AssemblyRef flag indicating that the row holds a full public key rather than a public key token.
        /// </summary>
        const int PublicKeyFlag = 0x0001;

        /// <inheritdoc />
        public override Guid ModuleVersionId => _metadata.GetGuid(_metadata.GetModuleDefinition().Mvid);

        /// <summary>
        /// Gets the fully qualified name and path of the module, or <c>&lt;Unknown&gt;</c> if it was not read from a file.
        /// </summary>
        public override string FullyQualifiedName => _location ?? "<Unknown>";

        /// <summary>
        /// Gets the file name of the module, or <c>&lt;Unknown&gt;</c> if it was not read from a file.
        /// </summary>
        public override string Name => _location == null ? "<Unknown>" : Path.GetFileName(_location);

        /// <inheritdoc />
        public override Assembly Assembly => _assembly;

        /// <summary>
        /// Finds a top-level type defined in, or forwarded by, the module.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The type, or <see langword="null"/> if it is not found.</returns>
        internal override Type FindType(TypeName typeName)
        {
            PopulateTypeDef();

            if (!_types.TryGetValue(typeName, out var type))
                if (_forwardedTypes.TryGetValue(typeName, out var fw))
                    return fw.GetType(this);

            return type;
        }

        /// <summary>
        /// Finds a top-level type defined in, or forwarded by, the module, ignoring case.
        /// </summary>
        /// <param name="lowerCaseName">The name of the type, in lower case.</param>
        /// <returns>The type, or <see langword="null"/> if it is not found.</returns>
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

        /// <summary>
        /// Creates the exception for a token that is not valid in this module.
        /// </summary>
        /// <param name="metadataToken">The token.</param>
        /// <returns>An <see cref="ArgumentOutOfRangeException"/> for <paramref name="metadataToken"/>.</returns>
        Exception TokenOutOfRangeException(int metadataToken) =>
            new ArgumentOutOfRangeException("metadataToken", string.Format("Token 0x{0:x8} is not valid in the scope of module {1}.", metadataToken, Name));

        /// <summary>
        /// Returns the type or member identified by the specified metadata token, in the context defined by the specified
        /// generic type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/>.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic method arguments of the method where the token is in scope, or <see langword="null"/>.</param>
        /// <returns>A <see cref="MemberInfo"/> object representing the type or member that is identified by the specified metadata token.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="metadataToken"/> is not a valid token in the scope of the module.</exception>
        public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            switch (metadataToken >> 24)
            {
                case (int)TableIndex.Field:
                    return ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
                case (int)TableIndex.MemberRef:
                    var index = (metadataToken & 0xFFFFFF) - 1;
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

        /// <summary>
        /// Gets the field defined by a Field row, creating and caching it on first use.
        /// </summary>
        /// <param name="owner">The type that declares the field, or <see langword="null"/> to look it up.</param>
        /// <param name="handle">The Field row.</param>
        /// <returns>The field.</returns>
        internal FieldInfo GetFieldAt(TypeDefImpl owner, FieldDefinitionHandle handle)
        {
            var index = MetadataTokens.GetRowNumber(handle) - 1;
            _fields ??= new FieldInfo[_metadata.FieldDefinitions.Count];
            _fields[index] ??= new FieldDefImpl(this, owner ?? GetTypeDef(_metadata.GetFieldDefinition(handle).GetDeclaringType()), handle);
            return _fields[index];
        }

        /// <summary>
        /// Gets the type defined by a TypeDef row.
        /// </summary>
        /// <param name="handle">The TypeDef row.</param>
        /// <returns>The type.</returns>
        TypeDefImpl GetTypeDef(TypeDefinitionHandle handle)
        {
            PopulateTypeDef();
            return _typeDefs[MetadataTokens.GetRowNumber(handle) - 1];
        }

        /// <summary>
        /// Returns the field identified by the specified metadata token, in the context defined by the specified generic
        /// type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/>.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic method arguments of the method where the token is in scope, or <see langword="null"/>.</param>
        /// <returns>A <see cref="FieldInfo"/> object representing the field that is identified by the specified metadata token.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="metadataToken"/> is not a valid token in the scope of the module.</exception>
        /// <exception cref="ArgumentException"><paramref name="metadataToken"/> is a MemberRef that does not refer to a field.</exception>
        public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            var index = (metadataToken & 0xFFFFFF) - 1;
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

                throw new ArgumentException(string.Format("Token 0x{0:x8} is not a valid FieldInfo token in the scope of module {1}.", metadataToken, Name), "metadataToken");
            }
            else
            {
                throw TokenOutOfRangeException(metadataToken);
            }
        }

        /// <summary>
        /// Gets the method or constructor defined by a MethodDef row, creating and caching it on first use.
        /// </summary>
        /// <param name="owner">The type that declares the method, or <see langword="null"/> to look it up.</param>
        /// <param name="handle">The MethodDef row.</param>
        /// <returns>The method, or a <see cref="ConstructorInfo"/> if it is a constructor.</returns>
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

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token, in the context defined by the
        /// specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <param name="genericTypeArguments">An array of <see cref="Type"/> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null"/>.</param>
        /// <param name="genericMethodArguments">An array of <see cref="Type"/> objects representing the generic method arguments of the method where the token is in scope, or <see langword="null"/>.</param>
        /// <returns>A <see cref="MethodBase"/> object representing the method that is identified by the specified metadata token.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="metadataToken"/> is not a valid token in the scope of the module.</exception>
        /// <exception cref="ArgumentException"><paramref name="metadataToken"/> is a MemberRef that does not refer to a method.</exception>
        public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            var index = (metadataToken & 0xFFFFFF) - 1;
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

                throw new ArgumentException(string.Format("Token 0x{0:x8} is not a valid MethodBase token in the scope of module {1}.", metadataToken, Name), "metadataToken");
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

        /// <inheritdoc />
        public override string ScopeName => GetString(_metadata.GetModuleDefinition().Name);

        /// <summary>
        /// Resolves the member referenced by a MemberRef row. Members of a TypeSpec or MethodDef parent are not cached, the
        /// former because they depend on the generic context; the others are.
        /// </summary>
        /// <param name="index">The zero-based index of the MemberRef row.</param>
        /// <param name="genericTypeArguments">The generic type arguments in scope, or <see langword="null"/>.</param>
        /// <param name="genericMethodArguments">The generic method arguments in scope, or <see langword="null"/>.</param>
        /// <returns>The referenced member.</returns>
        /// <exception cref="BadImageFormatException">The parent of the MemberRef is not valid.</exception>
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

        /// <summary>
        /// Gets the module pseudo type of the module named by a ModuleRef row, which declares the global members referenced
        /// through it.
        /// </summary>
        /// <param name="token">The ModuleRef token.</param>
        /// <returns>The module pseudo type.</returns>
        /// <exception cref="BadImageFormatException">The module is not found or is a resource module.</exception>
        Type ResolveModuleType(int token)
        {
            var name = GetString(_metadata.GetModuleReference(MetadataTokens.ModuleReferenceHandle(token & 0xFFFFFF)).Name);
            var module = _assembly.GetModule(name);
            if (module == null || module.IsResource())
                throw new BadImageFormatException();

            return module.GetModuleType();
        }

        /// <summary>
        /// Finds the field or method with the given name and signature in a type or its base types. If it is not declared
        /// by the type and missing members are allowed, a missing member on the type is returned instead.
        /// </summary>
        /// <param name="type">The type to search.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="sig">The signature of the member, whose first byte tells a field from a method.</param>
        /// <returns>The member.</returns>
        /// <exception cref="MissingFieldException">The field is not found.</exception>
        /// <exception cref="MissingMethodException">The method is not found.</exception>
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

        /// <summary>
        /// Returns the signature blob identified by a metadata token.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a signature in the module.</param>
        /// <returns>An array of bytes representing the signature blob.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="metadataToken"/> is not a valid StandAloneSig token in the scope of the module.</exception>
        public override byte[] ResolveSignature(int metadataToken)
        {
            var index = (metadataToken & 0xFFFFFF) - 1;
            if ((metadataToken >> 24) == (int)TableIndex.StandAloneSig && index >= 0 && index < _metadata.GetTableRowCount(TableIndex.StandAloneSig))
                return GetBlobCopy(_metadata.GetStandaloneSignature(MetadataTokens.StandaloneSignatureHandle(index + 1)).Signature);

            throw TokenOutOfRangeException(metadataToken);
        }

        /// <summary>
        /// Gets the managed entry point of the module.
        /// </summary>
        /// <returns>The entry point method, or <see langword="null"/> if there is none or it is native.</returns>
        internal MethodInfo GetEntryPoint()
        {
            var cor = _pe.PEHeaders.CorHeader;
            if (cor.EntryPointTokenOrRelativeVirtualAddress != 0 && (cor.Flags & CorFlags.NativeEntryPoint) == 0)
                return (MethodInfo)ResolveMethod(cor.EntryPointTokenOrRelativeVirtualAddress);

            return null;
        }

        /// <summary>
        /// Finds the ManifestResource row with the given name.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns>The row, or a nil handle if there is none.</returns>
        ManifestResourceHandle FindManifestResource(string resourceName)
        {
            foreach (var h in _metadata.ManifestResources)
                if (resourceName == GetString(_metadata.GetManifestResource(h).Name))
                    return h;

            return default;
        }

        /// <summary>
        /// Gets information about where a manifest resource is stored.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns>
        /// The resource information, or <see langword="null"/> if the resource is not found, including when it refers to a
        /// loaded assembly that does not have it.
        /// </returns>
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

        /// <summary>
        /// Opens a manifest resource, which may be embedded, linked from a file, or defined in a referenced assembly.
        /// </summary>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns>A stream over the resource, or <see langword="null"/> if it is not found or is an empty file.</returns>
        /// <exception cref="BadImageFormatException">The implementation of the resource is not valid.</exception>
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

        /// <summary>
        /// Gets the names of the assemblies referenced by the module, read directly from its AssemblyRef rows without
        /// loading them.
        /// </summary>
        /// <returns>The referenced assembly names.</returns>
        public AssemblyName[] __GetReferencedAssemblies()
        {
            var list = new List<AssemblyName>(_metadata.AssemblyReferences.Count);
            foreach (var h in _metadata.AssemblyReferences)
            {
                var reference = _metadata.GetAssemblyReference(h);
                var name = new AssemblyName
                {
                    Name = GetString(reference.Name),
                    Version = reference.Version
                };

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

        /// <summary>
        /// Resolves the types in the ExportedType table: forwarded types and the types of the other modules of the assembly.
        /// </summary>
        /// <returns>The exported types.</returns>
        public override Type[] __GetExportedTypes()
        {
            var arr = new Type[_metadata.ExportedTypes.Count];
            for (var i = 0; i < arr.Length; i++)
                arr[i] = ResolveExportedType(i);

            return arr;
        }

        /// <summary>
        /// Resolves the type described by an ExportedType row through its implementation.
        /// </summary>
        /// <param name="index">The zero-based index of the ExportedType row.</param>
        /// <returns>The type, or a missing type if it cannot be found and missing types are allowed.</returns>
        /// <exception cref="BadImageFormatException">The implementation of the row is not valid.</exception>
        Type ResolveExportedType(int index)
        {
            var exported = _metadata.GetExportedType(MetadataTokens.ExportedTypeHandle(index + 1));
            var typeName = GetTypeName(exported.Namespace, exported.Name);
            var implementation = exported.Implementation;
            var token = exported.GetTypeDefinitionId();
            switch (implementation.Kind)
            {
                case HandleKind.AssemblyReference:
                    return ResolveAssemblyRef(MetadataTokens.GetRowNumber(implementation) - 1).ResolveType(this, typeName).SetMetadataTokenForMissing(token);
                case HandleKind.ExportedType:
                    return ResolveExportedType(MetadataTokens.GetRowNumber(implementation) - 1).ResolveNestedType(this, typeName).SetMetadataTokenForMissing(token);
                case HandleKind.AssemblyFile:
                    var module = _assembly.GetModule(GetString(_metadata.GetAssemblyFile((AssemblyFileHandle)implementation).Name));
                    return module.FindType(typeName) ?? module.Universe.GetMissingTypeOrThrow(this, module, null, typeName).SetMetadataTokenForMissing(token);
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
        /// <param name="index">The zero-based index of the File row.</param>
        /// <returns>The file name.</returns>
        internal string GetFileName(int index) => GetString(_metadata.GetAssemblyFile(MetadataTokens.AssemblyFileHandle(index + 1)).Name);

        /// <summary>
        /// Gets whether the file at the specified index of the File table contains metadata.
        /// </summary>
        /// <param name="index">The zero-based index of the File row.</param>
        /// <returns><see langword="true"/> if the file is a module; <see langword="false"/> if it is a resource file.</returns>
        internal bool FileContainsMetadata(int index) => _metadata.GetAssemblyFile(MetadataTokens.AssemblyFileHandle(index + 1)).ContainsMetadata;

        /// <summary>
        /// Gets the module pseudo type <c>&lt;Module&gt;</c>, which declares the global members.
        /// </summary>
        /// <returns>The module pseudo type.</returns>
        internal override Type GetModuleType()
        {
            PopulateTypeDef();
            return _moduleType;
        }

        /// <summary>
        /// Gets the runtime version string stored in the metadata root.
        /// </summary>
        public string __ImageRuntimeVersion => _metadata.MetadataVersion;

        /// <inheritdoc />
        public override int MDStreamVersion => GetTablesStreamVersion();

        /// <summary>
        /// Gets the version of the #~ or #- stream, which System.Reflection.Metadata does not expose, from the metadata root.
        /// </summary>
        /// <returns>The major version shifted left by 16 bits, combined with the minor version.</returns>
        /// <exception cref="BadImageFormatException">The metadata has no tables stream.</exception>
        unsafe int GetTablesStreamVersion()
        {
            var root = new BlobReader(_metadata.MetadataPointer, _metadata.MetadataLength)
            {
                Offset = 12
            };
            root.Offset += 4 + root.ReadInt32() + 2; // version string and flags

            for (int i = root.ReadUInt16(); i > 0; i--)
            {
                var offset = root.ReadInt32();
                root.ReadInt32(); // size
                var name = root.ReadUTF8(root.IndexOf(0));
                root.Offset = (root.Offset + 4) & ~3; // the terminator and the padding to four bytes

                if (name is "#~" or "#-")
                {
                    var stream = new BlobReader(_metadata.MetadataPointer + offset, _metadata.MetadataLength - offset)
                    {
                        Offset = 4
                    };
                    return stream.ReadByte() << 16 | stream.ReadByte();
                }
            }

            throw new BadImageFormatException("The metadata has no tables stream.");
        }

        /// <summary>
        /// Gets a pair of values indicating the nature of the code in a module and the platform targeted by the module.
        /// </summary>
        /// <param name="peKind">When this method returns, a combination of the <see cref="PortableExecutableKinds"/> values indicating the nature of the code in the module.</param>
        /// <param name="machine">When this method returns, one of the <see cref="ImageFileMachine"/> values indicating the platform targeted by the module.</param>
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

        /// <summary>
        /// Releases the PE reader and the stream the module was read from.
        /// </summary>
        internal override void Dispose()
        {
            _pe.Dispose();

            _stream?.Dispose();
        }

    }

}
