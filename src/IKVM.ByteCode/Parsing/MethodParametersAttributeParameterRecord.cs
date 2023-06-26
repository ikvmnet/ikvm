namespace IKVM.ByteCode.Parsing
{
    internal record struct MethodParametersAttributeParameterRecord(ushort NameIndex, AccessFlag AccessFlags)
    {
        public static bool TryRead(ref ClassFormatReader reader, out MethodParametersAttributeParameterRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort accessFlags) == false)
                return false;

            record = new MethodParametersAttributeParameterRecord(nameIndex, (AccessFlag)accessFlags);
            return true;
        }

        public int GetSize() =>
            sizeof(ushort) + sizeof(ushort);

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(NameIndex) == false)
                return false;
            if (writer.TryWriteU2((ushort)AccessFlags) == false)
                return false;

            return true;
        }
    }

}
