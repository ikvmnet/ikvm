using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationSuperTypeTargetRecord(byte TargetType, ushort SuperTypeIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryReadTypeAnnotationSuperTypeTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadBigEndian(out ushort superTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationSuperTypeTargetRecord(targetType, superTypeIndex);
            return true;
        }

    }

}