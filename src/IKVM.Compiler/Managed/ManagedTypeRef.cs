using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a managed type.
    /// </summary>
    public readonly struct ManagedTypeRef
    {

        readonly IManagedAssemblyContext context;
        readonly AssemblyName assemblyName;
        readonly string typeName;

        readonly ManagedType? managedType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <param name="managedType"></param>
        public ManagedTypeRef(IManagedAssemblyContext context, AssemblyName assemblyName, string typeName, ManagedType? managedType = null)
        {
            this.context = context;
            this.assemblyName = assemblyName;
            this.typeName = typeName;
            this.managedType = managedType;
        }

        /// <summary>
        /// Gets the assembly context responsible for loading this type reference.
        /// </summary>
        public IManagedAssemblyContext Context => context;

        /// <summary>
        /// Gets the name of the assembly of the type.
        /// </summary>
        public AssemblyName AssemblyName => assemblyName;

        /// <summary>
        /// Gets the name of the type being referenced.
        /// </summary>
        public string TypeName => typeName;

        /// <summary>
        /// Gets the optional managed type included along with this type reference. This may be populated if the
        /// reference was generated from the same assembly as the referenced type itself.
        /// </summary>
        public ManagedType? ManagedType => managedType;

    }

}
