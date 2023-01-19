namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementConstantValueRecord(byte Tag, ushort ConstantValueIndex) : ElementValueRecord(Tag)
    {

        public static bool TryReadElementConstantValue(ref ClassFormatReader reader, byte tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort index) == false)
                return false;

            value = new ElementConstantValueRecord(tag, index);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
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

            if (writer.TryWriteU2(ConstantValueIndex) == false)
                return false;

            return true;
        }

    }

}