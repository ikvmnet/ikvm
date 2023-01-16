namespace IKVM.ByteCode
{

    public record struct ExceptionHandlerRecord(ushort StartOffset, ushort EndOffset, ushort HandlerOffset, ushort CatchTypeIndex);

}
