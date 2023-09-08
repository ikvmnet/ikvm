using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace IKVM.Runtime
{

    /// <summary>
    /// Maintains various information about the current runtime environment.
    /// </summary>
    public static class RuntimeUtil
    {

        static JsonElement? runtimeJsonElement;
        static string runtimeIdentifier;

        /// <summary>
        /// Returns <c>true</c> if the current runtime is Mono.
        /// </summary>
        public static bool IsMono { get; } = Type.GetType("Mono.Runtime") != null;

        /// <summary>
        /// Returns <c>true</c> if the current platform is Windows.
        /// </summary>
        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Returns <c>true</c> if the current platform is Linux.
        /// </summary>
        public static bool IsLinux { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Returns <c>true</c> if the current platform is Mac OS X.
        /// </summary>
        public static bool IsOSX { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Gets the current runtime identifier.
        /// </summary>
        public static string RuntimeIdentifier => runtimeIdentifier ??= GetRuntimeIdentifier();

        /// <summary>
        /// Discovers the current RID.
        /// </summary>
        /// <returns></returns>
        static string GetRuntimeIdentifier()
        {
            var rid = GetRuntimeIdentifierImpl();
            if (rid != null && RuntimeJson.TryGetProperty(rid, out _))
                return rid;

            return null;
        }

        /// <summary>
        /// Discovers the current RID.
        /// </summary>
        /// <returns></returns>
        static string GetRuntimeIdentifierImpl()
        {
            // allow overrides with environment
            if (Environment.GetEnvironmentVariable("DOTNET_RUNTIME_ID") is string env && string.IsNullOrWhiteSpace(env) == false)
                return env;

#if NETCOREAPP
            // TODO this is available under RuntimeInformation in .NET 5
            if (AppContext.GetData("RUNTIME_IDENTIFIER") is string rid && string.IsNullOrWhiteSpace(rid) == false)
                return rid;
#endif

            var arch = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "x86",
                Architecture.X64 => "x64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => null,
            };

            if (IsWindows)
            {
                var v = Environment.OSVersion.Version;

                // Windows 10
                if (v.Major > 10 || (v.Major == 10 && v.Minor >= 0))
                    return $"win10-{arch}";

                // Windows 8.1
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 3))
                    return $"win81-{arch}";

                // Windows 8
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 2))
                    return $"win8-{arch}";

                // Windows 7
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 1))
                    return $"win7-{arch}";

                // fallback
                return $"win-{arch}";
            }

            if (IsLinux)
            {
                GetLinuxDistroInfo(out var id, out var versionId);

                // no version avaiable, plain distro
                if (id != null && versionId == null && RuntimeJson.TryGetProperty($"{id}-{arch}", out _))
                    return $"{id}-{arch}";

                // all available
                if (id != null && versionId != null && RuntimeJson.TryGetProperty($"{id}.{versionId}-{arch}", out _))
                    return $"{id}.{versionId}-{arch}";

                // fall back to plain linux
                return $"linux-{arch}";
            }

            if (IsOSX)
            {
                var v = GetDarwinVersion();

                // no version information, fallback
                if (string.IsNullOrEmpty(v))
                    return $"osx-{arch}";

                // version available
                return $"osx.{v}-{arch}";
            }

            return null;
        }

        /// <summary>
        /// Gets the 'runtime.json' file.
        /// </summary>
        static JsonElement RuntimeJson => runtimeJsonElement ??= GetRuntimeJson();

        /// <summary>
        /// Loads the 'runtime.json' file.
        /// </summary>
        /// <returns></returns>
        static JsonElement GetRuntimeJson()
        {
            using var stream = typeof(RuntimeUtil).Assembly.GetManifestResourceStream("runtime.json");
            var g = JsonDocument.Parse(stream);
            return g.RootElement.GetProperty("runtimes");
        }

        /// <summary>
        /// Gets the supported RIDs, returned in most specific order.
        /// </summary>
        public static IEnumerable<string> SupportedRuntimeIdentifiers => GetSupportedRuntimeIdentifiers();

        /// <summary>
        /// Discovers the supported RIDs, returned in most specific order.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetSupportedRuntimeIdentifiers()
        {
            return GetRuntimeIdentifierIterator(RuntimeIdentifier).Distinct();
        }

        /// <summary>
        /// Recurses into the runtimes graph and attempts to resolve the RID and all RIDs it imports.
        /// </summary>
        /// <param name="runtimes"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        static IEnumerable<string> GetRuntimeIdentifierIterator(string rid)
        {
            if (RuntimeJson.TryGetProperty(rid, out var node) == false)
                yield break;

            // initiate a breadth first search across runtime.json from specified RID
            var v = new HashSet<string>();
            var q = new Queue<(string, JsonElement)>();
            v.Add(rid);
            q.Enqueue((rid, node));

            // continue until end is reached
            while (q.Count > 0)
            {
                // dequeue next item and yield
                var (thisRid, thisNode) = q.Dequeue();
                yield return thisRid;

                // enqueue referenced rids
                if (thisNode.TryGetProperty("#import", out var import))
                    foreach (var imports in import.EnumerateArray())
                        if (imports.GetString() is string nextRid && string.IsNullOrWhiteSpace(nextRid) == false)
                            if (v.Add(nextRid))
                                if (RuntimeJson.TryGetProperty(nextRid, out var nextNode))
                                    q.Enqueue((nextRid, nextNode));
            }
        }

        /// <summary>
        /// Much of this logic was taken from the old PlatformAbstractions package.
        /// </summary>
        /// <returns></returns>
        static void GetLinuxDistroInfo(out string id, out string versionId)
        {
            id = null;
            versionId = null;

            // Sample os-release file:
            //   NAME="Ubuntu"
            //   VERSION = "14.04.3 LTS, Trusty Tahr"
            //   ID = ubuntu
            //   ID_LIKE = debian
            //   PRETTY_NAME = "Ubuntu 14.04.3 LTS"
            //   VERSION_ID = "14.04"
            //   HOME_URL = "http://www.ubuntu.com/"
            //   SUPPORT_URL = "http://help.ubuntu.com/"
            //   BUG_REPORT_URL = "http://bugs.launchpad.net/ubuntu/"
            // We use ID and VERSION_ID

            if (File.Exists("/etc/os-release"))
            {
                var lines = File.ReadAllLines("/etc/os-release");
                foreach (var line in lines)
                {
                    if (line.StartsWith("ID=", StringComparison.Ordinal))
                        id = line.Substring(3).Trim('"', '\'');
                    else if (line.StartsWith("VERSION_ID=", StringComparison.Ordinal))
                        versionId = line.Substring(11).Trim('"', '\'');
                }
            }
            else if (File.Exists("/etc/redhat-release"))
            {
                var lines = File.ReadAllLines("/etc/redhat-release");
                if (lines.Length >= 1)
                {
                    var line = lines[0];
                    if (line.StartsWith("Red Hat Enterprise Linux Server release 6.") || line.StartsWith("CentOS release 6."))
                    {
                        id = "rhel";
                        versionId = "6";
                    }
                }
            }

            NormalizeDistroInfo(ref id, ref versionId);
        }

        /// <summary>
        /// For some distros, we don't want to use the full version from VERSION_ID. One example is
        /// Red Hat Enterprise Linux, which includes a minor version in their VERSION_ID but minor
        /// versions are backwards compatable.
        ///
        /// In this case, we'll normalized RIDs like 'rhel.7.2' and 'rhel.7.3' to a generic
        /// 'rhel.7'. This brings RHEL in line with other distros like CentOS or Debian which
        /// don't put minor version numbers in their VERSION_ID fields because all minor versions
        /// are backwards compatible.
        /// </summary>
        /// <returns></returns>
        static void NormalizeDistroInfo(ref string id, ref string versionId)
        {
            // handle if VersionId is null by just setting the index to -1.
            int lastVersionNumberSeparatorIndex = versionId?.IndexOf('.') ?? -1;
            if (lastVersionNumberSeparatorIndex != -1 && id == "alpine")
            {
                // for Alpine, the version reported has three components, so we need to find the second version separator
                lastVersionNumberSeparatorIndex = versionId.IndexOf('.', lastVersionNumberSeparatorIndex + 1);
            }

            if (lastVersionNumberSeparatorIndex != -1 && (id == "rhel" || id == "alpine"))
                versionId = versionId.Substring(0, lastVersionNumberSeparatorIndex);
        }

        /// <summary>
        /// Gets the version information for Darwin.
        /// </summary>
        /// <returns></returns>
        static string GetDarwinVersion()
        {
            Version version;
            var kernelRelease = GetDarwinKernelRelease();
            if (!Version.TryParse(kernelRelease, out version) || version.Major < 5)
            {
                // 10.0 covers all versions prior to Darwin 5
                // Similarly, if the version is not a valid version number, but we have still detected that it is Darwin, we just assume
                // it is OS X 10.0
                return "10.0";
            }
            else
            {
                // Mac OS X 10.1 mapped to Darwin 5.x, and the mapping continues that way
                // So just subtract 4 from the Darwin version.
                // https://en.wikipedia.org/wiki/Darwin_%28operating_system%29
                return $"10.{version.Major - 4}";
            }
        }

        /// <summary>
        /// Gets the kernel release information for Darwin.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        unsafe static string GetDarwinKernelRelease()
        {
            const int CTL_KERN = 1;
            const int KERN_OSRELEASE = 2;

            const uint BUFFER_LENGTH = 32;

            var name = stackalloc int[2];
            name[0] = CTL_KERN;
            name[1] = KERN_OSRELEASE;

            var buf = stackalloc byte[(int)BUFFER_LENGTH];
            var len = stackalloc uint[1];
            *len = BUFFER_LENGTH;

            try
            {
                // If the buffer isn't big enough, it seems sysctl still returns 0 and just sets len to the
                // necessary buffer size. This appears to be contrary to the man page, but it's easy to detect
                // by simply checking len against the buffer length.
                if (sysctl(name, 2, buf, len, IntPtr.Zero, 0) == 0 && *len < BUFFER_LENGTH)
                {
                    return Marshal.PtrToStringAnsi((IntPtr)buf, (int)*len);
                }
            }
            catch (Exception ex)
            {
                throw new PlatformNotSupportedException("Error reading Darwin Kernel Version.", ex);
            }

            throw new PlatformNotSupportedException("Unknown error reading Darwin Kernel Version.");
        }

        /// <summary>
        /// Invokes the native 'sysctl' method from 'libc'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="namelen"></param>
        /// <param name="oldp"></param>
        /// <param name="oldlenp"></param>
        /// <param name="newp"></param>
        /// <param name="newlen"></param>
        /// <returns></returns>
        [DllImport("libc")]
        unsafe static extern int sysctl(int* name, uint namelen, byte* oldp, uint* oldlenp, IntPtr newp, uint newlen);

    }

}
