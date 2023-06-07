namespace IKVM.ByteCode.Parsing
{

    public sealed record RuntimeInvisibleParameterAnnotationsAttributeRecord(ParameterAnnotationRecord[] Parameters) : AttributeRecord
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

    }

}
