using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record DoubleVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new DoubleVariableInfoRecord();
            return true;
        }

    }

}
