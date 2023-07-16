namespace IKVM.Compiler.Managed.Writer
{

    /// <summary>
    /// Represents a class that provides methods to emit data into an assembly.
    /// </summary>
    internal interface IManagedAssemblyWriter
    {

        /// <summary>
        /// Defines a named module in the assembly.
        /// </summary>
        /// <param name="name"></param>
        IManagedModuleWriter DefineModule(string name);

    }

}
