using System.Collections.Generic;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides custom attributes for reflection objects that support them.
    /// </summary>
    public interface ICustomAttributeProvider
    {

        /// <summary>
        /// Returns an array of all of the custom attributes defined on this member, excluding named attributes, or an empty array if there are no custom attributes.
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        IEnumerable<CustomAttribute> GetCustomAttributes(bool inherit = false);

        /// <summary>
        /// Returns an array of custom attributes defined on this member, identified by type, or an empty array if there are no custom attributes of that type.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        IEnumerable<CustomAttribute> GetCustomAttributes(TypeSymbol attributeType, bool inherit = false);

        /// <summary>
        /// Retrieves a custom attribute of a specified type that is applied to a specified member.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false);

        /// <summary>
        /// Indicates whether one or more instance of <paramref name="attributeType" /> is defined on this member.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        bool IsDefined(TypeSymbol attributeType, bool inherit = false);

    }

}
