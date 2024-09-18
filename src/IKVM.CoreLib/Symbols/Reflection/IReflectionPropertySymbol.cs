using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionPropertySymbol : IReflectionMemberSymbol, IPropertySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/>.
        /// </summary>
        PropertyInfo UnderlyingProperty { get; }

    }

}
