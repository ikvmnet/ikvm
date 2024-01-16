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
using System.Reflection.Metadata;

using IKVM.Reflection.Metadata;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    public abstract class Module : ICustomAttributeProvider
    {

        internal readonly Universe universe;
        internal readonly ModuleTable ModuleTable = new ModuleTable();
        internal readonly TypeRefTable TypeRefTable = new TypeRefTable();
        internal readonly TypeDefTable TypeDefTable = new TypeDefTable();
        internal readonly FieldPtrTable FieldPtrTable = new FieldPtrTable();
        internal readonly FieldTable FieldTable = new FieldTable();
        internal readonly MemberRefTable MemberRefTable = new MemberRefTable();
        internal readonly ConstantTable ConstantTable = new ConstantTable();
        internal readonly CustomAttributeTable CustomAttributeTable = new CustomAttributeTable();
        internal readonly FieldMarshalTable FieldMarshalTable = new FieldMarshalTable();
        internal readonly DeclSecurityTable DeclSecurityTable = new DeclSecurityTable();
        internal readonly ClassLayoutTable ClassLayoutTable = new ClassLayoutTable();
        internal readonly FieldLayoutTable FieldLayoutTable = new FieldLayoutTable();
        internal readonly ParamPtrTable ParamPtrTable = new ParamPtrTable();
        internal readonly ParamTable ParamTable = new ParamTable();
        internal readonly InterfaceImplTable InterfaceImplTable = new InterfaceImplTable();
        internal readonly StandAloneSigTable StandAloneSigTable = new StandAloneSigTable();
        internal readonly EventMapTable EventMapTable = new EventMapTable();
        internal readonly EventPtrTable EventPtrTable = new EventPtrTable();
        internal readonly EventTable EventTable = new EventTable();
        internal readonly PropertyMapTable PropertyMapTable = new PropertyMapTable();
        internal readonly PropertyPtrTable PropertyPtrTable = new PropertyPtrTable();
        internal readonly PropertyTable PropertyTable = new PropertyTable();
        internal readonly MethodSemanticsTable MethodSemanticsTable = new MethodSemanticsTable();
        internal readonly MethodImplTable MethodImplTable = new MethodImplTable();
        internal readonly ModuleRefTable ModuleRefTable = new ModuleRefTable();
        internal readonly TypeSpecTable TypeSpecTable = new TypeSpecTable();
        internal readonly ImplMapTable ImplMapTable = new ImplMapTable();
        internal readonly FieldRVATable FieldRVATable = new FieldRVATable();
        internal readonly AssemblyTable AssemblyTable = new AssemblyTable();
        internal readonly AssemblyRefTable AssemblyRefTable = new AssemblyRefTable();
        internal readonly MethodPtrTable MethodPtrTable = new MethodPtrTable();
        internal readonly MethodDefTable MethodDefTable = new MethodDefTable();
        internal readonly NestedClassTable NestedClassTable = new NestedClassTable();
        internal readonly FileTable FileTable = new FileTable();
        internal readonly ExportedTypeTable ExportedTypeTable = new ExportedTypeTable();
        internal readonly ManifestResourceTable ManifestResourceTable = new ManifestResourceTable();
        internal readonly GenericParamTable GenericParamTable = new GenericParamTable();
        internal readonly MethodSpecTable MethodSpecTable = new MethodSpecTable();
        internal readonly GenericParamConstraintTable GenericParamConstraint = new GenericParamConstraintTable();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        internal Module(Universe universe)
        {
            this.universe = universe;
        }

        internal Table[] GetTables()
        {
            var tables = new Table[64];
            tables[ModuleTable.Index] = ModuleTable;
            tables[TypeRefTable.Index] = TypeRefTable;
            tables[TypeDefTable.Index] = TypeDefTable;
            tables[FieldPtrTable.Index] = FieldPtrTable;
            tables[FieldTable.Index] = FieldTable;
            tables[MemberRefTable.Index] = MemberRefTable;
            tables[ConstantTable.Index] = ConstantTable;
            tables[CustomAttributeTable.Index] = CustomAttributeTable;
            tables[FieldMarshalTable.Index] = FieldMarshalTable;
            tables[DeclSecurityTable.Index] = DeclSecurityTable;
            tables[ClassLayoutTable.Index] = ClassLayoutTable;
            tables[FieldLayoutTable.Index] = FieldLayoutTable;
            tables[ParamPtrTable.Index] = ParamPtrTable;
            tables[ParamTable.Index] = ParamTable;
            tables[InterfaceImplTable.Index] = InterfaceImplTable;
            tables[StandAloneSigTable.Index] = StandAloneSigTable;
            tables[EventMapTable.Index] = EventMapTable;
            tables[EventPtrTable.Index] = EventPtrTable;
            tables[EventTable.Index] = EventTable;
            tables[PropertyMapTable.Index] = PropertyMapTable;
            tables[PropertyPtrTable.Index] = PropertyPtrTable;
            tables[PropertyTable.Index] = PropertyTable;
            tables[MethodSemanticsTable.Index] = MethodSemanticsTable;
            tables[MethodImplTable.Index] = MethodImplTable;
            tables[ModuleRefTable.Index] = ModuleRefTable;
            tables[TypeSpecTable.Index] = TypeSpecTable;
            tables[ImplMapTable.Index] = ImplMapTable;
            tables[FieldRVATable.Index] = FieldRVATable;
            tables[AssemblyTable.Index] = AssemblyTable;
            tables[AssemblyRefTable.Index] = AssemblyRefTable;
            tables[MethodPtrTable.Index] = MethodPtrTable;
            tables[MethodDefTable.Index] = MethodDefTable;
            tables[NestedClassTable.Index] = NestedClassTable;
            tables[FileTable.Index] = FileTable;
            tables[ExportedTypeTable.Index] = ExportedTypeTable;
            tables[ManifestResourceTable.Index] = ManifestResourceTable;
            tables[GenericParamTable.Index] = GenericParamTable;
            tables[MethodSpecTable.Index] = MethodSpecTable;
            tables[GenericParamConstraintTable.Index] = GenericParamConstraint;
            return tables;
        }

        public virtual void __GetDataDirectoryEntry(int index, out int rva, out int length)
        {
            throw new NotSupportedException();
        }

        public virtual long __RelativeVirtualAddressToFileOffset(int rva)
        {
            throw new NotSupportedException();
        }

        public bool __GetSectionInfo(int rva, out string name, out int characteristics)
        {
            return __GetSectionInfo(rva, out name, out characteristics, out _, out _, out _, out _);
        }

        public virtual bool __GetSectionInfo(int rva, out string name, out int characteristics, out int virtualAddress, out int virtualSize, out int pointerToRawData, out int sizeOfRawData)
        {
            throw new NotSupportedException();
        }

        public virtual int __ReadDataFromRVA(int rva, byte[] data, int offset, int length)
        {
            throw new NotSupportedException();
        }

        public virtual void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
        {
            throw new NotSupportedException();
        }

        public virtual int __Subsystem
        {
            get { throw new NotSupportedException(); }
        }

        public FieldInfo GetField(string name)
        {
            return GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public FieldInfo GetField(string name, BindingFlags bindingFlags)
        {
            return IsResource() ? null : GetModuleType().GetField(name, bindingFlags | BindingFlags.DeclaredOnly);
        }

        public FieldInfo[] GetFields()
        {
            return GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public FieldInfo[] GetFields(BindingFlags bindingFlags)
        {
            return IsResource() ? Array.Empty<FieldInfo>() : GetModuleType().GetFields(bindingFlags | BindingFlags.DeclaredOnly);
        }

        public MethodInfo GetMethod(string name)
        {
            return IsResource() ? null : GetModuleType().GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public MethodInfo GetMethod(string name, Type[] types)
        {
            return IsResource() ? null : GetModuleType().GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, types, null);
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConv, Type[] types, ParameterModifier[] modifiers)
        {
            return IsResource() ? null : GetModuleType().GetMethod(name, bindingAttr | BindingFlags.DeclaredOnly, binder, callConv, types, modifiers);
        }

        public MethodInfo[] GetMethods()
        {
            return GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public MethodInfo[] GetMethods(BindingFlags bindingFlags)
        {
            return IsResource() ? Array.Empty<MethodInfo>() : GetModuleType().GetMethods(bindingFlags | BindingFlags.DeclaredOnly);
        }

        public ConstructorInfo __ModuleInitializer
        {
            get { return IsResource() ? null : GetModuleType().TypeInitializer; }
        }

        public virtual byte[] ResolveSignature(int metadataToken)
        {
            throw new NotSupportedException();
        }

        public virtual __StandAloneMethodSig __ResolveStandAloneMethodSig(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw new NotSupportedException();
        }

        public virtual CustomModifiers __ResolveTypeSpecCustomModifiers(int typeSpecToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            throw new NotSupportedException();
        }

        public int MetadataToken
        {
            get { return IsResource() ? 0 : 1; }
        }

        public abstract int MDStreamVersion { get; }

        public abstract Assembly Assembly { get; }

        public abstract string FullyQualifiedName { get; }

        public abstract string Name { get; }

        public abstract Guid ModuleVersionId { get; }

        public abstract MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        public abstract FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);

        public abstract MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments);


        public abstract string ResolveString(int metadataToken);

        public abstract Type[] __ResolveOptionalParameterTypes(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments, out CustomModifiers[] customModifiers);

        public abstract string ScopeName { get; }

        internal abstract void GetTypesImpl(List<Type> list);

        internal abstract Type FindType(TypeName name);

        internal abstract Type FindTypeIgnoreCase(TypeName lowerCaseName);

        [Obsolete("Please use __ResolveOptionalParameterTypes(int, Type[], Type[], out CustomModifiers[]) instead.")]
        public Type[] __ResolveOptionalParameterTypes(int metadataToken)
        {
            CustomModifiers[] dummy;
            return __ResolveOptionalParameterTypes(metadataToken, null, null, out dummy);
        }

        public Type GetType(string className)
        {
            return GetType(className, false, false);
        }

        public Type GetType(string className, bool ignoreCase)
        {
            return GetType(className, false, ignoreCase);
        }

        public Type GetType(string className, bool throwOnError, bool ignoreCase)
        {
            var parser = TypeNameParser.Parse(className, throwOnError);
            if (parser.Error)
                return null;

            if (parser.AssemblyName != null)
            {
                if (throwOnError)
                    throw new ArgumentException("Type names passed to Module.GetType() must not specify an assembly.");
                else
                    return null;
            }

            var typeName = TypeName.Split(TypeNameParser.Unescape(parser.FirstNamePart));
            var type = ignoreCase ? FindTypeIgnoreCase(typeName.ToLowerInvariant()) : FindType(typeName);
            if (type == null && __IsMissing)
                throw new MissingModuleException((MissingModule)this);

            return parser.Expand(type, this, throwOnError, className, false, ignoreCase);
        }

        public Type[] GetTypes()
        {
            var list = new List<Type>();
            GetTypesImpl(list);
            return list.ToArray();
        }

        public Type[] FindTypes(TypeFilter filter, object filterCriteria)
        {
            var list = new List<Type>();
            foreach (var type in GetTypes())
                if (filter(type, filterCriteria))
                    list.Add(type);

            return list.ToArray();
        }

        public virtual bool IsResource()
        {
            return false;
        }

        public Type ResolveType(int metadataToken)
        {
            return ResolveType(metadataToken, null, null);
        }

        internal sealed class GenericContext : IGenericContext
        {

            readonly Type[] genericTypeArguments;
            readonly Type[] genericMethodArguments;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="genericTypeArguments"></param>
            /// <param name="genericMethodArguments"></param>
            internal GenericContext(Type[] genericTypeArguments, Type[] genericMethodArguments)
            {
                this.genericTypeArguments = genericTypeArguments;
                this.genericMethodArguments = genericMethodArguments;
            }

            public Type GetGenericTypeArgument(int index)
            {
                return genericTypeArguments[index];
            }

            public Type GetGenericMethodArgument(int index)
            {
                return genericMethodArguments[index];
            }

        }

        public Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
        {
            if ((metadataToken >> 24) == TypeSpecTable.Index)
                return ResolveType(metadataToken, new GenericContext(genericTypeArguments, genericMethodArguments));
            else
                return ResolveType(metadataToken, null);
        }

        internal abstract Type ResolveType(int metadataToken, IGenericContext context);

        public MethodBase ResolveMethod(int metadataToken)
        {
            return ResolveMethod(metadataToken, null, null);
        }

        public FieldInfo ResolveField(int metadataToken)
        {
            return ResolveField(metadataToken, null, null);
        }

        public MemberInfo ResolveMember(int metadataToken)
        {
            return ResolveMember(metadataToken, null, null);
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit).Count != 0;
        }

        public IList<CustomAttributeData> __GetCustomAttributes(Type attributeType, bool inherit)
        {
            return CustomAttributeData.__GetCustomAttributes(this, attributeType, inherit);
        }

        public IList<CustomAttributeData> GetCustomAttributesData()
        {
            return CustomAttributeData.GetCustomAttributes(this);
        }

        public IEnumerable<CustomAttributeData> CustomAttributes
        {
            get { return GetCustomAttributesData(); }
        }

        public virtual IList<CustomAttributeData> __GetPlaceholderAssemblyCustomAttributes(bool multiple, bool security)
        {
            return Array.Empty<CustomAttributeData>();
        }

        public abstract AssemblyName[] __GetReferencedAssemblies();

        public virtual void __ResolveReferencedAssemblies(Assembly[] assemblies)
        {
            throw new NotSupportedException();
        }

        public abstract string[] __GetReferencedModules();

        public abstract Type[] __GetReferencedTypes();

        public abstract Type[] __GetExportedTypes();

        public virtual bool __IsMissing => false;

        public ulong __ImageBase => GetImageBaseImpl();

        protected abstract ulong GetImageBaseImpl();

        public ulong __StackReserve => GetStackReserveImpl();

        protected abstract ulong GetStackReserveImpl();

        public uint __FileAlignment => GetFileAlignmentImpl();

        protected abstract uint GetFileAlignmentImpl();

        public DllCharacteristics __DllCharacteristics => GetDllCharacteristicsImpl();

        protected abstract DllCharacteristics GetDllCharacteristicsImpl();

        public virtual byte[] __ModuleHash => throw new NotSupportedException();

        public virtual int __EntryPointRVA => throw new NotSupportedException();

        public virtual int __EntryPointToken => throw new NotSupportedException();

        public virtual string __ImageRuntimeVersion => throw new NotSupportedException();

        public IEnumerable<CustomAttributeData> __EnumerateCustomAttributeTable()
        {
            var list = new List<CustomAttributeData>(CustomAttributeTable.RowCount);
            for (int i = 0; i < CustomAttributeTable.RowCount; i++)
                list.Add(new CustomAttributeData(this, i));

            return list;
        }

        [Obsolete]
        public List<CustomAttributeData> __GetCustomAttributesFor(int token)
        {
            return CustomAttributeData.GetCustomAttributesImpl(new List<CustomAttributeData>(), this, token, null);
        }

        public bool __TryGetImplMap(int token, out ImplMapFlags mappingFlags, out string importName, out string importScope)
        {
            foreach (int i in ImplMapTable.Filter(token))
            {
                mappingFlags = (ImplMapFlags)(ushort)ImplMapTable.records[i].MappingFlags;
                importName = GetString(ImplMapTable.records[i].ImportName);
                importScope = GetString(ModuleRefTable.records[(ImplMapTable.records[i].ImportScope & 0xFFFFFF) - 1]);
                return true;
            }

            mappingFlags = 0;
            importName = null;
            importScope = null;
            return false;
        }

        public virtual System.Security.Cryptography.X509Certificates.X509Certificate GetSignerCertificate()
        {
            return null;
        }

        internal abstract Type GetModuleType();

        internal IList<CustomAttributeData> GetDeclarativeSecurity(int metadataToken)
        {
            var list = new List<CustomAttributeData>();
            foreach (var i in DeclSecurityTable.Filter(metadataToken))
                CustomAttributeData.ReadDeclarativeSecurity(this, i, list);

            return list;
        }

        internal virtual void Dispose()
        {

        }

        internal virtual void ExportTypes(AssemblyFileHandle handle, IKVM.Reflection.Emit.ModuleBuilder manifestModule)
        {

        }

        internal virtual string GetString(StringHandle handle)
        {
            throw new NotImplementedException();
        }

        internal virtual ByteReader GetBlobReader(BlobHandle handle)
        {
            throw new NotImplementedException();
        }

    }

    public delegate bool TypeFilter(Type m, object filterCriteria);

    public delegate bool MemberFilter(MemberInfo m, object filterCriteria);

}
