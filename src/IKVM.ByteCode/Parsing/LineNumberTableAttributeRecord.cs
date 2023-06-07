namespace IKVM.ByteCode.Parsing
{

    public sealed record LineNumberTableAttributeRecord(LineNumberTableAttributeItemRecord[] Items) : AttributeRecord
    {

        public static bool TryReadLineNumberTableAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort itemCount) == false)
                return false;

            var items = new LineNumberTableAttributeItemRecord[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                if (reader.TryReadU2(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadU2(out ushort lineNumber) == false)
                    return false;

                items[i] = new LineNumberTableAttributeItemRecord(codeOffset, lineNumber);
            }

            attribute = new LineNumberTableAttributeRecord(items);
            return true;
        }

    }

}
