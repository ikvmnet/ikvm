namespace IKVM.ByteCode.Parsing
{
    internal sealed record StringConstantRecord(ushort ValueIndex) : ConstantRecord
    {
        /// <summary>
        /// Parses a Class constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadStringConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;

            constant = new StringConstantRecord(nameIndex);
            return true;
        }

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public override int GetSize() =>
            sizeof(ushort);

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(ValueIndex) == false)
                return false;

            return true;
        }
    }
}
