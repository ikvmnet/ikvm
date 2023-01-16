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
        /// <param name="constant"></param>
        public static bool TryReadIntegerConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out int value) == false)
                return false;

            constant = new IntegerConstantRecord(value);
            return true;
        }

    }

}
