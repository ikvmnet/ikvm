namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a type including any modifiers.
    /// </summary>
    internal sealed class ManagedTypeRefSignature : ManagedTypeSignature
    {

        readonly ManagedTypeRef reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ManagedTypeRefSignature(ManagedTypeRef reference)
        {
            this.reference = reference;
        }

        /// <summary>
        /// Gets the reference to the type.
        /// </summary>
        public ManagedTypeRef Reference => reference;

    }

}
