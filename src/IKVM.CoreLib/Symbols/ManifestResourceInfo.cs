using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides access to manifest resources, which are XML files that describe application dependencies.
    /// </summary>
    readonly struct ManifestResourceInfo(ResourceLocation ResourceLocation, string? FileName, AssemblySymbol? ReferencedAssembly)
    {

        /// <summary>
        /// Gets the manifest resource's location.
        /// </summary>
        public readonly ResourceLocation ResourceLocation = ResourceLocation;

        /// <summary>
        /// Gets the name of the file that contains the manifest resource, if it is not the same as the manifest file.
        /// </summary>
        public readonly string? FileName = FileName;

        /// <summary>
        /// Gets the containing assembly for the manifest resource.
        /// </summary>
        public readonly AssemblySymbol? ReferencedAssembly = ReferencedAssembly;

    }

}
