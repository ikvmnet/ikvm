namespace IKVM.ByteCode.Parsing
{

    public sealed record BootstrapMethodsAttributeRecord(BootstrapMethodsAttributeMethodRecord[] Methods) : AttributeRecord
    {

        public static bool TryReadBootstrapMethodsAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var methods = new BootstrapMethodsAttributeMethodRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (BootstrapMethodsAttributeMethodRecord.TryReadBootstrapMethod(ref reader, out var method) == false)
                    return false;

                methods[i] = method;
            }

            attribute = new BootstrapMethodsAttributeRecord(methods);
            return true;
        }

    }

}
