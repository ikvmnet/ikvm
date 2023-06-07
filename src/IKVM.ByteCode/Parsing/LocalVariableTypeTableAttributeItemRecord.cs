namespace IKVM.ByteCode.Parsing
{

    public record struct LocalVariableTypeTableAttributeItemRecord(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort SignatureIndex, ushort Index);

}
