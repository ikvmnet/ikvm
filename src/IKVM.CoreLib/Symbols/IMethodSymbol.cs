namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Discovers the attributes of a method and provides access to method metadata.
    /// </summary>
    interface IMethodSymbol : IMethodBaseSymbol
    {

        /// <summary>
        /// Gets a <see cref="IParameterSymbol"/> object that contains information about the return type of the method, such as whether the return type has custom modifiers.
        /// </summary>
        IParameterSymbol ReturnParameter { get; }

        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        ITypeSymbol ReturnType { get; }

        /// <summary>
        /// Gets the custom attributes for the return type.
        /// </summary>
        ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="IMethodSymbol"> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol GetBaseDefinition();

        /// <summary>
        /// Returns a <see cref="IMethodSymbol"> object that represents a generic method definition from which the current method can be constructed.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol GetGenericMethodDefinition();

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a MethodInfo object representing the resulting constructed method.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments);

    }

}