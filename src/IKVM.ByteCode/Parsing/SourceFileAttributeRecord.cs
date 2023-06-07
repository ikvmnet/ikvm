namespace IKVM.ByteCode.Parsing
{

    public record SourceFileAttributeRecord(ushort SourceFileIndex) : AttributeRecord
    {

        public static bool TryReadSourceFileAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort sourceFileIndex) == false)
                return false;

            attribute = new SourceFileAttributeRecord(sourceFileIndex);
            return true;
        }

    }

}