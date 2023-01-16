using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ModuleMainClassAttributeRecord(ushort MainClassIndex) : AttributeRecord
    {

        public static bool TryReadModuleMainClassAttribute(ref SequenceReader<byte> reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadBigEndian(out ushort mainClassIndex) == false)
                return false;

            attribute = new ModuleMainClassAttributeRecord(mainClassIndex);
            return true;
        }

    }

}
