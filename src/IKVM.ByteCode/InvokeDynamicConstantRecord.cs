namespace IKVM.ByteCode
{

    public sealed record InvokeDynamicConstantRecord(ushort BootstrapMethodAttributeIndex, ushort NameAndTypeIndex) : ConstantRecord;

}