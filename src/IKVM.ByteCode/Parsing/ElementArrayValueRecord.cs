using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementArrayValueRecord(ElementValueRecord[] Values) : ElementValueRecord
    {

        public static bool TryReadElementArrayValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryRead(out ushort length) == false)
                return false;

            var values = new ElementValueRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (TryReadElementValue(ref reader, out var j) == false)
                    return false;

                values[i] = j;
            }

            value = new ElementArrayValueRecord(values);
            return true;
        }

    }


}