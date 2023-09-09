using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using IKVM.Runtime.Vfs;

namespace IKVM.Runtime
{

    static partial class JVM
    {

        /// <summary>
        /// Property values loaded into the JVM from various sources.
        /// </summary>
        public static class Properties
        {

            static readonly IDictionary<string, string> user = new Dictionary<string, string>();
            static readonly Lazy<Dictionary<string, string>> ikvm = new Lazy<Dictionary<string, string>>(GetIkvmProperties);
            static readonly Lazy<Dictionary<string, string>> init = new Lazy<Dictionary<string, string>>(GetInitProperties);
            static readonly Lazy<string> homePath = new Lazy<string>(GetHomePath);

            /// <summary>
            /// Gets the set of properties that are set by the user before initialization.
            /// </summary>
            public static IDictionary<string, string> User => user;

            /// <summary>
            /// Gets the set of properties that are set in the 'ikvm.properties' file before initialization.
            /// </summary>
            internal static IDictionary<string, string> Ikvm => ikvm.Value;

            /// <summary>
            /// Gets the set of properties that are initialized with the JVM and provided to the JDK.
            /// </summary>
            internal static IReadOnlyDictionary<string, string> Init => init.Value;

            /// <summary>
            /// Gets the home path.
            /// </summary>
            internal static string HomePath => homePath.Value;

            /// <summary>
            /// Gets the  set of properties loaded from any companion 'ikvm.properties' file.
            /// </summary>
            /// <returns></returns>
            static Dictionary<string, string> GetIkvmProperties()
            {
                var props = new Dictionary<string, string>();

                // the runtime assembly will set the root of various relative paths
                var runtimePath = Path.GetDirectoryName(typeof(JVM).Assembly.Location);

                try
                {
                    var ikvmPropertiesPath = Path.Combine(runtimePath, "ikvm.properties");
                    if (File.Exists(ikvmPropertiesPath))
                        LoadProperties(File.ReadAllLines(ikvmPropertiesPath), props);
                }
                catch (Exception)
                {

                }

                return props;
            }

            /// <summary>
            /// Gets the home path for IKVM.
            /// </summary>
            /// <returns></returns>
            static string GetHomePath()
            {
                var rootPath = Path.GetDirectoryName(typeof(JVM).Assembly.Location);

#if NETFRAMEWORK
                // attempt to find settings in legacy app.config
                try
                {
                    // specified home directory
                    if (ConfigurationManager.AppSettings["ikvm:ikvm.home"] is string confHome)
                        return Path.GetFullPath(Path.Combine(rootPath, confHome));

                    // specified home root directory
                    if (ConfigurationManager.AppSettings["ikvm:ikvm.home.root"] is string confHomeRoot)
                        if (ResolveHomePathFromRoot(Path.Combine(rootPath, confHomeRoot)) is string confHomePath)
                            return confHomePath;
                }
                catch (ConfigurationException)
                {
                    // app.config is invalid, ignore
                }
#endif

                // user value takes priority
                if (User.TryGetValue("ikvm.home", out var homePath1))
                    return Path.GetFullPath(Path.Combine(rootPath, homePath1));

                // ikvm properties value comes next
                if (Ikvm.TryGetValue("ikvm.home", out var homePath2))
                    return Path.GetFullPath(Path.Combine(rootPath, homePath2));

                // find first occurance of home root
                if (User.TryGetValue("ikvm.home.root", out var homePathRoot) == false)
                    Ikvm.TryGetValue("ikvm.home.root", out homePathRoot);

                // attempt to resolve the path from the given root
                if (ResolveHomePathFromRoot(Path.GetFullPath(Path.Combine(rootPath, homePathRoot ?? "ikvm"))) is string resolvedHomePath)
                    return resolvedHomePath;

                // fallback to local 'ikvm' directory next to IKVM.Runtime
                return Path.GetFullPath(Path.Combine(rootPath, "ikvm"));
            }

            /// <summary>
            /// Scans the given root path for the home for the currently executing runtime.
            /// </summary>
            /// <param name="homePathRoot"></param>
            /// <returns></returns>
            static string ResolveHomePathFromRoot(string homePathRoot)
            {
                // calculate ikvm.home from ikvm.home.root
                if (Directory.Exists(homePathRoot))
                {
                    foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                    {
                        var ikvmHomePath = Path.GetFullPath(Path.Combine(homePathRoot, rid));
                        if (Directory.Exists(ikvmHomePath))
                            return ikvmHomePath;
                    }
                }

                return null;
            }

            /// <summary>
            /// Reads the property lines from the specified file into the dictionary.
            /// </summary>
            /// <param name="lines"></param>
            /// <param name="props"></param>
            static void LoadProperties(IEnumerable<string> lines, IDictionary<string, string> props)
            {
                foreach (var l in lines)
                {
                    var a = l.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (a.Length >= 2)
                        props[a[0].Trim()] = a[1]?.Trim() ?? "";
                }
            }

            /// <summary>
            /// Gets the set of init properties.
            /// </summary>
            /// <returns></returns>
            static Dictionary<string, string> GetInitProperties()
            {
#if FIRST_PASS || IMPORTER || EXPORTER
                throw new NotImplementedException();
#else
                var p = new Dictionary<string, string>();
                p["openjdk.version"] = Constants.openjdk_version;
                p["java.version"] = Constants.java_version;
                p["java.vendor"] = Constants.java_vendor;
                p["java.vendor.url"] = Constants.java_vendor_url;
                p["java.vendor.url.bug"] = Constants.java_vendor_url_bug;
                p["java.vm.name"] = Constants.java_vm_name;
                p["java.vm.version"] = Constants.java_vm_version;
                p["java.vm.vendor"] = Constants.java_vm_vendor;
                p["java.vm.specification.name"] = "Java Virtual Machine Specification";
                p["java.vm.specification.version"] = Constants.java_vm_specification_version;
                p["java.vm.specification.vendor"] = Constants.java_vm_specification_vendor;
                p["java.vm.info"] = "compiled mode";
                p["java.runtime.name"] = Constants.java_runtime_name;
                p["java.runtime.version"] = Constants.java_runtime_version;
                p["java.specification.name"] = "Java Platform API Specification";
                p["java.specification.version"] = Constants.java_specification_version;
                p["java.specification.vendor"] = Constants.java_specification_vendor;
                p["java.class.version"] = "52.0";

                // various directory paths
                p["ikvm.home"] = HomePath;
                p["java.home"] = HomePath;
                p["java.io.tmpdir"] = GetTempPath();
                p["java.library.path"] = GetLibraryPath();
                p["java.ext.dirs"] = Path.Combine(HomePath, "lib", "ext");
                p["java.endorsed.dirs"] = Path.Combine(HomePath, "lib", "endorsed");
                p["sun.boot.library.path"] = GetBootLibraryPath();
                p["sun.boot.class.path"] = VfsTable.Default.GetAssemblyClassesPath(BaseAssembly);

                // various OS information
                GetOSProperties(out var osname, out var osversion);
                p["os.name"] = osname;
                p["os.version"] = osversion;
                p["os.arch"] = GetArch();
                p["sun.os.patch.level"] = Environment.OSVersion.ServicePack;
                p["sun.arch.data.model"] = (IntPtr.Size * 8).ToString();
                p["sun.cpu.endian"] = BitConverter.IsLittleEndian ? "little" : "big";

                // text settings
                p["file.separator"] = Path.DirectorySeparatorChar.ToString();
                p["file.encoding"] = Encoding.Default.WebName;
                p["path.separator"] = Path.PathSeparator.ToString();
                p["line.separator"] = Environment.NewLine;
                p["file.encoding.pkg"] = "sun.io";
                p["sun.jnu.encoding"] = RuntimeUtil.IsOSX ? "UTF-8" : Encoding.Default.WebName;
                p["sun.stdout.encoding"] = RuntimeUtil.IsWindows && !Console.IsOutputRedirected ? GetWindowsConsoleEncoding() : null;
                p["sun.stderr.encoding"] = RuntimeUtil.IsWindows && !Console.IsErrorRedirected ? GetWindowsConsoleEncoding() : null;

                // culture/language properties
                GetCultureProperties(out var language, out var country, out var variant, out var script);
                p["user.language"] = language;
                p["user.country"] = country;
                p["user.variant"] = variant;
                p["user.script"] = script;
                p["user.timezone"] = "";

                try
                {
                    p["user.name"] = Environment.UserName;
                }
                catch (SecurityException)
                {
                    p["user.name"] = "(unknown)";
                }

                var home = SafeGetEnvironmentVariable("USERPROFILE");
                if (home == null)
                {
                    // maybe we're on *nix
                    home = SafeGetEnvironmentVariable("HOME");
                    if (home == null)
                    {
                        try
                        {
                            home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        }
                        catch (SecurityException)
                        {
                            home = ".";
                        }
                    }
                }

                p["user.home"] = home;

                try
                {
                    p["user.dir"] = Environment.CurrentDirectory;
                }
                catch (SecurityException)
                {
                    p["user.dir"] = ".";
                }

                // other various properties
                p["java.awt.headless"] = "true";
                p["sun.nio.MaxDirectMemorySize"] = "-1";

                // cacerts is mounted by the VFS into ikvmHome
                p.Add("javax.net.ssl.trustStore", Path.Combine(HomePath, "lib", "security", "cacerts"));

#if NETFRAMEWORK

                // read properties from app.config
                try
                {
                    foreach (string key in ConfigurationManager.AppSettings)
                        if (key.StartsWith("ikvm:"))
                            p.Add(key.Substring(5), ConfigurationManager.AppSettings[key]);
                }
                catch (ConfigurationException)
                {
                    // app.config is invalid, ignore
                }

#endif

                // set the properties that were specfied
                if (user != null)
                    foreach (var kvp in user)
                        p[kvp.Key] = kvp.Value;

                return p;
#endif
            }

            /// <summary>
            /// Gets the property values based on the culture.
            /// </summary>
            /// <param name="language"></param>
            /// <param name="country"></param>
            /// <param name="variant"></param>
            /// <param name="script"></param>
            static void GetCultureProperties(out string language, out string country, out string variant, out string script)
            {
                var culture = CultureInfo.CurrentCulture.Name.Split('-');
                if (culture.Length == 2)
                {
                    language = culture[0];
                    if (culture[1].Length == 4)
                    {
                        script = culture[1];
                        country = "";
                    }
                    else
                    {
                        script = "";
                        country = culture[1];
                    }
                }
                else if (culture.Length == 3)
                {
                    language = culture[0];
                    script = culture[1];
                    country = culture[2];
                }
                else
                {
                    language = "en";
                    script = "";
                    country = "US";
                }

                // Norwegian
                if (language == "nb")
                {
                    language = "no";
                    country = "NO";
                    variant = "";
                }
                else if (language == "nn")
                {
                    language = "no";
                    country = "NO";
                    variant = "NY";
                }
                else
                {
                    variant = "";
                }
            }

            /// <summary>
            /// Gets the OS name and OS version property values.
            /// </summary>
            /// <param name="osname"></param>
            /// <param name="osversion"></param>
            static void GetOSProperties(out string osname, out string osversion)
            {
                osname = null;
                osversion = null;

                if (RuntimeUtil.IsWindows)
                {
                    const byte VER_NT_WORKSTATION = 0x0000001;

                    var os = Environment.OSVersion;
                    int major = os.Version.Major;
                    int minor = os.Version.Minor;
                    int build = os.Version.Build;

                    switch (os.Platform)
                    {
                        case PlatformID.Win32Windows:
                            if (major == 4)
                            {
                                osname = minor switch
                                {
                                    0 => "Windows 95",
                                    10 => "Windows 98",
                                    90 => "Windows Me",
                                    _ => "Windows 9X (unknown)",
                                };
                            }
                            else
                            {
                                osname = "Windows 9X (unknown)";
                            }
                            break;
                        case PlatformID.Win32NT:
                            var kernel32 = GetKernel32FileVersionInfo();
                            if (kernel32 != null)
                            {
                                major = kernel32.ProductMajorPart;
                                minor = kernel32.ProductMinorPart;
                                build = kernel32.ProductBuildPart;
                            }

                            var productType = GetWindowsProductType();
                            if (productType < 0)
                                // error

                                osname = "Windows NT (unknown)";
                            switch (major)
                            {
                                case 3:
                                case 4:
                                    osname = "Windows NT";
                                    break;
                                case 5:
                                    switch (minor)
                                    {
                                        case 0:
                                            osname = "Windows 2000";
                                            break;
                                        case 1:
                                            osname = "Windows XP";
                                            break;
                                        case 2:
                                            if (productType == VER_NT_WORKSTATION && Environment.Is64BitOperatingSystem)
                                            {
                                                osname = "Windows XP";
                                            }
                                            else
                                            {
                                                osname = "Windows 2003";
                                            }
                                            break;
                                        default:
                                            osname = "Windows NT (unknown)";
                                            break;
                                    }
                                    break;
                                case 6:
                                    if (productType == VER_NT_WORKSTATION)
                                    {
                                        switch (minor)
                                        {
                                            case 0:
                                                osname = "Windows Vista";
                                                break;
                                            case 1:
                                                osname = "Windows 7";
                                                break;
                                            case 2:
                                                osname = "Windows 8";
                                                break;
                                            case 3:
                                                osname = "Windows 8.1";
                                                break;
                                            default:
                                                osname = "Windows NT (unknown)";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (minor)
                                        {
                                            case 0:
                                                osname = "Windows Server 2008";
                                                break;
                                            case 1:
                                                osname = "Windows Server 2008 R2";
                                                break;
                                            case 2:
                                                osname = "Windows Server 2012";
                                                break;
                                            case 3:
                                                osname = "Windows Server 2012 R2";
                                                break;
                                            default:
                                                osname = "Windows NT (unknown)";
                                                break;
                                        }
                                    }
                                    break;
                                case 10:
                                    if (productType == VER_NT_WORKSTATION)
                                    {
                                        switch (minor)
                                        {
                                            case 0:
                                                if (build >= 22000)
                                                    osname = "Windows 11";
                                                else
                                                    osname = "Windows 10";
                                                break;
                                            default:
                                                osname = "Windows NT (unknown)";
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (minor)
                                        {
                                            case 0:
                                                if (build > 20347)
                                                    osname = "Windows Server 2022";
                                                else if (build > 17676)
                                                    osname = "Windows Server 2019";
                                                else
                                                    osname = "Windows Server 2016";
                                                break;
                                            default:
                                                osname = "Windows NT (unknown)";
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    osname = "Windows (unknown)";
                                    break;
                            }
                            break;
                    }

                    osversion = major + "." + minor;
                }
                else if (RuntimeUtil.IsLinux)
                {
                    var sysname = GetLinuxSysnameAndRelease();
                    osname = sysname[0];
                    osversion = sysname[1];
                }
                else if (RuntimeUtil.IsOSX)
                {
                    osname = "Mac OS X";
                    osversion = "10.15";

                    // OpenJDK collects the version from a number of different places
                    // we should do that in the future
                }

                osname ??= Environment.OSVersion.ToString();
            }

            /// <summary>
            /// Gets the platform architecture.
            /// </summary>
            /// <returns></returns>
            static unsafe string GetArch()
            {
#if FIRST_PASS || IMPORTER || EXPORTER
                throw new NotSupportedException();
#else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.X64 => "amd64",
                        Architecture.X86 => "x86",
                        Architecture.Arm => "arm",
                        Architecture.Arm64 => "arm64",
                        _ => "unknown",
                    };
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.X86 => "i386",
                        Architecture.X64 => "amd64",
                        _ => Mono.Unix.Native.Syscall.uname(out var utsname) == 0 ? utsname.machine : "unknown",
                    };
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.X64 => "x86_64",
                        Architecture.Arm64 => "aarch64",
                        _ => "unknown",
                    };
                }

                return "unknown";
#endif
            }

            /// <summary>
            /// Gets the temporary path.
            /// </summary>
            /// <returns></returns>
            static string GetTempPath()
            {
                try
                {
                    return Path.GetTempPath();
                }
                catch (SecurityException)
                {
                    return ".";
                }
            }

            /// <summary>
            /// Gets the path string for loading native libraries.
            /// </summary>
            /// <returns></returns>
            static string GetLibraryPath()
            {
                var libraryPath = new List<string>();

                if (RuntimeUtil.IsWindows)
                {
                    // see /hotspot/src/os/windows/vm/os_windows.cpp for the comment that describes how we build the path
                    var windir = SafeGetEnvironmentVariable("SystemRoot");
                    if (windir != null)
                        libraryPath.Add(Path.Combine(windir, "Sun", "Java", "bin"));

                    try
                    {
                        libraryPath.Add(Environment.SystemDirectory);
                    }
                    catch (SecurityException)
                    {

                    }

                    if (windir != null)
                        libraryPath.Add(windir);

                    var path = SafeGetEnvironmentVariable("PATH");
                    if (path != null)
                        libraryPath.Add(path);
                }

                if (RuntimeUtil.IsLinux)
                {
                    // on Linux we have some hardcoded paths (from /hotspot/src/os/linux/vm/os_linux.cpp)
                    // and we can only guess the cpu arch based on bitness (that means only x86 and x64)
                    libraryPath.Add(Path.Combine("/usr/java/packages/lib/", IntPtr.Size == 4 ? "i386" : "amd64"));
                    libraryPath.Add("/lib");
                    libraryPath.Add("/usr/lib");

                    // prefix with LD_LIBRARY_PATH
                    var ld_library_path = SafeGetEnvironmentVariable("LD_LIBRARY_PATH");
                    if (ld_library_path != null)
                        libraryPath.Insert(0, ld_library_path);
                }

                if (RuntimeUtil.IsOSX)
                {
                    libraryPath.Add(".");
                }

                try
                {
                    libraryPath.Insert(0, Path.GetDirectoryName(BaseAssembly.Location));
                }
                catch (Exception)
                {
                    // ignore
                }

                if (RuntimeUtil.IsWindows)
                    libraryPath.Add(".");

                return string.Join(Path.PathSeparator.ToString(), libraryPath);
            }

            /// <summary>
            /// Gets the boot library paths.
            /// </summary>
            /// <returns></returns>
            static IEnumerable<string> GetBootLibraryPathsIter()
            {
                var self = Directory.GetParent(typeof(JVM).Assembly.Location)?.FullName;
                if (self == null)
                    yield break;

                // implicitly include native libraries along side application (publish)
                yield return self;

                // search in runtime specific directories
                foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                    yield return Path.Combine(self, "runtimes", rid, "native");
            }

            /// <summary>
            /// Gets the boot library paths 
            /// </summary>
            /// <returns></returns>
            static string GetBootLibraryPath()
            {
                return string.Join(Path.PathSeparator.ToString(), GetBootLibraryPathsIter());
            }

            /// <summary>
            /// Gets the version information from the kernel32 file, if present.
            /// </summary>
            /// <returns></returns>
            static FileVersionInfo GetKernel32FileVersionInfo()
            {
                try
                {
                    foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                        if (string.Compare(module.ModuleName, "kernel32.dll", StringComparison.OrdinalIgnoreCase) == 0)
                            return module.FileVersionInfo;
                }
                catch
                {

                }

                return null;
            }

            /// <summary>
            /// VER_NT_* values.
            /// </summary>
            enum VER_NT : byte
            {

                DOMAIN_CONTROLLER = 0x0000002,
                SERVER = 0x0000003,
                WORKSTATION = 0x0000001,

            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            struct OSVERSIONINFOEXW
            {

                public int dwOSVersionInfoSize;
                public int dwMajorVersion;
                public int dwMinorVersion;
                public int dwBuildNumber;
                public int dwPlatformId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string szCSDVersion;
                public ushort wServicePackMajor;
                public ushort wServicePackMinor;
                public ushort wSuiteMask;
                public VER_NT wProductType;
                public byte wReserved;

            }

            [DllImport("ntdll.dll", SetLastError = true)]
            static extern int RtlGetVersion(ref OSVERSIONINFOEXW versionInfo);

            /// <summary>
            /// Gets the Windows ProductType.
            /// </summary>
            /// <returns></returns>
            static byte GetWindowsProductType()
            {
#if FIRST_PASS
                throw new NotSupportedException();
#else
                if (RuntimeUtil.IsWindows == false)
                    throw new Exception("Cannot retrieve a Windows product type for this operating system.");

                var osvi = default(OSVERSIONINFOEXW);
                osvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEXW));
                if (RtlGetVersion(ref osvi) != 0)
                    return 0;

                return (byte)osvi.wProductType;
#endif
            }

            /// <summary>
            /// Gets the 'sysname' on Linux.
            /// </summary>
            /// <returns></returns>
            static string[] GetLinuxSysnameAndRelease()
            {
#if FIRST_PASS
                throw new NotSupportedException();
#else
                if (RuntimeUtil.IsLinux == false)
                    throw new Exception("Cannot retrieve sysname information for this operating system.");

                if (Mono.Unix.Native.Syscall.uname(out var utsname) != 0)
                    return null;

                return new[] { utsname.sysname, utsname.release };
#endif
            }

            /// <summary>
            /// Gets the console encoding.
            /// </summary>
            /// <returns></returns>
            static string GetWindowsConsoleEncoding()
            {
                var codepage = Console.InputEncoding.CodePage;
                return codepage is >= 874 and <= 950 ? $"ms{codepage}" : $"cp{codepage}";
            }

        }

    }

}
