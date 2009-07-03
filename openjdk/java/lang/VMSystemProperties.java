/*
  Copyright (C) 2004-2009 Jeroen Frijters

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

final class VMSystemProperties
{
    private VMSystemProperties() { }

    public static final String SPEC_TITLE = "Java Platform API Specification";
    public static final String SPEC_VERSION = "1.6";
    public static final String SPEC_VENDOR = "Sun Microsystems Inc.";

    private static native String getVersion();

    private static cli.System.Reflection.Assembly GetEntryAssembly()
        throws cli.System.MissingMethodException
    {
        return cli.System.Reflection.Assembly.GetEntryAssembly();
    }

    private static String GetSystemDirectory()
        throws cli.System.MissingMethodException,
               cli.System.Security.SecurityException
    {
        return cli.System.Environment.get_SystemDirectory();
    }

    private static String GetUserName()
        throws cli.System.MissingMethodException,
               cli.System.Security.SecurityException
    {
        return cli.System.Environment.get_UserName();
    }

    private static String GetCurrentDirectory()
        throws cli.System.MissingMethodException,
               cli.System.Security.SecurityException
    {
        return cli.System.Environment.get_CurrentDirectory();
    }

    private static String GetNewLine()
        throws cli.System.MissingMethodException
    {
        return cli.System.Environment.get_NewLine();
    }

    private static String GetEnvironmentVariableImpl(String name)
        throws cli.System.MissingMethodException
    {
        return cli.System.Environment.GetEnvironmentVariable(name);
    }

    private static String GetEnvironmentVariable(String name)
    {
        try
        {
            return GetEnvironmentVariableImpl(name);
        }
        catch(cli.System.MissingMethodException _)
        {
            return null;
        }
    }

    private static String SafeGetEnvironmentVariable(String name)
    {
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            return GetEnvironmentVariable(name);
        }
        catch(cli.System.Security.SecurityException _)
        {
            return null;
        }
    }

    private static String GetAssemblyLocation(cli.System.Reflection.Assembly asm)
        throws cli.System.MissingMethodException
    {
        // Assembly.get_Location() doesn't exist on the Compact Framework
        return asm.get_Location();
    }

    private static String getLibraryPath()
    {
        String libraryPath;
        if(ikvm.internal.Util.WINDOWS)
        {
	    // see /hotspot/src/os/win32/vm/os_win32.cpp for the comment that describes how we build the path
	    libraryPath = ".";
	    String windir = SafeGetEnvironmentVariable("SystemRoot");
	    if(windir != null)
	    {
		libraryPath += cli.System.IO.Path.PathSeparator + windir + "\\Sun\\Java\\bin";
	    }
            try
            {
                libraryPath += cli.System.IO.Path.PathSeparator + GetSystemDirectory();
            }
            catch(cli.System.MissingMethodException _)
            {
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
            cli.System.Reflection.Assembly entryAsm = GetEntryAssembly();
            // If the CLR was started by a native app (e.g. via COM interop) there is no entry assembly
            if (entryAsm != null)
            {
		// the application (or launcher) directory is prepended to the library path
		// (similar to how the JDK prepends its directory to the path)
                libraryPath = new cli.System.IO.FileInfo(GetAssemblyLocation(entryAsm)).get_DirectoryName() + cli.System.IO.Path.PathSeparator + libraryPath;
            }
        }
        catch(cli.System.MissingMethodException _)
        {
        }
        catch(Throwable _)
        {
            // ignore
        }
        return libraryPath;
    }

    private static void initCommonProperties(Properties p)
    {
        p.setProperty("java.version", "1.6.0");
        p.setProperty("java.vendor", "Jeroen Frijters");
        p.setProperty("java.vendor.url", "http://ikvm.net/");
        p.setProperty("java.vendor.url.bug", "http://www.ikvm.net/bugs");
        p.setProperty("java.vm.specification.version", "1.0");
        p.setProperty("java.vm.specification.vendor", "Sun Microsystems Inc.");
        p.setProperty("java.vm.specification.name", "Java Virtual Machine Specification");
        p.setProperty("java.vm.version", getVersion());
        p.setProperty("java.vm.vendor", "Jeroen Frijters");
        p.setProperty("java.vm.name", "IKVM.NET");
        p.setProperty("java.runtime.name", "IKVM.NET");
        p.setProperty("java.runtime.version", getVersion());
        p.setProperty("java.specification.version", SPEC_VERSION);
        p.setProperty("java.specification.vendor", SPEC_VENDOR);
        p.setProperty("java.specification.name", SPEC_TITLE);
        p.setProperty("java.class.version", "50.0");
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
            osver = cli.System.Environment.get_OSVersion().get_Version().ToString();
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
        try
        {
            p.setProperty("line.separator", GetNewLine());
        }
        catch(cli.System.MissingMethodException _)
        {
            p.setProperty("line.separator", "\r\n");
        }
        try
        {
            p.setProperty("user.name", GetUserName());
        }
        catch(cli.System.MissingMethodException _1)
        {
            p.setProperty("user.name", "(unknown)");
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
            p.setProperty("user.dir", GetCurrentDirectory());
        }
        catch(cli.System.MissingMethodException _1)
        {
            p.setProperty("user.dir", ".");
        }
        catch(cli.System.Security.SecurityException _)
        {
            p.setProperty("user.dir", ".");
        }
        p.setProperty("awt.toolkit", PropertyConstants.awt_toolkit);
    }

    public static void initOpenJDK(Properties p)
    {
	initCommonProperties(p);
        String[] culture = ((cli.System.String)(Object)cli.System.Globalization.CultureInfo.get_CurrentCulture().get_Name()).Split(new char[] { '-' });        
        p.setProperty("user.language", culture[0]);
        p.setProperty("user.country", culture.length > 1 ? culture[1] : "");
        p.setProperty("user.variant", culture.length > 2 ? culture[2] : "");
	p.setProperty("sun.cpu.endian", cli.System.BitConverter.IsLittleEndian ? "little" : "big");
	p.setProperty("file.encoding.pkg", "sun.io");
	p.setProperty("user.timezone", "");
	p.setProperty("sun.os.patch.level", "");
	p.setProperty("java.vm.info", "compiled mode");
	p.setProperty("sun.nio.MaxDirectMemorySize", "-1");
	p.setProperty("java.awt.graphicsenv", PropertyConstants.java_awt_graphicsenv);
	// TODO
	// sun.cpu.isalist:=pentium_pro+mmx pentium_pro pentium+mmx pentium i486 i386 i86
	// sun.desktop:=windows
	// sun.io.unicode.encoding:=UnicodeLittle
	// sun.java.launcher:=SUN_STANDARD
	// sun.jnu.encoding:=Cp1252
	// sun.management.compiler:=HotSpot Client Compiler
	// java.awt.printerjob:=sun.awt.windows.WPrinterJob
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
        cli.System.Collections.Hashtable props = ikvm.runtime.Startup.getProperties();
        if(props != null)
        {
            cli.System.Collections.IEnumerator entries = ((cli.System.Collections.IEnumerable)props).GetEnumerator();
            while(entries.MoveNext())
            {
                cli.System.Collections.DictionaryEntry de = (cli.System.Collections.DictionaryEntry)entries.get_Current();
                p.setProperty((String)de.get_Key(), (String)de.get_Value());
            }
        }
    }
}
