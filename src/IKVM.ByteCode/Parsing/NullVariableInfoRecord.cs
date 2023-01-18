using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record NullVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out VerificationTypeInfoRecord record)
        {
            record = new NullVariableInfoRecord();
            return true;
        }

    }    

}
