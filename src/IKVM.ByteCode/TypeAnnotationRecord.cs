namespace IKVM.ByteCode
{

    public readonly record struct TypeAnnotationRecord(TypeAnnotationTargetRecord Target, TypePathRecord TargetPath, ushort TypeIndex, ElementValuePairRecord[] Elements);

}
