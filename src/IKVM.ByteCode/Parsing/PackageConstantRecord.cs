namespace IKVM.ByteCode.Parsing
{
    internal sealed record PackageConstantRecord(ushort NameIndex) : ConstantRecord
    {
        /// <summary>
        /// Parses a Package constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadPackageConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;

            constant = new PackageConstantRecord(nameIndex);
            return true;
        }

        protected override int GetConstantSize() =>
            sizeof(ushort);

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(NameIndex) == false)
                return false;

            return true;
        }
    }
}
