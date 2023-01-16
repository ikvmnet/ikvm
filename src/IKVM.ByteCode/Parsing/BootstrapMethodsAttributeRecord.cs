using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record BootstrapMethodsAttributeRecord(BootstrapMethodRecord[] Methods) : AttributeRecord
    {

        public static bool TryReadBootstrapMethodsAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var methods = new BootstrapMethodRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (BootstrapMethodRecord.TryReadBootstrapMethod(ref reader, out var method) == false)
                    return false;

                methods[i] = method;
            }

            attribute = new BootstrapMethodsAttributeRecord(methods);
            return true;
        }

    }

}
