namespace IKVM.ByteCode.Parsing
{

    public sealed record DeprecatedAttributeRecord : AttributeRecord
    {

        public static bool TryReadDeprecatedAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = new DeprecatedAttributeRecord();
            return true;
        }

    }

}