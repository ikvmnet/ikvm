using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    internal record SourceFileAttributeRecord(ushort SourceFileIndex) : AttributeRecord
    {

        public static bool TryReadSourceFileAttribute(ref ClassFormatReader reader, out AttributeRecord attribute)
        {
            attribute = null;

            if (reader.TryReadU2(out ushort sourceFileIndex) == false)
                return false;

            attribute = new SourceFileAttributeRecord(sourceFileIndex);
            return true;
        }

    }

}