namespace IKVM.ByteCode.Parsing
{

    internal sealed record RuntimeInvisibleTypeAnnotationsAttributeRecord(TypeAnnotationRecord[] Annotations) : AttributeRecord
    {

        public static bool TryReadRuntimeInvisibleTypeAnnotationsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var annotations = new TypeAnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (TypeAnnotationRecord.TryReadTypeAnnotation(ref reader, out var annotation) == false)
                    return false;

                annotations[i] = annotation;
            }

            attribute = new RuntimeInvisibleTypeAnnotationsAttributeRecord(annotations);
            return true;
        }

        public override int GetSize()
        {
            throw new System.NotImplementedException();
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

}
