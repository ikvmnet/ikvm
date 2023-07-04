using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Loader capable of resolving metadata readers.
    /// </summary>
    public interface IMetadataReaderLoader : IMetadataReaderAssemblyFileLoader
    {

        /// <summary>
        /// Loads a <see cref="MetadataReader"/> for the specified assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        ValueTask<MetadataReader?> LoadAsync(AssemblyName assemblyName);

    }

}
