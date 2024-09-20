using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionMethodBaseSymbolBuilder : IIkvmReflectionSymbolBuilder, IIkvmReflectionMemberSymbolBuilder, IMethodBaseSymbolBuilder, IIkvmReflectionMethodBaseSymbol
    {

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbolBuilder"/> for the given <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter);

    }

}
