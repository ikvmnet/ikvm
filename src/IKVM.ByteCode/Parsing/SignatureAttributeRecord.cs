using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record SignatureAttributeRecord(ushort SignatureIndex) : AttributeRecord
    {

        public static bool TryReadSignatureAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort signatureIndex) == false)
                return false;

            attribute = new SignatureAttributeRecord(signatureIndex);
            return true;
        }

    }

}