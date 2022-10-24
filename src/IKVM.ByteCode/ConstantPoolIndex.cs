namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents an index within the constant pool.
    /// </summary>
    public readonly struct ConstantPoolIndex
    {

        readonly uint value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public ConstantPoolIndex(uint value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the real integer value of the index.
        /// </summary>
        public uint Value => value;

    }

}
