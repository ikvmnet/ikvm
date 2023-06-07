using System.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record SourceDebugExtensionAttributeRecord(byte[] Data) : AttributeRecord
    {

        public static bool TryReadSourceDebugExtensionAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadManyU1(reader.Length, out ReadOnlySequence<byte> data) == false)
                return false;

            var dataBuffer = new byte[data.Length];
            data.CopyTo(dataBuffer);

            attribute = new SourceDebugExtensionAttributeRecord(dataBuffer);
            return true;
        }

    }

}
