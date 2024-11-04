using System;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    abstract class GenericMethodParameterTypeSymbol : GenericParameterTypeSymbol
    {

        readonly MethodBaseSymbol _declaringMethod;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringMethod"></param>
        protected GenericMethodParameterTypeSymbol(ISymbolContext context, IModuleSymbol module, MethodBaseSymbol declaringMethod) :
            base(context, module, null)
        {
            _declaringMethod = declaringMethod ?? throw new ArgumentNullException(nameof(declaringMethod));
        }

        /// <inheritdoc />
        public override MethodBaseSymbol? DeclaringMethod => _declaringMethod;

        /// <inheritdoc />
        internal override TypeSymbol Specialize(IImmutableList<TypeSymbol>? genericTypeArguments, IImmutableList<TypeSymbol>? genericMethodArguments)
        {
            if (genericMethodArguments == null)
                throw new InvalidOperationException();

            return genericMethodArguments[GenericParameterPosition];
        }

    }

}
