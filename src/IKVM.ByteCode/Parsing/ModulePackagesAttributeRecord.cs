using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ModulePackagesAttributeRecord(ushort[] Packages) : AttributeRecord
    {

        public static bool TryReadModulePackagesAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort count) == false)
                return false;

            var packages = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                if (reader.TryReadBigEndian(out ushort packageIndex) == false)
                    return false;

                packages[i] = packageIndex;
            }

            attribute = new ModulePackagesAttributeRecord(packages);
            return true;
        }

    }

}