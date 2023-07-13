namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Reference to an custom attribute on a type.
    /// </summary>
    internal readonly struct ManagedTypeCustomAttribute
    {

        readonly ManagedType type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal ManagedTypeCustomAttribute(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this attribute.
        /// </summary>
        public readonly ManagedType DeclaringType => type;

        /// <inheritdoc />
        public override readonly string ToString() => "";

    }

}
