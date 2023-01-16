namespace IKVM.ByteCode
{

    sealed record SameExtendedStackMapFrameRecord(byte Tag, ushort OffsetDelta) : StackMapFrameRecord(Tag);

}
