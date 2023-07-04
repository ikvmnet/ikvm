namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array of a type signature.
    /// </summary>
    public sealed class ManagedArrayTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature elementType;
        readonly ManagedArrayShape shape;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="shape"></param>
        internal ManagedArrayTypeSignature(ManagedTypeSignature elementType, ManagedArrayShape shape)
        {
            this.elementType = elementType;
            this.shape = shape;
        }

        /// <summary>
        /// Gets the element type of the array.
        /// </summary>
        public ManagedTypeSignature ElementType => elementType;

        /// <summary>
        /// Gets the number of dimensions in the array.
        /// </summary>
        public int Rank => shape.Rank;

    }

}
