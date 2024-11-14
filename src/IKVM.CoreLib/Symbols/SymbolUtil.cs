using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    static class SymbolUtil
    {

        /// <summary>
        /// Returns <c>true</c> if the given binding flags match, combined with the specified state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="flags"></param>
        /// <param name="trueFlag"></param>
        /// <param name="falseFlag"></param>
        /// <returns></returns>
        static bool BindingFlagsMatch(bool state, BindingFlags flags, BindingFlags trueFlag, BindingFlags falseFlag)
        {
            return (state && (flags & trueFlag) == trueFlag) || (!state && (flags & falseFlag) == falseFlag);
        }

        /// <summary>
        /// Evaluates whether the specified <see cref="BindingFlags"/> would match the <see cref="IMethodBaseSymbol"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool BindingFlagsMatch(MethodBaseSymbol member, BindingFlags flags)
        {
            return BindingFlagsMatch(member.IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(member.IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        /// <summary>
        /// Evaluates whether the specified <see cref="BindingFlags"/> would match the <see cref="MethodBaseSymbol"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool BindingFlagsMatchInherited(MethodBaseSymbol method, BindingFlags flags)
        {
            return BindingFlagsMatch(method.IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(method.IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

    }

}
