using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionPropertySymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionPropertySymbol>, IIkvmReflectionMemberSymbolBuilder, IPropertySymbolBuilder, IIkvmReflectionPropertySymbol
    {

        PropertyBuilder UnderlyingPropertyBuilder { get; }

    }

}
