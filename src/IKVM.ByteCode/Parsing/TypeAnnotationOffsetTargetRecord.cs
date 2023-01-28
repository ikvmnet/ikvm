namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationOffsetTargetRecord(ushort Offset) : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort offset) == false)
                return false;

            targetInfo = new TypeAnnotationCatchTargetRecord(offset);
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

            return true;
        }

    }

}