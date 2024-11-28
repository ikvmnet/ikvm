using System;
using System.Collections.Immutable;
using System.Linq;

using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    static class IkvmReflectionUtil
    {

        /// <summary>
        /// Converts a <see cref="AssemblyName"/> to a <see cref="AssemblyIdentity"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static AssemblyIdentity Pack(this AssemblyName assemblyName)
        {
            var pk = assemblyName.GetPublicKey()?.ToImmutableArray() ?? ImmutableArray<byte>.Empty;
            var hasPublicKey = pk.Length > 0;
            var pkt = assemblyName.GetPublicKeyToken()?.ToImmutableArray() ?? ImmutableArray<byte>.Empty;

            return new AssemblyIdentity(
                assemblyName.Name ?? throw new InvalidOperationException(),
                assemblyName.Version,
                assemblyName.CultureName,
                hasPublicKey ? pk : pkt,
                hasPublicKey,
                (System.Reflection.AssemblyContentType)assemblyName.ContentType,
                (System.Reflection.ProcessorArchitecture)assemblyName.ProcessorArchitecture);
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
                a.Add(assemblyNames[i].Pack());

            return a.ToImmutable();
        }

        /// <summary>
        /// Converts a <see cref="AssemblyIdentity"/> to a <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyNameInfo"></param>
        /// <returns></returns>
        public static AssemblyName ToAssemblyName(this AssemblyIdentity assemblyNameInfo)
        {
            AssemblyName assemblyName = new();
            assemblyName.Name = assemblyNameInfo.Name;
            assemblyName.CultureName = assemblyNameInfo.CultureName;
            assemblyName.Version = assemblyNameInfo.Version;
            assemblyName.Flags = (AssemblyNameFlags)assemblyNameInfo.Flags;
            assemblyName.ContentType = (AssemblyContentType)assemblyNameInfo.ContentType;
            assemblyName.ProcessorArchitecture = (ProcessorArchitecture)assemblyNameInfo.ProcessorArchitecture;

            if (assemblyNameInfo.HasPublicKey)
                assemblyName.SetPublicKey(assemblyNameInfo.PublicKey.ToArray());
            else if (assemblyNameInfo.PublicKeyToken.IsDefaultOrEmpty == false)
                assemblyName.SetPublicKeyToken(assemblyNameInfo.PublicKeyToken.ToArray());

            return assemblyName;
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
                a[i] = ToAssemblyName(assemblyNameInfos[i]);

            return a;
        }

        /// <summary>
        /// Returns <c>true</c> if the given type represents a type definition.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeDefinition(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return type.HasElementType == false && type.IsConstructedGenericType == false && type.IsGenericParameter == false;
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
