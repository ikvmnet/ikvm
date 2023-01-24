namespace IKVM.ByteCode.Parsing
{

    internal record struct ExceptionHandlerRecord(ushort StartOffset, ushort EndOffset, ushort HandlerOffset, ushort CatchTypeIndex);

}
