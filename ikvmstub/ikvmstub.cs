/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007 Jeroen Frijters

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
using System.Reflection;
using System.IO;
using System.Collections;
using java.util.zip;
using java.lang.reflect;

public class NetExp
{
	private static int zipCount;
	private static ZipOutputStream zipFile;
	private static Hashtable done = new Hashtable();
	private static Hashtable todo = new Hashtable();
	private static FileInfo file;

	public static void Main(string[] args)
	{
		IKVM.Internal.Tracer.EnableTraceForDebug();
		string assemblyNameOrPath = null;
		foreach(string s in args)
		{
			if(s.StartsWith("-") || assemblyNameOrPath != null)
			{
				if(s == "-serialver")
				{
					java.lang.System.setProperty("ikvm.stubgen.serialver", "true");
				}
				else
				{
					// unrecognized option, or multiple assemblies, print usage message and exit
					assemblyNameOrPath = null;
					break;
				}
			}
			else
			{
				assemblyNameOrPath = s;
			}
		}
		if(assemblyNameOrPath == null)
		{
			Console.Error.WriteLine(ikvm.runtime.Startup.getVersionAndCopyrightInfo());
			Console.Error.WriteLine();
			Console.Error.WriteLine("usage: ikvmstub [-serialver] <assemblyNameOrPath>");
			return;
		}
		Assembly assembly = null;
		try
		{
			file = new FileInfo(assemblyNameOrPath);
		}
		catch(System.Exception x)
		{
			Console.Error.WriteLine("Error: unable to load \"{0}\"\n  {1}", assemblyNameOrPath, x.Message);
			return;
		}
		if(file != null && file.Exists)
		{
#if WHIDBEY
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
			assembly = Assembly.ReflectionOnlyLoadFrom(assemblyNameOrPath);
#else
			try
			{
				// If the same assembly can be found in the "Load" context, we prefer to use that
				// http://blogs.gotdotnet.com/suzcook/permalink.aspx/d5c5e14a-3612-4af1-a9b7-0a144c8dbf16
				// We use AssemblyName.FullName, because otherwise the assembly will be loaded in the
				// "LoadFrom" context using the path inside the AssemblyName object.
				assembly = Assembly.Load(AssemblyName.GetAssemblyName(assemblyNameOrPath).FullName);
				Console.Error.WriteLine("Warning: Assembly loaded from {0} instead", assembly.Location);
			}
			catch
			{
			}
			if(assembly == null)
			{
				// since we're loading the assembly in the LoadFrom context, we need to hook the AssemblyResolve event
				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
				assembly = Assembly.LoadFrom(assemblyNameOrPath);
			}
#endif
		}
		else
		{
			assembly = Assembly.LoadWithPartialName(assemblyNameOrPath);
		}
		int rc = 0;
		if(assembly == null)
		{
			Console.Error.WriteLine("Error: Assembly \"{0}\" not found", assemblyNameOrPath);
		}
		else
		{
			try
			{
				using (zipFile = new ZipOutputStream(new java.io.FileOutputStream(assembly.GetName().Name + ".jar")))
				{
					zipFile.setComment(ikvm.runtime.Startup.getVersionAndCopyrightInfo());
					try
					{
						ProcessAssembly(assembly);
					}
					catch (ReflectionTypeLoadException x)
					{
						Console.WriteLine(x);
						Console.WriteLine("LoaderExceptions:");
						foreach (Exception n in x.LoaderExceptions)
						{
							Console.WriteLine(n);
						}
					}
					catch (System.Exception x)
					{
						java.lang.Throwable.instancehelper_printStackTrace(ikvm.runtime.Util.mapException(x));
						rc = 1;
					}
				}
			}
			catch (ZipException x)
			{
				rc = 1;
				if (zipCount == 0)
				{
					Console.Error.WriteLine("Error: Assembly contains no public IKVM.NET compatible types");
				}
				else
				{
					Console.Error.WriteLine("Error: {0}", x.Message);
				}
			}
		}
		// FXBUG if we run a static initializer that starts a thread, we would never end,
		// so we force an exit here
		Environment.Exit(rc);
	}

#if WHIDBEY
	private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
	{
		foreach(Assembly a in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
		{
			if(args.Name.StartsWith(a.GetName().Name + ", "))
			{
				return a;
			}
		}
		Assembly asm = Assembly.ReflectionOnlyLoad(args.Name);
		if(asm != null)
		{
			return asm;
		}
		string path = args.Name;
		int index = path.IndexOf(',');
		if(index > 0)
		{
			path = path.Substring(0, index);
		}
		path = file.DirectoryName + Path.DirectorySeparatorChar + path + ".dll";
		Console.WriteLine("Loading referenced assembly: " + path);
		return Assembly.ReflectionOnlyLoadFrom(path);
	}
#endif

	private static void WriteClass(java.lang.Class c)
	{
		string name = c.getName().Replace('.', '/');
		java.io.InputStream inp = c.getResourceAsStream("/" + name + ".class");
		if(inp == null)
		{
			Console.Error.WriteLine("Class {0} not found", name);
			return;
		}
		byte[] buf = new byte[inp.available()];
		if(inp.read(buf) != buf.Length || inp.read() != -1)
		{
			throw new NotImplementedException();
		}
		zipCount++;
		zipFile.putNextEntry(new ZipEntry(name + ".class"));
		zipFile.write(buf, 0, buf.Length);
	}

	private static void ProcessAssembly(Assembly assembly)
	{
		foreach(System.Type t in assembly.GetTypes())
		{
			if(t.IsPublic)
			{
				java.lang.Class c;
				// NOTE we use getClassFromTypeHandle instead of getFriendlyClassFromType, to make sure
				// we don't get the remapped types when we're processing System.Object, System.String,
				// System.Throwable and System.IComparable.
				// NOTE we can't use getClassFromTypeHandle for ReflectionOnly assemblies
				// (because Type.TypeHandle is not supported by ReflectionOnly types), but this
				// isn't a problem because mscorlib is never loaded in the ReflectionOnly context.
#if WHIDBEY
				if(assembly.ReflectionOnly)
				{
					c = ikvm.runtime.Util.getFriendlyClassFromType(t);
				}
				else
				{
					c = ikvm.runtime.Util.getClassFromTypeHandle(t.TypeHandle);
				}
#else
				c = ikvm.runtime.Util.getClassFromTypeHandle(t.TypeHandle);
#endif
				if(c != null)
				{
					AddToExportList(c);
				}
			}
		}
		bool keepGoing;
		do
		{
			keepGoing = false;
			foreach(java.lang.Class c in new ArrayList(todo.Values))
			{
				if(!done.ContainsKey(c.getName()))
				{
					keepGoing = true;
					done.Add(c.getName(), null);
					ProcessClass(c);
					WriteClass(c);
				}
			}
		} while(keepGoing);
	}

	private static void AddToExportList(java.lang.Class c)
	{
		while(c.isArray())
		{
			c = c.getComponentType();
		}
		todo[c.getName()] = c;
	}

	private static bool IsGenericType(java.lang.Class c)
	{
#if WHIDBEY
		System.Type t = ikvm.runtime.Util.getInstanceTypeFromClass(c);
		while(t == null && c.getDeclaringClass() != null)
		{
			// dynamic only inner class, so we look at the declaring class
			c = c.getDeclaringClass();
			t = ikvm.runtime.Util.getInstanceTypeFromClass(c);
		}
		return t.IsGenericType;
#else
		return c.getName().IndexOf("$$0060") > 0;
#endif
	}

	private static void AddToExportListIfNeeded(java.lang.Class c)
	{
		if(IsGenericType(c) || (c.getModifiers() & Modifier.PUBLIC) == 0)
		{
			AddToExportList(c);
		}
	}

	private static void AddToExportListIfNeeded(java.lang.Class[] classes)
	{
		foreach(java.lang.Class c in classes)
		{
			AddToExportListIfNeeded(c);
		}
	}

	private static void ProcessClass(java.lang.Class c)
	{
		java.lang.Class superclass = c.getSuperclass();
		if(superclass != null)
		{
			AddToExportListIfNeeded(c.getSuperclass());
		}
		foreach(java.lang.Class iface in c.getInterfaces())
		{
			AddToExportListIfNeeded(iface);
		}
		java.lang.Class outerClass = c.getDeclaringClass();
		if(outerClass != null)
		{
			AddToExportList(outerClass);
		}
		foreach(java.lang.Class innerClass in c.getDeclaredClasses())
		{
			int mods = innerClass.getModifiers();
			if((mods & (Modifier.PUBLIC | Modifier.PROTECTED)) != 0)
			{
				AddToExportList(innerClass);
			}
		}
		foreach(Constructor constructor in c.getDeclaredConstructors())
		{
			int mods = constructor.getModifiers();
			if((mods & (Modifier.PUBLIC | Modifier.PROTECTED)) != 0)
			{
				AddToExportListIfNeeded(constructor.getParameterTypes());
			}
		}
		foreach(Method method in c.getDeclaredMethods())
		{
			int mods = method.getModifiers();
			if((mods & (Modifier.PUBLIC | Modifier.PROTECTED)) != 0)
			{
				AddToExportListIfNeeded(method.getParameterTypes());
				AddToExportListIfNeeded(method.getReturnType());
			}
		}
		foreach(Field field in c.getDeclaredFields())
		{
			int mods = field.getModifiers();
			if((mods & (Modifier.PUBLIC | Modifier.PROTECTED)) != 0)
			{
				AddToExportListIfNeeded(field.getType());
			}
		}
	}

	private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
		{
			if(asm.FullName == args.Name)
			{
				return asm;
			}
		}
		return null;
	}
}
