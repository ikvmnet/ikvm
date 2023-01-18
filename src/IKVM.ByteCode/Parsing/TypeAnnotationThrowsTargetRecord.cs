using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationThrowsTargetRecord(byte TargetType, ushort ThrowsTypeIndex) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryRead(ref ClassFormatReader reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = null;

            if (reader.TryReadU2(out ushort throwsTypeIndex) == false)
                return false;

            targetInfo = new TypeAnnotationThrowsTargetRecord(targetType, throwsTypeIndex);
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

            if (writer.TryWrite(ThrowsTypeIndex) == false)
                return false;

            return true;
        }

    }

}