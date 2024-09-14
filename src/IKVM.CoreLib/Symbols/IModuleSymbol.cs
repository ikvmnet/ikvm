using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Performs reflection on a module.
    /// </summary>
    interface IModuleSymbol : ISymbol, ICustomAttributeSymbolProvider
    {

        /// <summary>
        /// Gets the appropriate <see cref="IAssemblySymbol"> for this instance of Module.
        /// </summary>
        IAssemblySymbol Assembly { get; }

        /// <summary>
        /// Gets a string representing the fully qualified name and path to this module.
        /// </summary>
        string FullyQualifiedName { get; }

        /// <summary>
        /// Gets a token that identifies the module in metadata.
        /// </summary>
        int MetadataToken { get; }

        /// <summary>
        /// Gets a universally unique identifier (UUID) that can be used to distinguish between two versions of a module.
        /// </summary>
        Guid ModuleVersionId { get; }

        /// <summary>
        /// Gets a <see cref="string"/> representing the name of the module with the path removed.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a string representing the name of the module.
        /// </summary>
        string ScopeName { get; }

        /// <summary>
        /// Gets a value indicating whether the object is a resource.
        /// </summary>
        /// <returns></returns>
        bool IsResource();

        /// <summary>
        /// Returns a field having the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IFieldSymbol? GetField(string name);

        /// <summary>
        /// Returns a field having the specified name and binding attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        IFieldSymbol? GetField(string name, BindingFlags bindingAttr);

        /// <summary>
        /// Returns the global fields defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        IFieldSymbol[] GetFields(BindingFlags bindingFlags);

        /// <summary>
        /// Returns the global fields defined on the module.
        /// </summary>
        /// <returns></returns>
        IFieldSymbol[] GetFields();

        /// <summary>
        /// Returns a method having the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IMethodSymbol? GetMethod(string name);

        /// <summary>
        /// Returns a method having the specified name and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        IMethodSymbol? GetMethod(string name, ITypeSymbol[] types);

        /// <summary>
        /// Returns a method having the specified name, binding information, calling convention, and parameter types and modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="callConvention"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers);

        /// <summary>
        /// Returns the global methods defined on the module.
        /// </summary>
        /// <returns></returns>
        IMethodSymbol[] GetMethods();

        /// <summary>
        /// Returns the global methods defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        IMethodSymbol[] GetMethods(BindingFlags bindingFlags);

        /// <summary>
        /// Returns the specified type, performing a case-sensitive search.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        ITypeSymbol? GetType(string className);

        /// <summary>
        /// Returns the specified type, searching the module with the specified case sensitivity.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        ITypeSymbol? GetType(string className, bool ignoreCase);

        /// <summary>
        /// Returns the specified type, specifying whether to make a case-sensitive search of the module and whether to throw an exception if the type cannot be found.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="throwOnError"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Returns all the types defined within this module.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetTypes();

        /// <summary>
        /// Returns the type identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        ITypeSymbol ResolveType(int metadataToken);

        /// <summary>
        /// Returns the type identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <param name="genericTypeArguments"></param>
        /// <param name="genericMethodArguments"></param>
        /// <returns></returns>
        ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

        /// <summary>
        /// Returns the field identified by a metadata token.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        IFieldSymbol? ResolveField(int metadataToken);

        /// <summary>
        /// Returns the field identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <param name="genericTypeArguments"></param>
        /// <param name="genericMethodArguments"></param>
        /// <returns></returns>
        IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

        /// <summary>
        /// Returns the type or member identified by a metadata token.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        IMemberSymbol? ResolveMember(int metadataToken);

        /// <summary>
        /// Returns the type or member identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <param name="genericTypeArguments"></param>
        /// <param name="genericMethodArguments"></param>
        /// <returns></returns>
        IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        IMethodBaseSymbol? ResolveMethod(int metadataToken);

        /// <summary>
        /// Returns the method or constructor identified by the specified metadata token, in the context defined by the specified generic type parameters.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <param name="genericTypeArguments"></param>
        /// <param name="genericMethodArguments"></param>
        /// <returns></returns>
        IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments);

        /// <summary>
        /// Returns the signature blob identified by a metadata token.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        byte[] ResolveSignature(int metadataToken);

        /// <summary>
        /// Returns the string identified by the specified metadata token.
        /// </summary>
        /// <param name="metadataToken"></param>
        /// <returns></returns>
        string ResolveString(int metadataToken);

    }

}
