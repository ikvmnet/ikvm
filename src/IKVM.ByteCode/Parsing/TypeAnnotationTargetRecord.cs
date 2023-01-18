using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    public abstract record TypeAnnotationTargetRecord(byte TargetType)
    {

        public static bool TryReadTypeAnnotationTarget(ref SequenceReader<byte> reader, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte targetType) == false)
                return false;

            return targetType switch
            {
                0x00 or 0x01 => TypeAnnotationParameterTargetRecord.TryReadTypeAnnotationParameterTarget(ref reader, targetType, out targetInfo),
                0x10 => TypeAnnotationSuperTypeTargetRecord.TryRead(ref reader, targetType, out targetInfo),
                0x11 or 0x12 => TypeAnnotationParameterBoundTargetRecord.TryRead(ref reader, targetType, out targetInfo),
                0x13 or 0x14 or 0x15 => TypeAnnotationEmptyTargetRecord.TryRead(ref reader, targetType, out targetInfo),
                0x16 => TypeAnnotationFormalParameterTargetRecord.TryRead(ref reader, targetType, out targetInfo),
                0x17 => TypeAnnotationThrowsTargetRecord.TryRead(ref reader, targetType, out targetInfo),
                _ => throw new ByteCodeException($"Invalid type annotation target type: '{targetType}'."),
            };
        }

        public virtual int GetSize()
        {
            return sizeof(byte);
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public virtual bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWrite(TargetType) == false)
                return false;

            return true;
        }

    }

}
