namespace IKVM.ByteCode.Parsing
{

    public record struct AnnotationRecord(ushort TypeIndex, ElementValuePairRecord[] Elements)
    {

        public static bool TryReadAnnotation(ref ClassFormatReader reader, out AnnotationRecord annotation)
        {
            annotation = default;

            if (reader.TryReadU2(out ushort typeIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort pairCount) == false)
                return false;

            var elements = new ElementValuePairRecord[pairCount];
            for (int i = 0; i < pairCount; i++)
            {
                if (ElementValuePairRecord.TryRead(ref reader, out var element) == false)
                    return false;

                elements[i] = element;
            }

            annotation = new AnnotationRecord(typeIndex, elements);
            return true;
        }

        public int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);
            size += sizeof(ushort);

            foreach (var element in Elements)
                size += element.GetSize();

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(TypeIndex) == false)
                return false;
            if (writer.TryWriteU2((ushort)Elements.Length) == false)
                return false;

            foreach (var element in Elements)
                if (element.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}
