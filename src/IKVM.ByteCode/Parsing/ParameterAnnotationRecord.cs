namespace IKVM.ByteCode.Parsing
{

    public record struct ParameterAnnotationRecord(AnnotationRecord[] Annotations)
    {

        public static bool TryReadParameterAnnotation(ref ClassFormatReader reader, out ParameterAnnotationRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort count) == false)
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
