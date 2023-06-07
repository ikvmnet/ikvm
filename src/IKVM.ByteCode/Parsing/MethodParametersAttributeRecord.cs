namespace IKVM.ByteCode.Parsing
{

    public sealed record MethodParametersAttributeRecord(MethodParametersAttributeParameterRecord[] Parameters) : AttributeRecord
    {

        public static bool TryReadMethodParametersAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU1(out byte count) == false)
                return false;

            var arguments = new MethodParametersAttributeParameterRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadU2(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort accessFlags) == false)
                    return false;

                arguments[i] = new MethodParametersAttributeParameterRecord(nameIndex, (AccessFlag)accessFlags);
            }

            attribute = new MethodParametersAttributeRecord(arguments);
            return true;
        }

    }

}
