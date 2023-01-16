using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record DoubleConstantRecord(double Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Double constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadDoubleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out double value) == false)
                return false;

            constant = new DoubleConstantRecord(value);
            return true;
        }

    }

}
