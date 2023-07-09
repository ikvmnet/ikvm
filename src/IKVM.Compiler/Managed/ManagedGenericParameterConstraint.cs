using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    public readonly struct ManagedGenericParameterConstraint
    {

        /// <summary>
        /// Gets the type which the generic parameter is constrained to.
        /// </summary>
        public readonly ManagedSignature Type;

        /// <summary>
        /// Gets the custom attributes applied to this constraint.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Initailizes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="customAttributes"></param>
        public ManagedGenericParameterConstraint(in ManagedSignature type, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes)
        {
            Type = type;
            CustomAttributes = customAttributes;
        }

    }

}
