/*
  Copyright (C) 2002 Jeroen Frijters

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
using System.Text;

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

	private class ExtClassLoader : URLClassLoader
	{
		private static URL[] GetClassPath()
		{
			string classpath = java.lang.System.getProperty("java.ext.dirs", "");
			string[] s = classpath.Split(';');
			ArrayList jars = new ArrayList();
			for(int i = 0; i < s.Length; i++)
			{
				try
				{
					string[] files = Directory.GetFiles(s[i]);
					for(int j = 0; j < files.Length; j++)
					{
						jars.Add(new java.io.File(files[j]).toURL());
					}
				}
				catch(ArgumentException)
				{
					// ignore any malformed components
				}
			}
			return (URL[])jars.ToArray(typeof(URL));
		}

		internal ExtClassLoader(java.lang.ClassLoader parent)
			: base(GetClassPath(), parent)
		{
		}
	}

	public class AppClassLoader : URLClassLoader
	{
		private static URL[] GetClassPath()
		{
			string classpath = java.lang.System.getProperty("java.class.path", ".");
			string[] s = classpath.Split(';');
			URL[] urls = new URL[s.Length];
			for(int i = 0; i < urls.Length; i++)
			{
				// TODO non-existing file/dir is treated as current directory, this obviously isn't correct
				urls[i] = new java.io.File(s[i]).toURL();
			}
			return urls;
		}

		public AppClassLoader(java.lang.ClassLoader parent)
			: base(GetClassPath(), new ExtClassLoader(parent))
		{
		}
	}

	public class PathClassLoader : URLClassLoader
	{
		private static URL[] GetClassPath(string classpath)
		{
			string[] s = classpath.Split(';');
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

	[StackTraceInfo(Hidden = true, EatFrames = 1)]
	[STAThread]	// NOTE this is here because otherwise SWT's RegisterDragDrop (a COM thing) doesn't work
	static int Main(string[] args)
	{
		System.Threading.Thread.CurrentThread.Name = "main";
		bool jar = false;
		bool saveAssembly = false;
		string mainClass = null;
		string[] vmargs = null;
		for(int i = 0; i < args.Length; i++)
		{
			if(args[i][0] == '-')
			{
				if(args[i] == "-help")
				{
					break;
				}
				else if(args[i] == "-save")
				{
					saveAssembly = true;
				}
				else if(args[i] == "-time")
				{
					new Timer();
				}
				else if(args[i] == "-jar")
				{
					jar = true;
				}
				else if(args[i].StartsWith("-D"))
				{
					string[] keyvalue = args[i].Substring(2).Split('=');
					if(keyvalue.Length != 2)
					{
						keyvalue = new string[] { keyvalue[0], "" };
					}
					java.lang.System.setProperty(keyvalue[0], keyvalue[1]);
				}
				else if(args[i] == "-cp" || args[i] == "-classpath")
				{
					java.lang.System.setProperty("java.class.path", args[++i]);
				}
				else if(args[i].StartsWith("-Xbootclasspath:"))
				{
					java.lang.System.setProperty("ikvm.boot.class.path", args[i].Substring(16));
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
				vmargs = new string[args.Length - (i + 1)];
				System.Array.Copy(args, i + 1, vmargs, 0, vmargs.Length);
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
			Console.Error.WriteLine("    -help             display this message");
			Console.Error.WriteLine("    -cp -classpath <directories and zip/jar files separated by ;>");
			Console.Error.WriteLine("                      set search path for application classes and resources");
			Console.Error.WriteLine("    -save             save the generated assembly (for debugging)");
			Console.Error.WriteLine("    -time             time the execution");
			Console.Error.WriteLine("    -D<name>=<value>  set a system property");
			Console.Error.WriteLine("    -Xbootclasspath:<directories and zip/jar files separated by ;>");
			Console.Error.WriteLine("                      set search path for bootstrap classes and resources");
			return 1;
		}
		try
		{
			// HACK we take our own assembly location as the location of classpath (this is used by the Security infrastructure
			// to find the classpath.security file)
			java.lang.System.setProperty("gnu.classpath.home", new System.IO.FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName);
			if(jar)
			{
				// TODO if there is no classpath, we're adding the current directory, but is this correct when running a jar?
				java.lang.System.setProperty("java.class.path", mainClass + ";" + java.lang.System.getProperty("java.class.path"));
				JarFile jf = new JarFile(mainClass);
				try
				{
					// TODO use Attributes.Name.MAIN_CLASS (we don't support inner classes at the moment)
					mainClass = jf.getManifest().getMainAttributes().getValue("Main-Class");
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
			// NOTE we should use the default systemclassloader (gnu.java.lang.SystemClassLoader),
			// but at the moment it is broken (doesn't implement findClass())
			java.lang.System.setProperty("java.system.class.loader", typeof(AppClassLoader).AssemblyQualifiedName);
			java.lang.ClassLoader loader = java.lang.ClassLoader.getSystemClassLoader();
			string bootclasspath = java.lang.System.getProperty("ikvm.boot.class.path", "");
			if(bootclasspath != "")
			{
				StringBuilder sb = new StringBuilder();
				foreach(string part in bootclasspath.Split(';'))
				{
					if(part.ToUpper().EndsWith(".DLL"))
					{
						Assembly.Load(part.Substring(0, part.Length - 4));
					}
					else
					{
						if(sb.Length > 0)
						{
							sb.Append(';');
						}
						sb.Append(part);
					}
				}
				if(sb.Length > 0)
				{
					JVM.SetBootstrapClassLoader(new PathClassLoader(sb.ToString(), null));
				}
			}
			java.lang.Class clazz = loader.loadClass(mainClass);
			Method method = clazz.getMethod("main", new java.lang.Class[] { java.lang.Class.getClassFromType(typeof(string[])) });
			if(!Modifier.isPublic(method.getModifiers()))
			{
				Console.Error.WriteLine("Main method not public.");
				return 1;
			}
			try
			{
				try
				{
					method.invoke(null, new object[] { vmargs });
				}
				finally
				{
					if(saveAssembly)
					{
						// TODO it would be nice to wait for other threads to exit
						// TODO consider using a Shutdown hook!
						JVM.SaveDebugImage(clazz);
						saveAssembly = false;
					}
				}
			}
			catch(InvocationTargetException x)
			{
				throw x.getCause();
			}
			if(saveAssembly)
			{
				// TODO it would be nice to wait for other threads to exit
				// TODO consider using a Shutdown hook!
				JVM.SaveDebugImage(clazz);
			}
			return 0;
		}
		catch(System.Exception x)
		{
			java.lang.Thread thread = java.lang.Thread.currentThread();
			thread.getThreadGroup().uncaughtException(thread, ExceptionHelper.MapExceptionFast(x));
			return 1;
		}
	}
}
