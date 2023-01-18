using System;

using IKVM.ByteCode.Reading;

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

            var h = (ulong)a << 4;
            var l = (ulong)b;
            var z = h | l;
#if NETFRAMEWORK || NETCOREAPP3_1
            var v = BitConverter.Int64BitsToDouble(unchecked((long)z));
#else
            var v = BitConverter.UInt64BitsToDouble(z);
#endif

            constant = new DoubleConstantRecord(v);
            return true;
        }

    }

}
