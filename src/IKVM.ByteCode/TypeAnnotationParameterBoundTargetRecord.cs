namespace IKVM.ByteCode
{

    public sealed record TypeAnnotationParameterBoundTargetRecord(byte TargetType, byte ParameterIndex, byte BoundIndex) : TypeAnnotationTargetRecord(TargetType);

}