using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Provides the ability to load assemblies using System.Reflection.Metdata.
    /// </summary>
    internal class MetadataAssemblyResolver : IManagedAssemblyResolver
    {

        readonly IMetadataReaderLoader loader;
        readonly ConcurrentDictionary<AssemblyName, ManagedAssembly> assemblies = new ConcurrentDictionary<AssemblyName, ManagedAssembly>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="loader"></param>
        public MetadataAssemblyResolver(IMetadataReaderLoader loader)
        {
            this.loader = loader;
        }

        /// <summary>
        /// Resolves a <see cref="ManagedAssembly"/> from the given <see cref="AssemblyName"/>.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public ValueTask<ManagedAssembly?> ResolveAsync(AssemblyName assemblyName)
        {
            return assemblies.GetOrAdd(assemblyName, LoadAsync);
        }

        ValueTask<ManagedAssembly> LoadAsync(AssemblyName name)
        {

        }

    }

}
