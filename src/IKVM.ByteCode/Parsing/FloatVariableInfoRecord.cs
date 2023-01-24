namespace IKVM.ByteCode.Parsing
{

    internal sealed record FloatVariableInfoRecord : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = new FloatVariableInfoRecord();
            return true;
        }

    }

}
