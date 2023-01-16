namespace IKVM.ByteCode
{

    public sealed record FullStackMapFrameRecord(byte Tag, ushort OffsetDelta, VerificationTypeInfoRecord[] Locals, VerificationTypeInfoRecord[] Stack) : StackMapFrameRecord(Tag);

}
