using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record TypeAnnotationEmptyTargetRecord(byte TargetType) : TypeAnnotationTargetRecord(TargetType)
    {

        public static bool TryRead(ref ClassFormatReader reader, byte targetType, out TypeAnnotationTargetRecord targetInfo)
        {
            targetInfo = new TypeAnnotationEmptyTargetRecord(targetType);
            return true;
        }

        public override int GetSize()
        {
            return base.GetSize();
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

            return true;
        }

    }

}