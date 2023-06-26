using System.Buffers;

namespace IKVM.ByteCode.Parsing
{
    internal sealed record UnknownAttributeRecord(byte[] Data) : AttributeRecord
    {
        public static bool TryReadCustomAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadManyU1(reader.Length, out ReadOnlySequence<byte> data) == false)
                return false;

            attribute = new UnknownAttributeRecord(data.ToArray());
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
