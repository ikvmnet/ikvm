namespace IKVM.ByteCode.Parsing
{

    public sealed record InnerClassesAttributeRecord(InnerClassesAttributeItemRecord[] Items) : AttributeRecord
    {

        public static bool TryReadInnerClassesAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var items = new InnerClassesAttributeItemRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadU2(out ushort innerClassInfoIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort outerClassInfoIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort innerNameIndex) == false)
                    return false;
                if (reader.TryReadU2(out ushort innerClassAccessFlags) == false)
                    return false;

                items[i] = new InnerClassesAttributeItemRecord(innerClassInfoIndex, outerClassInfoIndex, innerNameIndex, (AccessFlag)innerClassAccessFlags);
            }

            attribute = new InnerClassesAttributeRecord(items);
            return true;
        }

    }

}
