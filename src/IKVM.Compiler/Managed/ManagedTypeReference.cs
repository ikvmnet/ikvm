using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a managed type.
    /// </summary>
    internal readonly struct ManagedTypeReference
    {

        readonly AssemblyName assemblyName;
        readonly string typeName;
        readonly IManagedAssemblyContext? assemblyContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="assemblyContext"></param>
        public ManagedTypeReference(AssemblyName assemblyName, string typeName, IManagedAssemblyContext? assemblyContext)
        {
            this.assemblyName = assemblyName;
            this.typeName = typeName;
            this.assemblyContext = assemblyContext;
        }

        /// <summary>
        /// Gets the name of the assembly of the type.
        /// </summary>
        public AssemblyName AssemblyName => assemblyName;

        /// <summary>
        /// Gets the name of the type being referenced.
        /// </summary>
        public string TypeName => typeName;

        /// <summary>
        /// Optionally gets the known <see cref="IManagedAssemblyContext"/> that holds the type.
        /// </summary>
        public IManagedAssemblyContext? AssemblyContext => assemblyContext;

    }

}
