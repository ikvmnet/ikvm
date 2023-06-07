namespace IKVM.ByteCode.Parsing
{

    public record struct LocalVariableTableAttributeItemRecord(ushort CodeOffset, ushort CodeLength, ushort NameIndex, ushort DescriptorIndex, ushort Index);

}
