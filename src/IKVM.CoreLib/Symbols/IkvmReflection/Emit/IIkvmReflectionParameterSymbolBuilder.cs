using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionParameterSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionParameterSymbol>, IParameterSymbolBuilder, IIkvmReflectionParameterSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ParameterBuilder"/>.
        /// </summary>
        ParameterBuilder UnderlyingParameterBuilder { get; }

        /// <summary>
        /// Invoked when the owning method of this parameter is completed.
        /// </summary>
        void OnComplete();

    }

}
