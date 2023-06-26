namespace IKVM.ByteCode.Parsing
{
    internal record struct InnerClassesAttributeItemRecord(ushort InnerClassIndex, ushort OuterClassIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags)
    {
        public static bool TryRead(ref ClassFormatReader reader, out InnerClassesAttributeItemRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort innerClassInfoIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort outerClassInfoIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort innerNameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort innerClassAccessFlags) == false)
                return false;

            record = new InnerClassesAttributeItemRecord(innerClassInfoIndex, outerClassInfoIndex, innerNameIndex, (AccessFlag)innerClassAccessFlags);
            return true;
        }

        public int GetSize() =>
            sizeof(ushort) + sizeof(ushort) + sizeof(ushort) + sizeof(ushort);

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(InnerClassIndex) == false)
                return false;
            if (writer.TryWriteU2(OuterClassIndex) == false)
                return false;
            if (writer.TryWriteU2(InnerNameIndex) == false)
                return false;
            if (writer.TryWriteU2((ushort)InnerClassAccessFlags) == false)
                return false;

            return true;
        }
    }
}
