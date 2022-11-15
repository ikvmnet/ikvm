/*
  Copyright (C) 2004-2015 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
package java.lang;

import java.lang.PropertyConstants;
import java.io.StringReader;
import java.util.Properties;

import ikvm.io.InputStreamWrapper;

import cli.System.Diagnostics.FileVersionInfo;

import static ikvm.internal.Util.SafeGetEnvironmentVariable;

final class VMSystemProperties
{

    private VMSystemProperties() { }

    public static final String SPEC_TITLE = "Java Platform API Specification";
    public static final String SPEC_VERSION = PropertyConstants.java_specification_version;
    public static final String SPEC_VENDOR = PropertyConstants.java_specification_vendor;

    private static String getLibraryPath()
    {
        String libraryPath;
        if (ikvm.internal.Util.WINDOWS)
        {
	        // see /hotspot/src/os/windows/vm/os_windows.cpp for the comment that describes how we build the path
	        String windir = SafeGetEnvironmentVariable("SystemRoot");
	        if (windir != null) {
		        libraryPath = cli.System.IO.Path.PathSeparator + windir + "\\Sun\\Java\\bin";
	        } else {
	            libraryPath = null;
	        }

            try {
                if (false) throw new cli.System.Security.SecurityException();
                if (libraryPath == null) {
                    libraryPath = cli.System.Environment.get_SystemDirectory();
                } else {
                    libraryPath += cli.System.IO.Path.PathSeparator + cli.System.Environment.get_SystemDirectory();
                }
            } catch (cli.System.Security.SecurityException _) {

            }

            if (windir != null) {
                libraryPath += cli.System.IO.Path.PathSeparator + windir;
            }

            String path = SafeGetEnvironmentVariable("PATH");
            if (path != null) {
                libraryPath += cli.System.IO.Path.PathSeparator + path;
            }
        } else if (ikvm.internal.Util.MACOSX) {
            libraryPath = ".";
        } else {
            // assume Linux, since that's the only other platform we support
            // on Linux we have some hardcoded paths (from /hotspot/src/os/linux/vm/os_linux.cpp)
            // and we can only guess the cpu arch based on bitness (that means only x86 and x64)
            String cpu_arch = cli.System.IntPtr.get_Size() == 4 ? "i386" : "amd64";
            libraryPath = "/usr/java/packages/lib/" + cpu_arch + ":/lib:/usr/lib";
            String ld_library_path = SafeGetEnvironmentVariable("LD_LIBRARY_PATH");
            if (ld_library_path != null) {
                libraryPath = ld_library_path + ":" + libraryPath;
            }
        }

        try {
            libraryPath = cli.System.IO.Path.GetDirectoryName(getRuntimeAssembly().get_Location()) + cli.System.IO.Path.PathSeparator + libraryPath;
        }
        catch (Throwable _)
        {
            // ignore
        }

        if (ikvm.internal.Util.WINDOWS) {
            libraryPath += cli.System.IO.Path.PathSeparator + ".";
        }

        return libraryPath;
    }

    public static void initProperties(Properties p) {
        initCommonProperties(p);
        initI18NProperties(p);

        // the runtime assembly will set the root of various relative paths
        String runtimePath = cli.System.IO.Path.GetDirectoryName(getRuntimeAssembly().get_Location());

        try {
            String ikvmPropertiesPath = cli.System.IO.Path.Combine(runtimePath, "ikvm.properties");
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
                String ikvmHomeRootPath = cli.System.IO.Path.Combine(runtimePath, ikvmHomeRoot);
                if (cli.System.IO.Directory.Exists(ikvmHomeRootPath)) {
                    String[] ikvmHomeArchs = getIkvmHomeArchs();
                    if (ikvmHomeArchs != null) {
                        for (String ikvmHomeArch : ikvmHomeArchs) {
                            String ikvmHomePath = cli.System.IO.Path.Combine(ikvmHomeRootPath, ikvmHomeArch);
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
            if (cli.System.IO.Path.IsPathRooted(ikvmHome) == false) {
                ikvmHome = cli.System.IO.Path.GetFullPath(cli.System.IO.Path.Combine(runtimePath, ikvmHome));
            }

            // adjust ikvm.home and java.home to match
            p.setProperty("ikvm.home", ikvmHome);
            p.setProperty("java.home", ikvmHome);
        } catch (Exception _) {

        }
        
        p.setProperty("openjdk.version", PropertyConstants.openjdk_version);
        p.setProperty("java.endorsed.dirs", cli.System.IO.Path.Combine(ikvmHome, "lib", "endorsed"));
        p.setProperty("sun.boot.library.path", cli.System.IO.Path.Combine(ikvmHome, "bin"));
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

        if (ikvm.internal.Util.MACOSX) {
            p.setProperty("sun.jnu.encoding", "UTF-8");
        } else {
            p.setProperty("sun.jnu.encoding", cli.System.Text.Encoding.get_Default().get_WebName());
        }
        
        // serve cacerts out of the VFS by default
        String vfsroot = getVirtualFileSystemRoot();
        p.setProperty("javax.net.ssl.trustStore", cli.System.IO.Path.Combine(vfsroot, "cacerts"));
        
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
            p.setProperty("java.io.tmpdir", cli.System.IO.Path.GetTempPath());
        } catch (cli.System.Security.SecurityException _) {
            p.setProperty("java.io.tmpdir", ".");
        }
        p.setProperty("java.ext.dirs", "");

        // NOTE os.name *must* contain "Windows" when running on Windows, because Classpath tests on that
        String osname = null;
        String osver = null;
        cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
        int major = os.get_Version().get_Major();
        int minor = os.get_Version().get_Minor();
        switch (os.get_Platform().Value)
        {
            case cli.System.PlatformID.Win32NT:
                // Windows lies about the version, so we extract the real version from kernel32.dll
                FileVersionInfo kernel32 = getKernel32FileVersionInfo();
                if (kernel32 != null) {
                    major = kernel32.get_ProductMajorPart();
                    minor = kernel32.get_ProductMinorPart();
                }
                osname = "Windows NT (unknown)";
                switch (major) {
                    case 3:
                    case 4:
                        osver = major + "." + minor;
                        osname = "Windows NT";
                        break;
                    case 5:
                        switch (minor)
                        {
                            case 0:
                                osver = "5.0";
                                osname = "Windows 2000";
                                break;
                            case 1:
                                osver = "5.1";
                                osname = "Windows XP";
                                break;
                            case 2:
                                osver = "5.2";
                                osname = "Windows 2003";
                                break;
                        }
                        break;
                    case 6:
                        // since there appears to be no managed way to differentiate between Client/Server, we report client names
                        switch (minor) {
                            case 0:
                                osver = "6.0";
                                osname = "Windows Vista";
                                break;
                            case 1:
                                osver = "6.1";
                                osname = "Windows 7";
                                break;
                            case 2:
                                osver = "6.2";
                                osname = "Windows 8";
                                break;
                            case 3:
                                osver = "6.3";
                                osname = "Windows 8.1";
                                break;
                        }
                        break;
                    case 10:
                        switch (minor) {
                            case 0:
                                osver = "10.0";
                                osname = "Windows 10";
                                break;
                        }
                        break;
                }
                break;
            case cli.System.PlatformID.Win32Windows:
                if (major == 4) {
                    switch (minor) {
                        case 0:
                            osver = "4.0";
                            osname = "Windows 95";
                            break;
                        case 10:
                            osver = "4.10";
                            osname = "Windows 98";
                            break;
                        case 90:
                            osver = "4.90";
                            osname = "Windows Me";
                            break;
                    }
                }
                break;
            case cli.System.PlatformID.Unix:
                if (ikvm.internal.Util.MACOSX) {
                    // for back compat Mono will return PlatformID.Unix when running on the Mac,
                    // so we handle that explicitly here
                    osname = "Mac OS X";
                    // HACK this tries to map the Darwin version to the OS X version
                    // (based on http://en.wikipedia.org/wiki/Darwin_(operating_system)#Releases)
                    cli.System.Version ver = cli.System.Environment.get_OSVersion().get_Version();
                    osver = "10." + (ver.get_Major() - 4) + "." + ver.get_Minor();
                }
            break;
        }

        if (osname == null) {
            osname = cli.System.Environment.get_OSVersion().ToString();
        }
        if (osver == null) {
            osver = major + "." + minor;
        }
        p.setProperty("os.name", osname);
        p.setProperty("os.version", osver);

        String arch = SafeGetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
        if (arch == null)
        {
	        // we don't know, so we make a guess
	        if (cli.System.IntPtr.get_Size() == 4) {
		        arch = ikvm.internal.Util.WINDOWS ? "x86" : "i386";
	        } else {
		        arch = "amd64";
	        }
        }

        if (arch.equals("AMD64")) {
            arch = "amd64";
        }

        p.setProperty("os.arch", arch);
        p.setProperty("sun.arch.data.model", String.valueOf(cli.System.IntPtr.get_Size() * 8));
        p.setProperty("file.separator", cli.System.Char.ToString(cli.System.IO.Path.DirectorySeparatorChar));
        p.setProperty("file.encoding", cli.System.Text.Encoding.get_Default().get_WebName());
        p.setProperty("path.separator", String.valueOf(cli.System.IO.Path.PathSeparator));
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
    private static native String getVirtualFileSystemRoot();
    private static native String getBootClassPath();
    private static native String getStdoutEncoding();
    private static native String getStderrEncoding();
    private static native FileVersionInfo getKernel32FileVersionInfo();

}
