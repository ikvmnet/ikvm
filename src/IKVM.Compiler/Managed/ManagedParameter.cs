using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed method parameter.
    /// </summary>
    public readonly struct ManagedParameter
    {

        readonly string? name;
        readonly ParameterAttributes attributes;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ManagedTypeSignature parameterType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameterType"></param>
        public ManagedParameter(string? name, ParameterAttributes attributes, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, ManagedTypeSignature parameterType)
        {
            this.name = name;
            this.attributes = attributes;
            this.customAttributes = customAttributes;
            this.parameterType = parameterType;
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public readonly string? Name => name;

        /// <summary>
        /// Gets the attributes for the parameter.
        /// </summary>
        public readonly ParameterAttributes Attributes => attributes;

        /// <summary>
        /// Gets the custom attributes on the parameter.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public readonly ManagedTypeSignature ParameterType => parameterType;

    }

}
