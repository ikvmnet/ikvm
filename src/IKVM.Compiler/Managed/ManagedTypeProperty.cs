namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeProperty
    {

        readonly ManagedType type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal ManagedTypeProperty(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this property.
        /// </summary>
        public ManagedType DeclaringType => type;

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name => type.data.Properties[index].Name ?? "";

        /// <inheritdoc />
        public override string ToString() => Name;

    }

}
