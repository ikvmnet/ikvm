namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Reference to a module of an assembly.
    /// </summary>
    internal readonly struct ManagedModuleCustomAttribute
    {

        readonly ManagedModule module;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        internal ManagedModuleCustomAttribute(ManagedModule module, int index)
        {
            this.module = module;
            this.index = index;
        }

        /// <summary>
        /// Gets the module that declared this attribute.
        /// </summary>
        public readonly ManagedModule Module => module;

        /// <inheritdoc />
        public override readonly string ToString() => "";

    }

}
