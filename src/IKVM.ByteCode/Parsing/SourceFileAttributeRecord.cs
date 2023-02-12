namespace IKVM.ByteCode.Parsing
{
    internal record SourceFileAttributeRecord(ushort SourceFileIndex) : AttributeRecord
    {
        public static bool TryReadSourceFileAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort sourceFileIndex) == false)
                return false;

            attribute = new SourceFileAttributeRecord(sourceFileIndex);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(SourceFileIndex) == false)
                return false;

            return true;
        }
    }
}