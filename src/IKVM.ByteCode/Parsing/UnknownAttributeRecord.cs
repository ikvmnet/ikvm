namespace IKVM.ByteCode.Parsing
{

    internal sealed record UnknownAttributeRecord(byte[] Data) : AttributeRecord
    {

        public static bool TryReadCustomAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            var data = new byte[reader.Length];
            reader.TryCopyTo(data);

            attribute = new UnknownAttributeRecord(data);
            return true;
        }

    }

}
