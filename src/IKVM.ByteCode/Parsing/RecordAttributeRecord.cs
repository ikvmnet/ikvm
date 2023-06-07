namespace IKVM.ByteCode.Parsing
{

    public sealed record RecordAttributeRecord(RecordAttributeComponentRecord[] Components) : AttributeRecord
    {

        public static bool TryReadRecordAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort componentsCount) == false)
                return false;

            var components = new RecordAttributeComponentRecord[componentsCount];
            for (int i = 0; i < componentsCount; i++)
            {
                if (reader.TryReadU2(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort descriptorIndex) == false)
                    return false;
                if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                    return false;

                components[i] = new RecordAttributeComponentRecord(nameIndex, descriptorIndex, attributes);
            }

            attribute = new RecordAttributeRecord(components);
            return true;
        }

    }

}
