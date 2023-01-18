using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record FieldrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : RefConstantRecord(ClassIndex, NameAndTypeIndex)
    {

        /// <summary>
        /// Parses a Fieldref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadFieldrefConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;
            
            constant = new FieldrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

    }

}