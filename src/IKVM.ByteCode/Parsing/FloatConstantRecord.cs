using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record FloatConstantRecord(float Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Float constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadFloatConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out float value) == false)
                return false;

            constant = new FloatConstantRecord(value);
            return true;
        }

    }

}
