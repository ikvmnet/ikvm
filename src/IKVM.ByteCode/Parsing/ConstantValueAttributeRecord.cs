namespace IKVM.ByteCode.Parsing
{

    internal sealed record ConstantValueAttributeRecord(ushort ValueIndex) : AttributeRecord
    {

        public static bool TryReadConstantValueAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort valueIndex) == false)
                return false;

            attribute = new ConstantValueAttributeRecord(valueIndex);
            return true;
        }

    }

}
