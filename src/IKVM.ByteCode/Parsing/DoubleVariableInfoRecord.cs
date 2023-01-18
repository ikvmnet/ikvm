namespace IKVM.ByteCode.Parsing
{

    internal sealed record DoubleVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = new DoubleVariableInfoRecord();
            return true;
        }

    }

}
