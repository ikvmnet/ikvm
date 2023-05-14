namespace IKVM.ByteCode.Parsing
{

    internal record struct TypeAnnotationLocalVarTargetItemRecord(ushort Offset, ushort Length, ushort Index)
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationLocalVarTargetItemRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort offset) == false)
                return false;

            if (reader.TryReadU2(out ushort length) == false)
                return false;

            if (reader.TryReadU2(out ushort index) == false)
                return false;

            record = new TypeAnnotationLocalVarTargetItemRecord(offset, length, index);
            return true;
        }

        /// <summary>
        /// Gets the size of the record if written.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            var length = 0;
            length += sizeof(ushort);
            length += sizeof(ushort);
            length += sizeof(ushort);
            return length;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(Offset) == false)
                return false;
            if (writer.TryWriteU2(Length) == false)
                return false;
            if (writer.TryWriteU2(Index) == false)
                return false;

            return true;
        }

    }

}
