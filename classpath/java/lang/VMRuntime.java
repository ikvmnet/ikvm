/*
  Copyright (C) 2003, 2004, 2006 Jeroen Frijters

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

import java.io.File;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Properties;
import java.nio.channels.Channels;
import gnu.classpath.SystemProperties;
import gnu.java.nio.FileChannelImpl;
import cli.System.Text.StringBuilder;
import cli.System.Diagnostics.ProcessStartInfo;

/**
 * VMRuntime represents the interface to the Virtual Machine.
 *
 * @author Jeroen Frijters
 */
final class VMRuntime
{
    /**
     * No instance is ever created.
     */
    private VMRuntime() 
    {
    }

    static void enableShutdownHooks()
    {
	cli.System.AppDomain.get_CurrentDomain().add_ProcessExit(new cli.System.EventHandler(new cli.System.EventHandler.Method() {
	    public void Invoke(Object sender, cli.System.EventArgs e) {
		Runtime.getRuntime().runShutdownHooks();
	    }
	}));
    }


    /**
     * Returns the number of available processors currently available to the
     * virtual machine. This number may change over time; so a multi-processor
     * program want to poll this to determine maximal resource usage.
     *
     * @return the number of processors available, at least 1
     */
    static int availableProcessors()
    {
	String s = cli.System.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
	if(s != null)
	{
	    return Integer.parseInt(s);
	}
	return 1;
    }

    /**
     * Find out how much memory is still free for allocating Objects on the heap.
     *
     * @return the number of bytes of free memory for more Objects
     */
    static long freeMemory()
    {
	// TODO figure out if there is anything meaningful we can return here
	return 10 * 1024 * 1024;
    }

    /**
     * Find out how much memory total is available on the heap for allocating
     * Objects.
     *
     * @return the total number of bytes of memory for Objects
     */
    static long totalMemory()
    {
	// NOTE this really is a bogus number, but we have to return something
	return cli.System.GC.GetTotalMemory(false) + freeMemory();
    }

    /**
     * Returns the maximum amount of memory the virtual machine can attempt to
     * use. This may be <code>Long.MAX_VALUE</code> if there is no inherent
     * limit (or if you really do have a 8 exabyte memory!).
     *
     * @return the maximum number of bytes the virtual machine will attempt
     *         to allocate
     */
    static long maxMemory()
    {
	// spec says: If there is no inherent limit then the value Long.MAX_VALUE will be returned.
	return Long.MAX_VALUE;
    }

    /**
     * Run the garbage collector. This method is more of a suggestion than
     * anything. All this method guarantees is that the garbage collector will
     * have "done its best" by the time it returns. Notice that garbage
     * collection takes place even without calling this method.
     */
    static void gc()
    {
	cli.System.GC.Collect();
    }

    /**
     * Run finalization on all Objects that are waiting to be finalized. Again,
     * a suggestion, though a stronger one than {@link #gc()}. This calls the
     * <code>finalize</code> method of all objects waiting to be collected.
     *
     * @see #finalize()
     */
    static void runFinalization()
    {
        cli.System.GC.WaitForPendingFinalizers();
    }

    static void runFinalizationForExit()
    {
	// The CLR has its own finalization for exit strategy
    }

    /**
     * Tell the VM to trace every bytecode instruction that executes (print out
     * a trace of it).  No guarantees are made as to where it will be printed,
     * and the VM is allowed to ignore this request.
     *
     * @param on whether to turn instruction tracing on
     */
    static void traceInstructions(boolean on)
    {
	// not supported
    }

    /**
     * Tell the VM to trace every method call that executes (print out a trace
     * of it).  No guarantees are made as to where it will be printed, and the
     * VM is allowed to ignore this request.
     *
     * @param on whether to turn method tracing on
     */
    static void traceMethodCalls(boolean on)
    {
	// TODO integrate with method tracing
    }

    /**
     * Native method that actually sets the finalizer setting.
     *
     * @param value whether to run finalizers on exit
     */
    static void runFinalizersOnExit(boolean value)
    {
        runFinalizersOnExitFlag = value;
    }
    // the default is not the run finalizers on exit
    static volatile boolean runFinalizersOnExitFlag;

    /**
     * Native method that actually shuts down the virtual machine.
     *
     * @param status the status to end the process with
     */
    static void exit(int status)
    {
	cli.System.Environment.Exit(status);
    }

    /**
     * Load a file. If it has already been loaded, do nothing. The name has
     * already been mapped to a true filename.
     *
     * @param filename the file to load
     * @return 0 on failure, nonzero on success
     */
    static native int nativeLoad(String filename, Object classLoader);

    /**
     * Map a system-independent "short name" to the full file name.
     *
     * @param libname the short version of the library name
     * @return the full filename
     */
    static String mapLibraryName(String libname)
    {
        String osname = SystemProperties.getProperty("os.name");
	if(osname.indexOf("Windows") >= 0)
	{
            return libname + ".dll";
        }
        else if(osname.equals("Mac OS X"))
        {
            return "lib" + libname + ".jnilib";
        }
	else
	{
            return "lib" + libname + ".so";
	}
    }

    /**
     * Execute a process. The command line has already been tokenized, and
     * the environment should contain name=value mappings. If directory is null,
     * use the current working directory; otherwise start the process in that
     * directory.  If env is null, then the new process should inherit
     * the environment of this process.
     *
     * @param cmd the non-null command tokens
     * @param env the environment setup
     * @param dir the directory to use, may be null
     * @return the newly created process
     * @throws NullPointerException if cmd or env have null elements
     */
    static Process exec(String[] cmd, String[] env, File dir) throws java.io.IOException
    {
	StringBuilder sb = new StringBuilder();
	for(int i = 1; i < cmd.length; i++)
	{
	    if(i > 1)
	    {
		sb.Append(' ');
	    }
	    // HACK if the arg contains a space, we surround it with quotes
	    // this isn't nearly good enough, but for now it'll have to do
	    if(cmd[i].indexOf(' ') >= 0 && cmd[i].charAt(0) != '"')
	    {
		sb.Append('"').Append(cmd[i]).Append('"');
	    }
	    else
	    {
		sb.Append(cmd[i]);
	    }
	}
	ProcessStartInfo si = new ProcessStartInfo(cmd[0], sb.ToString());
	si.set_UseShellExecute(false);
	si.set_RedirectStandardError(true);
	si.set_RedirectStandardOutput(true);
	si.set_RedirectStandardInput(true);
	si.set_CreateNoWindow(true);
	if(dir != null)
	{
	    si.set_WorkingDirectory(dir.toString());
	}
	if(env != null)
	{
	    for(int i = 0; i < env.length; i++)
	    {
		int pos = env[i].indexOf('=');
		si.get_EnvironmentVariables().set_Item(env[i].substring(0, pos), env[i].substring(pos + 1));
	    }
	}
	try
	{
	    if(false) throw new cli.System.ComponentModel.Win32Exception();
	    if(false) throw new cli.System.InvalidOperationException();
	    return new DotNetProcess(cli.System.Diagnostics.Process.Start(si));
	}
	catch(cli.System.ComponentModel.Win32Exception x1)
	{
	    throw new java.io.IOException(x1.getMessage());
	}
	catch(cli.System.InvalidOperationException x2)
	{
	    throw new java.io.IOException(x2.getMessage());
	}
    }

    private static class DotNetProcess extends Process
    {
	private cli.System.Diagnostics.Process proc;
	private OutputStream stdin;
	private InputStream stdout;
	private InputStream stderr;

	private DotNetProcess(cli.System.Diagnostics.Process proc)
	{
	    this.proc = proc;
	    stdin = Channels.newOutputStream(FileChannelImpl.create(proc.get_StandardInput().get_BaseStream()));
	    stdout = Channels.newInputStream(FileChannelImpl.create(proc.get_StandardOutput().get_BaseStream()));
	    stderr = Channels.newInputStream(FileChannelImpl.create(proc.get_StandardError().get_BaseStream()));
	}

	public OutputStream getOutputStream()
	{
	    return stdin;
	}

	public InputStream getInputStream()
	{
	    return stdout;
	}

	public InputStream getErrorStream()
	{
	    return stderr;
	}

	public int waitFor() throws InterruptedException
	{
            // to be interruptable we have to use polling
            for(;;)
            {
                if(Thread.interrupted())
                {
                    throw new InterruptedException();
                }
	        if(proc.WaitForExit(100))
                {
	            return proc.get_ExitCode();
                }
            }
	}

	public int exitValue()
	{
	    if(proc.get_HasExited())
	    {
		return proc.get_ExitCode();
	    }
	    throw new IllegalThreadStateException();
	}

	public void destroy()
	{
	    try
	    {
		if(false) throw new cli.System.InvalidOperationException();
		proc.Kill();
	    }
	    catch(cli.System.InvalidOperationException x)
	    {
	    }
	}
    }
} // class VMRuntime
