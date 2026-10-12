/*
  Copyright (C) 2010-2012 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;

namespace IKVM.Reflection
{

    /// <summary>
    /// The binder used when none is supplied. Selects among overloaded methods and properties the way the default
    /// System.Reflection binder does: parameters must accept the argument types, by identity, assignability or a
    /// widening primitive conversion, and the most specific match wins.
    /// </summary>
    sealed class DefaultBinder : Binder
    {

        /// <summary>
        /// Selects a method from the given set of methods, based on the argument types.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values.</param>
        /// <param name="match">
        /// The set of methods that are candidates for matching. The array is overwritten with the candidates whose
        /// parameters accept <paramref name="types"/>.
        /// </param>
        /// <param name="types">The parameter values used to locate a matching method.</param>
        /// <param name="modifiers">An array of parameter modifiers; not used.</param>
        /// <returns>The matching method, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">More than one method is equally good to match <paramref name="types"/>.</exception>
        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
        {
            var matchCount = 0;
            foreach (var method in match)
                if (MatchParameterTypes(method.GetParameters(), types))
                    match[matchCount++] = method;

            if (matchCount == 0)
                return null;

            var bestMatch = match[0];
            var ambiguous = false;
            for (var i = 1; i < matchCount; i++)
                SelectBestMatch(match[i], types, ref bestMatch, ref ambiguous);

            if (ambiguous)
                throw new AmbiguousMatchException();

            return bestMatch;
        }

        /// <summary>
        /// Determines whether each parameter accepts the argument type at the same position, by identity,
        /// assignability or a widening primitive conversion.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="types">The argument types.</param>
        /// <returns><see langword="true"/> if the counts are equal and every parameter accepts its argument type; otherwise, <see langword="false"/>.</returns>
        static bool MatchParameterTypes(ParameterInfo[] parameters, Type[] types)
        {
            if (parameters.Length != types.Length)
                return false;

            for (var i = 0; i < parameters.Length; i++)
            {
                var sourceType = types[i];
                var targetType = parameters[i].ParameterType;
                if (sourceType != targetType
                    && !targetType.IsAssignableFrom(sourceType)
                    && !IsAllowedPrimitiveConversion(sourceType, targetType))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares a candidate method with the current best match, and makes the candidate the best match if it is
        /// more specific. When neither is more specific by parameter types, a method with identical parameter types
        /// declared in a more derived type wins; otherwise the match is marked ambiguous.
        /// </summary>
        /// <param name="candidate">The candidate method.</param>
        /// <param name="types">The argument types.</param>
        /// <param name="currentBest">The current best match, replaced if the candidate is better.</param>
        /// <param name="ambiguous">Set when the two methods cannot be ordered, and cleared when a new best match is chosen.</param>
        static void SelectBestMatch(MethodBase candidate, Type[] types, ref MethodBase currentBest, ref bool ambiguous)
        {
            switch (MatchSignatures(currentBest, candidate, types))
            {
                case 1:
                    return;
                case 2:
                    ambiguous = false;
                    currentBest = candidate;
                    return;
            }

            if (currentBest.MethodSignature.MatchParameterTypes(candidate.MethodSignature))
            {
                var depth1 = GetInheritanceDepth(currentBest.DeclaringType);
                var depth2 = GetInheritanceDepth(candidate.DeclaringType);
                if (depth1 > depth2)
                {
                    return;
                }
                else if (depth1 < depth2)
                {
                    ambiguous = false;
                    currentBest = candidate;
                    return;
                }
            }

            ambiguous = true;
        }

        /// <summary>
        /// Counts the number of types in the inheritance chain of a type, including the type itself.
        /// </summary>
        /// <param name="type">The type, or <see langword="null"/>.</param>
        /// <returns>The inheritance depth, or zero for <see langword="null"/>.</returns>
        static int GetInheritanceDepth(Type type)
        {
            var depth = 0;
            while (type != null)
            {
                depth++;
                type = type.BaseType;
            }
            return depth;
        }

        /// <summary>
        /// Compares the parameter types of two methods, at the first position where they differ.
        /// </summary>
        /// <param name="mb1">The first method.</param>
        /// <param name="mb2">The second method.</param>
        /// <param name="types">The argument types.</param>
        /// <returns>1 if the first method is the better match, 2 if the second is, or 0 if neither is.</returns>
        static int MatchSignatures(MethodBase mb1, MethodBase mb2, Type[] types)
        {
            var sig1 = mb1.MethodSignature;
            var sig2 = mb2.MethodSignature;
            var gb1 = mb1 as IGenericBinder ?? mb1.DeclaringType;
            var gb2 = mb2 as IGenericBinder ?? mb2.DeclaringType;
            for (var i = 0; i < sig1.GetParameterCount(); i++)
            {
                var type1 = sig1.GetParameterType(gb1, i);
                var type2 = sig2.GetParameterType(gb2, i);
                if (type1 != type2)
                    return MatchTypes(type1, type2, types[i]);
            }

            return 0;
        }

        /// <summary>
        /// Compares the index parameter types of two properties, at the first position where they differ.
        /// </summary>
        /// <param name="sig1">The signature of the first property.</param>
        /// <param name="sig2">The signature of the second property.</param>
        /// <param name="types">The index argument types.</param>
        /// <returns>1 if the first property is the better match, 2 if the second is, or 0 if neither is.</returns>
        static int MatchSignatures(PropertySignature sig1, PropertySignature sig2, Type[] types)
        {
            for (var i = 0; i < sig1.ParameterCount; i++)
            {
                var type1 = sig1.GetParameter(i);
                var type2 = sig2.GetParameter(i);
                if (type1 != type2)
                    return MatchTypes(type1, type2, types[i]);
            }

            return 0;
        }

        /// <summary>
        /// Compares two parameter types for an argument type. A type identical to the argument type wins; otherwise the
        /// more specific type, the one assignable to the other, wins.
        /// </summary>
        /// <param name="type1">The first parameter type.</param>
        /// <param name="type2">The second parameter type.</param>
        /// <param name="type">The argument type.</param>
        /// <returns>1 if the first type is the better match, 2 if the second is, or 0 if neither is.</returns>
        static int MatchTypes(Type type1, Type type2, Type type)
        {
            if (type1 == type)
                return 1;
            if (type2 == type)
                return 2;

            var conv = type1.IsAssignableFrom(type2);
            return conv == type2.IsAssignableFrom(type1) ? 0 : conv ? 2 : 1;
        }

        /// <summary>
        /// Determines whether a value of one primitive type can be passed for another by a widening conversion.
        /// </summary>
        /// <param name="source">The argument type.</param>
        /// <param name="target">The parameter type.</param>
        /// <returns>
        /// <see langword="true"/> if both types are primitive and <paramref name="source"/> widens to
        /// <paramref name="target"/>; otherwise, <see langword="false"/>.
        /// </returns>
        static bool IsAllowedPrimitiveConversion(Type source, Type target)
        {
            // we need to check for primitives, because GetTypeCode will return the underlying type for enums
            if (!source.IsPrimitive || !target.IsPrimitive)
                return false;

            var sourceType = Type.GetTypeCode(source);
            var targetType = Type.GetTypeCode(target);
            return sourceType switch
            {
                TypeCode.Char => targetType switch
                {
                    TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.Int32 or TypeCode.UInt64 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.Byte => targetType switch
                {
                    TypeCode.Char or TypeCode.UInt16 or TypeCode.Int16 or TypeCode.UInt32 or TypeCode.Int32 or TypeCode.UInt64 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.SByte => targetType switch
                {
                    TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.UInt16 => targetType switch
                {
                    TypeCode.UInt32 or TypeCode.Int32 or TypeCode.UInt64 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.Int16 => targetType switch
                {
                    TypeCode.Int32 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.UInt32 => targetType switch
                {
                    TypeCode.UInt64 or TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.Int32 => targetType switch
                {
                    TypeCode.Int64 or TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.UInt64 => targetType switch
                {
                    TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.Int64 => targetType switch
                {
                    TypeCode.Single or TypeCode.Double => true,
                    _ => false,
                },
                TypeCode.Single => targetType switch
                {
                    TypeCode.Double => true,
                    _ => false,
                },
                _ => false,
            };
        }

        /// <summary>
        /// Selects a property from the given set of properties, based on the specified criteria.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values.</param>
        /// <param name="match">
        /// The set of properties that are candidates for matching. The array is overwritten with the candidates that
        /// meet the criteria.
        /// </param>
        /// <param name="returnType">The return value the matching property must have, or <see langword="null"/>.</param>
        /// <param name="indexes">The index types of the property being searched for, or <see langword="null"/>.</param>
        /// <param name="modifiers">An array of parameter modifiers; not used.</param>
        /// <returns>The matching property, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">More than one property is equally good to match the criteria.</exception>
        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            var matchCount = 0;

            foreach (var property in match)
            {
                if (indexes == null || MatchParameterTypes(property.GetIndexParameters(), indexes))
                {
                    if (returnType != null)
                    {
                        if (property.PropertyType.IsPrimitive)
                        {
                            if (!IsAllowedPrimitiveConversion(returnType, property.PropertyType))
                                continue;
                        }
                        else
                        {
                            if (!property.PropertyType.IsAssignableFrom(returnType))
                                continue;
                        }
                    }

                    match[matchCount++] = property;
                }
            }

            if (matchCount == 0)
                return null;

            if (matchCount == 1)
                return match[0];

            var bestMatch = match[0];
            var ambiguous = false;
            for (var i = 1; i < matchCount; i++)
            {
                var best = MatchTypes(bestMatch.PropertyType, match[i].PropertyType, returnType);

                if (best == 0 && indexes != null)
                    best = MatchSignatures(bestMatch.PropertySignature, match[i].PropertySignature, indexes);

                if (best == 0)
                {
                    var depth1 = GetInheritanceDepth(bestMatch.DeclaringType);
                    var depth2 = GetInheritanceDepth(match[i].DeclaringType);
                    if (bestMatch.Name == match[i].Name && depth1 != depth2)
                    {
                        if (depth1 > depth2)
                            best = 1;
                        else
                            best = 2;
                    }
                    else
                    {
                        ambiguous = true;
                    }
                }

                if (best == 2)
                {
                    ambiguous = false;
                    bestMatch = match[i];
                }
            }

            if (ambiguous)
                throw new AmbiguousMatchException();

            return bestMatch;
        }

    }

}
