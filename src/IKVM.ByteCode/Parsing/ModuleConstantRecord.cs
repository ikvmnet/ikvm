using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record ModuleConstantRecord(ushort NameIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a Module constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadModuleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort nameIndex) == false)
                return false;

            constant = new ModuleConstantRecord(nameIndex);
            return true;
        }

    }

}
