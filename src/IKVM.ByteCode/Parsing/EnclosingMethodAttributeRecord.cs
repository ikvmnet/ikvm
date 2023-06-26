namespace IKVM.ByteCode.Parsing
{
    internal sealed record EnclosingMethodAttributeRecord(ushort ClassIndex, ushort MethodIndex) : AttributeRecord
    {
        public static bool TryReadEnclosingMethodAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort classIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort methodIndex) == false)
                return false;

            attribute = new EnclosingMethodAttributeRecord(classIndex, methodIndex);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort) + sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(ClassIndex) == false)
                return false;
            if (writer.TryWriteU2(MethodIndex) == false)
                return false;

            return true;
        }
    }

}
