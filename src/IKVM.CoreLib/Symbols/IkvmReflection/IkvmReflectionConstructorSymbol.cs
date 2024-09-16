using ConstructorInfo = IKVM.Reflection.ConstructorInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionConstructorSymbol : IkvmReflectionMethodBaseSymbol, IConstructorSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        public IkvmReflectionConstructorSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, ConstructorInfo ctor) :
            base(context, type, ctor)
        {

        }

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/> wrapped by this symbol.
        /// </summary>
        internal new ConstructorInfo ReflectionObject => (ConstructorInfo)base.ReflectionObject;

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="type"></param>
        internal void Complete(ConstructorInfo type)
        {
            base.Complete(type);
        }

    }

}