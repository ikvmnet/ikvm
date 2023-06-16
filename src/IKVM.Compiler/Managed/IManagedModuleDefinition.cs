namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged module.
    /// </summary>
    public interface IManagedModuleDefinition
    {

        /// <summary>
        /// Gets the name of the managed module.
        /// </summary>
        string Name { get; }

    }

}
