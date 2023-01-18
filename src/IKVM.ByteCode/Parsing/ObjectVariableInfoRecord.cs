namespace IKVM.ByteCode.Parsing
{

    internal sealed record ObjectVariableInfoRecord(ushort ClassIndex) : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadU2(out ushort classIndex) == false)
                return false;

            record = new ObjectVariableInfoRecord(classIndex);
            return true;
        }

    }

}
