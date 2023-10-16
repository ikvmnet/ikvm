namespace IKVM.ByteCode.Parsing
{

    internal sealed record SameExtendedStackMapFrameRecord(byte Tag, ushort OffsetDelta) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadSameExtendedStackMapFrame(ref ClassFormatReader reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadU2(out ushort offsetDelta) == false)
                return false;

            frame = new SameExtendedStackMapFrameRecord(tag, offsetDelta);
            return true;
        }

    }


}
