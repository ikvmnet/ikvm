namespace IKVM.ByteCode
{

    public sealed record DynamicConstantRecord(ushort BootstrapMethodAttributeIndex, ushort NameAndTypeIndex) : ConstantRecord;

}
