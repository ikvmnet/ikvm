using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    public readonly struct ManagedGenericParameterConstraint
    {

        readonly ManagedTypeSignature type;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;

        /// <summary>
        /// Initailizes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="customAttributes"></param>
        public ManagedGenericParameterConstraint(ManagedTypeSignature type, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes)
        {
            this.type = type;
            this.customAttributes = customAttributes;
        }

        /// <summary>
        /// Gets the type which the generic parameter is constrained to.
        /// </summary>
        public readonly ManagedTypeSignature Type => type;

        /// <summary>
        /// Gets the custom attributes applied to this constraint.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

    }

}
