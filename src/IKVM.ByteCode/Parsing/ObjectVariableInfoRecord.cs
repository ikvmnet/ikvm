using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ObjectVariableInfoRecord(ushort ClassIndex) : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;

            record = new ObjectVariableInfoRecord(classIndex);
            return true;
        }

    }

}
