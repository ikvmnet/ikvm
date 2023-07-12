using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeGenericParameterConstraint
    {

        readonly ManagedTypeGenericParameter parameter;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        internal ManagedTypeGenericParameterConstraint(ManagedTypeGenericParameter parameter, int index)
        {
            this.parameter = parameter;
            this.index = index;
        }

        /// <summary>
        /// Gets the parameter that declared this constraint.
        /// </summary>
        public readonly ManagedTypeGenericParameter DeclaringParameter => parameter;

        /// <summary>
        /// Gets the constrained type.
        /// </summary>
        public readonly ManagedSignature Type => parameter.type.data.GenericParameters.GetItemRef(parameter.index).Constraints.GetItemRef(index).Type;

        /// <inheritdoc />
        public override readonly string ToString() => Type.ToString() ?? "";

    }

}
