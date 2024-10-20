using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionEventSymbol : IReflectionMemberSymbol, IEventSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="EventInfo"/>.
        /// </summary>
        EventInfo UnderlyingEvent { get; }

        /// <summary>
        /// Gets the underlying <see cref="EventInfo"/>.
        /// </summary>
        EventInfo UnderlyingRuntimeEvent { get; }


    }

}
