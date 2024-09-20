using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionEventSymbolBuilder : IIkvmReflectionSymbolBuilder, IIkvmReflectionMemberSymbolBuilder, IEventSymbolBuilder, IIkvmReflectionEventSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="EventBuilder"/>.
        /// </summary>
        EventBuilder UnderlyingEventBuilder { get; }

    }

}
