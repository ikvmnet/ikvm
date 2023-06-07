namespace IKVM.ByteCode.Parsing
{

    public sealed record ExceptionsAttributeRecord(ushort[] ExceptionsIndexes) : AttributeRecord
    {

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

    }

}