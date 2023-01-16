namespace IKVM.ByteCode
{

    public sealed record TypeAnnotationSuperTypeTargetRecord(byte TargetType, ushort SuperTypeIndex) : TypeAnnotationTargetRecord(TargetType);

}