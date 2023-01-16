namespace IKVM.ByteCode
{

    public record struct ClassRecord(ushort MinorVersion, ushort MajorVersion, ConstantRecord[] Constants, AccessFlag AccessFlags, ushort ThisClassIndex, ushort SuperClassIndex, InterfaceInfoRecord[] Interfaces, FieldInfoRecord[] Fields, MethodInfoRecord[] Methods, AttributeInfoRecord[] Attributes);

}
