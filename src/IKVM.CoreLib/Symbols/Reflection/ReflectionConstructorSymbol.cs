using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionConstructorSymbol : ReflectionMethodBaseSymbol, IConstructorSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="ctor"></param>
        public ReflectionConstructorSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, ConstructorInfo ctor) :
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
