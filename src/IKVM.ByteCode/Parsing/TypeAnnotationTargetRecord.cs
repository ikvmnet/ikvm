namespace IKVM.ByteCode.Parsing
{

    internal abstract record TypeAnnotationTargetRecord
    {

        /// <summary>
        /// Returns the size of the record when written.
        /// </summary>
        /// <returns></returns>
        public abstract int GetSize();

        /// <summary>
        /// Attempts to write the record to the given <see cref="ClassFormatWriter"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public abstract bool TryWrite(ref ClassFormatWriter writer);

    }

}
