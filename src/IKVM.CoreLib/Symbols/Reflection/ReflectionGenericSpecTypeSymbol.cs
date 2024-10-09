using System;
using System.Linq;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionGenericSpecTypeSymbol : ReflectionTypeSpecSymbol
    {

        readonly IReflectionTypeSymbol[] genericTypeArguments;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="elementType"></param>
        /// <param name="genericTypeArguments"></param>
        public ReflectionGenericSpecTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol elementType, IReflectionTypeSymbol[] genericTypeArguments) :
            base(context, resolvingModule, elementType)
        {
            this.genericTypeArguments = genericTypeArguments ?? throw new ArgumentNullException(nameof(genericTypeArguments));
        }

        /// <inheritdoc />
        public override Type UnderlyingType => ElementType.UnderlyingType.MakeGenericType(genericTypeArguments.Select(i => i.UnderlyingType).ToArray());

        /// <inheritdoc />
        public override Type UnderlyingEmitType => ElementType.UnderlyingEmitType.MakeGenericType(genericTypeArguments.Select(i => i.UnderlyingEmitType).ToArray());

        /// <inheritdoc />
        public override Type UnderlyingDynamicEmitType => ElementType.UnderlyingDynamicEmitType.MakeGenericType(genericTypeArguments.Select(i => i.UnderlyingDynamicEmitType).ToArray());

    }

}
