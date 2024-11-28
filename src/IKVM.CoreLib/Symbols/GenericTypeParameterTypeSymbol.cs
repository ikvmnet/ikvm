using System;

namespace IKVM.CoreLib.Symbols
{

    public abstract class GenericTypeParameterTypeSymbol : GenericParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        protected GenericTypeParameterTypeSymbol(SymbolContext context, TypeSymbol declaringType) :
            base(context, declaringType.Module, declaringType)
        {

        }

        /// <inheritdoc />
        public override bool IsGenericTypeParameter => true;

        /// <inheritdoc />
        public override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext genericContext)
        {
            if (genericContext.GenericTypeArguments.IsDefaultOrEmpty)
                throw new InvalidOperationException();

            return genericContext.GenericTypeArguments[GenericParameterPosition];
        }

    }

}
