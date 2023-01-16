namespace IKVM.ByteCode
{

    public record struct LocalVariableTableAttributeDataRecordItem(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort DescriptorIndex, ushort Index);

}
