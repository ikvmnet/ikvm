namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed member.
    /// </summary>
    public interface IManagedMemberInfo
    {

        /// <summary>
        /// Gets the module of the managed field.
        /// </summary>
        IManagedTypeInfo DeclaringType { get; }

        /// <summary>
        /// Gets the name of the managed member.
        /// </summary>
        string Name { get; }

    }

}
