namespace IKVM.ByteCode.Parsing
{

    internal sealed record LongConstantRecord(long Value) : ConstantRecord
    {

        /// <summary>
        /// Parses a Long constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadLongConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 1;

            if (reader.TryReadU4(out uint a) == false)
                return false;
            if (reader.TryReadU4(out uint b) == false)
                return false;

            constant = new LongConstantRecord((long)(((ulong)a << 32) | b));
            return true;
        }

        protected override int GetConstantSize()
        {
            var size = 0;
            size += sizeof(uint);
            size += sizeof(uint);
            return size;
        }

        protected override bool TryWriteConstant(ref ClassFormatWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }

}
