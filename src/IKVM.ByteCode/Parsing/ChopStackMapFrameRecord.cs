using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ChopStackMapFrameRecord(byte Tag, ushort OffsetDelta) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadChopStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            frame = new ChopStackMapFrameRecord(tag, offsetDelta);
            return true;
        }

    }

}
