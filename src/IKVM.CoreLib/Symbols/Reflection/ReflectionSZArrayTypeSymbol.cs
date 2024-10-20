using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionSZArrayTypeSymbol : ReflectionTypeSpecSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        public ReflectionSZArrayTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType) :
            base(context, resolvingModule, elementType)
        {

        }

        /// <inheritdoc />
        public override Type UnderlyingType => ElementType.UnderlyingType.MakeArrayType();

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => ElementType.UnderlyingRuntimeType.MakeArrayType();

    }

}
