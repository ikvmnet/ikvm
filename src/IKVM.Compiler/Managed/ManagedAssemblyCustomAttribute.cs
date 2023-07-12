namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an custom attribute on an assembly.
    /// </summary>
    internal readonly struct ManagedAssemblyCustomAttribute
    {

        readonly ManagedAssembly assembly;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringAssembly"></param>
        internal ManagedAssemblyCustomAttribute(ManagedAssembly declaringAssembly, int index)
        {
            this.assembly = declaringAssembly;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this attribute.
        /// </summary>
        public readonly ManagedAssembly DeclaringAssembly => assembly;

        /// <inheritdoc />
        public override readonly string ToString() => "";

    }

}
