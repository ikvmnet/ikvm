using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionMethodSymbolBuilder : IReflectionSymbolBuilder, IReflectionMethodBaseSymbolBuilder, IMethodSymbolBuilder, IReflectionMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodBuilder"/>.
        /// </summary>
        MethodBuilder UnderlyingMethodBuilder { get; }

    }

}
