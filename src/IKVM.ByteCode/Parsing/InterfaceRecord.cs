namespace IKVM.ByteCode.Parsing
{

    public record struct InterfaceRecord(ushort ClassIndex)
    {

        /// <summary>
        /// Parses an interface.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="iface"></param>
        public static bool TryRead(ref ClassFormatReader reader, out InterfaceRecord iface)
        {
            iface = default;

            if (reader.TryReadU2(out ushort classIndex) == false)
                return false;

            iface = new InterfaceRecord(classIndex);
            return true;
        }

    }

}
