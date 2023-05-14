namespace IKVM.ByteCode.Parsing
{

    internal record struct LocalVariableTypeTableAttributeItemRecord(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort SignatureIndex, ushort Index);

}
