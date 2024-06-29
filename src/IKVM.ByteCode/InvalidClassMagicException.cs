namespace IKVM.ByteCode
{

    /// <summary>
    /// Describes an attempt to parse an unsupported class magic value.
    /// </summary>
    internal sealed class InvalidClassMagicException :
        InvalidClassException
    {

        readonly uint magic;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="magic"></param>
        internal InvalidClassMagicException(uint magic) :
            base($"Invalid class magic value {magic}.")
        {
            this.magic = magic;
        }

        /// <summary>
        /// Gets the magic that was found in the class file.
        /// </summary>
        public uint Magic => magic;

    }

}
