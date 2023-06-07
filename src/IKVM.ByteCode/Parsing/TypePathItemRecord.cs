namespace IKVM.ByteCode.Parsing
{

    public record struct TypePathItemRecord(TypePathKind Kind, byte ArgumentIndex)
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypePathItemRecord record)
        {
            record = default;

            if (reader.TryReadU1(out byte kind) == false)
                return false;
            if (reader.TryReadU1(out byte argumentIndex) == false)
                return false;

            record = new TypePathItemRecord((TypePathKind)kind, argumentIndex);
            return true;
        }

        public int GetSize()
        {
            var size = 0;
            size += sizeof(byte);
            size += sizeof(byte);
            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU1((byte)Kind) == false)
                return false;
            if (writer.TryWriteU1(ArgumentIndex) == false)
                return false;

            return true;
        }

    }

}
