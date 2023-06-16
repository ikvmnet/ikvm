namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    public interface IManagedFieldDefinition : IManagedMemberDefinition
    {

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        IManagedTypeDefinition FieldType { get; }

    }

}
