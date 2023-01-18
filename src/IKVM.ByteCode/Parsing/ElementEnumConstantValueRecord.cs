using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Writing;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementEnumConstantValueRecord(char Tag, ushort TypeNameIndex, ushort ConstantNameIndex) : ElementValueRecord(Tag)
    {

        public static bool TryRead(ref SequenceReader<byte> reader, char tag, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort typeNameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort constantNameIndex) == false)
                return false;

            value = new ElementEnumConstantValueRecord(tag, typeNameIndex, constantNameIndex);
            return true;
        }

        public override int GetSize()
        {
            var size = base.GetSize();
            size += sizeof(ushort);
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

            if (writer.TryWrite(TypeNameIndex) == false)
                return false;
            if (writer.TryWrite(ConstantNameIndex) == false)
                return false;

            return true;
        }

    }

}