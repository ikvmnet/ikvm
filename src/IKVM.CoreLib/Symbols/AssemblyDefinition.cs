using System.Collections.Immutable;
using System.IO;

namespace IKVM.CoreLib.Symbols
{

    abstract class AssemblyDefinition
    {

        /// <summary>
        /// Gets the image runtime version.
        /// </summary>
        /// <returns></returns>
        public abstract string GetImageRuntimeVersion();

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <returns></returns>
        public abstract string GetLocation();

        /// <summary>
        /// Gets the entry point.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetEntryPoint();

        /// <summary>
        /// Gets the manifest module of this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract ModuleSymbol GetManifestModule();

        /// <summary>
        /// Gets the modules declared of this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<ModuleSymbol> GetModules();

        /// <summary>
        /// Gets the information about a specific manifest from this assembly.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public abstract ManifestResourceInfo? GetManifestResourceInfo(string resourceName);

        /// <summary>
        /// Gets a stream to read a manifest resource from this assembly.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract Stream? GetManifestResourceStream(string name);

        /// <summary>
        /// Gets the referenced assemblies of this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<AssemblyIdentity> GetReferencedAssemblies();

        /// <summary>
        /// Gets the custom attributes of this assembly.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<CustomAttribute> GetCustomAttributes();

    }

}
