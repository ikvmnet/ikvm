using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionTypeSpecSymbol : ReflectionTypeSymbolBase
    {

        readonly IReflectionTypeSymbol _specifiedType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="specifiedType"></param>
        public ReflectionTypeSpecSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol specifiedType) :
            base(context, resolvingModule)
        {
            _specifiedType = specifiedType ?? throw new ArgumentNullException(nameof(specifiedType));
        }

        /// <summary>
        /// Gets the element type.
        /// </summary>
        protected IReflectionTypeSymbol SpecifiedType => _specifiedType;

        /// <inheritdoc />
        public override bool IsComplete => SpecifiedType.IsComplete;

    }

}
