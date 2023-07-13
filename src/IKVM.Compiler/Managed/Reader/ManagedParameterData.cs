using System.Reflection;
using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Represents a managed method parameter.
    /// </summary>
    internal struct ManagedParameterData
    {

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string? Name;

        /// <summary>
        /// Gets the attributes for the parameter.
        /// </summary>
        public ParameterAttributes Attributes;

        /// <summary>
        /// Gets the custom attributes on the parameter.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public ManagedSignature ParameterType;

    }

}
