namespace IKVM.ByteCode.Parsing
{

    public abstract record StackMapFrameRecord(byte FrameType)
    {

        public static bool TryRead(ref ClassFormatReader reader, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadU1(out byte tag) == false)
                return false;

            if (TryRead(ref reader, tag, out frame) == false)
                return false;

            return true;
        }

        public static bool TryRead(ref ClassFormatReader reader, byte tag, out StackMapFrameRecord frame)
        {
            if (tag >= 0 && tag <= 63)
                return SameStackMapFrameRecord.TryReadSameStackMapFrame(ref reader, tag, out frame);
            else if (tag >= 64 && tag <= 127)
                return SameLocalsOneStackMapFrameRecord.TryReadSameLocalsOneStackItemStackMapFrame(ref reader, tag, out frame);
            else if (tag == 247)
                return SameLocalsOneExtendedStackMapFrameRecord.TryReadSameLocalsOneStackItemExtendedStackMapFrame(ref reader, tag, out frame);
            else if (tag >= 248 && tag <= 250)
                return ChopStackMapFrameRecord.TryReadChopStackMapFrame(ref reader, tag, out frame);
            else if (tag == 251)
                return SameExtendedStackMapFrameRecord.TryReadSameExtendedStackMapFrame(ref reader, tag, out frame);
            else if (tag >= 252 && tag <= 254)
                return AppendStackMapFrameRecord.TryReadAppendStackMapFrame(ref reader, tag, out frame);
            else if (tag == 255)
                return FullStackMapFrameRecord.TryReadFullStackMapFrame(ref reader, tag, out frame);
            else
                throw new ByteCodeException($"Invalid stack map frame tag value: '{tag}'.");
        }

    }

}
