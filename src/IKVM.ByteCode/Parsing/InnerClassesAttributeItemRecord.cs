namespace IKVM.ByteCode.Parsing
{

    internal record struct InnerClassesAttributeItemRecord(ushort InnerClassInfoIndex, ushort OuterClassInfoIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags);

}
