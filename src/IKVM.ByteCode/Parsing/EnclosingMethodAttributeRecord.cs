namespace IKVM.ByteCode.Parsing
{

    public sealed record EnclosingMethodAttributeRecord(ushort ClassIndex, ushort MethodIndex) : AttributeRecord
    {

        public static bool TryReadEnclosingMethodAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort classIndex) == false)
                return false;
            if (reader.TryReadU2(out ushort methodIndex) == false)
                return false;

            attribute = new EnclosingMethodAttributeRecord(classIndex, methodIndex);
            return true;
        }

    }

}
