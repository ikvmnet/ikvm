using System.Collections.Immutable;
using System.IO;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of information for a <see cref="DefinitionAssemblySymbol"/>.
    /// </summary>
    interface IAssemblyLoader
    {

        /// <summary>
        /// Gets whether or not the assembly is missing.
        /// </summary>
        bool GetIsMissing();

        /// <summary>
        /// Gets the identity of the assembly.
        /// </summary>
        /// <returns></returns>
        AssemblyIdentity GetIdentity();

        /// <summary>
        /// Gets the image runtime version.
        /// </summary>
        /// <returns></returns>
        string GetImageRuntimeVersion();

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <returns></returns>
        string GetLocation();

        /// <summary>
        /// Gets the entry point.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? GetEntryPoint();

        /// <summary>
        /// Gets the manifest module of this assembly.
        /// </summary>
        /// <returns></returns>
        ModuleSymbol GetManifestModule();

        /// <summary>
        /// Gets the modules declared of this assembly.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ModuleSymbol> GetModules();

        /// <summary>
        /// Gets the information about a specific manifest from this assembly.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        ManifestResourceInfo? GetManifestResourceInfo(string resourceName);

        /// <summary>
        /// Gets a stream to read a manifest resource from this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Stream? GetManifestResourceStream(string name);

        /// <summary>
        /// Gets the referenced assemblies of this assembly.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<AssemblyIdentity> GetReferencedAssemblies();

        /// <summary>
        /// Loads the custom attributes of the assembly.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();

    }

}
