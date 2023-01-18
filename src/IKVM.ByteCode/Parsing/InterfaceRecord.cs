using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record struct InterfaceRecord(ushort ClassIndex)
    {

        /// <summary>
        /// Parses an interface.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="iface"></param>
        public static bool TryRead(ref SequenceReader<byte> reader, out InterfaceRecord iface)
        {
            iface = default;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;

            iface = new InterfaceRecord(classIndex);
            return true;
        }

    }

}
