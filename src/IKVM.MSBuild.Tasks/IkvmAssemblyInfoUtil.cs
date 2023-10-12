namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Threading.Tasks;

    public class IkvmAssemblyInfoUtil
    {

        /// <summary>
        /// Defines the cached information per assembly.
        /// </summary>
        public struct AssemblyInfo
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="mvid"></param>
            /// <param name="references"></param>
            public AssemblyInfo(string name, Guid mvid, IList<string> references)
            {
                Name = name;
                Mvid = mvid;
                References = references ?? throw new ArgumentNullException(nameof(references));
            }

            /// <summary>
            /// Name of the assembly.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets the MVID of the assembly.
            /// </summary>
            public Guid Mvid { get; set; }

            /// <summary>
            /// Names of the references of the assembly.
            /// </summary>
            public IList<string> References { get; set; }

        }

        readonly ConcurrentDictionary<string, Task<AssemblyInfo?>> assemblyInfoCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmAssemblyInfoUtil()
        {

        }

        /// <summary>
        /// Gets the assembly info for the given assembly path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task<AssemblyInfo?> GetAssemblyInfoAsync(string path)
        {
            return assemblyInfoCache.GetOrAdd(path, ReadAssemblyInfoAsync);
        }

        /// <summary>
        /// Reads the assembly info from the given assembly path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<AssemblyInfo?> ReadAssemblyInfoAsync(string path)
        {
            return Task.Run<AssemblyInfo?>(() =>
            {
                try
                {
                    using var fsstm = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using var perdr = new PEReader(fsstm);
                    var mrdr = perdr.GetMetadataReader();
                    return new AssemblyInfo(mrdr.GetString(mrdr.GetAssemblyDefinition().Name), mrdr.GetGuid(mrdr.GetModuleDefinition().Mvid), mrdr.AssemblyReferences.Select(i => mrdr.GetString(mrdr.GetAssemblyReference(i).Name)).ToList());
                }
                catch
                {
                    return null;
                }
            });
        }

    }

}
