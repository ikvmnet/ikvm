using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record NestHostAttributeRecord(ushort HostClassIndex) : AttributeRecord
    {

        public static bool TryReadNestHostAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort hostClassIndex) == false)
                return false;

            attribute = new NestHostAttributeRecord(hostClassIndex);
            return true;
        }

    }

}
