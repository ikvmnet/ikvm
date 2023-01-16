using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record struct InterfaceInfoRecord(ushort ClassIndex)
    {

        /// <summary>
        /// Parses an interface.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="iface"></param>
        public static bool TryReadInterface(ref SequenceReader<byte> reader, out InterfaceInfoRecord iface)
        {
            iface = default;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;

            iface = new InterfaceInfoRecord(classIndex);
            return true;
        }

    }

}
