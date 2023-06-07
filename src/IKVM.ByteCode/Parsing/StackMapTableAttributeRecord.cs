namespace IKVM.ByteCode.Parsing
{

    public sealed record StackMapTableAttributeRecord(StackMapFrameRecord[] Frames) : AttributeRecord
    {

        public static bool TryRead(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort count) == false)
                return false;

            var frames = new StackMapFrameRecord[count];
            for (int i = 0; i < count; i++)
                if (StackMapFrameRecord.TryRead(ref reader, out frames[i]) == false)
                    return false;

            attribute = new StackMapTableAttributeRecord(frames);
            return true;
        }

    }

}
