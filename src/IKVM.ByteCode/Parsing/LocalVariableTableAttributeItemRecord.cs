namespace IKVM.ByteCode.Parsing
{
    internal record struct LocalVariableTableAttributeItemRecord(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort DescriptorIndex, ushort Index)
    {
        public static bool TryRead(ref ClassFormatReader reader, out LocalVariableTableAttributeItemRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort codeOffset) == false)
                return false;
            if (reader.TryReadU2(out ushort codeLength) == false)
                return false;
            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort descriptorIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort index) == false)
                return false;

            record = new LocalVariableTableAttributeItemRecord(codeOffset, codeLength, nameIndex, descriptorIndex, index);
            return true;
        }

        public int GetSize()
        {
            var size = 0;
            
            size += sizeof(ushort);
            size += sizeof(ushort);
            size += sizeof(ushort);
            size += sizeof(ushort);
            size += sizeof(ushort);

            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(CodeOffset) == false)
                return false;
            if (writer.TryWriteU2(CodeLength) == false)
                return false;
            if (writer.TryWriteU2(NameIndex) == false)
                return false;
            if (writer.TryWriteU2(DescriptorIndex) == false)
                return false;
            if (writer.TryWriteU2(Index) == false)
                return false;

            return true;
        }
    }
}
