namespace IKVM.ByteCode.Parsing
{

    internal sealed record IntegerConstantRecord(int Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Integer constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="record"></param>
        /// <param name="skip"></param>
        public static bool TryReadIntegerConstant(ref ClassFormatReader reader, out ConstantRecord record, out int skip)
        {
            record = null;
            skip = 0;

            if (reader.TryReadU4(out uint value) == false)
                return false;

            var v = unchecked((int)value);

            record = new IntegerConstantRecord(v);
            return true;
        }

    }

}
