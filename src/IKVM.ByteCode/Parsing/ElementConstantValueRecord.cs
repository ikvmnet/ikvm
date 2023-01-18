using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record ElementConstantValueRecord(char Tag, ushort ConstantValueIndex) : ElementValueRecord(Tag)
    {

        public static bool TryReadElementConstantValue(ref ClassFormatReader reader, char tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadU2(out ushort index) == false)
                return false;

            value = new ElementConstantValueRecord(tag, index);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
            size += sizeof(ushort);
            return size;
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

            if (writer.TryWrite(ConstantValueIndex) == false)
                return false;

            return true;
        }

    }

}