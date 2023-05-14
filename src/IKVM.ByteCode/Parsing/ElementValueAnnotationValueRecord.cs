namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementValueAnnotationValueRecord(AnnotationRecord Annotation) : ElementValueValueRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out ElementValueValueRecord value)
        {
            value = null;

            if (AnnotationRecord.TryReadAnnotation(ref reader, out var annotation) == false)
                return false;

            value = new ElementValueAnnotationValueRecord(annotation);
            return true;
        }

        public override int GetSize()
        {
            var size = 0;
            size += Annotation.GetSize();
            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (Annotation.TryWrite(ref writer) == false)
                return false;

            return true;
        }

    }

}
