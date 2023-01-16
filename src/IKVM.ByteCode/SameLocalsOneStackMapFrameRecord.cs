namespace IKVM.ByteCode
{

    sealed record SameLocalsOneStackMapFrameRecord(byte FrameType, VerificationTypeInfoRecord Stack) : StackMapFrameRecord(FrameType);

}
