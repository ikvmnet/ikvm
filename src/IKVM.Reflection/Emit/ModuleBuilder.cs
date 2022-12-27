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
using System.Resources;
using System.Runtime.InteropServices;

using IKVM.Reflection.Impl;
using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public sealed class ModuleBuilder : Module, ITypeOwner
    {

        static readonly bool usePublicKeyAssemblyReference = false;

        Guid mvid;
        uint timestamp;
        long imageBaseAddress = 0x00400000;
        long stackReserve = -1;
        int fileAlignment = 0x200;
        DllCharacteristics dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NoSEH | DllCharacteristics.NXCompat | DllCharacteristics.TerminalServerAware;
        readonly AssemblyBuilder asm;
        internal readonly string moduleName;
        internal readonly string fileName;
        internal readonly ISymbolWriterImpl symbolWriter;
        readonly TypeBuilder moduleType;
        readonly List<TypeBuilder> types = new List<TypeBuilder>();
        readonly Dictionary<Type, int> typeTokens = new Dictionary<Type, int>();
        readonly Dictionary<Type, int> memberRefTypeTokens = new Dictionary<Type, int>();
        internal readonly ByteBuffer methodBodies = new ByteBuffer(128 * 1024);
        internal readonly List<int> tokenFixupOffsets = new List<int>();
        internal readonly ByteBuffer initializedData = new ByteBuffer(512);
        internal ResourceSection unmanagedResources;
        readonly Dictionary<MemberRefKey, int> importedMemberRefs = new Dictionary<MemberRefKey, int>();
        readonly Dictionary<MethodSpecKey, int> importedMethodSpecs = new Dictionary<MethodSpecKey, int>();
        readonly Dictionary<Assembly, int> referencedAssemblies = new Dictionary<Assembly, int>();
        List<AssemblyName> referencedAssemblyNames;
        int nextPseudoToken = -1;
        readonly List<int> resolvedTokens = new List<int>();
        internal readonly TableHeap Tables = new TableHeap();
        internal readonly StringHeap Strings = new StringHeap();
        internal readonly UserStringHeap UserStrings = new UserStringHeap();
        internal readonly GuidHeap Guids = new GuidHeap();
        internal readonly BlobHeap Blobs = new BlobHeap();
        internal readonly List<VTableFixups> vtablefixups = new List<VTableFixups>();
        internal readonly List<UnmanagedExport> unmanagedExports = new List<UnmanagedExport>();
        List<InterfaceImplCustomAttribute> interfaceImplCustomAttributes;
        readonly List<ResourceWriterRecord> resourceWriters = new List<ResourceWriterRecord>();
        bool saved;

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

            internal void Emit(ModuleBuilder mb, int offset)
            {
                if (rw != null)
                    rw.Generate();

                var rec = new ManifestResourceTable.Record();
                rec.Offset = offset;
                rec.Flags = (int)attributes;
                rec.Name = mb.Strings.Add(name);
                rec.Implementation = 0;
                mb.ManifestResource.AddRecord(rec);
            }

            internal int GetLength()
            {
                return 4 + (int)stream.Length;
            }

            internal void Write(MetadataWriter mw)
            {
                mw.Write((int)stream.Length);
                stream.Position = 0;
                byte[] buffer = new byte[8192];
                int length;
                while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    mw.Write(buffer, 0, length);
            }

            internal void Close()
            {
                if (rw != null)
                    rw.Close();
            }

        }

        internal struct VTableFixups
        {

            internal uint initializedDataOffset;
            internal ushort count;
            internal ushort type;

            internal int SlotWidth
            {
                get { return (type & 0x02) == 0 ? 4 : 8; }
            }

        }

        struct InterfaceImplCustomAttribute
        {

            internal int type;
            internal int interfaceType;
            internal int pseudoToken;

        }

        struct MemberRefKey : IEquatable<MemberRefKey>
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

            public bool Equals(MemberRefKey other)
            {
                return other.type.Equals(type)
                    && other.name == name
                    && other.signature.Equals(signature);
            }

            public override bool Equals(object obj)
            {
                MemberRefKey? other = obj as MemberRefKey?;
                return other != null && Equals(other.Value);
            }

            public override int GetHashCode()
            {
                return type.GetHashCode() + name.GetHashCode() + signature.GetHashCode();
            }

            internal MethodBase LookupMethod()
            {
                return type.FindMethod(name, (MethodSignature)signature);
            }

        }

        struct MethodSpecKey : IEquatable<MethodSpecKey>
        {

            readonly Type type;
            readonly string name;
            readonly MethodSignature signature;
            readonly Type[] genericParameters;

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

            public override bool Equals(object obj)
            {
                var other = obj as MethodSpecKey?;
                return other != null && Equals(other.Value);
            }

            public override int GetHashCode()
            {
                return type.GetHashCode() + name.GetHashCode() + signature.GetHashCode() + Util.GetHashCode(genericParameters);
            }

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="moduleName"></param>
        /// <param name="fileName"></param>
        /// <param name="emitSymbolInfo"></param>
        /// <exception cref="NotSupportedException"></exception>
        internal ModuleBuilder(AssemblyBuilder asm, string moduleName, string fileName, bool emitSymbolInfo) : base(asm.universe)
        {
            this.asm = asm;
            this.moduleName = moduleName;
            this.fileName = fileName;
            if (emitSymbolInfo)
            {
                symbolWriter = SymbolSupport.CreateSymbolWriterFor(this);
                if (universe.Deterministic && !symbolWriter.IsDeterministic)
                    throw new NotSupportedException();
            }

            if (!universe.Deterministic)
            {
                __PEHeaderTimeDateStamp = DateTime.UtcNow;
                mvid = Guid.NewGuid();
            }

            // <Module> must be the first record in the TypeDef table
            moduleType = new TypeBuilder(this, null, "<Module>");
            types.Add(moduleType);
        }

        internal void PopulatePropertyAndEventTables()
        {
            // LAMESPEC the PropertyMap and EventMap tables are not required to be sorted by the CLI spec,
            // but .NET sorts them and Mono requires them to be sorted, so we have to populate the
            // tables in the right order
            foreach (var type in types)
                type.PopulatePropertyAndEventTables();
        }

        internal void WriteTypeDefTable(MetadataWriter mw)
        {
            int fieldList = 1;
            int methodList = 1;
            foreach (var type in types)
                type.WriteTypeDefRecord(mw, ref fieldList, ref methodList);
        }

        internal void WriteMethodDefTable(int baseRVA, MetadataWriter mw)
        {
            int paramList = 1;
            foreach (var type in types)
                type.WriteMethodDefRecords(baseRVA, mw, ref paramList);
        }

        internal void WriteParamTable(MetadataWriter mw)
        {
            foreach (var type in types)
                type.WriteParamRecords(mw);
        }

        internal void WriteFieldTable(MetadataWriter mw)
        {
            foreach (var type in types)
                type.WriteFieldRecords(mw);
        }

        internal int AllocPseudoToken()
        {
            return nextPseudoToken--;
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

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize)
        {
            return DefineType(name, attr, parent, PackingSize.Unspecified, typesize);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize)
        {
            return DefineType(name, attr, parent, packsize, 0);
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

        public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
        {
            var tb = DefineType(name, (visibility & TypeAttributes.VisibilityMask) | TypeAttributes.Sealed, universe.System_Enum);
            var fb = tb.DefineField("value__", underlyingType, FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
            return new EnumBuilder(tb, fb);
        }

        public FieldBuilder __DefineField(string name, Type type, CustomModifiers customModifiers, FieldAttributes attributes)
        {
            return moduleType.__DefineField(name, type, customModifiers, attributes);
        }

        [Obsolete("Please use __DefineField(string, Type, CustomModifiers, FieldAttributes) instead.")]
        public FieldBuilder __DefineField(string name, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
        {
            return moduleType.DefineField(name, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
        }

        public ConstructorBuilder __DefineModuleInitializer(MethodAttributes visibility)
        {
            return moduleType.DefineConstructor(visibility | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, Type.EmptyTypes);
        }

        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
        {
            return moduleType.DefineUninitializedData(name, size, attributes);
        }

        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
        {
            return moduleType.DefineInitializedData(name, data, attributes);
        }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
        {
            return moduleType.DefineMethod(name, attributes, returnType, parameterTypes);
        }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            return moduleType.DefineMethod(name, attributes, callingConvention, returnType, parameterTypes);
        }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            return moduleType.DefineMethod(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return moduleType.DefinePInvokeMethod(name, dllName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            return moduleType.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
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
                foreach (Type nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic))
                {
                    // we export all nested types (i.e. even the private ones)
                    // (this behavior is the same as the C# compiler)
                    AddTypeForwarder(nested, true);
                }
            }
        }

        int ExportType(Type type)
        {
            var rec = new ExportedTypeTable.Record();
            if (asm.ImageRuntimeVersion == "v2.0.50727")
            {
                // HACK we should *not* set the TypeDefId in this case, but 2.0 and 3.5 peverify gives a warning if it is missing (4.5 doesn't)
                rec.TypeDefId = type.MetadataToken;
            }

            SetTypeNameAndTypeNamespace(type.TypeName, out rec.TypeName, out rec.TypeNamespace);
            if (type.IsNested)
            {
                rec.Flags = 0;
                rec.Implementation = ExportType(type.DeclaringType);
            }
            else
            {
                rec.Flags = 0x00200000; // CorTypeAttr.tdForwarder
                rec.Implementation = ImportAssemblyRef(type.Assembly);
            }
            return 0x27000000 | this.ExportedType.FindOrAddRecord(rec);
        }

        void SetTypeNameAndTypeNamespace(TypeName name, out int typeName, out int typeNamespace)
        {
            typeName = this.Strings.Add(name.Name);
            typeNamespace = name.Namespace == null ? 0 : this.Strings.Add(name.Namespace);
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            SetCustomAttribute(0x00000001, customBuilder);
        }

        internal void SetCustomAttribute(int token, CustomAttributeBuilder customBuilder)
        {
            var rec = new CustomAttributeTable.Record();
            rec.Parent = token;
            rec.Type = asm.IsWindowsRuntime ? customBuilder.Constructor.ImportTo(this) : GetConstructorToken(customBuilder.Constructor).Token;
            rec.Value = customBuilder.WriteBlob(this);
            CustomAttribute.AddRecord(rec);
        }

        private void AddDeclSecurityRecord(int token, int action, int blob)
        {
            var rec = new DeclSecurityTable.Record();
            rec.Action = (short)action;
            rec.Parent = token;
            rec.PermissionSet = blob;
            DeclSecurity.AddRecord(rec);
        }

        internal void AddDeclarativeSecurity(int token, System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
        {
            AddDeclSecurityRecord(token, (int)securityAction, Blobs.Add(ByteBuffer.Wrap(System.Text.Encoding.Unicode.GetBytes(permissionSet.ToXml().ToString()))));
        }

        internal void AddDeclarativeSecurity(int token, List<CustomAttributeBuilder> declarativeSecurity)
        {
            var ordered = new Dictionary<int, List<CustomAttributeBuilder>>();
            foreach (var cab in declarativeSecurity)
            {
                int action;
                // check for HostProtectionAttribute without SecurityAction
                if (cab.ConstructorArgumentCount == 0)
                {
                    action = (int)System.Security.Permissions.SecurityAction.LinkDemand;
                }
                else
                {
                    action = (int)cab.GetConstructorArgument(0);
                }

                if (cab.IsLegacyDeclSecurity)
                {
                    AddDeclSecurityRecord(token, action, cab.WriteLegacyDeclSecurityBlob(this));
                    continue;
                }

                if (!ordered.TryGetValue(action, out var list))
                {
                    list = new List<CustomAttributeBuilder>();
                    ordered.Add(action, list);
                }

                list.Add(cab);
            }

            foreach (KeyValuePair<int, List<CustomAttributeBuilder>> kv in ordered)
                AddDeclSecurityRecord(token, kv.Key, WriteDeclSecurityBlob(kv.Value));
        }

        int WriteDeclSecurityBlob(List<CustomAttributeBuilder> list)
        {
            ByteBuffer namedArgs = new ByteBuffer(100);
            ByteBuffer bb = new ByteBuffer(list.Count * 100);
            bb.Write((byte)'.');
            bb.WriteCompressedUInt(list.Count);
            foreach (CustomAttributeBuilder cab in list)
            {
                bb.Write(cab.Constructor.DeclaringType.AssemblyQualifiedName);
                namedArgs.Clear();
                cab.WriteNamedArgumentsForDeclSecurity(this, namedArgs);
                bb.WriteCompressedUInt(namedArgs.Length);
                bb.Write(namedArgs);
            }
            return this.Blobs.Add(bb);
        }

        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
        {
            resourceWriters.Add(new ResourceWriterRecord(name, stream, attribute));
        }

        public IResourceWriter DefineResource(string name, string description)
        {
            return DefineResource(name, description, ResourceAttributes.Public);
        }

        public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
        {
            // FXBUG we ignore the description, because there is no such thing
            var mem = new MemoryStream();
            var rw = new ResourceWriter(mem);
            resourceWriters.Add(new ResourceWriterRecord(name, rw, mem, attribute));
            return rw;
        }

        internal void EmitResources()
        {
            int offset = 0;
            foreach (var rwr in resourceWriters)
            {
                // resources must be 8-byte aligned
                offset = (offset + 7) & ~7;
                rwr.Emit(this, offset);
                offset += rwr.GetLength();
            }
        }

        internal void WriteResources(MetadataWriter mw)
        {
            int offset = 0;
            foreach (var rwr in resourceWriters)
            {
                // resources must be 8-byte aligned
                int alignment = ((offset + 7) & ~7) - offset;
                for (int i = 0; i < alignment; i++)
                    mw.Write((byte)0);
                rwr.Write(mw);
                offset += rwr.GetLength() + alignment;
            }
        }

        internal void CloseResources()
        {
            foreach (var rwr in resourceWriters)
                rwr.Close();
        }

        internal int GetManifestResourcesLength()
        {
            int length = 0;
            foreach (var rwr in resourceWriters)
            {
                // resources must be 8-byte aligned
                length = (length + 7) & ~7;
                length += rwr.GetLength();
            }

            return length;
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
            return symbolWriter.DefineDocument(url, language, languageVendor, documentType);
        }

        public int __GetAssemblyToken(Assembly assembly)
        {
            return ImportAssemblyRef(assembly);
        }

        public TypeToken GetTypeToken(string name)
        {
            return new TypeToken(GetType(name, true, false).MetadataToken);
        }

        public TypeToken GetTypeToken(Type type)
        {
            if (type.Module == this && !asm.IsWindowsRuntime)
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
                if (!memberRefTypeTokens.TryGetValue(type, out var token))
                {
                    var spec = new ByteBuffer(5);
                    Signature.WriteTypeSpec(this, spec, type);
                    token = 0x1B000000 | this.TypeSpec.AddRecord(this.Blobs.Add(spec));
                    memberRefTypeTokens.Add(type, token);
                }
                return token;
            }
            else if (type.IsModulePseudoType)
            {
                return 0x1A000000 | this.ModuleRef.FindOrAddRecord(this.Strings.Add(type.Module.ScopeName));
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

        public MethodToken GetMethodToken(MethodInfo method, IEnumerable<Type> optionalParameterTypes)
        {
            return __GetMethodToken(method, Util.ToArray(optionalParameterTypes), null);
        }

        public MethodToken __GetMethodToken(MethodInfo method, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            var sig = new ByteBuffer(16);
            method.MethodSignature.WriteMethodRefSig(this, sig, optionalParameterTypes, customModifiers);

            var record = new MemberRefTable.Record();
            record.Class = method.Module == this ? method.MetadataToken : GetTypeTokenForMemberRef(method.DeclaringType ?? method.Module.GetModuleType());
            record.Name = Strings.Add(method.Name);
            record.Signature = Blobs.Add(sig);
            return new MethodToken(0x0A000000 | MemberRef.FindOrAddRecord(record));
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

        internal int GetMethodTokenWinRT(MethodInfo method)
        {
            return asm.IsWindowsRuntime ? method.ImportTo(this) : GetMethodToken(method).Token;
        }

        public MethodToken GetConstructorToken(ConstructorInfo constructor)
        {
            return GetMethodToken(constructor.GetMethodInfo());
        }

        public MethodToken GetConstructorToken(ConstructorInfo constructor, IEnumerable<Type> optionalParameterTypes)
        {
            return GetMethodToken(constructor.GetMethodInfo(), optionalParameterTypes);
        }

        public MethodToken __GetConstructorToken(ConstructorInfo constructor, Type[] optionalParameterTypes, CustomModifiers[] customModifiers)
        {
            return __GetMethodToken(constructor.GetMethodInfo(), optionalParameterTypes, customModifiers);
        }

        internal int ImportMethodOrField(Type declaringType, string name, Signature sig)
        {
            var key = new MemberRefKey(declaringType, name, sig);
            if (!importedMemberRefs.TryGetValue(key, out var token))
            {
                var rec = new MemberRefTable.Record();
                rec.Class = GetTypeTokenForMemberRef(declaringType);
                rec.Name = Strings.Add(name);
                var bb = new ByteBuffer(16);
                sig.WriteSig(this, bb);
                rec.Signature = Blobs.Add(bb);
                token = 0x0A000000 | MemberRef.AddRecord(rec);
                importedMemberRefs.Add(key, token);
            }

            return token;
        }

        internal int ImportMethodSpec(Type declaringType, MethodInfo method, Type[] genericParameters)
        {
            Debug.Assert(method.__IsMissing || method.GetMethodOnTypeDefinition() == method);

            var key = new MethodSpecKey(declaringType, method.Name, method.MethodSignature, genericParameters);
            if (!importedMethodSpecs.TryGetValue(key, out var token))
            {
                var rec = new MethodSpecTable.Record();
                var mb = method as MethodBuilder;
                if (mb != null && mb.ModuleBuilder == this && !declaringType.IsGenericType)
                {
                    rec.Method = mb.MetadataToken;
                }
                else
                {
                    // we're calling ImportMethodOrField directly here, because 'method' may be a MethodDef on a generic TypeDef and 'declaringType' the type instance
                    // (in order words the method and type have already been decoupled by the caller)
                    rec.Method = ImportMethodOrField(declaringType, method.Name, method.MethodSignature);
                }
                var spec = new ByteBuffer(10);
                Signature.WriteMethodSpec(this, spec, genericParameters);
                rec.Instantiation = Blobs.Add(spec);
                token = 0x2B000000 | MethodSpec.FindOrAddRecord(rec);
                importedMethodSpecs.Add(key, token);
            }

            return token;
        }

        internal int ImportType(Type type)
        {
            int token;
            if (!typeTokens.TryGetValue(type, out token))
            {
                if (type.HasElementType || type.IsConstructedGenericType || type.__IsFunctionPointer)
                {
                    var spec = new ByteBuffer(5);
                    Signature.WriteTypeSpec(this, spec, type);
                    token = 0x1B000000 | this.TypeSpec.AddRecord(this.Blobs.Add(spec));
                }
                else
                {
                    var rec = new TypeRefTable.Record();
                    if (type.IsNested)
                        rec.ResolutionScope = GetTypeToken(type.DeclaringType).Token;
                    else if (type.Module == this)
                        rec.ResolutionScope = 1;
                    else
                        rec.ResolutionScope = ImportAssemblyRef(type.Assembly);

                    SetTypeNameAndTypeNamespace(type.TypeName, out rec.TypeName, out rec.TypeNamespace);
                    token = 0x01000000 | this.TypeRef.AddRecord(rec);
                }
                typeTokens.Add(type, token);
            }

            return token;
        }

        int ImportAssemblyRef(Assembly asm)
        {
            if (!referencedAssemblies.TryGetValue(asm, out var token))
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
                    RegisterTokenFixup(kv.Value, FindOrAddAssemblyRef(kv.Key.GetName(), false));
        }

        private int FindOrAddAssemblyRef(AssemblyName name, bool alwaysAdd)
        {
            var rec = new AssemblyRefTable.Record();
            var ver = name.Version ?? new Version(0, 0, 0, 0);
            rec.MajorVersion = (ushort)ver.Major;
            rec.MinorVersion = (ushort)ver.Minor;
            rec.BuildNumber = (ushort)ver.Build;
            rec.RevisionNumber = (ushort)ver.Revision;
            rec.Flags = (int)(name.Flags & ~AssemblyNameFlags.PublicKey);
            const AssemblyNameFlags afPA_Specified = (AssemblyNameFlags)0x0080;
            const AssemblyNameFlags afPA_Mask = (AssemblyNameFlags)0x0070;
            if ((name.RawFlags & afPA_Specified) != 0)
            {
                rec.Flags |= (int)(name.RawFlags & afPA_Mask);
            }
            if (name.ContentType == AssemblyContentType.WindowsRuntime)
            {
                rec.Flags |= 0x0200;
            }
            byte[] publicKeyOrToken = null;
            if (usePublicKeyAssemblyReference)
            {
                publicKeyOrToken = name.GetPublicKey();
            }
            if (publicKeyOrToken == null || publicKeyOrToken.Length == 0)
            {
                publicKeyOrToken = name.GetPublicKeyToken() ?? Empty<byte>.Array;
            }
            else
            {
                const int PublicKey = 0x0001;
                rec.Flags |= PublicKey;
            }
            rec.PublicKeyOrToken = this.Blobs.Add(ByteBuffer.Wrap(publicKeyOrToken));
            rec.Name = this.Strings.Add(name.Name);
            rec.Culture = name.CultureName == null ? 0 : this.Strings.Add(name.CultureName);
            if (name.hash != null)
            {
                rec.HashValue = this.Blobs.Add(ByteBuffer.Wrap(name.hash));
            }
            else
            {
                rec.HashValue = 0;
            }

            return 0x23000000 | (alwaysAdd ? this.AssemblyRef.AddRecord(rec) : this.AssemblyRef.FindOrAddRecord(rec));
        }

        internal void WriteSymbolTokenMap()
        {
            for (int i = 0; i < resolvedTokens.Count; i++)
            {
                int newToken = resolvedTokens[i];
                // The symbol API doesn't support remapping arbitrary integers, the types have to be the same,
                // so we copy the type from the newToken, because our pseudo tokens don't have a type.
                // (see MethodToken.SymbolToken)
                int oldToken = (i + 1) | (newToken & ~0xFFFFFF);
                SymbolSupport.RemapToken(symbolWriter, oldToken, newToken);
            }
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
            int index = -(pseudoToken + 1);
            return resolvedTokens[index];
        }

        internal void ApplyUnmanagedExports(ImageFileMachine imageFileMachine)
        {
            if (unmanagedExports.Count != 0)
            {
                int type;
                int size;
                switch (imageFileMachine)
                {
                    case ImageFileMachine.I386:
                    case ImageFileMachine.ARM:
                        type = 0x05;
                        size = 4;
                        break;
                    case ImageFileMachine.AMD64:
                        type = 0x06;
                        size = 8;
                        break;
                    default:
                        throw new NotSupportedException();
                }

                var methods = new List<MethodBuilder>();
                for (int i = 0; i < unmanagedExports.Count; i++)
                    if (unmanagedExports[i].mb != null)
                        methods.Add(unmanagedExports[i].mb);

                if (methods.Count != 0)
                {
                    var rva = __AddVTableFixups(methods.ToArray(), type);
                    for (int i = 0; i < unmanagedExports.Count; i++)
                    {
                        if (unmanagedExports[i].mb != null)
                        {
                            var exp = unmanagedExports[i];
                            exp.rva = new RelativeVirtualAddress(rva.initializedDataOffset + (uint)(methods.IndexOf(unmanagedExports[i].mb) * size));
                            unmanagedExports[i] = exp;
                        }
                    }
                }
            }
        }

        internal void FixupMethodBodyTokens()
        {
            int methodToken = 0x06000001;
            int fieldToken = 0x04000001;
            int parameterToken = 0x08000001;
            foreach (TypeBuilder type in types)
            {
                type.ResolveMethodAndFieldTokens(ref methodToken, ref fieldToken, ref parameterToken);
            }
            foreach (int offset in tokenFixupOffsets)
            {
                methodBodies.Position = offset;
                int pseudoToken = methodBodies.GetInt32AtCurrentPosition();
                methodBodies.Write(ResolvePseudoToken(pseudoToken));
            }
            foreach (VTableFixups fixup in vtablefixups)
            {
                for (int i = 0; i < fixup.count; i++)
                {
                    initializedData.Position = (int)fixup.initializedDataOffset + i * fixup.SlotWidth;
                    initializedData.Write(ResolvePseudoToken(initializedData.GetInt32AtCurrentPosition()));
                }
            }
        }

        int GetHeaderLength()
        {
            return
                4 + // Signature
                2 + // MajorVersion
                2 + // MinorVersion
                4 + // Reserved
                4 + // ImageRuntimeVersion Length
                StringToPaddedUTF8Length(asm.ImageRuntimeVersion) +
                2 + // Flags
                2 + // Streams
                4 + // #~ Offset
                4 + // #~ Size
                4 + // StringToPaddedUTF8Length("#~")
                4 + // #Strings Offset
                4 + // #Strings Size
                12 + // StringToPaddedUTF8Length("#Strings")
                4 + // #US Offset
                4 + // #US Size
                4 + // StringToPaddedUTF8Length("#US")
                4 + // #GUID Offset
                4 + // #GUID Size
                8 + // StringToPaddedUTF8Length("#GUID")
                (Blobs.IsEmpty ? 0 :
                (
                4 + // #Blob Offset
                4 + // #Blob Size
                8   // StringToPaddedUTF8Length("#Blob")
                ));
        }

        internal int MetadataLength
        {
            get
            {
                return GetHeaderLength() + (Blobs.IsEmpty ? 0 : Blobs.Length) + Tables.Length + Strings.Length + UserStrings.Length + Guids.Length;
            }
        }

        internal void WriteMetadata(MetadataWriter mw, out uint guidHeapOffset)
        {
            mw.Write(0x424A5342);           // Signature ("BSJB")
            mw.Write((ushort)1);            // MajorVersion
            mw.Write((ushort)1);            // MinorVersion
            mw.Write(0);                    // Reserved
            byte[] version = StringToPaddedUTF8(asm.ImageRuntimeVersion);
            mw.Write(version.Length);       // Length
            mw.Write(version);
            mw.Write((ushort)0);            // Flags
                                            // #Blob is the only optional heap
            if (Blobs.IsEmpty)
            {
                mw.Write((ushort)4);        // Streams
            }
            else
            {
                mw.Write((ushort)5);        // Streams
            }

            int offset = GetHeaderLength();

            // Streams
            mw.Write(offset);               // Offset
            mw.Write(Tables.Length);        // Size
            mw.Write(StringToPaddedUTF8("#~"));
            offset += Tables.Length;

            mw.Write(offset);               // Offset
            mw.Write(Strings.Length);       // Size
            mw.Write(StringToPaddedUTF8("#Strings"));
            offset += Strings.Length;

            mw.Write(offset);               // Offset
            mw.Write(UserStrings.Length);   // Size
            mw.Write(StringToPaddedUTF8("#US"));
            offset += UserStrings.Length;

            mw.Write(offset);               // Offset
            mw.Write(Guids.Length);         // Size
            mw.Write(StringToPaddedUTF8("#GUID"));
            offset += Guids.Length;

            if (!Blobs.IsEmpty)
            {
                mw.Write(offset);               // Offset
                mw.Write(Blobs.Length);         // Size
                mw.Write(StringToPaddedUTF8("#Blob"));
            }

            Tables.Write(mw);
            Strings.Write(mw);
            UserStrings.Write(mw);
            guidHeapOffset = mw.Position;
            Guids.Write(mw);
            if (!Blobs.IsEmpty)
            {
                Blobs.Write(mw);
            }
        }

        static int StringToPaddedUTF8Length(string str)
        {
            return (System.Text.Encoding.UTF8.GetByteCount(str) + 4) & ~3;
        }

        static byte[] StringToPaddedUTF8(string str)
        {
            byte[] buf = new byte[(System.Text.Encoding.UTF8.GetByteCount(str) + 4) & ~3];
            System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, buf, 0);
            return buf;
        }

        internal override void ExportTypes(int fileToken, ModuleBuilder manifestModule)
        {
            manifestModule.ExportTypes(types.ToArray(), fileToken);
        }

        internal void ExportTypes(Type[] types, int fileToken)
        {
            Dictionary<Type, int> declaringTypes = new Dictionary<Type, int>();
            foreach (Type type in types)
            {
                if (!type.IsModulePseudoType && IsVisible(type))
                {
                    ExportedTypeTable.Record rec = new ExportedTypeTable.Record();
                    rec.Flags = (int)type.Attributes;
                    // LAMESPEC ECMA says that TypeDefId is a row index, but it should be a token
                    rec.TypeDefId = type.MetadataToken;
                    SetTypeNameAndTypeNamespace(type.TypeName, out rec.TypeName, out rec.TypeNamespace);
                    if (type.IsNested)
                    {
                        rec.Implementation = declaringTypes[type.DeclaringType];
                    }
                    else
                    {
                        rec.Implementation = fileToken;
                    }
                    int exportTypeToken = 0x27000000 | this.ExportedType.AddRecord(rec);
                    declaringTypes.Add(type, exportTypeToken);
                }
            }
        }

        private static bool IsVisible(Type type)
        {
            // NOTE this is not the same as Type.IsVisible, because that doesn't take into account family access
            return type.IsPublic || ((type.IsNestedFamily || type.IsNestedFamORAssem || type.IsNestedPublic) && IsVisible(type.DeclaringType));
        }

        internal void AddConstant(int parentToken, object defaultValue)
        {
            ConstantTable.Record rec = new ConstantTable.Record();
            rec.Parent = parentToken;
            ByteBuffer val = new ByteBuffer(16);
            if (defaultValue == null)
            {
                rec.Type = Signature.ELEMENT_TYPE_CLASS;
                val.Write((int)0);
            }
            else if (defaultValue is bool)
            {
                rec.Type = Signature.ELEMENT_TYPE_BOOLEAN;
                val.Write((bool)defaultValue ? (byte)1 : (byte)0);
            }
            else if (defaultValue is char)
            {
                rec.Type = Signature.ELEMENT_TYPE_CHAR;
                val.Write((char)defaultValue);
            }
            else if (defaultValue is sbyte)
            {
                rec.Type = Signature.ELEMENT_TYPE_I1;
                val.Write((sbyte)defaultValue);
            }
            else if (defaultValue is byte)
            {
                rec.Type = Signature.ELEMENT_TYPE_U1;
                val.Write((byte)defaultValue);
            }
            else if (defaultValue is short)
            {
                rec.Type = Signature.ELEMENT_TYPE_I2;
                val.Write((short)defaultValue);
            }
            else if (defaultValue is ushort)
            {
                rec.Type = Signature.ELEMENT_TYPE_U2;
                val.Write((ushort)defaultValue);
            }
            else if (defaultValue is int)
            {
                rec.Type = Signature.ELEMENT_TYPE_I4;
                val.Write((int)defaultValue);
            }
            else if (defaultValue is uint)
            {
                rec.Type = Signature.ELEMENT_TYPE_U4;
                val.Write((uint)defaultValue);
            }
            else if (defaultValue is long)
            {
                rec.Type = Signature.ELEMENT_TYPE_I8;
                val.Write((long)defaultValue);
            }
            else if (defaultValue is ulong)
            {
                rec.Type = Signature.ELEMENT_TYPE_U8;
                val.Write((ulong)defaultValue);
            }
            else if (defaultValue is float)
            {
                rec.Type = Signature.ELEMENT_TYPE_R4;
                val.Write((float)defaultValue);
            }
            else if (defaultValue is double)
            {
                rec.Type = Signature.ELEMENT_TYPE_R8;
                val.Write((double)defaultValue);
            }
            else if (defaultValue is string)
            {
                rec.Type = Signature.ELEMENT_TYPE_STRING;
                foreach (char c in (string)defaultValue)
                {
                    val.Write(c);
                }
            }
            else if (defaultValue is DateTime)
            {
                rec.Type = Signature.ELEMENT_TYPE_I8;
                val.Write(((DateTime)defaultValue).Ticks);
            }
            else
            {
                throw new ArgumentException();
            }
            rec.Value = this.Blobs.Add(val);
            this.Constant.AddRecord(rec);
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

        public override string FullyQualifiedName
        {
            get { return Path.GetFullPath(Path.Combine(asm.dir, fileName)); }
        }

        public override string Name
        {
            get { return fileName; }
        }

        internal Guid GetModuleVersionIdOrEmpty()
        {
            return mvid;
        }

        public override Guid ModuleVersionId
        {
            get
            {
                if (mvid == Guid.Empty && universe.Deterministic)
                {
                    // if a deterministic GUID is used, it can't be queried before the assembly has been written
                    throw new InvalidOperationException();
                }
                return mvid;
            }
        }

        public void __SetModuleVersionId(Guid guid)
        {
            if (guid == Guid.Empty && universe.Deterministic)
            {
                // if you want to use Guid.Empty, don't set UniverseOptions.DeterministicOutput
                throw new ArgumentOutOfRangeException();
            }
            mvid = guid;
        }

        internal uint GetTimeDateStamp()
        {
            return timestamp;
        }

        public DateTime __PEHeaderTimeDateStamp
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp); }
            set
            {
                if (value < new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) || value > new DateTime(2106, 2, 7, 6, 28, 15, DateTimeKind.Utc))
                {
                    throw new ArgumentOutOfRangeException();
                }
                timestamp = (uint)(value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
        }

        public override Type[] __ResolveOptionalParameterTypes(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments, out CustomModifiers[] customModifiers)
        {
            throw new NotImplementedException();
        }

        public override string ScopeName
        {
            get { return moduleName; }
        }

        public ISymbolWriter GetSymWriter()
        {
            return symbolWriter;
        }

        public void DefineUnmanagedResource(string resourceFileName)
        {
            // This method reads the specified resource file (Win32 .res file) and converts it into the appropriate format and embeds it in the .rsrc section,
            // also setting the Resource Directory entry.
            unmanagedResources = new ResourceSection();
            unmanagedResources.ExtractResources(System.IO.File.ReadAllBytes(resourceFileName));
        }

        public bool IsTransient()
        {
            return false;
        }

        public void SetUserEntryPoint(MethodInfo entryPoint)
        {
            int token = entryPoint.MetadataToken;
            if (token < 0)
            {
                token = -token | 0x06000000;
            }
            if (symbolWriter != null)
            {
                symbolWriter.SetUserEntryPoint(new SymbolToken(token));
            }
        }

        public StringToken GetStringConstant(string str)
        {
            return new StringToken(this.UserStrings.Add(str) | (0x70 << 24));
        }

        public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
        {
            return new SignatureToken(this.StandAloneSig.FindOrAddRecord(this.Blobs.Add(sigHelper.GetSignature(this))) | (StandAloneSigTable.Index << 24));
        }

        public SignatureToken GetSignatureToken(byte[] sigBytes, int sigLength)
        {
            return new SignatureToken(this.StandAloneSig.FindOrAddRecord(this.Blobs.Add(ByteBuffer.Wrap(sigBytes, sigLength))) | (StandAloneSigTable.Index << 24));
        }

        public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            return new ArrayMethod(this, arrayClass, methodName, callingConvention, returnType, parameterTypes);
        }

        public MethodToken GetArrayMethodToken(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            return GetMethodToken(GetArrayMethod(arrayClass, methodName, callingConvention, returnType, parameterTypes));
        }

        internal override Type GetModuleType()
        {
            return moduleType;
        }

        internal override IKVM.Reflection.Reader.ByteReader GetBlob(int blobIndex)
        {
            return Blobs.GetBlob(blobIndex);
        }

        internal int GetSignatureBlobIndex(Signature sig)
        {
            ByteBuffer bb = new ByteBuffer(16);
            sig.WriteSig(this, bb);
            return this.Blobs.Add(bb);
        }

        // non-standard API
        public new long __ImageBase
        {
            get { return imageBaseAddress; }
            set { imageBaseAddress = value; }
        }

        protected override long GetImageBaseImpl()
        {
            return imageBaseAddress;
        }

        public new long __StackReserve
        {
            get { return stackReserve; }
            set { stackReserve = value; }
        }

        protected override long GetStackReserveImpl()
        {
            return stackReserve;
        }

        [Obsolete("Use __StackReserve property.")]
        public void __SetStackReserve(long stackReserve)
        {
            __StackReserve = stackReserve;
        }

        internal ulong GetStackReserve(ulong defaultValue)
        {
            return stackReserve == -1 ? defaultValue : (ulong)stackReserve;
        }

        public new int __FileAlignment
        {
            get { return fileAlignment; }
            set { fileAlignment = value; }
        }

        protected override int GetFileAlignmentImpl()
        {
            return fileAlignment;
        }

        public new DllCharacteristics __DllCharacteristics
        {
            get { return dllCharacteristics; }
            set { dllCharacteristics = value; }
        }

        protected override DllCharacteristics GetDllCharacteristicsImpl()
        {
            return dllCharacteristics;
        }

        public override int MDStreamVersion
        {
            get { return asm.mdStreamVersion; }
        }

        private int AddTypeRefByName(int resolutionScope, string ns, string name)
        {
            TypeRefTable.Record rec = new TypeRefTable.Record();
            rec.ResolutionScope = resolutionScope;
            SetTypeNameAndTypeNamespace(new TypeName(ns, name), out rec.TypeName, out rec.TypeNamespace);
            return 0x01000000 | this.TypeRef.AddRecord(rec);
        }

        public void __Save(PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            SaveImpl(null, portableExecutableKind, imageFileMachine);
        }

        public void __Save(Stream stream, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek || stream.Position != 0)
            {
                throw new ArgumentException("Stream must support read/write/seek and current position must be zero.", "stream");
            }
            SaveImpl(stream, portableExecutableKind, imageFileMachine);
        }

        private void SaveImpl(Stream streamOrNull, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            SetIsSaved();
            PopulatePropertyAndEventTables();
            IList<CustomAttributeData> attributes = asm.GetCustomAttributesData(null);
            if (attributes.Count > 0)
            {
                int mscorlib = ImportAssemblyRef(universe.CoreLib);
                int[] placeholderTokens = new int[4];
                string[] placeholderTypeNames = new string[] { "AssemblyAttributesGoHere", "AssemblyAttributesGoHereM", "AssemblyAttributesGoHereS", "AssemblyAttributesGoHereSM" };
                foreach (CustomAttributeData cad in attributes)
                {
                    int index;
                    if (cad.Constructor.DeclaringType.BaseType == universe.System_Security_Permissions_CodeAccessSecurityAttribute)
                    {
                        if (cad.Constructor.DeclaringType.IsAllowMultipleCustomAttribute)
                        {
                            index = 3;
                        }
                        else
                        {
                            index = 2;
                        }
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
            EmitResources();
            ModuleWriter.WriteModule(null, null, this, PEFileKinds.Dll, portableExecutableKind, imageFileMachine, unmanagedResources, 0, streamOrNull);
            CloseResources();
        }

        public void __AddAssemblyReference(AssemblyName assemblyName)
        {
            __AddAssemblyReference(assemblyName, null);
        }

        public void __AddAssemblyReference(AssemblyName assemblyName, Assembly assembly)
        {
            if (referencedAssemblyNames == null)
            {
                referencedAssemblyNames = new List<AssemblyName>();
            }
            referencedAssemblyNames.Add((AssemblyName)assemblyName.Clone());
            int token = FindOrAddAssemblyRef(assemblyName, true);
            if (assembly != null)
            {
                referencedAssemblies.Add(assembly, token);
            }
        }

        public override AssemblyName[] __GetReferencedAssemblies()
        {
            List<AssemblyName> list = new List<AssemblyName>();
            if (referencedAssemblyNames != null)
            {
                foreach (AssemblyName name in referencedAssemblyNames)
                {
                    if (!list.Contains(name))
                    {
                        list.Add(name);
                    }
                }
            }
            foreach (Assembly asm in referencedAssemblies.Keys)
            {
                AssemblyName name = asm.GetName();
                if (!list.Contains(name))
                {
                    list.Add(name);
                }
            }
            return list.ToArray();
        }

        public void __AddModuleReference(string module)
        {
            this.ModuleRef.FindOrAddRecord(module == null ? 0 : this.Strings.Add(module));
        }

        public override string[] __GetReferencedModules()
        {
            string[] arr = new string[this.ModuleRef.RowCount];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = this.Strings.Find(this.ModuleRef.records[i]);
            }
            return arr;
        }

        public override Type[] __GetReferencedTypes()
        {
            List<Type> list = new List<Type>();
            foreach (KeyValuePair<Type, int> kv in typeTokens)
            {
                if (kv.Value >> 24 == TypeRefTable.Index)
                {
                    list.Add(kv.Key);
                }
            }
            return list.ToArray();
        }

        public override Type[] __GetExportedTypes()
        {
            throw new NotImplementedException();
        }

        public int __AddModule(int flags, string name, byte[] hash)
        {
            FileTable.Record file = new FileTable.Record();
            file.Flags = flags;
            file.Name = this.Strings.Add(name);
            file.HashValue = this.Blobs.Add(ByteBuffer.Wrap(hash));
            return 0x26000000 + this.File.AddRecord(file);
        }

        public int __AddManifestResource(int offset, ResourceAttributes flags, string name, int implementation)
        {
            ManifestResourceTable.Record res = new ManifestResourceTable.Record();
            res.Offset = offset;
            res.Flags = (int)flags;
            res.Name = this.Strings.Add(name);
            res.Implementation = implementation;
            return 0x28000000 + this.ManifestResource.AddRecord(res);
        }

        public void __SetCustomAttributeFor(int token, CustomAttributeBuilder customBuilder)
        {
            SetCustomAttribute(token, customBuilder);
        }

        public RelativeVirtualAddress __AddVTableFixups(MethodBuilder[] methods, int type)
        {
            initializedData.Align(8);
            VTableFixups fixups;
            fixups.initializedDataOffset = (uint)initializedData.Position;
            fixups.count = (ushort)methods.Length;
            fixups.type = (ushort)type;
            foreach (MethodBuilder mb in methods)
            {
                initializedData.Write(mb.MetadataToken);
                if (fixups.SlotWidth == 8)
                {
                    initializedData.Write(0);
                }
            }
            vtablefixups.Add(fixups);
            return new RelativeVirtualAddress(fixups.initializedDataOffset);
        }

        public void __AddUnmanagedExportStub(string name, int ordinal, RelativeVirtualAddress rva)
        {
            AddUnmanagedExport(name, ordinal, null, rva);
        }

        internal void AddUnmanagedExport(string name, int ordinal, MethodBuilder methodBuilder, RelativeVirtualAddress rva)
        {
            UnmanagedExport export;
            export.name = name;
            export.ordinal = ordinal;
            export.mb = methodBuilder;
            export.rva = rva;
            unmanagedExports.Add(export);
        }

        internal void SetInterfaceImplementationCustomAttribute(TypeBuilder typeBuilder, Type interfaceType, CustomAttributeBuilder cab)
        {
            // NOTE since interfaceimpls are extremely common and custom attributes on them are extremely rare,
            // we store (and resolve) the custom attributes in such away as to avoid impacting the common case performance
            if (interfaceImplCustomAttributes == null)
            {
                interfaceImplCustomAttributes = new List<InterfaceImplCustomAttribute>();
            }
            InterfaceImplCustomAttribute rec;
            rec.type = typeBuilder.MetadataToken;
            int token = GetTypeToken(interfaceType).Token;
            switch (token >> 24)
            {
                case TypeDefTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 0;
                    break;
                case TypeRefTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 1;
                    break;
                case TypeSpecTable.Index:
                    token = (token & 0xFFFFFF) << 2 | 2;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            rec.interfaceType = token;
            rec.pseudoToken = AllocPseudoToken();
            interfaceImplCustomAttributes.Add(rec);
            SetCustomAttribute(rec.pseudoToken, cab);
        }

        internal void ResolveInterfaceImplPseudoTokens()
        {
            if (interfaceImplCustomAttributes != null)
            {
                foreach (InterfaceImplCustomAttribute rec in interfaceImplCustomAttributes)
                {
                    for (int i = 0; i < InterfaceImpl.records.Length; i++)
                    {
                        if (InterfaceImpl.records[i].Class == rec.type && InterfaceImpl.records[i].Interface == rec.interfaceType)
                        {
                            RegisterTokenFixup(rec.pseudoToken, (InterfaceImplTable.Index << 24) | (i + 1));
                            break;
                        }
                    }
                }
            }
        }

        internal void FixupPseudoToken(ref int token)
        {
            if (IsPseudoToken(token))
            {
                token = ResolvePseudoToken(token);
            }
        }

        internal void SetIsSaved()
        {
            if (saved)
            {
                throw new InvalidOperationException();
            }
            saved = true;
        }

        internal bool IsSaved
        {
            get { return saved; }
        }

        internal override string GetString(int index)
        {
            return this.Strings.Find(index);
        }

    }

}
