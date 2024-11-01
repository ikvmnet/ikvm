using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionMethodSymbolBuilder : IIkvmReflectionSymbolBuilder, IIkvmReflectionMethodBaseSymbolBuilder, IMethodSymbolBuilder, IIkvmReflectionMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodBuilder"/>.
        /// </summary>
        MethodBuilder UnderlyingMethodBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionGenericTypeParameterSymbolBuilder"/> for the given <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter);

    }

}
