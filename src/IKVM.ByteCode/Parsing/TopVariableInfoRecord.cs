namespace IKVM.ByteCode.Parsing
{

    internal sealed record TopVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = new TopVariableInfoRecord();
            return true;
        }

    }

}
