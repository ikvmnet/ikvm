namespace IKVM.ByteCode
{

    public sealed record TypeAnnotationThrowsTargetRecord(byte TargetType, ushort ThrowsTypeIndex) : TypeAnnotationTargetRecord(TargetType);

}