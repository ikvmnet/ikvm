using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record DeprecatedAttributeRecord : AttributeRecord
    {

        public static bool TryReadDeprecatedAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = new DeprecatedAttributeRecord();
            return true;
        }

    }

}