namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationSuperTypeTargetRecord(byte TargetType, ushort SuperTypeIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryRead(ref ClassFormatReader reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort superTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationSuperTypeTargetRecord(targetType, superTypeIndex);
            return true;
        }

        public override int GetSize()
        {
            var length = base.GetSize();
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
            if (base.TryWrite(ref writer) == false)
                return false;

            if (writer.TryWriteU2(SuperTypeIndex) == false)
                return false;

            return true;
        }

    }

}