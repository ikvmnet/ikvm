namespace IKVM.ByteCode.Parsing
{

    public record struct ModuleAttributeExportsRecord(ushort Index, ModuleExportsFlag Flags, ushort[] Modules);

}
