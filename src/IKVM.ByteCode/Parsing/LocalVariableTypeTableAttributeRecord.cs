using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record LocalVariableTypeTableAttributeRecord(LocalVariableTypeTableAttributeItemRecord[] Items) : AttributeRecord
    {

        public static bool TryReadLocalVariableTypeTableAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;

            var items = new LocalVariableTypeTableAttributeItemRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (reader.TryReadBigEndian(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort codeLength) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort signatureIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort index) == false)
                    return false;

                items[i] = new LocalVariableTypeTableAttributeItemRecord(codeOffset, codeLength, nameIndex, signatureIndex, index);
            }

            attribute = new LocalVariableTypeTableAttributeRecord(items);
            return true;
        }

    }

}
