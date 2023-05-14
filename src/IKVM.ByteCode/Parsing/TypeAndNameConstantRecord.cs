namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAndNameConstantRecord(ushort NameIndex, ushort DescriptorIndex) : ConstantRecord;

}
