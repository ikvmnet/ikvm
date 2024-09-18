namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides custom attributes for reflection objects that support them.
    /// </summary>
    interface ICustomAttributeSymbolProvider
    {

        /// <summary>
        /// Returns an array of all of the custom attributes defined on this member, excluding named attributes, or an empty array if there are no custom attributes.
        /// </summary>
        /// <returns></returns>
        CustomAttributeSymbol[] GetCustomAttributes();

        /// <summary>
        /// Returns an array of custom attributes defined on this member, identified by type, or an empty array if there are no custom attributes of that type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType);

        /// <summary>
        /// Indicates whether one or more instance of <paramref name="attributeType" /> is defined on this member.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        bool IsDefined(ITypeSymbol attributeType);

    }

}
