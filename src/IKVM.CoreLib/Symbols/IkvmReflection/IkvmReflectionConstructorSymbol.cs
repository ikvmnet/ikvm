using System;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionConstructorSymbol : IkvmReflectionMethodBaseSymbol, IIkvmReflectionConstructorSymbol
    {

        readonly ConstructorInfo _ctor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="ctor"></param>
        public IkvmReflectionConstructorSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol resolvingModule, IIkvmReflectionTypeSymbol resolvingType, ConstructorInfo ctor) :
            base(context, resolvingModule, resolvingType)
        {
            _ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
        }

        /// <inheritdoc />
        public ConstructorInfo UnderlyingConstructor => _ctor;

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingConstructor;

    }

}
