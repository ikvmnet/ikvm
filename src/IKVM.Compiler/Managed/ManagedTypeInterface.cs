using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an interface.
    /// </summary>
    internal readonly struct ManagedTypeInterface
    {

        readonly ManagedType type;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        internal ManagedTypeInterface(ManagedType type, int index)
        {
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Gets the type that declared this interface.
        /// </summary>
        public readonly ManagedType DeclaringType => type;

        /// <summary>
        /// Gets the signature of the type of the interface which is implemented
        /// </summary>
        public readonly ManagedSignature Type => type.data.Interfaces.GetItemRef(index).Type;

        /// <inheritdoc />
        public override readonly string ToString() => Type.ToString() ?? "";

    }

}
