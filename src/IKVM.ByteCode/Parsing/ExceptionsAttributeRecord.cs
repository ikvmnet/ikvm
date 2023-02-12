namespace IKVM.ByteCode.Parsing
{
    internal sealed record ExceptionsAttributeRecord(ushort[] ExceptionsIndexes) : AttributeRecord
    {
        public const string Name = "Exceptions";

        public static bool TryReadExceptionsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var entries = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadU2(out ushort index) == false)
                    return false;

                entries[i] = index;
            }

            attribute = new ExceptionsAttributeRecord(entries);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort) + ExceptionsIndexes.Length * sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)ExceptionsIndexes.Length) == false)
                return false;
            if (writer.TryWriteManyU2(ExceptionsIndexes) == false)
                return false;

            return true;
        }
    }
}