using System;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    abstract class GenericTypeParameterTypeSymbol : GenericParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected GenericTypeParameterTypeSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol declaringType) :
            base(context, module, declaringType)
        {

        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(IImmutableList<TypeSymbol>? genericTypeArguments, IImmutableList<TypeSymbol>? genericMethodArguments)
        {
            if (genericTypeArguments == null)
                throw new InvalidOperationException();

            return genericTypeArguments[GenericParameterPosition];
        }

    }

}
