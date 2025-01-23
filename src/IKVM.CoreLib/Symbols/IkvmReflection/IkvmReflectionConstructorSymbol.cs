using System;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionConstructorSymbol : IkvmReflectionMethodBaseSymbol, IConstructorSymbol
    {

        readonly ConstructorInfo _underlyingConstructor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingConstructor"></param>
        public IkvmReflectionConstructorSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol type, ConstructorInfo underlyingConstructor) :
            base(context, module, type, underlyingConstructor)
        {
            _underlyingConstructor = underlyingConstructor ?? throw new ArgumentNullException(nameof(underlyingConstructor));
        }

        internal ConstructorInfo UnderlyingConstructor => _underlyingConstructor;

    }

}