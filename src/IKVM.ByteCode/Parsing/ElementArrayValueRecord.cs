namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementArrayValueRecord(byte Tag, ElementValueRecord[] Values) : ElementValueRecord(Tag)
    {

        public static bool TryReadElementArrayValue(ref ClassFormatReader reader, byte tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort length) == false)
                return false;

            var values = new ElementValueRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (TryRead(ref reader, out var j) == false)
                    return false;

                values[i] = j;
            }

            value = new ElementArrayValueRecord(tag, values);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
            size += sizeof(ushort);

            foreach (var value in Values)
                size += value.GetSize();

            return size;
        }

    }


}