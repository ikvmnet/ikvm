using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record LongVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new LongVariableInfoRecord();
            return true;
        }

    }

}
