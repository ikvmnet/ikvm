using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode.Parsing
{

    public sealed record MethodHandleConstantRecord(ReferenceKind Kind, ushort ReferenceIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a MethodHandle constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        public static bool TryReadMethodHandleConstant(ref SequenceReader<byte> reader, out ConstantRecord constant)
        {
            constant = null;

            if (reader.TryRead(out ReferenceKind referenceKind) == false)
                return false;
            if (reader.TryReadBigEndian(out ushort referenceIndex) == false)
                return false;

            constant = new MethodHandleConstantRecord(referenceKind, referenceIndex);
            return true;
        }

    }

}
