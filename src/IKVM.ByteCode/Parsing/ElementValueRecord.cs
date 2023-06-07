namespace IKVM.ByteCode.Parsing
{

    public record struct ElementValueRecord(ElementValueTag Tag, ElementValueValueRecord Value)
    {

        public static bool TryRead(ref ClassFormatReader reader, out ElementValueRecord record)
        {
            record = default;

            if (reader.TryReadU1(out byte tag) == false)
                return false;
            if (TryReadValue(ref reader, (ElementValueTag)tag, out var value) == false)
                return false;

            record = new ElementValueRecord((ElementValueTag)tag, value);
            return true;
        }

        static bool TryReadValue(ref ClassFormatReader reader, ElementValueTag tag, out ElementValueValueRecord value) => tag switch
        {
            ElementValueTag.Byte => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Char => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Double => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Float => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Integer => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Long => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Short => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Boolean => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.String => ElementValueConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Enum => ElementValueEnumConstantValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Class => ElementValueClassValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Annotation => ElementValueAnnotationValueRecord.TryRead(ref reader, out value),
            ElementValueTag.Array => ElementValueArrayValueRecord.TryRead(ref reader, out value),
            _ => throw new ByteCodeException($"Invalid element value tag: '{(char)tag}'."),
        };

        public int GetSize()
        {
            var size = 0;
            size += sizeof(byte);
            size += Value.GetSize();
            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU1((byte)Tag) == false)
                return false;

            if (Value.TryWrite(ref writer) == false)
                return false;

            return true;
        }

    }

}
