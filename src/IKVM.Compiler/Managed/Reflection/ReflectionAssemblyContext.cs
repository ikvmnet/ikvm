using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Maintains a context for a specific assembly file.
    /// </summary>
    internal class ReflectionAssemblyContext : IManagedAssemblyContext, IManagedAssemblyResolver
    {

        record class CacheEntry<T>(T Value);

        const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        readonly IReflectionAssemblyResolver resolver;
        readonly ConditionalWeakTable<Assembly, ManagedAssembly> assemblyCache = new();
        readonly ConditionalWeakTable<Type, ManagedType> typeCache = new();
        readonly ConditionalWeakTable<Type, CacheEntry<ManagedExportedType>> exportedTypeCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public ReflectionAssemblyContext(IReflectionAssemblyResolver resolver)
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.LoadAssembly(ManagedAssembly, out ManagedAssemblyData)" /> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        void IManagedAssemblyContext.LoadAssembly(ManagedAssembly assembly, out ManagedAssemblyData data) => LoadAssembly(assembly, out data);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveTypes(ManagedAssembly)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> IManagedAssemblyContext.ResolveTypes(ManagedAssembly assembly) => ResolveTypes(assembly);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveType(ManagedAssembly, string)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyContext.ResolveType(ManagedAssembly assembly, string typeName) => ResolveType(assembly, typeName);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveExportedTypes(ManagedAssembly)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> IManagedAssemblyContext.ResolveExportedTypes(ManagedAssembly assembly) => ResolveExportedTypes(assembly);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveExportedType(ManagedAssembly, string)"/> method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedExportedType? IManagedAssemblyContext.ResolveExportedType(ManagedAssembly assembly, string typeName) => ResolveExportedType(assembly, typeName);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.LoadType(ManagedType,  out ManagedTypeData)"/> method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void IManagedAssemblyContext.LoadType(ManagedType type, out ManagedTypeData result) => LoadType(type, out result);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveNestedTypes(ManagedType)"/> method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> IManagedAssemblyContext.ResolveNestedTypes(ManagedType type) => ResolveNestedTypes(type);

        /// <summary>
        /// Implements the <see cref="IManagedAssemblyContext.ResolveNestedType(ManagedType,  string)"/> method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyContext.ResolveNestedType(ManagedType type, string typeName) => ResolveNestedType(type, typeName);

        /// <summary>
        /// Attempts to load the managed assembly from the specified assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        ManagedAssembly? IManagedAssemblyResolver.ResolveAssembly(AssemblyName assemblyName) => ResolveAssembly(assemblyName);

        /// <summary>
        /// Resolves the managed assembly associated with the given reflection assembly.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        ManagedAssembly? ResolveAssembly(AssemblyName assemblyName)
        {
            return resolver.Resolve(assemblyName) is Assembly assembly ? ResolveAssembly(assembly) : null;
        }

        /// <summary>
        /// Resolves the managed assembly associated with the given reflection assembly.
        /// </summary>
        /// <param name="refAssembly"></param>
        /// <returns></returns>
        ManagedAssembly ResolveAssembly(Assembly refAssembly)
        {
            return assemblyCache.GetValue(refAssembly, CreateAssembly);
        }

        /// <summary>
        /// Resolves the managed assembly owned by this context.
        /// </summary>
        /// <returns></returns>
        ManagedAssembly CreateAssembly(Assembly refAssembly)
        {
            return new ManagedAssembly(this, refAssembly, refAssembly.GetName());
        }

        /// <summary>
        /// Resolves the named type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedAssembly assembly, string typeName)
        {
            var refType = ((Assembly)assembly.Handle).GetType(typeName);
            return refType != null ? ResolveType(assembly, refType) : null;
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedType ResolveType(ManagedAssembly assembly, Type refType)
        {
            return typeCache.GetValue(refType, _ => CreateType(assembly, refType));
        }

        /// <summary>
        /// Creates a new managed type for the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedType CreateType(ManagedAssembly assembly, Type refType)
        {
            return new ManagedType(assembly, refType, refType.Name, refType.Attributes);
        }

        /// <summary>
        /// Resolves all available types on the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly)
        {
            var refAssembly = (Assembly)assembly.Handle;
            return ResolveTypes(assembly, refAssembly.GetTypes());
        }

        /// <summary>
        /// Resolves all available exported types on the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refAssembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> ResolveExportedTypes(ManagedAssembly assembly)
        {
            var refAssembly = (Assembly)assembly.Handle;
            return ResolveExportedTypes(assembly, refAssembly.GetExportedTypes());
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refTypes"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly, Type[] refTypes)
        {
            for (int i = 0; i < refTypes.Length; i++)
                yield return ResolveType(assembly, refTypes[i]) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refTypes"></param>
        /// <returns></returns>
        IEnumerable<ManagedExportedType> ResolveExportedTypes(ManagedAssembly assembly, Type[] refTypes)
        {
            for (int i = 0; i < refTypes.Length; i++)
                yield return ResolveExportedType(assembly, refTypes[i]);
        }

        /// <summary>
        /// Resolves the specified exported type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedExportedType? ResolveExportedType(ManagedAssembly assembly, string typeName)
        {
            var refAssembly = (Assembly)assembly.Handle;
            var refExportedType = refAssembly.GetExportedTypes().FirstOrDefault(i => i.FullName == typeName);
            return refExportedType != null ? ResolveExportedType(assembly, refExportedType) : null;
        }

        /// <summary>
        /// Resolves the specified exported type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedExportedType ResolveExportedType(ManagedAssembly assembly, Type refType)
        {
            return exportedTypeCache.GetValue(refType, _ => CreateExportedType(assembly, _)).Value;
        }

        /// <summary>
        /// Creates a new managed type for the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        CacheEntry<ManagedExportedType> CreateExportedType(ManagedAssembly assembly, Type refType)
        {
            var result = new ManagedExportedType();
            result.Name = refType.FullName ?? throw new ManagedTypeException("Missing type name.");
            LoadExportedTypeCustomAttributes(assembly, refType.GetCustomAttributesData(), out result.CustomAttributes);
            return new CacheEntry<ManagedExportedType>(result);
        }

        /// <summary>
        /// Resolves all nested types of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type)
        {
            var refType = (Type)type.Handle;
            foreach (var i in refType.GetNestedTypes(BINDING_FLAGS))
                yield return ResolveType(type.Assembly, i);
        }

        /// <summary>
        /// Resolves the nested type of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveNestedType(ManagedType type, string typeName)
        {
            var refType = (Type)type.Handle;
            return refType.GetNestedType(typeName, BINDING_FLAGS) is Type t ? ResolveType(type.Assembly, t) : null;
        }

        /// <summary>
        /// Resolves a reference to the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedTypeRef ResolveTypeReference(ManagedAssembly assembly, Type refType)
        {
            return new ManagedTypeRef(refType.Assembly.GetName(), refType.FullName!);
        }

        /// <summary>
        /// Resolves a signature from the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedSignature ResolveTypeSignature(ManagedAssembly assembly, Type refType)
        {
            if (refType.IsPrimitive && refType.FullName == "System.Void")
                return ManagedSignature.Void;
            else if (refType.IsPrimitive && refType.FullName == "System.Boolean")
                return ManagedSignature.Boolean;
            else if (refType.IsPrimitive && refType.FullName == "System.Byte")
                return ManagedSignature.Byte;
            else if (refType.IsPrimitive && refType.FullName == "System.SByte")
                return ManagedSignature.SByte;
            else if (refType.IsPrimitive && refType.FullName == "System.Char")
                return ManagedSignature.Char;
            else if (refType.IsPrimitive && refType.FullName == "System.Int16")
                return ManagedSignature.Int16;
            else if (refType.IsPrimitive && refType.FullName == "System.UInt16")
                return ManagedSignature.UInt16;
            else if (refType.IsPrimitive && refType.FullName == "System.Int32")
                return ManagedSignature.Int32;
            else if (refType.IsPrimitive && refType.FullName == "System.UInt32")
                return ManagedSignature.UInt32;
            else if (refType.IsPrimitive && refType.FullName == "System.Int64")
                return ManagedSignature.Int64;
            else if (refType.IsPrimitive && refType.FullName == "System.UInt64")
                return ManagedSignature.UInt64;
            else if (refType.IsPrimitive && refType.FullName == "System.Single")
                return ManagedSignature.Single;
            else if (refType.IsPrimitive && refType.FullName == "System.Double")
                return ManagedSignature.Double;
            else if (refType.IsPrimitive && refType.FullName == "System.IntPtr")
                return ManagedSignature.IntPtr;
            else if (refType.IsPrimitive && refType.FullName == "System.UIntPtr")
                return ManagedSignature.UIntPtr;
            else if (refType.IsPrimitive && refType.FullName == "System.Object")
                return ManagedSignature.Object;
            else if (refType.IsPrimitive && refType.FullName == "System.String")
                return ManagedSignature.String;
            else if (refType.IsPrimitive && refType.FullName == "System.TypedReference")
                return ManagedSignature.TypedReference;
#if NETFRAMEWORK
            else if (refType.IsArray && refType.GetArrayRank() == 1 && refType == refType.GetElementType().MakeArrayType())
#else
            else if (refType.IsSZArray)
#endif
                return ResolveTypeSignature(assembly, refType.GetElementType()!).CreateArray();
            else if (refType.IsArray)
                throw new ManagedTypeException("Unsupported multidimensional array type.");
            else if (refType.IsByRef)
                return ResolveTypeSignature(assembly, refType.GetElementType()!).CreateByRef();
            else if (refType.IsPointer)
                return ResolveTypeSignature(assembly, refType.GetElementType()!).CreatePointer();
            else if (refType.IsGenericType)
            {
                var a = refType.GetGenericArguments();
                var l = new FixedValueList4<ManagedSignature>(a.Length);

                for (int i = 0; i < a.Length; i++)
                    l[i] = ResolveTypeSignature(assembly, a[i]);

                return ResolveTypeSignature(assembly, refType.GetGenericTypeDefinition()).CreateGeneric(l);
            }
            else
                return ManagedSignature.Type(ResolveTypeReference(assembly, refType));
        }

        /// <summary>
        /// Loads the data for the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadAssembly(ManagedAssembly assembly, out ManagedAssemblyData result)
        {
            var refAssembly = (Assembly)assembly.Handle;
            result = new ManagedAssemblyData();
            LoadAssemblyCustomAttributes(assembly, refAssembly.GetCustomAttributesData(), out result.CustomAttributes);
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refAttributeList"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        void LoadAssemblyCustomAttributes(ManagedAssembly assembly, IList<CustomAttributeData> refAttributeList, out FixedValueList8<ManagedCustomAttribute> list)
        {
            list = new FixedValueList8<ManagedCustomAttribute>(refAttributeList.Count);

            for (int i = 0; i < refAttributeList.Count; i++)
                LoadAssemblyCustomAttribute(assembly, refAttributeList[i], out list.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refAttribute"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadAssemblyCustomAttribute(ManagedAssembly assembly, CustomAttributeData refAttribute, out ManagedCustomAttribute result)
        {
            result = new ManagedCustomAttribute();
        }

        /// <summary>
        /// Loads the data for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void LoadType(ManagedType type, out ManagedTypeData result)
        {
            var refType = (Type)type.Handle;
            result.DeclaringType = refType.DeclaringType != null ? ResolveType(type.Assembly, refType.DeclaringType) : null;
            LoadTypeGenericParameters(type, refType.GetGenericArguments(), out result.GenericParameters);
            LoadTypeCustomAttributes(type, refType.GetCustomAttributesData(), out result.CustomAttributes);
            result.BaseType = refType.BaseType != null ? ResolveTypeSignature(type.Assembly, refType.BaseType) : null;
            LoadTypeInterfaces(type, refType.GetInterfaces(), out result.Interfaces);
            LoadTypeFields(type, refType.GetFields(BINDING_FLAGS), out result.Fields);
            LoadTypeMethods(type, refType.GetMethods(BINDING_FLAGS), out result.Methods);
            LoadTypeProperties(type, refType.GetProperties(BINDING_FLAGS), out result.Properties);
            LoadTypeEvents(type, refType.GetEvents(BINDING_FLAGS), out result.Events);
            LoadTypeNestedTypes(type, refType.GetNestedTypes(), out result.NestedTypes);
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="refAttributeList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeCustomAttributes(ManagedType type, IList<CustomAttributeData> refAttributeList, out FixedValueList1<ManagedCustomAttribute> result)
        {
            result = new FixedValueList1<ManagedCustomAttribute>(refAttributeList.Count);

            for (int i = 0; i < refAttributeList.Count; i++)
                LoadTypeCustomAttribute(type, refAttributeList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refAttribute"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeCustomAttribute(ManagedType type, CustomAttributeData refAttribute, out ManagedCustomAttribute result)
        {
            result = new ManagedCustomAttribute();
        }

        /// <summary>
        /// Loads the generic parameters from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refParameterList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeGenericParameters(ManagedType type, Type[] refParameterList, out FixedValueList1<ManagedGenericParameter> result)
        {
            result = new FixedValueList1<ManagedGenericParameter>(refParameterList.Length);

            for (int i = 0; i < refParameterList.Length; i++)
                LoadTypeGenericParameter(type, refParameterList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refParameter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeGenericParameter(ManagedType type, Type refParameter, out ManagedGenericParameter result)
        {
            result = new ManagedGenericParameter();
            result.Name = refParameter.Name;
            LoadTypeGenericParameterConstraints(type, refParameter.GetGenericParameterConstraints(), out result.Constraints);
        }

        /// <summary>
        /// Loads the generic parameter constraints from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refConstraintList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeGenericParameterConstraints(ManagedType type, Type[] refConstraintList, out FixedValueList1<ManagedGenericParameterConstraint> result)
        {
            result = new FixedValueList1<ManagedGenericParameterConstraint>(refConstraintList.Length);

            for (int i = 0; i < refConstraintList.Length; i++)
                LoadTypeGenericParameterConstraint(type, refConstraintList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refConstraint"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeGenericParameterConstraint(ManagedType type, Type refConstraint, out ManagedGenericParameterConstraint result)
        {
            result = new ManagedGenericParameterConstraint();
            result.Type = ResolveTypeSignature(type.Assembly, refConstraint);
            LoadTypeCustomAttributes(type, refConstraint.GetCustomAttributesData(), out result.CustomAttributes);
        }

        /// <summary>
        /// Loads the interfaces from the given collection.
        /// </summary>
        /// <param name="refInterfaceList"></param>
        /// <returns></returns>
        void LoadTypeInterfaces(ManagedType type, Type[] refInterfaceList, out FixedValueList2<ManagedInterface> result)
        {
            result = new FixedValueList2<ManagedInterface>(refInterfaceList.Length);

            for (int i = 0; i < refInterfaceList.Length; i++)
                LoadTypeInterface(type, refInterfaceList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="refInterface"></param>
        /// <returns></returns>
        void LoadTypeInterface(ManagedType type, Type refInterface, out ManagedInterface result)
        {
            result = new ManagedInterface();
            result.Type = ResolveTypeSignature(type.Assembly, refInterface);
            LoadTypeCustomAttributes(type, refInterface.GetCustomAttributesData(), out result.CustomAttributes);
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="refFieldList"></param>
        /// <returns></returns>
        void LoadTypeFields(ManagedType type, FieldInfo[] refFieldList, out FixedValueList4<ManagedField> result)
        {
            result = new FixedValueList4<ManagedField>(refFieldList.Length);

            for (int i = 0; i < refFieldList.Length; i++)
                LoadTypeField(type, refFieldList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given field.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refField"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeField(ManagedType type, FieldInfo refField, out ManagedField result)
        {
            result = new ManagedField();
            result.Name = refField.Name;
            result.Attributes = refField.Attributes;
            LoadTypeCustomAttributes(type, refField.GetCustomAttributesData(), out result.CustomAttributes);
            result.FieldType = ResolveTypeSignature(type.Assembly, refField.FieldType);
        }

        /// <summary>
        /// Loads the methods from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refMethodList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeMethods(ManagedType type, MethodBase[] refMethodList, out FixedValueList4<ManagedMethod> result)
        {
            result = new FixedValueList4<ManagedMethod>(refMethodList.Length);

            for (int i = 0; i < refMethodList.Length; i++)
                LoadTypeMethod(type, refMethodList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refMethod"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeMethod(ManagedType type, MethodBase refMethod, out ManagedMethod result)
        {
            result = new ManagedMethod(false);
            result.Name = refMethod.Name;
            result.Attributes = refMethod.Attributes;
            result.ImplAttributes = refMethod.MethodImplementationFlags;
            LoadTypeCustomAttributes(type, refMethod.GetCustomAttributesData(), out result.CustomAttributes);
            LoadTypeParameters(type, refMethod.GetParameters(), out result.Parameters);

            switch (refMethod)
            {
                case ConstructorInfo ctor:
                    result.GenericParameters = FixedValueList1<ManagedGenericParameter>.Empty;
                    result.ReturnType = ManagedSignature.Void;
                    break;
                case MethodInfo method:
                    result = new ManagedMethod(false);
                    LoadTypeGenericParameters(type, method.GetGenericArguments(), out result.GenericParameters);
                    result.ReturnType = ResolveTypeSignature(type.Assembly, method.ReturnType);
                    break;
            }
        }

        /// <summary>
        /// Loads the properties from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refPropertyList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeProperties(ManagedType type, PropertyInfo[] refPropertyList, out FixedValueList4<ManagedProperty> result)
        {
            result = new FixedValueList4<ManagedProperty>(refPropertyList.Length);

            for (int i = 0; i < refPropertyList.Length; i++)
                LoadTypeProperty(type, refPropertyList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given property.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refProperty"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeProperty(ManagedType type, PropertyInfo refProperty, out ManagedProperty result)
        {
            result = new ManagedProperty();
            result.Name = refProperty.Name;
            result.Attributes = refProperty.Attributes;
            LoadTypeCustomAttributes(type, refProperty.GetCustomAttributesData(), out result.CustomAttributes);
            result.PropertyType = ResolveTypeSignature(type.Assembly, refProperty.PropertyType);
        }

        /// <summary>
        /// Loads the evemts from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refEventList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeEvents(ManagedType type, EventInfo[] refEventList, out FixedValueList1<ManagedEvent> result)
        {
            result = new FixedValueList1<ManagedEvent>(refEventList.Length);

            for (int i = 0; i < refEventList.Length; i++)
                LoadTypeEvent(type, refEventList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refEvent"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeEvent(ManagedType type, EventInfo refEvent, out ManagedEvent result)
        {
            result = new ManagedEvent();
            result.Name = refEvent.Name;
            result.Attributes = refEvent.Attributes;
            LoadTypeCustomAttributes(type, refEvent.GetCustomAttributesData(), out result.CustomAttributes);
            result.EventHandlerType = ResolveTypeSignature(type.Assembly, refEvent.EventHandlerType!);
        }

        /// <summary>
        /// Loads the parameters from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refParameterList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeParameters(ManagedType type, ParameterInfo[] refParameterList, out FixedValueList4<ManagedParameter> result)
        {
            result = new FixedValueList4<ManagedParameter>(refParameterList.Length);

            for (int i = 0; i < refParameterList.Length; i++)
                LoadTypeParameter(type, refParameterList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given parameter.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refParameter"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeParameter(ManagedType type, ParameterInfo refParameter, out ManagedParameter result)
        {
            result = new ManagedParameter();
            result.Name = refParameter.Name;
            result.Attributes = refParameter.Attributes;
            LoadTypeCustomAttributes(type, refParameter.GetCustomAttributesData(), out result.CustomAttributes);
            result.ParameterType = ResolveTypeSignature(type.Assembly, refParameter.ParameterType);
        }

        /// <summary>
        /// Loads the nested types from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refTypeList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeNestedTypes(ManagedType type, Type[] refTypeList, out FixedValueList1<ManagedType> result)
        {
            result = new FixedValueList1<ManagedType>(refTypeList.Length);

            for (int i = 0; i < refTypeList.Length; i++)
                LoadTypeNestedType(type, refTypeList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given nested type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refType"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeNestedType(ManagedType type, Type refType, out ManagedType result)
        {
            result = ResolveType(type.Assembly, refType);
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refAttributeList"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        void LoadExportedTypeCustomAttributes(ManagedAssembly assembly, IList<CustomAttributeData> refAttributeList, out FixedValueList1<ManagedCustomAttribute> list)
        {
            list = new FixedValueList1<ManagedCustomAttribute>(refAttributeList.Count);

            for (int i = 0; i < refAttributeList.Count; i++)
                LoadAssemblyCustomAttribute(assembly, refAttributeList[i], out list.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refAttribute"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadExportedTypeCustomAttribute(ManagedAssembly assembly, CustomAttributeData refAttribute, out ManagedCustomAttribute result)
        {
            result = new ManagedCustomAttribute();
        }

    }

}
