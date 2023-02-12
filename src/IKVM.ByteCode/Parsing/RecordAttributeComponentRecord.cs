namespace IKVM.ByteCode.Parsing
{
    internal record struct RecordAttributeComponentRecord(ushort NameIndex, ushort DescriptorIndex, AttributeInfoRecord[] Attributes)
    {
        public static bool TryRead(ref ClassFormatReader reader, out RecordAttributeComponentRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort descriptorIndex) == false)
                return false;
            if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            record = new RecordAttributeComponentRecord(nameIndex, descriptorIndex, attributes);
            return true;
        }

        public int GetSize()
        {
            var size = 0;

            size += sizeof(ushort);
            size += sizeof(ushort);

            foreach (var attribute in Attributes)
                size += attribute.GetSize();

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(NameIndex) == false)
                return false;
            if (writer.TryWriteU2(DescriptorIndex) == false)
                return false;

            foreach (var attribute in Attributes)
                if (attribute.TryWrite(ref writer) == false)
                    return false;

            return true;
        }
    }
}
