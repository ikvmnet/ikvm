using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record StringConstantRecord(ushort ValueIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a Class constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadStringConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new StringConstantRecord(nameIndex);
            return true;
        }

    }

}
