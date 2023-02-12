namespace IKVM.ByteCode.Parsing
{
    internal sealed record PermittedSubclassesAttributeRecord(ushort[] ClassIndexes) : AttributeRecord
    {
        public static bool TryReadPermittedSubclassesAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var classes = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadU2(out ushort classIndex) == false)
                    return false;

                classes[i] = classIndex;
            }

            attribute = new PermittedSubclassesAttributeRecord(classes);
            return true;
        }

        public override int GetSize() =>
            sizeof(ushort) + ClassIndexes.Length * sizeof(ushort);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)ClassIndexes.Length) == false)
                return false;

            foreach (var classIndex in ClassIndexes)
                if (writer.TryWriteU2(classIndex) == false)
                    return false;

            return true;
        }
    }
}
