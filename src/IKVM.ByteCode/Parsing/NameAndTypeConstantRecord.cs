namespace IKVM.ByteCode.Parsing
{
    internal sealed record NameAndTypeConstantRecord(ushort NameIndex, ushort DescriptorIndex) : ConstantRecord
    {
        /// <summary>
        /// Parses a NameAndType constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadNameAndTypeConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort descriptorIndex) == false)
                return false;

            constant = new NameAndTypeConstantRecord(nameIndex, descriptorIndex);
            return true;
        }

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public override int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);
            size += sizeof(ushort);
            return size;
        }

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(NameIndex) == false)
                return false;
            if (writer.TryWriteU2(DescriptorIndex) == false)
                return false;

            return true;
        }
    }
}
