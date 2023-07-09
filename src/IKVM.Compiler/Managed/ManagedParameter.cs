using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method parameter.
    /// </summary>
    public readonly struct ManagedParameter
    {

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public readonly string? Name;

        /// <summary>
        /// Gets the attributes for the parameter.
        /// </summary>
        public readonly ParameterAttributes Attributes;

        /// <summary>
        /// Gets the custom attributes on the parameter.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;
        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public readonly ManagedSignature ParameterType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameterType"></param>
        public ManagedParameter(string? name, ParameterAttributes attributes, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes, in ManagedSignature parameterType)
        {
            Name = name;
            Attributes = attributes;
            CustomAttributes = customAttributes;
            ParameterType = parameterType;
        }

    }

}
