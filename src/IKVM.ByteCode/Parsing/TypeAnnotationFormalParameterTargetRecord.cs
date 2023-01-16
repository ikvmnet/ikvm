using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationFormalParameterTargetRecord(byte TargetType, byte ParameterIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryReadTypeAnnotationFormalParameterTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;

            targetInfo = new TypeAnnotationFormalParameterTargetRecord(targetType, parameterIndex);
            return true;
        }

    }

}