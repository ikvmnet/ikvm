using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record InnerClassesAttributeRecord(InnerClassRecord[] Entries) : AttributeRecord
    {

        public static bool TryReadInnerClassesAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var entries = new InnerClassRecord[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort innerClassInfoIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort outerClassInfoIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort innerNameIndex) == false)
                    return false;
                if (reader.TryReadBigEndian(out ushort innerClassAccessFlags) == false)
                    return false;

                entries[i] = new InnerClassRecord(innerClassInfoIndex, outerClassInfoIndex, innerNameIndex, (AccessFlag)innerClassAccessFlags);
            }

            attribute = new InnerClassesAttributeRecord(entries);
            return true;
        }

    }

}