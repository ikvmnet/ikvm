using System;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    abstract class ReflectionMethodBaseSymbolBuilder<TSymbol, TReflectionSymbol> : ReflectionSymbolBuilder<TSymbol, TReflectionSymbol>
        where TSymbol : ISymbol
        where TReflectionSymbol : ReflectionSymbol, TSymbol
    {

        readonly ReflectionModuleSymbolBuilder _containingModuleBuilder;
        readonly ReflectionTypeSymbolBuilder? _containingTypeBuilder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingTypeBuilder"></param>
        protected ReflectionMethodBaseSymbolBuilder(ReflectionSymbolContext context, ReflectionTypeSymbolBuilder containingTypeBuilder) :
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
        protected ReflectionMethodBaseSymbolBuilder(ReflectionSymbolContext context, ReflectionModuleSymbolBuilder containingModuleBuilder) :
            base(context)
        {
            _containingModuleBuilder = containingModuleBuilder ?? throw new ArgumentNullException(nameof(containingModuleBuilder));
        }

        /// <summary>
        /// Gets the containing <see cref="ReflectionModuleSymbolBuilder"/>.
        /// </summary>
        protected internal ReflectionModuleSymbolBuilder ContainingModuleBuilder => _containingModuleBuilder;

        /// <summary>
        /// Gets the containing <see cref="ReflectionTypeSymbolBuilder"/>.
        /// </summary>
        protected internal ReflectionTypeSymbolBuilder? ContainingTypeBuilder => _containingTypeBuilder;

    }

}
