/*
  Copyright (C) 2006 Jeroen Frijters

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

package ikvm.runtime;

import ikvm.lang.CIL;
import cli.System.Collections.ArrayList;
import cli.System.Collections.ICollection;
import cli.System.Environment;
import cli.System.InvalidOperationException;
import cli.System.IO.DirectoryInfo;
import cli.System.IO.FileSystemInfo;
import cli.System.IO.Path;
import cli.System.Reflection.Assembly;
import cli.System.Reflection.AssemblyTitleAttribute;
import cli.System.Reflection.AssemblyCopyrightAttribute;
import cli.System.Text.StringBuilder;
import cli.System.Threading.Thread;
import cli.System.Type;

public final class Startup
{
    private static cli.System.Collections.Hashtable props;

    private Startup()
    {
    }

    private static String[] glob(String arg)
    {
        try
        {
            String dir = Path.GetDirectoryName(arg);
            if(dir == "")
            {
                dir = null;
            }
            ArrayList list = new ArrayList();
            for(FileSystemInfo fsi : new DirectoryInfo(dir == null ? Environment.get_CurrentDirectory() : dir).GetFileSystemInfos(Path.GetFileName(arg)))
            {
                list.Add(dir != null ? Path.Combine(dir, fsi.get_Name()) : fsi.get_Name());
            }
            if(list.get_Count() == 0)
            {
                return new String[] { arg };
            }
            return (String[])(Object)list.ToArray(Type.GetType("System.String"));
        }
        catch(Throwable _)
        {
            return new String[] { arg };
        }
    }

    public static String[] glob()
    {
        return glob(1);
    }

    public static String[] glob(int skip)
    {
        if(Environment.get_OSVersion().ToString().indexOf("Unix") >= 0)
        {
            String[] args = Environment.GetCommandLineArgs();
            String[] vmargs = new String[args.length - skip];
            System.arraycopy(args, skip, vmargs, 0, args.length - skip);
            return vmargs;
        }
        else
        {
            ArrayList list = new ArrayList();
            String cmdline = Environment.get_CommandLine();
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < cmdline.length(); )
            {
                boolean quoted = cmdline.charAt(i) == '"';
                for(;;)
                {
                    while(i < cmdline.length() && cmdline.charAt(i) != ' ' && cmdline.charAt(i) != '"')
                    {
                        sb.Append(cmdline.charAt(i++));
                    }
                    if(i < cmdline.length() && cmdline.charAt(i) == '"')
                    {
                        if(quoted && i > 1 && cmdline.charAt(i - 1) == '"')
                        {
                            sb.Append('"');
                        }
                        i++;
                        while(i < cmdline.length() && cmdline.charAt(i) != '"')
                        {
                            sb.Append(cmdline.charAt(i++));
                        }
                        if(i < cmdline.length() && cmdline.charAt(i) == '"')
                        {
                            i++;
                        }
                        if(i < cmdline.length() && cmdline.charAt(i) != ' ')
                        {
                            continue;
                        }
                    }
                    break;
                }
                while(i < cmdline.length() && cmdline.charAt(i) == ' ')
                {
                    i++;
                }
                if(skip > 0)
                {
                    skip--;
                }
                else
                {
                    if(quoted)
                    {
                        list.Add(sb.ToString());
                    }
                    else
                    {
                        list.AddRange((ICollection)(Object)glob(sb.ToString()));
                    }
                }
                sb.set_Length(0);
            }
            return (String[])(Object)list.ToArray(Type.GetType("System.String"));
        }
    }

    public static void setProperties(cli.System.Collections.Hashtable props)
    {
        Startup.props = props;
    }
    
    @ikvm.lang.Internal
    public static cli.System.Collections.Hashtable getProperties()
    {
        cli.System.Collections.Hashtable h = props;
        props = null;
        return h;
    }

    public static void enterMainThread()
    {
        if(Thread.get_CurrentThread().get_Name() == null)
        {
            try
            {
                if(false) throw new InvalidOperationException();
                Thread.get_CurrentThread().set_Name("main");
            }
            catch(InvalidOperationException _)
            {
            }
        }
	java.lang.Thread.currentThread();
    }

    public static void exitMainThread()
    {
        // FXBUG when the main thread ends, it doesn't actually die, it stays around to manage the lifetime
        // of the CLR, but in doing so it also keeps alive the thread local storage for this thread and we
        // use the TLS as a hack to track when the thread dies (if the object stored in the TLS is finalized,
        // we know the thread is dead). So to make that work for the main thread, we use jniDetach which
        // explicitly cleans up our thread.
        VMThread.jniDetach();
    }

    public static String getVersionAndCopyrightInfo()
    {
        Assembly asm = Assembly.GetEntryAssembly();
        Object[] desc = asm.GetCustomAttributes(Type.GetType("System.Reflection.AssemblyTitleAttribute"), false);
        if(desc.length == 1)
        {
            Object[] copyright = asm.GetCustomAttributes(Type.GetType("System.Reflection.AssemblyCopyrightAttribute"), false);
            if(copyright.length == 1)
            {
                return cli.System.String.Format("{0} version {1}{2}{3}{2}http://www.ikvm.net/",
                    ((AssemblyTitleAttribute)desc[0]).get_Title(),
                    asm.GetName().get_Version(),
                    Environment.get_NewLine(),
                    ((AssemblyCopyrightAttribute)copyright[0]).get_Copyright());
            }
        }
        return "";
    }
}
