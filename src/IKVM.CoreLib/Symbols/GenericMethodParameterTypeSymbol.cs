using System;

namespace IKVM.CoreLib.Symbols
{

    public abstract class GenericMethodParameterTypeSymbol : GenericParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected GenericMethodParameterTypeSymbol(SymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => DeclaringMethod?.DeclaringType ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public override bool IsGenericMethodParameter => true;

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext genericContext)
        {
            return genericContext.GenericMethodArguments.IsDefault == false ? genericContext.GenericMethodArguments[GenericParameterPosition] : this;
        }

    }

}
