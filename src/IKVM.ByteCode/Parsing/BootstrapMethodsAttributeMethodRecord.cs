namespace IKVM.ByteCode.Parsing
{

    public record struct BootstrapMethodsAttributeMethodRecord(ushort MethodRefIndex, ushort[] Arguments)
    {

        public static bool TryReadBootstrapMethod(ref ClassFormatReader reader, out BootstrapMethodsAttributeMethodRecord method)
        {
            method = default;

            if (reader.TryReadU2(out ushort methodRefIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort argumentCount) == false)
                return false;

            var arguments = new ushort[argumentCount];
            for (int i = 0; i < argumentCount; i++)
            {
                if (reader.TryReadU2(out ushort argumentIndex) == false)
                    return false;

                arguments[i] = argumentIndex;
            }

            method = new BootstrapMethodsAttributeMethodRecord(methodRefIndex, arguments);
            return true;
        }

    }

}
