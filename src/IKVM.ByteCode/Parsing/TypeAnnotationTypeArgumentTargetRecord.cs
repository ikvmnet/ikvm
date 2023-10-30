namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationTypeArgumentTargetRecord(ushort Offset, byte TypeArgumentIndex) : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort offset) == false)
                return false;
            if (reader.TryReadU1(out byte typeArgumentIndex) == false)
                return false;

            targetInfo = new TypeAnnotationTypeArgumentTargetRecord(offset, typeArgumentIndex);
            return true;
        }

        /// <summary>
        /// Gets the size of the record if written.
        /// </summary>
        /// <returns></returns>
        public override int GetSize()
        {
            var length = 0;
            length += sizeof(ushort);
            length += sizeof(byte);
            return length;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(Offset) == false)
                return false;
            if (writer.TryWriteU1(TypeArgumentIndex) == false)
                return false;

            return true;
        }

    }

}