using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a generic type parameter of a type or method.
    /// </summary>
    public readonly struct ManagedGenericParameter
    {

        readonly string name;
        readonly ReadOnlyFixedValueList<ManagedGenericParameterConstraint> constraints;

        /// <summary>
        /// Initailizes a new instance.
        /// </summary>
        /// <param name="name"></param>
        public ManagedGenericParameter(string name, in ReadOnlyFixedValueList<ManagedGenericParameterConstraint> constraints)
        {
            this.name = name;
            this.constraints = constraints;
        }

        /// <summary>
        /// Gets the name of the generic parameter.
        /// </summary>
        public readonly string Name => name;

        /// <summary>
        /// Gets the constraints applied to this generic parameter.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedGenericParameterConstraint> Constraints => constraints;

    }

}
