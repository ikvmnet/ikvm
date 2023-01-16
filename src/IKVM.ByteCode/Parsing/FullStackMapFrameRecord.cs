using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record FullStackMapFrameRecord(byte Tag, ushort OffsetDelta, VerificationTypeInfoRecord[] Locals, VerificationTypeInfoRecord[] Stack) : StackMapFrameRecord(Tag)
    {

        public static bool TryReadFullStackMapFrame(ref SequenceReader<byte> reader, byte tag, out StackMapFrameRecord frame)
        {
            frame = null;

            if (reader.TryReadBigEndian(out ushort offsetDelta) == false)
                return false;

            if (reader.TryReadBigEndian(out ushort localsCount) == false)
                return false;

            var locals = new VerificationTypeInfoRecord[localsCount];
            for (int i = 0; i < localsCount; i++)
            {
                if (VerificationTypeInfoRecord.TryReadVerificationTypeInfo(ref reader, out var j) == false)
                    return false;

                locals[i] = j;
            }

            if (reader.TryReadBigEndian(out ushort stackCount) == false)
                return false;

            var stack = new VerificationTypeInfoRecord[stackCount];
            for (int i = 0; i < stackCount; i++)
            {
                if (VerificationTypeInfoRecord.TryReadVerificationTypeInfo(ref reader, out var j) == false)
                    return false;

                stack[i] = j;
            }

            frame = new FullStackMapFrameRecord(tag, offsetDelta, locals, stack);
            return true;
        }

    }

}
