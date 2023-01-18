namespace IKVM.ByteCode.Parsing
{

    public record struct InnerClassesAttributeItemRecord(ushort InnerClassInfoIndex, ushort OuterClassInfoIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags);

}
