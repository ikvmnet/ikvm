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
using System.Reflection.Emit;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using OpenSystem.Java;
using System.Text.RegularExpressions;

public class JVM
{
	private static bool debug = false;
	private static bool noJniStubs = false;
	private static bool isStaticCompiler = false;
	private static bool logClassLoadFailures = false;
	private static bool logVerifyErrors = true;		// TODO provide ikvm command line switch
	private static IJniProvider jniProvider;
	private static bool compilationPhase1;

	public static bool Debug
	{
		get
		{
			return debug;
		}
		set
		{
			debug = value;
		}
	}

	public static bool NoJniStubs
	{
		get
		{
			return noJniStubs;
		}
	}

	public static bool IsStaticCompiler
	{
		get
		{
			return isStaticCompiler;
		}
	}

	public static bool IsStaticCompilerPhase1
	{
		get
		{
			return compilationPhase1;
		}
	}

	public static bool CompileInnerClassesAsNestedTypes
	{
		get
		{
			// NOTE at the moment, we always do this when compiling statically
			// note that it makes no sense to turn this on when we're dynamically
			// running Java code, it only makes sense to turn it off when statically
			// compiling code that is never used as a library.
			return IsStaticCompiler;
		}
	}

	public static bool LogClassLoadFailures
	{
		get
		{
			return logClassLoadFailures;
		}
		set
		{
			logClassLoadFailures = value;
		}
	}

	public static bool LogVerifyErrors
	{
		get
		{
			return logVerifyErrors;
		}
		set
		{
			logVerifyErrors = value;
		}
	}

	public static IJniProvider JniProvider
	{
		get
		{
			if(jniProvider == null)
			{
				Type provider;
				if(Environment.GetEnvironmentVariable("IKVM_JNI_PROVIDER") != null)
				{
					provider = Assembly.LoadFrom(Environment.GetEnvironmentVariable("IKVM_JNI_PROVIDER")).GetType("JNI", true);
				}
				else
				{
					if(Environment.OSVersion.ToString().IndexOf("Unix") >= 0)
					{
						provider = Assembly.Load("Mono.IKVM.JNI").GetType("JNI", true);
					}
					else
					{
						provider = Assembly.Load("ik.vm.jni").GetType("JNI", true);
					}
				}
				jniProvider = (IJniProvider)Activator.CreateInstance(provider);
			}
			return jniProvider;
		}
	}

	private class CompilerClassLoader : ClassLoaderWrapper
	{
		private Hashtable classes;
		private string assembly;
		private string path;
        private string keyfilename;
        private string version;
        private bool targetIsModule;
		private AssemblyBuilder assemblyBuilder;

		internal CompilerClassLoader(string path, string keyfilename, string version, bool targetIsModule, string assembly, Hashtable classes)
			: base(null)
		{
			this.classes = classes;
			this.assembly = assembly;
			this.path = path;
            this.targetIsModule = targetIsModule;
            this.version = version;
            this.keyfilename = keyfilename;
		}

		protected override ModuleBuilder CreateModuleBuilder()
		{
			AssemblyName name = new AssemblyName();
			name.Name = assembly;
			if(keyfilename != null) 
			{
				using(FileStream stream = File.Open(keyfilename, FileMode.Open))
				{
					name.KeyPair = new StrongNameKeyPair(stream);
				}
			}
			name.Version = new Version(version);
			assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
			CustomAttributeBuilder ikvmModuleAttr = new CustomAttributeBuilder(typeof(JavaModuleAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			ModuleBuilder moduleBuilder;
			moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly, path, JVM.Debug);
			moduleBuilder.SetCustomAttribute(ikvmModuleAttr);
			if(JVM.Debug)
			{
				CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, true });
				assemblyBuilder.SetCustomAttribute(debugAttr);
			}
			return moduleBuilder;
		}

		internal override TypeWrapper GetTypeWrapperCompilerHook(string name)
		{
			TypeWrapper type = base.GetTypeWrapperCompilerHook(name);
			if(type == null)
			{
				ClassFile f = (ClassFile)classes[name];
				if(f != null)
				{
					// to enhance error reporting we special case loading of netexp
					// classes, to handle the case where the netexp type doesn't exist
					// (this happens when the .NET mscorlib.jar is used on Mono, for example)
					string netexp = f.NetExpAssemblyAttribute;
					if(netexp != null)
					{
						try
						{
							Assembly.Load(netexp);
						}
						catch(Exception)
						{
							Console.Error.WriteLine("netexp assembly not found: {0}", netexp);
						}
						if(DotNetTypeWrapper.LoadDotNetTypeWrapper(name) == null)
						{
							return null;
						}
					}
					type = DefineClass(f);
				}
			}
			return type;
		}

		internal void SetMain(MethodInfo m, PEFileKinds target)
		{
			assemblyBuilder.SetEntryPoint(m, target);
		}

		internal void Save()
		{
			FinishAll();

			if(targetIsModule)
			{
				string manifestAssembly = "temp.$$$";
				assemblyBuilder.Save(manifestAssembly);
				File.Delete(manifestAssembly);
			}
			else
			{
				assemblyBuilder.Save(path);
			}
		}

		internal void AddResources(Hashtable resources)
		{
			ModuleBuilder moduleBuilder = this.ModuleBuilder;
			foreach(DictionaryEntry d in resources)
			{
				byte[] buf = (byte[])d.Value;
				if(buf.Length > 0)
				{
					moduleBuilder.DefineInitializedData((string)d.Key, buf, FieldAttributes.Public | FieldAttributes.Static);
				}
			}
			moduleBuilder.CreateGlobalFunctions();
		}
	}

	public static void Compile(string path, string keyfilename, string version, bool targetIsModule, string assembly, string mainClass, PEFileKinds target, bool guessFileKind, byte[][] classes, string[] references, bool nojni, Hashtable resources, string[] classesToExclude)
	{
		isStaticCompiler = true;
		noJniStubs = nojni;
		foreach(string r in references)
		{
			Assembly.LoadFrom(r);
		}
		Hashtable h = new Hashtable();
		Console.WriteLine("Parsing class files");
		for(int i = 0; i < classes.Length; i++)
		{
			ClassFile f = new ClassFile(classes[i], 0, classes[i].Length, null);
			string name = f.Name;
			bool excluded = false;
			for(int j = 0; j < classesToExclude.Length; j++)
			{
				if(Regex.IsMatch(name, classesToExclude[j]))
				{
					excluded = true;
					break;
				}
			}
			if(h.ContainsKey(name))
			{
				Console.Error.WriteLine("Duplicate class name: {0}", name);
				return;
			}
			if(!excluded)
			{
				h[name] = f;
				if(mainClass == null && (guessFileKind || target != PEFileKinds.Dll))
				{
					foreach(ClassFile.Method m in f.Methods)
					{
						if(m.IsPublic && m.IsStatic && m.Name == "main" && m.Signature == "([Ljava.lang.String;)V")
						{
							Console.Error.WriteLine("Note: found main method in class: {0}", f.Name);
							mainClass = f.Name;
							break;
						}
					}
				}
			}
		}

		if(guessFileKind && mainClass == null)
		{
			target = PEFileKinds.Dll;
		}

		if(target != PEFileKinds.Dll && mainClass == null)
		{
			Console.Error.WriteLine("Error: no main method found");
			return;
		}

		if(path == null)
		{
			if(target == PEFileKinds.Dll)
			{
				if(targetIsModule)
				{
					path = assembly + ".netmodule";
				}
				else
				{
					path = assembly + ".dll";
				}
			}
			else
			{
				path = assembly + ".exe";
			}
			Console.Error.WriteLine("Note: output file is: {0}", path);
		}

		if(targetIsModule)
		{
			// TODO if we're overwriting a user specified assembly name, we need to emit a warning
			assembly = path;
		}

		if(target == PEFileKinds.Dll && !path.ToLower().EndsWith(".dll") && !targetIsModule)
		{
			Console.Error.WriteLine("Error: library output file must end with .dll");
			return;
		}

		if(target != PEFileKinds.Dll && !path.ToLower().EndsWith(".exe"))
		{
			Console.Error.WriteLine("Error: executable output file must end with .exe");
			return;
		}

		// make sure all inner classes have a reference to their outer class
		// note that you cannot use the InnerClasses attribute in the inner class for this, because
		// anonymous inner classes do not have a reference to their outer class
		foreach(ClassFile classFile in h.Values)
		{
			// don't handle inner classes for NetExp types
			if(classFile.NetExpAssemblyAttribute == null)
			{
				ClassFile.InnerClass[] innerClasses = classFile.InnerClasses;
				if(innerClasses != null)
				{
					for(int j = 0; j < innerClasses.Length; j++)
					{
						if(innerClasses[j].outerClass != 0 && classFile.GetConstantPoolClass(innerClasses[j].outerClass) == classFile.Name)
						{
							string inner = classFile.GetConstantPoolClass(innerClasses[j].innerClass);
							ClassFile innerClass = (ClassFile)h[inner];
							if(innerClass != null)
							{
								if(innerClass.OuterClass != null)
								{
									Console.Error.WriteLine("Error: inner class {0} has multiple outer classes", inner);
									return;
								}
								innerClass.OuterClass = classFile;
							}
							else
							{
								Console.Error.WriteLine("Warning: inner class {0} missing", inner);
							}
						}
					}
				}
			}
		}

		Console.WriteLine("Constructing compiler");
		CompilerClassLoader loader = new CompilerClassLoader(path, keyfilename, version, targetIsModule, assembly, h);
		ClassLoaderWrapper.SetBootstrapClassLoader(loader);
		compilationPhase1 = true;
		Console.WriteLine("Loading remapped types (1)");
		loader.LoadRemappedTypes();
		// Do a sanity check to make sure some of the bootstrap classes are available
		if(loader.LoadClassByDottedNameFast("java.lang.Object") == null ||
			loader.LoadClassByDottedNameFast("java.lang.String") == null ||
			loader.LoadClassByDottedNameFast("java.lang.NullPointerException") == null)
		{
			Assembly classpath = Assembly.LoadWithPartialName("classpath");
			if(classpath == null)
			{
				Console.Error.WriteLine("Error: bootstrap classes missing and classpath.dll not found");
				return;
			}
			Console.Error.WriteLine("Warning: bootstrap classes are missing, automatically adding reference to {0}", classpath.Location);
			Console.Error.WriteLine("  (to avoid this warning add \"-reference:{0}\" to the command line)", classpath.Location);
		}
		Console.WriteLine("Compiling class files (1)");
		foreach(string s in h.Keys)
		{
			TypeWrapper wrapper = null;
			try
			{
				wrapper = loader.LoadClassByDottedNameFast(s);
				if(wrapper == null)
				{
					// this should only happen for netexp types (because the other classes must exist, after all we just parsed them)
					Console.Error.WriteLine("Class not found: {0}", s);
				}
			}
			catch(Exception x)
			{
				Console.Error.WriteLine("Loading class {0} failed due to:", s);
				Console.Error.WriteLine(x);
			}
			if(s == mainClass && wrapper != null)
			{
				MethodWrapper mw = wrapper.GetMethodWrapper(MethodDescriptor.FromNameSig(loader, "main", "([Ljava.lang.String;)V"), false);
				if(mw == null)
				{
					Console.Error.WriteLine("Error: main method not found");
					return;
				}
				MethodInfo method = mw.GetMethod() as MethodInfo;
				if(method == null)
				{
					Console.Error.WriteLine("Error: redirected main method not supported");
					return;
				}
				loader.SetMain(method, target);
				mainClass = null;
			}
		}
		if(mainClass != null)
		{
			Console.Error.WriteLine("Error: main class not found");
			return;
		}
		compilationPhase1 = false;
		Console.WriteLine("Loading remapped types (2)");
		loader.LoadRemappedTypesStep2();
		Console.WriteLine("Compiling class files (2)");
		loader.AddResources(resources);
		loader.Save();
	}

	public static void PrepareForSaveDebugImage()
	{
		ClassLoaderWrapper.PrepareForSaveDebugImage();
	}
	
	public static void SaveDebugImage(object mainClass)
	{
		ClassLoaderWrapper.SaveDebugImage(mainClass);
	}

	public static void SetBootstrapClassLoader(object classLoader)
	{
		ClassLoaderWrapper.GetBootstrapClassLoader().SetJavaClassLoader(classLoader);
	}

	internal static void CriticalFailure(string message, Exception x)
	{
		// NOTE we use reflection to invoke MessageBox.Show, to make sure we run on Mono as well
		Assembly winForms = Assembly.LoadWithPartialName("System.Windows.Forms");
		Type messageBox = null;
		if(winForms != null)
		{
			messageBox = winForms.GetType("System.Windows.Forms.MessageBox");
		}
		message = String.Format("****** Critical Failure: {1} ******{0}" +
				"{2}{0}" + 
				"{3}{0}" +
				"{4}",
			Environment.NewLine,
			message,
			x,
			new StackTrace(x, true),
			new StackTrace(true));
		if(messageBox != null)
		{
			messageBox.InvokeMember("Show", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new object[] { message, "IKVM.NET Critical Failure" });
		}
		else
		{
			Console.Error.WriteLine(message);
		}
		Environment.Exit(1);
	}
}
