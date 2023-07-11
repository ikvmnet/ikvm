using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace IKVM.Compiler.Managed.Metadata
{

    /// <summary>
    /// Provides a <see cref="IMetadataReaderResolver"/> capable of resolving assemblies from a set of known file paths.
    /// </summary>
    public class MetadataPathReaderResolver : IMetadataReaderResolver
    {

        readonly IEnumerable<string> paths;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="paths"></param>
        public MetadataPathReaderResolver(IEnumerable<string> paths)
        {
            this.paths = paths;
        }

        /// <summary>
        /// Attempts to load a reader for the given assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public MetadataReader? Resolve(AssemblyName assemblyName)
        {
            foreach (var i in paths)
                if (TryOpen(assemblyName, i, out var pe))
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
        bool TryOpen(AssemblyName assemblyName, string path, out PEReader? reader)
        {
            reader = null;

            var n = assemblyName.Name + ".dll";
            if (Path.GetFileName(path) != n)
                return false;
            if (File.Exists(path) == false)
                return false;

            // open PE
            var pe = new PEReader(File.OpenRead(path));
            if (pe.HasMetadata == false)
            {
                pe.Dispose();
                return false;
            }

            // read metadata, check that matches
            var rd = pe.GetMetadataReader();
            if (rd.GetAssemblyDefinition().GetAssemblyName().Name != assemblyName.Name)
            {
                pe.Dispose();
                return false;
            }

            reader = pe;
            return true;
        }

        /// <summary>
        /// Resolves an additional file from an assembly.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="primaryPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public MetadataReader? Resolve(MetadataReader reader, string primaryPath, AssemblyFile file)
        {
            var d = Path.GetDirectoryName(primaryPath)!;
            if (Directory.Exists(d) == false)
                return null;

            var n = reader.GetString(file.Name);
            if (n == null)
                return null;

            var p = Path.Combine(d, n);
            if (File.Exists(p) == false)
                return null;

            // open file for read and return
            var pe = new PEReader(File.OpenRead(p));
            if (pe.HasMetadata == false)
            {
                pe.Dispose();
                return null;
            }

            return pe.GetMetadataReader();
        }

    }

}
