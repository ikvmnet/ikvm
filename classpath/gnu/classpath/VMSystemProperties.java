package gnu.classpath;

import java.util.Properties;

public class VMSystemProperties
{
    public static cli.System.Collections.Hashtable props;

    private static native String getVersion();

    private static String GetEnvironmentVariable(String name)
    {
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            return cli.System.Environment.GetEnvironmentVariable(name);
        }
        catch(cli.System.Security.SecurityException _)
        {
            return null;
        }
    }

    static void preInit(Properties p)
    {
        String[] culture = ((cli.System.String)(Object)cli.System.Globalization.CultureInfo.get_CurrentCulture().get_Name()).Split(new char[] { '-' });        
        p.setProperty("user.language", culture[0]);
        p.setProperty("user.region", culture.length > 1 ? culture[1] : "");
        p.setProperty("user.variant", culture.length > 2 ? culture[2] : "");
        p.setProperty("java.version", "1.4.1");
        p.setProperty("java.vendor", "Jeroen Frijters");
        p.setProperty("java.vendor.url", "http://ikvm.net/");
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            // HACK using the Assembly.Location property isn't correct
            cli.System.Reflection.Assembly asm = cli.System.Reflection.Assembly.GetExecutingAssembly();
            p.setProperty("java.home", new cli.System.IO.FileInfo(asm.get_Location()).get_DirectoryName());
        }
        catch(cli.System.Security.SecurityException _)
        {
            // when we're running in partial trust, we may not be allowed file access
            // TODO we may need to set some other value here
            p.setProperty("java.home", ".");
        }
        p.setProperty("java.vm.specification.version", "1.0");
        p.setProperty("java.vm.specification.vendor", "Sun Microsystems Inc.");
        p.setProperty("java.vm.specification.name", "Java Virtual Machine Specification");
        p.setProperty("java.vm.version", getVersion());
        p.setProperty("java.vm.vendor", "Jeroen Frijters");
        p.setProperty("java.vm.name", "IKVM.NET");
        p.setProperty("java.specification.version", "1.4");
        p.setProperty("java.specification.vendor", "Sun Microsystems Inc.");
        p.setProperty("java.specification.name", "Java Platform API Specification");
        p.setProperty("java.class.version", "48.0");
        try
        {
            if(false) throw new cli.System.Security.SecurityException();
            String classpath = cli.System.Environment.GetEnvironmentVariable("CLASSPATH");
            if(classpath == null)
            {
                classpath = ".";
            }
            p.setProperty("java.class.path", classpath);
        }
        catch(cli.System.Security.SecurityException _)
        {
            p.setProperty("java.class.path", "");
        }
        String libraryPath = null;
        if(cli.System.Environment.get_OSVersion().ToString().indexOf("Unix") >= 0)
        {
            libraryPath = GetEnvironmentVariable("LD_LIBRARY_PATH");
        }
        else
        {
            try
            {
                libraryPath = new cli.System.IO.FileInfo(cli.System.Reflection.Assembly.GetEntryAssembly().get_Location()).get_DirectoryName();
            }
            catch(Throwable t)
            {
                // ignore
            }
            if(libraryPath == null)
            {
                libraryPath = ".";
            }
            else
            {
                libraryPath += cli.System.IO.Path.PathSeparator + ".";
            }
            try
            {
                if(false) throw new cli.System.Security.SecurityException();
                libraryPath += cli.System.IO.Path.PathSeparator + cli.System.Environment.get_SystemDirectory();
            }
            catch(cli.System.Security.SecurityException _)
            {
                // ignore
            }
            try
            {
                if(false) throw new cli.System.Security.SecurityException();
                libraryPath += cli.System.IO.Path.PathSeparator + cli.System.Environment.GetEnvironmentVariable("PATH");
            }
            catch(cli.System.Security.SecurityException _)
            {
                // ignore
            }
        }
        if(libraryPath != null)
        {
            p.setProperty("java.library.path", libraryPath);
        }
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
        p.setProperty("java.compiler", "");
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
        String arch = GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
        if(arch == null)
        {
            // TODO get this info from somewhere else
            arch = "x86";
        }
        if(arch.equals("AMD64"))
        {
            arch = "amd64";
        }
        p.setProperty("os.arch", arch);
        p.setProperty("sun.arch.data.model", "" + (cli.System.IntPtr.get_Size() * 8));
        p.setProperty("file.separator", "" + cli.System.IO.Path.DirectorySeparatorChar);
        p.setProperty("file.encoding", "8859_1");
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
        String home = GetEnvironmentVariable("USERPROFILE");
        if(home == null)
        {
            // maybe we're on *nix
            home = GetEnvironmentVariable("HOME");
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
        p.setProperty("awt.toolkit", "ikvm.awt.NetToolkit, IKVM.AWT.WinForms");
        // HACK since we cannot use URL here (it depends on the properties being set), we manually encode the spaces in the assembly name
        p.setProperty("gnu.classpath.home.url", "ikvmres://" + ((cli.System.String)(Object)cli.System.Reflection.Assembly.GetExecutingAssembly().get_FullName()).Replace(" ", "%20") + "/lib");
        p.setProperty("gnu.cpu.endian", cli.System.BitConverter.IsLittleEndian ? "little" : "big");
    }

    static void postInit(Properties p)
    {
        // read properties from app.config
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
        // set the properties that were specified with IKVM.Runtime.Startup.SetProperties()
        if(props != null)
        {
            cli.System.Collections.IEnumerator entries = ((cli.System.Collections.IEnumerable)props).GetEnumerator();
            while(entries.MoveNext())
            {
                cli.System.Collections.DictionaryEntry de = (cli.System.Collections.DictionaryEntry)entries.get_Current();
                p.setProperty((String)de.get_Key(), (String)de.get_Value());
            }
            props = null;
        }
    }
}
