using IKVM.ByteCode.Buffers;
using System;

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

        protected override int GetConstantSize() =>
            sizeof(uint);

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
#if NETFRAMEWORK || NETCOREAPP3_1
            var v = RawBitConverter.SingleToUInt32Bits(Value);
#else
            var v = BitConverter.SingleToUInt32Bits(Value);
#endif

            if (writer.TryWriteU4(v) == false)
                return false;

            return true;
        }
    }
}
