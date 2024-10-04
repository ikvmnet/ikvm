using System.Collections.Generic;

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
        static bool BindingFlagsMatch(bool state, System.Reflection.BindingFlags flags, System.Reflection.BindingFlags trueFlag, System.Reflection.BindingFlags falseFlag)
        {
            return (state && (flags & trueFlag) == trueFlag) || (!state && (flags & falseFlag) == falseFlag);
        }

        /// <summary>
        /// Evaluates whether the specified <see cref="System.Reflection.BindingFlags"/> would match the <see cref="IMethodBaseSymbol"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool BindingFlagsMatch(IMethodSymbol member, System.Reflection.BindingFlags flags)
        {
            return BindingFlagsMatch(member.IsPublic, flags, System.Reflection.BindingFlags.Public, System.Reflection.BindingFlags.NonPublic)
                && BindingFlagsMatch(member.IsStatic, flags, System.Reflection.BindingFlags.Static, System.Reflection.BindingFlags.Instance);
        }

        /// <summary>
        /// Evaluates whether the specified <see cref="System.Reflection.BindingFlags"/> would match the <see cref="IMethodBaseSymbol"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool BindingFlagsMatchInherited(IMethodSymbol method, System.Reflection.BindingFlags flags)
        {
            return BindingFlagsMatch(method.IsPublic, flags, System.Reflection.BindingFlags.Public, System.Reflection.BindingFlags.NonPublic)
                && BindingFlagsMatch(method.IsStatic, flags, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy, System.Reflection.BindingFlags.Instance);
        }

        /// <summary>
        /// Filters the set of methods by the binding attributes.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IMethodSymbol> FilterMethods(ITypeSymbol type, IEnumerable<IMethodSymbol> methods, System.Reflection.BindingFlags bindingAttr = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance)
        {
            var list = new HashSet<IMethodSymbol>();

            foreach (var mb in methods)
            {
                var mi = mb as IMethodSymbol;
                if (mi != null && BindingFlagsMatch(mi, bindingAttr))
                    if (list.Add(mi))
                        yield return mi;
            }

            if ((bindingAttr & System.Reflection.BindingFlags.DeclaredOnly) == 0)
            {
                var baseMethods = new List<IMethodSymbol>();
                foreach (var mi in list)
                    if (mi.IsVirtual)
                        baseMethods.Add(mi.GetBaseDefinition());

                for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
                {
                    foreach (var mi in baseType.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance))
                    {
                        if (BindingFlagsMatchInherited(mi, bindingAttr))
                        {
                            if (mi.IsVirtual)
                            {
                                baseMethods ??= [];
                                if (baseMethods.Contains(mi.GetBaseDefinition()))
                                    continue;

                                baseMethods.Add(mi.GetBaseDefinition());
                            }

                            if (list.Add(mi))
                                yield return mi;
                        }
                    }
                }
            }
        }

    }

}
