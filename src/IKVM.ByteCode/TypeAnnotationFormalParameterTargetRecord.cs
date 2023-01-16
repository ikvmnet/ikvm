namespace IKVM.ByteCode
{

    public sealed record TypeAnnotationFormalParameterTargetRecord(byte TargetType, byte ParameterIndex) : TypeAnnotationTargetRecord(TargetType);

}