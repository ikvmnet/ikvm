using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionConstructorSymbol : IIkvmReflectionMethodBaseSymbol, IConstructorSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorInfo"/>.
        /// </summary>
        ConstructorInfo UnderlyingConstructor { get; }

    }

}
