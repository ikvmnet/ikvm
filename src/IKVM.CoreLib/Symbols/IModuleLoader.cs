using System;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    interface IModuleLoader
    {

        /// <summary>
        /// Gets whether or not the module is missing.
        /// </summary>
        /// <returns></returns>
        bool GetIsMissing();

        /// <summary>
        /// Gets the assembly of this module.
        /// </summary>
        /// <returns></returns>
        AssemblySymbol GetAssembly();

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Gets the fully qualified name of this module.
        /// </summary>
        /// <returns></returns>
        string GetFullyQualifiedName();

        /// <summary>
        /// Gets the scope name of this module.
        /// </summary>
        /// <returns></returns>
        string GetScopeName();

        /// <summary>
        /// Gets the MVID of this module.
        /// </summary>
        /// <returns></returns>
        Guid GetModuleVersionId();

        /// <summary>
        /// Gets whether this module is a resource module.
        /// </summary>
        /// <returns></returns>
        bool GetIsResource();

        /// <summary>
        /// Gets the fields declared of the module.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<FieldSymbol> GetDeclaredFields();

        /// <summary>
        /// Gets the methods declared on the module.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<MethodSymbol> GetDeclaredMethods();

        /// <summary>
        /// Gets the types declared on the module.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> GetDeclaredTypes();

        /// <summary>
        /// Gets the types declared on the module.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();
    }

}