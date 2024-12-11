using System;

namespace IKVM.CoreLib.Reflection
{

    public static class TypeExtensions
    {

#if NETFRAMEWORK

        /// <summary>
        /// Determines whether the current type can be assigned to a variable of the specified <paramref name="targetType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool IsAssignableTo(this Type type, Type? targetType)
        {
            return targetType != null && targetType.IsAssignableFrom(type);
        }

#endif

    }

}
