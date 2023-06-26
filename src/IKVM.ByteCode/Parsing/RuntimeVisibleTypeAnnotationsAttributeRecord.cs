namespace IKVM.ByteCode.Parsing
{
    internal sealed record RuntimeVisibleTypeAnnotationsAttributeRecord(TypeAnnotationRecord[] Annotations) : AttributeRecord
    {
        public static bool TryReadRuntimeVisibleTypeAnnotationsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
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

            attribute = new RuntimeVisibleTypeAnnotationsAttributeRecord(annotations);
            return true;
        }

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public override int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);

            foreach (var annotation in Annotations)
                size += annotation.GetSize();

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
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