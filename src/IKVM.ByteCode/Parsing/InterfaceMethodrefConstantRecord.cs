using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record InterfaceMethodrefConstantRecord(ushort ClassIndex, ushort NameAndTypeIndex) : RefConstantRecord(ClassIndex, NameAndTypeIndex)
    {

        /// <summary>
        /// Parses a InterfaceMethodref constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadInterfaceMethodrefConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadBigEndian(out ushort classIndex) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort nameAndTypeIndex) == false)
                return false;

            constant = new InterfaceMethodrefConstantRecord(classIndex, nameAndTypeIndex);
            return true;
        }

    }

}