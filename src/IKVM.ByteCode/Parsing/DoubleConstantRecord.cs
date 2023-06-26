using IKVM.ByteCode.Buffers;
using System;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record DoubleConstantRecord(double Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Double constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadDoubleConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 1;

            if (reader.TryReadU4(out uint a) == false)
                return false;
            if (reader.TryReadU4(out uint b) == false)
                return false;

#if NETFRAMEWORK || NETCOREAPP3_1
            var v = RawBitConverter.UInt64BitsToDouble(((ulong)a << 32) | b);
#else
            var v = BitConverter.UInt64BitsToDouble(((ulong)a << 32) | b);
#endif

            constant = new DoubleConstantRecord(v);
            return true;
        }

        protected override int GetConstantSize() =>
            sizeof(uint) + sizeof(uint);

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
#if NETFRAMEWORK || NETCOREAPP3_1
            var v = RawBitConverter.DoubleToUInt64Bits(Value);
#else
            var v = BitConverter.DoubleToUInt64Bits(Value);
#endif

            if (writer.TryWriteU4((uint)(v >> 32)) == false)
                return false;
            if (writer.TryWriteU4((uint)v) == false)
                return false;

            return true;
        }
    }

}
