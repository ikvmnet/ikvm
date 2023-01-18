using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementArrayValueRecord(char Tag, ElementValueRecord[] Values) : ElementValueRecord(Tag)
    {

        public static bool TryReadElementArrayValue(ref SequenceReader<byte> reader, char tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;

            var values = new ElementValueRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (TryRead(ref reader, out var j) == false)
                    return false;

                values[i] = j;
            }

            value = new ElementArrayValueRecord(tag, values);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
            size += sizeof(ushort);

            foreach (var value in Values)
                size += value.GetSize();

            return size;
        }

    }


}