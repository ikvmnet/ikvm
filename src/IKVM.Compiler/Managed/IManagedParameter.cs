namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method parameter.
    /// </summary>
    public interface IManagedParameter
    {

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        ManagedTypeSignature ParameterType { get; }

    }

}
