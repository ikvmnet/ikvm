using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a generic type parameter of a type or method.
    /// </summary>
    internal struct ManagedGenericParameterData
    {

        /// <summary>
        /// Gets the name of the generic parameter.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the constraints applied to this generic parameter.
        /// </summary>
        public FixedValueList1<ManagedGenericParameterConstraintData> Constraints;

    }

}
