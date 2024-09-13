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
        /// <param name="ctor"></param>
        public ReflectionConstructorSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol type, ConstructorInfo ctor) :
            base(context, module, type, ctor)
        {

        }

        internal new ConstructorInfo ReflectionObject => (ConstructorInfo)base.ReflectionObject;

    }

}
