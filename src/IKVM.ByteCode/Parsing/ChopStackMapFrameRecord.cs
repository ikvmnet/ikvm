namespace IKVM.ByteCode.Parsing
{

    internal sealed record ChopStackMapFrameRecord(byte Tag, ushort OffsetDelta) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadChopStackMapFrame(ref ClassFormatReader reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadU2(out ushort offsetDelta) == false)
                return false;

            frame = new ChopStackMapFrameRecord(tag, offsetDelta);
            return true;
        }

    }

}
