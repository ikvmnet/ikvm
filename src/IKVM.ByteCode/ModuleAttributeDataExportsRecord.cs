namespace IKVM.ByteCode
{

    public record struct ModuleAttributeDataExportsRecord(ushort Index, ModuleExportsFlag Flags, ushort[] Modules);

}
