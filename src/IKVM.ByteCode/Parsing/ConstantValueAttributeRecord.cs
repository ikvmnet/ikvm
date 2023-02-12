namespace IKVM.ByteCode.Parsing
{
    internal sealed record ConstantValueAttributeRecord(ushort ValueIndex) : AttributeRecord
    {
        public const string Name = "ConstantValue";

        public static bool TryReadConstantValueAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort valueIndex) == false)
                return false;

            attribute = new ConstantValueAttributeRecord(valueIndex);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(ValueIndex) == false)
                return false;

            return true;
        }
    }
}
