namespace IKVM.ByteCode
{

    public sealed record MethodrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : ConstantRecord;

}
