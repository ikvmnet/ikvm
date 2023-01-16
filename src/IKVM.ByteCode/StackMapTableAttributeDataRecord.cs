namespace IKVM.ByteCode
{

    public record StackMapTableAttributeDataRecord(StackMapFrameRecord[] Frames) : AttributeDataRecord;

}
