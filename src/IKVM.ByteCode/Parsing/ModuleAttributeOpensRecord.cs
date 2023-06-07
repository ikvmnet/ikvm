namespace IKVM.ByteCode.Parsing
{

    public record struct ModuleAttributeOpensRecord(ushort Index, ModuleOpensFlag Flags, ushort[] Modules);

}
