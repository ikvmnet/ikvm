namespace IKVM.ByteCode.Parsing
{
    internal record SignatureAttributeRecord(ushort SignatureIndex) : AttributeRecord
    {
        public const string Name = "Signature";

        public static bool TryReadSignatureAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort signatureIndex) == false)
                return false;

            attribute = new SignatureAttributeRecord(signatureIndex);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(SignatureIndex) == false)
                return false;

            return true;
        }
    }
}