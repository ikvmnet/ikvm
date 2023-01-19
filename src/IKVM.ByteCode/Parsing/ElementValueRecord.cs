namespace IKVM.ByteCode.Parsing
{

    internal abstract record class ElementValueRecord(byte Tag)
    {

        public static bool TryRead(ref ClassFormatReader reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadU1(out byte tag) == false)
                return false;

            return (char)tag switch
            {
                'B' or 'C' or 'D' or 'F' or 'I' or 'J' or 'S' or 'Z' or 's' => ElementConstantValueRecord.TryReadElementConstantValue(ref reader, tag, out value),
                'e' => ElementEnumConstantValueRecord.TryRead(ref reader, tag, out value),
                'c' => ElementClassInfoValueRecord.TryReadElementClassInfoValue(ref reader, tag, out value),
                '@' => ElementAnnotationValueRecord.TryReadElementAnnotationValue(ref reader, tag, out value),
                '[' => ElementArrayValueRecord.TryReadElementArrayValue(ref reader, tag, out value),
                _ => throw new ByteCodeException($"Invalid annotation element value tag: '{tag}'."),
            };
        }

        public virtual int GetSize()
        {
            var size = 0;
            size += sizeof(byte);
            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU1(Tag) == false)
                return false;

            return true;
        }

    }

}
