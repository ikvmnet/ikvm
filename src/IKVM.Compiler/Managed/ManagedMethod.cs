using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    public class ManagedMethod : ManagedMember
    {

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public ManagedTypeSignature ReturnType { get; }

        /// <summary>
        /// Gets the parameters of the method.
        /// </summary>
        public IReadOnlyList<ManagedParameter> Parameters { get; }

    }

}
