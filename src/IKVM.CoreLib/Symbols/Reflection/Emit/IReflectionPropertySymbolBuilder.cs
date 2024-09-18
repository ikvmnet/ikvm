using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionPropertySymbolBuilder : IReflectionSymbolBuilder<IReflectionPropertySymbol>, IReflectionMemberSymbolBuilder, IPropertySymbolBuilder, IReflectionPropertySymbol
    {

        PropertyBuilder UnderlyingPropertyBuilder { get; }

    }

}
