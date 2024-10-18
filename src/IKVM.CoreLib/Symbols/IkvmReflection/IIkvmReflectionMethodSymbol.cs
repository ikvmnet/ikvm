using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionMethodSymbol : IIkvmReflectionMethodBaseSymbol, IMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/>.
        /// </summary>
        MethodInfo UnderlyingMethod { get; }

        /// <summary>
        /// Gets or creates a <see cref="ITypeSymbol"/> for the given generic type parameter.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbol"/> for the given generic version of this method definition.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbol GetOrCreateGenericMethodSymbol(MethodInfo method);

    }

}
