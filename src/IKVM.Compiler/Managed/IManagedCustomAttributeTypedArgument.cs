namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a typed argument on a custom attribute.
    /// </summary>
    interface IManagedCustomAttributeTypedArgument
    {

        /// <summary>
        /// Type of the argument.
        /// </summary>
        IManagedType ArgumentType { get; }

        /// <summary>
        /// Value of the argument.
        /// </summary>
        object? Value { get; }

    }

}