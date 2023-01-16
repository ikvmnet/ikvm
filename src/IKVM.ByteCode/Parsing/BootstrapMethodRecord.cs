using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public readonly record struct BootstrapMethodRecord(ushort MethodRefIndex, ushort[] Arguments)
    {

        public static bool TryReadBootstrapMethod(ref SequenceReader<byte> reader, out BootstrapMethodRecord method)
        {
            method = default;

            if (reader.TryReadBigEndian(out ushort methodRefIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort argumentCount) == false)
                return false;

            var arguments = new ushort[argumentCount];
            for (int i = 0; i < argumentCount; i++)
            {
                if (reader.TryReadBigEndian(out ushort argumentIndex) == false)
                    return false;

                arguments[i] = argumentIndex;
            }

            method = new BootstrapMethodRecord(methodRefIndex, arguments);
            return true;
        }

    }

}
