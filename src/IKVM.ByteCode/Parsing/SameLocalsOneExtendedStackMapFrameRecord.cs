namespace IKVM.ByteCode.Parsing
{

    internal sealed record SameLocalsOneExtendedStackMapFrameRecord(byte FrameType, ushort OffsetDelta, VerificationTypeInfoRecord Stack) : StackMapFrameRecord(FrameType)
    {

        public static bool TryReadSameLocalsOneStackItemExtendedStackMapFrame(ref ClassFormatReader reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadU2(out ushort offsetDelta) == false)
                return false;
            if (VerificationTypeInfoRecord.TryReadVerificationTypeInfo(ref reader, out var verificationTypeInfo) == false)
                return false;

            frame = new SameLocalsOneExtendedStackMapFrameRecord(tag, offsetDelta, verificationTypeInfo);
            return true;
        }

    }

}
