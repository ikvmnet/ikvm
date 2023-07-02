namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array of a type signature.
    /// </summary>
    public class ManagedSzArrayTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature elementType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="lowerBound"></param>
        public ManagedSzArrayTypeSignature(ManagedTypeSignature elementType)
        {
            this.elementType = elementType;
        }

        /// <summary>
        /// Gets the element type of the array.
        /// </summary>
        public ManagedTypeSignature ElementType => elementType;

    }

}
