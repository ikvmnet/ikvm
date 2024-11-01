using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using IKVM.CoreLib.Collections;

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
        public static bool BindingFlagsMatch(IMethodSymbol member, BindingFlags flags)
        {
            return BindingFlagsMatch(member.IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(member.IsStatic, flags, BindingFlags.Static, BindingFlags.Instance);
        }

        /// <summary>
        /// Evaluates whether the specified <see cref="BindingFlags"/> would match the <see cref="IMethodBaseSymbol"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool BindingFlagsMatchInherited(IMethodSymbol method, BindingFlags flags)
        {
            return BindingFlagsMatch(method.IsPublic, flags, BindingFlags.Public, BindingFlags.NonPublic)
                && BindingFlagsMatch(method.IsStatic, flags, BindingFlags.Static | BindingFlags.FlattenHierarchy, BindingFlags.Instance);
        }

        /// <summary>
        /// Selects the single method that corresponds to the given query. Throws an <see cref="AmbiguousMatchException"/> if multiple candidates are found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="name"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AmbiguousMatchException"></exception>
        public static IMethodSymbol? SelectMethod(ITypeSymbol type, BindingFlags bindingFlags, string? name, IReadOnlyList<ITypeSymbol>? parameterTypes, IReadOnlyList<ParameterModifier>? modifiers)
        {
            var list = SelectMethods(type, bindingFlags, name, parameterTypes, modifiers);
            if (list == null)
                throw new InvalidOperationException();

            return list.Count switch
            {
                0 => null,
                1 => list[0],
                _ => throw new AmbiguousMatchException(),
            };
        }

        /// <summary>
        /// Selects the methods that corresponds to the given query.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static IImmutableList<IMethodSymbol> SelectMethods(ITypeSymbol type, BindingFlags bindingFlags, string? name, IReadOnlyList<ITypeSymbol>? parameterTypes, IReadOnlyList<ParameterModifier>? modifiers)
        {
            // we start by copying the declared methods and either removing or adding
            // optimally, if doing no work, we end up returning the same list
            // but we might also be appending only, thus preserving some space thanks to the immutable collection
            var list = type.GetDeclaredMethods();

            // remove items that do not match binding flags
            foreach (var declaredMethod in list)
                if (BindingFlagsMatch(declaredMethod, bindingFlags) == false)
                    list = list.Remove(declaredMethod);

            // we do want to check inherited methods
            if ((bindingFlags & BindingFlags.DeclaredOnly) == 0)
            {
                // keep a record of those methods which have been preserved
                var baseMethods = new HashSet<IMethodSymbol>();

                // record base methods of already emitted virtual methods
                foreach (var method in list)
                    if (method.IsVirtual)
                        baseMethods.Add(method.GetBaseDefinition());

                for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
                {
                    var baseTypeMethods = baseType.GetDeclaredMethods();
                    if (baseTypeMethods == null)
                        throw new InvalidOperationException();

                    // remove base methods that do not match inherited binding flags
                    // remove base methods that are virtual and already tracked
                    foreach (var baseTypeMethod in baseTypeMethods)
                        if (BindingFlagsMatchInherited(baseTypeMethod, bindingFlags) == false || baseTypeMethod.IsVirtual && baseMethods.Contains(baseTypeMethod.GetBaseDefinition()))
                            baseTypeMethods = baseTypeMethods.Remove(baseTypeMethod);

                    // append to emitted list
                    list = list.AddRange(baseTypeMethods);
                }
            }

            // remove methods that do not match filter conditions
            foreach (var method in list)
                if (MethodFilterPredicate(method, bindingFlags, name, parameterTypes, modifiers) == false)
                    list = list.Remove(method);

            // return final list
            return list;
        }

        /// <summary>
        /// Predicate that determines if the method matches the specification.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="name"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        static bool MethodFilterPredicate(IMethodSymbol method, BindingFlags bindingFlags, string? name, IReadOnlyList<ITypeSymbol>? parameterTypes, IReadOnlyList<ParameterModifier>? modifiers)
        {
            if (name != null)
            {
                var nameComparison = StringComparison.Ordinal;
                if ((bindingFlags & BindingFlags.IgnoreCase) != 0)
                    nameComparison = StringComparison.OrdinalIgnoreCase;
                if (string.Equals(method.Name, name, nameComparison) == false)
                    return false;
            }

            if (parameterTypes != null)
                if (MatchParameterTypes(method, parameterTypes) == false)
                    return false;

            if (modifiers != null)
                if (MatchModifiers(method, modifiers) == false)
                    return false;

            return true;
        }

        /// <summary>
        /// Checks that the parameters types of the method match the given parameter types.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        static bool MatchParameterTypes(IMethodSymbol method, IReadOnlyList<ITypeSymbol> parameterTypes)
        {
            var methodParameters = method.GetParameters();
            if (methodParameters.Length != parameterTypes.Count)
                return false;

            for (int i = 0; i < methodParameters.Length; i++)
                if (methodParameters[i].ParameterType != parameterTypes[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Checks that the parameter modifiers match. For now does nothing.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        static bool MatchModifiers(IMethodSymbol method, IReadOnlyList<ParameterModifier> modifiers)
        {
            return true;
        }

    }

}
