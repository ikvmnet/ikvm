using System.Collections.Generic;
using System.IO;
using System.Reflection;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols
{

    interface IAssemblySymbol : ISymbol, ICustomAttributeProvider
    {

        IEnumerable<ITypeSymbol> DefinedTypes { get; }

        IMethodSymbol? EntryPoint { get; }

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

        ITypeSymbol[] GetExportedTypes();

        IModuleSymbol? GetModule(string name);

        IModuleSymbol[] GetModules();

        IModuleSymbol[] GetModules(bool getResourceModules);

        AssemblyName GetName();

        AssemblyName GetName(bool copiedName);

        AssemblyName[] GetReferencedAssemblies();

        ITypeSymbol? GetType(string name, bool throwOnError);

        ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase);

        ITypeSymbol? GetType(string name);

        ITypeSymbol[] GetTypes();

    }

}