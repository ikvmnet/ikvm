using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record LongConstantRecord(long Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Long constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadLongConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 1;

            if (reader.TryReadBigEndian(out long value) == false)
                return false;

            constant = new LongConstantRecord(value);
            return true;
        }

    }

}
