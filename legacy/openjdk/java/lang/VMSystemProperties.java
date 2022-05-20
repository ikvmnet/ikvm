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

import java.util.Properties;
import cli.System.Diagnostics.FileVersionInfo;
import static ikvm.internal.Util.SafeGetEnvironmentVariable;

final class VMSystemProperties
{
    private VMSystemProperties() { }

    public static final String SPEC_TITLE = "Java Platform API Specification";
    public static final String SPEC_VERSION = "1.8";
    public static final String SPEC_VENDOR = "Oracle Corporation";

    private static String getLibraryPath()
    {
        String libraryPath;
        if(ikvm.internal.Util.WINDOWS)
        {
	    // see /hotspot/src/os/windows/vm/os_windows.cpp for the comment that describes how we build the path
	    String windir = SafeGetEnvironmentVariable("SystemRoot");
	    if(windir != null)
	    {
		libraryPath = cli.System.IO.Path.PathSeparator + windir + "\\Sun\\Java\\bin";
	    }
	    else
	    {
	        libraryPath = null;
	    }
            try
            {
                if(false) throw new cli.System.Security.SecurityException();
                if (libraryPath == null)
                {
                    libraryPath = cli.System.Environment.get_SystemDirectory();
                }
                else
                {
                    libraryPath += cli.System.IO.Path.PathSeparator + cli.System.Environment.get_SystemDirectory();
                }
            }
            catch(cli.System.Security.SecurityException _)
            {
            }
            if(windir != null)
            {
		libraryPath += cli.System.IO.Path.PathSeparator + windir;
            }
            String path = SafeGetEnvironmentVariable("PATH");
            if(path != null)
            {
                libraryPath += cli.System.IO.Path.PathSeparator + path;
            }
        }
        else if(ikvm.internal.Util.MACOSX)
        {
            libraryPath = ".";
        }
        else /* assume Linux, since that's the only other platform we support */
        {
	    // on Linux we have some hardcoded paths (from /hotspot/src/os/linux/vm/os_linux.cpp)
	    // and we can only guess the cpu arch based on bitness (that means only x86 and x64)
	    String cpu_arch = cli.System.IntPtr.get_Size() == 4 ? "i386" : "amd64";
	    libraryPath = "/usr/java/packages/lib/" + cpu_arch + ":/lib:/usr/lib";
            String ld_library_path = SafeGetEnvironmentVariable("LD_LIBRARY_PATH");
            if(ld_library_path != null)
            {
		libraryPath = ld_library_path + ":" + libraryPath;
            }
        }
        try
        {
            cli.System.Reflection.Assembly entryAsm = cli.System.Reflection.Assembly.GetEntryAssembly();
            // If the CLR was started by a native app (e.g. via COM interop) there is no entry assembly
            if (entryAsm != null)
            {
		// the application (or launcher) directory is prepended to the library path
		// (similar to how the JDK prepends its directory to the path)
                libraryPath = new cli.System.IO.FileInfo(entryAsm.get_Location()).get_DirectoryName() + cli.System.IO.Path.PathSeparator + libraryPath;
            }
        }
        catch(Throwable _)
        {
            // ignore
        }
        if(ikvm.internal.Util.WINDOWS)
        {
            libraryPath += cli.System.IO.Path.PathSeparator + ".";
        }
        return libraryPath;
    }

    private static void initCommonProperties(Properties p)
    {
        p.setProperty("java.version", "1.8.0");
        p.setProperty("java.vendor", "Jeroen Frijters");
        p.setProperty("java.vendor.url", "http://ikvm.net/");
        p.setProperty("java.vendor.url.bug", "http://www.ikvm.net/bugs");
        p.setProperty("java.vm.specification.version", "1.8");
        p.setProperty("java.vm.specification.vendor", "Oracle Corporation");
        p.setProperty("java.vm.specification.name", "Java Virtual Machine Specification");
        p.setProperty("java.vm.version", PropertyConstants.java_vm_version);
        p.setProperty("java.vm.vendor", "Jeroen Frijters");
        p.setProperty("java.vm.name", "IKVM.NET");
        p.setProperty("java.runtime.name", "IKVM.NET");
        p.setProperty("java.runtime.version", PropertyConstants.java_runtime_version);
        p.setProperty("java.specification.version", SPEC_VERSION);
        p.setProperty("java.specification.vendor", SPEC_VENDOR);
        p.setProperty("java.specification.name", SPEC_TITLE);
        p.setProperty("java.class.version", "52.0");
        p.setProperty("java.class.path", "");
        p.setProperty("java.library.path", getLibraryPath());
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            p.setProperty("java.io.tmpdir", cli.System.IO.Path.GetTempPath());
        }
        catch(cli.System.Security.SecurityException _)
        {
            // TODO should we set another value?
            p.setProperty("java.io.tmpdir", ".");
        }
        p.setProperty("java.ext.dirs", "");
        // NOTE os.name *must* contain "Windows" when running on Windows, because Classpath tests on that
        String osname = null;
        String osver = null;
        cli.System.OperatingSystem os = cli.System.Environment.get_OSVersion();
        int major = os.get_Version().get_Major();
        int minor = os.get_Version().get_Minor();
        switch(os.get_Platform().Value)
        {
            case cli.System.PlatformID.Win32NT:
                // Windows lies about the version, so we extract the real version from kernel32.dll
                FileVersionInfo kernel32 = getKernel32FileVersionInfo();
                if (kernel32 != null)
                {
                    major = kernel32.get_ProductMajorPart();
                    minor = kernel32.get_ProductMinorPart();
                }
                osname = "Windows NT (unknown)";
                switch(major)
                {
                    case 3:
                    case 4:
                        osver = major + "." + minor;
                        osname = "Windows NT";
                        break;
                    case 5:
                        switch(minor)
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
                        switch(minor)
                        {
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
                        switch(minor)
                        {
                            case 0:
                                osver = "10.0";
                                osname = "Windows 10";
                                break;
                        }
                        break;
                }
                break;
            case cli.System.PlatformID.Win32Windows:
                if(major == 4)
                {
                    switch(minor)
                    {
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
		if(ikvm.internal.Util.MACOSX)
		{
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
        if(osname == null)
        {
            osname = cli.System.Environment.get_OSVersion().ToString();
        }
        if(osver == null)
        {
            osver = major + "." + minor;
        }
        p.setProperty("os.name", osname);
        p.setProperty("os.version", osver);
        String arch = SafeGetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
        if(arch == null)
        {
	    // we don't know, so we make a guess
	    if(cli.System.IntPtr.get_Size() == 4)
	    {
		arch = ikvm.internal.Util.WINDOWS ? "x86" : "i386";
	    }
	    else
	    {
		arch = "amd64";
	    }
        }
        if(arch.equals("AMD64"))
        {
            arch = "amd64";
        }
        p.setProperty("os.arch", arch);
        p.setProperty("sun.arch.data.model", "" + (cli.System.IntPtr.get_Size() * 8));
        p.setProperty("file.separator", "" + cli.System.IO.Path.DirectorySeparatorChar);
        p.setProperty("file.encoding", cli.System.Text.Encoding.get_Default().get_WebName());
        p.setProperty("path.separator", "" + cli.System.IO.Path.PathSeparator);
        p.setProperty("line.separator", cli.System.Environment.get_NewLine());
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            p.setProperty("user.name", cli.System.Environment.get_UserName());
        }
        catch(cli.System.Security.SecurityException _)
        {
            p.setProperty("user.name", "(unknown)");
        }
        String home = SafeGetEnvironmentVariable("USERPROFILE");
        if(home == null)
        {
            // maybe we're on *nix
            home = SafeGetEnvironmentVariable("HOME");
            if(home == null)
            {
                // TODO maybe there is a better way
                // NOTE on MS .NET this doesn't return the correct path
                // (it returns "C:\\Documents and Settings\\username\\My Documents", but we really need
                // "C:\\Documents and Settings\\username" to be compatible with Sun, that's why we use %USERPROFILE% if it exists)
                try
                {
                    if(false) throw new cli.System.Security.SecurityException();
                    home = cli.System.Environment.GetFolderPath(cli.System.Environment.SpecialFolder.wrap(cli.System.Environment.SpecialFolder.Personal));
                }
                catch(cli.System.Security.SecurityException _)
                {
                    home = ".";
                }
            }
        }
        p.setProperty("user.home", home);
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            p.setProperty("user.dir", cli.System.Environment.get_CurrentDirectory());
        }
        catch(cli.System.Security.SecurityException _)
        {
            p.setProperty("user.dir", ".");
        }
        p.setProperty("awt.toolkit", PropertyConstants.awt_toolkit);
    }

    public static void initProperties(Properties p)
    {
        p.setProperty("openjdk.version", PropertyConstants.openjdk_version);
        String vfsroot = getVirtualFileSystemRoot();
        p.setProperty("java.home", vfsroot.substring(0, vfsroot.length() - 1));
        // the %home%\lib\endorsed directory does not exist, but neither does it on JDK 1.7
        p.setProperty("java.endorsed.dirs", vfsroot + "lib" + cli.System.IO.Path.DirectorySeparatorChar + "endorsed");
        p.setProperty("sun.boot.library.path", vfsroot + "bin");
        p.setProperty("sun.boot.class.path", getBootClassPath());
	initCommonProperties(p);
	setupI18N(p);
	p.setProperty("sun.cpu.endian", cli.System.BitConverter.IsLittleEndian ? "little" : "big");
	p.setProperty("file.encoding.pkg", "sun.io");
	p.setProperty("user.timezone", "");
	p.setProperty("sun.os.patch.level", cli.System.Environment.get_OSVersion().get_ServicePack());
	p.setProperty("java.vm.info", "compiled mode");
	p.setProperty("sun.nio.MaxDirectMemorySize", "-1");
	p.setProperty("java.awt.graphicsenv", PropertyConstants.java_awt_graphicsenv);
        p.setProperty("java.awt.printerjob", "sun.awt.windows.WPrinterJob");

        String stdoutEncoding = getStdoutEncoding();
        if(stdoutEncoding != null)
        {
            p.setProperty("sun.stdout.encoding", stdoutEncoding);
        }
        String stderrEncoding = getStderrEncoding();
        if(stderrEncoding != null)
        {
            p.setProperty("sun.stderr.encoding", stderrEncoding);
        }

        if(ikvm.internal.Util.MACOSX)
        {
            p.setProperty("sun.jnu.encoding", "UTF-8");
        }
        else
        {
            p.setProperty("sun.jnu.encoding", cli.System.Text.Encoding.get_Default().get_WebName());
        }

        
	// TODO
	// sun.cpu.isalist:=pentium_pro+mmx pentium_pro pentium+mmx pentium i486 i386 i86
	// sun.desktop:=windows
	// sun.io.unicode.encoding:=UnicodeLittle
	// sun.management.compiler:=HotSpot Client Compiler
        try
        {
            // read properties from app.config
            if(false) throw new cli.System.Configuration.ConfigurationException();
            cli.System.Collections.Specialized.NameValueCollection appSettings = cli.System.Configuration.ConfigurationSettings.get_AppSettings();
            cli.System.Collections.IEnumerator keys = appSettings.GetEnumerator();
            while(keys.MoveNext())
            {
                String key = (String)keys.get_Current();
                if(key.startsWith("ikvm:"))
                {
                    p.setProperty(key.substring(5), appSettings.get_Item(key));
                }
            }
        }
        catch(cli.System.Configuration.ConfigurationException _)
        {
            // app.config is invalid, ignore
        }
        // set the properties that were specified with ikvm.runtime.Startup.setProperties()
        cli.System.Collections.IDictionary props = ikvm.runtime.Startup.props;
        if(props != null)
        {
            cli.System.Collections.IDictionaryEnumerator entries = props.GetEnumerator();
            while(entries.MoveNext())
            {
                p.setProperty((String)entries.get_Key(), (String)entries.get_Value());
            }
        }
    }

    private static void setupI18N(Properties p)
    {
        String[] culture = ((cli.System.String)(Object)cli.System.Globalization.CultureInfo.get_CurrentCulture().get_Name()).Split(new char[] { '-' });
        String language;
        String script;
        String region;
        String variant;
        if (culture.length == 2)
        {
            language = culture[0];
            if (culture[1].length() == 4)
            {
                script = culture[1];
                region = "";
            }
            else
            {
                script = "";
                region = culture[1];
            }
        }
        else if (culture.length == 3)
        {
            language = culture[0];
            script = culture[1];
            region = culture[2];
        }
        else
        {
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
        }
        else if (language.equals("nn"))
        {
            language = "no";
            region = "NO";
            variant = "NY";
        }
        else
        {
            variant = "";
        }
        p.setProperty("user.language", language);
        p.setProperty("user.country", region);
        p.setProperty("user.variant", variant);
        p.setProperty("user.script", script);
    }

    private static native String getVirtualFileSystemRoot();
    private static native String getBootClassPath();
    private static native String getStdoutEncoding();
    private static native String getStderrEncoding();
    private static native FileVersionInfo getKernel32FileVersionInfo();
}
