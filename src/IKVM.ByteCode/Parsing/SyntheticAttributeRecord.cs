using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record SyntheticAttributeRecord : AttributeRecord
    {

        public static bool TryReadSyntheticAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = new SyntheticAttributeRecord();
            return true;
        }

    }

}
