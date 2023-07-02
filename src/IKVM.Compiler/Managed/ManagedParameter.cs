namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method parameter.
    /// </summary>
    public class ManagedParameter
    {

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public ManagedTypeSignature ParameterType { get; }

    }

}
