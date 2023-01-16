namespace IKVM.ByteCode
{

    public sealed record EnclosingMethodAttributeDataRecord(ushort ClassIndex, ushort MethodIndex) : AttributeDataRecord;

}
