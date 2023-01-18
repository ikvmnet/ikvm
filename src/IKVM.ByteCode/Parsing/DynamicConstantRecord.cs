namespace IKVM.ByteCode.Parsing
{

    internal sealed record DynamicConstantRecord(ushort BootstrapMethodAttributeIndex, ushort NameAndTypeIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a Dynamic constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skipIndex"></param>
        public static bool TryReadDynamicConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skipIndex)
        {
            constant = null;
            skipIndex = 0;

            if (reader.TryReadU2(out ushort bootstrapMethodAttrIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new DynamicConstantRecord(bootstrapMethodAttrIndex, nameAndTypeIndex);
            return true;
        }

    }

}
