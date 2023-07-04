namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type including any modifiers.
    /// </summary>
    internal sealed class ManagedTypeReferenceSignature : ManagedTypeSignature
    {

        readonly ManagedTypeRef reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ManagedTypeReferenceSignature(ManagedTypeRef reference)
        {
            this.reference = reference;
        }

        /// <summary>
        /// Gets the reference to the type.
        /// </summary>
        public ManagedTypeRef Reference => reference;

    }

}
