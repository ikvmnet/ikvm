using System.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public record class Utf8ConstantRecord(byte[] Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a UTF8 constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadUtf8Constant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort length) == false)
                return false;
            if (reader.TryReadManyU1(length, out ReadOnlySequence<byte> value) == false)
                return false;

            var valueBuffer = new byte[value.Length];
            value.CopyTo(valueBuffer);

            constant = new Utf8ConstantRecord(valueBuffer);
            return true;
        }

    }

}
