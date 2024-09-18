using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionEventSymbolBuilder : IReflectionSymbolBuilder<IReflectionEventSymbol>, IReflectionMemberSymbolBuilder, IEventSymbolBuilder, IReflectionEventSymbol
    {

        EventBuilder UnderlyingEventBuilder { get; }

    }

}
