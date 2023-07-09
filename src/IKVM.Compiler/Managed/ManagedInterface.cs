using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an interface of a managed type.
    /// </summary>
    public readonly struct ManagedInterface
    {

        /// <summary>
        /// Type of the implemented interface.
        /// </summary>
        public readonly ManagedSignature Type;

        /// <summary>
        /// Gets the set of custom attributes applied to the interface.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="customAttributes"></param>
        public ManagedInterface(in ManagedSignature type, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes)
        {
            Type = type;
            CustomAttributes = customAttributes;
        }

    }

}
