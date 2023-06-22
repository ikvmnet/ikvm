using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference of a type signature.
    /// </summary>
    public class ManagedGenericTypeSignature : ManagedTypeSignature
    {

        readonly ManagedTypeSignature genericType;
        readonly IReadOnlyList<ManagedTypeSignature> genericParameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="genericType"></param>
        public ManagedGenericTypeSignature(ManagedTypeSignature genericType, IReadOnlyList<ManagedTypeSignature> genericParameters)
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
        public IReadOnlyList<ManagedTypeSignature> GenericParameters => genericParameters;

    }

}
