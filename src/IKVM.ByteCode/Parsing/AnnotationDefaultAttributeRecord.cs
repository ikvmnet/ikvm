using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record AnnotationDefaultAttributeRecord(ElementValueRecord DefaultValue) : AttributeRecord
    {

        public static bool TryReadAnnotationDefaultAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (ElementValueRecord.TryRead(ref reader, out var defaultValue) == false)
                return false;

            attribute = new AnnotationDefaultAttributeRecord(defaultValue);
            return true;
        }

    }

}
