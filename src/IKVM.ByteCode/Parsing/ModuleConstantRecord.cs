namespace IKVM.ByteCode.Parsing
{

    public sealed record ModuleConstantRecord(ushort NameIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a Module constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadModuleConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;

            constant = new ModuleConstantRecord(nameIndex);
            return true;
        }

    }

}
