namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class IkvmFileIdentityUtil
    {

        readonly IkvmAssemblyInfoUtil assemblyInfoUtil;
        readonly ConcurrentDictionary<string, Task<string>> fileIdentityCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmFileIdentityUtil(IkvmAssemblyInfoUtil assemblyInfoUtil)
        {
            this.assemblyInfoUtil = assemblyInfoUtil ?? throw new ArgumentNullException(nameof(assemblyInfoUtil));
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetIdentityForFileAsync(string file, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException($"'{nameof(file)}' cannot be null or whitespace.", nameof(file));
            if (File.Exists(file) == false)
                throw new FileNotFoundException($"Could not find file '{file}'.");

            return fileIdentityCache.GetOrAdd(file, CreateIdentityForFileAsync);
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> CreateIdentityForFileAsync(string file)
        {
            return Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(file))
                    throw new ArgumentException($"'{nameof(file)}' cannot be null or whitespace.", nameof(file));
                if (File.Exists(file) == false)
                    throw new FileNotFoundException($"Could not find file '{file}'.");

                // file might have a companion SHA1 hash, let's use it, no calculation required
                var sha1File = file + ".sha1";
                if (File.Exists(sha1File))
                    if (File.ReadAllText(sha1File) is string h)
                        return $"SHA1:{Regex.Match(h.Trim(), @"^([\w\-]+)").Value}";

                // file might have a companion MD5 hash, let's use it, no calculation required
                var md5File = file + ".md5";
                if (File.Exists(md5File))
                    if (File.ReadAllText(md5File) is string h)
                        return $"MD5:{Regex.Match(h.Trim(), @"^([\w\-]+)").Value}";

                // if the file is potentially a .NET assembly
                if (Path.GetExtension(file) == ".dll" || Path.GetExtension(file) == ".exe")
                    if (await assemblyInfoUtil.GetAssemblyInfoAsync(file) is IkvmAssemblyInfoUtil.AssemblyInfo a)
                        return $"MVID:{a.Mvid}";

                // fallback to a standard full MD5 of the file
                using var stm = File.OpenRead(file);
                var hsh = ComputeHash(stm);
                var bld = new StringBuilder(hsh.Length * 2);
                foreach (var b in hsh)
                    bld.Append(b.ToString("x2"));

                return bld.ToString();
            });
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

    }

}
