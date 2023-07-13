using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
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
        public readonly ManagedType DeclaringType => type;

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public readonly string Name => type.data.Fields.GetItemRef(index).Name ?? "";

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public readonly ManagedSignature FieldType => type.data.Fields.GetItemRef(index).FieldType;

        /// <inheritdoc />
        public readonly override string ToString() => Name;

    }

}
