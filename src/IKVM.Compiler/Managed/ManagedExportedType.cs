using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed exported type.
    /// </summary>
    internal struct ManagedExportedType
    {

        /// <summary>
        /// Gets the name of the exported type.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the set of custom attributes applied to the exported type.
        /// </summary>
        public FixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
