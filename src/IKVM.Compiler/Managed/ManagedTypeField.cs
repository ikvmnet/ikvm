namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeField
    {

        readonly ManagedType type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal ManagedTypeField(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this field.
        /// </summary>
        public ManagedType DeclaringType => type;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name => type.data.Fields[index].Name ?? "";

        /// <inheritdoc />
        public override string ToString() => Name;

    }

}
