using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Provides a <see cref="IMetadataReaderLoader"/> capable of resolving assemblies from a directory structure.
    /// </summary>
    internal class MetadataAssemblyPathLoader : IMetadataReaderLoader
    {

        readonly IEnumerable<string> paths;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="paths"></param>
        public MetadataAssemblyPathLoader(IEnumerable<string> paths)
        {
            this.paths = paths;
        }

        /// <summary>
        /// Attempts to load a reader for the given assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public async ValueTask<MetadataReader?> LoadAsync(AssemblyName assemblyName)
        {
            foreach (var i in paths)
                if (await TryOpenAsync(assemblyName, i, out var pe))
                    return pe!.GetMetadataReader();

            return null;
        }

        /// <summary>
        /// Attempts to load the 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="path"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        ValueTask<bool> TryOpenAsync(AssemblyName assemblyName, string path, out PEReader? reader)
        {
            reader = null;

            var d = Path.GetDirectoryName(path)!;
            if (Directory.Exists(d) == false)
                return new ValueTask<bool>(false);

            var n = assemblyName.Name + ".dll";
            if (n == null)
                return new ValueTask<bool>(false);

            var p = Path.Combine(d, n);
            if (File.Exists(p) == false)
                return new ValueTask<bool>(false);

            // open file for read and return
            reader = new PEReader(File.OpenRead(p));
            return new ValueTask<bool>(true);
        }

        /// <summary>
        /// Loads the associated <see cref="AssemblyFile"/> of the primary assembly.
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="primaryPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        ValueTask<MetadataReader?> IMetadataReaderAssemblyFileLoader.LoadAsync(MetadataReader primary, string primaryPath, AssemblyFile file)
        {
            var d = Path.GetDirectoryName(primaryPath)!;
            if (Directory.Exists(d) == false)
                return new ValueTask<MetadataReader?>((MetadataReader?)null);

            var n = primary.GetString(file.Name);
            if (n == null)
                return new ValueTask<MetadataReader?>((MetadataReader?)null);

            var p = Path.Combine(d, n);
            if (File.Exists(p) == false)
                return new ValueTask<MetadataReader?>((MetadataReader?)null);
            
            // open file for read and return
            var pe = new PEReader(File.OpenRead(p));
            return new ValueTask<MetadataReader?>(pe.GetMetadataReader());
        }

    }

}
