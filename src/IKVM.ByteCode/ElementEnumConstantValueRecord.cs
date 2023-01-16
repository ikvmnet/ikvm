namespace IKVM.ByteCode
{

    public sealed record ElementEnumConstantValueRecord(ushort TypeNameIndex, ushort ConstantNameIndex) : ElementValueRecord;

}