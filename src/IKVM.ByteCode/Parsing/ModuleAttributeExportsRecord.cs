namespace IKVM.ByteCode.Parsing
{

    internal record struct ModuleAttributeExportsRecord(ushort Index, ModuleExportsFlag Flags, ushort[] Modules);

}
