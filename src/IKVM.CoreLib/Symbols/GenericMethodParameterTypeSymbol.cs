using System;

namespace IKVM.CoreLib.Symbols
{

    abstract class GenericMethodParameterTypeSymbol : GenericParameterTypeSymbol
    {

        readonly MethodBaseSymbol _declaringMethod;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMethod"></param>
        protected GenericMethodParameterTypeSymbol(SymbolContext context, MethodSymbol declaringMethod) :
            base(context, declaringMethod.Module)
        {
            _declaringMethod = declaringMethod ?? throw new ArgumentNullException(nameof(declaringMethod));
        }

        /// <inheritdoc />
        public override MethodBaseSymbol? DeclaringMethod => _declaringMethod;

        /// <inheritdoc />
        public override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public override bool IsGenericMethodParameter => true;

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext genericContext)
        {
            return genericContext.GenericMethodArguments?[GenericParameterPosition] ?? this;
        }

    }

}
