namespace IKVM.ByteCode
{

    public record struct ModuleAttributeDataOpensRecord(ushort Index, ModuleOpensFlag Flags, ushort[] Modules);

}