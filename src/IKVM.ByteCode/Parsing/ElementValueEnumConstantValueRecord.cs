namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementValueEnumConstantValueRecord(ushort TypeNameIndex, ushort ConstantNameIndex) : ElementValueValueRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out ElementValueValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort typeNameIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort constantNameIndex) == false)
                return false;

            value = new ElementValueEnumConstantValueRecord(typeNameIndex, constantNameIndex);
            return true;
        }

        public override int GetSize()
        {
            var size = 0;
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
            if (writer.TryWriteU2(TypeNameIndex) == false)
                return false;
            if (writer.TryWriteU2(ConstantNameIndex) == false)
                return false;

            return true;
        }

    }

}
