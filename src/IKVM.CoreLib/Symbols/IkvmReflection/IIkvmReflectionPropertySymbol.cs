using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionPropertySymbol : IIkvmReflectionMemberSymbol, IPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/>.
        /// </summary>
        PropertyInfo UnderlyingProperty { get; }

    }

}
