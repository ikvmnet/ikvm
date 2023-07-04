using System.Collections.Generic;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference of a type signature.
    /// </summary>
    public sealed class ManagedGenericTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature genericType;
        readonly ReadOnlyFixedValueList<ManagedTypeSignature> genericParameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="genericType"></param>
        public ManagedGenericTypeSignature(ManagedTypeSignature genericType, in ReadOnlyFixedValueList<ManagedTypeSignature> genericParameters)
        {
            this.genericType = genericType;
            this.genericParameters = genericParameters;
        }

        /// <summary>
        /// Gets the type refered to as a reference.
        /// </summary>
        public ManagedTypeSignature GenericType => genericType;

        /// <summary>
        /// Gets the signatures of the generic parameters.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedTypeSignature> GenericParameters => ref genericParameters;

    }

}
