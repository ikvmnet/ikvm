using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementConstantValueRecord(ushort ConstantValueIndex) : ElementValueRecord
    {

        public static bool TryReadElementConstantValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort index) == false)
                return false;

            value = new ElementConstantValueRecord(index);
            return true;
        }

    }

}