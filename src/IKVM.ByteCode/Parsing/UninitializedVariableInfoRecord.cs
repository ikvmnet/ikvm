using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record UninitializedVariableInfoRecord(ushort Offset) : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadBigEndian(out ushort offset) == false)
                return false;

            record = new UninitializedVariableInfoRecord(offset);
            return true;
        }

    }

}
