namespace IKVM.ByteCode
{

    public sealed record ModuleAttributeDataRecord(ushort NameIndex, ModuleFlag Flags, ushort VersionIndex, ModuleAttributeDataRequiresRecord[] Requires, ModuleAttributeDataExportsRecord[] Exports, ModuleAttributeDataOpensRecord[] Opens, ushort[] Uses, ModuleAttributeDataProvidesRecord[] Provides) : AttributeDataRecord;

}
