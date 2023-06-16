namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method.
    /// </summary>
    public interface IManagedMethodInfo : IManagedMemberInfo
    {

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        IManagedTypeInfo ReturnType { get; }

    }

}
