using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record NameAndTypeConstantRecord(ushort NameIndex, ushort DescriptorIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a NameAndType constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadNameAndTypeConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;

            constant = new NameAndTypeConstantRecord(nameIndex, descriptorIndex);
            return true;
        }

    }

}
