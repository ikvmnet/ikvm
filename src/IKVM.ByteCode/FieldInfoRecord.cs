namespace IKVM.ByteCode
{

    public record struct FieldInfoRecord(AccessFlag AccessFlags, ushort NameIndex, ushort DescriptorIndex, AttributeInfoRecord[] Attributes);

}
