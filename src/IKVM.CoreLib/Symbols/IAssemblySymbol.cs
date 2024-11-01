using System.Collections.Generic;
using System.IO;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents an assembly, which is a reusable, versionable, and self-describing building block of a common language runtime application.
    /// </summary>
    interface IAssemblySymbol : ISymbol, ICustomAttributeProvider
    {

        /// <summary>
        /// Gets a collection of the types defined in this assembly.
        /// </summary>
        IEnumerable<ITypeSymbol> DefinedTypes { get; }

        /// <summary>
        /// Gets a collection of the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        IEnumerable<ITypeSymbol> ExportedTypes { get; }

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        string? FullName { get; }

        /// <summary>
        /// Gets a string representing the version of the common language runtime (CLR) saved in the file containing the manifest.
        /// </summary>
        string ImageRuntimeVersion { get; }

        /// <summary>
        /// Gets the full path or UNC location of the loaded file that contains the manifest.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Gets the module that contains the manifest for the current assembly.
        /// </summary>
        IModuleSymbol ManifestModule { get; }

        /// <summary>
        /// Gets a collection that contains the modules in this assembly.
        /// </summary>
        IEnumerable<IModuleSymbol> Modules { get; }

        /// <summary>
        /// Loads the specified manifest resource from this assembly.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        ManifestResourceInfo? GetManifestResourceInfo(string resourceName);

        /// <summary>
        /// Loads the specified manifest resource from this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Stream? GetManifestResourceStream(string name);

        /// <summary>
        /// Loads the specified manifest resource, scoped by the namespace of the specified type, from this assembly.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Stream? GetManifestResourceStream(ITypeSymbol type, string name);

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
        IModuleSymbol[] GetModules();

        /// <summary>
        /// Gets all the modules that are part of this assembly, specifying whether to include resource modules.
        /// </summary>
        /// <param name="getResourceModules"></param>
        /// <returns></returns>
        IModuleSymbol[] GetModules(bool getResourceModules);

        /// <summary>
        /// Gets an <see cref="AssemblyIdentity"/> for this assembly.
        /// </summary>
        /// <returns></returns>
        AssemblyIdentity GetIdentity();

        /// <summary>
        /// Gets the <see cref="AssemblyIdentity"/> objects for all the assemblies referenced by this assembly.
        /// </summary>
        /// <returns></returns>
        AssemblyIdentity[] GetReferencedAssemblies();

        /// <summary>
        /// Gets the Type object with the specified name in the assembly instance.
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
        /// Gets the <see cref="ITypeSymbol"/> object with the specified name in the assembly instance, with the options of ignoring the case, and of throwing an exception if the type is not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="throwOnError"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase);

        /// <summary>
        /// Gets all types defined in this assembly.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetTypes();

        /// <summary>
        /// Gets the public types defined in this assembly that are visible outside the assembly.
        /// </summary>
        /// <returns></returns>
        ITypeSymbol[] GetExportedTypes();

        /// <summary>
        /// Gets the entry point of this assembly.
        /// </summary>
        IMethodSymbol? EntryPoint { get; }

    }

}