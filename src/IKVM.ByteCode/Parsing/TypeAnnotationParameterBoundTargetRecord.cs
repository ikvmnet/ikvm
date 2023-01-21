namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationParameterBoundTargetRecord(byte ParameterIndex, byte BoundIndex) : TypeAnnotationTargetRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU1(out byte parameterIndex) == false)
                return false;
            if (reader.TryReadU1(out byte boundIndex) == false)
                return false;

            targetInfo = new TypeAnnotationParameterBoundTargetRecord(parameterIndex, boundIndex);
            return true;
        }

        public override int GetSize()
        {
            var length = 0;
            length += sizeof(byte);
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
            if (writer.TryWriteU1(ParameterIndex) == false)
                return false;
            if (writer.TryWriteU1(BoundIndex) == false)
                return false;

            return true;
        }

    }

}