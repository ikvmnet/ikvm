namespace IKVM.ByteCode.Parsing
{
    internal sealed record MethodParametersAttributeRecord(MethodParametersAttributeParameterRecord[] Parameters) : AttributeRecord
    {
        public const string Name = "MethodParameters";

        public static bool TryReadMethodParametersAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU1(out byte count) == false)
                return false;

            var arguments = new MethodParametersAttributeParameterRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (MethodParametersAttributeParameterRecord.TryRead(ref reader, out var j) == false)
                    return false;

                arguments[i] = j;
            }

            attribute = new MethodParametersAttributeRecord(arguments);
            return true;
        }

        public override int GetSize()
        {
            var size = 0;

            size += sizeof(byte);

            foreach (var argument in Parameters)
                size += argument.GetSize();

            return size;
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU1((byte)Parameters.Length) == false)
                return false;

            foreach (var argument in Parameters)
                if (argument.TryWrite(ref writer) == false)
                    return false;

            return true;
        }
    }
}