using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record LineNumberTableAttributeRecord(LineNumberTableAttributeItemRecord[] Items) : AttributeRecord
    {

        public static bool TryReadLineNumberTableAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort itemCount) == false)
                return false;

            var items = new LineNumberTableAttributeItemRecord[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort lineNumber) == false)
                    return false;

                items[i] = new LineNumberTableAttributeItemRecord(codeOffset, lineNumber);
            }

            attribute = new LineNumberTableAttributeRecord(items);
            return true;
        }

    }

}
