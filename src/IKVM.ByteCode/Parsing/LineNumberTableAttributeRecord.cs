namespace IKVM.ByteCode.Parsing
{
    internal sealed record LineNumberTableAttributeRecord(LineNumberTableAttributeItemRecord[] Items) : AttributeRecord
    {
        public static bool TryReadLineNumberTableAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort itemCount) == false)
                return false;

            var items = new LineNumberTableAttributeItemRecord[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                if (LineNumberTableAttributeItemRecord.TryRead(ref reader, out var j) == false)
                    return false;

                items[i] = j;
            }

            attribute = new LineNumberTableAttributeRecord(items);
            return true;
        }

        public override int GetSize()
        {
            var size = sizeof(ushort);

            foreach (var item in Items)
                size += item.GetSize();

            return size;
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)Items.Length) == false)
                return false;

            foreach (var item in Items)
                if (item.TryWrite(ref writer) == false)
                    return false;

            return true;
        }
    }
}
