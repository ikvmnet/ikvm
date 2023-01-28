namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationSuperTypeTargetRecord(ushort SuperTypeIndex) : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort superTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationSuperTypeTargetRecord(superTypeIndex);
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
            if (writer.TryWriteU2(SuperTypeIndex) == false)
                return false;

            return true;
        }

    }

}