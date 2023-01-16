using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record NestMembersAttributeRecord(ushort[] ClassIndexes) : AttributeRecord
    {

        public static bool TryReadNestMembersAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var classes = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort classIndex) == false)
                    return false;

                classes[i] = classIndex;
            }

            attribute = new NestMembersAttributeRecord(classes);
            return true;
        }

    }

}
