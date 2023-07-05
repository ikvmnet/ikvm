namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array of a type signature.
    /// </summary>
    public sealed class ManagedSzArrayTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature elementType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="lowerBound"></param>
        public ManagedSzArrayTypeSignature(ManagedTypeSignature baseType)
        {
            this.elementType = baseType;
        }

        /// <summary>
        /// Gets the element type of the array.
        /// </summary>
        public ManagedTypeSignature ElementType => elementType;

    }

}
