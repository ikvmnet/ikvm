namespace IKVM.Compiler.Managed
{

    internal interface IManagedCustomAttributeNamedArgument
    {

        /// <summary>
        /// Gets a value that indicates whether the named argument is a field.
        /// </summary>
        bool IsField { get; }

        /// <summary>
        /// Gets the attribute member that would be used to set the named argument.
        /// </summary>
        ManagedMember Member { get; }

        /// <summary>
        /// Gets the name of the attribute member that would be used to set the named argument.
        /// </summary>
        string MemberName { get; }

        /// <summary>
        /// Gets a <see cref="IManagedCustomAttributeTypedArgument"/> that can be used to obtain the type and value of the current named argument.
        /// /// </summary>
        IManagedCustomAttributeTypedArgument TypedValue { get; }

    }

}