using System;
using System.Buffers;

namespace IKVM.ByteCode.Parsing
{
    internal record struct AttributeInfoRecord(ushort NameIndex, byte[] Data)
    {
        /// <summary>
        /// Parses an attribute.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attribute"></param>
        public static bool TryReadAttribute(ref ClassFormatReader reader, out AttributeInfoRecord attribute)
        {
            attribute = default;

            if (reader.TryReadU2(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadU4(out uint length) == false)
                return false;
            if (reader.TryReadManyU1(length, out var info) == false)
                return false;

            var infoBuffer = new byte[info.Length];
            info.CopyTo(infoBuffer);

            attribute = new AttributeInfoRecord(nameIndex, infoBuffer);
            return true;
        }

        public bool TryWrite(ref ClassFormatWriter writer)
        {
            if (writer.TryWriteU2(NameIndex) == false)
                return false;
            if (writer.TryWriteU4((uint)Data.Length) == false)
                return false;
            if (writer.TryWriteManyU1(Data) == false)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the number of bytes required to write the record.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            var size = 0;

            size += sizeof(ushort);
            size += sizeof(uint);
            size += Data.Length * sizeof(byte);

            return size;
        }
    }
}
