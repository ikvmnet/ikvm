using System.Diagnostics.CodeAnalysis;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    /// <summary>
    /// Reflection-specific implementation of <see cref="ISymbolBuilder"/>.
    /// </summary>
    abstract class IkvmReflectionSymbolBuilder : IkvmReflectionSymbol, IIkvmReflectionSymbolBuilder
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected IkvmReflectionSymbolBuilder(IkvmReflectionSymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        [return: NotNullIfNotNull("genericTypeParameter")]
        public IIkvmReflectionGenericTypeParameterSymbolBuilder? ResolveGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return Context.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

    }

}
