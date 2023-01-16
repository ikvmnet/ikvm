using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record RuntimeVisibleAnnotationsAttributeRecord(AnnotationRecord[] Annotations) : AttributeRecord
    {

        public static bool TryReadRuntimeVisibleAnnotationsAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var items = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (AnnotationRecord.TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                items[i] = annotation;
            }

            attribute = new RuntimeVisibleAnnotationsAttributeRecord(items);
            return true;
        }

    }

}
