using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionTypeSpecSymbol : ReflectionTypeSymbolBase
    {

        readonly IReflectionTypeSymbol _elementType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        public ReflectionTypeSpecSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType) :
            base(context, resolvingModule)
        {
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
        }

        /// <summary>
        /// Gets the element type.
        /// </summary>
        protected IReflectionTypeSymbol ElementType => _elementType;

        /// <inheritdoc />
        public override bool IsComplete => ElementType.IsComplete;

    }

}
