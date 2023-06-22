using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    public interface IManagedMethod : IManagedMember
    {

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        ManagedTypeSignature ReturnType { get; }

        /// <summary>
        /// Gets the parameters of the method.
        /// </summary>
        IReadOnlyList<IManagedParameter> Parameters { get; }

    }

}
