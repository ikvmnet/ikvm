/*
  Copyright (C) 2003, 2004 Jeroen Frijters

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
import gnu.java.nio.channels.FileChannelImpl;
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

    static
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
	// the CLR always runs the finalizers, so we can ignore this
    }

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
    static native int nativeLoad(String filename);

    /**
     * Map a system-independent "short name" to the full file name, and append
     * it to the path.
     * XXX This method is being replaced by System.mapLibraryName.
     *
     * @param pathname the path
     * @param libname the short version of the library name
     * @return the full filename
     */
    static String nativeGetLibname(String pathname, String libname)
    {
	if(cli.System.Environment.get_OSVersion().ToString().indexOf("Unix") >= 0)
	{
	    return "lib" + libname + ".so";
	}

	// HACK this seems like a lame way of doing things, but in order to get Eclipse to work,
	// we have append .dll to the libname here
	if(!libname.toUpperCase().endsWith(".DLL"))
	{
	    libname += ".dll";
	}
	return libname;
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
	    // TODO enable this when Channels.new[Out|in]putStream is working
	    //stdin = Channels.newOutputStream(new FileChannelImpl(proc.get_StandardInput().get_BaseStream()));
	    //stdout = Channels.newInputStream(new FileChannelImpl(proc.get_StandardOutput().get_BaseStream()));
	    //stderr = Channels.newInputStream(new FileChannelImpl(proc.get_StandardError().get_BaseStream()));
	    stdin = new gnu.java.nio.ChannelOutputStream(new FileChannelImpl(proc.get_StandardInput().get_BaseStream()));
	    stdout = new gnu.java.nio.ChannelInputStream(new FileChannelImpl(proc.get_StandardOutput().get_BaseStream()));
	    stderr = new gnu.java.nio.ChannelInputStream(new FileChannelImpl(proc.get_StandardError().get_BaseStream()));
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
	    proc.WaitForExit();
	    return proc.get_ExitCode();
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


    /**
     * Get the system properties. This is done here, instead of in System,
     * because of the bootstrap sequence. Note that the native code should
     * not try to use the Java I/O classes yet, as they rely on the properties
     * already existing. The only safe method to use to insert these default
     * system properties is {@link Properties#setProperty(String, String)}.
     *
     * <p>These properties MUST include:
     * <dl>
     * <dt>java.version         <dd>Java version number
     * <dt>java.vendor          <dd>Java vendor specific string
     * <dt>java.vendor.url      <dd>Java vendor URL
     * <dt>java.home            <dd>Java installation directory
     * <dt>java.vm.specification.version <dd>VM Spec version
     * <dt>java.vm.specification.vendor  <dd>VM Spec vendor
     * <dt>java.vm.specification.name    <dd>VM Spec name
     * <dt>java.vm.version      <dd>VM implementation version
     * <dt>java.vm.vendor       <dd>VM implementation vendor
     * <dt>java.vm.name         <dd>VM implementation name
     * <dt>java.specification.version    <dd>Java Runtime Environment version
     * <dt>java.specification.vendor     <dd>Java Runtime Environment vendor
     * <dt>java.specification.name       <dd>Java Runtime Environment name
     * <dt>java.class.version   <dd>Java class version number
     * <dt>java.class.path      <dd>Java classpath
     * <dt>java.library.path    <dd>Path for finding Java libraries
     * <dt>java.io.tmpdir       <dd>Default temp file path
     * <dt>java.compiler        <dd>Name of JIT to use
     * <dt>java.ext.dirs        <dd>Java extension path
     * <dt>os.name              <dd>Operating System Name
     * <dt>os.arch              <dd>Operating System Architecture
     * <dt>os.version           <dd>Operating System Version
     * <dt>file.separator       <dd>File separator ("/" on Unix)
     * <dt>path.separator       <dd>Path separator (":" on Unix)
     * <dt>line.separator       <dd>Line separator ("\n" on Unix)
     * <dt>user.name            <dd>User account name
     * <dt>user.home            <dd>User home directory
     * <dt>user.dir             <dd>User's current working directory
     * </dl>
     *
     * @param p the Properties object to insert the system properties into
     */
    static void insertSystemProperties(Properties p)
    {
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
	String libraryPath = ".";
	if(cli.System.Environment.get_OSVersion().ToString().indexOf("Unix") >= 0)
	{
	    String ldLibraryPath = cli.System.Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
	    if (ldLibraryPath != null)
	    {
		libraryPath = ldLibraryPath;
	    }
	    else
	    {
		libraryPath = "";
	    }
	}
	p.setProperty("java.library.path", libraryPath);
	p.setProperty("java.io.tmpdir", cli.System.IO.Path.GetTempPath());
	p.setProperty("java.compiler", "");
	p.setProperty("java.ext.dirs", "");
	// NOTE os.name *must* contain "Windows" when running on Windows, because Classpath tests on that
	String osname = cli.System.Environment.get_OSVersion().ToString();
	String osver = cli.System.Environment.get_OSVersion().get_Version().ToString();
	// HACK if the osname contains the version, we remove it
	osname = ((cli.System.String)(Object)osname).Replace(osver, "").trim();
	p.setProperty("os.name", osname);
	String arch = cli.System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
	if(arch == null)
	{
	    // TODO get this info from somewhere else
	    arch = "x86";
	}
	p.setProperty("os.arch", arch);
	p.setProperty("os.version", osver);
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
    }

    // HACK we need a way to get the assembly version of ik.vm.net.dll
    static native String getVersion();
} // class VMRuntime
