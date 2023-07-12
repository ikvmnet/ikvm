using System;
using System.Collections.Concurrent;
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

        const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        readonly IReflectionAssemblyResolver resolver;
        readonly ConcurrentDictionary<string, Assembly?> assemblyNameCache = new();
        readonly ConcurrentDictionary<Assembly, ManagedAssembly> assemblyCache = new();
        readonly ConcurrentDictionary<Type, ManagedType> typeCache = new();
        readonly ConcurrentDictionary<Type, ManagedExportedType> exportedTypeCache = new();

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
        ManagedAssembly? IManagedAssemblyResolver.ResolveAssembly(string assemblyName) => ResolveAssembly(assemblyName);

        /// <summary>
        /// Resolves the specified assembly by name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        internal ManagedAssembly? ResolveAssembly(string assemblyName)
        {
            var refAssembly = assemblyNameCache.GetOrAdd(assemblyName, resolver.Resolve);
            if (refAssembly == null)
                return null;

            return ResolveAssembly(refAssembly);
        }

        /// <summary>
        /// Resolves the specified assembly.
        /// </summary>
        /// <param name="refAssembly"></param>
        /// <returns></returns>
        ManagedAssembly? ResolveAssembly(Assembly refAssembly)
        {
            return assemblyCache.GetOrAdd(refAssembly, CreateAssembly);
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
            var refAssembly = (Assembly)assembly.Handle;
            if (refAssembly == null)
                throw new ManagedException($"Invalid assembly, missing handle.");

            var refType = refAssembly.GetType(typeName);
            if (refType == null)
                throw new ManagedException($"Invalid assembly, missing handle.");

            return ResolveType(refType);
        }

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedType? ResolveType(Type refType)
        {
            // assembly of type takes priority over passed assembly.
            var refAssembly = ResolveAssembly(refType.Assembly);
            if (refAssembly == null)
                return null;

            return typeCache.GetOrAdd(refType, _ => CreateType(refAssembly, refType));
        }

        /// <summary>
        /// Creates a new managed type for the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedType CreateType(ManagedAssembly assembly, Type refType)
        {
            return new ManagedType(assembly, refType, refType.FullName, refType.Attributes);
        }

        /// <summary>
        /// Resolves all available types on the given assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly)
        {
            var refAssembly = (Assembly)assembly.Handle;
            if (refAssembly == null)
                throw new ManagedException($"Invalid assembly, missing handle.");

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
            if (refAssembly == null)
                throw new ManagedException($"Invalid assembly, missing handle.");

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
                yield return ResolveType(refTypes[i]) ?? throw new ManagedException("Could not resolve type.");
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
            if (refAssembly == null)
                throw new ManagedException($"Invalid assembly, missing handle.");

            var refExportedType = refAssembly.GetExportedTypes().FirstOrDefault(i => i.FullName == typeName);
            if (refExportedType == null)
                return null;

            return ResolveExportedType(assembly, refExportedType);
        }

        /// <summary>
        /// Resolves the specified exported type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedExportedType ResolveExportedType(ManagedAssembly assembly, Type refType)
        {
            return exportedTypeCache.GetOrAdd(refType, _ => CreateExportedType(assembly, _));
        }

        /// <summary>
        /// Creates a new managed type for the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedExportedType CreateExportedType(ManagedAssembly assembly, Type refType)
        {
            var result = new ManagedExportedType();
            result.Name = refType.FullName ?? throw new ManagedException("Missing type name.");
            LoadExportedTypeCustomAttributes(assembly, refType.GetCustomAttributesData(), out result.CustomAttributes);
            return result;
        }

        /// <summary>
        /// Resolves all nested types of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type)
        {
            var refType = (Type)type.Handle;
            if (refType == null)
                throw new ManagedException($"Invalid type, missing handle.");

            foreach (var i in refType.GetNestedTypes(BINDING_FLAGS))
                yield return ResolveType(i) ?? throw new ManagedException("Could not resolve nested type.");
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
            if (refType == null)
                throw new ManagedException($"Invalid type, missing handle.");

            var nestedRefType = refType.GetNestedType(typeName, BINDING_FLAGS);
            if (nestedRefType == null)
                return null;

            return ResolveType(nestedRefType);
        }

        /// <summary>
        /// Resolves a signature from the specified type.
        /// </summary>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedSignature ResolveSignature(Type refType)
        {
#if NETFRAMEWORK
            if (refType.IsArray && refType.GetArrayRank() == 1 && refType == refType.GetElementType().MakeArrayType())
#else
            if (refType.IsSZArray)
#endif
                return ResolveSignature(refType.GetElementType()!).CreateArray();
            else if (refType.IsArray)
                return ResolveSignature(refType.GetElementType()!).CreateArray(refType.GetArrayRank());
            else if (refType.IsByRef)
                return ResolveSignature(refType.GetElementType()!).CreateByRef();
            else if (refType.IsPointer)
                return ResolveSignature(refType.GetElementType()!).CreatePointer();
            else if (refType.IsConstructedGenericType)
                return ResolveGenericSignature(refType);
            else
                return ManagedSignature.Type(ResolveType(refType) ?? throw new ManagedResolveException($"Could not resolve signature type '{refType}'."));
        }

        /// <summary>
        /// Resolves a signature from the specified constructed generic type.
        /// </summary>
        /// <param name="refType"></param>
        /// <returns></returns>
        ManagedSignature ResolveGenericSignature(Type refType)
        {
            var argRefTypes = refType.GetGenericArguments();
            var argSigs = new FixedValueList4<ManagedSignature>(argRefTypes.Length);

            // resolve signatures for each argument
            for (int i = 0; i < argRefTypes.Length; i++)
                argSigs[i] = ResolveSignature(argRefTypes[i]);

            return ResolveSignature(refType.GetGenericTypeDefinition()).CreateGeneric(argSigs);
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
            if (refAssembly == null)
                throw new ManagedException($"Invalid assembly, missing handle.");

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
        void LoadAssemblyCustomAttributes(ManagedAssembly assembly, IList<CustomAttributeData> refAttributeList, out FixedValueList8<ManagedCustomAttributeData> list)
        {
            list = new FixedValueList8<ManagedCustomAttributeData>(refAttributeList.Count);

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
        void LoadAssemblyCustomAttribute(ManagedAssembly assembly, CustomAttributeData refAttribute, out ManagedCustomAttributeData result)
        {
            result = new ManagedCustomAttributeData();
        }

        /// <summary>
        /// Loads the data for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="result"></param>
        void LoadType(ManagedType type, out ManagedTypeData result)
        {
            var refType = (Type)type.Handle;

            result = new ManagedTypeData();
            result.DeclaringType = refType.DeclaringType != null ? ResolveType(refType.DeclaringType) : null;
            LoadTypeGenericParameters(type, refType.GetGenericArguments(), out result.GenericParameters);
            LoadTypeCustomAttributes(type, refType.GetCustomAttributesData(), out result.CustomAttributes);
            result.BaseType = refType.BaseType != null ? ResolveSignature(refType.BaseType) : null;
            LoadTypeInterfaces(type, refType.GetInterfaces(), out result.Interfaces);
            LoadTypeFields(type, refType.GetFields(BINDING_FLAGS), out result.Fields);
            LoadTypeMethods(type, refType.GetMethods(BINDING_FLAGS), out result.Methods);
            LoadTypeProperties(type, refType.GetProperties(BINDING_FLAGS), out result.Properties);
            LoadTypeEvents(type, refType.GetEvents(BINDING_FLAGS), out result.Events);
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="refAttributeList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeCustomAttributes(ManagedType type, IList<CustomAttributeData> refAttributeList, out FixedValueList1<ManagedCustomAttributeData> result)
        {
            result = new FixedValueList1<ManagedCustomAttributeData>(refAttributeList.Count);

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
        void LoadTypeCustomAttribute(ManagedType type, CustomAttributeData refAttribute, out ManagedCustomAttributeData result)
        {
            result = new ManagedCustomAttributeData();
        }

        /// <summary>
        /// Loads the generic parameters from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refParameterList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeGenericParameters(ManagedType type, Type[] refParameterList, out FixedValueList1<ManagedGenericParameterData> result)
        {
            result = new FixedValueList1<ManagedGenericParameterData>(refParameterList.Length);

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
        void LoadTypeGenericParameter(ManagedType type, Type refParameter, out ManagedGenericParameterData result)
        {
            result = new ManagedGenericParameterData();
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
        void LoadTypeGenericParameterConstraints(ManagedType type, Type[] refConstraintList, out FixedValueList1<ManagedGenericParameterConstraintData> result)
        {
            result = new FixedValueList1<ManagedGenericParameterConstraintData>(refConstraintList.Length);

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
        void LoadTypeGenericParameterConstraint(ManagedType type, Type refConstraint, out ManagedGenericParameterConstraintData result)
        {
            result = new ManagedGenericParameterConstraintData();
            result.Type = ResolveSignature(refConstraint);
            LoadTypeCustomAttributes(type, refConstraint.GetCustomAttributesData(), out result.CustomAttributes);
        }

        /// <summary>
        /// Loads the interfaces from the given collection.
        /// </summary>
        /// <param name="refInterfaceList"></param>
        /// <returns></returns>
        void LoadTypeInterfaces(ManagedType type, Type[] refInterfaceList, out FixedValueList2<ManagedInterfaceData> result)
        {
            result = new FixedValueList2<ManagedInterfaceData>(refInterfaceList.Length);

            for (int i = 0; i < refInterfaceList.Length; i++)
                LoadTypeInterface(type, refInterfaceList[i], out result.GetItemRef(i));
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="refInterface"></param>
        /// <returns></returns>
        void LoadTypeInterface(ManagedType type, Type refInterface, out ManagedInterfaceData result)
        {
            result = new ManagedInterfaceData();
            result.Type = ResolveSignature(refInterface);
            LoadTypeCustomAttributes(type, refInterface.GetCustomAttributesData(), out result.CustomAttributes);
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="refFieldList"></param>
        /// <returns></returns>
        void LoadTypeFields(ManagedType type, FieldInfo[] refFieldList, out FixedValueList4<ManagedFieldData> result)
        {
            result = new FixedValueList4<ManagedFieldData>(refFieldList.Length);

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
        void LoadTypeField(ManagedType type, FieldInfo refField, out ManagedFieldData result)
        {
            result = new ManagedFieldData();
            result.Name = refField.Name;
            result.Attributes = refField.Attributes;
            LoadTypeCustomAttributes(type, refField.GetCustomAttributesData(), out result.CustomAttributes);
            result.FieldType = ResolveSignature(refField.FieldType);
        }

        /// <summary>
        /// Loads the methods from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refMethodList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeMethods(ManagedType type, MethodBase[] refMethodList, out FixedValueList4<ManagedMethodData> result)
        {
            result = new FixedValueList4<ManagedMethodData>(refMethodList.Length);

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
        void LoadTypeMethod(ManagedType type, MethodBase refMethod, out ManagedMethodData result)
        {
            result = new ManagedMethodData();
            result.Name = refMethod.Name;
            result.Attributes = refMethod.Attributes;
            result.ImplAttributes = refMethod.MethodImplementationFlags;
            LoadTypeCustomAttributes(type, refMethod.GetCustomAttributesData(), out result.CustomAttributes);
            LoadTypeParameters(type, refMethod.GetParameters(), out result.Parameters);

            switch (refMethod)
            {
                case ConstructorInfo ctor:
                    result.GenericParameters = FixedValueList1<ManagedGenericParameterData>.Empty;
                    result.ReturnType = ResolveSignature(typeof(void));
                    break;
                case MethodInfo method:
                    LoadTypeGenericParameters(type, method.GetGenericArguments(), out result.GenericParameters);
                    result.ReturnType = ResolveSignature(method.ReturnType);
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
        void LoadTypeProperties(ManagedType type, PropertyInfo[] refPropertyList, out FixedValueList4<ManagedPropertyData> result)
        {
            result = new FixedValueList4<ManagedPropertyData>(refPropertyList.Length);

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
        void LoadTypeProperty(ManagedType type, PropertyInfo refProperty, out ManagedPropertyData result)
        {
            result = new ManagedPropertyData();
            result.Name = refProperty.Name;
            result.Attributes = refProperty.Attributes;
            LoadTypeCustomAttributes(type, refProperty.GetCustomAttributesData(), out result.CustomAttributes);
            result.PropertyType = ResolveSignature(refProperty.PropertyType);
        }

        /// <summary>
        /// Loads the evemts from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refEventList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeEvents(ManagedType type, EventInfo[] refEventList, out FixedValueList1<ManagedEventData> result)
        {
            result = new FixedValueList1<ManagedEventData>(refEventList.Length);

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
        void LoadTypeEvent(ManagedType type, EventInfo refEvent, out ManagedEventData result)
        {
            result = new ManagedEventData();
            result.Name = refEvent.Name;
            result.Attributes = refEvent.Attributes;
            LoadTypeCustomAttributes(type, refEvent.GetCustomAttributesData(), out result.CustomAttributes);
            result.EventHandlerType = ResolveSignature(refEvent.EventHandlerType!);
        }

        /// <summary>
        /// Loads the parameters from the given collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refParameterList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void LoadTypeParameters(ManagedType type, ParameterInfo[] refParameterList, out FixedValueList4<ManagedParameterData> result)
        {
            result = new FixedValueList4<ManagedParameterData>(refParameterList.Length);

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
        void LoadTypeParameter(ManagedType type, ParameterInfo refParameter, out ManagedParameterData result)
        {
            result = new ManagedParameterData();
            result.Name = refParameter.Name;
            result.Attributes = refParameter.Attributes;
            LoadTypeCustomAttributes(type, refParameter.GetCustomAttributesData(), out result.CustomAttributes);
            result.ParameterType = ResolveSignature(refParameter.ParameterType);
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="refAttributeList"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        void LoadExportedTypeCustomAttributes(ManagedAssembly assembly, IList<CustomAttributeData> refAttributeList, out FixedValueList1<ManagedCustomAttributeData> list)
        {
            list = new FixedValueList1<ManagedCustomAttributeData>(refAttributeList.Count);

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
        void LoadExportedTypeCustomAttribute(ManagedAssembly assembly, CustomAttributeData refAttribute, out ManagedCustomAttributeData result)
        {
            result = new ManagedCustomAttributeData();
        }

    }

}
