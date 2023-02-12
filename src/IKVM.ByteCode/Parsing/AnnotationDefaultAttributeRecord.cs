namespace IKVM.ByteCode.Parsing
{
    internal sealed record AnnotationDefaultAttributeRecord(ElementValueRecord DefaultValue) : AttributeRecord
    {
        public static bool TryReadAnnotationDefaultAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (ElementValueRecord.TryRead(ref reader, out var defaultValue) == false)
                return false;

            attribute = new AnnotationDefaultAttributeRecord(defaultValue);
            return true;
        }

        public override int GetSize() => DefaultValue.GetSize();

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (DefaultValue.TryWrite(ref writer) == false)
                return false;

            return true;
        }
    }
}
