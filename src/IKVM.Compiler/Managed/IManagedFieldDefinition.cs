namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    public interface IManagedFieldInfo : IManagedMemberInfo
    {

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        IManagedTypeInfo FieldType { get; }

    }

}
