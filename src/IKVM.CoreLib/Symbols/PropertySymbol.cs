using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class PropertySymbol : MemberSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public PropertySymbol(SymbolContext context, TypeSymbol declaringType) :
            base(context, declaringType.Module, declaringType)
        {

        }

        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        public abstract PropertyAttributes Attributes { get; }

        /// <inheritdoc />
        public sealed override MemberTypes MemberType => MemberTypes.Property;

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        public abstract TypeSymbol PropertyType { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        public abstract bool CanRead { get; }

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        public abstract bool CanWrite { get; }

        /// <summary>
        /// Gets a value indicating whether the property is the special name.
        /// </summary>
        public bool IsSpecialName => (Attributes & PropertyAttributes.SpecialName) != 0;

        /// <summary>
        /// Gets the get accessor for this property.
        /// </summary>
        public MethodSymbol? GetMethod => GetGetMethod(true);

        /// <summary>
        /// Gets the set accessor for this property.
        /// </summary>
        public MethodSymbol? SetMethod => GetSetMethod(true);

        /// <summary>
        /// Returns an array whose elements reflect the public get and set accessors of the property reflected by the current instance.
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<MethodSymbol> GetAccessors() => GetAccessors(false);

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public get and set accessors of the property reflected by the current instance.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract ImmutableArray<MethodSymbol> GetAccessors(bool nonPublic);

        /// <summary>
        /// Returns the public get accessor for this property.
        /// </summary>
        /// <returns></returns>
        public MethodSymbol? GetGetMethod() => GetGetMethod(false);

        /// <summary>
        /// When overridden in a derived class, returns the public or non-public get accessor for this property.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract MethodSymbol? GetGetMethod(bool nonPublic);

        /// <summary>
        /// Returns the public set accessor for this property.
        /// </summary>
        /// <returns></returns>
        public MethodSymbol? GetSetMethod() => GetSetMethod(false);

        /// <summary>
        /// When overridden in a derived class, returns the set accessor for this property.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract MethodSymbol? GetSetMethod(bool nonPublic);

        /// <summary>
        /// When overridden in a derived class, returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<ParameterSymbol> GetIndexParameters();

        /// <summary>
        /// Gets the modified type of this property object.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol GetModifiedPropertyType();

        /// <summary>
        /// Returns a literal value associated with the property by a compiler.
        /// </summary>
        /// <returns></returns>
        public abstract object? GetRawConstantValue();

        /// <summary>
        /// Returns an array of types representing the optional custom modifiers of the property.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// Returns an array of types representing the required custom modifiers of the property.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

    }

}
