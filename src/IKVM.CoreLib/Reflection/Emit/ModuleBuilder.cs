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

    internal sealed class ModuleBuilder : Module, ITypeOwner
    {

        struct ResourceWriterRecord
        {

            readonly string _name;
            readonly ResourceWriter _rw;
            readonly Stream _stream;
            readonly ResourceAttributes _attributes;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="stream"></param>
            /// <param name="attributes"></param>
            internal ResourceWriterRecord(string name, Stream stream, ResourceAttributes attributes) :
                this(name, null, stream, attributes)
            {
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="rw"></param>
            /// <param name="stream"></param>
            /// <param name="attributes"></param>
            internal ResourceWriterRecord(string name, ResourceWriter rw, Stream stream, ResourceAttributes attributes)
            {
                this._name = name;
                this._rw = rw;
                this._stream = stream;
                this._attributes = attributes;
            }

            /// <summary>
            /// Writes the resource to the resource stream.
            /// </summary>
            /// <param name="module"></param>
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

            internal readonly void Close()
            {
                _rw?.Close();
            }

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
        /// A constant of a field, parameter or property. The parent can be a pseudo token until the module is written.
        /// </summary>
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

        readonly struct MemberRefKey : IEquatable<MemberRefKey>
        {

            readonly Type _type;
            readonly string _name;
            readonly Signature _signature;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            /// <param name="signature"></param>
            internal MemberRefKey(Type type, string name, Signature signature)
            {
                this._type = type;
                this._name = name;
                this._signature = signature;
            }

            public bool Equals(MemberRefKey other) => other._type.Equals(_type) && other._name == _name && other._signature.Equals(_signature);

            public override bool Equals(object obj) => obj is MemberRefKey other && Equals(other);

            public override int GetHashCode()
            {
                return _type.GetHashCode() + _name.GetHashCode() + _signature.GetHashCode();
            }

            internal MethodBase LookupMethod()
            {
                return _type.FindMethod(_name, (MethodSignature)_signature);
            }

        }

        readonly struct MethodSpecKey : IEquatable<MethodSpecKey>
        {

            readonly Type _type;
            readonly string _name;
            readonly MethodSignature _signature;
            readonly Type[] _genericParameters;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            /// <param name="signature"></param>
            /// <param name="genericParameters"></param>
            internal MethodSpecKey(Type type, string name, MethodSignature signature, Type[] genericParameters)
            {
                this._type = type;
                this._name = name;
                this._signature = signature;
                this._genericParameters = genericParameters;
            }

            public bool Equals(MethodSpecKey other)
            {
                return other._type.Equals(_type) && other._name == _name && other._signature.Equals(_signature) && Util.ArrayEquals(other._genericParameters, _genericParameters);
            }

            public override bool Equals(object obj) => obj is MethodSpecKey other && Equals(other);

            public override int GetHashCode() => _type.GetHashCode() + _name.GetHashCode() + _signature.GetHashCode() + Util.GetHashCode(_genericParameters);

        }

        readonly MetadataBuilder _metadata;
        readonly BlobBuilder _ilStream;
        readonly MethodBodyStreamEncoder _methodBodyEncoder;
        readonly BlobBuilder _resourceStream;
        readonly AssemblyBuilder _asm;
        Guid _mvid;
        ReservedBlob<GuidHandle> _mvidFixup;
        ulong _imageBaseAddress = 0;
        uint _fileAlignment = 0;
        DllCharacteristics _dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NoSEH | DllCharacteristics.NXCompat | DllCharacteristics.TerminalServerAware;
        readonly string _moduleName;
        internal readonly string FileName;
        readonly TypeBuilder _moduleType;
        readonly List<TypeBuilder> _types = new List<TypeBuilder>();
        readonly Dictionary<Type, int> _typeTokens = new Dictionary<Type, int>();
        readonly Dictionary<Type, int> _memberRefTypeTokens = new Dictionary<Type, int>();
        internal ModuleResourceSectionBuilder NativeResources;
        readonly Dictionary<MemberRefKey, int> _importedMemberRefs = new Dictionary<MemberRefKey, int>();
        readonly Dictionary<MethodSpecKey, int> _importedMethodSpecs = new Dictionary<MethodSpecKey, int>();
        readonly Dictionary<Assembly, int> _referencedAssemblies = new Dictionary<Assembly, int>();
        int _nextPseudoToken = -1;
        int _typeCount;
        int _propertyCount;
        int _genericParameterCount;
        readonly List<int> _resolvedTokens = new List<int>();
        ISymbolWriter _symbolWriter;

        readonly List<ResourceWriterRecord> _resourceWriters = new List<ResourceWriterRecord>();
        readonly List<CustomAttributeRow> _customAttributes = new List<CustomAttributeRow>();
        readonly List<TypeRefRow> _typeRefs = new List<TypeRefRow>();
        readonly List<MemberRefRow> _memberRefs = new List<MemberRefRow>();
        readonly Dictionary<(int Class, StringHandle Name, BlobHandle Signature), int> _memberRefTokens = new Dictionary<(int, StringHandle, BlobHandle), int>();
        readonly List<MethodSpecRow> _methodSpecs = new List<MethodSpecRow>();
        readonly Dictionary<(int Method, BlobHandle Instantiation), int> _methodSpecTokens = new Dictionary<(int, BlobHandle), int>();
        readonly Dictionary<BlobHandle, int> _standAloneSignatureTokens = new Dictionary<BlobHandle, int>();
        readonly Dictionary<StringHandle, ModuleReferenceHandle> _moduleRefs = new Dictionary<StringHandle, ModuleReferenceHandle>();
        readonly Dictionary<(StringHandle Name, Version Version, StringHandle Culture, BlobHandle PublicKeyOrToken, int Flags), AssemblyReferenceHandle> _assemblyRefs = new Dictionary<(StringHandle, Version, StringHandle, BlobHandle, int), AssemblyReferenceHandle>();
        readonly List<ExportedTypeRow> _exportedTypes = new List<ExportedTypeRow>();
        readonly Dictionary<(int Implementation, StringHandle Name, StringHandle Namespace), int> _exportedTypeTokens = new Dictionary<(int, StringHandle, StringHandle), int>();
        readonly List<ManifestResourceRow> _manifestResources = new List<ManifestResourceRow>();
        bool _referencesWritten;
        readonly List<MethodSemanticsRow> _methodSemantics = new List<MethodSemanticsRow>();
        readonly List<ConstantRow> _constants = new List<ConstantRow>();
        readonly List<FieldMarshalRow> _fieldMarshals = new List<FieldMarshalRow>();
        readonly List<ImplMapRow> _implMaps = new List<ImplMapRow>();
        bool _saved;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="moduleName"></param>
        /// <param name="fileName"></param>
        /// <exception cref="NotSupportedException"></exception>
        internal ModuleBuilder(AssemblyBuilder asm, string moduleName, string fileName) :
            base(asm.Universe)
        {
            this._metadata = new MetadataBuilder();
            this._ilStream = new BlobBuilder();
            this._methodBodyEncoder = new MethodBodyStreamEncoder(_ilStream);
            this._resourceStream = new BlobBuilder();

            this._asm = asm ?? throw new ArgumentNullException(nameof(asm));
            this._metadata = _metadata ?? throw new ArgumentNullException(nameof(_metadata));
            this._moduleName = moduleName;
            this.FileName = fileName;

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
        /// Gets a new string handle from the metadata.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal StringHandle GetOrAddString(string value) => _metadata.GetOrAddString(value);

        /// <summary>
        /// Gets a new blob handle from the metadata.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal BlobHandle GetOrAddBlob(BlobBuilder value) => _metadata.GetOrAddBlob(value);

        /// <summary>
        /// Gets a new blob handle from the metadata.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal BlobHandle GetOrAddBlob(byte[] value) => _metadata.GetOrAddBlob(value);

        /// <summary>
        /// Sets the active symbol writer.
        /// </summary>
        /// <param name="writer"></param>
        internal void SetSymWriter(ISymbolWriter writer)
        {
            this._symbolWriter = writer;
        }

        internal void PopulatePropertyTables()
        {
            // LAMESPEC the PropertyMap and EventMap tables are not required to be sorted by the CLI spec,
            // but .NET sorts them and Mono requires them to be sorted, so we have to populate the
            // tables in the right order
            foreach (var type in _types)
                type.PopulatePropertyTable();
        }

        void WriteTypeDefTable()
        {
            int fieldList = 1;
            int methodList = 1;
            foreach (var type in _types)
                type.WriteTypeDefRecord(ref fieldList, ref methodList);
        }

        void WriteMethodDefTable()
        {
            int paramList = 1;
            foreach (var type in _types)
                type.WriteMethodDefRecords(ref paramList);
        }

        void WriteParamTable()
        {
            foreach (var type in _types)
                type.WriteParamRecords();
        }

        void WriteFieldTable()
        {
            foreach (var type in _types)
                type.WriteFieldRecords();
        }

        internal int AllocPseudoToken()
        {
            return _nextPseudoToken--;
        }

        /// <summary>
        /// Allocates the TypeDef token of a new type. Types are written in the order they are defined.
        /// </summary>
        /// <returns></returns>
        internal int AllocTypeToken()
        {
            return MetadataTokens.GetToken(MetadataTokens.TypeDefinitionHandle(++_typeCount));
        }

        /// <summary>
        /// Gets the number of properties written so far.
        /// </summary>
        internal int PropertyCount => _propertyCount;

        /// <summary>
        /// Writes a property definition, returning its token. Properties are written type by type while the module is
        /// being saved, which keeps each type's properties contiguous.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
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
        /// <param name="semantics"></param>
        /// <param name="methodToken"></param>
        /// <param name="association"></param>
        internal void AddMethodSemantics(short semantics, int methodToken, int association)
        {
            _methodSemantics.Add(new MethodSemanticsRow() { Semantics = semantics, Method = methodToken, Association = association });
        }

        /// <summary>
        /// Allocates the provisional index a generic parameter reports as its token until the module is written.
        /// </summary>
        /// <returns></returns>
        internal int AllocGenericParameterIndex()
        {
            return ++_genericParameterCount;
        }

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
            for (int i = 0; i < sorted.Count; i++)
            {
                var p = sorted[i];
                p.SetRow(i + 1);
                _metadata.AddGenericParameter(MetadataTokens.EntityHandle(ResolvePseudoToken(p.OwnerToken)), (System.Reflection.GenericParameterAttributes)p.GenericParameterAttributesValue, GetOrAddString(p.Name), p.Position);
            }

            for (int i = 0; i < sorted.Count; i++)
                foreach (var constraint in sorted[i].Constraints)
                    _metadata.AddGenericParameterConstraint(MetadataTokens.GenericParameterHandle(i + 1), MetadataTokens.EntityHandle(constraint));
        }

        public TypeBuilder DefineType(string name)
        {
            return DefineType(name, TypeAttributes.Class);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr)
        {
            return DefineType(name, attr, null);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent)
        {
            return DefineType(name, attr, parent, PackingSize.Unspecified, 0);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            var tb = DefineType(name, attr, parent);
            foreach (Type iface in interfaces)
                tb.AddInterfaceImplementation(iface);

            return tb;
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
        {
            string ns = null;
            int lastdot = name.LastIndexOf('.');
            if (lastdot > 0)
            {
                ns = name.Substring(0, lastdot);
                name = name.Substring(lastdot + 1);
            }

            var typeBuilder = __DefineType(ns, name);
            typeBuilder.__SetAttributes(attr);
            typeBuilder.SetParent(parent);
            if (packingSize != PackingSize.Unspecified || typesize != 0)
                typeBuilder.__SetLayout((int)packingSize, typesize);

            return typeBuilder;
        }

        public TypeBuilder __DefineType(string ns, string name)
        {
            return DefineType(this, ns, name);
        }

        internal TypeBuilder DefineType(ITypeOwner owner, string ns, string name)
        {
            var typeBuilder = new TypeBuilder(owner, ns, name);
            _types.Add(typeBuilder);
            return typeBuilder;
        }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return _moduleType.DefineMethod(name, attributes, returnType, parameterTypes);
        }

        public void CreateGlobalFunctions()
        {
            _moduleType.CreateType();
        }

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
                row.Flags = (TypeAttributes)0x00200000; // CorTypeAttr.tdForwarder
                row.Implementation = ImportAssemblyRef(type.Assembly);
            }

            var key = (row.Implementation, row.Name, row.Namespace);
            if (_exportedTypeTokens.TryGetValue(key, out var token) == false)
                _exportedTypeTokens.Add(key, token = AddExportedType(row));

            return token;
        }

        int AddExportedType(ExportedTypeRow row)
        {
            CheckReferencesNotWritten();
            _exportedTypes.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.ExportedTypeHandle(_exportedTypes.Count));
        }

        void SetTypeNameAndTypeNamespace(TypeName name, out StringHandle typeName, out StringHandle typeNamespace)
        {
            typeName = GetOrAddString(name.Name);
            typeNamespace = name.Namespace == null ? default : GetOrAddString(name.Namespace);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            SetCustomAttribute(0x00000001, customBuilder);
        }

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
        /// all of them if it is <c>null</c>.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="token"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        internal List<CustomAttributeData> GetCustomAttributes(List<CustomAttributeData> list, int token, Type attributeType)
        {
            foreach (var row in _customAttributes)
                if (row.Parent == token && (attributeType == null || attributeType.IsAssignableFrom(row.Builder.Constructor.DeclaringType)))
                    (list ??= new List<CustomAttributeData>()).Add(row.Builder.ToData(_asm));

            return list;
        }

        /// <summary>
        /// Resolves the pseudo tokens of the custom attributes and adds them to the metadata, which sorts them.
        /// </summary>
        void WriteCustomAttributes()
        {
            for (int i = 0; i < _customAttributes.Count; i++)
            {
                var row = _customAttributes[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                row.Constructor = ResolvePseudoToken(row.Constructor);
                _customAttributes[i] = row;
                _metadata.AddCustomAttribute(MetadataTokens.EntityHandle(row.Parent), MetadataTokens.EntityHandle(row.Constructor), row.Value);
            }
        }

        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
        {
            _resourceWriters.Add(new ResourceWriterRecord(name, stream, attribute));
        }

        /// <summary>
        /// Writes the defined resources to the resource blob and metadata tables.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void WriteResources()
        {
            foreach (var rwr in _resourceWriters)
            {
                rwr.Write(this);
                rwr.Close();
            }
        }

        public override Assembly Assembly
        {
            get { return _asm; }
        }

        internal override Type FindType(TypeName name)
        {
            foreach (var type in _types)
                if (type.TypeName == name)
                    return type;

            return null;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var type in _types)
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            return null;
        }

        internal override void GetTypesImpl(List<Type> list)
        {
            foreach (var type in _types)
                if (type != _moduleType)
                    list.Add(type);
        }

        public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            if (_symbolWriter != null)
                return _symbolWriter.DefineDocument(url, language, languageVendor, documentType);
            else
                throw new NotSupportedException();
        }

        public TypeToken GetTypeToken(Type type)
        {
            if (type.Module == this)
                return new TypeToken(type.GetModuleBuilderToken());
            else
                return new TypeToken(ImportType(type));
        }

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

        static bool IsFromGenericTypeDefinition(MemberInfo member)
        {
            var decl = member.DeclaringType;
            return decl != null && !decl.__IsMissing && decl.IsGenericTypeDefinition;
        }

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

        public MethodToken GetMethodToken(MethodInfo method)
        {
            var mb = method as MethodBuilder;
            if (mb != null && mb.ModuleBuilder == this)
                return new MethodToken(mb.MetadataToken);
            else
                return new MethodToken(method.ImportTo(this));
        }

        public MethodToken __GetMethodToken(MethodInfo method, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            var sig = new BlobBuilder(16);
            method.MethodSignature.WriteMethodRef(this, sig, optionalParameterTypes, customModifiers);

            var row = new MemberRefRow();
            row.Class = method.Module == this ? method.MetadataToken : GetTypeTokenForMemberRef(method.DeclaringType ?? method.Module.GetModuleType());
            row.Name = GetOrAddString(method.Name);
            row.Signature = GetOrAddBlob(sig);

            var key = (row.Class, row.Name, row.Signature);
            if (_memberRefTokens.TryGetValue(key, out var token) == false)
                _memberRefTokens.Add(key, token = AddMemberRef(row));

            return new MethodToken(token);
        }

        // when we refer to a method on a generic type definition in the IL stream,
        // we need to use a MemberRef (even if the method is in the same module)
        internal MethodToken GetMethodTokenForIL(MethodInfo method)
        {
            if (method.IsGenericMethodDefinition)
                method = method.MakeGenericMethod(method.GetGenericArguments());

            if (IsFromGenericTypeDefinition(method))
                return new MethodToken(method.ImportTo(this));
            else
                return GetMethodToken(method);
        }

        public MethodToken GetConstructorToken(ConstructorInfo constructor)
        {
            return GetMethodToken(constructor.GetMethodInfo());
        }

        internal int ImportMethodOrField(Type declaringType, string name, Signature sig)
        {
            var key = new MemberRefKey(declaringType, name, sig);
            if (!_importedMemberRefs.TryGetValue(key, out var token))
            {
                var row = new MemberRefRow();
                row.Class = GetTypeTokenForMemberRef(declaringType);
                row.Name = GetOrAddString(name);
                var bb = new BlobBuilder(16);
                sig.Write(this, bb);
                row.Signature = GetOrAddBlob(bb);
                token = AddMemberRef(row);
                _importedMemberRefs.Add(key, token);
            }

            return token;
        }

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

        int ImportAssemblyRef(Assembly asm)
        {
            if (_referencedAssemblies.TryGetValue(asm, out var token) == false)
            {
                // We can't write the AssemblyRef record here yet, because the identity of the assembly can still change
                // (if it's an AssemblyBuilder).
                token = AllocPseudoToken();
                _referencedAssemblies.Add(asm, token);
            }

            return token;
        }

        internal void FillAssemblyRefTable()
        {
            foreach (var kv in _referencedAssemblies)
                if (IsPseudoToken(kv.Value))
                    RegisterTokenFixup(kv.Value, FindOrAddAssemblyRef(kv.Key.GetName()));
        }

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
        /// <param name="signature"></param>
        /// <returns></returns>
        internal int AddTypeSpec(BlobHandle signature)
        {
            return MetadataTokens.GetToken(_metadata.AddTypeSpecification(signature));
        }

        /// <summary>
        /// Gets the stand alone signature with the given blob, adding it if needed.
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        internal StandaloneSignatureHandle GetStandAloneSignature(BlobHandle signature)
        {
            if (_standAloneSignatureTokens.TryGetValue(signature, out var token) == false)
                _standAloneSignatureTokens.Add(signature, token = MetadataTokens.GetToken(_metadata.AddStandaloneSignature(signature)));

            return MetadataTokens.StandaloneSignatureHandle(MetadataTokens.GetRowNumber(MetadataTokens.EntityHandle(token)));
        }

        /// <summary>
        /// Gets the module reference with the given name, adding it if needed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ModuleReferenceHandle GetModuleRef(string name)
        {
            var handle = name == null ? default : GetOrAddString(name);
            if (_moduleRefs.TryGetValue(handle, out var moduleRef) == false)
                _moduleRefs.Add(handle, moduleRef = _metadata.AddModuleReference(handle));

            return moduleRef;
        }

        int AddTypeRef(int resolutionScope, TypeName name)
        {
            CheckReferencesNotWritten();
            var row = new TypeRefRow() { ResolutionScope = resolutionScope };
            SetTypeNameAndTypeNamespace(name, out row.Name, out row.Namespace);
            _typeRefs.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.TypeReferenceHandle(_typeRefs.Count));
        }

        int AddMemberRef(MemberRefRow row)
        {
            CheckReferencesNotWritten();
            _memberRefs.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.MemberReferenceHandle(_memberRefs.Count));
        }

        /// <summary>
        /// Records a manifest resource.
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="name"></param>
        /// <param name="implementation"></param>
        /// <param name="offset"></param>
        internal void AddManifestResource(ResourceAttributes flags, string name, int implementation, int offset)
        {
            CheckReferencesNotWritten();
            _manifestResources.Add(new ManifestResourceRow() { Flags = flags, Name = GetOrAddString(name), Implementation = implementation, Offset = offset });
        }

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

        internal void RegisterTokenFixup(int pseudoToken, int realToken)
        {
            int index = -(pseudoToken + 1);
            while (_resolvedTokens.Count <= index)
                _resolvedTokens.Add(0);

            _resolvedTokens[index] = realToken;
        }

        internal static bool IsPseudoToken(int token)
        {
            return token < 0;
        }

        internal int ResolvePseudoToken(int pseudoToken)
        {
            return IsPseudoToken(pseudoToken) ? _resolvedTokens[-(pseudoToken + 1)] : pseudoToken;
        }

        internal void FixupMethodBodyTokens()
        {
            int methodToken = 0x06000001;
            int fieldToken = 0x04000001;
            int parameterToken = 0x08000001;

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

        internal override void ExportTypes(AssemblyFileHandle fileToken, ModuleBuilder manifestModule)
        {
            manifestModule.ExportTypes(_types.ToArray(), fileToken);
        }

        internal void ExportTypes(Type[] types, AssemblyFileHandle fileToken)
        {
            var declaringTypes = new Dictionary<Type, ExportedTypeHandle>();

            foreach (var type in types)
            {
                if (type.IsModulePseudoType == false && IsVisible(type))
                {
                    var row = new ExportedTypeRow();
                    row.Flags = type.Attributes;
                    // LAMESPEC ECMA says that TypeDefId is a row index, but it should be a token
                    row.TypeDefId = type.MetadataToken;
                    SetTypeNameAndTypeNamespace(type.TypeName, out row.Name, out row.Namespace);
                    row.Implementation = type.IsNested ? MetadataTokens.GetToken(declaringTypes[type.DeclaringType]) : MetadataTokens.GetToken(fileToken);
                    declaringTypes.Add(type, (ExportedTypeHandle)MetadataTokens.EntityHandle(AddExportedType(row)));
                }
            }
        }

        static bool IsVisible(Type type)
        {
            // NOTE this is not the same as Type.IsVisible, because that doesn't take into account family access
            return type.IsPublic || ((type.IsNestedFamily || type.IsNestedFamORAssem || type.IsNestedPublic) && IsVisible(type.DeclaringType));
        }

        /// <summary>
        /// Records the constant value of a field, parameter or property.
        /// </summary>
        /// <param name="parentToken"></param>
        /// <param name="value"></param>
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
        /// <param name="token"></param>
        /// <returns></returns>
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
        /// <param name="parentToken"></param>
        /// <param name="nativeType"></param>
        internal void AddFieldMarshal(int parentToken, byte[] nativeType)
        {
            _fieldMarshals.Add(new FieldMarshalRow() { Parent = parentToken, NativeType = nativeType });
        }

        /// <summary>
        /// Gets the marshalling descriptor recorded for the given token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="nativeType"></param>
        /// <returns></returns>
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
        /// <param name="methodToken"></param>
        /// <param name="flags"></param>
        /// <param name="importName"></param>
        /// <param name="importScope"></param>
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
        /// Resolves the pseudo tokens of the constants, marshalling descriptors and P/Invoke imports and adds them to the
        /// metadata. The metadata sorts the Constant and FieldMarshal tables, but requires ImplMap to be added in order.
        /// </summary>
        void WriteMemberData()
        {
            foreach (var row in _methodSemantics)
                _metadata.AddMethodSemantics(MetadataTokens.EntityHandle(row.Association), (System.Reflection.MethodSemanticsAttributes)row.Semantics, (MethodDefinitionHandle)MetadataTokens.EntityHandle(ResolvePseudoToken(row.Method)));

            for (int i = 0; i < _constants.Count; i++)
            {
                var row = _constants[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                _constants[i] = row;
                _metadata.AddConstant(MetadataTokens.EntityHandle(row.Parent), row.Value);
            }

            for (int i = 0; i < _fieldMarshals.Count; i++)
            {
                var row = _fieldMarshals[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                _fieldMarshals[i] = row;
                _metadata.AddMarshallingDescriptor(MetadataTokens.EntityHandle(row.Parent), _metadata.GetOrAddBlob(row.NativeType));
            }

            for (int i = 0; i < _implMaps.Count; i++)
            {
                var row = _implMaps[i];
                row.Method = ResolvePseudoToken(row.Method);
                _implMaps[i] = row;
            }

            _implMaps.Sort((x, y) => x.Method.CompareTo(y.Method));
            foreach (var row in _implMaps)
                _metadata.AddMethodImport((MethodDefinitionHandle)MetadataTokens.EntityHandle(row.Method), (System.Reflection.MethodImportAttributes)row.Flags, GetOrAddString(row.ImportName), row.ImportScopeHandle);
        }

        ModuleBuilder ITypeOwner.ModuleBuilder
        {
            get { return this; }
        }

        internal override Type ResolveType(int metadataToken, IGenericContext context)
        {
            if (metadataToken >> 24 != (int)TableIndex.TypeDef)
            {
                throw new NotImplementedException();
            }
            return _types[(metadataToken & 0xFFFFFF) - 1];
        }

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

        public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw new NotImplementedException();
        }

        public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw new NotImplementedException();
        }

        public override string ResolveString(int metadataToken)
        {
            throw new NotImplementedException();
        }

        public override string FullyQualifiedName => Path.GetFullPath(Path.Combine(_asm.Dir, FileName));

        public override string Name => FileName;

        internal ReservedBlob<GuidHandle> GetModuleVersionIdFixup()
        {
            return _mvidFixup;
        }

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
        /// <param name="value"></param>
        internal void SetModuleVersionId(Guid value) => _mvid = value;

        public override string ScopeName
        {
            get { return _moduleName; }
        }

        /// <summary>
        /// Returns the symbol writer associated with this dynamic module.
        /// </summary>
        /// <returns></returns>
        public ISymbolWriter GetSymWriter()
        {
            return _symbolWriter;
        }

        public StringToken GetStringConstant(string str)
        {
            return new StringToken(MetadataTokens.GetHeapOffset(_metadata.GetOrAddUserString(str)) | (0x70 << 24));
        }

        public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
        {
            return new SignatureToken(MetadataTokens.GetToken(GetStandAloneSignature(GetOrAddBlob(sigHelper.GetSignature(this)))));
        }

        internal override Type GetModuleType()
        {
            return _moduleType;
        }

        internal BlobHandle GetSignatureBlobIndex(Signature sig)
        {
            var bb = new BlobBuilder(16);
            sig.Write(this, bb);
            return GetOrAddBlob(bb);
        }

        // non-standard API
        public new ulong __ImageBase
        {
            get { return _imageBaseAddress; }
            set { _imageBaseAddress = value; }
        }

        public new uint __FileAlignment
        {
            get { return _fileAlignment; }
            set { _fileAlignment = value; }
        }

        public new DllCharacteristics __DllCharacteristics
        {
            get { return _dllCharacteristics; }
            set { _dllCharacteristics = value; }
        }

        public override int MDStreamVersion
        {
            get { return _asm.MdStreamVersion; }
        }

        int AddTypeRefByName(int resolutionScope, string ns, string name)
        {
            return AddTypeRef(resolutionScope, new TypeName(ns, name));
        }

        /// <summary>
        /// Saves the module.
        /// </summary>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        public void __Save(PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            __Save(null, null, portableExecutableKind, imageFileMachine);
        }

        /// <summary>
        /// Saves the module and it's debug information to the specified streams.
        /// </summary>
        /// <param name="peStream"></param>
        /// <param name="pdbStream"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        public void __Save(Stream peStream, Stream pdbStream, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            SaveImpl(peStream, pdbStream, portableExecutableKind, imageFileMachine);
        }

        /// <summary>
        /// Implements the Save functionality.
        /// </summary>
        /// <param name="peStream"></param>
        /// <param name="pdbStream"></param>
        /// <param name="portableExecutableKind"></param>
        /// <param name="imageFileMachine"></param>
        /// <exception cref="ArgumentException"></exception>
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
            ModuleWriter.WriteModule(null, null, this, PEFileKinds.Dll, portableExecutableKind, imageFileMachine, NativeResources, default, null, peStream, null, pdbStream);
        }

        public override Type[] __GetExportedTypes()
        {
            throw new NotImplementedException();
        }

        public int __AddModule(int flags, string name, byte[] hash)
        {
            const int ContainsNoMetaData = 0x0001;
            return MetadataTokens.GetToken(_metadata.AddAssemblyFile(GetOrAddString(name), GetOrAddBlob(hash), (flags & ContainsNoMetaData) == 0));
        }

        internal void SetIsSaved()
        {
            if (_saved)
                throw new InvalidOperationException();

            _saved = true;
        }

        internal bool IsSaved
        {
            get { return _saved; }
        }

    }

}
