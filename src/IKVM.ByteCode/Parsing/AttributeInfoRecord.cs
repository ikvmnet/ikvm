using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public readonly record struct AttributeInfoRecord(ushort NameIndex, byte[] Data)
    {

        /// <summary>
        /// Parses an attribute.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attribute"></param>
        public static bool TryReadAttribute(ref SequenceReader<byte> reader, out AttributeInfoRecord attribute)
        {
            attribute = default;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out uint length) == false)
                return false;
            if (reader.TryReadExact((int)length, out var info) == false)
                return false;

            var infoBuffer = new byte[info.Length];
            info.CopyTo(infoBuffer);

            attribute = new AttributeInfoRecord(nameIndex, infoBuffer);
            return true;
        }

    }

}
