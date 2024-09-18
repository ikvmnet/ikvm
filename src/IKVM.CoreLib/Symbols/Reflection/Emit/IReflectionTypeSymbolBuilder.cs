using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionTypeSymbolBuilder : IReflectionSymbolBuilder<IReflectionTypeSymbol>, IReflectionMemberSymbolBuilder, ITypeSymbolBuilder, IReflectionTypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="TypeBuilder"/>.
        /// </summary>
        TypeBuilder UnderlyingTypeBuilder { get; }

    }

}
