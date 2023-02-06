namespace IKVM.ByteCode.Parsing
{
    internal record struct MethodRecord(AccessFlag AccessFlags, ushort NameIndex, ushort DescriptorIndex, AttributeInfoRecord[] Attributes)
    {
        /// <summary>
        /// Parses a method.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="method"></param>
        public static bool TryRead(ref ClassFormatReader reader, out MethodRecord method)
        {
            method = default;

            if (reader.TryReadU2(out ushort accessFlags) == false)
                return false;
            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort descriptorIndex) == false)
                return false;
            if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            method = new MethodRecord((AccessFlag)accessFlags, nameIndex, descriptorIndex, attributes);
            return true;
        }

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)AccessFlags) == false)
                return false;
            if (writer.TryWriteU2(NameIndex) == false)
                return false;
            if (writer.TryWriteU2(DescriptorIndex) == false)
                return false;
            if (ClassRecord.TryWriteAttributes(ref writer, Attributes) == false)
                return false;

            return true;
        }
    }

}
