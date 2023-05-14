using System;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record FloatConstantRecord(float Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Float constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadFloatConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU4(out uint value) == false)
                return false;

#if NETFRAMEWORK || NETCOREAPP3_1
            var v = RawBitConverter.UInt32BitsToSingle(value);
#else
            var v = BitConverter.UInt32BitsToSingle(value);
#endif

            constant = new FloatConstantRecord(v);
            return true;
        }

    }

}
