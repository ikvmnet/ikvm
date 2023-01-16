namespace IKVM.ByteCode
{

    public sealed record FieldrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : ConstantRecord;

}