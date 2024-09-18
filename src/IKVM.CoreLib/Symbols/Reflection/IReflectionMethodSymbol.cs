using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionMethodSymbol : IReflectionMethodBaseSymbol, IMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/>.
        /// </summary>
        MethodInfo UnderlyingMethod { get; }

    }

}
