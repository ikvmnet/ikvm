using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed assembly reference.
    /// </summary>
    internal struct ManagedAssemblyReferenceData
    {

        /// <summary>
        /// Gets the name of the managed assembly reference.
        /// </summary>
        public AssemblyName AssemblyName;

        /// <inhericdoc />
        public override readonly string ToString() => AssemblyName.ToString();

    }

}
