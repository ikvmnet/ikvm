using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationParameterTargetRecord(byte TargetType, byte ParameterIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryReadTypeAnnotationParameterTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;

            targetInfo = new TypeAnnotationParameterTargetRecord(targetType, parameterIndex);
            return true;
        }

    }

}