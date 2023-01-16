using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record SameStackMapFrameRecord(byte Tag) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadSameStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = new SameStackMapFrameRecord(tag);
            return true;
        }

    }

}
