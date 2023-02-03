namespace IKVM.ByteCode.Parsing
{

    internal abstract record VerificationTypeInfoRecord
    {

        public static bool TryReadVerificationTypeInfo(ref ClassFormatReader reader, out VerificationTypeInfoRecord record)
        {
            record = null;

            if (reader.TryReadU1(out byte tag) == false)
                return false;

            return tag switch
            {
                0 => TopVariableInfoRecord.TryRead(ref reader, out record),
                1 => IntegerVariableInfoRecord.TryRead(ref reader, out record),
                2 => FloatVariableInfoRecord.TryRead(ref reader, out record),
                3 => DoubleVariableInfoRecord.TryRead(ref reader, out record),
                4 => LongVariableInfoRecord.TryRead(ref reader, out record),
                5 => NullVariableInfoRecord.TryRead(ref reader, out record),
                6 => UninitializedThisVariableInfoRecord.TryRead(ref reader, out record),
                7 => ObjectVariableInfoRecord.TryRead(ref reader, out record),
                8 => UninitializedVariableInfoRecord.TryRead(ref reader, out record),
                _ => throw new ByteCodeException($"Invalid verification info tag: '{tag}'."),
            };
        }

    }

}
