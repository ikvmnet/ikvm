using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record IntegerConstantRecord(int Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Integer constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="record"></param>
        /// <param name="skip"></param>
        public static bool TryReadIntegerConstant(ref SequenceReader<byte> reader, out ConstantRecord record, out int skip)
        {
            record = null;
            skip = 0;

            if (reader.TryReadBigEndian(out int value) == false)
                return false;

            record = new IntegerConstantRecord(value);
            return true;
        }

    }

}
