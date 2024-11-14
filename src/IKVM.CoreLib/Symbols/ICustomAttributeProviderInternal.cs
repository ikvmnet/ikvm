using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Internal implementation support for <see cref="ICustomAttributeProvider"/>.
    /// </summary>
    interface ICustomAttributeProviderInternal : ICustomAttributeProvider
    {

        /// <summary>
        /// Gets the custom attributes on this provider.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes();

        /// <summary>
        /// Gets the further provider that may declare inherited custom attributes.
        /// </summary>
        /// <returns></returns>
        ICustomAttributeProviderInternal? GetInheritedCustomAttributeProvider();

    }

}
