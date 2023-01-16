namespace IKVM.ByteCode
{

    public sealed record ChopStackMapFrameRecord(byte Tag, ushort OffsetDelta) : StackMapFrameRecord(Tag);

}
