using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record SourceFileAttributeRecord(ushort SourceFileIndex) : AttributeRecord
    {

        public static bool TryReadSourceFileAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort sourceFileIndex) == false)
                return false;

            attribute = new SourceFileAttributeRecord(sourceFileIndex);
            return true;
        }

    }

}