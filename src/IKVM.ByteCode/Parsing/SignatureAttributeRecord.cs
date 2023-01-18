using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    internal record SignatureAttributeRecord(ushort SignatureIndex) : AttributeRecord
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