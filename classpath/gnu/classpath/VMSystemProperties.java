package gnu.classpath;

import java.util.Properties;

public class VMSystemProperties
{
    public static cli.System.Collections.Hashtable props;

    private static native String getVersion();

    static void preInit(Properties p)
    {
        String[] culture = ((cli.System.String)(Object)cli.System.Globalization.CultureInfo.get_CurrentCulture().get_Name()).Split(new char[] { '-' });        
        p.setProperty("user.language", culture[0]);
        p.setProperty("user.region", culture.length > 1 ? culture[1] : "");
        p.setProperty("user.variant", culture.length > 2 ? culture[2] : "");
        p.setProperty("java.version", "1.4");
        p.setProperty("java.vendor", "Jeroen Frijters");
        p.setProperty("java.vendor.url", "http://ikvm.net/");
        // HACK using the Assembly.Location property isn't correct
        cli.System.Reflection.Assembly asm = cli.System.Reflection.Assembly.GetExecutingAssembly();
        p.setProperty("java.home", new cli.System.IO.FileInfo(asm.get_Location()).get_DirectoryName());
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
        String classpath = cli.System.Environment.GetEnvironmentVariable("CLASSPATH");
        if(classpath == null)
        {
            classpath = ".";
        }
        p.setProperty("java.class.path", classpath);
        String libraryPath = null;
        if(cli.System.Environment.get_OSVersion().ToString().indexOf("Unix") >= 0)
        {
            libraryPath = cli.System.Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
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
            libraryPath += cli.System.IO.Path.PathSeparator + cli.System.Environment.get_SystemDirectory() +
                cli.System.IO.Path.PathSeparator + cli.System.Environment.GetEnvironmentVariable("PATH");
        }
        if(libraryPath != null)
        {
            p.setProperty("java.library.path", libraryPath);
        }
        p.setProperty("java.io.tmpdir", cli.System.IO.Path.GetTempPath());
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
        String arch = cli.System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
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
        p.setProperty("user.name", cli.System.Environment.get_UserName());
        String home = cli.System.Environment.GetEnvironmentVariable("USERPROFILE");
        if(home == null)
        {
            // maybe we're on *nix
            home = cli.System.Environment.GetEnvironmentVariable("HOME");
            if(home == null)
            {
                // TODO maybe there is a better way
                // NOTE on MS .NET this doesn't return the correct path
                // (it returns "C:\\Documents and Settings\\username\\My Documents", but we really need
                // "C:\\Documents and Settings\\username" to be compatible with Sun, that's why we use %USERPROFILE% if it exists)
                home = cli.System.Environment.GetFolderPath(cli.System.Environment.SpecialFolder.wrap(cli.System.Environment.SpecialFolder.Personal));
            }
        }
        p.setProperty("user.home", home);
        p.setProperty("user.dir", cli.System.Environment.get_CurrentDirectory());
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
