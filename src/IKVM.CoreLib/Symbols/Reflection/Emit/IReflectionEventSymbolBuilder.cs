using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionEventSymbolBuilder : IReflectionSymbolBuilder, IReflectionMemberSymbolBuilder, IEventSymbolBuilder, IReflectionEventSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="EventBuilder"/>.
        /// </summary>
        EventBuilder UnderlyingEventBuilder { get; }

    }

}
