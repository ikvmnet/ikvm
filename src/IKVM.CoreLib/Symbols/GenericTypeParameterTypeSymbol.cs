using System;

namespace IKVM.CoreLib.Symbols
{

    public abstract class GenericTypeParameterTypeSymbol : GenericParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected GenericTypeParameterTypeSymbol(SymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => true;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext genericContext)
        {
            if (genericContext.GenericTypeArguments.IsDefaultOrEmpty)
                throw new InvalidOperationException();

            return genericContext.GenericTypeArguments[GenericParameterPosition];
        }

    }

}
