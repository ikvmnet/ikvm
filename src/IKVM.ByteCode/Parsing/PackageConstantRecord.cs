using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record PackageConstantRecord(ushort NameIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a Package constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadPackageConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new PackageConstantRecord(nameIndex);
            return true;
        }

    }

}
