using System;

namespace IKVM.CoreLib.Symbols
{

    static class TypeSymbolExtensions
    {

        /// <summary>
        /// Gets the associated <see cref="System.Type"/> for the specified type code.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Type GetSystemType(this TypeSymbol symbol)
        {
            return symbol.TypeCode switch
            {
                TypeCode.Empty => typeof(void),
                TypeCode.Object => typeof(object),
                TypeCode.DBNull => typeof(DBNull),
                TypeCode.Boolean => typeof(bool),
                TypeCode.Char => typeof(Char),
                TypeCode.SByte => typeof(SByte),
                TypeCode.Byte => typeof(Byte),
                TypeCode.Int16 => typeof(Int16),
                TypeCode.UInt16 => typeof(UInt16),
                TypeCode.Int32 => typeof(Int32),
                TypeCode.UInt32 => typeof(UInt32),
                TypeCode.Int64 => typeof(Int64),
                TypeCode.UInt64 => typeof(UInt64),
                TypeCode.Single => typeof(Single),
                TypeCode.Double => typeof(Double),
                TypeCode.Decimal => typeof(Decimal),
                TypeCode.DateTime => typeof(DateTime),
                TypeCode.String => typeof(string),
                _ => throw new InvalidOperationException(),
            };
        }

    }

}
