namespace IKVM.ByteCode.Parsing
{
    internal sealed record RuntimeInvisibleAnnotationsAttributeRecord(AnnotationRecord[] Annotations) : AttributeRecord
    {
        public static bool TryReadRuntimeInvisibleAnnotationsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var items = new AnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (AnnotationRecord.TryReadAnnotation(ref reader, out var annotation) == false)
                    return false;

                items[i] = annotation;
            }

            attribute = new RuntimeInvisibleAnnotationsAttributeRecord(items);
            return true;
        }

        public override int GetSize()
        {
            var size = sizeof(ushort);

            foreach (var annotation in Annotations)
                size += annotation.GetSize();

            return size;
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)Annotations.Length) == false)
                return false;

            foreach (var annotation in Annotations)
                if (annotation.TryWrite(ref writer) == false)
                    return false;

            return true;
        }
    }
}