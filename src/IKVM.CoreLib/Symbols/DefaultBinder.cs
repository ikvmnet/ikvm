using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides methods to select various symbols based on requirements.
    /// </summary>
    internal class DefaultBinder
    {

        readonly SymbolContext _context;

        TypeSymbol? _lazyObjectType;
        TypeSymbol? _lazyIntPtrType;
        TypeSymbol? _lazyUIntPtrType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefaultBinder(SymbolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the symbol for <see cref="Object"/>.
        /// </summary>
        TypeSymbol ObjectType => _lazyObjectType ??= _context.ResolveCoreType("System.Object");

        /// <summary>
        /// Gets the symbol for <see cref="IntPtr"/>.
        /// </summary>
        TypeSymbol IntPtrType => _lazyIntPtrType ??= _context.ResolveCoreType("System.IntPtr");

        /// <summary>
        /// Gets the symbol for <see cref="UIntPtr"/>.
        /// </summary>
        TypeSymbol UIntPtrType => _lazyUIntPtrType ??= _context.ResolveCoreType("System.UIntPtr");

        /// <summary>
        /// Given a set of methods that match the base criteria, select a method based upon an array of parameter types. This
        /// method should return null if no method matches the criteria.
        /// </summary>
        public MethodBaseSymbol? SelectMethod(IReadOnlyList<MethodBaseSymbol> match, BindingFlags bindingFlags, ImmutableArray<TypeSymbol> types, ImmutableArray<ParameterModifier>? modifiers)
        {
            // we don't automatically jump out on exact match
            if (match == null || match.Count == 0)
                throw new ArgumentException("Unexpected empty array.", nameof(match));

            var candidates = new List<MethodBaseSymbol>(match);

            // find all the methods that can be described by the types parameter
            // remove all of them that cannot
            int curIdx = 0;
            for (var i = 0; i < candidates.Count; i++)
            {
                var par = candidates[i].GetParameters();
                if (par.Length != types.Length)
                    continue;

                int j;
                for (j = 0; j < types.Length; j++)
                {
                    var type = types[j];
                    var parameterType = par[j].ParameterType;

                    // exact parameter type match
                    if (type == parameterType)
                        continue;

                    // everything is convertable to object
                    if (parameterType == ObjectType)
                        continue;

                    // primitive paramter that can't be converted to parameter type
                    if (parameterType.IsPrimitive && CanChangePrimitive(type, parameterType) == false)
                        break;

                    // can't otherwise assign type to parameter type
                    if (parameterType.IsAssignableFrom(type) == false)
                        break;
                }

                if (j == types.Length)
                    candidates[curIdx++] = candidates[i];
            }

            if (curIdx == 0)
                return null;
            if (curIdx == 1)
                return candidates[0];

            // walk all of the methods looking the most specific method to invoke
            int currentMin = 0;
            var ambig = false;

            var paramOrder = types.Length > 0 ? stackalloc int[types.Length] : Array.Empty<int>();
            for (var i = 0; i < types.Length; i++)
                paramOrder[i] = i;

            for (var i = 1; i < curIdx; i++)
            {
                int newMin = FindMostSpecificMethod(candidates[currentMin], paramOrder, null, candidates[i], paramOrder, null, types);
                if (newMin == 0)
                    ambig = true;
                else
                {
                    if (newMin == 2)
                    {
                        ambig = false;
                        currentMin = i;
                    }
                }
            }

            var bestMatch = candidates[currentMin];
            if (ambig)
                throw new AmbiguousMatchException($"Ambiguous match found for '{bestMatch.DeclaringType} {bestMatch}'.");

            return bestMatch;
        }

        /// <summary>
        /// Selects the property that matches the base criteria.
        /// </summary>
        /// <param name="bindingAttr"></param>
        /// <param name="match"></param>
        /// <param name="returnType"></param>
        /// <param name="indexes"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="AmbiguousMatchException"></exception>
        public PropertySymbol? SelectProperty(BindingFlags bindingAttr, IReadOnlyList<PropertySymbol> match, TypeSymbol? returnType, ImmutableArray<TypeSymbol>? indexes, ImmutableArray<ParameterModifier>? modifiers)
        {
            // if indexes is present every element must be non-null
            if (indexes != null)
                foreach (var index in indexes)
                    throw new ArgumentNullException(nameof(index));

            if (match == null || match.Count == 0)
                throw new ArgumentException(nameof(match));

            var candidates = new List<PropertySymbol>(match);

            int i, j = 0;

            // Find all the properties that can be described by type indexes parameter
            int curIdx = 0;
            for (i = 0; i < candidates.Count; i++)
            {
                if (indexes != null)
                {
                    var par = candidates[i].GetIndexParameters();
                    if (par.Length != indexes.Value.Length)
                        continue;

                    for (j = 0; j < indexes.Value.Length; j++)
                    {
                        var pCls = par[j].ParameterType;

                        // If the classes exactly match continue
                        if (pCls == indexes.Value[j])
                            continue;

                        if (pCls == ObjectType)
                            continue;

                        if (pCls.IsPrimitive)
                        {
                            if (CanChangePrimitive(indexes.Value[j], pCls) == false)
                                break;
                        }
                        else
                        {
                            if (pCls.IsAssignableFrom(indexes.Value[j]) == false)
                                break;
                        }
                    }
                }

                if (indexes == null || j == indexes.Value.Length)
                {
                    if (returnType != null)
                    {
                        if (candidates[i].PropertyType.IsPrimitive)
                        {
                            if (CanChangePrimitive(returnType, candidates[i].PropertyType) == false)
                                continue;
                        }
                        else
                        {
                            if (candidates[i].PropertyType.IsAssignableFrom(returnType) == false)
                                continue;
                        }
                    }

                    candidates[curIdx++] = candidates[i];
                }
            }

            if (curIdx == 0)
                return null;

            if (curIdx == 1)
                return candidates[0];

            int currentMin = 0;
            var ambig = false;

            var paramOrder = indexes != null && indexes.Value.Length > 0 ? stackalloc int[indexes.Value.Length] : Array.Empty<int>();
            for (i = 0; i < paramOrder.Length; i++)
                paramOrder[i] = i;

            for (i = 1; i < curIdx; i++)
            {
                int newMin = FindMostSpecificType(candidates[currentMin].PropertyType, candidates[i].PropertyType, returnType);
                if (newMin == 0 && indexes != null)
                    newMin = FindMostSpecific(candidates[currentMin].GetIndexParameters(), paramOrder, null, candidates[i].GetIndexParameters(), paramOrder, null, indexes);

                if (newMin == 0)
                {
                    newMin = FindMostSpecificProperty(candidates[currentMin], candidates[i]);
                    if (newMin == 0)
                        ambig = true;
                }

                if (newMin == 2)
                {
                    ambig = false;
                    currentMin = i;
                }
            }

            var bestMatch = candidates[currentMin];
            if (ambig)
                throw new AmbiguousMatchException(bestMatch.ToString());

            return bestMatch;
        }

        /// <summary>
        /// Returns any exact bindings that may exist.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MethodBaseSymbol? ExactBinding(IReadOnlyList<MethodBaseSymbol> match, ImmutableArray<TypeSymbol> types)
        {
            if (match is null)
                throw new ArgumentNullException(nameof(match));

            var aExactMatches = new MethodBaseSymbol[match.Count];
            int cExactMatches = 0;

            for (int i = 0; i < match.Count; i++)
            {
                var par = match[i].GetParameters();
                if (par.Length == 0)
                    continue;

                int j;
                for (j = 0; j < types.Length; j++)
                {
                    var pCls = par[j].ParameterType;

                    // If the classes  exactly match continue
                    if (!pCls.Equals(types[j]))
                        break;
                }

                if (j < types.Length)
                    continue;

                // Add the exact match to the array of exact matches.
                aExactMatches[cExactMatches] = match[i];
                cExactMatches++;
            }

            if (cExactMatches == 0)
                return null;

            if (cExactMatches == 1)
                return aExactMatches[0];

            return FindMostDerivedNewSlotMeth(aExactMatches, cExactMatches);
        }

        /// <summary>
        /// Returns any exact bindings that may exist.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="returnType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AmbiguousMatchException"></exception>
        public PropertySymbol? ExactPropertyBinding(IReadOnlyList<PropertySymbol> match, TypeSymbol? returnType, ImmutableArray<TypeSymbol> types)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            PropertySymbol? bestMatch = null;

            for (int i = 0; i < match.Count; i++)
            {
                var parameter = match[i].GetIndexParameters();

                int j;
                for (j = 0; j < types.Length; j++)
                {
                    var parameterType = parameter[j].ParameterType;

                    // If the classes  exactly match continue
                    if (parameterType != types![j])
                        break;
                }

                if (j < types.Length)
                    continue;

                if (returnType != null && returnType != match[i].PropertyType)
                    continue;

                if (bestMatch != null)
                    throw new AmbiguousMatchException(bestMatch.ToString());

                bestMatch = match[i];
            }

            return bestMatch;
        }

        int FindMostSpecific(ImmutableArray<ParameterSymbol> p1, ReadOnlySpan<int> paramOrder1, TypeSymbol? paramArrayType1,
                             ImmutableArray<ParameterSymbol> p2, ReadOnlySpan<int> paramOrder2, TypeSymbol? paramArrayType2,
                             IReadOnlyList<TypeSymbol> types)
        {
            // a method using params is always less specific than one not using params
            if (paramArrayType1 != null && paramArrayType2 == null) return 2;
            if (paramArrayType2 != null && paramArrayType1 == null) return 1;

            // now either p1 and p2 both use params or neither does.
            var p1Less = false;
            var p2Less = false;

            for (int i = 0; i < types.Count; i++)
            {
                TypeSymbol c1, c2;

                // If a param array is present, then either
                //      the user re-ordered the parameters in which case
                //          the argument to the param array is either an array
                //              in which case the params is conceptually ignored and so paramArrayType1 == null
                //          or the argument to the param array is a single element
                //              in which case paramOrder[i] == p1.Length - 1 for that element
                //      or the user did not re-order the parameters in which case
                //          the paramOrder array could contain indexes larger than p.Length - 1 (see VSW 577286)
                //          so any index >= p.Length - 1 is being put in the param array

                if (paramArrayType1 != null && paramOrder1[i] >= p1.Length - 1)
                    c1 = paramArrayType1;
                else
                    c1 = p1[paramOrder1[i]].ParameterType;

                if (paramArrayType2 != null && paramOrder2[i] >= p2.Length - 1)
                    c2 = paramArrayType2;
                else
                    c2 = p2[paramOrder2[i]].ParameterType;

                if (c1 == c2) continue;

                switch (FindMostSpecificType(c1, c2, types[i]))
                {
                    case 0: return 0;
                    case 1: p1Less = true; break;
                    case 2: p2Less = true; break;
                }
            }

            // Two way p1Less and p2Less can be equal.  All the arguments are the
            // same they both equal false, otherwise there were things that both
            // were the most specific type on....
            if (p1Less == p2Less)
            {
                return 0;
            }
            else
            {
                return p1Less ? 1 : 2;
            }
        }

        /// <summary>
        /// Finds which type is the most specific.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        int FindMostSpecificType(TypeSymbol c1, TypeSymbol c2, TypeSymbol? t)
        {
            // if the two types are exact, move on
            if (c1 == c2)
                return 0;

            if (c1 == t)
                return 1;

            if (c2 == t)
                return 2;

            bool c1FromC2;
            bool c2FromC1;

            if (c1.IsByRef || c2.IsByRef)
            {
                if (c1.IsByRef && c2.IsByRef)
                {
                    c1 = c1.GetElementType()!;
                    c2 = c2.GetElementType()!;
                }
                else if (c1.IsByRef)
                {
                    if (c1.GetElementType() == c2)
                        return 2;

                    c1 = c1.GetElementType()!;
                }
                else // if (c2.IsByRef)
                {
                    if (c2.GetElementType() == c1)
                        return 1;

                    c2 = c2.GetElementType()!;
                }
            }

            if (c1.IsPrimitive && c2.IsPrimitive)
            {
                c1FromC2 = CanChangePrimitive(c2, c1);
                c2FromC1 = CanChangePrimitive(c1, c2);
            }
            else
            {
                c1FromC2 = c1.IsAssignableFrom(c2);
                c2FromC1 = c2.IsAssignableFrom(c1);
            }

            if (c1FromC2 == c2FromC1)
                return 0;

            if (c1FromC2)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        int FindMostSpecificMethod(MethodBaseSymbol m1, ReadOnlySpan<int> paramOrder1, TypeSymbol? paramArrayType1,
                                   MethodBaseSymbol m2, ReadOnlySpan<int> paramOrder2, TypeSymbol? paramArrayType2,
                                   ImmutableArray<TypeSymbol> types)
        {
            // Find the most specific method based on the parameters.
            int res = FindMostSpecific(m1.GetParameters(), paramOrder1, paramArrayType1,
                                       m2.GetParameters(), paramOrder2, paramArrayType2, types);

            // If the match was not ambiguous then return the result.
            if (res != 0)
                return res;

            // Check to see if the methods have the exact same name and signature.
            if (CompareMethodSig(m1, m2))
            {
                // Determine the depth of the declaring types for both methods.
                var hierarchyDepth1 = GetHierarchyDepth(m1.DeclaringType!);
                var hierarchyDepth2 = GetHierarchyDepth(m2.DeclaringType!);

                // the most derived method is the most specific one
                if (hierarchyDepth1 == hierarchyDepth2)
                    return 0;

                if (hierarchyDepth1 < hierarchyDepth2)
                    return 2;

                return 1;
            }

            // The match is ambiguous.
            return 0;
        }

        int FindMostSpecificField(FieldSymbol cur1, FieldSymbol cur2)
        {
            // Check to see if the fields have the same name.
            if (cur1.Name == cur2.Name)
            {
                int hierarchyDepth1 = GetHierarchyDepth(cur1.DeclaringType!);
                int hierarchyDepth2 = GetHierarchyDepth(cur2.DeclaringType!);

                if (hierarchyDepth1 == hierarchyDepth2)
                {
                    Debug.Assert(cur1.IsStatic != cur2.IsStatic, "hierarchyDepth1 == hierarchyDepth2");
                    return 0;
                }
                else if (hierarchyDepth1 < hierarchyDepth2)
                    return 2;
                else
                    return 1;
            }

            // The match is ambiguous.
            return 0;
        }

        int FindMostSpecificProperty(PropertySymbol cur1, PropertySymbol cur2)
        {
            // Check to see if the fields have the same name.
            if (cur1.Name == cur2.Name)
            {
                int hierarchyDepth1 = GetHierarchyDepth(cur1.DeclaringType!);
                int hierarchyDepth2 = GetHierarchyDepth(cur2.DeclaringType!);

                if (hierarchyDepth1 == hierarchyDepth2)
                {
                    return 0;
                }
                else if (hierarchyDepth1 < hierarchyDepth2)
                    return 2;
                else
                    return 1;
            }

            // The match is ambiguous.
            return 0;
        }

        /// <summary>
        /// Returns <c>true</c> if the two methods have the exact same signature.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool CompareMethodSig(MethodBaseSymbol m1, MethodBaseSymbol m2)
        {
            var params1 = m1.GetParameters();
            var params2 = m2.GetParameters();

            if (params1.Length != params2.Length)
                return false;

            for (int i = 0; i < params1.Length; i++)
                if (params1[i].ParameterType != params2[i].ParameterType)
                    return false;

            return true;
        }

        /// <summary>
        /// Gets the depth of the type within the type hierarchy.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        int GetHierarchyDepth(TypeSymbol type)
        {
            int depth = 0;

            for (var cType = (TypeSymbol?)type; cType != null; cType = cType.BaseType)
                depth++;

            return depth;
        }

        internal MethodBaseSymbol? FindMostDerivedNewSlotMeth(ReadOnlySpan<MethodBaseSymbol> match, int cMatches)
        {
            int deepestHierarchy = 0;
            MethodBaseSymbol? methWithDeepestHierarchy = null;

            for (int i = 0; i < cMatches; i++)
            {
                // Calculate the depth of the hierarchy of the declaring type of the current method.
                int currentHierarchyDepth = GetHierarchyDepth(match[i].DeclaringType!);

                // The two methods have the same name, signature, and hierarchy depth.
                // This can only happen if at least one is vararg or generic.
                if (currentHierarchyDepth == deepestHierarchy)
                    throw new AmbiguousMatchException(methWithDeepestHierarchy!.ToString());

                // Check to see if this method is on the most derived class.
                if (currentHierarchyDepth > deepestHierarchy)
                {
                    deepestHierarchy = currentHierarchyDepth;
                    methWithDeepestHierarchy = match[i];
                }
            }

            return methWithDeepestHierarchy;
        }

        // This method will create the mapping between the Parameters and the underlying
        //  data based upon the names array.  The names array is stored in the same order
        //  as the values and maps to the parameters of the method.  We store the mapping
        //  from the parameters to the names in the paramOrder array.  All parameters that
        //  don't have matching names are then stored in the array in order.
        bool CreateParamOrder(int[] paramOrder, ImmutableArray<ParameterSymbol> pars, string[] names)
        {
            var used = new bool[pars.Length];

            // Mark which parameters have not been found in the names list
            for (var i = 0; i < pars.Length; i++)
                paramOrder[i] = -1;

            // Find the parameters with names.
            for (var i = 0; i < names.Length; i++)
            {
                int j;
                for (j = 0; j < pars.Length; j++)
                {
                    if (names[i].Equals(pars[j].Name))
                    {
                        paramOrder[j] = i;
                        used[i] = true;
                        break;
                    }
                }

                // This is an error condition.  The name was not found.  This method must not match what we sent.
                if (j == pars.Length)
                    return false;
            }

            // Now we fill in the holes with the parameters that are unused.
            int pos = 0;
            for (int i = 0; i < pars.Length; i++)
            {
                if (paramOrder[i] == -1)
                {
                    for (; pos < pars.Length; pos++)
                    {
                        if (!used[pos])
                        {
                            paramOrder[i] = pos;
                            pos++;
                            break;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the given source primitive type can be converted to the given target primitive type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        internal bool CanChangePrimitive(TypeSymbol source, TypeSymbol target)
        {
            if ((source == IntPtrType && target == IntPtrType) || (source == UIntPtrType && target == UIntPtrType))
                return true;

            var widerCodes = PrimitiveConversions[(int)source.TypeCode];
            var targetCode = (Primitives)(1 << (int)target.TypeCode);

            return (widerCodes & targetCode) != 0;
        }

        static ReadOnlySpan<Primitives> PrimitiveConversions =>
        [
            /* Empty    */  0, // not primitive
            /* Object   */  0, // not primitive
            /* DBNull   */  0, // not primitive
            /* Boolean  */  Primitives.Boolean,
            /* Char     */  Primitives.Char    | Primitives.UInt16 | Primitives.UInt32 | Primitives.Int32  | Primitives.UInt64 | Primitives.Int64  | Primitives.Single |  Primitives.Double,
            /* SByte    */  Primitives.SByte   | Primitives.Int16  | Primitives.Int32  | Primitives.Int64  | Primitives.Single | Primitives.Double,
            /* Byte     */  Primitives.Byte    | Primitives.Char   | Primitives.UInt16 | Primitives.Int16  | Primitives.UInt32 | Primitives.Int32  | Primitives.UInt64 |  Primitives.Int64 |  Primitives.Single |  Primitives.Double,
            /* Int16    */  Primitives.Int16   | Primitives.Int32  | Primitives.Int64  | Primitives.Single | Primitives.Double,
            /* UInt16   */  Primitives.UInt16  | Primitives.UInt32 | Primitives.Int32  | Primitives.UInt64 | Primitives.Int64  | Primitives.Single | Primitives.Double,
            /* Int32    */  Primitives.Int32   | Primitives.Int64  | Primitives.Single | Primitives.Double,
            /* UInt32   */  Primitives.UInt32  | Primitives.UInt64 | Primitives.Int64  | Primitives.Single | Primitives.Double,
            /* Int64    */  Primitives.Int64   | Primitives.Single | Primitives.Double,
            /* UInt64   */  Primitives.UInt64  | Primitives.Single | Primitives.Double,
            /* Single   */  Primitives.Single  | Primitives.Double,
            /* Double   */  Primitives.Double,
            /* Decimal  */  Primitives.Decimal,
            /* DateTime */  Primitives.DateTime,
            /* [Unused] */  0,
            /* String   */  Primitives.String,
        ];

        [Flags]
        enum Primitives
        {
            Boolean = 1 << TypeCode.Boolean,
            Char = 1 << TypeCode.Char,
            SByte = 1 << TypeCode.SByte,
            Byte = 1 << TypeCode.Byte,
            Int16 = 1 << TypeCode.Int16,
            UInt16 = 1 << TypeCode.UInt16,
            Int32 = 1 << TypeCode.Int32,
            UInt32 = 1 << TypeCode.UInt32,
            Int64 = 1 << TypeCode.Int64,
            UInt64 = 1 << TypeCode.UInt64,
            Single = 1 << TypeCode.Single,
            Double = 1 << TypeCode.Double,
            Decimal = 1 << TypeCode.Decimal,
            DateTime = 1 << TypeCode.DateTime,
            String = 1 << TypeCode.String,
        }

    }

}
