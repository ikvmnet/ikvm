using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    public abstract record TypeAnnotationTargetRecord(byte TargetType)
    {

        public static bool TryReadTypeAnnotationTarget(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo) => targetType switch
        {
            0x00 or 0x01 => TypeAnnotationParameterTargetRecord.TryReadTypeAnnotationParameterTarget(ref reader, targetType, out targetInfo),
            0x10 => TypeAnnotationSuperTypeTargetRecord.TryReadTypeAnnotationSuperTypeTarget(ref reader, targetType, out targetInfo),
            0x11 or 0x12 => TypeAnnotationParameterBoundTargetRecord.TryReadTypeAnnotationParameterBoundTarget(ref reader, targetType, out targetInfo),
            0x13 or 0x14 or 0x15 => TypeAnnotationEmptyTargetRecord.TryReadTypeAnnotationEmptyTarget(ref reader, targetType, out targetInfo),
            0x16 => TypeAnnotationFormalParameterTargetRecord.TryReadTypeAnnotationFormalParameterTarget(ref reader, targetType, out targetInfo),
            0x17 => TypeAnnotationThrowsTargetRecord.TryReadTypeAnnotationThrowsTarget(ref reader, targetType, out targetInfo),
            _ => throw new ByteCodeException($"Invalid type annotation target type: '{targetType}'."),
        };

    }

}
