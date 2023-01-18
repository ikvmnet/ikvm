using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record struct MethodRecord(AccessFlag AccessFlags, ushort NameIndex, ushort DescriptorIndex, AttributeInfoRecord[] Attributes)
    {

        /// <summary>
        /// Parses a method.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="method"></param>
        public static bool TryRead(ref SequenceReader<byte> reader, out MethodRecord method)
        {
            method = default;

            if (reader.TryReadBigEndian(out ushort accessFlags) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort descriptorIndex) == false)
                return false;
            if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            method = new MethodRecord((AccessFlag)accessFlags, nameIndex, descriptorIndex, attributes);
            return true;
        }

    }

}
