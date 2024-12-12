using System;

namespace IKVM.CoreLib.Symbols
{

    static class TypeSymbolExtensions
    {

        /// <summary>
        /// Returns the appropriate <see cref="TypeCode"/> for the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeCode GetTypeCode(TypeSymbol? type)
        {
            if (type == null)
                return TypeCode.Empty;

            if (type.IsMissing == false && type.IsEnum)
                type = type.GetEnumUnderlyingType();

            var context = type.Context;
            if (type == context.ResolveCoreType("System.Boolean"))
                return TypeCode.Boolean;

            if (type == context.ResolveCoreType("System.Char"))
                return TypeCode.Char;

            if (type == context.ResolveCoreType("System.SByte"))
                return TypeCode.SByte;

            if (type == context.ResolveCoreType("System.Byte"))
                return TypeCode.Byte;

            if (type == context.ResolveCoreType("System.Int16"))
                return TypeCode.Int16;

            if (type == context.ResolveCoreType("System.UInt16"))
                return TypeCode.UInt16;

            if (type == context.ResolveCoreType("System.Int32"))
                return TypeCode.Int32;

            if (type == context.ResolveCoreType("System.UInt32"))
                return TypeCode.UInt32;

            if (type == context.ResolveCoreType("System.Int64"))
                return TypeCode.Int64;

            if (type == context.ResolveCoreType("System.UInt64"))
                return TypeCode.UInt64;

            if (type == context.ResolveCoreType("System.Single"))
                return TypeCode.Single;

            if (type == context.ResolveCoreType("System.Double"))
                return TypeCode.Double;

            if (type == context.ResolveCoreType("System.DateTime"))
                return TypeCode.DateTime;

            if (type == context.ResolveCoreType("System.DBNull"))
                return TypeCode.DBNull;

            if (type == context.ResolveCoreType("System.Decimal"))
                return TypeCode.Decimal;

            if (type == context.ResolveCoreType("System.String"))
                return TypeCode.String;

            if (type.IsMissing)
                throw new MissingTypeSymbolException(type);

            return TypeCode.Object;

        }

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
