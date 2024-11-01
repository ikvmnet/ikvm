using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionConstructorSymbol : IReflectionMethodBaseSymbol, IConstructorSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/>.
        /// </summary>
        ConstructorInfo UnderlyingConstructor { get; }

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/> used for IL emit operations.
        /// </summary>
        ConstructorInfo UnderlyingRuntimeConstructor { get; }

    }

}
