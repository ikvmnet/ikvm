namespace IKVM.ByteCode.Parsing
{

    public record struct InnerClassesAttributeItemRecord(ushort InnerClassIndex, ushort OuterClassIndex, ushort InnerNameIndex, AccessFlag InnerClassAccessFlags);

}
