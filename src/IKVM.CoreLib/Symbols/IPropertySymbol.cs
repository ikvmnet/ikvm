using System.Reflection;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Discovers the attributes of a property and provides access to property metadata.
    /// </summary>
    interface IPropertySymbol : IMemberSymbol
    {

        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        PropertyAttributes Attributes { get; }

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        ITypeSymbol PropertyType { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        bool CanRead { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        bool CanWrite { get; }

        /// <summary>
        /// Gets a value indicating whether the property is the special name.
        /// </summary>
        bool IsSpecialName { get; }

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <returns></returns>
        object? GetRawConstantValue();

        /// <summary>
        /// Gets the get accessor for this property.
        /// </summary>
        IMethodSymbol? GetMethod { get; }

        /// <summary>
        /// Gets the set accessor for this property.
        /// </summary>
        IMethodSymbol? SetMethod { get; }

        /// <summary>
        /// Returns an array whose elements reflect the public get and set accessors of the property reflected by the current instance.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol[] GetAccessors();

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public get and set accessors of the property reflected by the current instance.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        IMethodSymbol[] GetAccessors(bool nonPublic);

        /// <summary>
        /// Returns the public get accessor for this property.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol? GetGetMethod();

        /// <summary>
        /// When overridden in a derived class, returns the public or non-public get accessor for this property.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        IMethodSymbol? GetGetMethod(bool nonPublic);

        /// <summary>
        /// Returns the public set accessor for this property.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol? GetSetMethod();

        /// <summary>
        /// When overridden in a derived class, returns the set accessor for this property.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        IMethodSymbol? GetSetMethod(bool nonPublic);

        /// <summary>
        /// When overridden in a derived class, returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns></returns>
        IParameterSymbol[] GetIndexParameters();

        /// <summary>
        /// Gets the modified type of this property object.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol GetModifiedPropertyType();

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the property.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetOptionalCustomModifiers();

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the property.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetRequiredCustomModifiers();

    }

}
