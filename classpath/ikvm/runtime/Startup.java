/*
  Copyright (C) 2006-2013 Jeroen Frijters

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

import cli.System.Collections.ArrayList;
import cli.System.Collections.ICollection;
import cli.System.ComponentModel.EditorBrowsableAttribute;
import cli.System.ComponentModel.EditorBrowsableState;
import cli.System.Environment;
import cli.System.InvalidOperationException;
import cli.System.IO.DirectoryInfo;
import cli.System.IO.FileSystemInfo;
import cli.System.IO.Path;
import cli.System.ObsoleteAttribute;
import cli.System.Reflection.Assembly;
import cli.System.Reflection.AssemblyTitleAttribute;
import cli.System.Reflection.AssemblyCopyrightAttribute;
import cli.System.Text.StringBuilder;
import cli.System.Threading.Thread;
import cli.System.Type;
import sun.misc.SignalHandler;

public final class Startup
{
    @ikvm.lang.Internal
    public static cli.System.Collections.IDictionary props;

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

    @Deprecated
    public static String[] glob()
    {
        return glob(1);
    }

    @Deprecated
    public static String[] glob(int skip)
    {
        return glob(Environment.GetCommandLineArgs(), skip);
    }

    public static String[] glob(String[] args, int skip)
    {
        if (!ikvm.internal.Util.WINDOWS)
        {
            String[] vmargs = new String[args.length - skip];
            System.arraycopy(args, skip, vmargs, 0, args.length - skip);
            return vmargs;
        }
        else
        {
            ArrayList list = new ArrayList();
            for (int i = skip; i < args.length; i++)
            {
                String arg = args[i];
                if (arg.indexOf('*') != -1 || arg.indexOf('?') != -1)
                {
                    list.AddRange((ICollection)(Object)glob(arg));
                }
                else
                {
                    list.Add(arg);
                }
            }
            return (String[])(Object)list.ToArray(Type.GetType("System.String"));
        }
    }

    public static void setProperties(cli.System.Collections.IDictionary props)
    {
        Startup.props = props;
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
        
        try{
            sun.misc.Signal.handle(new sun.misc.Signal("BREAK"), SignalHandler.SIG_DFL);
        }catch(IllegalArgumentException ex){
            // ignore it;
        }
    }

    public static void exitMainThread()
    {
        // FXBUG when the main thread ends, it doesn't actually die, it stays around to manage the lifetime
        // of the CLR, but in doing so it also keeps alive the thread local storage for this thread and we
        // use the TLS as a hack to track when the thread dies (if the object stored in the TLS is finalized,
        // we know the thread is dead). So to make that work for the main thread, we use jniDetach which
        // explicitly cleans up our thread.
        jniDetach();
    }
    
    private static native void jniDetach();

    public static String getVersionAndCopyrightInfo()
    {
        Assembly asm = Assembly.GetEntryAssembly();
        Object[] desc = asm.GetCustomAttributes(Util.getInstanceTypeFromClass(AssemblyTitleAttribute.class), false);
        if(desc.length == 1)
        {
            Object[] copyright = asm.GetCustomAttributes(Util.getInstanceTypeFromClass(AssemblyCopyrightAttribute.class), false);
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

    // note the stupid typo, this exists for compatibility
    @EditorBrowsableAttribute.Annotation(EditorBrowsableState.__Enum.Never)
    @ObsoleteAttribute.Annotation("Please use the version without the typo.")
    @Deprecated
    public static void addBootClassPathAssemby(Assembly assembly)
    {
        addBootClassPathAssembly(assembly);
    }

    public static native void addBootClassPathAssembly(Assembly assembly);
}
