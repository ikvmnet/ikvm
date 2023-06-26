namespace IKVM.ByteCode.Parsing
{

    internal sealed record RuntimeInvisibleParameterAnnotationsAttributeRecord(ParameterAnnotationRecord[] Parameters) : AttributeRecord
    {

        public static bool TryReadRuntimeInvisibleParameterAnnotationsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU1(out byte count) == false)
                return false;

            var parameters = new ParameterAnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (ParameterAnnotationRecord.TryReadParameterAnnotation(ref reader, out var parameter) == false)
                    return false;

                parameters[i] = parameter;
            }

            attribute = new RuntimeInvisibleParameterAnnotationsAttributeRecord(parameters);
            return true;
        }

        public override int GetSize()
        {
            throw new System.NotImplementedException();
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

}
