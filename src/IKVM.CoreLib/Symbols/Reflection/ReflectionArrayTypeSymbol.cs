using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionArrayTypeSymbol : ReflectionTypeSpecSymbol
    {

        readonly int rank;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        public ReflectionArrayTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType, int rank) :
            base(context, resolvingModule, elementType)
        {
            this.rank = rank;
        }

        /// <inheritdoc />
        public override Type UnderlyingType => SpecifiedType.UnderlyingType.MakeArrayType(rank);

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => SpecifiedType.UnderlyingRuntimeType.MakeArrayType(rank);

    }

}
