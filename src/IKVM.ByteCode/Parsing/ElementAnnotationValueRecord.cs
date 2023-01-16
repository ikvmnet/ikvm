using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementAnnotationValueRecord(AnnotationRecord Annotation) : ElementValueRecord
    {

        public static bool TryReadElementAnnotationValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (AnnotationRecord.TryReadAnnotation(ref reader, out var annotation) == false)
                return false;

            value = new ElementAnnotationValueRecord(annotation);
            return true;
        }

    }

}