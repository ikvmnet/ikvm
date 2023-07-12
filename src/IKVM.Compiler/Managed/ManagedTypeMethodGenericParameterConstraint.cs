namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Reference to a generic method parameter constraint..
    /// </summary>
    internal readonly struct ManagedTypeMethodGenericParameterConstraint
    {

        internal readonly ManagedTypeMethodGenericParameter parameter;
        internal readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="index"></param>
        internal ManagedTypeMethodGenericParameterConstraint(ManagedTypeMethodGenericParameter parameter, int index)
        {
            this.parameter = parameter;
            this.index = index;
        }

        /// <summary>
        /// Gets the generic method parameter that declared this constraint.
        /// </summary>
        public ManagedTypeMethodGenericParameter Parameter => parameter;

        /// <inheritdoc />
        public override string ToString() => "";

    }

}
