namespace IKVM.ByteCode.Parsing
{
    internal sealed record DeprecatedAttributeRecord : AttributeRecord
    {
        public static bool TryReadDeprecatedAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = new DeprecatedAttributeRecord();
            return true;
        }

        protected bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU4(0) == false)
                return false;

            return true;
        }
    }
}