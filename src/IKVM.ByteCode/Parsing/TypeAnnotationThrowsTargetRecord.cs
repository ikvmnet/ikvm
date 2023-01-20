namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationThrowsTargetRecord(ushort ThrowsTypeIndex) : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort throwsTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationThrowsTargetRecord(throwsTypeIndex);
            return true;
        }

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
            if (writer.TryWriteU2(ThrowsTypeIndex) == false)
                return false;

            return true;
        }

    }

}