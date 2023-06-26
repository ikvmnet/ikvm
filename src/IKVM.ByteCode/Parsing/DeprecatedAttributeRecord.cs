namespace IKVM.ByteCode.Parsing
{
    internal sealed record DeprecatedAttributeRecord : AttributeRecord
    {
        public const string Name = "Deprecated";

        public static bool TryReadDeprecatedAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = new DeprecatedAttributeRecord();
            return true;
        }

        public override int GetSize() => 0;

        public override bool TryWrite(ref ClassFormatWriter writer) => true;
    }
}