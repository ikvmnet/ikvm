using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Reference to an event.
    /// </summary>
    internal readonly struct ManagedTypeMethodGenericParameter
    {

        internal readonly ManagedTypeMethod method;
        internal readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="index"></param>
        internal ManagedTypeMethodGenericParameter(ManagedTypeMethod method, int index)
        {
            this.method = method;
            this.index = index;
        }

        /// <summary>
        /// Gets the method that declared this parameter.
        /// </summary>
        public ManagedTypeMethod Method => method;

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name => method.type.data.Methods.GetItemRef(method.index).GenericParameters.GetItemRef(index).Name ?? "";

        /// <summary>
        /// Gets the constraints applied to this generic parameter.
        /// </summary>
        public ManagedTypeMethodGenericParameterConstraintList Constraints => new ManagedTypeMethodGenericParameterConstraintList(this);

        /// <inheritdoc />
        public override string ToString() => Name;

    }

}
