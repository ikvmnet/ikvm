using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ConstantValueAttributeRecord(ushort ValueIndex) : AttributeRecord
    {

        public static bool TryReadConstantValueAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort valueIndex) == false)
                return false;

            attribute = new ConstantValueAttributeRecord(valueIndex);
            return true;
        }

    }

}
