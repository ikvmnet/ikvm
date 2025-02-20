﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents an assembly, which is a reusable, versionable, and self-describing building block of a common language runtime application.
    /// </summary>
    interface IAssemblySymbol : ISymbol, ICustomAttributeSymbolProvider
    {

        /// <summary>
        /// Gets a collection of the types defined in this assembly.
        /// </summary>
        IEnumerable<ITypeSymbol> DefinedTypes { get; }

        /// <summary>
        /// Gets the entry point of this assembly.
        /// </summary>
        IMethodSymbol? EntryPoint { get; }

        /// <summary>
        /// Gets a collection of the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        IEnumerable<ITypeSymbol> ExportedTypes { get; }

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        string? FullName { get; }

        /// <summary>
        /// Gets the module that contains the manifest for the current assembly.
        /// </summary>
        IModuleSymbol ManifestModule { get; }

        /// <summary>
        /// Gets a collection that contains the modules in this assembly.
        /// </summary>
        IEnumerable<IModuleSymbol> Modules { get; }

        /// <summary>
        /// Gets the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetExportedTypes();

        /// <summary>
        /// Gets the specified module in this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IModuleSymbol? GetModule(string name);

        /// <summary>
        /// Gets all the modules that are part of this assembly.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<IModuleSymbol> GetModules();

        /// <summary>
        /// Gets all the modules that are part of this assembly, specifying whether to include resource modules.
        /// </summary>
        /// <param name="getResourceModules"></param>
        /// <returns></returns>
        ImmutableArray<IModuleSymbol> GetModules(bool getResourceModules);

        /// <summary>
        /// Gets an <see cref="AssemblyName"/> for this assembly.
        /// </summary>
        /// <returns></returns>
        AssemblyName GetName();

        /// <summary>
        /// Gets the <see cref="AssemblyName"/> objects for all the assemblies referenced by this assembly.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<AssemblyName> GetReferencedAssemblies();

        /// <summary>
        /// Gets the <see cref="ITypeSymbol"/> object with the specified name in the assembly instance.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ITypeSymbol? GetType(string name);

        /// <summary>
        /// Gets the <see cref="ITypeSymbol"/> object with the specified name in the assembly instance and optionally throws an exception if the type is not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        ITypeSymbol? GetType(string name, bool throwOnError);

        /// <summary>
        /// Gets all types defined in this assembly.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetTypes();

    }

}