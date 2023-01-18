namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementClassInfoValueRecord(char Tag, ushort ClassInfoIndex) : ElementValueRecord(Tag)
    {

        public static bool TryReadElementClassInfoValue(ref ClassFormatReader reader, char tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort classInfoIndex) == false)
                return false;

            value = new ElementClassInfoValueRecord(tag, classInfoIndex);
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

            if (writer.TryWriteU2(ClassInfoIndex) == false)
                return false;

            return true;
        }

    }

}