using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    internal record struct ElementValuePairRecord(ushort NameIndex, ElementValueRecord Value)
    {

        public static bool TryRead(ref ClassFormatReader reader, out ElementValuePairRecord record)
        {
            record = default;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (ElementValueRecord.TryRead(ref reader, out var elementValue) == false)
                return false;

            record = new ElementValuePairRecord(nameIndex, elementValue);
            return true;
        }

        public int GetSize()
        {
            var size = 0;
            size += sizeof(ushort);
            size += Value.GetSize();
            return size;
        }

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWrite(NameIndex) == false)
                return false;
            if (Value.TryWrite(ref writer) == false)
                return false;

            return true;
        }

    }

}
