using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed assembly reference.
    /// </summary>
    internal readonly struct ManagedAssemblyReference
    {

        readonly ManagedAssembly assembly;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        internal ManagedAssemblyReference(ManagedAssembly assembly, int index)
        {
            this.assembly = assembly;
            this.index = index;
        }

        /// <summary>
        /// Gets the assembly that declared this reference.
        /// </summary>
        public readonly ManagedAssembly DeclaringAssembly => assembly;

        /// <summary>
        /// Gets the name of the reference.
        /// </summary>
        public readonly AssemblyName Name => assembly.data.References[index].AssemblyName;

        /// <inheritdoc />
        public override string ToString() => Name.ToString();

    }

}
