using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record MethodHandleConstantRecord(ReferenceKind Kind, ushort Index) : ConstantRecord
    {

        /// <summary>
        /// Parses a MethodHandle constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadMethodHandleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryRead(out ReferenceKind kind) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort index) == false)
                return false;

            constant = new MethodHandleConstantRecord(kind, index);
            return true;
        }

    }

}
