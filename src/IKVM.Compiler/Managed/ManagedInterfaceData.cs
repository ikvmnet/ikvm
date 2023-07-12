using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an interface of a managed type.
    /// </summary>
    internal struct ManagedInterfaceData
    {

        /// <summary>
        /// Type of the implemented interface.
        /// </summary>
        public ManagedSignature Type;

        /// <summary>
        /// Gets the set of custom attributes applied to the interface.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

    }

}
