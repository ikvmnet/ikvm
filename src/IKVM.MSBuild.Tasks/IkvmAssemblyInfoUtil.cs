namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class IkvmAssemblyInfoUtil
    {

        const string XML_ASSEMBLY_ELEMENT_NAME = "Assembly";
        const string XML_PATH_ATTRIBUTE_NAME = "Path";
        const string XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME = "LastWriteTimeUtc";
        const string XML_ASSEMBLY_INFO_ELEMENT_NAME = "AssemblyInfo";

        /// <summary>
        /// Defines the cached information per assembly.
        /// </summary>
        [XmlRoot(XML_ASSEMBLY_INFO_ELEMENT_NAME)]
        public struct AssemblyInfo
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="mvid"></param>
            /// <param name="references"></param>
            public AssemblyInfo(string name, Guid mvid, List<string> references)
            {
                Name = name;
                Mvid = mvid;
                References = references ?? throw new ArgumentNullException(nameof(references));
            }

            /// <summary>
            /// Name of the assembly.
            /// </summary>
            [XmlAttribute("Name")]
            public string Name { get; set; }

            /// <summary>
            /// Gets the MVID of the assembly.
            /// </summary>
            [XmlAttribute("Mvid")]
            public Guid Mvid { get; set; }

            /// <summary>
            /// Names of the references of the assembly.
            /// </summary>
            [XmlElement("Reference")]
            public List<string> References { get; set; }

        }

        readonly static XmlSerializer assemblyInfoSerializer = new XmlSerializer(typeof(AssemblyInfo));

        readonly Dictionary<string, (DateTime LastWriteTimeUtc, AssemblyInfo? Info)> state = new();
        readonly ConcurrentDictionary<string, Task<(DateTime LastWriteTimeUtc, AssemblyInfo? Info)>> cache = new();

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
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            foreach (var element in root.Elements(XML_ASSEMBLY_ELEMENT_NAME))
            {
                var path = (string)element.Attribute(XML_PATH_ATTRIBUTE_NAME);
                if (path == null)
                    continue;

                var lastWriteTimeUtc = (DateTime?)element.Attribute(XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME);
                if (lastWriteTimeUtc == null)
                    continue;

                var assemblyInfoXml = new XDocument(element.Element(XML_ASSEMBLY_INFO_ELEMENT_NAME));
                if (assemblyInfoXml == null)
                    continue;

                var assemblyInfo = (AssemblyInfo?)assemblyInfoSerializer.Deserialize(assemblyInfoXml.CreateReader());
                if (assemblyInfo == null)
                    continue;

                state[path] = (lastWriteTimeUtc.Value, assemblyInfo);
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
                var (lastWriteTimeUtc, info) = await i.Value;

                // serialize assembly info structure
                var infoXmlDoc = new XDocument();
                using (var infoXmlWrt = infoXmlDoc.CreateWriter())
                    assemblyInfoSerializer.Serialize(infoXmlWrt, info);

                root.Add(new XElement(XML_ASSEMBLY_ELEMENT_NAME, new XAttribute(XML_PATH_ATTRIBUTE_NAME, i.Key), new XAttribute(XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME, lastWriteTimeUtc), infoXmlDoc.Root));
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

            return (await cache.GetOrAdd(path, path => CreateAssemblyInfoAsync(path, log, cancellationToken))).Info;
        }

        /// <summary>
        /// Reads the assembly info from the given assembly path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        Task<(DateTime LastWriteTimeUtc, AssemblyInfo? Info)> CreateAssemblyInfoAsync(string path, TaskLoggingHelper log, CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    var lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);

                    // check if loaded state contains up to date information
                    if (state.TryGetValue(path, out var entry))
                        if (entry.LastWriteTimeUtc == lastWriteTimeUtc)
                            return (lastWriteTimeUtc, entry.Info);

                    try
                    {
                        log.LogMessage(MessageImportance.Low, "Loading assembly info from '{0}'.", path);
                        using var fsstm = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                        using var perdr = new PEReader(fsstm);
                        var mrdr = perdr.GetMetadataReader();
                        return (lastWriteTimeUtc, new AssemblyInfo(mrdr.GetString(mrdr.GetAssemblyDefinition().Name), mrdr.GetGuid(mrdr.GetModuleDefinition().Mvid), mrdr.AssemblyReferences.Select(i => mrdr.GetString(mrdr.GetAssemblyReference(i).Name)).ToList()));
                    }
                    catch (Exception e)
                    {
                        log.LogWarning("Exception loading assembly info from '{0}': {1}", path, e.Message);
                        return (lastWriteTimeUtc, null);
                    }
                }
                catch (Exception e)
                {
                    log.LogWarning("Exception loading assembly info from '{0}': {1}", path, e.Message);
                    return (default, null);
                }
            });
        }

    }

}
