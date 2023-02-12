using System.Buffers;

namespace IKVM.ByteCode.Parsing
{
    internal record SourceDebugExtensionAttributeRecord(byte[] Data) : AttributeRecord
    {
        public static bool TryReadSourceDebugExtensionAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadManyU1(reader.Length, out ReadOnlySequence<byte> data) == false)
                return false;

            attribute = new SourceDebugExtensionAttributeRecord(data.ToArray());
            return true;
        }

        public override int GetSize() =>
            Data.Length * sizeof(byte);

        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteManyU1(Data) == false)
                return false;

            return true;
        }
    }
}
