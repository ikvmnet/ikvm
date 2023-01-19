namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementEnumConstantValueRecord(byte Tag, ushort TypeNameIndex, ushort ConstantNameIndex) : ElementValueRecord(Tag)
    {

        public static bool TryRead(ref ClassFormatReader reader, byte tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort typeNameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort constantNameIndex) == false)
                return false;

            value = new ElementEnumConstantValueRecord(tag, typeNameIndex, constantNameIndex);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
            size += sizeof(ushort);
            size += sizeof(ushort);
            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (base.TryWrite(ref writer) == false)
                return false;

            if (writer.TryWriteU2(TypeNameIndex) == false)
                return false;
            if (writer.TryWriteU2(ConstantNameIndex) == false)
                return false;

            return true;
        }

    }

}