namespace IKVM.ByteCode
{

    public record struct InnerClassRecord(ushort InnerClassInfoIndex, ushort OuterClassInfoIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags);

}