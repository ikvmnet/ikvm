using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record AnnotationDefaultAttributeRecord(ElementValueRecord DefaultValue) : AttributeRecord
    {

        public static bool TryReadAnnotationDefaultAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (ElementValueRecord.TryReadElementValue(ref reader, out var defaultValue) == false)
                return false;

            attribute = new AnnotationDefaultAttributeRecord(defaultValue);
            return true;
        }

    }

}
