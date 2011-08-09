/*
  Copyright (C) 2002-2011 Jeroen Frijters

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
		foreach(string s in args)
		{
			if(s.StartsWith("-") || assemblyNameOrPath != null)
			{
				if(s == "-serialver")
				{
					includeSerialVersionUID = true;
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
			Console.Error.WriteLine("    -serialver                 Include serialVersionUID fields");
			Console.Error.WriteLine("    -skiperror                 Continue when errors are encountered");
			Console.Error.WriteLine("    -shared                    Process all assemblies in shared group");
			Console.Error.WriteLine("    -nostdlib                  Do not reference standard libraries");
			Console.Error.WriteLine("    -lib:<dir>                 Additional directories to search for references");
			Console.Error.WriteLine("    -namespace:<ns>            Only include types from specified namespace");
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
			int rc1 = StaticCompiler.Resolver.ResolveReference(cache, ref dummy, reference);
			if (rc1 != 0)
			{
				return rc1;
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
							if (ProcessAssembly(asm, continueOnError) != 0)
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
		string name = tw.Name.Replace('.', '/');
		string super = null;
		if (tw.IsInterface)
		{
			super = "java/lang/Object";
		}
		else if (tw.BaseTypeWrapper != null)
		{
			super = tw.BaseTypeWrapper.Name.Replace('.', '/');
		}
		IKVM.StubGen.ClassFileWriter writer = new IKVM.StubGen.ClassFileWriter(tw.Modifiers, name, super, 0, 49);
		foreach (TypeWrapper iface in tw.Interfaces)
		{
			if (iface.IsPublic)
			{
				writer.AddInterface(iface.Name.Replace('.', '/'));	
			}
		}
		IKVM.StubGen.InnerClassesAttribute innerClassesAttribute = null;
		if (tw.DeclaringTypeWrapper != null)
		{
			TypeWrapper outer = tw.DeclaringTypeWrapper;
			string innername = name;
			int idx = name.LastIndexOf('$');
			if (idx >= 0)
			{
				innername = innername.Substring(idx + 1);
			}
			innerClassesAttribute = new IKVM.StubGen.InnerClassesAttribute(writer);
			innerClassesAttribute.Add(name, outer.Name.Replace('.', '/'), innername, (ushort)tw.ReflectiveModifiers);
		}
		foreach (TypeWrapper inner in tw.InnerClasses)
		{
			if (inner.IsPublic)
			{
				if (innerClassesAttribute == null)
				{
					innerClassesAttribute = new IKVM.StubGen.InnerClassesAttribute(writer);
				}
				string namePart = inner.Name;
				namePart = namePart.Substring(namePart.LastIndexOf('$') + 1);
				innerClassesAttribute.Add(inner.Name.Replace('.', '/'), name, namePart, (ushort)inner.ReflectiveModifiers);
			}
		}
		if (innerClassesAttribute != null)
		{
			writer.AddAttribute(innerClassesAttribute);
		}
		string genericTypeSignature = tw.GetGenericSignature();
		if (genericTypeSignature != null)
		{
			writer.AddStringAttribute("Signature", genericTypeSignature);
		}
		writer.AddStringAttribute("IKVM.NET.Assembly", GetAssemblyName(tw));
		if (tw.TypeAsBaseType.IsDefined(StaticCompiler.Universe.Import(typeof(ObsoleteAttribute)), false))
		{
			writer.AddAttribute(new IKVM.StubGen.DeprecatedAttribute(writer));
		}
		foreach (MethodWrapper mw in tw.GetMethods())
		{
			if (!mw.IsHideFromReflection && (mw.IsPublic || mw.IsProtected))
			{
				IKVM.StubGen.FieldOrMethod m;
				if (mw.Name == "<init>")
				{
					m = writer.AddMethod(mw.Modifiers, mw.Name, mw.Signature.Replace('.', '/'));
					IKVM.StubGen.CodeAttribute code = new IKVM.StubGen.CodeAttribute(writer);
					code.MaxLocals = (ushort)(mw.GetParameters().Length * 2 + 1);
					code.MaxStack = 3;
					ushort index1 = writer.AddClass("java/lang/UnsatisfiedLinkError");
					ushort index2 = writer.AddString("ikvmstub generated stubs can only be used on IKVM.NET");
					ushort index3 = writer.AddMethodRef("java/lang/UnsatisfiedLinkError", "<init>", "(Ljava/lang/String;)V");
					code.ByteCode = new byte[] {
						187, (byte)(index1 >> 8), (byte)index1,	// new java/lang/UnsatisfiedLinkError
						89,										// dup
						19,	 (byte)(index2 >> 8), (byte)index2,	// ldc_w "..."
						183, (byte)(index3 >> 8), (byte)index3, // invokespecial java/lang/UnsatisfiedLinkError/init()V
						191										// athrow
					};
					m.AddAttribute(code);
				}
				else
				{
					Modifiers mods = mw.Modifiers;
					if ((mods & Modifiers.Abstract) == 0)
					{
						mods |= Modifiers.Native;
					}
					m = writer.AddMethod(mods, mw.Name, mw.Signature.Replace('.', '/'));
					if (mw.IsOptionalAttributeAnnotationValue)
					{
						m.AddAttribute(new IKVM.StubGen.AnnotationDefaultClassFileAttribute(writer, GetAnnotationDefault(writer, mw.ReturnType)));
					}
				}
				MethodBase mb = mw.GetMethod();
				if (mb != null)
				{
					ThrowsAttribute throws = AttributeHelper.GetThrows(mb);
					if (throws == null)
					{
						string[] throwsArray = mw.GetDeclaredExceptions();
						if (throwsArray != null && throwsArray.Length > 0)
						{
							IKVM.StubGen.ExceptionsAttribute attrib = new IKVM.StubGen.ExceptionsAttribute(writer);
							foreach (string ex in throwsArray)
							{
								attrib.Add(ex.Replace('.', '/'));
							}
							m.AddAttribute(attrib);
						}
					}
					else
					{
						IKVM.StubGen.ExceptionsAttribute attrib = new IKVM.StubGen.ExceptionsAttribute(writer);
						if (throws.classes != null)
						{
							foreach (string ex in throws.classes)
							{
								attrib.Add(ex.Replace('.', '/'));
							}
						}
						if (throws.types != null)
						{
							foreach (Type ex in throws.types)
							{
								attrib.Add(ex.FullName.Replace('.', '/'));
							}
						}
						m.AddAttribute(attrib);
					}
					if (mb.IsDefined(StaticCompiler.Universe.Import(typeof(ObsoleteAttribute)), false))
					{
						m.AddAttribute(new IKVM.StubGen.DeprecatedAttribute(writer));
					}
				}
				string sig = tw.GetGenericMethodSignature(mw);
				if (sig != null)
				{
					m.AddAttribute(writer.MakeStringAttribute("Signature", sig));
				}
			}
		}
		bool hasSerialVersionUID = false;
		foreach (FieldWrapper fw in tw.GetFields())
		{
			if (!fw.IsHideFromReflection)
			{
				bool isSerialVersionUID = includeSerialVersionUID && fw.Name == "serialVersionUID" && fw.FieldTypeWrapper == PrimitiveTypeWrapper.LONG;
				hasSerialVersionUID |= isSerialVersionUID;
				if (fw.IsPublic || fw.IsProtected || isSerialVersionUID)
				{
					object constant = null;
					if (fw.GetField() != null && fw.GetField().IsLiteral && (fw.FieldTypeWrapper.IsPrimitive || fw.FieldTypeWrapper == CoreClasses.java.lang.String.Wrapper))
					{
						constant = fw.GetField().GetRawConstantValue();
						if (fw.GetField().FieldType.IsEnum)
						{
							constant = EnumHelper.GetPrimitiveValue(EnumHelper.GetUnderlyingType(fw.GetField().FieldType), constant);
						}
					}
					IKVM.StubGen.FieldOrMethod f = writer.AddField(fw.Modifiers, fw.Name, fw.Signature.Replace('.', '/'), constant);
					string sig = tw.GetGenericFieldSignature(fw);
					if (sig != null)
					{
						f.AddAttribute(writer.MakeStringAttribute("Signature", sig));
					}
					if (fw.GetField() != null && fw.GetField().IsDefined(StaticCompiler.Universe.Import(typeof(ObsoleteAttribute)), false))
					{
						f.AddAttribute(new IKVM.StubGen.DeprecatedAttribute(writer));
					}
				}
			}
		}
		if (includeSerialVersionUID && !hasSerialVersionUID && IsSerializable(tw))
		{
			// class is serializable but doesn't have an explicit serialVersionUID, so we add the field to record
			// the serialVersionUID as we see it (mainly to make the Japi reports more realistic)
			writer.AddField(Modifiers.Private | Modifiers.Static | Modifiers.Final, "serialVersionUID", "J", IKVM.StubGen.SerialVersionUID.Compute(tw));
		}
		AddMetaAnnotations(writer, tw);
		zipCount++;
		MemoryStream mem = new MemoryStream();
		writer.Write(mem);
		ZipEntry entry = new ZipEntry(name + ".class");
		entry.Size = mem.Position;
		zipFile.PutNextEntry(entry);
		mem.WriteTo(zipFile);
	}

	private static string GetAssemblyName(TypeWrapper tw)
	{
		ClassLoaderWrapper loader = tw.GetClassLoader();
		AssemblyClassLoader acl = loader as AssemblyClassLoader;
		if (acl != null)
		{
			return acl.GetAssembly(tw).FullName;
		}
		else
		{
			return ((GenericClassLoader)loader).GetName();
		}
	}

	private static bool IsSerializable(TypeWrapper tw)
	{
		if (tw.Name == "java.io.Serializable")
		{
			return true;
		}
		while (tw != null)
		{
			foreach (TypeWrapper iface in tw.Interfaces)
			{
				if (IsSerializable(iface))
				{
					return true;
				}
			}
			tw = tw.BaseTypeWrapper;
		}
		return false;
	}

	private static void AddMetaAnnotations(IKVM.StubGen.ClassFileWriter writer, TypeWrapper tw)
	{
		DotNetTypeWrapper.AttributeAnnotationTypeWrapperBase attributeAnnotation = tw as DotNetTypeWrapper.AttributeAnnotationTypeWrapperBase;
		if (attributeAnnotation != null)
		{
			// TODO write the annotation directly, instead of going thru the object[] encoding
			IKVM.StubGen.RuntimeVisibleAnnotationsAttribute annot = new IKVM.StubGen.RuntimeVisibleAnnotationsAttribute(writer);
			annot.Add(new object[] {
					AnnotationDefaultAttribute.TAG_ANNOTATION,
					"Ljava/lang/annotation/Retention;",
					"value",
					new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/RetentionPolicy;", "RUNTIME" }
				});
			AttributeTargets validOn = attributeAnnotation.AttributeTargets;
			List<object[]> targets = new List<object[]>();
			if ((validOn & (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
			{
				targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "TYPE" });
			}
			if ((validOn & AttributeTargets.Constructor) != 0)
			{
				targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "CONSTRUCTOR" });
			}
			if ((validOn & AttributeTargets.Field) != 0)
			{
				targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "FIELD" });
			}
			if ((validOn & (AttributeTargets.Method | AttributeTargets.ReturnValue)) != 0)
			{
				targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "METHOD" });
			}
			if ((validOn & AttributeTargets.Parameter) != 0)
			{
				targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "PARAMETER" });
			}
			annot.Add(new object[] {
					AnnotationDefaultAttribute.TAG_ANNOTATION,
					"Ljava/lang/annotation/Target;",
					"value",
					new object[] { AnnotationDefaultAttribute.TAG_ARRAY, targets.ToArray() }
				});
			writer.AddAttribute(annot);
		}
	}

	private static byte[] GetAnnotationDefault(IKVM.StubGen.ClassFileWriter classFile, TypeWrapper type)
	{
		MemoryStream mem = new MemoryStream();
		IKVM.StubGen.BigEndianStream bes = new IKVM.StubGen.BigEndianStream(mem);
		if (type == PrimitiveTypeWrapper.BOOLEAN)
		{
			bes.WriteByte((byte)'Z');
			bes.WriteUInt16(classFile.AddInt(0));
		}
        else if(type == PrimitiveTypeWrapper.BYTE)
        {
			bes.WriteByte((byte)'B');
			bes.WriteUInt16(classFile.AddInt(0));
        }
        else if(type == PrimitiveTypeWrapper.CHAR)
        {
			bes.WriteByte((byte)'C');
			bes.WriteUInt16(classFile.AddInt(0));
        }
        else if(type == PrimitiveTypeWrapper.SHORT)
        {
			bes.WriteByte((byte)'S');
			bes.WriteUInt16(classFile.AddInt(0));
        }
        else if(type == PrimitiveTypeWrapper.INT)
        {
			bes.WriteByte((byte)'I');
			bes.WriteUInt16(classFile.AddInt(0));
        }
        else if(type == PrimitiveTypeWrapper.FLOAT)
        {
			bes.WriteByte((byte)'F');
			bes.WriteUInt16(classFile.AddFloat(0));
        }
        else if(type == PrimitiveTypeWrapper.LONG)
        {
			bes.WriteByte((byte)'J');
			bes.WriteUInt16(classFile.AddLong(0));
        }
		else if (type == PrimitiveTypeWrapper.DOUBLE)
        {
			bes.WriteByte((byte)'D');
			bes.WriteUInt16(classFile.AddDouble(0));
        }
		else if (type == CoreClasses.java.lang.String.Wrapper)
		{
			bes.WriteByte((byte)'s');
			bes.WriteUInt16(classFile.AddUtf8(""));
		}
		else if ((type.Modifiers & Modifiers.Enum) != 0)
		{
			bes.WriteByte((byte)'e');
			bes.WriteUInt16(classFile.AddUtf8("L" + type.Name.Replace('.', '/') + ";"));
			bes.WriteUInt16(classFile.AddUtf8("__unspecified"));
		}
		else if (type == CoreClasses.java.lang.Class.Wrapper)
		{
			bes.WriteByte((byte)'c');
			bes.WriteUInt16(classFile.AddUtf8("Likvm/internal/__unspecified;"));
		}
		else if (type.IsArray)
		{
			bes.WriteByte((byte)'[');
			bes.WriteUInt16(0);
		}
		else
		{
			throw new InvalidOperationException();
		}
		return mem.ToArray();
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

	private static int ProcessAssembly(Assembly assembly, bool continueOnError)
	{
		int rc = 0;
		foreach (Type t in assembly.GetTypes())
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
					WriteClass(c);
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
				AddToExportListIfNeeded(mw.ReturnType);
				AddToExportListIfNeeded(mw.GetParameters());
			}
		}
		foreach (FieldWrapper fw in tw.GetFields())
		{
			if (fw.IsPublic || fw.IsProtected)
			{
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
	internal static readonly Universe Universe = new Universe();
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
	}
}

sealed class StubTypeWrapper : TypeWrapper
{
	private readonly bool remapped;

	internal StubTypeWrapper(Modifiers modifiers, string name, TypeWrapper baseWrapper, bool remapped)
		: base(modifiers, name, baseWrapper)
	{
		this.remapped = remapped;
	}

	internal override ClassLoaderWrapper GetClassLoader()
	{
		return ClassLoaderWrapper.GetBootstrapClassLoader();
	}

	internal override Type TypeAsTBD
	{
		get { throw new NotSupportedException(); }
	}

	internal override TypeWrapper[] Interfaces
	{
		get { return TypeWrapper.EmptyArray; }
	}

	internal override TypeWrapper[] InnerClasses
	{
		get { return TypeWrapper.EmptyArray; }
	}

	internal override TypeWrapper DeclaringTypeWrapper
	{
		get { return null; }
	}

	internal override void Finish()
	{
	}

	internal override bool IsRemapped
	{
		get { return remapped; }
	}
}
