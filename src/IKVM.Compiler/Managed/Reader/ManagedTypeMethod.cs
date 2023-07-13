namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeMethod
    {

        internal readonly ManagedType type;
        internal readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal ManagedTypeMethod(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this method.
        /// </summary>
        public ManagedType DeclaringType => type;

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name => type.data.Methods[index].Name ?? "";

        /// <summary>
        /// Gets the parameters of the method.
        /// </summary>
        public ManagedTypeMethodParameterList Parameters => new ManagedTypeMethodParameterList(this);

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public ManagedSignature ReturnType => type.data.Methods[index].ReturnType;

        /// <inheritdoc />
        public override string ToString() => Name;

    }

}
