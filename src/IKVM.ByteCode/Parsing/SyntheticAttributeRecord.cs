namespace IKVM.ByteCode.Parsing
{

    public record SyntheticAttributeRecord : AttributeRecord
    {

        public static bool TryReadSyntheticAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = new SyntheticAttributeRecord();
            return true;
        }

    }

}
