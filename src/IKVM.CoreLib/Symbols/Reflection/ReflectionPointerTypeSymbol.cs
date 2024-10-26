using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionPointerTypeSymbol : ReflectionTypeSpecSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        public ReflectionPointerTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType) :
            base(context, resolvingModule, elementType)
        {

        }

        /// <inheritdoc />
        public override Type UnderlyingType => SpecifiedType.UnderlyingType.MakePointerType();

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => SpecifiedType.UnderlyingRuntimeType.MakePointerType();

    }

}
