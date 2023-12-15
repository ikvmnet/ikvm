namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class IkvmAssemblyInfoUtil
    {

        const string XML_ASSEMBLY_ELEMENT_NAME = "Assembly";
        const string XML_PATH_ATTRIBUTE_NAME = "Path";
        const string XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME = "LastWriteTimeUtc";
        const string XML_NAME_ATTRIBUTE_NAME = "Name";
        const string XML_MVID_ATTRIBUTE_NAME = "Mvid";
        const string XML_REFERENCE_ELEMENT_NAME = "Reference";

        /// <summary>
        /// Defines the cached information per assembly.
        /// </summary>
        public struct AssemblyInfo
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="path"></param>
            /// <param name="name"></param>
            /// <param name="mvid"></param>
            /// <param name="references"></param>
            public AssemblyInfo(string path, DateTime lastWriteTimeUtc, string name, Guid mvid, string[] references)
            {
                Path = path;
                LastWriteTimeUtc = lastWriteTimeUtc;
                Name = name;
                Mvid = mvid;
                References = references ?? throw new ArgumentNullException(nameof(references));
            }

            /// <summary>
            /// Path to the assembly from which this information was derived.
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// Gets the last write time of the assembly.
            /// </summary>
            public DateTime LastWriteTimeUtc { get; set; }

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
            public string[] References { get; set; }

        }

        readonly Dictionary<string, AssemblyInfo> state = new();
        readonly ConcurrentDictionary<string, Task<AssemblyInfo?>> cache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmAssemblyInfoUtil()
        {

        }

        /// <summary>
        /// Loads a previously saved XML element representing the stored state.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public void LoadStateXml(XElement root)
        {
            if (root is null)
                throw new ArgumentNullException(nameof(root));

            foreach (var element in root.Elements(XML_ASSEMBLY_ELEMENT_NAME))
            {
                var path = (string)element.Attribute(XML_PATH_ATTRIBUTE_NAME);
                if (path is null)
                    continue;

                var lastWriteTimeUtc = (DateTime?)element.Attribute(XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME);
                if (lastWriteTimeUtc is null)
                    continue;

                var name = (string)element.Attribute(XML_NAME_ATTRIBUTE_NAME);
                if (name is null)
                    continue;

                var mvid = (Guid?)element.Attribute(XML_MVID_ATTRIBUTE_NAME);
                if (mvid is null)
                    continue;

                var references = element.Elements(XML_REFERENCE_ELEMENT_NAME).Select(i => i.Value).ToArray();
                if (references is null)
                    continue;

                state[path] = new AssemblyInfo(path, lastWriteTimeUtc.Value, name, mvid.Value, references);
            }
        }

        /// <summary>
        /// Saves a new XML element representing the stored state.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SaveStateXmlAsync(XElement root)
        {
            foreach (var i in cache)
            {
                var info = await i.Value;
                if (info is null)
                    continue;

                root.Add(
                    new XElement(XML_ASSEMBLY_ELEMENT_NAME,
                        new XAttribute(XML_PATH_ATTRIBUTE_NAME, i.Key),
                        new XAttribute(XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME, info.Value.LastWriteTimeUtc),
                        new XAttribute(XML_NAME_ATTRIBUTE_NAME, info.Value.Name),
                        new XAttribute(XML_MVID_ATTRIBUTE_NAME, info.Value.Mvid),
                        info.Value.References.Select(i => new XElement(XML_REFERENCE_ELEMENT_NAME, i))));
            }
        }

        /// <summary>
        /// Gets the assembly info for the given assembly path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AssemblyInfo?> GetAssemblyInfoAsync(string path, TaskLoggingHelper log, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));

            return await cache.GetOrAdd(path, path => CreateAssemblyInfoAsync(path, log, cancellationToken));
        }

        /// <summary>
        /// Reads the assembly info from the given assembly path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        Task<AssemblyInfo?> CreateAssemblyInfoAsync(string path, TaskLoggingHelper log, CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.Run<AssemblyInfo?>(() =>
            {
                try
                {
                    var lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);

                    // check if loaded state contains up to date information
                    if (state.TryGetValue(path, out var entry))
                        if (entry.LastWriteTimeUtc == lastWriteTimeUtc)
                            return entry;

                    try
                    {
                        log?.LogMessage(MessageImportance.Low, "Loading assembly info from '{0}'.", path);
                        using var fsstm = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                        using var perdr = new PEReader(fsstm);
                        var mrdr = perdr.GetMetadataReader();
                        foreach (var h in mrdr.TypeDefinitions)
                        {
                            var t = mrdr.GetTypeDefinition(h);
                            var ns = mrdr.GetString(t.Namespace) ?? "";
                            var tn = mrdr.GetString(t.Name) ?? "";
                            var fn = new StringBuilder(ns.Length + tn.Length + 1);
                            if (t.IsNested == false)
                            {
                                if (ns != null)
                                    fn.Append(ns).Append('.');

                                fn.Append(tn);
                                t.getDe
                            }
                        }
                        return new AssemblyInfo(path, lastWriteTimeUtc, mrdr.GetString(mrdr.GetAssemblyDefinition().Name), mrdr.GetGuid(mrdr.GetModuleDefinition().Mvid), mrdr.AssemblyReferences.Select(i => mrdr.GetString(mrdr.GetAssemblyReference(i).Name)).OrderBy(i => i).ToArray());
                    }
                    catch (Exception e)
                    {
                        log?.LogWarning("Exception loading assembly info from '{0}': {1}", path, e.Message);
                        return null;
                    }
                }
                catch (Exception e)
                {
                    log?.LogWarning("Exception loading assembly info from '{0}': {1}", path, e.Message);
                    return null;
                }
            });
        }

    }

}
