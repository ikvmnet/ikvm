namespace IKVM.ByteCode
{

    public sealed record InterfaceMethodrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : ConstantRecord;

}