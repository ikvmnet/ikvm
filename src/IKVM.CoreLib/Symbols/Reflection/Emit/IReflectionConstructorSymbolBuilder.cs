using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionConstructorSymbolBuilder : IReflectionSymbolBuilder<IReflectionConstructorSymbol>, IReflectionMethodBaseSymbolBuilder, IConstructorSymbolBuilder, IReflectionConstructorSymbol
    {

        ConstructorBuilder UnderlyingConstructorBuilder { get; }

    }

}
