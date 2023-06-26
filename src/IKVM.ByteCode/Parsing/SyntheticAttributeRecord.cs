namespace IKVM.ByteCode.Parsing
{
    internal record SyntheticAttributeRecord : AttributeRecord
    {
        public static bool TryReadSyntheticAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = new SyntheticAttributeRecord();
            return true;
        }

        public override int GetSize() => 0;

        public override bool TryWrite(ref ClassFormatWriter writer) => true;
    }
}
