using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    public sealed record TypeAnnotationSuperTypeTargetRecord(byte TargetType, ushort SuperTypeIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryRead(ref SequenceReader<byte> reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadBigEndian(out ushort superTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationSuperTypeTargetRecord(targetType, superTypeIndex);
            return true;
        }

        public override int GetSize()
        {
            var length = base.GetSize();
            length += sizeof(ushort);
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

            if (writer.TryWrite(SuperTypeIndex) == false)
                return false;

            return true;
        }

    }

}