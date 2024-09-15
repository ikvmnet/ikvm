using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    static class ReflectionUtil
    {

        /// <summary>
        /// Unpacks the symbol into their original type.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static Module Unpack(this IModuleSymbol module)
        {
            return ((ReflectionModuleSymbol)module).ReflectionObject;
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
            return ((ReflectionTypeSymbol)type).ReflectionObject;
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
            return ((ReflectionMemberSymbol)member).ReflectionObject;
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
            return ((ReflectionConstructorSymbol)ctor).ReflectionObject;
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
            return ((ReflectionMethodSymbol)ctor).ReflectionObject;
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

    }

}
