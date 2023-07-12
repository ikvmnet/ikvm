using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Holds the data of a managed assembly.
    /// </summary>
    internal struct ManagedAssemblyData
    {

        /// <summary>
        /// Gets the set of custom attributes applied to the assembly.
        /// </summary>
        public FixedValueList8<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets the set of references to other assemblies.
        /// </summary>
        public FixedValueList8<ManagedAssemblyReferenceData> References;

    }

}
