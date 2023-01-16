using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record SourceDebugExtensionAttributeRecord(byte[] Data) : AttributeRecord
    {

        public static bool TryReadSourceDebugExtensionAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadExact((int)reader.Length, out ReadOnlySequence<byte> data) == false)
                return false;

            var dataBuffer = new byte[data.Length];
            data.CopyTo(dataBuffer);

            attribute = new SourceDebugExtensionAttributeRecord(dataBuffer);
            return true;
        }

    }

}
