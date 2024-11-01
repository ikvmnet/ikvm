using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionPropertySymbolBuilder : IReflectionSymbolBuilder, IReflectionMemberSymbolBuilder, IPropertySymbolBuilder, IReflectionPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyBuilder"/>.
        /// </summary>
        PropertyBuilder UnderlyingPropertyBuilder { get; }

    }

}
