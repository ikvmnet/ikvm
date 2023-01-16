namespace IKVM.ByteCode
{

    sealed record SameStackMapFrameRecord(byte Tag) : StackMapFrameRecord(Tag);

}
