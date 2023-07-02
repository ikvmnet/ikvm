using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Provides the capability of resolver a <see cref="MetadataReader"/>.
    /// </summary>
    internal interface IMetadataReaderAssemblyFileLoader
    {

        /// <summary>
        /// Loads the metadata reader associated with an assembly file of the given primary assembly.
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="primaryPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        ValueTask<MetadataReader?> LoadAsync(MetadataReader primary, string primaryPath, AssemblyFile file);

    }

}