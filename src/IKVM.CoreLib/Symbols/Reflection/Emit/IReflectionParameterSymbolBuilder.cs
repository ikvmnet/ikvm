using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionParameterSymbolBuilder : IReflectionSymbolBuilder<IReflectionParameterSymbol>, IParameterSymbolBuilder, IReflectionParameterSymbol
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
