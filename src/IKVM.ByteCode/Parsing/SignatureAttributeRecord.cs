namespace IKVM.ByteCode.Parsing
{

    public record SignatureAttributeRecord(ushort SignatureIndex) : AttributeRecord
    {

        public static bool TryReadSignatureAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort signatureIndex) == false)
                return false;

            attribute = new SignatureAttributeRecord(signatureIndex);
            return true;
        }

    }

}