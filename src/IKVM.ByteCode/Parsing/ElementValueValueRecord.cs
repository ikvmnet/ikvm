namespace IKVM.ByteCode.Parsing
{

    public abstract record ElementValueValueRecord
    {

        /// <summary>
        /// Gets the size of the record if written.
        /// </summary>
        /// <returns></returns>
        public abstract int GetSize();

        /// <summary>
        /// Attempts to write the record to the <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public abstract bool TryWrite(ref ClassFormatWriter writer);

    }

}
