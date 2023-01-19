namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementAnnotationValueRecord(byte Tag, AnnotationRecord Annotation) : ElementValueRecord(Tag)
    {

        public static bool TryReadElementAnnotationValue(ref ClassFormatReader reader, byte tag, out ElementValueRecord value)
        {
            value = null;

            if (AnnotationRecord.TryReadAnnotation(ref reader, out var annotation) == false)
                return false;

            value = new ElementAnnotationValueRecord(tag, annotation);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
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
            if (base.TryWrite(ref writer) == false)
                return false;

            if (Annotation.TryWrite(ref writer) == false)
                return false;

            return true;
        }

    }

}