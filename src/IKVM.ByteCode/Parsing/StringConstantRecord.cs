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

    }

}
