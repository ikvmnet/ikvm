using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionConstructorSymbol : IReflectionMethodBaseSymbol, IConstructorSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/>.
        /// </summary>
        ConstructorInfo UnderlyingConstructor { get; }

    }

}
