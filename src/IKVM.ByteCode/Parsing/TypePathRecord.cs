namespace IKVM.ByteCode.Parsing
{

    public record struct TypePathRecord(params TypePathItemRecord[] Path)
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypePathRecord typePath)
        {
            typePath = default;

            if (reader.TryReadU1(out byte length) == false)
                return false;

            var path = new TypePathItemRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (TypePathItemRecord.TryRead(ref reader, out var item) == false)
                    return false;

                path[i] = item;
            }

            typePath = new TypePathRecord(path);
            return true;
        }

        public int GetSize()
        {
            var size = 0;
            size += sizeof(byte);

            foreach (var item in Path)
                size += item.GetSize();

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU1((byte)Path.Length) == false)
                return false;

            foreach (var item in Path)
                if (item.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }

}
