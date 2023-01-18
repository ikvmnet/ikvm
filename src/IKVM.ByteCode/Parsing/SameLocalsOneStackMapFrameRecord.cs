namespace IKVM.ByteCode.Parsing
{

    internal sealed record SameLocalsOneStackMapFrameRecord(byte FrameType, VerificationTypeInfoRecord Stack) : StackMapFrameRecord(FrameType)
    {

        public static bool TryReadSameLocalsOneStackItemStackMapFrame(ref ClassFormatReader reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (VerificationTypeInfoRecord.TryReadVerificationTypeInfo(ref reader, out var verificationTypeInfo) == false)
                return false;

            frame = new SameLocalsOneStackMapFrameRecord(tag, verificationTypeInfo);
            return true;
        }

    }

}
