namespace IKVM.ByteCode.Parsing
{

    public record struct RecordAttributeComponentRecord(ushort NameIndex, ushort DescriptorIndex, AttributeInfoRecord[] Attributes);

}
