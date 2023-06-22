namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array of a type signature.
    /// </summary>
    public class ManagedArrayTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature elementType;
        readonly int rank;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        public ManagedArrayTypeSignature(ManagedTypeSignature elementType, int rank = 1)
        {
            this.elementType = elementType;
            this.rank = rank;
        }

        /// <summary>
        /// Gets the element type of the array.
        /// </summary>
        public ManagedTypeSignature ElementType => elementType;

        /// <summary>
        /// Gets the rank of the array.
        /// </summary>
        public int Rank => rank;

    }

}
