using System.Buffers;

using IKVM.ByteCode.Buffers;
using IKVM.ByteCode.Reading;

namespace IKVM.ByteCode.Parsing
{

    internal sealed record FloatConstantRecord(float Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Float constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadFloatConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU4(out uint value) == false)
                return false;

            var v = unchecked((float)value);

            constant = new FloatConstantRecord(v);
            return true;
        }

    }

}
