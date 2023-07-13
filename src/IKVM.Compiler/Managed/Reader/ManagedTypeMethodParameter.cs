using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Reference to a parameter of a method.
    /// </summary>
    internal readonly struct ManagedTypeMethodParameter
    {

        internal readonly ManagedTypeMethod method;
        internal readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="index"></param>
        internal ManagedTypeMethodParameter(ManagedTypeMethod method, int index)
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
        public string Name => method.type.data.Methods.GetItemRef(method.index).Parameters.GetItemRef(index).Name ?? "";

        /// <inheritdoc />
        public override string ToString() => Name;

    }

}
