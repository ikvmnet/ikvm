using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using IKVM.Runtime.Vfs;

namespace IKVM.Runtime
{

    static partial class JVM
    {


        /// <summary>
        /// Property values loaded into the JVV from various sources.
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
                // user value takes priority
                if (User.TryGetValue("ikvm.home", out var homePath1))
                    return homePath1;

                // ikvm properties value comes next
                if (Ikvm.TryGetValue("ikvm.home", out var homePath2))
                    return homePath2;

                // find first occurance of home root
                if (User.TryGetValue("ikvm.home.root", out var homePathRoot) == false)
                    Ikvm.TryGetValue("ikvm.home.root", out homePathRoot);

                // start search for architecture specific directory from a relative path
                if (homePathRoot == null)
                    homePathRoot = "ikvm";

                // calculate ikvm.home from ikvm.home.root
                var ikvmHomeRootPath = Path.Combine(Path.GetDirectoryName(BaseAssembly.Location), homePathRoot);
                if (Directory.Exists(ikvmHomeRootPath))
                {
                    foreach (var ikvmHomeArch in GetIkvmHomeArchNames())
                    {
                        var ikvmHomePath = Path.Combine(ikvmHomeRootPath, ikvmHomeArch);
                        if (Directory.Exists(ikvmHomePath))
                            return ikvmHomePath;
                    }
                }

                // fallback to local 'ikvm' directory next to IKVM.Runtime
                return Path.Combine(Path.GetDirectoryName(BaseAssembly.Location), "ikvm");
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
                p["java.runtime.name"] = Constants.java_runtime_name;
                p["java.runtime.version"] = Constants.java_runtime_version;
                p["java.specification.name"] = "Java Platform API Specification";
                p["java.specification.version"] = Constants.java_specification_version;
                p["java.specification.vendor"] = Constants.java_specification_vendor;
                p["java.class.version"] = "52.0";
                p["java.class.path"] = "";
                p["java.library.path"] = GetLibraryPath();

                try
                {
                    p["java.io.tmpdir"] = Path.GetTempPath();
                }
                catch (SecurityException)
                {
                    p["java.io.tmpdir"] = ".";
                }

                p["java.ext.dirs"] = "";

                GetOSProperties(out var osname, out var osversion);
                p["os.name"] = osname;
                p["os.version"] = osversion;

                var arch = SafeGetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                if (arch == null)
                {
                    // guess based on OS and bit size
                    if (IntPtr.Size == 4)
                        arch = RuntimeUtil.IsWindows ? "x86" : "i386";
                    else
                        arch = "amd64";
                }

                if (arch.Equals("AMD64", StringComparison.OrdinalIgnoreCase))
                    arch = "amd64";

                p["os.arch"] = arch;
                p["sun.arch.data.model"] = (IntPtr.Size * 8).ToString();
                p["file.separator"] = Path.DirectorySeparatorChar.ToString();
                p["file.encoding"] = Encoding.Default.WebName;
                p["path.separator"] = Path.PathSeparator.ToString();
                p["line.separator"] = Environment.NewLine;

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

                GetCultureProperties(out var language, out var country, out var variant, out var script);
                p["user.language"] = language;
                p["user.country"] = country;
                p["user.variant"] = variant;
                p["user.script"] = script;

                // adjust ikvm.home and java.home to match
                p["ikvm.home"] = HomePath;
                p["java.home"] = HomePath;

                // other various properties
                p["openjdk.version"] = Constants.openjdk_version;
                p["java.endorsed.dirs"] = Path.Combine(HomePath, "lib", "endorsed");
                p["sun.boot.library.path"] = Path.Combine(HomePath, "bin");
                p["sun.boot.class.path"] = VfsTable.Default.GetAssemblyClassesPath(BaseAssembly);
                p["file.encoding.pkg"] = "sun.io";
                p["java.vm.info"] = "compiled mode";
                p["java.awt.headless"] = "true";
                p["user.timezone"] = "";
                p["sun.cpu.endian"] = BitConverter.IsLittleEndian ? "little" : "big";
                p["sun.nio.MaxDirectMemorySize"] = "-1";
                p["sun.os.patch.level"] = Environment.OSVersion.ServicePack;

                var stdoutEncoding = IsWindowsConsole(true) ? GetConsoleEncoding() : null;
                if (stdoutEncoding != null)
                    p.Add("sun.stdout.encoding", stdoutEncoding);

                var stderrEncoding = IsWindowsConsole(false) ? GetConsoleEncoding() : null;
                if (stderrEncoding != null)
                    p.Add("sun.stderr.encoding", stderrEncoding);

                if (RuntimeUtil.IsOSX)
                {
                    p.Add("sun.jnu.encoding", "UTF-8");
                }
                else
                {
                    p.Add("sun.jnu.encoding", Encoding.Default.WebName);
                }

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
                    const byte VER_NT_DOMAIN_CONTROLLER = 0x0000002;
                    const byte VER_NT_SERVER = 0x0000003;
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
                    // openjdk calls Foundation libraries here
                }

                osname ??= Environment.OSVersion.ToString();
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
            /// Returns the possible architecture names of the ikvm.home directory to use for this run.
            /// </summary>
            /// <returns></returns>
            static IEnumerable<string> GetIkvmHomeArchNames()
            {
                var arch = RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X86 => "x86",
                    Architecture.X64 => "x64",
                    Architecture.Arm => "arm",
                    Architecture.Arm64 => "arm64",
                    _ => throw new NotSupportedException(),
                };

                if (RuntimeUtil.IsWindows)
                {
                    var v = Environment.OSVersion.Version;

                    // Windows 10
                    if (v.Major > 10 || (v.Major == 10 && v.Minor >= 0))
                        yield return $"win10-{arch}";

                    // Windows 8.1
                    if (v.Major > 6 || (v.Major == 6 && v.Minor >= 3))
                        yield return $"win81-{arch}";

                    // Windows 7
                    if (v.Major > 6 || (v.Major == 6 && v.Minor >= 1))
                        yield return $"win7-{arch}";

                    // fallback
                    yield return $"win-{arch}";
                }

                if (RuntimeUtil.IsLinux)
                {
                    yield return $"linux-{arch}";
                }

                if (RuntimeUtil.IsOSX)
                {
                    yield return $"osx-{arch}";
                }
            }

            /// <summary>
            /// Returns whether 
            /// </summary>
            /// <param name="stdout"></param>
            /// <returns></returns>
            static bool IsWindowsConsole(bool stdout)
            {
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                    return false;
                else
                    return stdout ? !Console.IsOutputRedirected : !Console.IsErrorRedirected;
            }

            /// <summary>
            /// Gets the console encoding.
            /// </summary>
            /// <returns></returns>
            static string GetConsoleEncoding()
            {
                var codepage = Console.InputEncoding.CodePage;
                return codepage is >= 847 and <= 950 ? $"ms{codepage}" : $"cp{codepage}";
            }

        }

    }

}