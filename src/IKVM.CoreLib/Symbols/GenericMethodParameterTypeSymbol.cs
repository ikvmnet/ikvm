using System;

namespace IKVM.CoreLib.Symbols
{

    public abstract class GenericMethodParameterTypeSymbol : GenericParameterTypeSymbol
    {

        readonly MethodSymbol _declaringMethod;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMethod"></param>
        protected GenericMethodParameterTypeSymbol(SymbolContext context, MethodSymbol declaringMethod) :
            base(context, declaringMethod.Module, declaringMethod.DeclaringType)
        {
            _declaringMethod = declaringMethod ?? throw new ArgumentNullException(nameof(declaringMethod));
        }

        /// <inheritdoc />
        public override MethodSymbol? DeclaringMethod => _declaringMethod;

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
