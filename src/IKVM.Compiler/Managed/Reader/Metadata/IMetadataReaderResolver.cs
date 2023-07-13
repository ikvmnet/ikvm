using System.Reflection.Metadata;

namespace IKVM.Compiler.Managed.Reader.Metadata
{

    /// <summary>
    /// Provides the capability of resolver a <see cref="MetadataReader"/>.
    /// </summary>
    public interface IMetadataReaderResolver
    {

        /// <summary>
        /// Loads the metadata reader for the given assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        MetadataReader? Resolve(string assemblyName);

        /// <summary>
        /// Loads the metadata reader associated with an assembly file of the given primary assembly.
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="primaryPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        MetadataReader? Resolve(MetadataReader primary, string primaryPath, AssemblyFile file);

    }

}