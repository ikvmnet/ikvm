using System;

namespace IKVM.Reflection.Tests.Fixture
{

    /// <summary>
    /// An enum with the default underlying type and a value that does not follow its predecessor.
    /// </summary>
    public enum PlainEnum
    {
        A,
        B,
        C = 10,
    }

    /// <summary>
    /// A flags enum with an unsigned underlying type and a value with every bit set.
    /// </summary>
    [Flags]
    public enum FlagsEnum : uint
    {
        None = 0,
        One = 1,
        Two = 2,
        All = 0xFFFFFFFF,
    }

    /// <summary>
    /// An enum with an unsigned 8-bit underlying type.
    /// </summary>
    public enum ByteEnum : byte { A = 1, B = 255 }

    /// <summary>
    /// An enum with a signed 8-bit underlying type.
    /// </summary>
    public enum SByteEnum : sbyte { A = -128, B = 127 }

    /// <summary>
    /// An enum with a signed 16-bit underlying type.
    /// </summary>
    public enum ShortEnum : short { A = -1 }

    /// <summary>
    /// An enum with an unsigned 16-bit underlying type.
    /// </summary>
    public enum UShortEnum : ushort { A = 65535 }

    /// <summary>
    /// An enum with a signed 64-bit underlying type.
    /// </summary>
    public enum LongEnum : long { A = long.MinValue, B = long.MaxValue }

    /// <summary>
    /// An enum with an unsigned 64-bit underlying type.
    /// </summary>
    public enum ULongEnum : ulong { A = ulong.MaxValue }

    /// <summary>
    /// Constants of every type a constant can have, so the rendering covers how each is stored.
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// A Boolean constant.
        /// </summary>
        public const bool Bool = true;

        /// <summary>
        /// A character constant.
        /// </summary>
        public const char Char = 'c';

        /// <summary>
        /// An unsigned 8-bit constant at its maximum.
        /// </summary>
        public const byte Byte = 255;

        /// <summary>
        /// A signed 8-bit constant at its minimum.
        /// </summary>
        public const sbyte SByte = -128;

        /// <summary>
        /// A signed 16-bit constant at its minimum.
        /// </summary>
        public const short Short = -32768;

        /// <summary>
        /// An unsigned 16-bit constant at its maximum.
        /// </summary>
        public const ushort UShort = 65535;

        /// <summary>
        /// A signed 32-bit constant at its minimum.
        /// </summary>
        public const int Int = int.MinValue;

        /// <summary>
        /// An unsigned 32-bit constant at its maximum.
        /// </summary>
        public const uint UInt = uint.MaxValue;

        /// <summary>
        /// A signed 64-bit constant at its minimum.
        /// </summary>
        public const long Long = long.MinValue;

        /// <summary>
        /// An unsigned 64-bit constant at its maximum.
        /// </summary>
        public const ulong ULong = ulong.MaxValue;

        /// <summary>
        /// A single-precision constant.
        /// </summary>
        public const float Float = 1.5f;

        /// <summary>
        /// A double-precision constant that is not a number.
        /// </summary>
        public const double Double = double.NaN;

        /// <summary>
        /// A double-precision constant that is infinite.
        /// </summary>
        public const double DoubleInfinity = double.PositiveInfinity;

        /// <summary>
        /// A string constant.
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// An empty string constant.
        /// </summary>
        public const string EmptyString = "";

        /// <summary>
        /// A <see langword="null"/> string constant.
        /// </summary>
        public const string NullString = null;

        /// <summary>
        /// A <see langword="null"/> object constant.
        /// </summary>
        public const object NullObject = null;

        /// <summary>
        /// An enum constant.
        /// </summary>
        public const PlainEnum Enum = PlainEnum.C;

        /// <summary>
        /// A flags enum constant with two flags set.
        /// </summary>
        public const FlagsEnum Flags = FlagsEnum.One | FlagsEnum.Two;

        /// <summary>
        /// A decimal constant, which is not a metadata constant but a static field with a custom attribute.
        /// </summary>
        public const decimal Decimal = 1.5m;

    }

}
