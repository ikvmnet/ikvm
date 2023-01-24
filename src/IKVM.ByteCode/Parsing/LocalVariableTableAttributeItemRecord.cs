namespace IKVM.ByteCode.Parsing
{

    internal record struct LocalVariableTableAttributeItemRecord(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort DescriptorIndex, ushort Index);

}
