using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record struct ParameterAnnotationRecord(AnnotationRecord[] Annotations)
    {

        public static bool TryReadParameterAnnotation(ref SequenceReader<byte> reader, out ParameterAnnotationRecord record)
        {
            record = default;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var annotations = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (AnnotationRecord.TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                annotations[i] = annotation;
            }

            record = new ParameterAnnotationRecord(annotations);
            return true;
        }

    }

}
