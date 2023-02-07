namespace IKVM.ByteCode.Parsing
{
    internal sealed record InvokeDynamicConstantRecord(ushort BootstrapMethodAttributeIndex, ushort NameAndTypeIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a InvokeDynamic constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadInvokeDynamicConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort bootstrapMethodAttrIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new InvokeDynamicConstantRecord(bootstrapMethodAttrIndex, nameAndTypeIndex);
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
            if (writer.TryWriteU2(BootstrapMethodAttributeIndex) == false)
                return false;
            if (writer.TryWriteU2(NameAndTypeIndex) == false)
                return false;

            return true;
        }
    }
}