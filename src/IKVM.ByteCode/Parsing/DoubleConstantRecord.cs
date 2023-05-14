using System;

using IKVM.ByteCode.Buffers;

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

    }

}
