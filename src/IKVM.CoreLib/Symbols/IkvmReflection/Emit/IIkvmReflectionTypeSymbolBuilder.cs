using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionTypeSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionTypeSymbol>, IIkvmReflectionMemberSymbolBuilder, ITypeSymbolBuilder, IIkvmReflectionTypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="TypeBuilder"/>.
        /// </summary>
        TypeBuilder UnderlyingTypeBuilder { get; }

    }

}
