using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record Utf8ConstantRecord(byte[] Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a UTF8 constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadUtf8Constant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryReadBigEndian(out ushort length) == false)
                return false;
            if (reader.TryReadExact(length, out ReadOnlySequence<byte> value) == false)
                return false;

            var valueBuffer = new byte[value.Length];
            value.CopyTo(valueBuffer);

            constant = new Utf8ConstantRecord(valueBuffer);
            return true;
        }

    }

}
