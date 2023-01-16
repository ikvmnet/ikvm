using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record struct TypePathRecord(TypePathItemRecord[] Path)
    {

        public static bool TryReadTypePath(ref SequenceReader<byte> reader, out TypePathRecord typePath)
        {
            typePath = default;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;

            var path = new TypePathItemRecord[length];
            for (int i = 0; i < length; i++)
            {
                if (reader.TryRead(out byte kind) == false)
                    return false;
                if (reader.TryRead(out byte argumentIndex) == false)
                    return false;

                path[i] = new TypePathItemRecord((TypePathKind)kind, argumentIndex);
            }

            typePath = new TypePathRecord(path);
            return true;
        }

    }

}
