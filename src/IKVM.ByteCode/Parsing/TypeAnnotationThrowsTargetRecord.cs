using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationThrowsTargetRecord(byte TargetType, ushort ThrowsTypeIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryReadTypeAnnotationThrowsTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadBigEndian(out ushort throwsTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationThrowsTargetRecord(targetType, throwsTypeIndex);
            return true;
        }

    }

}