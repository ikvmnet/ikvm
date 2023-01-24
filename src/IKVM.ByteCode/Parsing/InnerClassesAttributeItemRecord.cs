namespace IKVM.ByteCode.Parsing
{

    internal record struct InnerClassesAttributeItemRecord(ushort InnerClassIndex, ushort OuterClassIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags);

}
