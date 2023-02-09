namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementValueArrayValueRecord(ElementValueRecord[] Values) : ElementValueValueRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out ElementValueValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort length) == false)
                return false;

            var values = new ElementValueRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (ElementValueRecord.TryRead(ref reader, out var j) == false)
                    return false;

                values[i] = j;
            }

            value = new ElementValueArrayValueRecord(values);
            return true;
        }

        public override int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);

            foreach (var value in Values)
                size += value.GetSize();

            return size;
        }

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2((ushort)Values.Length) == false)
                return false;

            foreach (var value in Values)
                if (value.TryWrite(ref writer) == false)
                    return false;

            return true;
        }

    }


}
