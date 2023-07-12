namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeEvent
    {

        readonly ManagedType type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal ManagedTypeEvent(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this event.
        /// </summary>
        public ManagedType DeclaringType => type;

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        public string Name => type.data.Events[index].Name;

        /// <inheritdoc />
        public override string ToString() => type.data.Events[index].ToString();

    }

}
