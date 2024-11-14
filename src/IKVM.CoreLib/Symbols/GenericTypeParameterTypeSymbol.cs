namespace IKVM.CoreLib.Symbols
{

    abstract class GenericTypeParameterTypeSymbol : GenericParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        protected GenericTypeParameterTypeSymbol(SymbolContext context, TypeSymbol declaringType) :
            base(context, declaringType.Module)
        {

        }

        /// <inheritdoc />
        public override bool IsGenericTypeParameter => true;

        /// <inheritdoc />
        public override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext genericContext)
        {
            return genericContext.GenericTypeArguments?[GenericParameterPosition] ?? this;
        }

    }

}
