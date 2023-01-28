namespace IKVM.ByteCode.Parsing
{

    internal sealed record LongVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = new LongVariableInfoRecord();
            return true;
        }

    }

}
