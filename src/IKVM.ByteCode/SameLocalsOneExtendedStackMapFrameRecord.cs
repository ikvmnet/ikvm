namespace IKVM.ByteCode
{

    sealed record SameLocalsOneExtendedStackMapFrameRecord(byte FrameType, ushort OffsetDelta, VerificationTypeInfoRecord Stack) : StackMapFrameRecord(FrameType);

}
