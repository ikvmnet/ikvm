using System;

using IKVM.Reflection;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using MethodInfo = IKVM.Reflection.MethodInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    static class IkvmReflectionUtil
    {

        /// <summary>
        /// Converts the <see cref="IKVM.Reflection.AssemblyName"/> to a <see cref="System.Reflection.AssemblyName"/>.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static System.Reflection.AssemblyName ToAssemblyName(this AssemblyName src)
        {
#pragma warning disable SYSLIB0037 // Type or member is obsolete
            return new System.Reflection.AssemblyName()
            {
                Name = src.Name,
                Version = src.Version,
                CultureName = src.CultureName,
                HashAlgorithm = (System.Configuration.Assemblies.AssemblyHashAlgorithm)src.HashAlgorithm,
                Flags = (System.Reflection.AssemblyNameFlags)src.Flags,
                ContentType = (System.Reflection.AssemblyContentType)src.ContentType,
            };
#pragma warning restore SYSLIB0037 // Type or member is obsolete
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static Module Unpack(this IModuleSymbol module)
        {
            return ((IkvmReflectionModuleSymbol)module).ReflectionObject;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public static Module[] Unpack(this IModuleSymbol[] modules)
        {
            var a = new Module[modules.Length];
            for (int i = 0; i < modules.Length; i++)
                a[i] = modules[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type Unpack(this ITypeSymbol type)
        {
            return ((IkvmReflectionTypeSymbol)type).ReflectionObject;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type[] Unpack(this ITypeSymbol[] types)
        {
            var a = new Type[types.Length];
            for (int i = 0; i < types.Length; i++)
                a[i] = types[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static Type[][] Unpack(this ITypeSymbol[][] types)
        {
            var a = new Type[types.Length][];
            for (int i = 0; i < types.Length; i++)
                a[i] = types[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static MemberInfo Unpack(this IMemberSymbol member)
        {
            return ((IkvmReflectionMemberSymbol)member).ReflectionObject;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public static MemberInfo[] Unpack(this IMemberSymbol[] members)
        {
            var a = new MemberInfo[members.Length];
            for (int i = 0; i < members.Length; i++)
                a[i] = members[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static ConstructorInfo Unpack(this IConstructorSymbol ctor)
        {
            return ((IkvmReflectionConstructorSymbol)ctor).ReflectionObject;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static ConstructorInfo[] Unpack(this IConstructorSymbol[] ctor)
        {
            var a = new ConstructorInfo[ctor.Length];
            for (int i = 0; i < ctor.Length; i++)
                a[i] = ctor[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static MethodInfo Unpack(this IMethodSymbol ctor)
        {
            return ((IkvmReflectionMethodSymbol)ctor).ReflectionObject;
        }

        /// <summary>
        /// Unpacks the symbols into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static MethodInfo[] Unpack(this IMethodSymbol[] ctor)
        {
            var a = new MethodInfo[ctor.Length];
            for (int i = 0; i < ctor.Length; i++)
                a[i] = ctor[i].Unpack();

            return a;
        }

        /// <summary>
        /// Unpacks the modifier into their original type.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static ParameterModifier Unpack(this System.Reflection.ParameterModifier modifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unpacks the modifiers into their original types.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public static ParameterModifier[] Unpack(this System.Reflection.ParameterModifier[] ctor)
        {
            var a = new ParameterModifier[ctor.Length];
            for (int i = 0; i < ctor.Length; i++)
                a[i] = ctor[i].Unpack();

            return a;
        }

    }

}
