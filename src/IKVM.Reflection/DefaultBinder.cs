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

    sealed class DefaultBinder : Binder
    {

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
            for (int i = 1; i < matchCount; i++)
                SelectBestMatch(match[i], types, ref bestMatch, ref ambiguous);

            if (ambiguous)
                throw new AmbiguousMatchException();

            return bestMatch;
        }

        static bool MatchParameterTypes(ParameterInfo[] parameters, Type[] types)
        {
            if (parameters.Length != types.Length)
                return false;

            for (int i = 0; i < parameters.Length; i++)
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
                int depth1 = GetInheritanceDepth(currentBest.DeclaringType);
                int depth2 = GetInheritanceDepth(candidate.DeclaringType);
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

        static int GetInheritanceDepth(Type type)
        {
            int depth = 0;
            while (type != null)
            {
                depth++;
                type = type.BaseType;
            }
            return depth;
        }

        static int MatchSignatures(MethodBase mb1, MethodBase mb2, Type[] types)
        {
            var sig1 = mb1.MethodSignature;
            var sig2 = mb2.MethodSignature;
            var gb1 = mb1 as IGenericBinder ?? mb1.DeclaringType;
            var gb2 = mb2 as IGenericBinder ?? mb2.DeclaringType;
            for (int i = 0; i < sig1.GetParameterCount(); i++)
            {
                var type1 = sig1.GetParameterType(gb1, i);
                var type2 = sig2.GetParameterType(gb2, i);
                if (type1 != type2)
                    return MatchTypes(type1, type2, types[i]);
            }

            return 0;
        }

        static int MatchSignatures(PropertySignature sig1, PropertySignature sig2, Type[] types)
        {
            for (int i = 0; i < sig1.ParameterCount; i++)
            {
                var type1 = sig1.GetParameter(i);
                var type2 = sig2.GetParameter(i);
                if (type1 != type2)
                    return MatchTypes(type1, type2, types[i]);
            }

            return 0;
        }

        static int MatchTypes(Type type1, Type type2, Type type)
        {
            if (type1 == type)
                return 1;
            if (type2 == type)
                return 2;

            var conv = type1.IsAssignableFrom(type2);
            return conv == type2.IsAssignableFrom(type1) ? 0 : conv ? 2 : 1;
        }

        static bool IsAllowedPrimitiveConversion(Type source, Type target)
        {
            // we need to check for primitives, because GetTypeCode will return the underlying type for enums
            if (!source.IsPrimitive || !target.IsPrimitive)
                return false;

            var sourceType = Type.GetTypeCode(source);
            var targetType = Type.GetTypeCode(target);
            switch (sourceType)
            {
                case TypeCode.Char:
                    switch (targetType)
                    {
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.Int32:
                        case TypeCode.UInt64:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.Byte:
                    switch (targetType)
                    {
                        case TypeCode.Char:
                        case TypeCode.UInt16:
                        case TypeCode.Int16:
                        case TypeCode.UInt32:
                        case TypeCode.Int32:
                        case TypeCode.UInt64:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.SByte:
                    switch (targetType)
                    {
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.UInt16:
                    switch (targetType)
                    {
                        case TypeCode.UInt32:
                        case TypeCode.Int32:
                        case TypeCode.UInt64:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.Int16:
                    switch (targetType)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.UInt32:
                    switch (targetType)
                    {
                        case TypeCode.UInt64:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.Int32:
                    switch (targetType)
                    {
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.UInt64:
                    switch (targetType)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.Int64:
                    switch (targetType)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                case TypeCode.Single:
                    switch (targetType)
                    {
                        case TypeCode.Double:
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

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
            for (int i = 1; i < matchCount; i++)
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
