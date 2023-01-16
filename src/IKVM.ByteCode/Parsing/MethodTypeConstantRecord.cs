using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record MethodTypeConstantRecord(ushort DescriptorIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a MethodType constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadMethodTypeConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;

            constant = new MethodTypeConstantRecord(descriptorIndex);
            return true;
        }

    }

}
