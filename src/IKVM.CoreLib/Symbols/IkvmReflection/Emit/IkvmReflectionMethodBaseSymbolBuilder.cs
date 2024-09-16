using System;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    abstract class IkvmReflectionMethodBaseSymbolBuilder<TSymbol, TReflectionSymbol> : IkvmReflectionSymbolBuilder<TSymbol, TReflectionSymbol>
        where TSymbol : ISymbol
        where TReflectionSymbol : IkvmReflectionSymbol, TSymbol
    {

        readonly IkvmReflectionModuleSymbolBuilder _containingModuleBuilder;
        readonly IkvmReflectionTypeSymbolBuilder? _containingTypeBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        protected IkvmReflectionMethodBaseSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbolBuilder containingTypeBuilder) :
            base(context)
        {
            _containingTypeBuilder = containingTypeBuilder ?? throw new ArgumentNullException(nameof(containingTypeBuilder));
            _containingModuleBuilder = containingTypeBuilder.ContainingModuleBuilder;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingModuleBuilder"></param>
        protected IkvmReflectionMethodBaseSymbolBuilder(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbolBuilder containingModuleBuilder) :
            base(context)
        {
            _containingModuleBuilder = containingModuleBuilder ?? throw new ArgumentNullException(nameof(containingModuleBuilder));
        }

        /// <summary>
        /// Gets the containing <see cref="IkvmReflectionModuleSymbolBuilder"/>.
        /// </summary>
        protected internal IkvmReflectionModuleSymbolBuilder ContainingModuleBuilder => _containingModuleBuilder;

        /// <summary>
        /// Gets the containing <see cref="IkvmReflectionTypeSymbolBuilder"/>.
        /// </summary>
        protected internal IkvmReflectionTypeSymbolBuilder? ContainingTypeBuilder => _containingTypeBuilder;

    }

}
