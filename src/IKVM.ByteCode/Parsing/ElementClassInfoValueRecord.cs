using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementClassInfoValueRecord(ushort ClassInfoIndex) : ElementValueRecord
    {

        public static bool TryReadElementClassInfoValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort classInfoIndex) == false)
                return false;

            value = new ElementClassInfoValueRecord(classInfoIndex);
            return true;
        }

    }

}