namespace IKVM.ByteCode.Parsing
{

    public record struct InnerClassRecord(ushort InnerClassInfoIndex, ushort OuterClassInfoIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags);

}