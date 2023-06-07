namespace IKVM.ByteCode.Parsing
{

    public sealed record RuntimeVisibleParameterAnnotationsAttributeRecord(ParameterAnnotationRecord[] Parameters) : AttributeRecord
    {

        public static bool TryReadRuntimeVisibleParameterAnnotationsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU1(out byte count) == false)
                return false;

            var items = new ParameterAnnotationRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (ParameterAnnotationRecord.TryReadParameterAnnotation(ref reader, out var parameter) == false)
                    return false;

                items[i] = parameter;
            }

            attribute = new RuntimeVisibleParameterAnnotationsAttributeRecord(items);
            return true;
        }

    }

}
