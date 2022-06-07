/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using IKVM.Internal;
using ikvm.runtime;
using java.lang.reflect;

public static class Starter
{
	private class Timer
	{
		private static Timer t;
		private DateTime now = DateTime.Now;

		internal Timer()
		{
			t = this;
		}

		~Timer()
		{
			Console.WriteLine(DateTime.Now - now);
		}
	}

	private class SaveAssemblyShutdownHook : java.lang.Thread
	{
		private java.lang.Class clazz;

		internal SaveAssemblyShutdownHook(java.lang.Class clazz)
			: base("SaveAssemblyShutdownHook")
		{
			this.clazz = clazz;
		}

		public override void run()
		{
			System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
			try
			{
				IKVM.Internal.Starter.SaveDebugImage();
			}
			catch(Exception x)
			{
				Console.Error.WriteLine(x);
				Console.Error.WriteLine(new System.Diagnostics.StackTrace(x, true));
				System.Diagnostics.Debug.Assert(false, x.ToString());
			}
		}
	}

	private class WaitShutdownHook : java.lang.Thread
	{
		internal WaitShutdownHook()
			: base("WaitShutdownHook")
		{
		}

		public override void run()
		{
			Console.Error.WriteLine("IKVM runtime terminated. Waiting for Ctrl+C...");
			for(;;)
			{
				System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
			}
		}
	}

	[STAThread]	// NOTE this is here because otherwise SWT's RegisterDragDrop (a COM thing) doesn't work
	[IKVM.Attributes.HideFromJava]
	static int Main(string[] args)
	{
		Tracer.EnableTraceConsoleListener();
		Tracer.EnableTraceForDebug();
		System.Collections.Hashtable props = new System.Collections.Hashtable();
		string classpath = Environment.GetEnvironmentVariable("CLASSPATH");
		if(classpath == null || classpath == "")
		{
			classpath = ".";
		}
		props["java.class.path"] = classpath;
		bool jar = false;
		bool saveAssembly = false;
		bool saveAssemblyX = false;
		bool waitOnExit = false;
		bool showVersion = false;
		string mainClass = null;
		int vmargsIndex = -1;
        bool debug = false;
        String debugArg = null;
		bool noglobbing = false;
		for(int i = 0; i < args.Length; i++)
		{
            String arg = args[i];
			if(arg[0] == '-')
			{
				if(arg == "-help" || arg == "-?")
				{
					PrintHelp();
					return 1;
				}
				else if(arg == "-X")
				{
					PrintXHelp();
					return 1;
				}
				else if(arg == "-Xsave")
				{
					saveAssembly = true;
					IKVM.Internal.Starter.PrepareForSaveDebugImage();
				}
				else if(arg == "-XXsave")
				{
					saveAssemblyX = true;
					IKVM.Internal.Starter.PrepareForSaveDebugImage();
				}
				else if(arg == "-Xtime")
				{
					new Timer();
				}
				else if(arg == "-Xwait")
				{
					waitOnExit = true;
				}
				else if(arg == "-Xbreak")
				{
					System.Diagnostics.Debugger.Break();
				}
				else if(arg == "-Xnoclassgc")
				{
					IKVM.Internal.Starter.ClassUnloading = false;
				}
				else if(arg == "-Xverify")
				{
					IKVM.Internal.Starter.RelaxedVerification = false;
				}
				else if(arg == "-jar")
				{
					jar = true;
				}
				else if(arg == "-version")
				{
					Console.WriteLine(Startup.getVersionAndCopyrightInfo());
					Console.WriteLine();
					Console.WriteLine("CLR version: {0} ({1} bit)", Environment.Version, IntPtr.Size * 8);
					System.Type type = System.Type.GetType("Mono.Runtime");
					if(type != null)
					{
						Console.WriteLine("Mono version: {0}", type.InvokeMember("GetDisplayName", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic, null, null, new object[0]));
					}
					foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
					{
						Console.WriteLine("{0}: {1}", asm.GetName().Name, asm.GetName().Version);
					}
					string ver = java.lang.System.getProperty("openjdk.version");
					if(ver != null)
					{
						Console.WriteLine("OpenJDK version: {0}", ver);
					}
					return 0;
				}
				else if(arg == "-showversion")
				{
					showVersion = true;
				}
				else if(arg.StartsWith("-D"))
				{
				    arg = arg.Substring(2);
                    string[] keyvalue = arg.Split('=');
				    string value;
					if(keyvalue.Length == 2) // -Dabc=x
					{
                        value = keyvalue[1];
					} 
                    else if (keyvalue.Length == 1) // -Dabc
                    {
                        value = "";
                    } 
                    else // -Dabc=x=y
					{
                        value = arg.Substring(keyvalue[0].Length + 1);
					}
                    props[keyvalue[0]] = value;
				}
				else if(arg == "-ea" || arg == "-enableassertions")
				{
					IKVM.Runtime.Assertions.EnableAssertions();
				}
				else if(arg == "-da" || arg == "-disableassertions")
				{
					IKVM.Runtime.Assertions.DisableAssertions();
				}
				else if(arg == "-esa" || arg == "-enablesystemassertions")
				{
					IKVM.Runtime.Assertions.EnableSystemAssertions();
				}
				else if(arg == "-dsa" || arg == "-disablesystemassertions")
				{
					IKVM.Runtime.Assertions.DisableSystemAssertions();
				}
				else if(arg.StartsWith("-ea:") || arg.StartsWith("-enableassertions:"))
				{
					IKVM.Runtime.Assertions.EnableAssertions(arg.Substring(arg.IndexOf(':') + 1));
				}
				else if(arg.StartsWith("-da:") || arg.StartsWith("-disableassertions:"))
				{
					IKVM.Runtime.Assertions.DisableAssertions(arg.Substring(arg.IndexOf(':') + 1));
				}
				else if(arg == "-cp" || arg == "-classpath")
				{
					props["java.class.path"] = args[++i];
				}
				else if(arg.StartsWith("-Xtrace:"))
				{
					Tracer.SetTraceLevel(arg.Substring(8));
				}
				else if(arg.StartsWith("-Xmethodtrace:"))
				{
					Tracer.HandleMethodTrace(arg.Substring(14));
				}
                else if(arg == "-Xdebug")
                {
                    debug = true;
                }
                else if (arg == "-Xnoagent")
                {
                    //ignore it, disable support for oldjdb
                }
                else if (arg.StartsWith("-Xrunjdwp") || arg.StartsWith("-agentlib:jdwp"))
                {
                    debugArg = arg;
                    debug = true;
                }
                else if (arg.StartsWith("-Xreference:"))
                {
                    Startup.addBootClassPathAssembly(Assembly.LoadFrom(arg.Substring(12)));
                }
                else if (arg == "-Xnoglobbing")
                {
                    noglobbing = true;
                }
                else if (arg == "-XX:+AllowNonVirtualCalls")
                {
                    IKVM.Internal.Starter.AllowNonVirtualCalls = true;
                }
                else if (arg.StartsWith("-Xms")
                    || arg.StartsWith("-Xmx")
                    || arg.StartsWith("-Xmn")
                    || arg.StartsWith("-Xss")
                    || arg.StartsWith("-XX:")
                    || arg == "-Xmixed"
                    || arg == "-Xint"
                    || arg == "-Xincgc"
                    || arg == "-Xbatch"
                    || arg == "-Xfuture"
                    || arg == "-Xrs"
                    || arg == "-Xcheck:jni"
                    || arg == "-Xshare:off"
                    || arg == "-Xshare:auto"
                    || arg == "-Xshare:on"
                    )
                {
                    Console.Error.WriteLine("Unsupported option ignored: {0}", arg);
                }
                else
                {
                    Console.Error.WriteLine("{0}: illegal argument", arg);
                    break;
                }
			}
			else
			{
				mainClass = arg;
				vmargsIndex = i + 1;
				break;
			}
		}
		if(mainClass == null || showVersion)
		{
			Console.Error.WriteLine(Startup.getVersionAndCopyrightInfo());
			Console.Error.WriteLine();
		}
		if(mainClass == null)
		{
			PrintHelp();
			return 1;
		}
		try
		{
            if (debug)
            {
                // Starting the debugger
                Assembly asm = Assembly.GetExecutingAssembly();
                String arguments = debugArg + " -pid:" + System.Diagnostics.Process.GetCurrentProcess().Id;
                String program = new FileInfo(asm.Location).DirectoryName + "\\debugger.exe";
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo(program, arguments);
                    info.UseShellExecute = false;
                    Process debugger = new Process();
                    debugger.StartInfo = info;
                    debugger.Start();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(program + " " + arguments);
                    throw ex;
                }
            }
			if(jar)
			{
				props["java.class.path"] = mainClass;
			}
			// like the JDK we don't quote the args (even if they contain spaces)
			props["sun.java.command"] = String.Join(" ", args, vmargsIndex - 1, args.Length - (vmargsIndex - 1));
			props["sun.java.launcher"] = "SUN_STANDARD";
			Startup.setProperties(props);
			Startup.enterMainThread();
			string[] vmargs;
			if (noglobbing)
			{
				vmargs = new string[args.Length - vmargsIndex];
				System.Array.Copy(args, vmargsIndex, vmargs, 0, vmargs.Length);
			}
			else
			{
				// Startup.glob() uses Java code, so we need to do this after we've initialized
				vmargs = Startup.glob(args, vmargsIndex);
			}
			try
			{
				java.lang.Class clazz = sun.launcher.LauncherHelper.checkAndLoadMain(true, jar ? 2 : 1, mainClass);
				// we don't need to do any checking on the main method, as that was already done by checkAndLoadMain
				Method method = clazz.getMethod("main", typeof(string[]));
				// if clazz isn't public, we can still call main
				method.setAccessible(true);
				if(saveAssembly)
				{
					java.lang.Runtime.getRuntime().addShutdownHook(new SaveAssemblyShutdownHook(clazz));
				}
				if(waitOnExit)
				{
					java.lang.Runtime.getRuntime().addShutdownHook(new WaitShutdownHook());
				}
				try
				{
					method.invoke(null, new object[] { vmargs });
					return 0;
				}
				catch(InvocationTargetException x)
				{
					throw x.getCause();
				}
			}
			finally
			{
				if(saveAssemblyX)
				{
					IKVM.Internal.Starter.SaveDebugImage();
				}
			}
		}
		catch(System.Exception x)
		{
			java.lang.Thread thread = java.lang.Thread.currentThread();
			thread.getThreadGroup().uncaughtException(thread, ikvm.runtime.Util.mapException(x));
		}
		finally
		{
			Startup.exitMainThread();
		}
		return 1;
	}

	private static void PrintHelp()
	{
		Console.Error.WriteLine("usage: ikvm [-options] <class> [args...]");
		Console.Error.WriteLine("          (to execute a class)");
		Console.Error.WriteLine("    or ikvm -jar [-options] <jarfile> [args...]");
		Console.Error.WriteLine("          (to execute a jar file)");
		Console.Error.WriteLine();
		Console.Error.WriteLine("where options include:");
		Console.Error.WriteLine("    -? -help          Display this message");
		Console.Error.WriteLine("    -X                Display non-standard options");
		Console.Error.WriteLine("    -version          Display IKVM and runtime version");
		Console.Error.WriteLine("    -showversion      Display version and continue running");
		Console.Error.WriteLine("    -cp -classpath <directories and zip/jar files separated by {0}>", Path.PathSeparator);
		Console.Error.WriteLine("                      Set search path for application classes and resources");
		Console.Error.WriteLine("    -D<name>=<value>  Set a system property");
		Console.Error.WriteLine("    -ea[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("    -enableassertions[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("                      Enable assertions.");
		Console.Error.WriteLine("    -da[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("    -disableassertions[:<packagename>...|:<classname>]");
		Console.Error.WriteLine("                      Disable assertions");
	}

	private static void PrintXHelp()
	{
		Console.Error.WriteLine("    -Xsave            Save the generated assembly (for troubleshooting)");
		Console.Error.WriteLine("    -Xtime            Time the execution");
		Console.Error.WriteLine("    -Xtrace:<string>  Displays all tracepoints with the given name");
		Console.Error.WriteLine("    -Xmethodtrace:<string>");
		Console.Error.WriteLine("                      Builds method trace into the specified output methods");
		Console.Error.WriteLine("    -Xwait            Keep process hanging around after exit");
		Console.Error.WriteLine("    -Xbreak           Trigger a user defined breakpoint at startup");
		Console.Error.WriteLine("    -Xnoclassgc       Disable class garbage collection");
		Console.Error.WriteLine("    -Xnoglobbing      Disable argument globbing");
		Console.Error.WriteLine("    -Xverify          Enable strict class file verification");
		Console.Error.WriteLine();
		Console.Error.WriteLine("The -X options are non-standard and subject to change without notice.");
		Console.Error.WriteLine();
	}
}
