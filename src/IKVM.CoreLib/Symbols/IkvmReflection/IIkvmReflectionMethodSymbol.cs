using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionMethodSymbol : IIkvmReflectionMethodBaseSymbol, IMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodInfo"/>.
        /// </summary>
        MethodInfo UnderlyingMethod { get; }

    }

}
