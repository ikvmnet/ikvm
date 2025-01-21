using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionConstructorSymbol : ReflectionMethodBaseSymbol, IConstructorSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="underlyingConstructor"></param>
        public ReflectionConstructorSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol type, ConstructorInfo underlyingConstructor) :
            base(context, module, type, underlyingConstructor)
        {

        }

        internal ConstructorInfo UnderlyingConstructor => (ConstructorInfo)UnderlyingMethodBase;

    }

}
