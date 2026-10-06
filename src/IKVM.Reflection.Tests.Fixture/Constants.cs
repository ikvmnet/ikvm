using System;

namespace IKVM.Reflection.Tests.Fixture
{

    public enum PlainEnum
    {
        A,
        B,
        C = 10,
    }

    [Flags]
    public enum FlagsEnum : uint
    {
        None = 0,
        One = 1,
        Two = 2,
        All = 0xFFFFFFFF,
    }

    public enum ByteEnum : byte { A = 1, B = 255 }

    public enum SByteEnum : sbyte { A = -128, B = 127 }

    public enum ShortEnum : short { A = -1 }

    public enum UShortEnum : ushort { A = 65535 }

    public enum LongEnum : long { A = long.MinValue, B = long.MaxValue }

    public enum ULongEnum : ulong { A = ulong.MaxValue }

    public static class Constants
    {

        public const bool Bool = true;

        public const char Char = 'c';

        public const byte Byte = 255;

        public const sbyte SByte = -128;

        public const short Short = -32768;

        public const ushort UShort = 65535;

        public const int Int = int.MinValue;

        public const uint UInt = uint.MaxValue;

        public const long Long = long.MinValue;

        public const ulong ULong = ulong.MaxValue;

        public const float Float = 1.5f;

        public const double Double = double.NaN;

        public const double DoubleInfinity = double.PositiveInfinity;

        public const string String = "string";

        public const string EmptyString = "";

        public const string NullString = null;

        public const object NullObject = null;

        public const PlainEnum Enum = PlainEnum.C;

        public const FlagsEnum Flags = FlagsEnum.One | FlagsEnum.Two;

        public const decimal Decimal = 1.5m;

    }

}
