using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of information for a <see cref="DefinitionPropertySymbol" />.
    /// </summary>
    interface IPropertyLoader
    {

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetDeclaringType();

        /// <summary>
        /// Gets whether or not the property is missing.
        /// </summary>
        /// <returns></returns>
        bool GetIsMissing();

        /// <summary>
        /// Gets the attributes of the property.
        /// </summary>
        /// <returns></returns>
        PropertyAttributes GetAttributes();

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetPropertyType();
        
        /// <summary>
        /// Gets the get method of the property.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? GetGetMethod();

        /// <summary>
        /// Gets the set method of the property.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? GetSetMethod();

        /// <summary>
        /// Loads the optional custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Loads the required custom modifiers of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <summary>
        /// Loads the custom attributes of the type.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();

        /// <summary>
        /// Gets the parameters of the property indexer.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ParameterSymbol> GetIndexParameters();

        /// <summary>
        /// Gets the modified property type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetModifiedPropertyType();

        /// <summary>
        /// Gets the constant vlaue.
        /// </summary>
        /// <returns></returns>
        object? GetRawConstantValue();
    }

}
