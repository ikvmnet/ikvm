namespace IKVM.ByteCode.Parsing
{
    internal record struct ExceptionHandlerRecord(ushort StartOffset, ushort EndOffset, ushort HandlerOffset, ushort CatchTypeIndex)
    {
        public static bool TryRead(ref ClassFormatReader reader, out ExceptionHandlerRecord exceptionHandler)
        {
            exceptionHandler = default;

            if (reader.TryReadU2(out ushort startOffset) == false)
                return false;
            if (reader.TryReadU2(out ushort endOffset) == false)
                return false;
            if (reader.TryReadU2(out ushort handlerOffset) == false)
                return false;
            if (reader.TryReadU2(out ushort catchTypeIndex) == false)
                return false;

            exceptionHandler = new ExceptionHandlerRecord(startOffset, endOffset, handlerOffset, catchTypeIndex);
            return true;
        }

        public int GetSize() =>
            sizeof(ushort) + sizeof(ushort) + sizeof(ushort) + sizeof(ushort);

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(StartOffset) == false)
                return false;
            if (writer.TryWriteU2(EndOffset) == false)
                return false;
            if (writer.TryWriteU2(HandlerOffset) == false)
                return false;
            if (writer.TryWriteU2(CatchTypeIndex) == false)
                return false;

            return true;
        }
    }
}
