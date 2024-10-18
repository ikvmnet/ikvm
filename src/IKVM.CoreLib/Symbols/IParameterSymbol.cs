using System.Reflection;

using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Discovers the attributes of a parameter and provides access to parameter metadata.
    /// </summary>
    interface IParameterSymbol : ISymbol, ICustomAttributeProvider
    {

        /// <summary>
        /// Gets the attributes for this parameter.
        /// </summary>
        ParameterAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        IMemberSymbol Member { get; }

        /// <summary>
        /// Gets the Type of this parameter.
        /// </summary>
        ITypeSymbol ParameterType { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// Gets the zero-based position of the parameter in the formal parameter list.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Gets a value that indicates whether this parameter has a default value.
        /// </summary>
        bool HasDefaultValue { get; }

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        object? DefaultValue { get; }

        /// <summary>
        /// Gets a value indicating whether this is an input parameter.
        /// </summary>
        bool IsIn { get; }

        /// <summary>
        /// Gets a value indicating whether this is an output parameter.
        /// </summary>
        bool IsOut { get; }

        /// <summary>
        /// Gets a value indicating whether this parameter is optional.
        /// </summary>
        bool IsOptional { get; }

        /// <summary>
        /// Gets a value indicating whether this parameter is a locale identifier (lcid).
        /// </summary>
        bool IsLcid { get; }

        /// <summary>
        /// Gets a value indicating whether this is a Retval parameter.
        /// </summary>
        bool IsRetval { get; }

        /// <summary>
        /// Gets a value that identifies this parameter in metadata.
        /// </summary>
        int MetadataToken { get; }

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the parameter.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetOptionalCustomModifiers();

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the parameter.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetRequiredCustomModifiers();

    }

}
