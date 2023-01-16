namespace IKVM.ByteCode
{

    public record struct LocalVariableTypeTableAttributeDataRecordItem(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort SignatureIndex, ushort Index);

}
