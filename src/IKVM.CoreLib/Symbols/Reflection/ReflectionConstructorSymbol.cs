using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionConstructorSymbol : ReflectionMethodBaseSymbol, IReflectionConstructorSymbol
    {

        readonly ConstructorInfo _ctor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="ctor"></param>
        public ReflectionConstructorSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol resolvingType, ConstructorInfo ctor) :
            base(context, resolvingModule, resolvingType)
        {
            _ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));
        }

        /// <inheritdoc />
        public virtual ConstructorInfo UnderlyingConstructor => _ctor;

        /// <inheritdoc />
        public virtual ConstructorInfo UnderlyingRuntimeConstructor => UnderlyingConstructor;

        /// <inheritdoc />
        public override MethodBase UnderlyingMethodBase => UnderlyingConstructor;

        /// <inheritdoc />
        public override MethodBase UnderlyingRuntimeMethodBase => UnderlyingRuntimeConstructor;

    }

}
