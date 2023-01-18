using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record FloatVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new FloatVariableInfoRecord();
            return true;
        }

    }

}
