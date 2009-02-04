/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007, 2008 Jeroen Frijters

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
using System.Collections.Generic;
using java.util.zip;
using java.lang.reflect;

public class NetExp
{
	private static int zipCount;
	private static ZipOutputStream zipFile;
	private static Dictionary<string, NetExp> done = new Dictionary<string, NetExp>();
	private static Dictionary<string, java.lang.Class> todo = new Dictionary<string, java.lang.Class>();
	private static FileInfo file;

	public static int Main(string[] args)
	{
		IKVM.Internal.Tracer.EnableTraceConsoleListener();
		IKVM.Internal.Tracer.EnableTraceForDebug();
		string assemblyNameOrPath = null;
		bool continueOnError = false;
		foreach(string s in args)
		{
			if(s.StartsWith("-") || assemblyNameOrPath != null)
			{
				if(s == "-serialver")
				{
					java.lang.System.setProperty("ikvm.stubgen.serialver", "true");
				}
				else if(s == "-skiperror")
				{
					continueOnError = true;
				}
				else if(s.StartsWith("-r:") || s.StartsWith("-reference:"))
				{
					string path = s.Substring(s.IndexOf(':') + 1);
					try
					{
						Assembly.ReflectionOnlyLoadFrom(path);
					}
					catch (Exception x)
					{
						Console.Error.WriteLine("Error: unable to load reference {0}", path);
						Console.Error.WriteLine("    ({0})", x.Message);
						return 1;
					}
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
			Console.Error.WriteLine("usage: ikvmstub [-serialver] [-skiperror] <assemblyNameOrPath>");
			return 1;
		}
		Assembly assembly = null;
		try
		{
			file = new FileInfo(assemblyNameOrPath);
		}
		catch(System.Exception x)
		{
			Console.Error.WriteLine("Error: unable to load \"{0}\"\n  {1}", assemblyNameOrPath, x.Message);
			return 1;
		}
		if(file != null && file.Exists)
		{
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_ReflectionOnlyAssemblyResolve);
			assembly = Assembly.ReflectionOnlyLoadFrom(assemblyNameOrPath);
		}
		else
		{
#pragma warning disable 618
			// Assembly.LoadWithPartialName is obsolete
			assembly = Assembly.LoadWithPartialName(assemblyNameOrPath);
#pragma warning restore
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
						rc = ProcessAssembly(assembly, continueOnError);
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
						
						if (!continueOnError)
						{
							Console.Error.WriteLine("Warning: Assembly reflection encountered an error. Resultant JAR may be incomplete.");
						}
						
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
		return rc;
	}

	private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
	{
		foreach(Assembly a in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
		{
			if(args.Name.StartsWith(a.GetName().Name + ", "))
			{
				return a;
			}
		}
		string path = args.Name;
		int index = path.IndexOf(',');
		if(index > 0)
		{
			path = path.Substring(0, index);
		}
		path = file.DirectoryName + Path.DirectorySeparatorChar + path + ".dll";
		if(File.Exists(path))
		{
			return Assembly.ReflectionOnlyLoadFrom(path);
		}
		return Assembly.ReflectionOnlyLoad(args.Name);
	}

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

	private static int ProcessAssembly(Assembly assembly, bool continueOnError)
	{
		int rc = 0;
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
				if(assembly.ReflectionOnly)
				{
					c = ikvm.runtime.Util.getFriendlyClassFromType(t);
				}
				else
				{
					c = ikvm.runtime.Util.getClassFromTypeHandle(t.TypeHandle);
				}
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
			foreach(java.lang.Class c in new List<java.lang.Class>(todo.Values))
			{
				if(!done.ContainsKey(c.getName()))
				{
					keepGoing = true;
					done.Add(c.getName(), null);
					
					try
					{
						ProcessClass(c);
					}
					catch (Exception x)
					{
						if (continueOnError)
						{
							rc = 1;
							java.lang.Throwable.instancehelper_printStackTrace(ikvm.runtime.Util.mapException(x));
						}
						else
						{
							throw;
						}
					}
					WriteClass(c);
				}
			}
		} while(keepGoing);
		return rc;
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
		System.Type t = ikvm.runtime.Util.getInstanceTypeFromClass(c);
		while(t == null && c.getDeclaringClass() != null)
		{
			// dynamic only inner class, so we look at the declaring class
			c = c.getDeclaringClass();
			t = ikvm.runtime.Util.getInstanceTypeFromClass(c);
		}
		return t.IsGenericType;
	}

	private static bool IsNonVectorArray(java.lang.Class c)
	{
		System.Type t = ikvm.runtime.Util.getInstanceTypeFromClass(c);
		return t.IsArray && !c.isArray();
	}

	private static void AddToExportListIfNeeded(java.lang.Class c)
	{
		if(IsGenericType(c) || IsNonVectorArray(c) || (c.getModifiers() & Modifier.PUBLIC) == 0)
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
}
