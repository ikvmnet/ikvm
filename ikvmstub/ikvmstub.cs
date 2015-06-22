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
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using IKVM.Attributes;
using IKVM.Internal;
using IKVM.Reflection;
using Type = IKVM.Reflection.Type;

static class NetExp
{
	private static int zipCount;
	private static ZipOutputStream zipFile;
	private static Dictionary<string, string> done = new Dictionary<string, string>();
	private static Dictionary<string, TypeWrapper> todo = new Dictionary<string, TypeWrapper>();
	private static FileInfo file;
	private static bool includeSerialVersionUID;
	private static bool includeNonPublicInterfaces;
	private static bool includeNonPublicMembers;
	private static bool includeParameterNames;
	private static List<string> namespaces = new List<string>();

	static int Main(string[] args)
	{
		IKVM.Internal.Tracer.EnableTraceConsoleListener();
		IKVM.Internal.Tracer.EnableTraceForDebug();
		string assemblyNameOrPath = null;
		bool continueOnError = false;
		bool autoLoadSharedClassLoaderAssemblies = false;
		List<string> references = new List<string>();
		List<string> libpaths = new List<string>();
		bool nostdlib = false;
		bool bootstrap = false;
		string outputFile = null;
		bool forwarders = false;
		foreach(string s in args)
		{
			if(s.StartsWith("-") || assemblyNameOrPath != null)
			{
				if(s == "-serialver")
				{
					Console.Error.WriteLine("The -serialver option is deprecated and will be removed in the future. Use -japi instead.");
					includeSerialVersionUID = true;
				}
				else if(s == "-japi")
				{
					includeSerialVersionUID = true;
					includeNonPublicInterfaces = true;
					includeNonPublicMembers = true;
				}
				else if(s == "-skiperror")
				{
					continueOnError = true;
				}
				else if(s == "-shared")
				{
					autoLoadSharedClassLoaderAssemblies = true;
				}
				else if(s.StartsWith("-r:") || s.StartsWith("-reference:"))
				{
					references.Add(s.Substring(s.IndexOf(':') + 1));
				}
				else if(s == "-nostdlib")
				{
					nostdlib = true;
				}
				else if(s.StartsWith("-lib:"))
				{
					libpaths.Add(s.Substring(5));
				}
				else if(s == "-bootstrap")
				{
					bootstrap = true;
				}
				else if(s.StartsWith("-out:"))
				{
					outputFile = s.Substring(5);
				}
				else if(s.StartsWith("-namespace:"))
				{
					namespaces.Add(s.Substring(11) + ".");
				}
				else if(s == "-forwarders")
				{
					forwarders = true;
				}
				else if(s == "-parameters")
				{
					includeParameterNames = true;
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
			Console.Error.WriteLine(GetVersionAndCopyrightInfo());
			Console.Error.WriteLine();
			Console.Error.WriteLine("usage: ikvmstub [-options] <assemblyNameOrPath>");
			Console.Error.WriteLine();
			Console.Error.WriteLine("options:");
			Console.Error.WriteLine("    -out:<outputfile>          Specify the output filename");
			Console.Error.WriteLine("    -reference:<filespec>      Reference an assembly (short form -r:<filespec>)");
			Console.Error.WriteLine("    -japi                      Generate jar suitable for comparison with japitools");
			Console.Error.WriteLine("    -skiperror                 Continue when errors are encountered");
			Console.Error.WriteLine("    -shared                    Process all assemblies in shared group");
			Console.Error.WriteLine("    -nostdlib                  Do not reference standard libraries");
			Console.Error.WriteLine("    -lib:<dir>                 Additional directories to search for references");
			Console.Error.WriteLine("    -namespace:<ns>            Only include types from specified namespace");
			Console.Error.WriteLine("    -forwarders                Export forwarded types too");
			Console.Error.WriteLine("    -parameters                Emit Java 8 classes with parameter names");
			return 1;
		}
		if(File.Exists(assemblyNameOrPath) && nostdlib)
		{
			// Add the target assembly to the references list, to allow it to be considered as "mscorlib".
			// This allows "ikvmstub -nostdlib \...\mscorlib.dll" to work.
			references.Add(assemblyNameOrPath);
		}
		StaticCompiler.Resolver.Warning += new AssemblyResolver.WarningEvent(Resolver_Warning);
		StaticCompiler.Resolver.Init(StaticCompiler.Universe, nostdlib, references, libpaths);
		Dictionary<string, Assembly> cache = new Dictionary<string, Assembly>();
		foreach (string reference in references)
		{
			Assembly[] dummy = null;
			if (!StaticCompiler.Resolver.ResolveReference(cache, ref dummy, reference))
			{
				Console.Error.WriteLine("Error: reference not found {0}", reference);
				return 1;
			}
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
			assembly = StaticCompiler.LoadFile(assemblyNameOrPath);
		}
		else
		{
			assembly = StaticCompiler.Resolver.LoadWithPartialName(assemblyNameOrPath);
		}
		int rc = 0;
		if(assembly == null)
		{
			Console.Error.WriteLine("Error: Assembly \"{0}\" not found", assemblyNameOrPath);
		}
		else
		{
			if (bootstrap)
			{
				StaticCompiler.runtimeAssembly = StaticCompiler.LoadFile(typeof(NetExp).Assembly.Location);
				ClassLoaderWrapper.SetBootstrapClassLoader(new BootstrapBootstrapClassLoader());
			}
			else
			{
				StaticCompiler.LoadFile(typeof(NetExp).Assembly.Location);
				StaticCompiler.runtimeAssembly = StaticCompiler.LoadFile(Path.Combine(typeof(NetExp).Assembly.Location, "../IKVM.Runtime.dll"));
				JVM.CoreAssembly = StaticCompiler.LoadFile(Path.Combine(typeof(NetExp).Assembly.Location, "../IKVM.OpenJDK.Core.dll"));
			}
			if (AttributeHelper.IsJavaModule(assembly.ManifestModule))
			{
				Console.Error.WriteLine("Warning: Running ikvmstub on ikvmc compiled assemblies is not supported.");
			}
			if (outputFile == null)
			{
				outputFile = assembly.GetName().Name + ".jar";
			}
			try
			{
				using (zipFile = new ZipOutputStream(new FileStream(outputFile, FileMode.Create)))
				{
					zipFile.SetComment(GetVersionAndCopyrightInfo());
					try
					{
						List<Assembly> assemblies = new List<Assembly>();
						assemblies.Add(assembly);
						if (autoLoadSharedClassLoaderAssemblies)
						{
							LoadSharedClassLoaderAssemblies(assembly, assemblies);
						}
						foreach (Assembly asm in assemblies)
						{
							if (ProcessTypes(asm.GetTypes(), continueOnError) != 0)
							{
								rc = 1;
								if (!continueOnError)
								{
									break;
								}
							}
							if (forwarders && ProcessTypes(asm.ManifestModule.__GetExportedTypes(), continueOnError) != 0)
							{
								rc = 1;
								if (!continueOnError)
								{
									break;
								}
							}
						}
					}
					catch (System.Exception x)
					{
						Console.Error.WriteLine(x);
						
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

	static void Resolver_Warning(AssemblyResolver.WarningId warning, string message, string[] parameters)
	{
		if (warning != AssemblyResolver.WarningId.HigherVersion)
		{
			Console.Error.WriteLine("Warning: " + message, parameters);
		}
	}

	private static string GetVersionAndCopyrightInfo()
	{
		System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
		object[] desc = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false);
		if (desc.Length == 1)
		{
			object[] copyright = asm.GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
			if (copyright.Length == 1)
			{
				return string.Format("{0} version {1}{2}{3}{2}http://www.ikvm.net/",
					((System.Reflection.AssemblyTitleAttribute)desc[0]).Title,
					asm.GetName().Version,
					Environment.NewLine,
					((System.Reflection.AssemblyCopyrightAttribute)copyright[0]).Copyright);
			}
		}
		return "";
	}

	private static void LoadSharedClassLoaderAssemblies(Assembly assembly, List<Assembly> assemblies)
	{
		if (assembly.GetManifestResourceInfo("ikvm.exports") != null)
		{
			using (Stream stream = assembly.GetManifestResourceStream("ikvm.exports"))
			{
				BinaryReader rdr = new BinaryReader(stream);
				int assemblyCount = rdr.ReadInt32();
				for (int i = 0; i < assemblyCount; i++)
				{
					string name = rdr.ReadString();
					int typeCount = rdr.ReadInt32();
					if (typeCount > 0)
					{
						for (int j = 0; j < typeCount; j++)
						{
							rdr.ReadInt32();
						}
						try
						{
							assemblies.Add(StaticCompiler.Load(name));
						}
						catch
						{
							Console.WriteLine("Warning: Unable to load shared class loader assembly: {0}", name);
						}
					}
				}
			}
		}
	}

	private static void WriteClass(TypeWrapper tw)
	{
		zipCount++;
		MemoryStream mem = new MemoryStream();
		IKVM.StubGen.StubGenerator.WriteClass(mem, tw, includeNonPublicInterfaces, includeNonPublicMembers, includeSerialVersionUID, includeParameterNames);
		ZipEntry entry = new ZipEntry(tw.Name.Replace('.', '/') + ".class");
		entry.Size = mem.Position;
		zipFile.PutNextEntry(entry);
		mem.WriteTo(zipFile);
	}

	private static bool ExportNamespace(Type type)
	{
		if (namespaces.Count == 0)
		{
			return true;
		}
		string name = type.FullName;
		foreach (string ns in namespaces)
		{
			if (name.StartsWith(ns, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private static int ProcessTypes(Type[] types, bool continueOnError)
	{
		int rc = 0;
		foreach (Type t in types)
		{
			if (t.IsPublic
				&& ExportNamespace(t)
				&& !t.IsGenericTypeDefinition
				&& !AttributeHelper.IsHideFromJava(t)
				&& (!t.IsGenericType || !AttributeHelper.IsJavaModule(t.Module)))
			{
				TypeWrapper c;
				if (ClassLoaderWrapper.IsRemappedType(t) || t.IsPrimitive || t == Types.Void)
				{
					c = DotNetTypeWrapper.GetWrapperFromDotNetType(t);
				}
				else
				{
					c = ClassLoaderWrapper.GetWrapperFromType(t);
				}
				if (c != null)
				{
					AddToExportList(c);
				}
			}
		}
		bool keepGoing;
		do
		{
			keepGoing = false;
			foreach (TypeWrapper c in new List<TypeWrapper>(todo.Values))
			{
				if(!done.ContainsKey(c.Name))
				{
					keepGoing = true;
					done.Add(c.Name, null);
					
					try
					{
						ProcessClass(c);
						WriteClass(c);
					}
					catch (Exception x)
					{
						if (continueOnError)
						{
							rc = 1;
							Console.WriteLine(x);
						}
						else
						{
							throw;
						}
					}
				}
			}
		} while(keepGoing);
		return rc;
	}

	private static void AddToExportList(TypeWrapper c)
	{
		todo[c.Name] = c;
	}

	private static bool IsNonVectorArray(TypeWrapper tw)
	{
		return !tw.IsArray && tw.TypeAsBaseType.IsArray;
	}

	private static void AddToExportListIfNeeded(TypeWrapper tw)
	{
		while (tw.IsArray)
		{
			tw = tw.ElementTypeWrapper;
		}
		if (tw.IsUnloadable && tw.Name.StartsWith("Missing/"))
		{
			Console.Error.WriteLine("Error: unable to find assembly '{0}'", tw.Name.Substring(8));
			Environment.Exit(1);
			return;
		}
		if (tw is StubTypeWrapper)
		{
			// skip
		}
		else if ((tw.TypeAsTBD != null && tw.TypeAsTBD.IsGenericType) || IsNonVectorArray(tw) || !tw.IsPublic)
		{
			AddToExportList(tw);
		}
	}

	private static void AddToExportListIfNeeded(TypeWrapper[] types)
	{
		foreach (TypeWrapper tw in types)
		{
			AddToExportListIfNeeded(tw);
		}
	}

	private static void ProcessClass(TypeWrapper tw)
	{
		TypeWrapper superclass = tw.BaseTypeWrapper;
		if (superclass != null)
		{
			AddToExportListIfNeeded(superclass);
		}
		AddToExportListIfNeeded(tw.Interfaces);
		TypeWrapper outerClass = tw.DeclaringTypeWrapper;
		if (outerClass != null)
		{
			AddToExportList(outerClass);
		}
		foreach (TypeWrapper innerClass in tw.InnerClasses)
		{
			if (innerClass.IsPublic)
			{
				AddToExportList(innerClass);
			}
		}
		foreach (MethodWrapper mw in tw.GetMethods())
		{
			if (mw.IsPublic || mw.IsProtected)
			{
				mw.Link();
				AddToExportListIfNeeded(mw.ReturnType);
				AddToExportListIfNeeded(mw.GetParameters());
			}
		}
		foreach (FieldWrapper fw in tw.GetFields())
		{
			if (fw.IsPublic || fw.IsProtected)
			{
				fw.Link();
				AddToExportListIfNeeded(fw.FieldTypeWrapper);
			}
		}
	}
}

static class Intrinsics
{
	internal static bool IsIntrinsic(MethodWrapper methodWrapper)
	{
		return false;
	}
}

static class StaticCompiler
{
	internal static readonly Universe Universe = new Universe(UniverseOptions.EnableFunctionPointers);
	internal static readonly AssemblyResolver Resolver = new AssemblyResolver();
	internal static Assembly runtimeAssembly;

	internal static Type GetRuntimeType(string typeName)
	{
		return runtimeAssembly.GetType(typeName, true);
	}

	internal static Assembly LoadFile(string fileName)
	{
		return Resolver.LoadFile(fileName);
	}

	internal static Assembly Load(string name)
	{
		return Universe.Load(name);
	}
}

static class FakeTypes
{
	private static readonly Type genericType;

	class Holder<T> { }

	static FakeTypes()
	{
		genericType = StaticCompiler.Universe.Import(typeof(Holder<>));
	}

	internal static Type GetAttributeType(Type type)
	{
		return genericType.MakeGenericType(type);
	}

	internal static Type GetAttributeReturnValueType(Type type)
	{
		return genericType.MakeGenericType(type);
	}

	internal static Type GetAttributeMultipleType(Type type)
	{
		return genericType.MakeGenericType(type);
	}

	internal static Type GetDelegateType(Type type)
	{
		return genericType.MakeGenericType(type);
	}

	internal static Type GetEnumType(Type type)
	{
		return genericType.MakeGenericType(type);
	}
}

sealed class BootstrapBootstrapClassLoader : ClassLoaderWrapper
{
	internal BootstrapBootstrapClassLoader()
		: base(CodeGenOptions.None, null)
	{
		TypeWrapper javaLangObject = new StubTypeWrapper(Modifiers.Public, "java.lang.Object", null, true);
		SetRemappedType(JVM.Import(typeof(object)), javaLangObject);
		SetRemappedType(JVM.Import(typeof(string)), new StubTypeWrapper(Modifiers.Public | Modifiers.Final, "java.lang.String", javaLangObject, true));
		SetRemappedType(JVM.Import(typeof(Exception)), new StubTypeWrapper(Modifiers.Public, "java.lang.Throwable", javaLangObject, true));
		SetRemappedType(JVM.Import(typeof(IComparable)), new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.Comparable", null, true));
		TypeWrapper tw = new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.AutoCloseable", null, true);
		tw.SetMethods(new MethodWrapper[] { new SimpleCallMethodWrapper(tw, "close", "()V", JVM.Import(typeof(IDisposable)).GetMethod("Dispose"), PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Public | Modifiers.Abstract, MemberFlags.None, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt) });
		SetRemappedType(JVM.Import(typeof(IDisposable)), tw);

		RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public, "java.lang.Enum", javaLangObject, false));
		RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.annotation.Annotation", null, false));
		RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public | Modifiers.Final, "java.lang.Class", javaLangObject, false));
		RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract, "java.lang.invoke.MethodHandle", javaLangObject, false));
	}
}

sealed class StubTypeWrapper : TypeWrapper
{
	private readonly bool remapped;
	private readonly TypeWrapper baseWrapper;

	internal StubTypeWrapper(Modifiers modifiers, string name, TypeWrapper baseWrapper, bool remapped)
		: base(TypeFlags.None, modifiers, name)
	{
		this.remapped = remapped;
		this.baseWrapper = baseWrapper;
	}

	internal override TypeWrapper BaseTypeWrapper
	{
		get { return baseWrapper; }
	}

	internal override ClassLoaderWrapper GetClassLoader()
	{
		return ClassLoaderWrapper.GetBootstrapClassLoader();
	}

	internal override Type TypeAsTBD
	{
		get { throw new NotSupportedException(); }
	}

	internal override bool IsRemapped
	{
		get { return remapped; }
	}
}
