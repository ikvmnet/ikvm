using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record EnclosingMethodAttributeRecord(ushort ClassIndex, ushort MethodIndex) : AttributeRecord
    {

        public static bool TryReadEnclosingMethodAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort methodIndex) == false)
                return false;

            attribute = new EnclosingMethodAttributeRecord(classIndex, methodIndex);
            return true;
        }

    }

}
