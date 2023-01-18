namespace IKVM.ByteCode.Parsing
{

    internal sealed record UninitializedVariableInfoRecord(ushort Offset) : VerificationTypeInfoRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadU2(out ushort offset) == false)
                return false;

            record = new UninitializedVariableInfoRecord(offset);
            return true;
        }

    }

}
