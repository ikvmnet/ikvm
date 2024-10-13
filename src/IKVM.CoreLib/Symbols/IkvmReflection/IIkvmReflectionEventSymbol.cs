using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionEventSymbol : IIkvmReflectionMemberSymbol, IEventSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="EventInfo"/>.
        /// </summary>
        EventInfo UnderlyingEvent { get; }

    }

}
