using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Emit
{

    interface IReflectionGenericTypeParameterSymbolBuilder : IReflectionSymbolBuilder<IReflectionTypeSymbol>, IGenericTypeParameterSymbolBuilder, IReflectionTypeSymbol
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
