namespace IKVM.ByteCode.Parsing
{
    internal sealed record MethodrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : RefConstantRecord(ClassIndex, NameAndTypeIndex)
    {

        /// <summary>
        /// Parses a Methodref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadMethodrefConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort classIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new MethodrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

        protected override int GetConstantSize()
        {
            var size = 0;
            size += sizeof(ushort);
            size += sizeof(ushort);
            return size;
        }

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(ClassIndex) == false)
                return false;
            if (writer.TryWriteU2(NameAndTypeIndex) == false)
                return false;

            return true;
        }
    }
}
