using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Jvm
{

    /// <summary>
    /// Describes the JVM type system: the types, fields and methods that class files are linked against, and the
    /// operations on them. The type, member, field and method representations are opaque handles; every operation
    /// goes through the type system.
    /// </summary>
    /// <remarks>
    /// Implementations are intended to be value types. Code that is generic over a <c>TTypeSystem</c> parameter
    /// constrained to <c>struct</c> is compiled separately for each type system, so calls through it are bound
    /// statically and can be inlined, instead of being dispatched through the interface.
    /// </remarks>
    /// <typeparam name="TType">The representation of a type.</typeparam>
    /// <typeparam name="TMember">The common representation of fields and methods.</typeparam>
    /// <typeparam name="TField">The representation of a field.</typeparam>
    /// <typeparam name="TMethod">The representation of a method.</typeparam>
    internal interface IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        /// <summary>
        /// Returns whether this type system belongs to the importer.
        /// </summary>
        /// <remarks>
        /// This is non-optimal, and the required options should be more explicitly specified.
        /// </remarks>
        bool IsImporter { get; }

        /// <summary>
        /// Gets the diagnostics handler for this type system.
        /// </summary>
        IDiagnosticHandler Diagnostics { get; }

        /// <summary>
        /// Gets whether the type system allows non-virtual calls.
        /// </summary>
        bool AllowNonVirtualCalls { get; }

        /// <summary>
        /// Gets the type for 'java.lang.Object'.
        /// </summary>
        TType TypeOfJavaLangObject { get; }

        /// <summary>
        /// Gets the 'null' verifier type.
        /// </summary>
        TType TypeOfVerifierNull { get; }

        /// <summary>
        /// Creates an unloadable type with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TType CreateUnloadableType(string name);

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetName(TType type);

        /// <summary>
        /// Gets whether or not the type is unloadable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsUnloadable(TType type);

        /// <summary>
        /// Gets whether or not the type is an interface.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsInterface(TType type);

        /// <summary>
        /// Gets the modifiers of the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ClassFileAccessFlags GetAccessFlags(TType type);

        /// <summary>
        /// Gets the base type of the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TType? GetBaseType(TType type);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="type"/> is a sub type of <paramref name="baseType"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        bool IsSubTypeOf(TType type, TType baseType);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="type"/> is permitted to access the package of <paramref name="target"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool CheckPackageAccess(TType type, TType target);

        /// <summary>
        /// Gets the field of the type with the given name and signature.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        TField? GetField(TType type, string name, string signature);

        /// <summary>
        /// Gets the method of the type with the given name and signature. Optionally specifies whether inherited methods should be included in the search.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        TMethod? GetMethod(TType type, string name, string signature, bool inherit);

        /// <summary>
        /// Gets the method of the type which implements the interface method with the given name and signature.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        TMethod? GetInterfaceMethod(TType type, string name, string signature);

        /// <summary>
        /// Loads a type by name, using <paramref name="context"/> as the loading context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TType LoadType(TType context, string name, LoadMode mode);

        /// <summary>
        /// Gets the type described by a field signature, using <paramref name="context"/> as the loading context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="signature"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TType GetFieldTypeFromSignature(TType context, string signature, LoadMode mode);

        /// <summary>
        /// Gets the argument types described by a method descriptor, using <paramref name="context"/> as the loading context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="descriptor"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TType[] GetArgTypeListFromSignature(TType context, string descriptor, LoadMode mode);

        /// <summary>
        /// Gets the return type described by a method descriptor, using <paramref name="context"/> as the loading context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="descriptor"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        TType GetReturnTypeFromSignature(TType context, string descriptor, LoadMode mode);

        /// <summary>
        /// Initiates a link of the field with the specified mode.
        /// </summary>
        /// <remarks>
        /// These link methods may not belong on the type system. Unlike the other members, they change the state of a
        /// member rather than describe it, and only constant pool linking calls them.
        /// </remarks>
        /// <param name="field"></param>
        /// <param name="mode"></param>
        void LinkField(TField field, LoadMode mode);

        /// <summary>
        /// Initiates a link of the method with the specified mode.
        /// </summary>
        /// <remarks>
        /// These link methods may not belong on the type system. Unlike the other members, they change the state of a
        /// member rather than describe it, and only constant pool linking calls them.
        /// </remarks>
        /// <param name="method"></param>
        /// <param name="mode"></param>
        void LinkMethod(TMethod method, LoadMode mode);

    }

}
