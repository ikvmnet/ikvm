using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record StackMapTableAttributeRecord(StackMapFrameRecord[] Frames) : AttributeRecord
    {

        public static bool TryRead(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var frames = new StackMapFrameRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (StackMapFrameRecord.TryRead(ref reader, out var frame) == false)
                    return false;

                frames[i] = frame;
            }

            attribute = new StackMapTableAttributeRecord(frames);
            return true;
        }

    }

}
