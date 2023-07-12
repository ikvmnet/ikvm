using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeGenericParameter
    {

        internal readonly ManagedType type;
        internal readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal ManagedTypeGenericParameter(ManagedType type, int index)
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
        public readonly string Name => type.data.GenericParameters.GetItemRef(index).Name ?? "";

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public readonly ManagedTypeGenericParameterConstraintList Constraints => new ManagedTypeGenericParameterConstraintList(this);

        /// <inheritdoc />
        public override string ToString() => Name;

    }

}
