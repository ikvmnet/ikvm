using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationParameterBoundTargetRecord(byte TargetType, byte ParameterIndex, byte BoundIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryReadTypeAnnotationParameterBoundTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;
            if (reader.TryRead(out byte boundIndex) == false)
                return false;

            targetInfo = new TypeAnnotationParameterBoundTargetRecord(targetType, parameterIndex, boundIndex);
            return true;
        }

    }

}