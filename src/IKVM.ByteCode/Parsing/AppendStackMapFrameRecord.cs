using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record AppendStackMapFrameRecord(byte Tag, ushort OffsetDelta, VerificationTypeInfoRecord[] Locals) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadAppendStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            var locals = new VerificationTypeInfoRecord[tag - 251];
            for (int i = 0; i < tag - 251; i++)
            {
                if (VerificationTypeInfoRecord.TryReadVerificationTypeInfo(ref reader, out var local) == false)
                    return false;

                locals[i] = local;
            }

            frame = new AppendStackMapFrameRecord(tag, offsetDelta, locals);
            return true;
        }

    }

}
