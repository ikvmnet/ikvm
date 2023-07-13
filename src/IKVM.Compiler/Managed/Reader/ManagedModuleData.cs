using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    internal struct ManagedModuleData
    {

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the set of custom attributes applied to the module.
        /// </summary>
        public FixedValueList8<ManagedCustomAttributeData> CustomAttributes;

    }

}
