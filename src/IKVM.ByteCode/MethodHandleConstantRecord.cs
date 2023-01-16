namespace IKVM.ByteCode
{

    public sealed record MethodHandleConstantRecord(ReferenceKind Kind, ushort ReferenceIndex) : ConstantRecord;

}
