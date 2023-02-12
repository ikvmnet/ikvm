namespace IKVM.ByteCode.Parsing
{

    internal record struct ModuleAttributeOpensRecord(ushort Index, ModuleOpensFlag Flags, ushort[] Modules);

}
