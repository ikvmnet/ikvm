namespace IKVM.ByteCode.Parsing
{
    internal sealed record RecordAttributeRecord(RecordAttributeComponentRecord[] Components) : AttributeRecord
    {
        public static bool TryReadRecordAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort componentsCount) == false)
                return false;

            var components = new RecordAttributeComponentRecord[componentsCount];
            for (int i = 0; i < componentsCount; i++)
            {
                if (RecordAttributeComponentRecord.TryRead(ref reader, out var j) == false)
                    return false;

                components[i] = j;
            }

            attribute = new RecordAttributeRecord(components);
            return true;
        }

        public override int GetSize()
        {
            var size = sizeof(ushort);

            foreach (var component in Components)
                size += component.GetSize();

            return size;
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)Components.Length) == false)
                return false;

            foreach (var component in Components)
                if (component.TryWrite(ref writer) == false)
                    return false;

            return true;
        }
    }
}
