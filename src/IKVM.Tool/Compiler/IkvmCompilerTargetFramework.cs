namespace IKVM.Tool.Compiler
{

    /// <summary>
    /// Available tool frameworks.
    /// </summary>
    public enum IkvmCompilerTargetFramework
    {

        /// <summary>
        /// indicates that we will be emitting a .NET Framework version of the assembly.
        /// </summary>
        NetFramework,

        /// <summary>
        /// Indicates that we will be emitting a .NET Core version of the assembly.
        /// </summary>
        NetCore

    }

}