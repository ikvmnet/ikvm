namespace IKVM.ByteCode.Parsing
{

    internal sealed record FieldrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : RefConstantRecord(ClassIndex, NameAndTypeIndex)
    {

        /// <summary>
        /// Parses a Fieldref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadFieldrefConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort classIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new FieldrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

    }

}
