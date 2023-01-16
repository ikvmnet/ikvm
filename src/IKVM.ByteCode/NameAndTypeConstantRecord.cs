namespace IKVM.ByteCode
{

    public sealed record NameAndTypeConstantRecord(ushort NameIndex, ushort DescriptorIndex) : ConstantRecord;

}
