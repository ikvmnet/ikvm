using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionGenericTypeParameterSymbolBuilder : IReflectionSymbolBuilder, IGenericTypeParameterSymbolBuilder, IReflectionTypeSymbol
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
