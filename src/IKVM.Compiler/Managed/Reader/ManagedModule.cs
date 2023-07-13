using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedModule
    {

        internal readonly ManagedAssembly assembly;
        internal readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        internal ManagedModule(ManagedAssembly assembly, int index)
        {
            this.assembly = assembly;
            this.index = index;
        }

        /// <summary>
        /// Gets the assembly that declared this module.
        /// </summary>
        public readonly ManagedAssembly Assembly => assembly;

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        public readonly string Name => assembly.data.Modules.GetItemRef(index).Name ?? "";

        /// <inheritdoc />
        public readonly override string ToString() => Name;

    }

}
