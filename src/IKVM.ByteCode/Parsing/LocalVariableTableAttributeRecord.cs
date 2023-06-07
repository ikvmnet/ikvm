namespace IKVM.ByteCode.Parsing
{

    public sealed record LocalVariableTableAttributeRecord(LocalVariableTableAttributeItemRecord[] Items) : AttributeRecord
    {

        public static bool TryReadLocalVariableTableAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort length) == false)
                return false;

            var items = new LocalVariableTableAttributeItemRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (reader.TryReadU2(out ushort codeOffset) == false)
                    return false;
                if (reader.TryReadU2(out ushort codeLength) == false)
                    return false;
                if (reader.TryReadU2(out ushort nameIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort descriptorIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort index) == false)
                    return false;

                items[i] = new LocalVariableTableAttributeItemRecord(codeOffset, codeLength, nameIndex, descriptorIndex, index);
            }

            attribute = new LocalVariableTableAttributeRecord(items);
            return true;
        }

    }

}
