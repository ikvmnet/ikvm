using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationEmptyTargetRecord(byte TargetType) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryReadTypeAnnotationEmptyTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = new TypeAnnotationEmptyTargetRecord(targetType);
            return true;
        }

    }

}