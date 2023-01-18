using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public readonly record struct BootstrapMethodsAttributeMethodRecord(ushort MethodRefIndex, ushort[] Arguments)
    {

        public static bool TryReadBootstrapMethod(ref SequenceReader<byte> reader, out BootstrapMethodsAttributeMethodRecord method)
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

            method = new BootstrapMethodsAttributeMethodRecord(methodRefIndex, arguments);
            return true;
        }

    }

}
