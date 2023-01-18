using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record DynamicConstantRecord(ushort BootstrapMethodAttributeIndex, ushort NameAndTypeIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a Dynamic constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skipIndex"></param>
        public static bool TryReadDynamicConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skipIndex)
        {
            constant = null;
            skipIndex = 0;

            if (reader.TryReadBigEndian(out ushort bootstrapMethodAttrIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new DynamicConstantRecord(bootstrapMethodAttrIndex, nameAndTypeIndex);
            return true;
        }

    }

}
