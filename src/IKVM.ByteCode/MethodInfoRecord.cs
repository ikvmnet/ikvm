namespace IKVM.ByteCode
{

    public record struct MethodInfoRecord(AccessFlag AccessFlags, ushort NameIndex, ushort DescriptorIndex, AttributeInfoRecord[] Attributes);

}
