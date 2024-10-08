using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionMethodSymbol : IReflectionMethodBaseSymbol, IMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/>.
        /// </summary>
        MethodInfo UnderlyingMethod { get; }

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/> used for IL emit operations.
        /// </summary>
        MethodInfo UnderlyingEmitMethod { get; }

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/> used for IL emit operations against dynamic methods.
        /// </summary>
        MethodInfo UnderlyingDynamicEmitMethod { get; }

        /// <summary>
        /// Gets or creates a <see cref="ITypeSymbol"/> for the given generic type parameter.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> for the given generic version of this method definition.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodSymbol GetOrCreateGenericMethodSymbol(MethodInfo method);

    }

}
