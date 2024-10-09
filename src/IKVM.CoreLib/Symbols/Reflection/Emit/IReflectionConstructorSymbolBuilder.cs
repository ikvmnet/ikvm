using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionConstructorSymbolBuilder : IReflectionSymbolBuilder, IReflectionMethodBaseSymbolBuilder, IConstructorSymbolBuilder, IReflectionConstructorSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorBuilder"/>.
        /// </summary>
        ConstructorBuilder UnderlyingConstructorBuilder { get; }

    }

}
