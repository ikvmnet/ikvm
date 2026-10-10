/*
  Copyright (C) 2008-2015 Jeroen Frijters

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
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Runtime.InteropServices;

using IKVM.Reflection.Impl;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines and represents a module in a dynamic assembly. Accumulates the metadata, IL, resources and pseudo
    /// tokens of everything defined in the module until it is written to a PE image.
    /// </summary>
    sealed class ModuleBuilder : Module, ITypeOwner
    {

        /// <summary>
        /// A managed resource embedded in the module, written to the resource stream when the module is saved.
        /// </summary>
        struct ResourceWriterRecord
        {

            readonly string _name;
            readonly ResourceWriter _rw;
            readonly Stream _stream;
            readonly ResourceAttributes _attributes;

            /// <summary>
            /// Initializes a new instance for a resource whose content is supplied as a stream.
            /// </summary>
            /// <param name="name">The name of the resource.</param>
            /// <param name="stream">The stream that holds the content of the resource.</param>
            /// <param name="attributes">The attributes of the resource.</param>
            internal ResourceWriterRecord(string name, Stream stream, ResourceAttributes attributes) :
                this(name, null, stream, attributes)
            {
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name">The name of the resource.</param>
            /// <param name="rw">The resource writer that generates the content into <paramref name="stream"/>, or <see langword="null"/> if the stream already holds the content.</param>
            /// <param name="stream">The stream that holds the content of the resource.</param>
            /// <param name="attributes">The attributes of the resource.</param>
            internal ResourceWriterRecord(string name, ResourceWriter rw, Stream stream, ResourceAttributes attributes)
            {
                _name = name;
                _rw = rw;
                _stream = stream;
                _attributes = attributes;
            }

            /// <summary>
            /// Writes the resource to the resource stream and adds its ManifestResource row.
            /// </summary>
            /// <param name="module">The module whose resource stream receives the resource.</param>
            internal readonly void Write(ModuleBuilder module)
            {
                // write resource to internal stream
                _rw?.Generate();

                // align the start of the resource
                module._resourceStream.Align(8);
                var offset = module._resourceStream.Count;

                // resource begins with a length, followed by the data
                module._resourceStream.WriteInt32((int)_stream.Length);
                _stream.Position = 0;
                var buffer = new byte[8192];
                int length;
                while ((length = _stream.Read(buffer, 0, buffer.Length)) != 0)
                    module._resourceStream.WriteBytes(buffer, 0, length);

                module.AddManifestResource(_attributes, _name, 0, offset);
            }

            /// <summary>
            /// Closes the resource writer, if any.
            /// </summary>
            internal readonly void Close() => _rw?.Close();

        }

        /// <summary>
        /// A custom attribute set on this module. The parent and constructor can be pseudo tokens until the module is
        /// written.
        /// </summary>
        struct CustomAttributeRow
        {

            internal int Parent;
            internal int Constructor;
            internal BlobHandle Value;
            internal CustomAttributeBuilder Builder;

        }

        /// <summary>
        /// A reference to a type. The resolution scope can be a pseudo token for an assembly reference until the module is
        /// written.
        /// </summary>
        struct TypeRefRow
        {

            internal int ResolutionScope;
            internal StringHandle Namespace;
            internal StringHandle Name;

        }

        /// <summary>
        /// A reference to a field or method. The parent can be a pseudo token until the module is written.
        /// </summary>
        struct MemberRefRow
        {

            internal int Class;
            internal StringHandle Name;
            internal BlobHandle Signature;

        }

        /// <summary>
        /// An instantiation of a generic method. The method can be a pseudo token until the module is written.
        /// </summary>
        struct MethodSpecRow
        {

            internal int Method;
            internal BlobHandle Instantiation;

        }

        /// <summary>
        /// A type exported from or forwarded by the assembly. The implementation can be a pseudo token for an assembly
        /// reference until the module is written.
        /// </summary>
        struct ExportedTypeRow
        {

            internal TypeAttributes Flags;
            internal int TypeDefId;
            internal StringHandle Name;
            internal StringHandle Namespace;
            internal int Implementation;

        }

        /// <summary>
        /// A manifest resource, embedded at an offset or stored in a file.
        /// </summary>
        struct ManifestResourceRow
        {

            internal ResourceAttributes Flags;
            internal StringHandle Name;
            internal int Implementation;
            internal int Offset;

        }

        /// <summary>
        /// Associates an accessor method with a property. The method can be a pseudo token until the module is written.
        /// </summary>
        struct MethodSemanticsRow
        {

            internal int Association;
            internal short Semantics;
            internal int Method;

        }

        /// <summary>
        /// A constant of a field, parameter or property. The parent can be a pseudo token until the module is written.
        /// </summary>
        struct ConstantRow
        {

            internal int Parent;
            internal object Value;

        }

        /// <summary>
        /// A marshalling descriptor of a field or parameter. The parent can be a pseudo token until the module is written.
        /// </summary>
        struct FieldMarshalRow
        {

            internal int Parent;
            internal byte[] NativeType;

        }

        /// <summary>
        /// The P/Invoke import of a method. The method can be a pseudo token until the module is written.
        /// </summary>
        struct ImplMapRow
        {

            internal int Method;
            internal ImplMapFlags Flags;
            internal string ImportName;
            internal string ImportScope;
            internal ModuleReferenceHandle ImportScopeHandle;

        }

        /// <summary>
        /// Identifies an imported field or method by declaring type, name and signature, so that each member is only
        /// given one MemberRef.
        /// </summary>
        readonly struct MemberRefKey : IEquatable<MemberRefKey>
        {

            readonly Type _type;
            readonly string _name;
            readonly Signature _signature;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type">The type that declares the member.</param>
            /// <param name="name">The name of the member.</param>
            /// <param name="signature">The signature of the member.</param>
            internal MemberRefKey(Type type, string name, Signature signature)
            {
                _type = type;
                _name = name;
                _signature = signature;
            }

            /// <summary>
            /// Returns whether this key identifies the same member as another key.
            /// </summary>
            /// <param name="other">The key to compare with.</param>
            /// <returns><see langword="true"/> if the declaring type, name and signature are equal; otherwise <see langword="false"/>.</returns>
            public bool Equals(MemberRefKey other) => other._type.Equals(_type) && other._name == _name && other._signature.Equals(_signature);

            /// <inheritdoc />
            public override bool Equals(object obj) => obj is MemberRefKey other && Equals(other);

            /// <inheritdoc />
            public override int GetHashCode() => _type.GetHashCode() + _name.GetHashCode() + _signature.GetHashCode();

            /// <summary>
            /// Finds the method this key identifies on its declaring type.
            /// </summary>
            /// <returns>The method, or <see langword="null"/> if it is not found.</returns>
            internal MethodBase LookupMethod() => _type.FindMethod(_name, (MethodSignature)_signature);

        }

        /// <summary>
        /// Identifies an imported generic method instantiation by declaring type, name, signature and type arguments,
        /// so that each instantiation is only given one MethodSpec.
        /// </summary>
        readonly struct MethodSpecKey : IEquatable<MethodSpecKey>
        {

            readonly Type _type;
            readonly string _name;
            readonly MethodSignature _signature;
            readonly Type[] _genericParameters;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type">The type that declares the method.</param>
            /// <param name="name">The name of the method.</param>
            /// <param name="signature">The signature of the method.</param>
            /// <param name="genericParameters">The type arguments of the instantiation.</param>
            internal MethodSpecKey(Type type, string name, MethodSignature signature, Type[] genericParameters)
            {
                _type = type;
                _name = name;
                _signature = signature;
                _genericParameters = genericParameters;
            }

            /// <summary>
            /// Returns whether this key identifies the same method instantiation as another key.
            /// </summary>
            /// <param name="other">The key to compare with.</param>
            /// <returns><see langword="true"/> if the declaring type, name, signature and type arguments are equal; otherwise <see langword="false"/>.</returns>
            public bool Equals(MethodSpecKey other) =>
                other._type.Equals(_type) && other._name == _name && other._signature.Equals(_signature) && Util.ArrayEquals(other._genericParameters, _genericParameters);

            /// <inheritdoc />
            public override bool Equals(object obj) => obj is MethodSpecKey other && Equals(other);

            /// <inheritdoc />
            public override int GetHashCode() =>
                _type.GetHashCode() + _name.GetHashCode() + _signature.GetHashCode() + Util.GetHashCode(_genericParameters);

        }

        readonly MetadataBuilder _metadata;
        readonly BlobBuilder _ilStream;
        readonly MethodBodyStreamEncoder _methodBodyEncoder;
        readonly BlobBuilder _resourceStream;
        readonly AssemblyBuilder _asm;
        Guid _mvid;
        readonly ReservedBlob<GuidHandle> _mvidFixup;
        ulong _imageBaseAddress = 0;
        uint _fileAlignment = 0;
        DllCharacteristics _dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NoSEH | DllCharacteristics.NXCompat | DllCharacteristics.TerminalServerAware;
        readonly string _moduleName;

        /// <summary>
        /// The name of the file the module is written to.
        /// </summary>
        internal readonly string FileName;

        readonly TypeBuilder _moduleType;
        readonly List<TypeBuilder> _types = [];
        readonly Dictionary<Type, int> _typeTokens = [];

        /// <summary>
        /// The TypeSpec tokens of generic type definitions used as the parent of a MemberRef.
        /// </summary>
        readonly Dictionary<Type, int> _memberRefTypeTokens = [];

        readonly Dictionary<MemberRefKey, int> _importedMemberRefs = [];
        readonly Dictionary<MethodSpecKey, int> _importedMethodSpecs = [];
        readonly Dictionary<Assembly, int> _referencedAssemblies = [];
        int _nextPseudoToken = -1;
        int _typeCount;
        int _propertyCount;
        int _genericParameterCount;

        /// <summary>
        /// The real tokens of the pseudo tokens, indexed by -(pseudoToken + 1).
        /// </summary>
        readonly List<int> _resolvedTokens = [];

        ISymbolWriter _symbolWriter;

        readonly List<ResourceWriterRecord> _resourceWriters = [];
        readonly List<CustomAttributeRow> _customAttributes = [];
        readonly List<TypeRefRow> _typeRefs = [];
        readonly List<MemberRefRow> _memberRefs = [];
        readonly Dictionary<(int Class, StringHandle Name, BlobHandle Signature), int> _memberRefTokens = [];
        readonly List<MethodSpecRow> _methodSpecs = [];
        readonly Dictionary<(int Method, BlobHandle Instantiation), int> _methodSpecTokens = [];
        readonly Dictionary<BlobHandle, int> _standAloneSignatureTokens = [];
        readonly Dictionary<StringHandle, ModuleReferenceHandle> _moduleRefs = [];
        readonly Dictionary<(StringHandle Name, Version Version, StringHandle Culture, BlobHandle PublicKeyOrToken, int Flags), AssemblyReferenceHandle> _assemblyRefs = [];
        readonly List<ExportedTypeRow> _exportedTypes = [];
        readonly Dictionary<(int Implementation, StringHandle Name, StringHandle Namespace), int> _exportedTypeTokens = [];
        readonly List<ManifestResourceRow> _manifestResources = [];

        /// <summary>
        /// Set once the reference tables have been written, after which no more references can be added.
        /// </summary>
        bool _referencesWritten;

        readonly List<MethodSemanticsRow> _methodSemantics = [];
        readonly List<ConstantRow> _constants = [];
        readonly List<FieldMarshalRow> _fieldMarshals = [];
        readonly List<ImplMapRow> _implMaps = [];
        bool _saved;

        /// <summary>
        /// Initializes a new instance. Adds the Module row and the &lt;Module&gt; type, which must be the first TypeDef.
        /// </summary>
        /// <param name="asm">The assembly that contains the module.</param>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="fileName">The name of the file the module is written to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="asm"/> is <see langword="null"/>.</exception>
        internal ModuleBuilder(AssemblyBuilder asm, string moduleName, string fileName) :
            base(asm.Universe)
        {
            _metadata = new MetadataBuilder();
            _ilStream = new BlobBuilder();
            _methodBodyEncoder = new MethodBodyStreamEncoder(_ilStream);
            _resourceStream = new BlobBuilder();

            _asm = asm ?? throw new ArgumentNullException(nameof(asm));
            _metadata = _metadata ?? throw new ArgumentNullException(nameof(_metadata));
            _moduleName = moduleName;
            FileName = fileName;

            // the image gets its real module version id when it is written; until then report a provisional one
            if (Universe.Deterministic == false)
                _mvid = Guid.NewGuid();

            // add module
            _mvidFixup = _metadata.ReserveGuid();
            _metadata.AddModule(0, GetOrAddString(moduleName), _mvidFixup.Handle, default, default);

            // <Module> must be the first record in the TypeDef table
            _moduleType = new TypeBuilder(this, null, "<Module>");
            _types.Add(_moduleType);
        }

        /// <summary>
        /// Gets a reference to the <see cref="MetadataBuilder"/> underlying this instance.
        /// </summary>
        internal MetadataBuilder Metadata => _metadata;

        /// <summary>
        /// Gets a reference to the <see cref="BlobBuilder"/> for the IL stream.
        /// </summary>
        internal BlobBuilder ILStream => _ilStream;

        /// <summary>
        /// Gets a reference to the <see cref="MethodBodyStreamEncoder"/> for the IL stream.
        /// </summary>
        internal MethodBodyStreamEncoder MethodBodyEncoder => _methodBodyEncoder;

        /// <summary>
        /// Gets a reference to the <see cref="BlobBuilder"/> for the resource stream.
        /// </summary>
        internal BlobBuilder ResourceStream => _resourceStream;

        /// <summary>
        /// Adds a string to the string heap, or gets the handle of the existing entry.
        /// </summary>
        /// <param name="value">The string to add.</param>
        /// <returns>The handle of the string in the string heap.</returns>
        internal StringHandle GetOrAddString(string value) => _metadata.GetOrAddString(value);

        /// <summary>
        /// Adds a blob to the blob heap, or gets the handle of the existing entry.
        /// </summary>
        /// <param name="value">The builder holding the blob content.</param>
        /// <returns>The handle of the blob in the blob heap.</returns>
        internal BlobHandle GetOrAddBlob(BlobBuilder value) => _metadata.GetOrAddBlob(value);

        /// <summary>
        /// Adds a blob to the blob heap, or gets the handle of the existing entry.
        /// </summary>
        /// <param name="value">The blob content.</param>
        /// <returns>The handle of the blob in the blob heap.</returns>
        internal BlobHandle GetOrAddBlob(byte[] value) => _metadata.GetOrAddBlob(value);

        /// <summary>
        /// Sets the active symbol writer.
        /// </summary>
        /// <param name="writer">The symbol writer that receives debugging information for the module.</param>
        internal void SetSymWriter(ISymbolWriter writer) => _symbolWriter = writer;

        /// <summary>
        /// Writes the PropertyMap row and the properties of each type, in type order.
        /// </summary>
        internal void PopulatePropertyTables()
        {
            // LAMESPEC the PropertyMap and EventMap tables are not required to be sorted by the CLI spec,
            // but .NET sorts them and Mono requires them to be sorted, so we have to populate the
            // tables in the right order
            foreach (var type in _types)
                type.PopulatePropertyTable();
        }

        /// <summary>
        /// Writes the TypeDef rows of all types, in the order they were defined, along with the start of each type's
        /// field and method lists.
        /// </summary>
        void WriteTypeDefTable()
        {
            var fieldList = 1;
            var methodList = 1;
            foreach (var type in _types)
                type.WriteTypeDefRecord(ref fieldList, ref methodList);
        }

        /// <summary>
        /// Writes the MethodDef rows of all types, along with the start of each method's parameter list.
        /// </summary>
        void WriteMethodDefTable()
        {
            var paramList = 1;
            foreach (var type in _types)
                type.WriteMethodDefRecords(ref paramList);
        }

        /// <summary>
        /// Writes the Param rows of the methods of all types.
        /// </summary>
        void WriteParamTable()
        {
            foreach (var type in _types)
                type.WriteParamRecords();
        }

        /// <summary>
        /// Writes the Field rows of all types.
        /// </summary>
        void WriteFieldTable()
        {
            foreach (var type in _types)
                type.WriteFieldRecords();
        }

        /// <summary>
        /// Allocates a new pseudo token. Pseudo tokens are negative and stand for a definition until its real token is
        /// known, at which point they are resolved through <see cref="RegisterTokenFixup"/>.
        /// </summary>
        /// <returns>The new pseudo token.</returns>
        internal int AllocPseudoToken() => _nextPseudoToken--;

        /// <summary>
        /// Allocates the TypeDef token of a new type. Types are written in the order they are defined.
        /// </summary>
        /// <returns>The TypeDef token.</returns>
        internal int AllocTypeToken() => MetadataTokens.GetToken(MetadataTokens.TypeDefinitionHandle(++_typeCount));

        /// <summary>
        /// Gets the number of properties written so far.
        /// </summary>
        internal int PropertyCount => _propertyCount;

        /// <summary>
        /// Writes a property definition, returning its token. Properties are written type by type while the module is
        /// being saved, which keeps each type's properties contiguous.
        /// </summary>
        /// <param name="attributes">The attributes of the property.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="signature">The signature of the property.</param>
        /// <returns>The Property token.</returns>
        internal int AddProperty(PropertyAttributes attributes, string name, PropertySignature signature)
        {
            var h = _metadata.AddProperty((System.Reflection.PropertyAttributes)attributes, GetOrAddString(name), GetSignatureBlobIndex(signature));
            Debug.Assert(MetadataTokens.GetRowNumber(h) == _propertyCount + 1);
            _propertyCount++;
            return MetadataTokens.GetToken(h);
        }

        /// <summary>
        /// Records an accessor of a property.
        /// </summary>
        /// <param name="semantics">The MethodSemanticsAttributes value that says which accessor the method is.</param>
        /// <param name="methodToken">The token or pseudo token of the accessor method.</param>
        /// <param name="association">The token of the property the accessor belongs to.</param>
        internal void AddMethodSemantics(short semantics, int methodToken, int association) =>
            _methodSemantics.Add(new MethodSemanticsRow() { Semantics = semantics, Method = methodToken, Association = association });

        /// <summary>
        /// Allocates the provisional index a generic parameter reports as its token until the module is written.
        /// </summary>
        /// <returns>The provisional index.</returns>
        internal int AllocGenericParameterIndex() => ++_genericParameterCount;

        /// <summary>
        /// Writes the tables that describe the structure of the types: layouts, interface implementations, method
        /// overrides, nesting, field layouts, generic parameters and their constraints, all of which the metadata
        /// requires to be added in sorted order.
        /// </summary>
        void WriteTypeStructure()
        {
            foreach (var type in _types)
                type.WriteStructure();

            // generic parameters are sorted by their owner, a TypeOrMethodDef coded index, and then by number
            var parameters = new List<GenericTypeParameterBuilder>();
            foreach (var type in _types)
                type.CollectGenericParameters(parameters);

            var keys = new Dictionary<GenericTypeParameterBuilder, int>(parameters.Count);
            foreach (var p in parameters)
                keys[p] = CodedIndex.TypeOrMethodDef(MetadataTokens.EntityHandle(ResolvePseudoToken(p.OwnerToken)));

            var sorted = parameters.OrderBy(p => keys[p]).ThenBy(p => p.Position).ToList();
            for (var i = 0; i < sorted.Count; i++)
            {
                var p = sorted[i];
                p.SetRow(i + 1);
                _metadata.AddGenericParameter(MetadataTokens.EntityHandle(ResolvePseudoToken(p.OwnerToken)), (System.Reflection.GenericParameterAttributes)p.GenericParameterAttributesValue, GetOrAddString(p.Name), p.Position);
            }

            for (var i = 0; i < sorted.Count; i++)
                foreach (var constraint in sorted[i].Constraints)
                    _metadata.AddGenericParameterConstraint(MetadataTokens.GenericParameterHandle(i + 1), MetadataTokens.EntityHandle(constraint));
        }

        /// <summary>
        /// Constructs a <see cref="TypeBuilder"/> for a private type with the specified name in this module.
        /// </summary>
        /// <param name="name">The full path of the type, including the namespace.</param>
        /// <returns>A private type with the specified name.</returns>
        public TypeBuilder DefineType(string name) => DefineType(name, TypeAttributes.Class);

        /// <summary>
        /// Constructs a <see cref="TypeBuilder"/> given the type name and the type attributes.
        /// </summary>
        /// <param name="name">The full path of the type.</param>
        /// <param name="attr">The attributes of the defined type.</param>
        /// <returns>A type created with all requested attributes.</returns>
        public TypeBuilder DefineType(string name, TypeAttributes attr) => DefineType(name, attr, null);

        /// <summary>
        /// Constructs a <see cref="TypeBuilder"/> given type name, its attributes, and the type that the defined type
        /// extends.
        /// </summary>
        /// <param name="name">The full path of the type.</param>
        /// <param name="attr">The attributes of the defined type.</param>
        /// <param name="parent">The type that the defined type extends.</param>
        /// <returns>A type created with all requested attributes.</returns>
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent) => DefineType(name, attr, parent, PackingSize.Unspecified, 0);

        /// <summary>
        /// Constructs a <see cref="TypeBuilder"/> given the type name, attributes, the type that the defined type
        /// extends, and the interfaces that the defined type implements.
        /// </summary>
        /// <param name="name">The full path of the type.</param>
        /// <param name="attr">The attributes to be associated with the type.</param>
        /// <param name="parent">The type that the defined type extends.</param>
        /// <param name="interfaces">The list of interfaces that the type implements.</param>
        /// <returns>A type created with all requested attributes.</returns>
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            var tb = DefineType(name, attr, parent);
            foreach (Type iface in interfaces)
                tb.AddInterfaceImplementation(iface);

            return tb;
        }

        /// <summary>
        /// Constructs a <see cref="TypeBuilder"/> given the type name, the attributes, the type that the defined type
        /// extends, the packing size of the defined type, and the total size of the defined type.
        /// </summary>
        /// <param name="name">The full path of the type.</param>
        /// <param name="attr">The attributes of the defined type.</param>
        /// <param name="parent">The type that the defined type extends.</param>
        /// <param name="packingSize">The packing size of the type.</param>
        /// <param name="typesize">The total size of the type.</param>
        /// <returns>A type created with all requested attributes.</returns>
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
        {
            string ns = null;
            var lastdot = name.LastIndexOf('.');
            if (lastdot > 0)
            {
                ns = name[..lastdot];
                name = name[(lastdot + 1)..];
            }

            var typeBuilder = __DefineType(ns, name);
            typeBuilder.__SetAttributes(attr);
            typeBuilder.SetParent(parent);
            if (packingSize != PackingSize.Unspecified || typesize != 0)
                typeBuilder.__SetLayout((int)packingSize, typesize);

            return typeBuilder;
        }

        /// <summary>
        /// Defines a top-level type with the specified namespace and name, without splitting the name on dots and
        /// without setting attributes or a parent.
        /// </summary>
        /// <param name="ns">The namespace of the type, or <see langword="null"/>.</param>
        /// <param name="name">The name of the type.</param>
        /// <returns>The new type builder.</returns>
        public TypeBuilder __DefineType(string ns, string name) => DefineType(this, ns, name);

        /// <summary>
        /// Defines a type owned by this module or by an enclosing type, and adds it to the module's types.
        /// </summary>
        /// <param name="owner">The module, or the type builder for a nested type.</param>
        /// <param name="ns">The namespace of the type, or <see langword="null"/>.</param>
        /// <param name="name">The name of the type.</param>
        /// <returns>The new type builder.</returns>
        internal TypeBuilder DefineType(ITypeOwner owner, string ns, string name)
        {
            var typeBuilder = new TypeBuilder(owner, ns, name);
            _types.Add(typeBuilder);
            return typeBuilder;
        }

        /// <summary>
        /// Defines a global method with the specified name, attributes, return type, and parameter types.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="attributes">The attributes of the method.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="parameterTypes">The types of the method's parameters.</param>
        /// <returns>The defined global method.</returns>
        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes) =>
            _moduleType.DefineMethod(name, attributes, returnType, parameterTypes);

        /// <summary>
        /// Completes the global function definitions and global data definitions for this dynamic module.
        /// </summary>
        public void CreateGlobalFunctions() => _moduleType.CreateType();

        /// <summary>
        /// Adds an ExportedType row that forwards the specified type to its assembly, optionally including all of its
        /// nested types.
        /// </summary>
        /// <param name="type">The type to forward.</param>
        /// <param name="includeNested"><see langword="true"/> to also forward the nested types, whatever their visibility.</param>
        internal void AddTypeForwarder(Type type, bool includeNested)
        {
            ExportType(type);

            if (includeNested && !type.__IsMissing)
            {
                foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // we export all nested types (i.e. even the private ones)
                    // (this behavior is the same as the C# compiler)
                    AddTypeForwarder(nested, true);
                }
            }
        }

        /// <summary>
        /// Gets the ExportedType token for a forwarded type, adding the row, and the rows of its enclosing types, if
        /// needed.
        /// </summary>
        /// <param name="type">The forwarded type.</param>
        /// <returns>The ExportedType token.</returns>
        int ExportType(Type type)
        {
            var row = new ExportedTypeRow();

            // HACK we should *not* set the TypeDefId in this case, but 2.0 and 3.5 peverify gives a warning if it is missing (4.5 doesn't)
            if (_asm.ImageRuntimeVersion == "v2.0.50727")
                row.TypeDefId = type.MetadataToken;

            SetTypeNameAndTypeNamespace(type.TypeName, out row.Name, out row.Namespace);
            if (type.IsNested)
            {
                row.Flags = 0;
                row.Implementation = ExportType(type.DeclaringType);
            }
            else
            {
                // CorTypeAttr.tdForwarder
                row.Flags = (TypeAttributes)0x00200000;
                row.Implementation = ImportAssemblyRef(type.Assembly);
            }

            var key = (row.Implementation, row.Name, row.Namespace);
            if (_exportedTypeTokens.TryGetValue(key, out var token) == false)
                _exportedTypeTokens.Add(key, token = AddExportedType(row));

            return token;
        }

        /// <summary>
        /// Adds an ExportedType row.
        /// </summary>
        /// <param name="row">The row to add.</param>
        /// <returns>The ExportedType token of the new row.</returns>
        /// <exception cref="InvalidOperationException">The reference tables have already been written.</exception>
        int AddExportedType(ExportedTypeRow row)
        {
            CheckReferencesNotWritten();
            _exportedTypes.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.ExportedTypeHandle(_exportedTypes.Count));
        }

        /// <summary>
        /// Adds the name and namespace of a type to the string heap.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="typeName">The handle of the type's name.</param>
        /// <param name="typeNamespace">The handle of the type's namespace, or a nil handle if it has none.</param>
        void SetTypeNameAndTypeNamespace(TypeName name, out StringHandle typeName, out StringHandle typeNamespace)
        {
            typeName = GetOrAddString(name.Name);
            typeNamespace = name.Namespace == null ? default : GetOrAddString(name.Namespace);
        }

        /// <summary>
        /// Applies a custom attribute to this module by using a custom attribute builder.
        /// </summary>
        /// <param name="customBuilder">An instance of a helper class that specifies the custom attribute to apply.</param>
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) => SetCustomAttribute(0x00000001, customBuilder);

        /// <summary>
        /// Applies a custom attribute to the metadata element with the specified token.
        /// </summary>
        /// <param name="token">The token or pseudo token of the element the attribute applies to.</param>
        /// <param name="customBuilder">The custom attribute.</param>
        internal void SetCustomAttribute(int token, CustomAttributeBuilder customBuilder)
        {
            _customAttributes.Add(new CustomAttributeRow()
            {
                Parent = token,
                Constructor = GetConstructorToken(customBuilder.Constructor).Token,
                Value = customBuilder.WriteBlob(this),
                Builder = customBuilder,
            });
        }

        /// <summary>
        /// Adds the custom attributes set on the given token that are assignable to <paramref name="attributeType"/>, or
        /// all of them if it is <see langword="null"/>.
        /// </summary>
        /// <param name="list">The list to add to, or <see langword="null"/> to create one when a match is found.</param>
        /// <param name="token">The token or pseudo token of the element the attributes are set on.</param>
        /// <param name="attributeType">The attribute type to filter on, or <see langword="null"/> for all.</param>
        /// <returns>The list, or <see langword="null"/> if <paramref name="list"/> was <see langword="null"/> and nothing matched.</returns>
        internal List<CustomAttributeData> GetCustomAttributes(List<CustomAttributeData> list, int token, Type attributeType)
        {
            foreach (var row in _customAttributes)
                if (row.Parent == token && (attributeType == null || attributeType.IsAssignableFrom(row.Builder.Constructor.DeclaringType)))
                    (list ??= []).Add(row.Builder.ToData(_asm));

            return list;
        }

        /// <summary>
        /// Resolves the pseudo tokens of the custom attributes and adds them to the metadata, which sorts them.
        /// </summary>
        void WriteCustomAttributes()
        {
            for (var i = 0; i < _customAttributes.Count; i++)
            {
                var row = _customAttributes[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                row.Constructor = ResolvePseudoToken(row.Constructor);
                _customAttributes[i] = row;
                _metadata.AddCustomAttribute(MetadataTokens.EntityHandle(row.Parent), MetadataTokens.EntityHandle(row.Constructor), row.Value);
            }
        }

        /// <summary>
        /// Defines a binary large object (BLOB) that represents a manifest resource to be embedded in the dynamic
        /// assembly.
        /// </summary>
        /// <param name="name">The case-sensitive name for the resource.</param>
        /// <param name="stream">A stream that contains the bytes for the resource. It is read when the module is written.</param>
        /// <param name="attribute">An enumeration value that specifies whether the resource is public or private.</param>
        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute) =>
            _resourceWriters.Add(new ResourceWriterRecord(name, stream, attribute));

        /// <summary>
        /// Writes the defined resources to the resource blob and metadata tables.
        /// </summary>
        internal void WriteResources()
        {
            foreach (var rwr in _resourceWriters)
            {
                rwr.Write(this);
                rwr.Close();
            }
        }

        /// <summary>
        /// Gets the dynamic assembly that defined this instance of <see cref="ModuleBuilder"/>.
        /// </summary>
        public override Assembly Assembly => _asm;

        /// <summary>
        /// Finds a type defined in this module by name, including nested types.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <returns>The type, or <see langword="null"/> if it is not found.</returns>
        internal override Type FindType(TypeName name)
        {
            foreach (var type in _types)
                if (type.TypeName == name)
                    return type;

            return null;
        }

        /// <summary>
        /// Finds a type defined in this module by name, ignoring case.
        /// </summary>
        /// <param name="lowerCaseName">The name of the type, converted to lower case.</param>
        /// <returns>The type, or <see langword="null"/> if it is not found.</returns>
        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var type in _types)
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            return null;
        }

        /// <summary>
        /// Adds the types defined in this module, except the &lt;Module&gt; type, to the list.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        internal override void GetTypesImpl(List<Type> list)
        {
            foreach (var type in _types)
                if (type != _moduleType)
                    list.Add(type);
        }

        /// <summary>
        /// Defines a document for source.
        /// </summary>
        /// <param name="url">The URL for the document.</param>
        /// <param name="language">The GUID that identifies the document language.</param>
        /// <param name="languageVendor">The GUID that identifies the document language vendor.</param>
        /// <param name="documentType">The GUID that identifies the document type.</param>
        /// <returns>The defined document.</returns>
        /// <exception cref="NotSupportedException">No symbol writer has been set for this module.</exception>
        public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            if (_symbolWriter != null)
                return _symbolWriter.DefineDocument(url, language, languageVendor, documentType);
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the token used to identify the specified type within this module.
        /// </summary>
        /// <param name="type">The type object that represents the class type.</param>
        /// <returns>The token used to identify the given type within this module, importing the type if it is defined elsewhere.</returns>
        public TypeToken GetTypeToken(Type type)
        {
            if (type.Module == this)
                return new TypeToken(type.GetModuleBuilderToken());
            else
                return new TypeToken(ImportType(type));
        }

        /// <summary>
        /// Gets the token to use as the parent of a MemberRef on the specified type. A generic type definition is
        /// referenced through a TypeSpec, and a module's &lt;Module&gt; type through a ModuleRef.
        /// </summary>
        /// <param name="type">The type that declares the member.</param>
        /// <returns>The TypeRef, TypeDef, TypeSpec or ModuleRef token.</returns>
        internal int GetTypeTokenForMemberRef(Type type)
        {
            if (type.__IsMissing)
            {
                return ImportType(type);
            }
            else if (type.IsGenericTypeDefinition)
            {
                if (_memberRefTypeTokens.TryGetValue(type, out var token) == false)
                {
                    var spec = new BlobBuilder(5);
                    Signature.WriteTypeSpec(this, spec, type);
                    token = AddTypeSpec(GetOrAddBlob(spec));
                    _memberRefTypeTokens.Add(type, token);
                }
                return token;
            }
            else if (type.IsModulePseudoType)
            {
                return MetadataTokens.GetToken(GetModuleRef(type.Module.ScopeName));
            }
            else
            {
                return GetTypeToken(type).Token;
            }
        }

        /// <summary>
        /// Returns whether a member is declared on a generic type definition, in which case IL must reference it
        /// through a MemberRef on the definition's TypeSpec.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns><see langword="true"/> if the declaring type is a generic type definition; otherwise <see langword="false"/>.</returns>
        static bool IsFromGenericTypeDefinition(MemberInfo member)
        {
            var decl = member.DeclaringType;
            return decl != null && !decl.__IsMissing && decl.IsGenericTypeDefinition;
        }

        /// <summary>
        /// Returns the token identifying the specified field within this module.
        /// </summary>
        /// <param name="field">The field object representing the field.</param>
        /// <returns>The token of the field: a FieldDef token for a field defined in this module, otherwise a MemberRef token.</returns>
        public FieldToken GetFieldToken(FieldInfo field)
        {
            // NOTE for some reason, when TypeBuilder.GetFieldToken() is used on a field in a generic type definition,
            // a memberref token is returned (confirmed on .NET) unlike for Get(Method|Constructor)Token which always
            // simply returns the MethodDef token (if the method is from the same module).
            var fb = field as FieldBuilder;
            if (fb != null && fb.Module == this && !IsFromGenericTypeDefinition(fb))
            {
                return new FieldToken(fb.MetadataToken);
            }
            else
            {
                return new FieldToken(field.ImportTo(this));
            }
        }

        /// <summary>
        /// Returns the token identifying the specified method within this module.
        /// </summary>
        /// <param name="method">The method to get a token for.</param>
        /// <returns>The token of the method: a MethodDef token for a method defined in this module, otherwise an imported token.</returns>
        public MethodToken GetMethodToken(MethodInfo method)
        {
            var mb = method as MethodBuilder;
            if (mb != null && mb.ModuleBuilder == this)
                return new MethodToken(mb.MetadataToken);
            else
                return new MethodToken(method.ImportTo(this));
        }

        /// <summary>
        /// Returns a MemberRef token for a call site of the specified method, with the signature extended by the
        /// specified vararg parameter types and custom modifiers.
        /// </summary>
        /// <param name="method">The method being called.</param>
        /// <param name="optionalParameterTypes">The types of the optional (vararg) parameters, or <see langword="null"/>.</param>
        /// <param name="customModifiers">The custom modifiers of the return type and parameters, or <see langword="null"/>.</param>
        /// <returns>The MemberRef token.</returns>
        public MethodToken __GetMethodToken(MethodInfo method, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            var sig = new BlobBuilder(16);
            method.MethodSignature.WriteMethodRef(this, sig, optionalParameterTypes, customModifiers);

            var row = new MemberRefRow
            {
                Class = method.Module == this ? method.MetadataToken : GetTypeTokenForMemberRef(method.DeclaringType ?? method.Module.GetModuleType()),
                Name = GetOrAddString(method.Name),
                Signature = GetOrAddBlob(sig)
            };

            var key = (row.Class, row.Name, row.Signature);
            if (_memberRefTokens.TryGetValue(key, out var token) == false)
                _memberRefTokens.Add(key, token = AddMemberRef(row));

            return new MethodToken(token);
        }

        /// <summary>
        /// Returns the token to use for the specified method in the IL stream. A method on a generic type definition
        /// must be referenced through a MemberRef, even if it is in this module, and a generic method definition is
        /// referenced as its instantiation over its own type parameters.
        /// </summary>
        /// <param name="method">The method to get a token for.</param>
        /// <returns>The token of the method.</returns>
        internal MethodToken GetMethodTokenForIL(MethodInfo method)
        {
            if (method.IsGenericMethodDefinition)
                method = method.MakeGenericMethod(method.GetGenericArguments());

            if (IsFromGenericTypeDefinition(method))
                return new MethodToken(method.ImportTo(this));
            else
                return GetMethodToken(method);
        }

        /// <summary>
        /// Returns the token used to identify the specified constructor within this module.
        /// </summary>
        /// <param name="constructor">The constructor to get a token for.</param>
        /// <returns>The token used to identify the specified constructor within this module.</returns>
        public MethodToken GetConstructorToken(ConstructorInfo constructor) => GetMethodToken(constructor.GetMethodInfo());

        /// <summary>
        /// Gets the MemberRef token for a field or method, adding the MemberRef the first time the member is
        /// imported.
        /// </summary>
        /// <param name="declaringType">The type that declares the member.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="sig">The signature of the member.</param>
        /// <returns>The MemberRef token.</returns>
        internal int ImportMethodOrField(Type declaringType, string name, Signature sig)
        {
            var key = new MemberRefKey(declaringType, name, sig);
            if (!_importedMemberRefs.TryGetValue(key, out var token))
            {
                var row = new MemberRefRow
                {
                    Class = GetTypeTokenForMemberRef(declaringType),
                    Name = GetOrAddString(name)
                };
                var bb = new BlobBuilder(16);
                sig.Write(this, bb);
                row.Signature = GetOrAddBlob(bb);
                token = AddMemberRef(row);
                _importedMemberRefs.Add(key, token);
            }

            return token;
        }

        /// <summary>
        /// Gets the MethodSpec token for an instantiation of a generic method, adding the MethodSpec the first time
        /// the instantiation is imported.
        /// </summary>
        /// <param name="declaringType">The type that declares the method, which may be a generic type instance.</param>
        /// <param name="method">The generic method definition.</param>
        /// <param name="genericParameters">The type arguments of the instantiation.</param>
        /// <returns>The MethodSpec token.</returns>
        internal int ImportMethodSpec(Type declaringType, MethodInfo method, Type[] genericParameters)
        {
            var key = new MethodSpecKey(declaringType, method.Name, method.MethodSignature, genericParameters);
            if (!_importedMethodSpecs.TryGetValue(key, out var token))
            {
                var row = new MethodSpecRow();

                // 'method' may be a MethodDef on a generic TypeDef and 'declaringType' the type instance (in other words
                // the method and type have already been decoupled by the caller), so import it with the declaring type
                if (method is MethodBuilder mb && mb.ModuleBuilder == this && !declaringType.IsGenericType)
                    row.Method = mb.MetadataToken;
                else
                    row.Method = ImportMethodOrField(declaringType, method.Name, method.MethodSignature);

                var spec = new BlobBuilder(10);
                Signature.WriteMethodSpec(this, spec, genericParameters);
                row.Instantiation = GetOrAddBlob(spec);

                var rowKey = (row.Method, row.Instantiation);
                if (_methodSpecTokens.TryGetValue(rowKey, out token) == false)
                {
                    CheckReferencesNotWritten();
                    _methodSpecs.Add(row);
                    _methodSpecTokens.Add(rowKey, token = MetadataTokens.GetToken(MetadataTokens.MethodSpecificationHandle(_methodSpecs.Count)));
                }

                _importedMethodSpecs.Add(key, token);
            }

            return token;
        }

        /// <summary>
        /// Gets the token by which this module references a type, adding a TypeSpec for an array, pointer, by-ref,
        /// generic instance or function pointer type, and a TypeRef otherwise.
        /// </summary>
        /// <param name="type">The type to reference.</param>
        /// <returns>The TypeSpec or TypeRef token.</returns>
        internal int ImportType(Type type)
        {
            if (_typeTokens.TryGetValue(type, out var token) == false)
            {
                if (type.HasElementType || type.IsConstructedGenericType || type.IsFunctionPointer)
                {
                    var spec = new BlobBuilder(5);
                    Signature.WriteTypeSpec(this, spec, type);
                    token = AddTypeSpec(GetOrAddBlob(spec));
                }
                else
                {
                    int scope;
                    if (type.IsNested)
                        scope = GetTypeToken(type.DeclaringType).Token;
                    else if (type.Module == this)
                        scope = 1;
                    else
                        scope = ImportAssemblyRef(type.Assembly);

                    token = AddTypeRef(scope, type.TypeName);
                }

                _typeTokens.Add(type, token);
            }

            return token;
        }

        /// <summary>
        /// Gets the pseudo token that stands for the AssemblyRef of the specified assembly.
        /// </summary>
        /// <param name="asm">The referenced assembly.</param>
        /// <returns>The pseudo token, resolved to an AssemblyRef token by <see cref="FillAssemblyRefTable"/>.</returns>
        int ImportAssemblyRef(Assembly asm)
        {
            if (_referencedAssemblies.TryGetValue(asm, out var token) == false)
            {
                // we can't write the AssemblyRef record here yet, because the identity of the assembly can still change
                // (if it's an AssemblyBuilder)
                token = AllocPseudoToken();
                _referencedAssemblies.Add(asm, token);
            }

            return token;
        }

        /// <summary>
        /// Adds the AssemblyRef rows of the referenced assemblies and resolves the pseudo tokens that stand for them.
        /// </summary>
        internal void FillAssemblyRefTable()
        {
            foreach (var kv in _referencedAssemblies)
                if (IsPseudoToken(kv.Value))
                    RegisterTokenFixup(kv.Value, FindOrAddAssemblyRef(kv.Key.GetName()));
        }

        /// <summary>
        /// Gets the AssemblyRef token for an assembly name, adding the row if no equal reference exists.
        /// </summary>
        /// <param name="name">The name of the referenced assembly.</param>
        /// <returns>The AssemblyRef token.</returns>
        int FindOrAddAssemblyRef(AssemblyName name)
        {
            var version = name.Version ?? new Version(0, 0, 0, 0);
            var flags = (int)(name.Flags & ~AssemblyNameFlags.PublicKey);
            const AssemblyNameFlags afPA_Specified = (AssemblyNameFlags)0x0080;
            const AssemblyNameFlags afPA_Mask = (AssemblyNameFlags)0x0070;
            if ((name.RawFlags & afPA_Specified) != 0)
                flags |= (int)(name.RawFlags & afPA_Mask);
            if (name.ContentType == AssemblyContentType.WindowsRuntime)
                flags |= 0x0200;

            var publicKeyOrToken = GetOrAddBlob(name.GetPublicKeyToken() ?? Array.Empty<byte>());
            var nameHandle = GetOrAddString(name.Name);
            var culture = name.CultureName == null ? default : GetOrAddString(name.CultureName);

            // references that differ only in their hash are the same reference
            var key = (nameHandle, version, culture, publicKeyOrToken, flags);
            if (_assemblyRefs.TryGetValue(key, out var handle) == false)
                _assemblyRefs.Add(key, handle = _metadata.AddAssemblyReference(nameHandle, version, culture, publicKeyOrToken, (System.Reflection.AssemblyFlags)flags, name.Hash != null ? GetOrAddBlob(name.Hash) : default));

            return MetadataTokens.GetToken(handle);
        }

        /// <summary>
        /// Adds a type specification, returning its token. Type specifications refer to nothing that is resolved later, so
        /// they are written immediately.
        /// </summary>
        /// <param name="signature">The handle of the type specification signature.</param>
        /// <returns>The TypeSpec token.</returns>
        internal int AddTypeSpec(BlobHandle signature) => MetadataTokens.GetToken(_metadata.AddTypeSpecification(signature));

        /// <summary>
        /// Gets the stand alone signature with the given blob, adding it if needed.
        /// </summary>
        /// <param name="signature">The handle of the signature blob.</param>
        /// <returns>The handle of the StandAloneSig row.</returns>
        internal StandaloneSignatureHandle GetStandAloneSignature(BlobHandle signature)
        {
            if (_standAloneSignatureTokens.TryGetValue(signature, out var token) == false)
                _standAloneSignatureTokens.Add(signature, token = MetadataTokens.GetToken(_metadata.AddStandaloneSignature(signature)));

            return MetadataTokens.StandaloneSignatureHandle(MetadataTokens.GetRowNumber(MetadataTokens.EntityHandle(token)));
        }

        /// <summary>
        /// Gets the module reference with the given name, adding it if needed.
        /// </summary>
        /// <param name="name">The name of the referenced module, or <see langword="null"/>.</param>
        /// <returns>The handle of the ModuleRef row.</returns>
        ModuleReferenceHandle GetModuleRef(string name)
        {
            var handle = name == null ? default : GetOrAddString(name);
            if (_moduleRefs.TryGetValue(handle, out var moduleRef) == false)
                _moduleRefs.Add(handle, moduleRef = _metadata.AddModuleReference(handle));

            return moduleRef;
        }

        /// <summary>
        /// Records a TypeRef row, to be written with the other references.
        /// </summary>
        /// <param name="resolutionScope">The token or pseudo token of the resolution scope.</param>
        /// <param name="name">The name of the referenced type.</param>
        /// <returns>The TypeRef token of the new row.</returns>
        /// <exception cref="InvalidOperationException">The reference tables have already been written.</exception>
        int AddTypeRef(int resolutionScope, TypeName name)
        {
            CheckReferencesNotWritten();
            var row = new TypeRefRow() { ResolutionScope = resolutionScope };
            SetTypeNameAndTypeNamespace(name, out row.Name, out row.Namespace);
            _typeRefs.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.TypeReferenceHandle(_typeRefs.Count));
        }

        /// <summary>
        /// Records a MemberRef row, to be written with the other references.
        /// </summary>
        /// <param name="row">The row to add.</param>
        /// <returns>The MemberRef token of the new row.</returns>
        /// <exception cref="InvalidOperationException">The reference tables have already been written.</exception>
        int AddMemberRef(MemberRefRow row)
        {
            CheckReferencesNotWritten();
            _memberRefs.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.MemberReferenceHandle(_memberRefs.Count));
        }

        /// <summary>
        /// Records a manifest resource.
        /// </summary>
        /// <param name="flags">The attributes of the resource.</param>
        /// <param name="name">The name of the resource.</param>
        /// <param name="implementation">The token of the File or AssemblyRef that holds the resource, or 0 for a resource embedded in this module.</param>
        /// <param name="offset">The offset of an embedded resource in the resource stream.</param>
        /// <exception cref="InvalidOperationException">The reference tables have already been written.</exception>
        internal void AddManifestResource(ResourceAttributes flags, string name, int implementation, int offset)
        {
            CheckReferencesNotWritten();
            _manifestResources.Add(new ManifestResourceRow() { Flags = flags, Name = GetOrAddString(name), Implementation = implementation, Offset = offset });
        }

        /// <summary>
        /// Throws if the reference tables have already been written, since rows added afterwards would be lost.
        /// </summary>
        /// <exception cref="InvalidOperationException">The reference tables have already been written.</exception>
        void CheckReferencesNotWritten()
        {
            if (_referencesWritten)
                throw new InvalidOperationException("A reference was added after the references of the module were written.");
        }

        /// <summary>
        /// Resolves the pseudo tokens of the type, member and method references, exported types and manifest resources and
        /// adds them to the metadata, in the order they were created so their tokens stay valid.
        /// </summary>
        void WriteReferences()
        {
            _referencesWritten = true;

            foreach (var row in _typeRefs)
                _metadata.AddTypeReference(MetadataTokens.EntityHandle(ResolvePseudoToken(row.ResolutionScope)), row.Namespace, row.Name);

            foreach (var row in _memberRefs)
                _metadata.AddMemberReference(MetadataTokens.EntityHandle(ResolvePseudoToken(row.Class)), row.Name, row.Signature);

            foreach (var row in _methodSpecs)
                _metadata.AddMethodSpecification(MetadataTokens.EntityHandle(ResolvePseudoToken(row.Method)), row.Instantiation);

            foreach (var row in _exportedTypes)
                _metadata.AddExportedType((System.Reflection.TypeAttributes)row.Flags, row.Namespace, row.Name, MetadataTokens.EntityHandle(ResolvePseudoToken(row.Implementation)), row.TypeDefId);

            foreach (var row in _manifestResources)
                _metadata.AddManifestResource((System.Reflection.ManifestResourceAttributes)row.Flags, row.Name, MetadataTokens.EntityHandle(ResolvePseudoToken(row.Implementation)), (uint)row.Offset);
        }

        /// <summary>
        /// Records the real token that a pseudo token stands for.
        /// </summary>
        /// <param name="pseudoToken">The pseudo token.</param>
        /// <param name="realToken">The real metadata token.</param>
        internal void RegisterTokenFixup(int pseudoToken, int realToken)
        {
            var index = -(pseudoToken + 1);
            while (_resolvedTokens.Count <= index)
                _resolvedTokens.Add(0);

            _resolvedTokens[index] = realToken;
        }

        /// <summary>
        /// Returns whether a token is a pseudo token, which is the case when it is negative.
        /// </summary>
        /// <param name="token">The token to check.</param>
        /// <returns><see langword="true"/> if <paramref name="token"/> is a pseudo token; otherwise <see langword="false"/>.</returns>
        internal static bool IsPseudoToken(int token) => token < 0;

        /// <summary>
        /// Returns the real token a pseudo token stands for, or the token itself if it is not a pseudo token.
        /// </summary>
        /// <param name="pseudoToken">The token to resolve.</param>
        /// <returns>The real token.</returns>
        internal int ResolvePseudoToken(int pseudoToken) => IsPseudoToken(pseudoToken) ? _resolvedTokens[-(pseudoToken + 1)] : pseudoToken;

        /// <summary>
        /// Assigns the real MethodDef, FieldDef and Param tokens of all types' members, in table order, and registers
        /// them as the fixups of their pseudo tokens.
        /// </summary>
        internal void FixupMethodBodyTokens()
        {
            var methodToken = 0x06000001;
            var fieldToken = 0x04000001;
            var parameterToken = 0x08000001;

            foreach (var type in _types)
                type.ResolveMethodAndFieldTokens(ref methodToken, ref fieldToken, ref parameterToken);
        }

        /// <summary>
        /// Writes all of the metadata of the module.
        /// </summary>
        internal void WriteMetadata()
        {
            WriteTypeDefTable();
            WriteFieldTable();
            WriteMethodDefTable();
            WriteParamTable();
            WriteTypeStructure();
            WriteMemberData();
            WriteReferences();
            WriteCustomAttributes();
        }

        /// <summary>
        /// Adds ExportedType rows to the manifest module for the visible types of this module, which is a secondary
        /// module of a multi-module assembly.
        /// </summary>
        /// <param name="fileToken">The File row of this module in the manifest module.</param>
        /// <param name="manifestModule">The manifest module of the assembly.</param>
        internal void ExportTypes(AssemblyFileHandle fileToken, ModuleBuilder manifestModule) =>
            manifestModule.ExportTypes(_types.ToArray(), fileToken);

        /// <summary>
        /// Adds ExportedType rows for the visible types of another module of the assembly.
        /// </summary>
        /// <param name="types">The types of the other module. Enclosing types must come before their nested types.</param>
        /// <param name="fileToken">The File row of the module that defines the types.</param>
        internal void ExportTypes(Type[] types, AssemblyFileHandle fileToken)
        {
            var declaringTypes = new Dictionary<Type, ExportedTypeHandle>();

            foreach (var type in types)
            {
                if (type.IsModulePseudoType == false && IsVisible(type))
                {
                    var row = new ExportedTypeRow
                    {
                        Flags = type.Attributes,
                        // LAMESPEC ECMA says that TypeDefId is a row index, but it should be a token
                        TypeDefId = type.MetadataToken
                    };
                    SetTypeNameAndTypeNamespace(type.TypeName, out row.Name, out row.Namespace);
                    row.Implementation = type.IsNested ? MetadataTokens.GetToken(declaringTypes[type.DeclaringType]) : MetadataTokens.GetToken(fileToken);
                    declaringTypes.Add(type, (ExportedTypeHandle)MetadataTokens.EntityHandle(AddExportedType(row)));
                }
            }
        }

        /// <summary>
        /// Returns whether a type is visible outside the assembly and therefore needs an ExportedType row. Unlike
        /// <see cref="Type.IsVisible"/>, nested types with family access count as visible.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><see langword="true"/> if the type is visible outside the assembly; otherwise <see langword="false"/>.</returns>
        static bool IsVisible(Type type) =>
            type.IsPublic || ((type.IsNestedFamily || type.IsNestedFamORAssem || type.IsNestedPublic) && IsVisible(type.DeclaringType));

        /// <summary>
        /// Records the constant value of a field, parameter or property.
        /// </summary>
        /// <param name="parentToken">The token or pseudo token of the field, parameter or property.</param>
        /// <param name="value">The constant value. A <see cref="DateTime"/> is stored as its ticks.</param>
        /// <exception cref="ArgumentException">The value is not of a type a constant can have.</exception>
        internal void AddConstant(int parentToken, object value)
        {
            // metadata has no DateTime constants; like .NET, store the ticks
            if (value is DateTime dateTime)
                value = dateTime.Ticks;
            else if (value is not (null or bool or char or sbyte or byte or short or ushort or int or uint or long or ulong or float or double or string))
                throw new ArgumentException("Unsupported constant type.", nameof(value));

            _constants.Add(new ConstantRow() { Parent = parentToken, Value = value });
        }

        /// <summary>
        /// Gets the constant value recorded for the given token.
        /// </summary>
        /// <param name="token">The token the constant was recorded with.</param>
        /// <returns>The constant value.</returns>
        /// <exception cref="InvalidOperationException">No constant was recorded.</exception>
        internal object GetConstant(int token)
        {
            foreach (var row in _constants)
                if (row.Parent == token)
                    return row.Value;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Records the marshalling descriptor of a field or parameter.
        /// </summary>
        /// <param name="parentToken">The token or pseudo token of the field or parameter.</param>
        /// <param name="nativeType">The encoded marshalling descriptor.</param>
        internal void AddFieldMarshal(int parentToken, byte[] nativeType) =>
            _fieldMarshals.Add(new FieldMarshalRow() { Parent = parentToken, NativeType = nativeType });

        /// <summary>
        /// Gets the marshalling descriptor recorded for the given token.
        /// </summary>
        /// <param name="token">The token the descriptor was recorded with.</param>
        /// <param name="nativeType">The encoded marshalling descriptor, or <see langword="null"/> if none was recorded.</param>
        /// <returns><see langword="true"/> if a descriptor was recorded; otherwise <see langword="false"/>.</returns>
        internal bool TryGetFieldMarshal(int token, out byte[] nativeType)
        {
            foreach (var row in _fieldMarshals)
            {
                if (row.Parent == token)
                {
                    nativeType = row.NativeType;
                    return true;
                }
            }

            nativeType = default;
            return false;
        }

        /// <summary>
        /// Records the P/Invoke import of a method.
        /// </summary>
        /// <param name="methodToken">The token or pseudo token of the method.</param>
        /// <param name="flags">The P/Invoke flags.</param>
        /// <param name="importName">The name of the imported function.</param>
        /// <param name="importScope">The name of the module (DLL) that exports the function.</param>
        internal void AddImplMap(int methodToken, ImplMapFlags flags, string importName, string importScope)
        {
            _implMaps.Add(new ImplMapRow()
            {
                Method = methodToken,
                Flags = flags,
                ImportName = importName,
                ImportScope = importScope,
                ImportScopeHandle = GetModuleRef(importScope),
            });
        }

        /// <summary>
        /// Gets the P/Invoke import recorded for the given method token.
        /// </summary>
        /// <param name="token">The token the import was recorded with.</param>
        /// <param name="flags">The P/Invoke flags.</param>
        /// <param name="importName">The name of the imported function.</param>
        /// <param name="importScope">The name of the module (DLL) that exports the function.</param>
        /// <returns><see langword="true"/> if an import was recorded; otherwise <see langword="false"/>.</returns>
        internal bool TryGetImplMap(int token, out ImplMapFlags flags, out string importName, out string importScope)
        {
            foreach (var row in _implMaps)
            {
                if (row.Method == token)
                {
                    flags = row.Flags;
                    importName = row.ImportName;
                    importScope = row.ImportScope;
                    return true;
                }
            }

            flags = 0;
            importName = null;
            importScope = null;
            return false;
        }

        /// <summary>
        /// Resolves the pseudo tokens of the method semantics, constants, marshalling descriptors and P/Invoke imports
        /// and adds them to the metadata. The metadata sorts the Constant and FieldMarshal tables, but requires ImplMap
        /// to be added in order.
        /// </summary>
        void WriteMemberData()
        {
            foreach (var row in _methodSemantics)
                _metadata.AddMethodSemantics(MetadataTokens.EntityHandle(row.Association), (System.Reflection.MethodSemanticsAttributes)row.Semantics, (MethodDefinitionHandle)MetadataTokens.EntityHandle(ResolvePseudoToken(row.Method)));

            for (var i = 0; i < _constants.Count; i++)
            {
                var row = _constants[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                _constants[i] = row;
                _metadata.AddConstant(MetadataTokens.EntityHandle(row.Parent), row.Value);
            }

            for (var i = 0; i < _fieldMarshals.Count; i++)
            {
                var row = _fieldMarshals[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                _fieldMarshals[i] = row;
                _metadata.AddMarshallingDescriptor(MetadataTokens.EntityHandle(row.Parent), _metadata.GetOrAddBlob(row.NativeType));
            }

            for (var i = 0; i < _implMaps.Count; i++)
            {
                var row = _implMaps[i];
                row.Method = ResolvePseudoToken(row.Method);
                _implMaps[i] = row;
            }

            _implMaps.Sort((x, y) => x.Method.CompareTo(y.Method));
            foreach (var row in _implMaps)
                _metadata.AddMethodImport((MethodDefinitionHandle)MetadataTokens.EntityHandle(row.Method), (System.Reflection.MethodImportAttributes)row.Flags, GetOrAddString(row.ImportName), row.ImportScopeHandle);
        }

        /// <inheritdoc />
        ModuleBuilder ITypeOwner.ModuleBuilder => this;

        /// <summary>
        /// Returns the type defined in this module with the specified TypeDef token.
        /// </summary>
        /// <param name="metadataToken">A TypeDef token.</param>
        /// <param name="context">The generic context. Not used.</param>
        /// <returns>The type with the specified token.</returns>
        /// <exception cref="NotImplementedException">The token is not a TypeDef token.</exception>
        internal override Type ResolveType(int metadataToken, IGenericContext context)
        {
            if (metadataToken >> 24 != (int)TableIndex.TypeDef)
            {
                throw new NotImplementedException();
            }
            return _types[(metadataToken & 0xFFFFFF) - 1];
        }

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token. Supports MemberRef tokens of
        /// imported methods and the tokens of methods defined in this module.
        /// </summary>
        /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
        /// <param name="genericTypeArguments">Must be <see langword="null"/>.</param>
        /// <param name="genericMethodArguments">Must be <see langword="null"/>.</param>
        /// <returns>The method, or <see langword="null"/> if it is not found.</returns>
        /// <exception cref="NotImplementedException"><paramref name="genericTypeArguments"/> or <paramref name="genericMethodArguments"/> is not <see langword="null"/>.</exception>
        public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            if (genericTypeArguments != null || genericMethodArguments != null)
            {
                throw new NotImplementedException();
            }
            // this method is inefficient, but since it isn't used we don't care
            if ((metadataToken >> 24) == (int)TableIndex.MemberRef)
            {
                foreach (KeyValuePair<MemberRefKey, int> kv in _importedMemberRefs)
                {
                    if (kv.Value == metadataToken)
                    {
                        return kv.Key.LookupMethod();
                    }
                }
            }
            // HACK if we're given a SymbolToken, we need to convert back
            if ((metadataToken & 0xFF000000) == 0x06000000)
            {
                metadataToken = -(metadataToken & 0x00FFFFFF);
            }
            foreach (Type type in _types)
            {
                MethodBase method = ((TypeBuilder)type).LookupMethod(metadataToken);
                if (method != null)
                {
                    return method;
                }
            }
            return ((TypeBuilder)_moduleType).LookupMethod(metadataToken);
        }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override string ResolveString(int metadataToken) => throw new NotImplementedException();

        /// <summary>
        /// Gets the fully qualified name of the file the module is written to, in the assembly's directory.
        /// </summary>
        public override string FullyQualifiedName => Path.GetFullPath(Path.Combine(_asm.Dir, FileName));

        /// <summary>
        /// Gets the name of the file the module is written to.
        /// </summary>
        public override string Name => FileName;

        /// <summary>
        /// Gets the reserved space in the GUID heap that receives the module version id when the image is written.
        /// </summary>
        /// <returns>The reserved GUID blob.</returns>
        internal ReservedBlob<GuidHandle> GetModuleVersionIdFixup() => _mvidFixup;

        /// <summary>
        /// Gets the module version id. Before the image is written this is a provisional random id, which the image
        /// replaces when it is written.
        /// </summary>
        /// <exception cref="InvalidOperationException">The universe is deterministic and the image has not been written yet.</exception>
        public override Guid ModuleVersionId
        {
            get
            {
                // if a deterministic GUID is used, it can't be queried before the assembly has been written
                if (_mvid == Guid.Empty && Universe.Deterministic)
                    throw new InvalidOperationException();

                return _mvid;
            }
        }

        /// <summary>
        /// Records the module version id the image was written with.
        /// </summary>
        /// <param name="value">The module version id.</param>
        internal void SetModuleVersionId(Guid value) => _mvid = value;

        /// <summary>
        /// Gets a string that represents the name of the dynamic module.
        /// </summary>
        public override string ScopeName => _moduleName;

        /// <summary>
        /// Returns the symbol writer associated with this dynamic module.
        /// </summary>
        /// <returns>The symbol writer, or <see langword="null"/> if none has been set.</returns>
        public ISymbolWriter GetSymWriter() => _symbolWriter;

        /// <summary>
        /// Returns the token of the given string in the module's constant pool.
        /// </summary>
        /// <param name="str">The string to add to the module's constant pool.</param>
        /// <returns>The token of the added string.</returns>
        public StringToken GetStringConstant(string str) =>
            new StringToken(MetadataTokens.GetHeapOffset(_metadata.GetOrAddUserString(str)) | (0x70 << 24));

        /// <summary>
        /// Defines a token for the signature that is defined by the specified <see cref="SignatureHelper"/>.
        /// </summary>
        /// <param name="sigHelper">The signature.</param>
        /// <returns>The token for the defined signature.</returns>
        public SignatureToken GetSignatureToken(SignatureHelper sigHelper) =>
            new SignatureToken(MetadataTokens.GetToken(GetStandAloneSignature(GetOrAddBlob(sigHelper.GetSignature(this)))));

        /// <summary>
        /// Gets the &lt;Module&gt; type, which holds the global methods and fields.
        /// </summary>
        /// <returns>The &lt;Module&gt; type.</returns>
        internal override Type GetModuleType() => _moduleType;

        /// <summary>
        /// Encodes a signature and adds it to the blob heap.
        /// </summary>
        /// <param name="sig">The signature to add.</param>
        /// <returns>The handle of the signature in the blob heap.</returns>
        internal BlobHandle GetSignatureBlobIndex(Signature sig)
        {
            var bb = new BlobBuilder(16);
            sig.Write(this, bb);
            return GetOrAddBlob(bb);
        }

        /// <summary>
        /// Gets or sets the preferred base address of the image.
        /// </summary>
        public new ulong __ImageBase
        {
            get => _imageBaseAddress;
            set => _imageBaseAddress = value;
        }

        /// <summary>
        /// Gets or sets the file alignment of the image sections.
        /// </summary>
        public new uint __FileAlignment
        {
            get => _fileAlignment;
            set => _fileAlignment = value;
        }

        /// <summary>
        /// Gets or sets the DLL characteristics written to the optional header of the image.
        /// </summary>
        public new DllCharacteristics __DllCharacteristics
        {
            get => _dllCharacteristics;
            set => _dllCharacteristics = value;
        }

        /// <summary>
        /// Gets the metadata stream version of the assembly.
        /// </summary>
        public override int MDStreamVersion => _asm.MdStreamVersion;

        /// <summary>
        /// Records a TypeRef row for a type given by namespace and name.
        /// </summary>
        /// <param name="resolutionScope">The token or pseudo token of the resolution scope.</param>
        /// <param name="ns">The namespace of the type.</param>
        /// <param name="name">The name of the type.</param>
        /// <returns>The TypeRef token of the new row.</returns>
        int AddTypeRefByName(int resolutionScope, string ns, string name) => AddTypeRef(resolutionScope, new TypeName(ns, name));

        /// <summary>
        /// Saves the module as a stand-alone module.
        /// </summary>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="imageFileMachine">The platform targeted by the image.</param>
        public void __Save(PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine) =>
            __Save(null, null, portableExecutableKind, imageFileMachine);

        /// <summary>
        /// Saves the module and its debug information to the specified streams.
        /// </summary>
        /// <param name="peStream">The stream to write the image to.</param>
        /// <param name="pdbStream">The stream to write the debug information to, or <see langword="null"/>.</param>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="imageFileMachine">The platform targeted by the image.</param>
        /// <exception cref="ArgumentException">A stream does not support writing.</exception>
        public void __Save(Stream peStream, Stream pdbStream, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine) =>
            SaveImpl(peStream, pdbStream, portableExecutableKind, imageFileMachine);

        /// <summary>
        /// Implements the Save functionality. The assembly's custom attributes are attached to placeholder TypeRefs
        /// (System.Runtime.CompilerServices.AssemblyAttributesGoHere and its variants), since a module that is not
        /// the manifest module has no Assembly row to hold them.
        /// </summary>
        /// <param name="peStream">The stream to write the image to.</param>
        /// <param name="pdbStream">The stream to write the debug information to, or <see langword="null"/>.</param>
        /// <param name="portableExecutableKind">The nature of the code in the image.</param>
        /// <param name="imageFileMachine">The platform targeted by the image.</param>
        /// <exception cref="ArgumentException">A stream does not support writing.</exception>
        /// <exception cref="InvalidOperationException">The module has already been saved.</exception>
        void SaveImpl(Stream peStream, Stream pdbStream, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (peStream != null && peStream.CanWrite == false)
                throw new ArgumentException("PE stream must support write.", nameof(peStream));
            if (pdbStream != null && pdbStream.CanWrite == false)
                throw new ArgumentException("PDB stream must support write.", nameof(pdbStream));

            SetIsSaved();
            PopulatePropertyTables();

            var attributes = _asm.GetCustomAttributesData(null);
            if (attributes.Count > 0)
            {
                var mscorlib = ImportAssemblyRef(Universe.CoreLib);
                var placeholderTokens = new int[4];
                var placeholderTypeNames = new string[] { "AssemblyAttributesGoHere", "AssemblyAttributesGoHereM", "AssemblyAttributesGoHereS", "AssemblyAttributesGoHereSM" };

                foreach (var cad in attributes)
                {
                    int index;
                    if (cad.Constructor.DeclaringType.BaseType == Universe.System_Security_Permissions_CodeAccessSecurityAttribute)
                    {
                        if (cad.Constructor.DeclaringType.IsAllowMultipleCustomAttribute)
                            index = 3;
                        else
                            index = 2;
                    }
                    else if (cad.Constructor.DeclaringType.IsAllowMultipleCustomAttribute)
                    {
                        index = 1;
                    }
                    else
                    {
                        index = 0;
                    }

                    if (placeholderTokens[index] == 0)
                    {
                        // we manually add a TypeRef without looking it up in mscorlib, because Mono and Silverlight's mscorlib don't have these types
                        placeholderTokens[index] = AddTypeRefByName(mscorlib, "System.Runtime.CompilerServices", placeholderTypeNames[index]);
                    }

                    SetCustomAttribute(placeholderTokens[index], cad.__ToBuilder());
                }
            }

            FillAssemblyRefTable();
            ModuleWriter.WriteModule(null, null, this, PEFileKinds.Dll, portableExecutableKind, imageFileMachine, null, default, null, peStream, null, pdbStream);
        }

        /// <inheritdoc />
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override Type[] __GetExportedTypes() => throw new NotImplementedException();

        /// <summary>
        /// Adds a File row that references another file of the assembly.
        /// </summary>
        /// <param name="flags">The file attributes. 0x0001 (ContainsNoMetaData) marks a file that is not a module.</param>
        /// <param name="name">The name of the file.</param>
        /// <param name="hash">The hash of the file's contents.</param>
        /// <returns>The File token.</returns>
        public int __AddModule(int flags, string name, byte[] hash)
        {
            const int ContainsNoMetaData = 0x0001;
            return MetadataTokens.GetToken(_metadata.AddAssemblyFile(GetOrAddString(name), GetOrAddBlob(hash), (flags & ContainsNoMetaData) == 0));
        }

        /// <summary>
        /// Marks the module as saved, after which members report their real tokens rather than their pseudo tokens.
        /// </summary>
        /// <exception cref="InvalidOperationException">The module has already been saved.</exception>
        internal void SetIsSaved()
        {
            if (_saved)
                throw new InvalidOperationException();

            _saved = true;
        }

        /// <summary>
        /// Gets whether the module has been saved, after which members report their real tokens rather than their
        /// pseudo tokens.
        /// </summary>
        internal bool IsSaved => _saved;

    }

}
