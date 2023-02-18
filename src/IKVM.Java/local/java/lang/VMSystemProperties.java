package java.lang;

import java.lang.PropertyConstants;
import java.io.StringReader;
import java.util.Properties;

import ikvm.io.InputStreamWrapper;

import cli.System.Environment;
import cli.System.Diagnostics.FileVersionInfo;
import cli.System.IO.Path;
import cli.System.Runtime.InteropServices.RuntimeInformation;
import cli.System.Runtime.InteropServices.OSPlatform;

import static ikvm.internal.Util.SafeGetEnvironmentVariable;

final class VMSystemProperties
{

    private static final byte VER_NT_DOMAIN_CONTROLLER = 0x0000002;
    private static final byte VER_NT_SERVER = 0x0000003;
    private static final byte VER_NT_WORKSTATION = 0x0000001;

    private static final String SPEC_TITLE = "Java Platform API Specification";
    private static final String SPEC_VERSION = PropertyConstants.java_specification_version;
    private static final String SPEC_VENDOR = PropertyConstants.java_specification_vendor;

    private static String getLibraryPath()
    {
        String libraryPath = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.get_Windows())) {
	        // see /hotspot/src/os/windows/vm/os_windows.cpp for the comment that describes how we build the path
	        String windir = SafeGetEnvironmentVariable("SystemRoot");
	        if (windir != null) {
		        libraryPath = Path.PathSeparator + windir + "\\Sun\\Java\\bin";
	        }

            try {
                if (false) throw new cli.System.Security.SecurityException();
                if (libraryPath == null) {
                    libraryPath = Environment.get_SystemDirectory();
                } else {
                    libraryPath += Path.PathSeparator + Environment.get_SystemDirectory();
                }
            } catch (cli.System.Security.SecurityException _) {

            }

            if (windir != null) {
                libraryPath += Path.PathSeparator + windir;
            }

            String path = SafeGetEnvironmentVariable("PATH");
            if (path != null) {
                libraryPath += Path.PathSeparator + path;
            }
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.get_Linux())) {
            // on Linux we have some hardcoded paths (from /hotspot/src/os/linux/vm/os_linux.cpp)
            // and we can only guess the cpu arch based on bitness (that means only x86 and x64)
            String cpu_arch = cli.System.IntPtr.get_Size() == 4 ? "i386" : "amd64";
            libraryPath = "/usr/java/packages/lib/" + cpu_arch + ":/lib:/usr/lib";
            String ld_library_path = SafeGetEnvironmentVariable("LD_LIBRARY_PATH");
            if (ld_library_path != null) {
                libraryPath = ld_library_path + ":" + libraryPath;
            }
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.get_OSX())) {
            libraryPath = ".";
        }

        try {
            libraryPath = Path.GetDirectoryName(getRuntimeAssembly().get_Location()) + Path.PathSeparator + libraryPath;
        } catch (Throwable _) {
            // ignore
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.get_Windows())) {
            libraryPath += Path.PathSeparator + ".";
        }

        return libraryPath;
    }

    public static void initProperties(Properties p) {
        initCommonProperties(p);
        initI18NProperties(p);

        // the runtime assembly will set the root of various relative paths
        String runtimePath = Path.GetDirectoryName(getRuntimeAssembly().get_Location());

        try {
            String ikvmPropertiesPath = Path.Combine(runtimePath, "ikvm.properties");
            if (cli.System.IO.File.Exists(ikvmPropertiesPath)) {
                try (StringReader ikvmPropertiesReader = new StringReader(cli.System.IO.File.ReadAllText(ikvmPropertiesPath))) {
                    p.load(ikvmPropertiesReader);
                }
            }
        } catch (Exception _) { 

        }

        // lookup existing ikvm.home property
        String ikvmHome = p.getProperty("ikvm.home");

        // calculate ikvm.home from ikvm.home.root
        if (ikvmHome == null) {
            String ikvmHomeRoot = p.getProperty("ikvm.home.root");
            if (ikvmHomeRoot != null) {
                String ikvmHomeRootPath = Path.Combine(runtimePath, ikvmHomeRoot);
                if (cli.System.IO.Directory.Exists(ikvmHomeRootPath)) {
                    String[] ikvmHomeArchs = getIkvmHomeArchs();
                    if (ikvmHomeArchs != null) {
                        for (String ikvmHomeArch : ikvmHomeArchs) {
                            String ikvmHomePath = Path.Combine(ikvmHomeRootPath, ikvmHomeArch);
                            if (cli.System.IO.Directory.Exists(ikvmHomePath)) {
                                ikvmHome = ikvmHomePath;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // default value for ikvm.home
        if (ikvmHome == null) {
            ikvmHome = "ikvm";
        }
        
        try {
            // ensure ikvm.home is absolute
            if (Path.IsPathRooted(ikvmHome) == false) {
                ikvmHome = Path.GetFullPath(Path.Combine(runtimePath, ikvmHome));
            }

            // adjust ikvm.home and java.home to match
            p.setProperty("ikvm.home", ikvmHome);
            p.setProperty("java.home", ikvmHome);
        } catch (Exception _) {

        }
        
        p.setProperty("openjdk.version", PropertyConstants.openjdk_version);
        p.setProperty("java.endorsed.dirs", Path.Combine(ikvmHome, "lib", "endorsed"));
        p.setProperty("sun.boot.library.path", Path.Combine(ikvmHome, "bin"));
        p.setProperty("sun.boot.class.path", getBootClassPath());
        p.setProperty("file.encoding.pkg", "sun.io");
        p.setProperty("java.vm.info", "compiled mode");
        p.setProperty("java.awt.headless", "true");
        p.setProperty("user.timezone", "");
        p.setProperty("sun.cpu.endian", cli.System.BitConverter.IsLittleEndian ? "little" : "big");
        p.setProperty("sun.nio.MaxDirectMemorySize", "-1");
        p.setProperty("sun.os.patch.level", cli.System.Environment.get_OSVersion().get_ServicePack());

        String stdoutEncoding = getStdoutEncoding();
        if (stdoutEncoding != null) {
            p.setProperty("sun.stdout.encoding", stdoutEncoding);
        }

        String stderrEncoding = getStderrEncoding();
        if (stderrEncoding != null) {
            p.setProperty("sun.stderr.encoding", stderrEncoding);
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.get_OSX())) {
            p.setProperty("sun.jnu.encoding", "UTF-8");
        } else {
            p.setProperty("sun.jnu.encoding", cli.System.Text.Encoding.get_Default().get_WebName());
        }
        
        // cacerts is mounted by the VFS into ikvmHome
        p.setProperty("javax.net.ssl.trustStore", Path.Combine(ikvmHome, "lib", "security", "cacerts"));
        
        // TODO
        // sun.cpu.isalist:=pentium_pro+mmx pentium_pro pentium+mmx pentium i486 i386 i86
        // sun.desktop:=windows
        // sun.io.unicode.encoding:=UnicodeLittle
        // sun.management.compiler:=HotSpot Client Compiler
        
        // read properties from app.config
        try {
            if (false) throw new cli.System.Configuration.ConfigurationException();
            cli.System.Collections.Specialized.NameValueCollection appSettings = cli.System.Configuration.ConfigurationSettings.get_AppSettings();
            cli.System.Collections.IEnumerator keys = appSettings.GetEnumerator();
            while (keys.MoveNext()) {
                String key = (String)keys.get_Current();
                if (key.startsWith("ikvm:")) {
                    p.setProperty(key.substring(5), appSettings.get_Item(key));
                }
            }
        } catch (cli.System.Configuration.ConfigurationException _) {
            // app.config is invalid, ignore
        }

        // set the properties that were specified with IKVM.Runtime.Launcher.SetProperties()
        cli.System.Collections.IDictionary importProperties = get_ImportProperties();
        if (importProperties != null) {
            cli.System.Collections.IDictionaryEnumerator entries = importProperties.GetEnumerator();
            while (entries.MoveNext()) {
                p.setProperty((String)entries.get_Key(), (String)entries.get_Value());
            }
        }
    }

    private static void initCommonProperties(Properties p)
    {
        p.setProperty("java.version", PropertyConstants.java_version);
        p.setProperty("java.vendor", PropertyConstants.java_vendor);
        p.setProperty("java.vendor.url", PropertyConstants.java_vendor_url);
        p.setProperty("java.vendor.url.bug", PropertyConstants.java_vendor_url_bug);
        p.setProperty("java.vm.name", PropertyConstants.java_vm_name);
        p.setProperty("java.vm.version", PropertyConstants.java_vm_version);
        p.setProperty("java.vm.vendor", PropertyConstants.java_vm_vendor);
        p.setProperty("java.vm.specification.name",  "Java Virtual Machine Specification");
        p.setProperty("java.vm.specification.version", PropertyConstants.java_vm_specification_version);
        p.setProperty("java.vm.specification.vendor", PropertyConstants.java_vm_specification_vendor);
        p.setProperty("java.runtime.name", PropertyConstants.java_runtime_name);
        p.setProperty("java.runtime.version", PropertyConstants.java_runtime_version);
        p.setProperty("java.specification.name", "Java Platform API Specification");
        p.setProperty("java.specification.version", PropertyConstants.java_specification_version);
        p.setProperty("java.specification.vendor", PropertyConstants.java_specification_vendor);
        p.setProperty("java.class.version", "52.0");
        p.setProperty("java.class.path", "");
        p.setProperty("java.library.path", getLibraryPath());

        try {
            if (false) throw new cli.System.Security.SecurityException();
            p.setProperty("java.io.tmpdir", Path.GetTempPath());
        } catch (cli.System.Security.SecurityException _) {
            p.setProperty("java.io.tmpdir", ".");
        }
        p.setProperty("java.ext.dirs", "");

        // NOTE os.name *must* contain "Windows" when running on Windows, because Classpath tests on that
        String osname = null;
        String osversion = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.get_Windows())) {
            cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
            int major = os.get_Version().get_Major();
            int minor = os.get_Version().get_Minor();
            int build = os.get_Version().get_Build();

            switch (os.get_Platform().Value) {
                case cli.System.PlatformID.Win32Windows:
                    if (major == 4) {
                        switch (minor) {
                            case 0:
                                osname = "Windows 95";
                                break;
                            case 10:
                                osname = "Windows 98";
                                break;
                            case 90:
                                osname = "Windows Me";
                                break;
                            default:
                                osname = "Windows 9X (unknown)";
                        }
                    } else {
                        osname = "Windows 9X (unknown)";
                    }
                    break;
                case cli.System.PlatformID.Win32NT:
                    FileVersionInfo kernel32 = getKernel32FileVersionInfo();
                    if (kernel32 != null) {
                        major = kernel32.get_ProductMajorPart();
                        minor = kernel32.get_ProductMinorPart();
                        build = kernel32.get_ProductBuildPart();
                    }

                    byte productType = getWindowsProductType();
                    if (productType < 0) {
                        // error
                    }

                    osname = "Windows NT (unknown)";
                    switch (major) {
                        case 3:
                        case 4:
                            osname = "Windows NT";
                            break;
                        case 5:
                            switch (minor) {
                                case 0:
                                    osname = "Windows 2000";
                                    break;
                                case 1:
                                    osname = "Windows XP";
                                    break;
                                case 2:
                                    if (productType == VER_NT_WORKSTATION && Environment.get_Is64BitOperatingSystem()) {
                                        osname = "Windows XP";
                                    } else {
                                        osname = "Windows 2003";
                                    }
                                    break;
                                default:
                                    osname = "Windows NT (unknown)";
                                    break;
                            }
                            break;
                        case 6:
                            if (productType == VER_NT_WORKSTATION) {
                                switch (minor) {
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
                            } else {
                                switch (minor) {
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
                            if (productType == VER_NT_WORKSTATION) {
                                switch (minor) {
                                    case 0:
                                        if (build >= 22000) {
                                            osname = "Windows 11";
                                        } else {
                                            osname = "Windows 10";
                                        }
                                        break;
                                    default:
                                        osname = "Windows NT (unknown)";
                                        break;
                                }
                            } else {
                                switch (minor) {
                                    case 0:
                                        if (build > 20347) {
                                            osname = "Windows Server 2022";
                                        } else if (build > 17676) {
                                            osname = "Windows Server 2019";
                                        } else {
                                            osname = "Windows Server 2016";
                                        }
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
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.get_Linux())) {
            String[] sysname = getLinuxSysnameAndRelease();
            osname = sysname[0];
            osversion = sysname[1];
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.get_OSX())) {
            // openjdk calls Foundation libraries here
        }

        if (osname == null) {
            osname = cli.System.Environment.get_OSVersion().ToString();
        }

        p.setProperty("os.name", osname);
        p.setProperty("os.version", osversion);

        String arch = SafeGetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
        if (arch == null)
        {
	        // we don't know, so we make a guess
	        if (cli.System.IntPtr.get_Size() == 4) {
		        arch = RuntimeInformation.IsOSPlatform(OSPlatform.get_Windows()) ? "x86" : "i386";
	        } else {
		        arch = "amd64";
	        }
        }

        if (arch.equals("AMD64")) {
            arch = "amd64";
        }

        p.setProperty("os.arch", arch);
        p.setProperty("sun.arch.data.model", String.valueOf(cli.System.IntPtr.get_Size() * 8));
        p.setProperty("file.separator", cli.System.Char.ToString(Path.DirectorySeparatorChar));
        p.setProperty("file.encoding", cli.System.Text.Encoding.get_Default().get_WebName());
        p.setProperty("path.separator", String.valueOf(Path.PathSeparator));
        p.setProperty("line.separator", cli.System.Environment.get_NewLine());

        try {
            if (false) throw new cli.System.Security.SecurityException();
            p.setProperty("user.name", cli.System.Environment.get_UserName());
        } catch(cli.System.Security.SecurityException _) {
            p.setProperty("user.name", "(unknown)");
        }

        String home = SafeGetEnvironmentVariable("USERPROFILE");
        if (home == null) {
            // maybe we're on *nix
            home = SafeGetEnvironmentVariable("HOME");
            if (home == null)
            {
                // TODO maybe there is a better way
                // NOTE on MS .NET this doesn't return the correct path
                // (it returns "C:\\Documents and Settings\\username\\My Documents", but we really need
                // "C:\\Documents and Settings\\username" to be compatible with Sun, that's why we use %USERPROFILE% if it exists)
                try {
                    if (false) throw new cli.System.Security.SecurityException();
                    home = cli.System.Environment.GetFolderPath(cli.System.Environment.SpecialFolder.wrap(cli.System.Environment.SpecialFolder.Personal));
                } catch (cli.System.Security.SecurityException _) {
                    home = ".";
                }
            }
        }
        p.setProperty("user.home", home);

        try {
            if (false) throw new cli.System.Security.SecurityException();
            p.setProperty("user.dir", cli.System.Environment.get_CurrentDirectory());
        } catch (cli.System.Security.SecurityException _) {
            p.setProperty("user.dir", ".");
        }
    }

    private static void initI18NProperties(Properties p) {
        String[] culture = ((cli.System.String)(Object)cli.System.Globalization.CultureInfo.get_CurrentCulture().get_Name()).Split(new char[] { '-' });
        String language;
        String script;
        String region;
        String variant;

        if (culture.length == 2) {
            language = culture[0];
            if (culture[1].length() == 4) {
                script = culture[1];
                region = "";
            } else {
                script = "";
                region = culture[1];
            }
        } else if (culture.length == 3) {
            language = culture[0];
            script = culture[1];
            region = culture[2];
        } else {
            language = "en";
            script = "";
            region = "US";
        }

        // Norwegian
        if (language.equals("nb"))
        {
            language = "no";
            region = "NO";
            variant = "";
        } else if (language.equals("nn")) {
            language = "no";
            region = "NO";
            variant = "NY";
        } else {
            variant = "";
        }

        p.setProperty("user.language", language);
        p.setProperty("user.country", region);
        p.setProperty("user.variant", variant);
        p.setProperty("user.script", script);
    }
    
    private static native cli.System.Collections.IDictionary get_ImportProperties();
    private static native cli.System.Reflection.Assembly getRuntimeAssembly();
    private static native String[] getIkvmHomeArchs();
    private static native String getBootClassPath();
    private static native String getStdoutEncoding();
    private static native String getStderrEncoding();
    private static native FileVersionInfo getKernel32FileVersionInfo();
    private static native byte getWindowsProductType();
    private static native String[] getLinuxSysnameAndRelease();

    private VMSystemProperties() { }

}
