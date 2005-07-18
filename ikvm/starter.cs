/*
  Copyright (C) 2002, 2003, 2004, 2005 Jeroen Frijters

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
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using IKVM.Internal;
using IKVM.Runtime;

using java.lang.reflect;
using java.net;
using java.util.jar;
using java.io;

public class Starter
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

	public class PathClassLoader : URLClassLoader
	{
		private static URL[] GetClassPath(string classpath)
		{
			string[] s = classpath.Split(Path.PathSeparator);
			URL[] urls = new URL[s.Length];
			for(int i = 0; i < urls.Length; i++)
			{
				// TODO non-existing file/dir is treated as current directory, this obviously isn't correct
				urls[i] = new java.io.File(s[i]).toURL();
			}
			return urls;
		}

		public PathClassLoader(string classpath, java.lang.ClassLoader parent)
			: base(GetClassPath(classpath), parent)
		{
		}

		protected override java.lang.Class loadClass(string name, bool resolve)
		{
			java.lang.Class c = findClass(name);
			if(resolve)
			{
				resolveClass(c);
			}
			return c;
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
			Console.Error.WriteLine("Saving dynamic assembly...");
			try
			{
				JVM.SaveDebugImage(clazz);
				Console.Error.WriteLine("Saving done.");
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
		Tracer.EnableTraceForDebug();
		Hashtable props = new Hashtable();
		bool jar = false;
		bool saveAssembly = false;
		bool saveAssemblyX = false;
		bool waitOnExit = false;
		string mainClass = null;
		string[] vmargs = null;
		string bootclasspath = null;
		for(int i = 0; i < args.Length; i++)
		{
			if(args[i][0] == '-')
			{
				if(args[i] == "-help" || args[i] == "-?")
				{
					break;
				}
				else if(args[i] == "-Xsave")
				{
					saveAssembly = true;
					JVM.PrepareForSaveDebugImage();
				}
				else if(args[i] == "-XXsave")
				{
					saveAssemblyX = true;
					JVM.PrepareForSaveDebugImage();
				}
				else if(args[i] == "-Xtime")
				{
					new Timer();
				}
				else if(args[i] == "-Xwait")
				{
					waitOnExit = true;
				}
				else if(args[i] == "-Xbreak")
				{
					System.Diagnostics.Debugger.Break();
				}
				else if(args[i] == "-jar")
				{
					jar = true;
				}
				else if(args[i] == "-version")
				{
					Console.WriteLine("CLR version: {0} ({1} bit)", Environment.Version, IntPtr.Size * 8);
					foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
					{
						Console.WriteLine("{0}: {1}", asm.GetName().Name, asm.GetName().Version);
					}
					Console.WriteLine("GNU Classpath version: {0}", java.lang.System.getProperty("gnu.classpath.version"));
					return 0;
				}
				else if(args[i].StartsWith("-D"))
				{
					string[] keyvalue = args[i].Substring(2).Split('=');
					if(keyvalue.Length != 2)
					{
						keyvalue = new string[] { keyvalue[0], "" };
					}
					props[keyvalue[0]] = keyvalue[1];
				}
				else if(args[i] == "-ea" || args[i] == "-enableassertions")
				{
					props["ikvm.assert.default"] = "true";
				}
				else if(args[i] == "-da" || args[i] == "-disableassertions")
				{
					props["ikvm.assert.default"] = "false";
				}
				else if(args[i].StartsWith("-ea:") || args[i].StartsWith("-enableassertions:"))
				{
					props["ikvm.assert.enable"] = args[i].Substring(args[i].IndexOf(':') + 1);
				}
				else if(args[i].StartsWith("-da:") || args[i].StartsWith("-disableassertions:"))
				{
					props["ikvm.assert.disable"] = args[i].Substring(args[i].IndexOf(':') + 1);
				}
				else if(args[i] == "-cp" || args[i] == "-classpath")
				{
					props["java.class.path"] = args[++i];
				}
				else if(args[i].StartsWith("-Xbootclasspath:"))
				{
					bootclasspath = args[i].Substring(16);
				}
				else if(args[i].StartsWith("-Xtrace:"))
				{
					Tracer.SetTraceLevel(args[i].Substring(8));
				}
				else if(args[i].StartsWith("-Xmethodtrace:"))
				{
					Tracer.HandleMethodTrace(args[i].Substring(14));
				}
				else if(args[i].StartsWith("-Xms")
					|| args[i].StartsWith("-Xmx")
					|| args[i].StartsWith("-Xss")
					|| args[i] == "-Xmixed"
					|| args[i] == "-Xint"
					|| args[i] == "-Xnoclassgc"
					|| args[i] == "-Xincgc"
					|| args[i] == "-Xbatch"
					|| args[i] == "-Xfuture"
					|| args[i] == "-Xrs"
					|| args[i] == "-Xcheck:jni"
					|| args[i] == "-Xshare:off"
					|| args[i] == "-Xshare:auto"
					|| args[i] == "-Xshare:on"
					)
				{
					Console.Error.WriteLine("Unsupported option ignored: {0}", args[i]);
				}
				else
				{
					Console.Error.WriteLine("{0}: illegal argument", args[i]);
					break;
				}
			}
			else
			{
				mainClass = args[i];
				vmargs = Startup.Glob(i + 2);
				break;
			}
		}
		if(mainClass == null)
		{
			Console.Error.WriteLine("usage: ikvm [-options] <class> [args...]");
			Console.Error.WriteLine("          (to execute a class)");
			Console.Error.WriteLine("    or ikvm -jar [-options] <jarfile> [args...]");
			Console.Error.WriteLine("          (to execute a jar file)");
			Console.Error.WriteLine();
			Console.Error.WriteLine("where options include:");
			Console.Error.WriteLine("    -? -help          Display this message");
			Console.Error.WriteLine("    -version          Display IKVM and runtime version");
			Console.Error.WriteLine("    -cp -classpath <directories and zip/jar files separated by {0}>", Path.PathSeparator);
			Console.Error.WriteLine("                      Set search path for application classes and resources");
			Console.Error.WriteLine("    -D<name>=<value>  Set a system property");
			Console.Error.WriteLine("    -ea[:<packagename>...|:<classname>]");
			Console.Error.WriteLine("    -enableassertions[:<packagename>...|:<classname>]");
			Console.Error.WriteLine("                      Enable assertions.");
			Console.Error.WriteLine("    -da[:<packagename>...|:<classname>]");
			Console.Error.WriteLine("    -disableassertions[:<packagename>...|:<classname>]");
			Console.Error.WriteLine("                      Disable assertions");
			Console.Error.WriteLine("    -Xsave            Save the generated assembly (for debugging)");
			Console.Error.WriteLine("    -Xtime            Time the execution");
			Console.Error.WriteLine("    -Xbootclasspath:<directories and zip/jar files separated by {0}>", Path.PathSeparator);
			Console.Error.WriteLine("                      Set search path for bootstrap classes and resources");
			Console.Error.WriteLine("    -Xtrace:<string>  Displays all tracepoints with the given name");
			Console.Error.WriteLine("    -Xmethodtrace:<string>");
			Console.Error.WriteLine("                      Builds method trace into the specified output methods");
			Console.Error.WriteLine("    -Xwait            Keep process hanging around after exit");
			Console.Error.WriteLine("    -Xbreak           Trigger a user defined breakpoint at startup");
			return 1;
		}
		try
		{
			if(jar)
			{
				props["java.class.path"] = mainClass;
			}
			Startup.SetProperties(props);
			Startup.EnterMainThread();
			if(jar)
			{
				JarFile jf = new JarFile(mainClass);
				try
				{
					mainClass = jf.getManifest().getMainAttributes().getValue(Attributes.Name.MAIN_CLASS).Replace('/', '.');
				}
				finally
				{
					jf.close();
				}
				if(mainClass == null)
				{
					Console.Error.WriteLine("Manifest doesn't contain a Main-Class.");
					return 1;
				}
			}
			if(bootclasspath != null)
			{
				JVM.SetBootstrapClassLoader(new PathClassLoader(bootclasspath, null));
			}
			java.lang.Class clazz = java.lang.Class.forName(mainClass, true, java.lang.ClassLoader.getSystemClassLoader());
			Method method = FindMainMethod(clazz);
			if(method == null)
			{
				throw new java.lang.NoSuchMethodError("main");
			}
			else if(!Modifier.isPublic(method.getModifiers()))
			{
				Console.Error.WriteLine("Main method not public.");
			}
			else
			{
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
				finally
				{
					if(saveAssemblyX)
					{
						JVM.SaveDebugImage(clazz);
					}
				}
			}
		}
		catch(System.Exception x)
		{
			java.lang.Thread thread = java.lang.Thread.currentThread();
			thread.getThreadGroup().uncaughtException(thread, IKVM.Runtime.Util.MapException(x));
		}
		finally
		{
			Startup.ExitMainThread();
		}
		return 1;
	}

	private static Method FindMainMethod(java.lang.Class clazz)
	{
		// HACK without this hack, clazz.getDeclaredMethods would throw a NoClassDefFoundError if any
		// of the methods in the class had an unloadable parameter type, but we don't want that.
		JVM.EnableReflectionOnMethodsWithUnloadableTypeParameters = true;
		try
		{
			while(clazz != null)
			{
				foreach(Method m in clazz.getDeclaredMethods())
				{
					if(m.getName() == "main" && m.getReturnType() == java.lang.Void.TYPE)
					{
						java.lang.Class[] parameters = m.getParameterTypes();
						if(parameters.Length == 1 && parameters[0] == java.lang.Class.forName("[Ljava.lang.String;"))
						{
							return m;
						}
					}
				}
				clazz = clazz.getSuperclass();
			}
			return null;
		}
		finally
		{
			JVM.EnableReflectionOnMethodsWithUnloadableTypeParameters = false;
		}
	}
}
