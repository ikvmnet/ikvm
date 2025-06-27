using System;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a selector that can match types.
    /// </summary>
    public readonly struct TypeSymbolSelector
    {

        /// <summary>
        /// Returns a <see cref="TypeSymbolSelector"/> that matches any type.
        /// </summary>
        /// <returns></returns>
        public static implicit operator TypeSymbolSelector(TypeSymbol? type)
        {
            if (type is null)
                return Any();
            else
                return Exact(type);
        }

        /// <summary>
        /// Returns a <see cref="TypeSymbolSelector"/> that matches any type.
        /// </summary>
        /// <returns></returns>
        public static TypeSymbolSelector Any() => new TypeSymbolSelector();

        /// <summary>
        /// Returns a <see cref="TypeSymbolSelector"/> that matches the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeSymbolSelector Exact(TypeSymbol type) => new TypeSymbolSelector(type);

        /// <summary>
        /// Returns a <see cref="TypeSymbolSelector"/> that matches a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TypeSymbolSelector Predicate(Func<TypeSymbol, bool> predicate) => new TypeSymbolSelector(predicate);

        /// <summary>
        /// Allowable primitive converstions.
        /// </summary>
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

        /// <summary>
        /// Primitive types mapped.
        /// </summary>
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

        readonly TypeSymbol? _directType;
        readonly Func<TypeSymbol, bool>? _predicate;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TypeSymbolSelector()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="directType"></param>
        public TypeSymbolSelector(TypeSymbol directType)
        {
            _directType = directType ?? throw new ArgumentNullException(nameof(directType));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="predicate"></param>
        public TypeSymbolSelector(Func<TypeSymbol, bool> predicate)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        /// Returns <c>true</c> if this selector matches with the given type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public readonly bool Match(SymbolContext context, TypeSymbol type)
        {
            if (_predicate != null)
                return _predicate(type);
            else if (_directType != null)
                return MatchDirect(context, type);
            else
                return true;
        }

        /// <summary>
        /// Returns <c>true</c> if this selector matches with the given type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        readonly bool MatchDirect(SymbolContext context, TypeSymbol type)
        {
            // exact parameter type match
            if (_directType == type)
                return true;

            // everything is convertable to object
            if (type == context.ResolveCoreType("System.Object"))
                return true;

            // primitive paramter that can't be converted to parameter type
            if (type.IsPrimitive && CanChangePrimitive(context, _directType!, type) == false)
                return false;

            // can't otherwise assign type to parameter type
            if (type.IsAssignableFrom(_directType) == false)
                return false;

            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the given source primitive type can be converted to the given target primitive type.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool CanChangePrimitive(SymbolContext context, TypeSymbol source, TypeSymbol target)
        {
            var intPtrType = context.ResolveCoreType("System.IntPtr");
            var uintPtrType = context.ResolveCoreType("System.UIntPtr");

            if ((source == intPtrType && target == intPtrType) || (source == uintPtrType && target == uintPtrType))
                return true;

            var widerCodes = PrimitiveConversions[(int)source.TypeCode];
            var targetCode = (Primitives)(1 << (int)target.TypeCode);

            return (widerCodes & targetCode) != 0;
        }

        /// <summary>
        /// Returns 0, 1 or 2, depending on which type is most specific.    
        /// </summary>
        /// <param name="context"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public readonly int FindMostSpecific(SymbolContext context, TypeSymbol c1, TypeSymbol c2)
        {
            // if the two types are exact, move on
            if (c1 == c2)
                return 0;

            if (_directType != null && c1 == _directType)
                return 1;

            if (_directType != null && c2 == _directType)
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
                c1FromC2 = CanChangePrimitive(context, c2, c1);
                c2FromC1 = CanChangePrimitive(context, c1, c2);
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

    }

}
