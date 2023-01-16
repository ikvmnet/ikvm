using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record SameExtendedStackMapFrameRecord(byte Tag, ushort OffsetDelta) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadSameExtendedStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            frame = new SameExtendedStackMapFrameRecord(tag, offsetDelta);
            return true;
        }

    }


}
