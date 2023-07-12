using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    internal struct ManagedGenericParameterConstraint
    {

        /// <summary>
        /// Gets the type which the generic parameter is constrained to.
        /// </summary>
        public ManagedSignature Type;

        /// <summary>
        /// Gets the custom attributes applied to this constraint.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

    }

}
