using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record StackMapTableAttributeRecord(StackMapFrameRecord[] Frames) : AttributeRecord
    {

        public static bool TryReadStackMapTableAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out uint entryCount) == false)
                return false;

            var entries = new StackMapFrameRecord[(int)entryCount];
            for (int i = 0; i < entryCount; i++)
            {
                if (StackMapFrameRecord.TryReadStackFrame(ref reader, out var frame) == false)
                    return false;

                entries[i] = frame;
            }

            if (ClassRecord.TryReadAttributes(ref reader, out var attributes) == false)
                return false;

            attribute = new StackMapTableAttributeRecord(entries);
            return true;
        }

    }

}
