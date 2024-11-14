using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    static class ReflectionUtil
    {

        /// <summary>
        /// Converts a <see cref="AssemblyName"/> to a <see cref="AssemblyIdentity"/>.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static AssemblyIdentity Pack(this AssemblyName assemblyName)
        {
            var pk = assemblyName.GetPublicKey()?.ToImmutableArray() ?? ImmutableArray<byte>.Empty;
            var hasPublicKey = pk.Length > 0;
            var pkt = assemblyName.GetPublicKeyToken()?.ToImmutableArray() ?? ImmutableArray<byte>.Empty;

#pragma warning disable SYSLIB0037 // Type or member is obsolete
            return new AssemblyIdentity(
                assemblyName.Name ?? throw new InvalidOperationException(),
                assemblyName.Version,
                assemblyName.CultureName,
                hasPublicKey ? pk : pkt,
                hasPublicKey,
                assemblyName.ContentType,
                assemblyName.ProcessorArchitecture);
#pragma warning restore SYSLIB0037 // Type or member is obsolete
        }

        /// <summary>
        /// Converts a set of <see cref="AssemblyName"/> to a set of <see cref="AssemblyIdentity"/>.
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static ImmutableArray<AssemblyIdentity> Pack(this AssemblyName[] assemblyNames)
        {
            if (assemblyNames.Length == 0)
                return [];

            var a = ImmutableArray.CreateBuilder<AssemblyIdentity>(assemblyNames.Length);
            for (int i = 0; i < assemblyNames.Length; i++)
                a[i] = assemblyNames[i].Pack();

            return a.ToImmutable();
        }

        /// <summary>
        /// Converts a <see cref="AssemblyIdentity"/> to a <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyNameInfo"></param>
        /// <returns></returns>
        public static AssemblyName Unpack(this AssemblyIdentity assemblyNameInfo)
        {
            return assemblyNameInfo.ToAssemblyName();
        }

        /// <summary>
        /// Converts a set of <see cref="AssemblyIdentity"/> to a set of <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyNameInfos"></param>
        /// <returns></returns>
        public static AssemblyName[] Unpack(this AssemblyIdentity[] assemblyNameInfos)
        {
            if (assemblyNameInfos.Length == 0)
                return [];

            var a = new AssemblyName[assemblyNameInfos.Length];
            for (int i = 0; i < assemblyNameInfos.Length; i++)
                a[i] = assemblyNameInfos[i].Unpack();

            return a;
        }

        ///// <summary>
        ///// Unpacks the <see cref="CustomAttribute"/>.
        ///// </summary>
        ///// <param name="customAttributes"></param>
        ///// <returns></returns>
        //public static CustomAttributeBuilder Unpack(this CustomAttribute attributes)
        //{
        //    // unpack constructor arg values
        //    var constructorValues = new object?[attributes.ConstructorArguments.Length];
        //    for (int i = 0; i < attributes.ConstructorArguments.Length; i++)
        //        constructorValues[i] = UnpackCustomAttributeValue(attributes.ConstructorArguments[i]);

        //    // unpack named arguments into sets of properties and fields
        //    List<PropertyInfo>? namedProperties = null;
        //    List<object?>? propertyValues = null;
        //    List<FieldInfo>? namedFields = null;
        //    List<object?>? fieldValues = null;

        //    // split named arguments into two sets
        //    foreach (var i in attributes.NamedArguments)
        //    {
        //        var v = UnpackCustomAttributeValue(i.TypedValue.Value);
        //        if (i.IsField == false)
        //        {
        //            namedProperties ??= new();
        //            namedProperties.Add(((ReflectionPropertySymbol)i.MemberInfo).Unpack());
        //            propertyValues ??= new();
        //            propertyValues.Add(v);
        //        }
        //        else
        //        {
        //            namedFields ??= new();
        //            namedFields.Add(((FieldSymbol)i.MemberInfo).Unpack());
        //            fieldValues ??= new();
        //            fieldValues.Add(v);
        //        }
        //    }

        //    return new CustomAttributeBuilder(
        //        attributes.Constructor.Unpack(),
        //        constructorValues,
        //        namedProperties?.ToArray() ?? [],
        //        propertyValues?.ToArray() ?? [],
        //        namedFields?.ToArray() ?? [],
        //        fieldValues?.ToArray() ?? []);
        //}

        ///// <summary>
        ///// Unpacks the <see cref="CustomAttribute"/>s.
        ///// </summary>
        ///// <param name="customAttributes"></param>
        ///// <returns></returns>
        //public static CustomAttributeBuilder[] Unpack(this CustomAttribute[] customAttributes)
        //{
        //    if (customAttributes.Length == 0)
        //        return [];

        //    var a = new CustomAttributeBuilder[customAttributes.Length];
        //    for (int i = 0; i < customAttributes.Length; i++)
        //        a[i] = customAttributes[i].Unpack();

        //    return a;
        //}

        ///// <summary>
        ///// Unpacks the <see cref="CustomAttribute"/>s.
        ///// </summary>
        ///// <param name="attributes"></param>
        ///// <returns></returns>
        //public static CustomAttributeBuilder[] Unpack(this ImmutableArray<CustomAttribute> attributes)
        //{
        //    if (attributes.Length == 0)
        //        return [];

        //    var a = new CustomAttributeBuilder[attributes.Length];
        //    for (int i = 0; i < attributes.Length; i++)
        //        a[i] = attributes[i].Unpack();

        //    return a;
        //}

        ///// <summary>
        ///// Unpacks the attribute value object.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //static object? UnpackCustomAttributeValue(object? value)
        //{
        //    return value switch
        //    {
        //        IReadOnlyList<object> l => UnpackCustomAttributeArrayValue(l),
        //        TypeSymbol t => t.Unpack(),
        //        System.Reflection.CustomAttributeTypedArgument a => UnpackCustomAttributeValue(a.Value),
        //        _ => value,
        //    };
        //}

        ///// <summary>
        ///// Unpacks the attribute value array.
        ///// </summary>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //static object?[] UnpackCustomAttributeArrayValue(IReadOnlyList<object> items)
        //{
        //    var a = new object?[items.Count];
        //    for (int i = 0; i < items.Count; i++)
        //        a[i] = UnpackCustomAttributeValue(items[i]);

        //    return a;
        //}

        /// <summary>
        /// Returns <c>true</c> if the given type represents a type definition.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeDefinition(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

#if NET
            return type.IsTypeDefinition;
#else
            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
#endif
        }

        /// <summary>
        /// Substitutes generic type parameters into the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public static Type SubstituteGenericTypes(this Type type, Type[] genericTypeArguments)
        {
            if (type.IsGenericParameter)
                return genericTypeArguments[type.GenericParameterPosition];

            if (type.IsGenericType)
            {
                var anySubstituted = false;
                var typeDef = type.GetGenericTypeDefinition();
                var genericTypeParameters = typeDef.GetGenericArguments();

                for (int i = 0; i < genericTypeParameters.Length; i++)
                {
                    var typeParameter = SubstituteGenericTypes(genericTypeParameters[i], genericTypeArguments);
                    if (genericTypeParameters[i] != typeParameter)
                    {
                        genericTypeParameters[i] = typeParameter;
                        anySubstituted = true;
                    }
                }

                // if any parameters were substituted
                if (anySubstituted)
                    return typeDef.MakeGenericType(genericTypeParameters);
            }

            return type;
        }

    }

}
