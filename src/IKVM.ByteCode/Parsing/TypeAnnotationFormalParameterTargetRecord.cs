namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationFormalParameterTargetRecord(byte TargetType, byte ParameterIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryRead(ref ClassFormatReader reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU1(out byte parameterIndex) == false)
                return false;

            targetInfo = new TypeAnnotationFormalParameterTargetRecord(targetType, parameterIndex);
            return true;
        }

        public override int GetSize()
        {
            var length = base.GetSize();
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
            if (base.TryWrite(ref writer) == false)
                return false;

            if (writer.TryWriteU1(ParameterIndex) == false)
                return false;

            return true;
        }

    }

}