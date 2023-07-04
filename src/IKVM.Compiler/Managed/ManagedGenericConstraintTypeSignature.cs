using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference of a type signature.
    /// </summary>
    public sealed class ManagedGenericConstraintTypeSignature : ManagedTypeSignature
    {

        readonly ReadOnlyFixedValueList<ManagedTypeSignature> constraints;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="constraints"></param>
        public ManagedGenericConstraintTypeSignature(in ReadOnlyFixedValueList<ManagedTypeSignature> constraints)
        {
            this.constraints = constraints;
        }

        /// <summary>
        /// Gets the signatures of the generic constraints.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedTypeSignature> Constraints => ref constraints;

    }

}
