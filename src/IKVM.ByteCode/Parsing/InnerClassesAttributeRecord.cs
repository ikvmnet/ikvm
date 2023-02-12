namespace IKVM.ByteCode.Parsing
{
    internal sealed record InnerClassesAttributeRecord(InnerClassesAttributeItemRecord[] Items) : AttributeRecord
    {
        public const string Name = "InnerClasses";

        public static bool TryReadInnerClassesAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var items = new InnerClassesAttributeItemRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (InnerClassesAttributeItemRecord.TryRead(ref reader, out var j) == false)
                    return false;

                items[i] = j;
            }

            attribute = new InnerClassesAttributeRecord(items);
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