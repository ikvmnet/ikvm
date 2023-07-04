using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Maintains a context for a specific assembly file.
    /// </summary>
    internal class ReflectionAssemblyContext : IManagedAssemblyContext
    {

        /// <summary>
        /// Loads the given <see cref="Assembly"/> into a new <see cref="ManagedAssembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static ManagedAssembly Load(Assembly assembly) => new ReflectionAssemblyContext(assembly).Assembly;

        readonly Assembly reflectionAssembly;
        readonly ManagedAssembly assembly;
        readonly ConcurrentDictionary<Type, ManagedType> typeCache = new ConcurrentDictionary<Type, ManagedType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectionAssembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionAssemblyContext(Assembly reflectionAssembly)
        {
            this.reflectionAssembly = reflectionAssembly;

            assembly = LoadAssembly();
        }

        /// <summary>
        /// Gets the underlying <see cref="System.Reflection.Assembly"/> source.
        /// </summary>
        public Assembly ReflectionAssembly => reflectionAssembly;

        /// <summary>
        /// Gets the assembly that defines this context.
        /// </summary>
        public ManagedAssembly Assembly => assembly;

        /// <summary>
        /// Implements the IManagedAssemblyContext.ResolveTypes method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        ManagedType? IManagedAssemblyContext.ResolveType(ManagedAssembly assembly, string typeName) => ResolveType(assembly, typeName);

        /// <summary>
        /// Implements the IManagedAssemblyContext.ResolveTypes method.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> IManagedAssemblyContext.ResolveTypes(ManagedAssembly assembly) => ResolveTypes(assembly);

        /// <summary>
        /// Lodas the types from the specified assembly.
        /// </summary>
        /// <param name="reflectionType"></param>
        /// <returns></returns>
        internal IEnumerable<ManagedType> ResolveNestedTypes(Type reflectionType) => ResolveTypes(reflectionType.GetNestedTypes());

        /// <summary>
        /// Lodas the types from the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly)
        {
            if (assembly != this.assembly)
                throw new ManagedTypeException("Invalid context for assembly.");

            return ResolveTypes(reflectionAssembly.GetTypes());
        }

        /// <summary>
        /// Loads the types from the given collection.
        /// </summary>
        /// <param name="reflectionTypes"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(Type[] reflectionTypes)
        {
            var l = new ManagedType[reflectionTypes.Length];

            for (int i = 0; i < reflectionTypes.Length; i++)
                l[i] = ResolveType(reflectionTypes[i]) ?? throw new InvalidOperationException();

            return new ReadOnlyFixedValueList<ManagedType>(l);
        }

        /// <summary>
        /// Resolves the named type from this assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedAssembly assembly, string typeName)
        {
            if (assembly != this.assembly)
                return null;

            var type = reflectionAssembly.GetType(typeName);
            if (type == null)
                return null;

            return ResolveType(type);
        }

        /// <summary>
        /// Resolves the specified type from this assembly.
        /// </summary>
        /// <param name="reflectionType"></param>
        /// <returns></returns>
        ManagedType? ResolveType(Type reflectionType)
        {
            var managedType = typeCache.GetOrAdd(reflectionType, LoadType);
            if (managedType == null)
                return null;

            return managedType;
        }

        /// <summary>
        /// Resolves a reference to the specified type.
        /// </summary>
        /// <param name="reflectionType"></param>
        /// <returns></returns>
        ManagedTypeRef ResolveTypeReference(Type reflectionType)
        {
            return new ManagedTypeRef(this, reflectionType.Assembly.GetName(), reflectionType.FullName!, ResolveType(reflectionType));
        }

        /// <summary>
        /// Resolves a signature from the specified type.
        /// </summary>
        /// <param name="reflectionType"></param>
        /// <returns></returns>
        ManagedTypeSignature ResolveTypeSignature(Type reflectionType)
        {
            if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Void")
                return ManagedTypeSignature.Void;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Boolean")
                return ManagedTypeSignature.Boolean;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Byte")
                return ManagedTypeSignature.Byte;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.SByte")
                return ManagedTypeSignature.SByte;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Char")
                return ManagedTypeSignature.Char;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Int16")
                return ManagedTypeSignature.Int16;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.UInt16")
                return ManagedTypeSignature.UInt16;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Int32")
                return ManagedTypeSignature.Int32;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.UInt32")
                return ManagedTypeSignature.UInt32;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Int64")
                return ManagedTypeSignature.Int64;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.UInt64")
                return ManagedTypeSignature.UInt64;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Single")
                return ManagedTypeSignature.Single;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Double")
                return ManagedTypeSignature.Double;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.IntPtr")
                return ManagedTypeSignature.IntPtr;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.UIntPtr")
                return ManagedTypeSignature.UIntPtr;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.Object")
                return ManagedTypeSignature.Object;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.String")
                return ManagedTypeSignature.String;
            else if (reflectionType.IsPrimitive && reflectionType.FullName == "System.TypedReference")
                return ManagedTypeSignature.TypedReference;
#if NETFRAMEWORK
            else if (reflectionType.IsArray && reflectionType.GetArrayRank() == 1 && reflectionType == reflectionType.GetElementType().MakeArrayType())
#else
            else if (reflectionType.IsSZArray)
#endif
                return ResolveTypeSignature(reflectionType.GetElementType()!).Array();
            else if (reflectionType.IsArray)
                throw new ManagedTypeException("Unsupported multidimensional array type.");
            else if (reflectionType.IsByRef)
                return ResolveTypeSignature(reflectionType.GetElementType()!).ByRef();
            else if (reflectionType.IsPointer)
                return ResolveTypeSignature(reflectionType.GetElementType()!).Pointer();
            else if (reflectionType.IsGenericType)
            {
                var a = reflectionType.GetGenericArguments();
                var l = new FixedValueList<ManagedTypeSignature>(a.Length);

                for (int i = 0; i < a.Length; i++)
                    l[i] = ResolveTypeSignature(a[i]);

                return ResolveTypeSignature(reflectionType.GetGenericTypeDefinition()).Generic(l.AsReadOnly());
            }
            else
                return new ManagedTypeRefSignature(ResolveTypeReference(reflectionType));
        }

        /// <summary>
        /// Loads a managed assembly from the source.
        /// </summary>
        /// <param name="primary"></param>
        /// <returns></returns>
        ManagedAssembly LoadAssembly()
        {
            return new ManagedAssembly(this, assembly.Name, LoadCustomAttributes(reflectionAssembly.GetCustomAttributesData()));
        }

        /// <summary>
        /// Loads the custom attributes from the given collection.
        /// </summary>
        /// <param name="reflectionAttributes"></param>
        /// <param name="handles"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedCustomAttribute> LoadCustomAttributes(IList<CustomAttributeData> reflectionAttributes)
        {
            var l = new ManagedCustomAttribute[reflectionAttributes.Count];

            for (int i = 0; i < reflectionAttributes.Count; i++)
                l[i] = LoadCustomAttribute(reflectionAttributes[i]);

            return new ReadOnlyFixedValueList<ManagedCustomAttribute>(l);
        }

        /// <summary>
        /// Loads the given custom attribute.
        /// </summary>
        /// <param name="reflectionAttribute"></param>
        /// <returns></returns>
        ManagedCustomAttribute LoadCustomAttribute(CustomAttributeData reflectionAttribute)
        {
            return new ManagedCustomAttribute();
        }

        /// <summary>
        /// Loads a <see cref="ManagedType"/> from the given <see cref="Type"/>.
        /// </summary>
        /// <param name="reflectionType"></param>
        /// <returns></returns>
        ManagedType LoadType(Type reflectionType)
        {
            if (reflectionType.IsGenericType)
                throw new ManagedTypeException("Cannot load a generic type.");

            var customAttributes = LoadCustomAttributes(reflectionType.GetCustomAttributesData());
            var genericParameters = LoadGenericParameters(reflectionType.GetGenericArguments());

            return new ManagedType(
                new ReflectionTypeContext(this, reflectionType),
                assembly,
                reflectionType.DeclaringType != null ? ResolveType(reflectionType.DeclaringType) : null,
                reflectionType.FullName!,
                reflectionType.Attributes,
                customAttributes,
                genericParameters,
                reflectionType.BaseType != null ? ResolveTypeReference(reflectionType.BaseType) : null,
                LoadInterfaces(reflectionType.GetInterfaces()),
                LoadFields(reflectionType.GetFields()),
                LoadMethods(reflectionType.GetMethods()));
        }

        /// <summary>
        /// Loads the generic parameters from the given collection.
        /// </summary>
        /// <param name="reflectionFields"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedGenericParameter> LoadGenericParameters(Type[] reflectionInterfaces)
        {
            var l = new FixedValueList<ManagedGenericParameter>(reflectionInterfaces.Length);

            for (int i = 0; i < reflectionInterfaces.Length; i++)
                l[i] = LoadGenericParameter(reflectionInterfaces[i]);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="reflectionGenericParameter"></param>
        /// <returns></returns>
        ManagedGenericParameter LoadGenericParameter(Type reflectionGenericParameter)
        {
            return new ManagedGenericParameter(reflectionGenericParameter.Name);
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="reflectionFields"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedInterface> LoadInterfaces(Type[] reflectionInterfaces)
        {
            var l = new FixedValueList<ManagedInterface>(reflectionInterfaces.Length);

            for (int i = 0; i < reflectionInterfaces.Length; i++)
                l[i] = LoadInterface(reflectionInterfaces[i]);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given interface.
        /// </summary>
        /// <param name="reflectionInterface"></param>
        /// <returns></returns>
        ManagedInterface LoadInterface(Type reflectionInterface)
        {
            return new ManagedInterface(new FixedValueList<ManagedCustomAttribute>(0).AsReadOnly(), ResolveTypeReference(reflectionInterface));
        }

        /// <summary>
        /// Loads the fields from the given collection.
        /// </summary>
        /// <param name="reflectionFields"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedField> LoadFields(FieldInfo[] reflectionFields)
        {
            var l = new FixedValueList<ManagedField>(reflectionFields.Length);

            for (int i = 0; i < reflectionFields.Length; i++)
                l[i] = LoadField(reflectionFields[i]);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given field.
        /// </summary>
        /// <param name="reflectionField"></param>
        /// <returns></returns>
        ManagedField LoadField(FieldInfo reflectionField)
        {
            return new ManagedField(
                reflectionField.Name,
                LoadCustomAttributes(reflectionField.GetCustomAttributesData()),
                reflectionField.Attributes,
                ResolveTypeSignature(reflectionField.FieldType));
        }

        /// <summary>
        /// Loads the methods from the given collection.
        /// </summary>
        /// <param name="reflectionMethods"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedMethod> LoadMethods(MethodBase[] reflectionMethods)
        {
            var l = new FixedValueList<ManagedMethod>(reflectionMethods.Length);

            for (int i = 0; i < reflectionMethods.Length; i++)
                l[i] = LoadMethod(reflectionMethods[i]);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given method.
        /// </summary>
        /// <param name="reflectionMethod"></param>
        /// <returns></returns>
        ManagedMethod LoadMethod(MethodBase reflectionMethod)
        {
            if (reflectionMethod is ConstructorInfo ctor)
                return new ManagedMethod(
                    ctor.Name,
                    LoadCustomAttributes(reflectionMethod.GetCustomAttributesData()),
                    ctor.Attributes,
                    ctor.MethodImplementationFlags,
                    ManagedTypeSignature.Void,
                    LoadParameters(ctor.GetParameters()));
            else if (reflectionMethod is MethodInfo method)
                return new ManagedMethod(
                    method.Name,
                    LoadCustomAttributes(reflectionMethod.GetCustomAttributesData()),
                    method.Attributes,
                    method.MethodImplementationFlags,
                    ResolveTypeSignature(method.ReturnType),
                    LoadParameters(method.GetParameters()));
            else
                throw new ManagedTypeException();
        }

        /// <summary>
        /// Loads the methods from the given collection.
        /// </summary>
        /// <param name="reflectionMethods"></param>
        /// <returns></returns>
        ReadOnlyFixedValueList<ManagedParameter> LoadParameters(ParameterInfo[] reflectionParameters)
        {
            var l = new FixedValueList<ManagedParameter>(reflectionParameters.Length);

            for (int i = 0; i < reflectionParameters.Length; i++)
                l[i] = LoadParameter(reflectionParameters[i]);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Loads the given parameter.
        /// </summary>
        /// <param name="reflectionParameter"></param>
        /// <returns></returns>
        ManagedParameter LoadParameter(ParameterInfo reflectionParameter)
        {
            return new ManagedParameter(
                reflectionParameter.Name,
                reflectionParameter.Attributes,
                LoadCustomAttributes(reflectionParameter.GetCustomAttributesData()),
                ResolveTypeSignature(reflectionParameter.ParameterType));

        }

    }

}
