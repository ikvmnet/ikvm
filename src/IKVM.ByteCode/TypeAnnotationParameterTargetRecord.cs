namespace IKVM.ByteCode
{

    public sealed record TypeAnnotationParameterTargetRecord(byte TargetType, byte ParameterIndex) : TypeAnnotationTargetRecord(TargetType);

}