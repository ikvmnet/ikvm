using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationParameterBoundTargetRecord(byte TargetType, byte ParameterIndex, byte BoundIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryRead(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryRead(out byte parameterIndex) == false)
                return false;
            if (reader.TryRead(out byte boundIndex) == false)
                return false;

            targetInfo = new TypeAnnotationParameterBoundTargetRecord(targetType, parameterIndex, boundIndex);
            return true;
        }

        public override int GetSize()
        {
            var length = base.GetSize();
            length += sizeof(byte);
            length += sizeof(byte);
            return length;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public override bool TryWrite(ref ClassFormatWriter writer)
        {
            if (base.TryWrite(ref writer) == false)
                return false;

            if (writer.TryWrite(ParameterIndex) == false)
                return false;
            if (writer.TryWrite(BoundIndex) == false)
                return false;

            return true;
        }

    }

}