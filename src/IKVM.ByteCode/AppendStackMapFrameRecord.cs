namespace IKVM.ByteCode
{

    sealed record AppendStackMapFrameRecord(byte Tag, ushort OffsetDelta, VerificationTypeInfoRecord[] Locals) : StackMapFrameRecord(Tag);

}
