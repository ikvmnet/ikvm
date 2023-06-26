using System.Buffers;

namespace IKVM.ByteCode.Parsing
{
    internal record struct LineNumberTableAttributeItemRecord(ushort CodeOffset, ushort LineNumber)
    {
        public static bool TryRead(ref ClassFormatReader reader, out LineNumberTableAttributeItemRecord attribute)
        {
            attribute = default;

            if (reader.TryReadU2(out ushort codeOffset) == false)
                return false;
            if (reader.TryReadU2(out ushort lineNumber) == false)
                return false;

            attribute = new LineNumberTableAttributeItemRecord(codeOffset, lineNumber);
            return true;
        }

        public int GetSize() =>
            sizeof(ushort) + sizeof(ushort);

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(CodeOffset) == false)
                return false;
            if (writer.TryWriteU2(LineNumber) == false)
                return false;

            return true;
        }
    }
}
