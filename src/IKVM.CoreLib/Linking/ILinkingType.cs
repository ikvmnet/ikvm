using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Represents a type for linking against.
    /// </summary>
    internal interface ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        /// <summary>
        /// Gets the name of the linkage type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether or not the linking type is unloadable.
        /// </summary>
        bool IsUnloadable { get; }

        /// <summary>
        /// Gets whether or not the linking type is an interface.
        /// </summary>
        bool IsInterface { get; }

        /// <summary>
        /// Gets the modifiers of the linking type.
        /// </summary>
        ClassFileAccessFlags AccessFlags { get; }

        /// <summary>
        /// Gets the base linking type of this type.
        /// </summary>
        TLinkingType? BaseType { get; }

        /// <summary>
        /// Gets the field with the given name and signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        TLinkingField? GetField(string name, string signature);

        /// <summary>
        /// Gets the method with the given name and signature. Optionally specifies whether inherited methods should be included in the search.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        TLinkingMethod? GetMethod(string name, string signature, bool inherit);

        /// <summary>
        /// Gets the method which implements the method with the interface method with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        TLinkingMethod? GetInterfaceMethod(string name, string signature);

        /// <summary>
        /// Returns <c>true</c> if this type is a sub type of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsSubTypeOf(TLinkingType type);

        /// <summary>
        /// Loads another type using this type as the loading context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TLinkingType LoadType(string name, LoadMode mode);

        /// <summary>
        /// Checks whether this type can access the specified type given the specified protection domain.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckPackageAccess(TLinkingType type);

        /// <summary>
        /// Gets the field type for the specified signature given this type as context.
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TLinkingType GetFieldTypeFromSignature(string signature, LoadMode mode);

        /// <summary>
        /// Gets the argument type list from the specified signature using this type as context.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TLinkingType[] GetArgTypeListFromSignature(string descriptor, LoadMode mode);

        /// <summary>
        /// Gets the return type from the specified signature using this type as context.
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TLinkingType GetReturnTypeFromSignature(string descriptor, LoadMode mode);

    }

}
