using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionTypeSymbol : IIkvmReflectionMemberSymbol, ITypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="Type"/>.
        /// </summary>
        Type UnderlyingType { get; }

    }

}
