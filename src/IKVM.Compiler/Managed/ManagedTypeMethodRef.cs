namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a reference to a managed method.
    /// </summary>
    internal readonly struct ManagedTypeMethodRef
    {

        readonly ManagedType type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public ManagedTypeMethodRef(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string Name => type.Methods[index].Name;

    }

}
