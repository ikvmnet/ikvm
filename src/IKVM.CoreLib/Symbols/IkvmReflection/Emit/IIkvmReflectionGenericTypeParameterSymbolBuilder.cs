using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionGenericTypeParameterSymbolBuilder : IIkvmReflectionSymbolBuilder, IGenericTypeParameterSymbolBuilder, IIkvmReflectionTypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        GenericTypeParameterBuilder UnderlyingGenericTypeParameterBuilder { get; }

        /// <summary>
        /// Invoked when the type containing this generic type parameter is completed.
        /// </summary>
        void OnComplete();

    }

}
