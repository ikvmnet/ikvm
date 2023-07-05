namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an array of a type signature.
    /// </summary>
    public sealed class ManagedModifiedTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature baseType;
        readonly ManagedTypeSignature modifierType;
        readonly bool required;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="modifierType"></param>
        /// <param name="required"></param>
        public ManagedModifiedTypeSignature(ManagedTypeSignature baseType, ManagedTypeSignature modifierType, bool required)
        {
            this.baseType = baseType;
            this.modifierType = modifierType;
            this.required = required;
        }

        /// <summary>
        /// Gets the base type that was modified.
        /// </summary>
        public ManagedTypeSignature BaseType => baseType;

        /// <summary>
        /// Gets the type by which it was modified.
        /// </summary>
        public ManagedTypeSignature ModifierType => modifierType;

        /// <summary>
        /// Gets whether the modification is required.
        /// </summary>
        public bool Required => required;

    }

}
