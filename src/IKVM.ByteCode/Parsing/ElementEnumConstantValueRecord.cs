using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ElementEnumConstantValueRecord(ushort TypeNameIndex, ushort ConstantNameIndex) : ElementValueRecord
    {

        public static bool TryReadElementEnumConstantValue(ref SequenceReader<byte> reader, out ElementValueRecord value)
        {
            value = null;

            if (reader.TryReadBigEndian(out ushort typeNameIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort constantNameIndex) == false)
                return false;

            value = new ElementEnumConstantValueRecord(typeNameIndex, constantNameIndex);
            return true;
        }

    }

}