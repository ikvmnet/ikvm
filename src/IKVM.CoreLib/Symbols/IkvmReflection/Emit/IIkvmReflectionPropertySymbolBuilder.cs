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

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbolBuilder"/> for the given <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter);

    }

}
