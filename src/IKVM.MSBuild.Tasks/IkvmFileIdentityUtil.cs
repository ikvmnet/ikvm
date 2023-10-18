namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using Microsoft.Build.Utilities;

    public class IkvmFileIdentityUtil
    {

        const string XML_FILE_ELEMENT_NAME = "File";
        const string XML_PATH_ATTRIBUTE_NAME = "Path";
        const string XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME = "LastWriteTimeUtc";
        const string XML_IDENTITY_ATTRIBUTE_NAME = "Identity";

        readonly static Regex sha1Regex = new(@"^([\w\-]+)", RegexOptions.Compiled);
        readonly static Regex md5Regex = new(@"^([\w\-]+)", RegexOptions.Compiled);

        readonly IkvmAssemblyInfoUtil assemblyInfoUtil;
        readonly Dictionary<string, (DateTime LastWriteTimeUtc, string Identity)> state = new();
        readonly ConcurrentDictionary<string, Task<(DateTime LastWriteTimeUtc, string Identity)>> cache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblyInfoUtil"></param>
        public IkvmFileIdentityUtil(IkvmAssemblyInfoUtil assemblyInfoUtil)
        {
            this.assemblyInfoUtil = assemblyInfoUtil ?? throw new ArgumentNullException(nameof(assemblyInfoUtil));
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

            foreach (var element in root.Elements(XML_FILE_ELEMENT_NAME))
            {
                var path = (string)element.Attribute(XML_PATH_ATTRIBUTE_NAME);
                if (path == null)
                    continue;

                var lastWriteTimeUtc = (DateTime?)element.Attribute(XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME);
                if (lastWriteTimeUtc == null)
                    continue;

                var identity = (string)element.Attribute(XML_IDENTITY_ATTRIBUTE_NAME);
                if (identity == null)
                    continue;

                state[path] = (lastWriteTimeUtc.Value, identity);
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
                var (lastWriteTimeUtc, identity) = await i.Value;
                root.Add(new XElement(XML_FILE_ELEMENT_NAME, new XAttribute(XML_PATH_ATTRIBUTE_NAME, i.Key), new XAttribute(XML_LAST_WRITE_TIME_UTC_ATTRIBUTE_NAME, lastWriteTimeUtc), new XAttribute(XML_IDENTITY_ATTRIBUTE_NAME, identity)));
            }
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetIdentityForFileAsync(string path, TaskLoggingHelper log, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));

            return (await cache.GetOrAdd(path, path => CreateIdentityForFileAsync(path, log, cancellationToken))).Identity;
        }

        /// <summary>
        /// Attempts to read all of the text from a file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        bool TryReadAllText(string path, out string text)
        {
            if (File.Exists(path) == false)
            {
                text = null;
                return false;
            }

            try
            {
                text = File.ReadAllText(path);
                return true;
            }
            catch (FileNotFoundException)
            {
                text = null;
                return false;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the given file path is potentially a .NET assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsPossibleAssemblyPath(string path)
        {
            return Path.GetExtension(path) is ".exe" or ".dll";
        }

        /// <summary>
        /// Calculates the hash of the value.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        byte[] ComputeHash(Stream stream)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(stream);
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(DateTime LastWriteTimeUtc, string Identity)> CreateIdentityForFileAsync(string path, TaskLoggingHelper log, CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    var lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);

                    // check if loaded state contains up to date information
                    if (state.TryGetValue(path, out var entry))
                        if (entry.LastWriteTimeUtc == lastWriteTimeUtc)
                            return (lastWriteTimeUtc, entry.Identity);

                    // file might have a companion SHA1 hash, let's use it, no calculation required
                    if (TryReadAllText(path + ".sha1", out var sha1))
                        return (lastWriteTimeUtc, $"SHA1:{sha1Regex.Match(sha1).Value}");

                    // file might have a companion MD5 hash, let's use it, no calculation required
                    if (TryReadAllText(path + ".md5", out var md5))
                        return (lastWriteTimeUtc, $"MD5:{md5Regex.Match(md5).Value}");

                    // if the file is potentially a .NET assembly
                    if (IsPossibleAssemblyPath(path))
                        if (await assemblyInfoUtil.GetAssemblyInfoAsync(path, log, cancellationToken) is IkvmAssemblyInfoUtil.AssemblyInfo a)
                            return (lastWriteTimeUtc, $"MVID:{a.Mvid}");

                    // fallback to a standard full MD5 of the file
                    using var stm = File.OpenRead(path);
                    var hsh = ComputeHash(stm);
                    var hex = IkvmTaskUtil.ToHex(hsh);

                    return (lastWriteTimeUtc, hex);
                }
                catch (FileNotFoundException)
                {
                    return (default, null);
                }
            });
        }

    }

}
