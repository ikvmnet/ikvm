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
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    internal sealed class ModuleBuilder : Module, ITypeOwner
    {

        struct ResourceWriterRecord
        {

            readonly string name;
            readonly ResourceWriter rw;
            readonly Stream stream;
            readonly ResourceAttributes attributes;

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
                this.name = name;
                this.rw = rw;
                this.stream = stream;
                this.attributes = attributes;
            }

            /// <summary>
            /// Writes the resource to the resource stream.
            /// </summary>
            /// <param name="module"></param>
            internal readonly void Write(ModuleBuilder module)
            {
                // write resource to internal stream
                rw?.Generate();

                // align the start of the resource
                module.resourceStream.Align(8);
                var offset = module.resourceStream.Count;

                // resource begins with a length, followed by the data
                module.resourceStream.WriteInt32((int)stream.Length);
                stream.Position = 0;
                var buffer = new byte[8192];
                int length;
                while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    module.resourceStream.WriteBytes(buffer, 0, length);

                module.AddManifestResource(attributes, name, 0, offset);
            }

            internal readonly void Close()
            {
                rw?.Close();
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

            readonly Type type;
            readonly string name;
            readonly Signature signature;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            /// <param name="signature"></param>
            internal MemberRefKey(Type type, string name, Signature signature)
            {
                this.type = type;
                this.name = name;
                this.signature = signature;
            }

            public bool Equals(MemberRefKey other) => other.type.Equals(type) && other.name == name && other.signature.Equals(signature);

            public override bool Equals(object obj) => obj is MemberRefKey other && Equals(other);

            public override int GetHashCode()
            {
                return type.GetHashCode() + name.GetHashCode() + signature.GetHashCode();
            }

            internal MethodBase LookupMethod()
            {
                return type.FindMethod(name, (MethodSignature)signature);
            }

        }

        readonly struct MethodSpecKey : IEquatable<MethodSpecKey>
        {

            readonly Type type;
            readonly string name;
            readonly MethodSignature signature;
            readonly Type[] genericParameters;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            /// <param name="signature"></param>
            /// <param name="genericParameters"></param>
            internal MethodSpecKey(Type type, string name, MethodSignature signature, Type[] genericParameters)
            {
                this.type = type;
                this.name = name;
                this.signature = signature;
                this.genericParameters = genericParameters;
            }

            public bool Equals(MethodSpecKey other)
            {
                return other.type.Equals(type) && other.name == name && other.signature.Equals(signature) && Util.ArrayEquals(other.genericParameters, genericParameters);
            }

            public override bool Equals(object obj) => obj is MethodSpecKey other && Equals(other);

            public override int GetHashCode() => type.GetHashCode() + name.GetHashCode() + signature.GetHashCode() + Util.GetHashCode(genericParameters);

        }


        readonly MetadataBuilder metadata;
        readonly BlobBuilder ilStream;
        readonly MethodBodyStreamEncoder methodBodyEncoder;
        readonly BlobBuilder resourceStream;
        readonly AssemblyBuilder asm;
        Guid mvid;
        ReservedBlob<GuidHandle> mvidFixup;
        ulong imageBaseAddress = 0;
        uint fileAlignment = 0;
        DllCharacteristics dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NoSEH | DllCharacteristics.NXCompat | DllCharacteristics.TerminalServerAware;
        internal readonly string moduleName;
        internal readonly string fileName;
        readonly TypeBuilder moduleType;
        readonly List<TypeBuilder> types = new List<TypeBuilder>();
        readonly Dictionary<Type, int> typeTokens = new Dictionary<Type, int>();
        readonly Dictionary<Type, int> memberRefTypeTokens = new Dictionary<Type, int>();
        internal ModuleResourceSectionBuilder nativeResources;
        readonly Dictionary<MemberRefKey, int> importedMemberRefs = new Dictionary<MemberRefKey, int>();
        readonly Dictionary<MethodSpecKey, int> importedMethodSpecs = new Dictionary<MethodSpecKey, int>();
        readonly Dictionary<Assembly, int> referencedAssemblies = new Dictionary<Assembly, int>();
        int nextPseudoToken = -1;
        int typeCount;
        int propertyCount;
        int genericParameterCount;
        readonly List<int> resolvedTokens = new List<int>();
        ISymbolWriter symbolWriter;

        readonly List<ResourceWriterRecord> resourceWriters = new List<ResourceWriterRecord>();
        readonly List<CustomAttributeRow> customAttributes = new List<CustomAttributeRow>();
        readonly List<TypeRefRow> typeRefs = new List<TypeRefRow>();
        readonly List<MemberRefRow> memberRefs = new List<MemberRefRow>();
        readonly Dictionary<(int Class, StringHandle Name, BlobHandle Signature), int> memberRefTokens = new Dictionary<(int, StringHandle, BlobHandle), int>();
        readonly List<MethodSpecRow> methodSpecs = new List<MethodSpecRow>();
        readonly Dictionary<(int Method, BlobHandle Instantiation), int> methodSpecTokens = new Dictionary<(int, BlobHandle), int>();
        readonly Dictionary<BlobHandle, int> standAloneSignatureTokens = new Dictionary<BlobHandle, int>();
        readonly Dictionary<StringHandle, ModuleReferenceHandle> moduleRefs = new Dictionary<StringHandle, ModuleReferenceHandle>();
        readonly Dictionary<(StringHandle Name, Version Version, StringHandle Culture, BlobHandle PublicKeyOrToken, int Flags), AssemblyReferenceHandle> assemblyRefs = new Dictionary<(StringHandle, Version, StringHandle, BlobHandle, int), AssemblyReferenceHandle>();
        readonly List<ExportedTypeRow> exportedTypes = new List<ExportedTypeRow>();
        readonly Dictionary<(int Implementation, StringHandle Name, StringHandle Namespace), int> exportedTypeTokens = new Dictionary<(int, StringHandle, StringHandle), int>();
        readonly List<ManifestResourceRow> manifestResources = new List<ManifestResourceRow>();
        bool referencesWritten;
        readonly List<MethodSemanticsRow> methodSemantics = new List<MethodSemanticsRow>();
        readonly List<ConstantRow> constants = new List<ConstantRow>();
        readonly List<FieldMarshalRow> fieldMarshals = new List<FieldMarshalRow>();
        readonly List<ImplMapRow> implMaps = new List<ImplMapRow>();
        bool saved;

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
            this.metadata = new MetadataBuilder();
            this.ilStream = new BlobBuilder();
            this.methodBodyEncoder = new MethodBodyStreamEncoder(ilStream);
            this.resourceStream = new BlobBuilder();

            this.asm = asm ?? throw new ArgumentNullException(nameof(asm));
            this.metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            this.moduleName = moduleName;
            this.fileName = fileName;

            // the image gets its real module version id when it is written; until then report a provisional one
            if (Universe.Deterministic == false)
                mvid = Guid.NewGuid();

            // add module
            mvidFixup = metadata.ReserveGuid();
            metadata.AddModule(0, GetOrAddString(moduleName), mvidFixup.Handle, default, default);

            // <Module> must be the first record in the TypeDef table
            moduleType = new TypeBuilder(this, null, "<Module>");
            types.Add(moduleType);
        }

        /// <summary>
        /// Gets a reference to the <see cref="MetadataBuilder"/> underlying this instance.
        /// </summary>
        internal MetadataBuilder Metadata => metadata;

        /// <summary>
        /// Gets a reference to the <see cref="BlobBuilder"/> for the IL stream.
        /// </summary>
        internal BlobBuilder ILStream => ilStream;

        /// <summary>
        /// Gets a reference to the <see cref="MethodBodyStreamEncoder"/> for the IL stream.
        /// </summary>
        internal MethodBodyStreamEncoder MethodBodyEncoder => methodBodyEncoder;

        /// <summary>
        /// Gets a reference to the <see cref="BlobBuilder"/> for the resource stream.
        /// </summary>
        internal BlobBuilder ResourceStream => resourceStream;

        /// <summary>
        /// Gets a new string handle from the metadata.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal StringHandle GetOrAddString(string value) => metadata.GetOrAddString(value);

        /// <summary>
        /// Gets a new blob handle from the metadata.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal BlobHandle GetOrAddBlob(BlobBuilder value) => metadata.GetOrAddBlob(value);

        /// <summary>
        /// Gets a new blob handle from the metadata.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal BlobHandle GetOrAddBlob(byte[] value) => metadata.GetOrAddBlob(value);

        /// <summary>
        /// Sets the active symbol writer.
        /// </summary>
        /// <param name="writer"></param>
        internal void SetSymWriter(ISymbolWriter writer)
        {
            this.symbolWriter = writer;
        }

        internal void PopulatePropertyTables()
        {
            // LAMESPEC the PropertyMap and EventMap tables are not required to be sorted by the CLI spec,
            // but .NET sorts them and Mono requires them to be sorted, so we have to populate the
            // tables in the right order
            foreach (var type in types)
                type.PopulatePropertyTable();
        }

        void WriteTypeDefTable()
        {
            int fieldList = 1;
            int methodList = 1;
            foreach (var type in types)
                type.WriteTypeDefRecord(ref fieldList, ref methodList);
        }

        void WriteMethodDefTable()
        {
            int paramList = 1;
            foreach (var type in types)
                type.WriteMethodDefRecords(ref paramList);
        }

        void WriteParamTable()
        {
            foreach (var type in types)
                type.WriteParamRecords();
        }

        void WriteFieldTable()
        {
            foreach (var type in types)
                type.WriteFieldRecords();
        }

        internal int AllocPseudoToken()
        {
            return nextPseudoToken--;
        }

        /// <summary>
        /// Allocates the TypeDef token of a new type. Types are written in the order they are defined.
        /// </summary>
        /// <returns></returns>
        internal int AllocTypeToken()
        {
            return MetadataTokens.GetToken(MetadataTokens.TypeDefinitionHandle(++typeCount));
        }

        /// <summary>
        /// Gets the number of properties written so far.
        /// </summary>
        internal int PropertyCount => propertyCount;

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
            var h = metadata.AddProperty((System.Reflection.PropertyAttributes)attributes, GetOrAddString(name), GetSignatureBlobIndex(signature));
            Debug.Assert(MetadataTokens.GetRowNumber(h) == propertyCount + 1);
            propertyCount++;
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
            methodSemantics.Add(new MethodSemanticsRow() { Semantics = semantics, Method = methodToken, Association = association });
        }

        /// <summary>
        /// Allocates the provisional index a generic parameter reports as its token until the module is written.
        /// </summary>
        /// <returns></returns>
        internal int AllocGenericParameterIndex()
        {
            return ++genericParameterCount;
        }

        /// <summary>
        /// Writes the tables that describe the structure of the types: layouts, interface implementations, method
        /// overrides, nesting, field layouts, generic parameters and their constraints, all of which the metadata
        /// requires to be added in sorted order.
        /// </summary>
        void WriteTypeStructure()
        {
            foreach (var type in types)
                type.WriteStructure();

            // generic parameters are sorted by their owner, a TypeOrMethodDef coded index, and then by number
            var parameters = new List<GenericTypeParameterBuilder>();
            foreach (var type in types)
                type.CollectGenericParameters(parameters);

            var keys = new Dictionary<GenericTypeParameterBuilder, int>(parameters.Count);
            foreach (var p in parameters)
                keys[p] = GenericParamTable.EncodeOwner(ResolvePseudoToken(p.OwnerToken));

            var sorted = parameters.OrderBy(p => keys[p]).ThenBy(p => p.Position).ToList();
            for (int i = 0; i < sorted.Count; i++)
            {
                var p = sorted[i];
                p.SetRow(i + 1);
                metadata.AddGenericParameter(MetadataTokens.EntityHandle(ResolvePseudoToken(p.OwnerToken)), (System.Reflection.GenericParameterAttributes)p.GenericParameterAttributesValue, GetOrAddString(p.Name), p.Position);
            }

            for (int i = 0; i < sorted.Count; i++)
                foreach (var constraint in sorted[i].Constraints)
                    metadata.AddGenericParameterConstraint(MetadataTokens.GenericParameterHandle(i + 1), MetadataTokens.EntityHandle(constraint));
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
            types.Add(typeBuilder);
            return typeBuilder;
        }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return moduleType.DefineMethod(name, attributes, returnType, parameterTypes);
        }

        public void CreateGlobalFunctions()
        {
            moduleType.CreateType();
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
            if (asm.ImageRuntimeVersion == "v2.0.50727")
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
            if (exportedTypeTokens.TryGetValue(key, out var token) == false)
                exportedTypeTokens.Add(key, token = AddExportedType(row));

            return token;
        }

        int AddExportedType(ExportedTypeRow row)
        {
            CheckReferencesNotWritten();
            exportedTypes.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.ExportedTypeHandle(exportedTypes.Count));
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
            customAttributes.Add(new CustomAttributeRow()
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
            foreach (var row in customAttributes)
                if (row.Parent == token && (attributeType == null || attributeType.IsAssignableFrom(row.Builder.Constructor.DeclaringType)))
                    (list ??= new List<CustomAttributeData>()).Add(row.Builder.ToData(asm));

            return list;
        }

        /// <summary>
        /// Resolves the pseudo tokens of the custom attributes and adds them to the metadata, which sorts them.
        /// </summary>
        void WriteCustomAttributes()
        {
            for (int i = 0; i < customAttributes.Count; i++)
            {
                var row = customAttributes[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                row.Constructor = ResolvePseudoToken(row.Constructor);
                customAttributes[i] = row;
                metadata.AddCustomAttribute(MetadataTokens.EntityHandle(row.Parent), MetadataTokens.EntityHandle(row.Constructor), row.Value);
            }
        }

        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
        {
            resourceWriters.Add(new ResourceWriterRecord(name, stream, attribute));
        }

        /// <summary>
        /// Writes the defined resources to the resource blob and metadata tables.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void WriteResources()
        {
            foreach (var rwr in resourceWriters)
            {
                rwr.Write(this);
                rwr.Close();
            }
        }

        public override Assembly Assembly
        {
            get { return asm; }
        }

        internal override Type FindType(TypeName name)
        {
            foreach (var type in types)
                if (type.TypeName == name)
                    return type;

            return null;
        }

        internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var type in types)
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            return null;
        }

        internal override void GetTypesImpl(List<Type> list)
        {
            foreach (var type in types)
                if (type != moduleType)
                    list.Add(type);
        }

        public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            if (symbolWriter != null)
                return symbolWriter.DefineDocument(url, language, languageVendor, documentType);
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
                if (memberRefTypeTokens.TryGetValue(type, out var token) == false)
                {
                    var spec = new BlobBuilder(5);
                    Signature.WriteTypeSpec(this, spec, type);
                    token = AddTypeSpec(GetOrAddBlob(spec));
                    memberRefTypeTokens.Add(type, token);
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
            if (memberRefTokens.TryGetValue(key, out var token) == false)
                memberRefTokens.Add(key, token = AddMemberRef(row));

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
            if (!importedMemberRefs.TryGetValue(key, out var token))
            {
                var row = new MemberRefRow();
                row.Class = GetTypeTokenForMemberRef(declaringType);
                row.Name = GetOrAddString(name);
                var bb = new BlobBuilder(16);
                sig.Write(this, bb);
                row.Signature = GetOrAddBlob(bb);
                token = AddMemberRef(row);
                importedMemberRefs.Add(key, token);
            }

            return token;
        }

        internal int ImportMethodSpec(Type declaringType, MethodInfo method, Type[] genericParameters)
        {

            var key = new MethodSpecKey(declaringType, method.Name, method.MethodSignature, genericParameters);
            if (!importedMethodSpecs.TryGetValue(key, out var token))
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
                if (methodSpecTokens.TryGetValue(rowKey, out token) == false)
                {
                    CheckReferencesNotWritten();
                    methodSpecs.Add(row);
                    methodSpecTokens.Add(rowKey, token = MetadataTokens.GetToken(MetadataTokens.MethodSpecificationHandle(methodSpecs.Count)));
                }

                importedMethodSpecs.Add(key, token);
            }

            return token;
        }

        internal int ImportType(Type type)
        {
            if (typeTokens.TryGetValue(type, out var token) == false)
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

                typeTokens.Add(type, token);
            }

            return token;
        }

        int ImportAssemblyRef(Assembly asm)
        {
            if (referencedAssemblies.TryGetValue(asm, out var token) == false)
            {
                // We can't write the AssemblyRef record here yet, because the identity of the assembly can still change
                // (if it's an AssemblyBuilder).
                token = AllocPseudoToken();
                referencedAssemblies.Add(asm, token);
            }

            return token;
        }

        internal void FillAssemblyRefTable()
        {
            foreach (var kv in referencedAssemblies)
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
            if (assemblyRefs.TryGetValue(key, out var handle) == false)
                assemblyRefs.Add(key, handle = metadata.AddAssemblyReference(nameHandle, version, culture, publicKeyOrToken, (System.Reflection.AssemblyFlags)flags, name.hash != null ? GetOrAddBlob(name.hash) : default));

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
            return MetadataTokens.GetToken(metadata.AddTypeSpecification(signature));
        }

        /// <summary>
        /// Gets the stand alone signature with the given blob, adding it if needed.
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        internal StandaloneSignatureHandle GetStandAloneSignature(BlobHandle signature)
        {
            if (standAloneSignatureTokens.TryGetValue(signature, out var token) == false)
                standAloneSignatureTokens.Add(signature, token = MetadataTokens.GetToken(metadata.AddStandaloneSignature(signature)));

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
            if (moduleRefs.TryGetValue(handle, out var moduleRef) == false)
                moduleRefs.Add(handle, moduleRef = metadata.AddModuleReference(handle));

            return moduleRef;
        }

        int AddTypeRef(int resolutionScope, TypeName name)
        {
            CheckReferencesNotWritten();
            var row = new TypeRefRow() { ResolutionScope = resolutionScope };
            SetTypeNameAndTypeNamespace(name, out row.Name, out row.Namespace);
            typeRefs.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.TypeReferenceHandle(typeRefs.Count));
        }

        int AddMemberRef(MemberRefRow row)
        {
            CheckReferencesNotWritten();
            memberRefs.Add(row);
            return MetadataTokens.GetToken(MetadataTokens.MemberReferenceHandle(memberRefs.Count));
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
            manifestResources.Add(new ManifestResourceRow() { Flags = flags, Name = GetOrAddString(name), Implementation = implementation, Offset = offset });
        }

        void CheckReferencesNotWritten()
        {
            if (referencesWritten)
                throw new InvalidOperationException("A reference was added after the references of the module were written.");
        }

        /// <summary>
        /// Resolves the pseudo tokens of the type, member and method references, exported types and manifest resources and
        /// adds them to the metadata, in the order they were created so their tokens stay valid.
        /// </summary>
        void WriteReferences()
        {
            referencesWritten = true;

            foreach (var row in typeRefs)
                metadata.AddTypeReference(MetadataTokens.EntityHandle(ResolvePseudoToken(row.ResolutionScope)), row.Namespace, row.Name);

            foreach (var row in memberRefs)
                metadata.AddMemberReference(MetadataTokens.EntityHandle(ResolvePseudoToken(row.Class)), row.Name, row.Signature);

            foreach (var row in methodSpecs)
                metadata.AddMethodSpecification(MetadataTokens.EntityHandle(ResolvePseudoToken(row.Method)), row.Instantiation);

            foreach (var row in exportedTypes)
                metadata.AddExportedType((System.Reflection.TypeAttributes)row.Flags, row.Namespace, row.Name, MetadataTokens.EntityHandle(ResolvePseudoToken(row.Implementation)), row.TypeDefId);

            foreach (var row in manifestResources)
                metadata.AddManifestResource((System.Reflection.ManifestResourceAttributes)row.Flags, row.Name, MetadataTokens.EntityHandle(ResolvePseudoToken(row.Implementation)), (uint)row.Offset);
        }

        internal void RegisterTokenFixup(int pseudoToken, int realToken)
        {
            int index = -(pseudoToken + 1);
            while (resolvedTokens.Count <= index)
                resolvedTokens.Add(0);

            resolvedTokens[index] = realToken;
        }

        internal static bool IsPseudoToken(int token)
        {
            return token < 0;
        }

        internal int ResolvePseudoToken(int pseudoToken)
        {
            return IsPseudoToken(pseudoToken) ? resolvedTokens[-(pseudoToken + 1)] : pseudoToken;
        }

        internal void FixupMethodBodyTokens()
        {
            int methodToken = 0x06000001;
            int fieldToken = 0x04000001;
            int parameterToken = 0x08000001;

            foreach (var type in types)
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
            manifestModule.ExportTypes(types.ToArray(), fileToken);
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

            constants.Add(new ConstantRow() { Parent = parentToken, Value = value });
        }

        /// <summary>
        /// Gets the constant value recorded for the given token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">No constant was recorded.</exception>
        internal object GetConstant(int token)
        {
            foreach (var row in constants)
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
            fieldMarshals.Add(new FieldMarshalRow() { Parent = parentToken, NativeType = nativeType });
        }

        /// <summary>
        /// Gets the marshalling descriptor recorded for the given token.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        internal bool TryGetFieldMarshal(int token, out byte[] nativeType)
        {
            foreach (var row in fieldMarshals)
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
            implMaps.Add(new ImplMapRow()
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
            foreach (var row in implMaps)
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
            foreach (var row in methodSemantics)
                metadata.AddMethodSemantics(MetadataTokens.EntityHandle(row.Association), (System.Reflection.MethodSemanticsAttributes)row.Semantics, (MethodDefinitionHandle)MetadataTokens.EntityHandle(ResolvePseudoToken(row.Method)));

            for (int i = 0; i < constants.Count; i++)
            {
                var row = constants[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                constants[i] = row;
                metadata.AddConstant(MetadataTokens.EntityHandle(row.Parent), row.Value);
            }

            for (int i = 0; i < fieldMarshals.Count; i++)
            {
                var row = fieldMarshals[i];
                row.Parent = ResolvePseudoToken(row.Parent);
                fieldMarshals[i] = row;
                metadata.AddMarshallingDescriptor(MetadataTokens.EntityHandle(row.Parent), metadata.GetOrAddBlob(row.NativeType));
            }

            for (int i = 0; i < implMaps.Count; i++)
            {
                var row = implMaps[i];
                row.Method = ResolvePseudoToken(row.Method);
                implMaps[i] = row;
            }

            implMaps.Sort((x, y) => x.Method.CompareTo(y.Method));
            foreach (var row in implMaps)
                metadata.AddMethodImport((MethodDefinitionHandle)MetadataTokens.EntityHandle(row.Method), (System.Reflection.MethodImportAttributes)row.Flags, GetOrAddString(row.ImportName), row.ImportScopeHandle);
        }

        ModuleBuilder ITypeOwner.ModuleBuilder
        {
            get { return this; }
        }

        internal override Type ResolveType(int metadataToken, IGenericContext context)
        {
            if (metadataToken >> 24 != TypeDefTable.Index)
            {
                throw new NotImplementedException();
            }
            return types[(metadataToken & 0xFFFFFF) - 1];
        }

        public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            if (genericTypeArguments != null || genericMethodArguments != null)
            {
                throw new NotImplementedException();
            }
            // this method is inefficient, but since it isn't used we don't care
            if ((metadataToken >> 24) == MemberRefTable.Index)
            {
                foreach (KeyValuePair<MemberRefKey, int> kv in importedMemberRefs)
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
            foreach (Type type in types)
            {
                MethodBase method = ((TypeBuilder)type).LookupMethod(metadataToken);
                if (method != null)
                {
                    return method;
                }
            }
            return ((TypeBuilder)moduleType).LookupMethod(metadataToken);
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

        public override string FullyQualifiedName => Path.GetFullPath(Path.Combine(asm.dir, fileName));

        public override string Name => fileName;

        internal ReservedBlob<GuidHandle> GetModuleVersionIdFixup()
        {
            return mvidFixup;
        }

        public override Guid ModuleVersionId
        {
            get
            {
                // if a deterministic GUID is used, it can't be queried before the assembly has been written
                if (mvid == Guid.Empty && Universe.Deterministic)
                    throw new InvalidOperationException();

                return mvid;
            }
        }

        /// <summary>
        /// Records the module version id the image was written with.
        /// </summary>
        /// <param name="value"></param>
        internal void SetModuleVersionId(Guid value) => mvid = value;

        public override string ScopeName
        {
            get { return moduleName; }
        }

        /// <summary>
        /// Returns the symbol writer associated with this dynamic module.
        /// </summary>
        /// <returns></returns>
        public ISymbolWriter GetSymWriter()
        {
            return symbolWriter;
        }

        public StringToken GetStringConstant(string str)
        {
            return new StringToken(MetadataTokens.GetHeapOffset(metadata.GetOrAddUserString(str)) | (0x70 << 24));
        }

        public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
        {
            return new SignatureToken(MetadataTokens.GetToken(GetStandAloneSignature(GetOrAddBlob(sigHelper.GetSignature(this)))));
        }

        internal override Type GetModuleType()
        {
            return moduleType;
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
            get { return imageBaseAddress; }
            set { imageBaseAddress = value; }
        }

        public new uint __FileAlignment
        {
            get { return fileAlignment; }
            set { fileAlignment = value; }
        }

        public new DllCharacteristics __DllCharacteristics
        {
            get { return dllCharacteristics; }
            set { dllCharacteristics = value; }
        }

        public override int MDStreamVersion
        {
            get { return asm.mdStreamVersion; }
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

            var attributes = asm.GetCustomAttributesData(null);
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
            ModuleWriter.WriteModule(null, null, this, PEFileKinds.Dll, portableExecutableKind, imageFileMachine, nativeResources, default, null, peStream, null, pdbStream);
        }

        public override Type[] __GetExportedTypes()
        {
            throw new NotImplementedException();
        }

        public int __AddModule(int flags, string name, byte[] hash)
        {
            const int ContainsNoMetaData = 0x0001;
            return MetadataTokens.GetToken(metadata.AddAssemblyFile(GetOrAddString(name), GetOrAddBlob(hash), (flags & ContainsNoMetaData) == 0));
        }

        internal void FixupPseudoToken(ref int token)
        {
            if (IsPseudoToken(token))
                token = ResolvePseudoToken(token);
        }

        internal void SetIsSaved()
        {
            if (saved)
                throw new InvalidOperationException();

            saved = true;
        }

        internal bool IsSaved
        {
            get { return saved; }
        }

    }

}
