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
        ConstructorInfo UnderlyingEmitConstructor { get; }

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/> used for IL emit operations against dynamic methods.
        /// </summary>
        ConstructorInfo UnderlyingDynamicEmitConstructor { get; }

    }

}
