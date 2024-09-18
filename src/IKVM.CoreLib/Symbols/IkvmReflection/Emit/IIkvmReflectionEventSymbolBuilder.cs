using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionEventSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionEventSymbol>, IIkvmReflectionMemberSymbolBuilder, IEventSymbolBuilder, IIkvmReflectionEventSymbol
    {

        EventBuilder UnderlyingEventBuilder { get; }

    }

}
