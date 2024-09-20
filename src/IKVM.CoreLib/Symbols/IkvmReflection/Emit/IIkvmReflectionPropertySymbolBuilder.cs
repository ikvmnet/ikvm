using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionPropertySymbolBuilder : IIkvmReflectionSymbolBuilder, IIkvmReflectionMemberSymbolBuilder, IPropertySymbolBuilder, IIkvmReflectionPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyBuilder"/>.
        /// </summary>
        PropertyBuilder UnderlyingPropertyBuilder { get; }

    }

}
