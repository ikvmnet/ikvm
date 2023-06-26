namespace IKVM.ByteCode.Parsing
{
    internal sealed record NestHostAttributeRecord(ushort HostClassIndex) : AttributeRecord
    {
        public static bool TryReadNestHostAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort hostClassIndex) == false)
                return false;

            attribute = new NestHostAttributeRecord(hostClassIndex);
            return true;
        }

        public override int GetSize() => sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(HostClassIndex) == false)
                return false;

            return true;
        }
    }
}
