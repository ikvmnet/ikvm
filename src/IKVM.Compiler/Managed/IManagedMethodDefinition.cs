namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    public interface IManagedMethodDefinition : IManagedMemberDefinition
    {

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        IManagedTypeDefinition ReturnType { get; }

    }

}
