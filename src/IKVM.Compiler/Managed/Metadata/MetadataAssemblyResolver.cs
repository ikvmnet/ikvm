using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Provides the ability to load assemblies using System.Reflection.Metdata.
    /// </summary>
    public class MetadataAssemblyResolver : IManagedAssemblyResolver
    {

        readonly IMetadataReaderLoader loader;
        readonly ConcurrentDictionary<AssemblyName, ValueTask<ManagedAssembly?>> cache = new();

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
        public ValueTask<ManagedAssembly?> ResolveAsync(AssemblyName assemblyName) => cache.GetOrAdd(assemblyName, LoadAsync);

        /// <summary>
        /// Implements the loading of the specified assembly name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ValueTask<ManagedAssembly?> LoadAsync(AssemblyName name)
        {
            var primary = loader.LoadAsync(name);
            if (primary.IsCompleted)
                return new ValueTask<ManagedAssembly?>(primary.Result != null ? new MetadataAssemblyContext(primary.Result, loader).Assembly : null);
            else
                return new ValueTask<ManagedAssembly?>(primary.AsTask().ContinueWith(p => p.Result != null ? new MetadataAssemblyContext(p.Result, loader).Assembly : null));
        }

    }

}
