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
using System.Threading;
using System.Resources;
using System.Reflection.Emit;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using IKVM.Attributes;
using IKVM.Runtime;

using ILGenerator = CountingILGenerator;

namespace IKVM.Runtime
{
	public sealed class Startup
	{
		private Startup()
		{
		}

		public static string[] Glob(string arg)
		{
			if(IKVM.Internal.JVM.IsUnix)
			{
				return new string[] { arg };
			}
			try
			{
				string dir = Path.GetDirectoryName(arg);
				if(dir == "")
				{
					dir = null;
				}
				ArrayList list = new ArrayList();
				foreach(FileSystemInfo fsi in new DirectoryInfo(dir == null ? Environment.CurrentDirectory : dir).GetFileSystemInfos(Path.GetFileName(arg)))
				{
					list.Add(dir != null ? Path.Combine(dir, fsi.Name) : fsi.Name);
				}
				if(list.Count == 0)
				{
					return new string[] { arg };
				}
				return (string[])list.ToArray(typeof(string));
			}
			catch
			{
				return new string[] { arg };
			}
		}

		public static string[] Glob()
		{
			if(IKVM.Internal.JVM.IsUnix)
			{
				return Environment.GetCommandLineArgs();
			}
			else
			{
				return Glob(1);
			}
		}

		public static string[] Glob(int skip)
		{
			if(IKVM.Internal.JVM.IsUnix)
			{
				string[] args = Environment.GetCommandLineArgs();
				string[] vmargs = new string[args.Length - skip];
				Array.Copy(args, skip, vmargs, 0, args.Length - skip);
				return vmargs;
			}
			else
			{
				ArrayList list = new ArrayList();
				string cmdline = Environment.CommandLine;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				for(int i = 0; i < cmdline.Length;)
				{
					bool quoted = cmdline[i] == '"';
				cont_arg:
					while(i < cmdline.Length && cmdline[i] != ' ' && cmdline[i] != '"')
					{
						sb.Append(cmdline[i++]);
					}
					if(i < cmdline.Length && cmdline[i] == '"')
					{
						if(quoted && i > 1 && cmdline[i - 1] == '"')
						{
							sb.Append('"');
						}
						i++;
						while(i < cmdline.Length && cmdline[i] != '"')
						{
							sb.Append(cmdline[i++]);
						}
						if(i < cmdline.Length && cmdline[i] == '"')
						{
							i++;
						}
						if(i < cmdline.Length && cmdline[i] != ' ')
						{
							goto cont_arg;
						}
					}
					while(i < cmdline.Length && cmdline[i] == ' ')
					{
						i++;
					}
					if(skip > 0)
					{
						skip--;
					}
					else
					{
						if(quoted)
						{
							list.Add(sb.ToString());
						}
						else
						{
							list.AddRange(Glob(sb.ToString()));
						}
					}
					sb.Length = 0;
				}
				return (string[])list.ToArray(typeof(string));
			}
		}

		public static void SetProperties(System.Collections.Hashtable props)
		{
			IKVM.Internal.JVM.Library.setProperties(props);
		}

		public static void EnterMainThread()
		{
			if(Thread.CurrentThread.Name == null)
			{
				Thread.CurrentThread.Name = "main";
			}
		}

		public static void ExitMainThread()
		{
			// FXBUG when the main thread ends, it doesn't actually die, it stays around to manage the lifetime
			// of the CLR, but in doing so it also keeps alive the thread local storage for this thread and we
			// use the TLS as a hack to track when the thread dies (if the object stored in the TLS is finalized,
			// we know the thread is dead). So to make that work for the main thread, we explicitly clear the TLS
			// slot that contains our hack object.
			try
			{
				Thread.SetData(Thread.GetNamedDataSlot("ikvm-thread-hack"), null);
			}
			catch(NullReferenceException)
			{
				// MONOBUG Thread.SetData throws a NullReferenceException on Mono
				// if the slot hadn't already been allocated
			}
		}
	}

	public sealed class Util
	{
		private Util()
		{
		}

		public static object GetClassFromObject(object o)
		{
			Type t = o.GetType();
			if(t.IsPrimitive || (ClassLoaderWrapper.IsRemappedType(t) && !t.IsSealed))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).ClassObject;
			}
			return ClassLoaderWrapper.GetWrapperFromType(t).ClassObject;
		}

		public static object GetClassFromTypeHandle(RuntimeTypeHandle handle)
		{
			Type t = Type.GetTypeFromHandle(handle);
			if(t.IsPrimitive || ClassLoaderWrapper.IsRemappedType(t))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).ClassObject;
			}
			return ClassLoaderWrapper.GetWrapperFromType(t).ClassObject;
		}

		private static FieldWrapper GetFieldWrapperFromField(object field)
		{
			if(field == null)
			{
				throw new ArgumentNullException("field");
			}
			if(field.GetType().FullName != "java.lang.reflect.Field")
			{
				throw new ArgumentException("field");
			}
			return (FieldWrapper)IKVM.Internal.JVM.Library.getWrapperFromField(field);
		}

		public static object GetFieldConstantValue(object field)
		{
			return GetFieldWrapperFromField(field).GetConstant();
		}

		public static bool IsFieldDeprecated(object field)
		{
			FieldInfo fi = GetFieldWrapperFromField(field).GetField();
			return fi != null && fi.IsDefined(typeof(ObsoleteAttribute), false);
		}

		public static bool IsMethodDeprecated(object method)
		{
			if(method == null)
			{
				throw new ArgumentNullException("method");
			}
			if(method.GetType().FullName != "java.lang.reflect.Method")
			{
				throw new ArgumentException("method");
			}
			MethodWrapper mw = (MethodWrapper)IKVM.Internal.JVM.Library.getWrapperFromMethodOrConstructor(method);
			MethodBase mb = mw.GetMethod();
			return mb != null && mb.IsDefined(typeof(ObsoleteAttribute), false);
		}

		public static bool IsConstructorDeprecated(object constructor)
		{
			if(constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			if(constructor.GetType().FullName != "java.lang.reflect.Constructor")
			{
				throw new ArgumentException("constructor");
			}
			MethodWrapper mw = (MethodWrapper)IKVM.Internal.JVM.Library.getWrapperFromMethodOrConstructor(constructor);
			MethodBase mb = mw.GetMethod();
			return mb != null && mb.IsDefined(typeof(ObsoleteAttribute), false);
		}

		public static bool IsClassDeprecated(object clazz)
		{
			if(clazz == null)
			{
				throw new ArgumentNullException("clazz");
			}
			if(clazz.GetType().FullName != "java.lang.Class")
			{
				throw new ArgumentException("clazz");
			}
			TypeWrapper wrapper = IKVM.NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
			return wrapper.TypeAsTBD.IsDefined(typeof(ObsoleteAttribute), false);
		}

		[HideFromJava]
		public static Exception MapException(Exception x)
		{
			return IKVM.Internal.JVM.Library.mapException(x);
		}
	}
}

namespace IKVM.Internal
{
	public class JVM
	{
		private static bool debug;
		private static bool noJniStubs;
		private static bool isStaticCompiler;
		private static bool noStackTraceInfo;
		private static bool compilationPhase1;
		private static string sourcePath;
		private static bool monoBugWorkaround;
		private static ikvm.@internal.LibraryVMInterface lib;

		internal static ikvm.@internal.LibraryVMInterface Library
		{
			get
			{
				if(lib == null)
				{
					foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
					{
						Type type = asm.GetType("java.lang.LibraryVMInterfaceImpl");
						if(type != null)
						{
							lib = (ikvm.@internal.LibraryVMInterface)Activator.CreateInstance(type, true);
							break;
						}
					}
					if(lib == null && !IsStaticCompiler)
					{
						JVM.CriticalFailure("Unable to find java.lang.LibraryVMInterfaceImpl", null);
					}
				}
				return lib;
			}
		}

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

		public static string SourcePath
		{
			get
			{
				return sourcePath;
			}
			set
			{
				sourcePath = value;
			}
		}

		internal static bool NoJniStubs
		{
			get
			{
				return noJniStubs;
			}
		}

		internal static bool NoStackTraceInfo
		{
			get
			{
				return noStackTraceInfo;
			}
		}

		internal static bool IsStaticCompiler
		{
			get
			{
				return isStaticCompiler;
			}
		}

		internal static bool IsStaticCompilerPhase1
		{
			get
			{
				return compilationPhase1;
			}
		}

		internal static bool CompileInnerClassesAsNestedTypes
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

		internal static bool IsUnix
		{
			get
			{
				return Environment.OSVersion.ToString().IndexOf("Unix") >= 0;
			}
		}
	
		internal static string MangleResourceName(string name)
		{
			// FXBUG there really shouldn't be any need to mangle the resource names,
			// but in order for ILDASM/ILASM round tripping to work reliably, we have
			// to make sure that we don't produce resource names that'll cause ILDASM
			// to generate invalid filenames.
			System.Text.StringBuilder sb = new System.Text.StringBuilder("ikvm__", name.Length + 6);
			foreach(char c in name)
			{
				if("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_-+.()$#@~=&{}[]0123456789`".IndexOf(c) != -1)
				{
					sb.Append(c);
				}
				else if(c == '/')
				{
					sb.Append('!');
				}
				else
				{
					sb.Append('%');
					sb.Append(string.Format("{0:X4}", (int)c));
				}
			}
			return sb.ToString();
		}

		private class CompilerClassLoader : ClassLoaderWrapper
		{
			private Hashtable classes;
			private Hashtable remapped = new Hashtable();
			private string assemblyName;
			private string assemblyFile;
			private string assemblyDir;
			private string keyfilename;
			private string keycontainer;
			private string version;
			private bool targetIsModule;
			private AssemblyBuilder assemblyBuilder;

			internal CompilerClassLoader(string path, string keyfilename, string keycontainer, string version, bool targetIsModule, string assemblyName, Hashtable classes)
				: base(null)
			{
				this.classes = classes;
				this.assemblyName = assemblyName;
				FileInfo assemblyPath = new FileInfo(path);
				this.assemblyFile = assemblyPath.Name;
				this.assemblyDir = assemblyPath.DirectoryName;
				this.targetIsModule = targetIsModule;
				this.version = version;
				this.keyfilename = keyfilename;
				this.keycontainer = keycontainer;
				Tracer.Info(Tracer.Compiler, "Instantiate CompilerClassLoader for {0}", assemblyName);
			}

			protected override ModuleBuilder CreateModuleBuilder()
			{
				AssemblyName name = new AssemblyName();
				name.Name = assemblyName;
				if(keyfilename != null) 
				{
					using(FileStream stream = File.Open(keyfilename, FileMode.Open))
					{
						name.KeyPair = new StrongNameKeyPair(stream);
					}
				}
				if(keycontainer != null)
				{
					name.KeyPair = new StrongNameKeyPair(keycontainer);
				}
				name.Version = new Version(version);
				assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave, assemblyDir);
				ModuleBuilder moduleBuilder;
				moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFile, JVM.Debug);
				CustomAttributeBuilder ikvmModuleAttr = new CustomAttributeBuilder(typeof(JavaModuleAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
				moduleBuilder.SetCustomAttribute(ikvmModuleAttr);
				CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, JVM.Debug });
				assemblyBuilder.SetCustomAttribute(debugAttr);
				return moduleBuilder;
			}

			internal override TypeWrapper GetTypeWrapperCompilerHook(string name)
			{
				TypeWrapper type = base.GetTypeWrapperCompilerHook(name);
				if(type == null)
				{
					type = (TypeWrapper)remapped[name];
					if(type != null)
					{
						return type;
					}
					ClassFile f = (ClassFile)classes[name];
					if(f != null)
					{
						// to enhance error reporting we special case loading of netexp
						// classes, to handle the case where the ikvmstub type doesn't exist
						// (this happens when the .NET mscorlib.jar is used on Mono, for example)
						string netexp = f.IKVMAssemblyAttribute;
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
							// HACK create a new wrapper to see if the type is visible now
							if(DotNetTypeWrapper.CreateDotNetTypeWrapper(name) == null)
							{
								return null;
							}
						}
						type = DefineClass(f, null);
					}
				}
				return type;
			}

			internal void SetMain(MethodInfo m, PEFileKinds target, Hashtable props, bool noglobbing, Type apartmentAttributeType)
			{
				Type[] args = Type.EmptyTypes;
				if(noglobbing)
				{
					args = new Type[] { typeof(string[]) };
				}
				MethodBuilder mainStub = this.ModuleBuilder.DefineGlobalMethod("main", MethodAttributes.Public | MethodAttributes.Static, typeof(int), args);
				if(apartmentAttributeType != null)
				{
					mainStub.SetCustomAttribute(new CustomAttributeBuilder(apartmentAttributeType.GetConstructor(Type.EmptyTypes), new object[0]));
				}
				ILGenerator ilgen = mainStub.GetILGenerator();
				LocalBuilder rc = ilgen.DeclareLocal(typeof(int));
				if(props.Count > 0)
				{
					ilgen.Emit(OpCodes.Newobj, typeof(Hashtable).GetConstructor(Type.EmptyTypes));
					foreach(DictionaryEntry de in props)
					{
						ilgen.Emit(OpCodes.Dup);
						ilgen.Emit(OpCodes.Ldstr, (string)de.Key);
						ilgen.Emit(OpCodes.Ldstr, (string)de.Value);
						ilgen.Emit(OpCodes.Callvirt, typeof(Hashtable).GetMethod("Add"));
					}
					ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Startup).GetMethod("SetProperties"));
				}
				ilgen.BeginExceptionBlock();
				ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Startup).GetMethod("EnterMainThread"));
				if(noglobbing)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
				}
				else
				{
					ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Startup).GetMethod("Glob", Type.EmptyTypes));
				}
				ilgen.Emit(OpCodes.Call, m);
				ilgen.BeginCatchBlock(typeof(Exception));
				ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Util).GetMethod("MapException", new Type[] { typeof(Exception) }));
				LocalBuilder exceptionLocal = ilgen.DeclareLocal(typeof(Exception));
				ilgen.Emit(OpCodes.Stloc, exceptionLocal);
				TypeWrapper threadTypeWrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.Thread");
				LocalBuilder threadLocal = ilgen.DeclareLocal(threadTypeWrapper.TypeAsLocalOrStackType);
				threadTypeWrapper.GetMethodWrapper("currentThread", "()Ljava.lang.Thread;", false).EmitCall(ilgen);
				ilgen.Emit(OpCodes.Stloc, threadLocal);
				ilgen.Emit(OpCodes.Ldloc, threadLocal);
				threadTypeWrapper.GetMethodWrapper("getThreadGroup", "()Ljava.lang.ThreadGroup;", false).EmitCallvirt(ilgen);
				ilgen.Emit(OpCodes.Ldloc, threadLocal);
				ilgen.Emit(OpCodes.Ldloc, exceptionLocal);
				ClassLoaderWrapper.LoadClassCritical("java.lang.ThreadGroup").GetMethodWrapper("uncaughtException", "(Ljava.lang.Thread;Ljava.lang.Throwable;)V", false).EmitCall(ilgen);
				ilgen.Emit(OpCodes.Ldc_I4_1);
				ilgen.Emit(OpCodes.Stloc, rc);
				ilgen.BeginFinallyBlock();
				ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Startup).GetMethod("ExitMainThread", Type.EmptyTypes));
				ilgen.EndExceptionBlock();
				ilgen.Emit(OpCodes.Ldloc, rc);
				ilgen.Emit(OpCodes.Ret);
				assemblyBuilder.SetEntryPoint(mainStub, target);
			}

			private void MonoBugWorkaround()
			{
				// Mono 1.0.5 (and earlier) and 1.1.3 (and earlier) have bug in the metadata routines.
				// Zoltan says:
				// The size calculation for the MethodSematics:Association metadata column was wrong,
				// it was based on the size of the Method table, so when the number of methods in the
				// dll exceeded some number, all the tables after the MethodSematics table had the
				// wrong address. This means the only workaround for this bug is to emit
				// like 32768 dummy Events or Properties, or decrease the size of the Method table to
				// below 32768 by dropping some classes.
				// ---
				// We distribute the properties in 128 nested classes, because peverify has some very
				// non-linear algorithms and is extremely slow with 32768 properties in a single class.
				// (it also helps make the overhead a little smaller.)
				TypeBuilder tb = ModuleBuilder.DefineType("MonoBugWorkaround", TypeAttributes.NotPublic);
				TypeBuilder[] nested = new TypeBuilder[128];
				for(int i = 0; i < nested.Length; i++)
				{
					nested[i] = tb.DefineNestedType(i.ToString(), TypeAttributes.NestedPrivate);
					for(int j = 0; j < 32768 / nested.Length; j++)
					{
						nested[i].DefineProperty(j.ToString(), PropertyAttributes.None, typeof(int), Type.EmptyTypes);
					}
				}
				tb.CreateType();
				for(int i = 0; i < nested.Length; i++)
				{
					nested[i].CreateType();
				}
			}

			internal void Save()
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader.Save...");
				FinishAll();

				if(monoBugWorkaround)
				{
					MonoBugWorkaround();
				}

				ModuleBuilder.CreateGlobalFunctions();

				if(targetIsModule)
				{
					Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving temp.$$$ in {0}", assemblyDir);
					string manifestAssembly = "temp.$$$";
					assemblyBuilder.Save(manifestAssembly);
					File.Delete(assemblyDir + manifestAssembly);
				}
				else
				{
					Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving {0} in {1}", assemblyFile, assemblyDir);
					assemblyBuilder.Save(assemblyFile);
				}
			}

			internal void AddResources(Hashtable resources)
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader adding resources...");
				ModuleBuilder moduleBuilder = this.ModuleBuilder;
				foreach(DictionaryEntry d in resources)
				{
					byte[] buf = (byte[])d.Value;
					if(buf.Length > 0)
					{
						IResourceWriter writer = moduleBuilder.DefineResource(JVM.MangleResourceName((string)d.Key), "");
						writer.AddResource("ikvm", buf);
					}
				}
			}

			private static MethodAttributes MapMethodAccessModifiers(IKVM.Internal.MapXml.MapModifiers mod)
			{
				const IKVM.Internal.MapXml.MapModifiers access = IKVM.Internal.MapXml.MapModifiers.Public | IKVM.Internal.MapXml.MapModifiers.Protected | IKVM.Internal.MapXml.MapModifiers.Private;
				switch(mod & access)
				{
					case IKVM.Internal.MapXml.MapModifiers.Public:
						return MethodAttributes.Public;
					case IKVM.Internal.MapXml.MapModifiers.Protected:
						return MethodAttributes.FamORAssem;
					case IKVM.Internal.MapXml.MapModifiers.Private:
						return MethodAttributes.Private;
					default:
						return MethodAttributes.Assembly;
				}
			}

			private static FieldAttributes MapFieldAccessModifiers(IKVM.Internal.MapXml.MapModifiers mod)
			{
				const IKVM.Internal.MapXml.MapModifiers access = IKVM.Internal.MapXml.MapModifiers.Public | IKVM.Internal.MapXml.MapModifiers.Protected | IKVM.Internal.MapXml.MapModifiers.Private;
				switch(mod & access)
				{
					case IKVM.Internal.MapXml.MapModifiers.Public:
						return FieldAttributes.Public;
					case IKVM.Internal.MapXml.MapModifiers.Protected:
						return FieldAttributes.FamORAssem;
					case IKVM.Internal.MapXml.MapModifiers.Private:
						return FieldAttributes.Private;
					default:
						return FieldAttributes.Assembly;
				}
			}

			private class RemapperTypeWrapper : TypeWrapper
			{
				private TypeBuilder typeBuilder;
				private TypeBuilder helperTypeBuilder;
				private Type shadowType;
				private IKVM.Internal.MapXml.Class classDef;
				private TypeWrapper[] interfaceWrappers;

				internal override Assembly Assembly
				{
					get
					{
						return typeBuilder.Assembly;
					}
				}

				internal override bool IsRemapped
				{
					get
					{
						return true;
					}
				}

				private static TypeWrapper GetBaseWrapper(IKVM.Internal.MapXml.Class c)
				{
					if((c.Modifiers & IKVM.Internal.MapXml.MapModifiers.Interface) != 0)
					{
						return null;
					}
					if(c.Name == "java.lang.Object")
					{
						return null;
					}
					return CoreClasses.java.lang.Object.Wrapper;
				}

				internal RemapperTypeWrapper(CompilerClassLoader classLoader, IKVM.Internal.MapXml.Class c, IKVM.Internal.MapXml.Root map)
					: base((Modifiers)c.Modifiers, c.Name, GetBaseWrapper(c), classLoader, null)
				{
					classDef = c;
					bool baseIsSealed = false;
					shadowType = Type.GetType(c.Shadows, true);
					classLoader.SetRemappedType(shadowType, this);
					Type baseType = shadowType;
					Type baseInterface = null;
					if(baseType.IsInterface)
					{
						baseInterface = baseType;
					}
					TypeAttributes attrs = TypeAttributes.Public;
					if((c.Modifiers & IKVM.Internal.MapXml.MapModifiers.Interface) == 0)
					{
						attrs |= TypeAttributes.Class;
						if(baseType.IsSealed)
						{
							baseIsSealed = true;
							// FXBUG .NET framework bug
							// ideally we would make the type sealed and abstract,
							// but Reflection.Emit incorrectly prohibits that
							// (the ECMA spec explicitly mentions this is valid)
							// attrs |= TypeAttributes.Abstract | TypeAttributes.Sealed;
							attrs |= TypeAttributes.Abstract;
						}
					}
					else
					{
						attrs |= TypeAttributes.Interface | TypeAttributes.Abstract;
						baseType = null;
					}
					if((c.Modifiers & IKVM.Internal.MapXml.MapModifiers.Abstract) != 0)
					{
						attrs |= TypeAttributes.Abstract;
					}
					string name = c.Name.Replace('/', '.');
					typeBuilder = classLoader.ModuleBuilder.DefineType(name, attrs, baseIsSealed ? typeof(object) : baseType);
					if(baseInterface != null)
					{
						typeBuilder.AddInterfaceImplementation(baseInterface);
					}
					if(!JVM.NoStackTraceInfo)
					{
						AttributeHelper.SetSourceFile(typeBuilder, IKVM.Internal.MapXml.Root.filename);
					}

					if(c.Deprecated)
					{
						AttributeHelper.SetDeprecatedAttribute(typeBuilder);
					}

					if(baseIsSealed)
					{
						AttributeHelper.SetModifiers(typeBuilder, (Modifiers)c.Modifiers);
					}

					if(c.scope == IKVM.Internal.MapXml.Scope.Public)
					{
						// FXBUG we would like to emit an attribute with a Type argument here, but that doesn't work because
						// of a bug in SetCustomAttribute that causes type arguments to be serialized incorrectly (if the type
						// is in the same assembly). Normally we use AttributeHelper.FreezeDry to get around this, but that doesn't
						// work in this case (no attribute is emitted at all). So we work around by emitting a string instead
						ConstructorInfo remappedClassAttribute = typeof(RemappedClassAttribute).GetConstructor(new Type[] { typeof(string), typeof(Type) });
						classLoader.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute, new object[] { name, shadowType }));

						ConstructorInfo remappedTypeAttribute = typeof(RemappedTypeAttribute).GetConstructor(new Type[] { typeof(Type) });
						typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute, new object[] { shadowType }));
					}

					// HACK because of the above FXBUG that prevents us from making the type both abstract and sealed,
					// we need to emit a private constructor (otherwise reflection will automatically generate a public
					// default constructor, another lame feature)
					if(baseIsSealed)
					{
						ConstructorBuilder cb = typeBuilder.DefineConstructor(MethodAttributes.Private, CallingConventions.Standard, Type.EmptyTypes);
						ILGenerator ilgen = cb.GetILGenerator();
						// lazyman's way to create a type-safe bogus constructor
						ilgen.Emit(OpCodes.Ldnull);
						ilgen.Emit(OpCodes.Throw);
					}

					ArrayList methods = new ArrayList();

					if(c.Constructors != null)
					{
						foreach(IKVM.Internal.MapXml.Constructor m in c.Constructors)
						{
							methods.Add(new RemappedConstructorWrapper(this, m));
						}
					}

					if(c.Methods != null)
					{
						// TODO we should also add methods from our super classes (e.g. Throwable should have Object's methods)
						foreach(IKVM.Internal.MapXml.Method m in c.Methods)
						{
							methods.Add(new RemappedMethodWrapper(this, m, map));
						}
					}

					SetMethods((MethodWrapper[])methods.ToArray(typeof(MethodWrapper)));
				}

				abstract class RemappedMethodBaseWrapper : MethodWrapper
				{
					internal RemappedMethodBaseWrapper(RemapperTypeWrapper typeWrapper, string name, string sig, Modifiers modifiers)
						: base(typeWrapper, name, sig, null, null, null, modifiers, MemberFlags.None)
					{
					}

					internal abstract MethodBase DoLink();

					internal abstract void Finish();

					internal static void AddDeclaredExceptions(MethodBase mb, IKVM.Internal.MapXml.Throws[] throws)
					{
						if(throws != null)
						{
							string[] exceptions = new string[throws.Length];
							for(int i = 0; i < exceptions.Length; i++)
							{
								exceptions[i] = throws[i].Class;
							}
							AttributeHelper.SetThrowsAttribute(mb, exceptions);
						}
					}
				}

				sealed class RemappedConstructorWrapper : RemappedMethodBaseWrapper
				{
					private IKVM.Internal.MapXml.Constructor m;
					private MethodBuilder mbHelper;

					internal RemappedConstructorWrapper(RemapperTypeWrapper typeWrapper, IKVM.Internal.MapXml.Constructor m)
						: base(typeWrapper, "<init>", m.Sig, (Modifiers)m.Modifiers)
					{
						this.m = m;
					}

					internal override void EmitCall(ILGenerator ilgen)
					{
						ilgen.Emit(OpCodes.Call, (ConstructorInfo)GetMethod());
					}

					internal override void EmitNewobj(ILGenerator ilgen)
					{
						if(mbHelper != null)
						{
							ilgen.Emit(OpCodes.Call, mbHelper);
						}
						else
						{
							ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)GetMethod());
						}
					}

					internal override MethodBase DoLink()
					{
						MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers);
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
						Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);

						ConstructorBuilder cbCore = null;

						if(typeWrapper.shadowType.IsSealed)
						{
							mbHelper = typeWrapper.typeBuilder.DefineMethod("newhelper", attr | MethodAttributes.Static, CallingConventions.Standard, typeWrapper.shadowType, paramTypes);
							AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers);
							AttributeHelper.SetNameSig(mbHelper, "<init>", m.Sig);
							AddDeclaredExceptions(mbHelper, m.throws);
							if(m.Deprecated)
							{
								AttributeHelper.SetDeprecatedAttribute(mbHelper);
							}
						}
						else
						{
							cbCore = typeWrapper.typeBuilder.DefineConstructor(attr, CallingConventions.Standard, paramTypes);
							AddDeclaredExceptions(cbCore, m.throws);
							if(m.Deprecated)
							{
								AttributeHelper.SetDeprecatedAttribute(cbCore);
							}
						}
						return cbCore;
					}
				
					internal override void Finish()
					{
						// TODO we should insert method tracing (if enabled)

						Type[] paramTypes = this.GetParametersForDefineMethod();

						ConstructorBuilder cbCore = GetMethod() as ConstructorBuilder;

						if(cbCore != null)
						{
							ILGenerator ilgen = cbCore.GetILGenerator();
							// TODO we need to support ghost (and other funky?) parameter types
							if(m.body != null)
							{
								// TODO do we need return type conversion here?
								m.body.Emit(ilgen);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldarg_0);
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
								}
								if(m.redirect != null)
								{
									throw new NotImplementedException();
								}
								else
								{
									ConstructorInfo baseCon = DeclaringType.TypeAsTBD.GetConstructor(paramTypes);
									if(baseCon == null)
									{
										// TODO better error handling
										throw new InvalidOperationException("base class constructor not found: " + DeclaringType.Name + ".<init>" + m.Sig);
									}
									ilgen.Emit(OpCodes.Call, baseCon);
								}
								ilgen.Emit(OpCodes.Ret);
							}
							ilgen.EmitLineNumberTable(cbCore);
						}

						if(mbHelper != null)
						{
							ILGenerator ilgen = mbHelper.GetILGenerator();
							if(m.redirect != null)
							{
								if(m.redirect.Type != "static" || m.redirect.Class == null || m.redirect.Name == null || m.redirect.Sig == null)
								{
									throw new NotImplementedException();
								}
								Type[] redirParamTypes = ClassLoaderWrapper.GetBootstrapClassLoader().ArgTypeListFromSig(m.redirect.Sig);
								for(int i = 0; i < redirParamTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								// HACK if the class name contains a comma, we assume it is a .NET type
								if(m.redirect.Class.IndexOf(',') >= 0)
								{
									Type type = Type.GetType(m.redirect.Class, true);
									MethodInfo mi = type.GetMethod(m.redirect.Name, redirParamTypes);
									if(mi == null)
									{
										throw new InvalidOperationException();
									}
									ilgen.Emit(OpCodes.Call, mi);
								}
								else
								{
									TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(m.redirect.Class);
									MethodWrapper mw = tw.GetMethodWrapper(m.redirect.Name, m.redirect.Sig, false);
									if(mw == null)
									{
										throw new InvalidOperationException();
									}
									mw.Link();
									mw.EmitCall(ilgen);
								}
								// TODO we may need a cast here (or a stack to return type conversion)
								ilgen.Emit(OpCodes.Ret);
							}
							else if(m.alternateBody != null)
							{
								m.alternateBody.Emit(ilgen);
							}
							else if(m.body != null)
							{
								// <body> doesn't make sense for helper constructors (which are actually factory methods)
								throw new InvalidOperationException();
							}
							else
							{
								ConstructorInfo baseCon = DeclaringType.TypeAsTBD.GetConstructor(paramTypes);
								if(baseCon == null)
								{
									// TODO better error handling
									throw new InvalidOperationException("constructor not found: " + DeclaringType.Name + ".<init>" + m.Sig);
								}
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								ilgen.Emit(OpCodes.Newobj, baseCon);
								ilgen.Emit(OpCodes.Ret);
							}
							ilgen.EmitLineNumberTable(mbHelper);
						}
					}
				}

				sealed class RemappedMethodWrapper : RemappedMethodBaseWrapper
				{
					private IKVM.Internal.MapXml.Method m;
					private IKVM.Internal.MapXml.Root map;
					private MethodBuilder mbHelper;
					private ArrayList overriders = new ArrayList();

					internal RemappedMethodWrapper(RemapperTypeWrapper typeWrapper, IKVM.Internal.MapXml.Method m, IKVM.Internal.MapXml.Root map)
						: base(typeWrapper, m.Name, m.Sig, (Modifiers)m.Modifiers)
					{
						this.m = m;
						this.map = map;
					}

					internal override void EmitCall(ILGenerator ilgen)
					{
						ilgen.Emit(OpCodes.Call, (MethodInfo)GetMethod());
					}

					internal override void EmitCallvirt(ILGenerator ilgen)
					{
						if(mbHelper != null)
						{
							ilgen.Emit(OpCodes.Call, mbHelper);
						}
						else
						{
							ilgen.Emit(OpCodes.Callvirt, (MethodInfo)GetMethod());
						}
					}

					internal override MethodBase DoLink()
					{
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;

						if(typeWrapper.IsInterface)
						{
							if(m.@override == null)
							{
								throw new InvalidOperationException(typeWrapper.Name + "." + m.Name + m.Sig);
							}
							MethodInfo interfaceMethod = typeWrapper.shadowType.GetMethod(m.@override.Name, typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig));
							if(interfaceMethod == null)
							{
								throw new InvalidOperationException(typeWrapper.Name + "." + m.Name + m.Sig);
							}
							if(m.throws != null)
							{
								// TODO we need a place to stick the declared exceptions
								throw new NotImplementedException();
							}
							// if any of the remapped types has a body for this interface method, we need a helper method
							// to special invocation through this interface for that type
							ArrayList specialCases = null;
							foreach(IKVM.Internal.MapXml.Class c in map.assembly)
							{
								if(c.Methods != null)
								{
									foreach(IKVM.Internal.MapXml.Method mm in c.Methods)
									{
										if(mm.Name == m.Name && mm.Sig == m.Sig && mm.body != null)
										{
											if(specialCases == null)
											{
												specialCases = new ArrayList();
											}
											specialCases.Add(c);
											break;
										}
									}
								}
							}
							CustomAttributeBuilder cab = new CustomAttributeBuilder(typeof(RemappedInterfaceMethodAttribute).GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { m.Name, m.@override.Name } );
							typeWrapper.typeBuilder.SetCustomAttribute(cab);
							MethodBuilder helper = null;
							if(specialCases != null)
							{
								Type[] temp = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);
								Type[] argTypes = new Type[temp.Length + 1];
								temp.CopyTo(argTypes, 1);
								argTypes[0] = typeWrapper.shadowType;
								if(typeWrapper.helperTypeBuilder == null)
								{
									// FXBUG we use a nested helper class, because Reflection.Emit won't allow us to add a static method to the interface
									typeWrapper.helperTypeBuilder = typeWrapper.typeBuilder.DefineNestedType("__Helper", TypeAttributes.NestedPublic | TypeAttributes.Class);
									AttributeHelper.HideFromJava(typeWrapper.helperTypeBuilder);
								}
								helper = typeWrapper.helperTypeBuilder.DefineMethod(m.Name, MethodAttributes.Public | MethodAttributes.Static, typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig).TypeAsParameterType, argTypes);
								ILGenerator ilgen = helper.GetILGenerator();
								foreach(IKVM.Internal.MapXml.Class c in specialCases)
								{
									TypeWrapper tw = typeWrapper.GetClassLoader().LoadClassByDottedName(c.Name);
									ilgen.Emit(OpCodes.Ldarg_0);
									ilgen.Emit(OpCodes.Isinst, tw.TypeAsTBD);
									ilgen.Emit(OpCodes.Dup);
									Label label = ilgen.DefineLabel();
									ilgen.Emit(OpCodes.Brfalse_S, label);
									for(int i = 1; i < argTypes.Length; i++)
									{
										ilgen.Emit(OpCodes.Ldarg, (short)i);
									}
									MethodWrapper mw = tw.GetMethodWrapper(m.Name, m.Sig, false);
									mw.Link();
									mw.EmitCallvirt(ilgen);
									ilgen.Emit(OpCodes.Ret);
									ilgen.MarkLabel(label);
									ilgen.Emit(OpCodes.Pop);
								}
								for(int i = 0; i < argTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								ilgen.Emit(OpCodes.Callvirt, interfaceMethod);
								ilgen.Emit(OpCodes.Ret);
							}
							mbHelper = helper;
							return interfaceMethod;
						}
						else
						{
							MethodBuilder mbCore = null;
							Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);
							Type retType = typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig).TypeAsParameterType;

							if(typeWrapper.shadowType.IsSealed && (m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) == 0)
							{
								// skip instance methods in sealed types, but we do need to add them to the overriders
								if(typeWrapper.BaseTypeWrapper != null && (m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Private) == 0)
								{
									RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(m.Name, m.Sig, true) as RemappedMethodWrapper;
									if(baseMethod != null &&
										!baseMethod.IsFinal &&
										!baseMethod.IsPrivate &&
										(baseMethod.m.@override != null ||
										baseMethod.m.redirect != null ||
										baseMethod.m.body != null ||
										baseMethod.m.alternateBody != null))
									{
										baseMethod.overriders.Add(typeWrapper);
									}
								}
							}
							else
							{
								MethodInfo overrideMethod = null;
								MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers);
								if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) != 0)
								{
									attr |= MethodAttributes.Static;
								}
								else if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Private) == 0 && (m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Final) == 0)
								{
									attr |= MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
									if(typeWrapper.BaseTypeWrapper != null)
									{
										RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(m.Name, m.Sig, true) as RemappedMethodWrapper;
										if(baseMethod != null)
										{
											baseMethod.overriders.Add(typeWrapper);
											if(baseMethod.m.@override != null)
											{
												overrideMethod = typeWrapper.BaseTypeWrapper.TypeAsTBD.GetMethod(baseMethod.m.@override.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
												if(overrideMethod == null)
												{
													throw new InvalidOperationException();
												}
											}
										}
									}
								}
								mbCore = typeWrapper.typeBuilder.DefineMethod(m.Name, attr, CallingConventions.Standard, retType, paramTypes);
								if(overrideMethod != null)
								{
									typeWrapper.typeBuilder.DefineMethodOverride(mbCore, overrideMethod);
								}
								AddDeclaredExceptions(mbCore, m.throws);
								if(m.Deprecated)
								{
									AttributeHelper.SetDeprecatedAttribute(mbCore);
								}
							}

							if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) == 0)
							{
								// instance methods must have an instancehelper method
								MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.Static;
								// NOTE instancehelpers for protected methods are made public,
								// because cli.System.Object derived types can call protected methods
								if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Protected) != 0)
								{
									attr &= ~MethodAttributes.MemberAccessMask;
									attr |= MethodAttributes.Public;
									// mark with specialname, so that tools (hopefully) won't show them
									attr |= MethodAttributes.SpecialName;
								}
								Type[] exParamTypes = new Type[paramTypes.Length + 1];
								Array.Copy(paramTypes, 0, exParamTypes, 1, paramTypes.Length);
								exParamTypes[0] = typeWrapper.shadowType;
								mbHelper = typeWrapper.typeBuilder.DefineMethod("instancehelper_" + m.Name, attr, CallingConventions.Standard, retType, exParamTypes);
								AttributeHelper.SetEditorBrowsableNever(mbHelper);
								AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers);
								AttributeHelper.SetNameSig(mbHelper, m.Name, m.Sig);
								AddDeclaredExceptions(mbHelper, m.throws);
								if(m.Deprecated)
								{
									AttributeHelper.SetDeprecatedAttribute(mbHelper);
								}
							}
							return mbCore;
						}
					}

					internal override void Finish()
					{
						// TODO we should insert method tracing (if enabled)
						Type[] paramTypes = this.GetParametersForDefineMethod();

						MethodBuilder mbCore = GetMethod() as MethodBuilder;

						// NOTE sealed types don't have instance methods (only instancehelpers)
						if(mbCore != null)
						{
							ILGenerator ilgen = mbCore.GetILGenerator();
							MethodInfo baseMethod = null;
							if(m.@override != null)
							{
								baseMethod = DeclaringType.TypeAsTBD.GetMethod(m.@override.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
								if(baseMethod == null)
								{
									throw new InvalidOperationException();
								}
								((TypeBuilder)DeclaringType.TypeAsBaseType).DefineMethodOverride(mbCore, baseMethod);
							}
							// TODO we need to support ghost (and other funky?) parameter types
							if(m.body != null)
							{
								// we manually walk the instruction list, because we need to special case the ret instructions
								Hashtable context = new Hashtable();
								foreach(IKVM.Internal.MapXml.Instruction instr in m.body.invoke)
								{
									if(instr is IKVM.Internal.MapXml.Ret)
									{
										this.ReturnType.EmitConvStackToParameterType(ilgen, null);
									}
									instr.Generate(context, ilgen);
								}
							}
							else
							{
								if(m.redirect != null && m.redirect.LineNumber != -1)
								{
									ilgen.SetLineNumber((ushort)m.redirect.LineNumber);
								}
								int thisOffset = 0;
								if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) == 0)
								{
									thisOffset = 1;
									ilgen.Emit(OpCodes.Ldarg_0);
								}
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + thisOffset));
								}
								if(m.redirect != null)
								{
									EmitRedirect(DeclaringType.TypeAsTBD, ilgen);
								}
								else
								{
									if(baseMethod == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									ilgen.Emit(OpCodes.Call, baseMethod);
								}
								this.ReturnType.EmitConvStackToParameterType(ilgen, null);
								ilgen.Emit(OpCodes.Ret);
							}
							ilgen.EmitLineNumberTable(mbCore);
						}

						// NOTE static methods don't have helpers
						if(mbHelper != null)
						{
							ILGenerator ilgen = mbHelper.GetILGenerator();
							// check "this" for null
							if(m.@override != null && m.redirect == null && m.body == null && m.alternateBody == null)
							{
								// we're going to be calling the overridden version, so we don't need the null check
							}
							else
							{
								ilgen.Emit(OpCodes.Ldarg_0);
								EmitHelper.NullCheck(ilgen);
							}
							if(mbCore != null && 
								(m.@override == null || m.redirect != null) &&
								(m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Private) == 0 && (m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Final) == 0)
							{
								// TODO we should have a way to supress this for overridden methods
								ilgen.Emit(OpCodes.Ldarg_0);
								ilgen.Emit(OpCodes.Isinst, DeclaringType.TypeAsBaseType);
								ilgen.Emit(OpCodes.Dup);
								Label skip = ilgen.DefineLabel();
								ilgen.Emit(OpCodes.Brfalse_S, skip);
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
								}
								ilgen.Emit(OpCodes.Callvirt, mbCore);
								this.ReturnType.EmitConvStackToParameterType(ilgen, null);
								ilgen.Emit(OpCodes.Ret);
								ilgen.MarkLabel(skip);
								ilgen.Emit(OpCodes.Pop);
							}
							foreach(RemapperTypeWrapper overrider in overriders)
							{
								RemappedMethodWrapper mw = (RemappedMethodWrapper)overrider.GetMethodWrapper(Name, Signature, false);
								if(mw.m.redirect == null && mw.m.body == null && mw.m.alternateBody == null)
								{
									// the overridden method doesn't actually do anything special (that means it will end
									// up calling the .NET method it overrides), so we don't need to special case this
								}
								else
								{
									ilgen.Emit(OpCodes.Ldarg_0);
									ilgen.Emit(OpCodes.Isinst, overrider.TypeAsTBD);
									ilgen.Emit(OpCodes.Dup);
									Label skip = ilgen.DefineLabel();
									ilgen.Emit(OpCodes.Brfalse_S, skip);
									for(int i = 0; i < paramTypes.Length; i++)
									{
										ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
									}
									mw.Link();
									mw.EmitCallvirt(ilgen);
									this.ReturnType.EmitConvStackToParameterType(ilgen, null);
									ilgen.Emit(OpCodes.Ret);
									ilgen.MarkLabel(skip);
									ilgen.Emit(OpCodes.Pop);
								}
							}
							if(m.body != null || m.alternateBody != null)
							{
								IKVM.Internal.MapXml.InstructionList body = m.alternateBody == null ? m.body : m.alternateBody;
								// we manually walk the instruction list, because we need to special case the ret instructions
								Hashtable context = new Hashtable();
								foreach(IKVM.Internal.MapXml.Instruction instr in body.invoke)
								{
									if(instr is IKVM.Internal.MapXml.Ret)
									{
										this.ReturnType.EmitConvStackToParameterType(ilgen, null);
									}
									instr.Generate(context, ilgen);
								}
							}
							else
							{
								if(m.redirect != null && m.redirect.LineNumber != -1)
								{
									ilgen.SetLineNumber((ushort)m.redirect.LineNumber);
								}
								Type shadowType = ((RemapperTypeWrapper)DeclaringType).shadowType;
								for(int i = 0; i < paramTypes.Length + 1; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								if(m.redirect != null)
								{
									EmitRedirect(shadowType, ilgen);
								}
								else if(m.@override != null)
								{
									MethodInfo baseMethod = shadowType.GetMethod(m.@override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
									if(baseMethod == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									ilgen.Emit(OpCodes.Callvirt, baseMethod);
								}
								else
								{
									RemappedMethodWrapper baseMethod = DeclaringType.BaseTypeWrapper.GetMethodWrapper(Name, Signature, true) as RemappedMethodWrapper;
									if(baseMethod == null || baseMethod.m.@override == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									MethodInfo overrideMethod = shadowType.GetMethod(baseMethod.m.@override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
									if(overrideMethod == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									ilgen.Emit(OpCodes.Callvirt, overrideMethod);
								}
								this.ReturnType.EmitConvStackToParameterType(ilgen, null);
								ilgen.Emit(OpCodes.Ret);
							}
							ilgen.EmitLineNumberTable(mbHelper);
						}

						// do we need a helper for non-virtual reflection invocation?
						if(m.nonvirtualAlternateBody != null || (m.@override != null && overriders.Count > 0))
						{
							RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
							Type[] argTypes = new Type[paramTypes.Length + 1];
							argTypes[0] = typeWrapper.TypeAsParameterType;
							this.GetParametersForDefineMethod().CopyTo(argTypes, 1);
							MethodBuilder mb = typeWrapper.typeBuilder.DefineMethod("nonvirtualhelper/" + this.Name, MethodAttributes.Private | MethodAttributes.Static, this.ReturnTypeForDefineMethod, argTypes);
							AttributeHelper.HideFromJava(mb);
							ILGenerator ilgen = mb.GetILGenerator();
							if(m.nonvirtualAlternateBody != null)
							{
								m.nonvirtualAlternateBody.Emit(ilgen);
							}
							else
							{
								Type shadowType = ((RemapperTypeWrapper)DeclaringType).shadowType;
								MethodInfo baseMethod = shadowType.GetMethod(m.@override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
								if(baseMethod == null)
								{
									throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
								}
								ilgen.Emit(OpCodes.Ldarg_0);
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
								}
								ilgen.Emit(OpCodes.Call, baseMethod);
								ilgen.Emit(OpCodes.Ret);
							}
						}
					}

					private void EmitRedirect(Type baseType, ILGenerator ilgen)
					{
						string redirName = m.redirect.Name;
						string redirSig = m.redirect.Sig;
						if(redirName == null)
						{
							redirName = m.Name;
						}
						if(redirSig == null)
						{
							redirSig = m.Sig;
						}
						ClassLoaderWrapper classLoader = ClassLoaderWrapper.GetBootstrapClassLoader();
						// HACK if the class name contains a comma, we assume it is a .NET type
						if(m.redirect.Class == null || m.redirect.Class.IndexOf(',') >= 0)
						{
							// TODO better error handling
							Type type = m.redirect.Class == null ? baseType : Type.GetType(m.redirect.Class, true);
							Type[] redirParamTypes = classLoader.ArgTypeListFromSig(redirSig);
							MethodInfo mi = type.GetMethod(m.redirect.Name, redirParamTypes);
							if(mi == null)
							{
								throw new InvalidOperationException();
							}
							ilgen.Emit(OpCodes.Call, mi);
						}
						else
						{
							TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(m.redirect.Class);
							MethodWrapper mw = tw.GetMethodWrapper(redirName, redirSig, false);
							if(mw == null)
							{
								throw new InvalidOperationException("Missing redirect method: " + tw.Name + "." + redirName + redirSig);
							}
							mw.Link();
							mw.EmitCall(ilgen);
						}
						if(!classLoader.RetTypeWrapperFromSig(redirSig).IsAssignableTo(this.ReturnType))
						{
							// NOTE we're passing a null context, this is safe because the return type
							// should always be loadable
							System.Diagnostics.Debug.Assert(!this.ReturnType.IsUnloadable);
							this.ReturnType.EmitCheckcast(null, ilgen);
						}
					}
				}

				internal void Process2ndPass(IKVM.Internal.MapXml.Root map)
				{
					IKVM.Internal.MapXml.Class c = classDef;
					TypeBuilder tb = typeBuilder;
					bool baseIsSealed = shadowType.IsSealed;

					if(c.Interfaces != null)
					{
						interfaceWrappers = new TypeWrapper[c.Interfaces.Length];
						for(int i = 0; i < c.Interfaces.Length; i++)
						{
							TypeWrapper ifaceTypeWrapper = ClassLoaderWrapper.LoadClassCritical(c.Interfaces[i].Name);
							interfaceWrappers[i] = ifaceTypeWrapper;
							if(!baseIsSealed)
							{
								tb.AddInterfaceImplementation(ifaceTypeWrapper.TypeAsBaseType);
							}
						}
						AttributeHelper.SetImplementsAttribute(tb, interfaceWrappers);
					}
					else
					{
						interfaceWrappers = TypeWrapper.EmptyArray;
					}

					ArrayList fields = new ArrayList();

					// TODO fields should be moved to the RemapperTypeWrapper constructor as well
					if(c.Fields != null)
					{
						foreach(IKVM.Internal.MapXml.Field f in c.Fields)
						{
							if(f.redirect != null)
							{
								TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(f.redirect.Class);
								MethodWrapper method = tw.GetMethodWrapper(f.redirect.Name, f.redirect.Sig, false);
								if(method == null || !method.IsStatic)
								{
									// TODO better error handling
									throw new InvalidOperationException("remapping field: " + f.Name + f.Sig + " not found");
								}
								// TODO emit an static helper method that enables access to the field at runtime
								method.Link();
								fields.Add(new GetterFieldWrapper(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), null, f.Name, f.Sig, (Modifiers)f.Modifiers, (MethodInfo)method.GetMethod()));
							}
							else if((f.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) != 0)
							{
								FieldAttributes attr = MapFieldAccessModifiers(f.Modifiers) | FieldAttributes.Static;
								if(f.Constant != null)
								{
									attr |= FieldAttributes.Literal;
								}
								else if((f.Modifiers & IKVM.Internal.MapXml.MapModifiers.Final) != 0)
								{
									attr |= FieldAttributes.InitOnly;
								}
								FieldBuilder fb = tb.DefineField(f.Name, GetClassLoader().FieldTypeWrapperFromSig(f.Sig).TypeAsFieldType, attr);
								object constant;
								if(f.Constant != null)
								{
									switch(f.Sig[0])
									{
										case 'J':
											constant = long.Parse(f.Constant);
											break;
										default:
											// TODO support other types
											throw new NotImplementedException("remapped constant field of type: " + f.Sig);
									}
									fb.SetConstant(constant);
									fields.Add(new ConstantFieldWrapper(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), f.Name, f.Sig, (Modifiers)f.Modifiers, fb, constant));
								}
								else
								{
									fields.Add(FieldWrapper.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), fb, f.Name, f.Sig, (Modifiers)f.Modifiers));
								}
								if(f.Deprecated)
								{
									AttributeHelper.SetDeprecatedAttribute(fb);
								}
							}
							else
							{
								// TODO we should support adding arbitrary instance fields (the runtime will have to use
								// a weak identity hashtable to store the extra information for subclasses that don't extend our stub)
								throw new NotImplementedException(this.Name + "." + f.Name + f.Sig);
							}
						}
					}
					SetFields((FieldWrapper[])fields.ToArray(typeof(FieldWrapper)));
				}

				internal void Process3rdPass()
				{
					foreach(RemappedMethodBaseWrapper m in GetMethods())
					{
						m.Link();
					}
				}

				internal void Process4thPass(ICollection remappedTypes)
				{
					foreach(RemappedMethodBaseWrapper m in GetMethods())
					{
						m.Finish();
					}

					if(classDef.Clinit != null)
					{
						ConstructorBuilder cb = typeBuilder.DefineTypeInitializer();
						ILGenerator ilgen = cb.GetILGenerator();
						// TODO emit code to make sure super class is initialized
						classDef.Clinit.body.Emit(ilgen);
					}

					// FXBUG because the AppDomain.TypeResolve event doesn't work correctly for inner classes,
					// we need to explicitly finish the interface we implement (if they are ghosts, we need the nested __Interface type)
					if(classDef.Interfaces != null)
					{
						foreach(IKVM.Internal.MapXml.Interface iface in classDef.Interfaces)
						{
							GetClassLoader().LoadClassByDottedName(iface.Name).Finish();
						}
					}

					CreateShadowInstanceOf(remappedTypes);
					CreateShadowCheckCast(remappedTypes);

					if(!shadowType.IsInterface)
					{
						// For all inherited methods, we emit a method that hide the inherited method and
						// annotate it with EditorBrowsableAttribute(EditorBrowsableState.Never) to make
						// sure the inherited methods don't show up in Intellisense.
						Hashtable methods = new Hashtable();
						foreach(MethodInfo mi in typeBuilder.BaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
						{
							ParameterInfo[] paramInfo = mi.GetParameters();
							Type[] paramTypes = new Type[paramInfo.Length];
							for(int i = 0; i < paramInfo.Length; i++)
							{
								paramTypes[i] = paramInfo[i].ParameterType;
							}
							MethodBuilder mb = typeBuilder.DefineMethod(mi.Name, mi.Attributes & ~(MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot), mi.ReturnType, paramTypes);
							AttributeHelper.HideFromJava(mb);
							AttributeHelper.SetEditorBrowsableNever(mb);
							ILGenerator ilgen = mb.GetILGenerator();
							for(int i = 0; i < paramTypes.Length; i++)
							{
								ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
							}
							if(!mi.IsStatic)
							{
								ilgen.Emit(OpCodes.Ldarg_S, (byte)paramTypes.Length);
							}
							ilgen.Emit(OpCodes.Call, mi);
							ilgen.Emit(OpCodes.Ret);
							methods[mb.Name] = mb;
						}
						foreach(PropertyInfo pi in typeBuilder.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
						{
							ParameterInfo[] paramInfo = pi.GetIndexParameters();
							Type[] paramTypes = new Type[paramInfo.Length];
							for(int i = 0; i < paramInfo.Length; i++)
							{
								paramTypes[i] = paramInfo[i].ParameterType;
							}
							PropertyBuilder pb = typeBuilder.DefineProperty(pi.Name, pi.Attributes, pi.PropertyType, paramTypes);
							if(pi.CanRead)
							{
								pb.SetGetMethod((MethodBuilder)methods[pi.GetGetMethod().Name]);
							}
							if(pi.CanWrite)
							{
								pb.SetSetMethod((MethodBuilder)methods[pi.GetSetMethod().Name]);
							}
							AttributeHelper.SetEditorBrowsableNever(pb);
						}
					}

					typeBuilder.CreateType();
					if(helperTypeBuilder != null)
					{
						helperTypeBuilder.CreateType();
					}
				}

				private void CreateShadowInstanceOf(ICollection remappedTypes)
				{
					// FXBUG .NET 1.1 doesn't allow static methods on interfaces
					if(typeBuilder.IsInterface)
					{
						return;
					}
					MethodAttributes attr = MethodAttributes.SpecialName | MethodAttributes.Public | MethodAttributes.Static;
					MethodBuilder mb = typeBuilder.DefineMethod("__<instanceof>", attr, typeof(bool), new Type[] { typeof(object) });
					AttributeHelper.HideFromJava(mb);
					AttributeHelper.SetEditorBrowsableNever(mb);
					ILGenerator ilgen = mb.GetILGenerator();

					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Isinst, shadowType);
					Label retFalse = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Brfalse_S, retFalse);

					if(!shadowType.IsSealed)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, typeBuilder);
						ilgen.Emit(OpCodes.Brtrue_S, retFalse);
					}

					if(shadowType == typeof(object))
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, typeof(Array));
						ilgen.Emit(OpCodes.Brtrue_S, retFalse);
					}

					foreach(RemapperTypeWrapper r in remappedTypes)
					{
						if(!r.shadowType.IsInterface && r.shadowType.IsSubclassOf(shadowType))
						{
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Isinst, r.shadowType);
							ilgen.Emit(OpCodes.Brtrue_S, retFalse);
						}
					}
					ilgen.Emit(OpCodes.Ldc_I4_1);
					ilgen.Emit(OpCodes.Ret);

					ilgen.MarkLabel(retFalse);
					ilgen.Emit(OpCodes.Ldc_I4_0);
					ilgen.Emit(OpCodes.Ret);
				}

				private void CreateShadowCheckCast(ICollection remappedTypes)
				{
					// FXBUG .NET 1.1 doesn't allow static methods on interfaces
					if(typeBuilder.IsInterface)
					{
						return;
					}
					MethodAttributes attr = MethodAttributes.SpecialName | MethodAttributes.Public | MethodAttributes.Static;
					MethodBuilder mb = typeBuilder.DefineMethod("__<checkcast>", attr, shadowType, new Type[] { typeof(object) });
					AttributeHelper.HideFromJava(mb);
					AttributeHelper.SetEditorBrowsableNever(mb);
					ILGenerator ilgen = mb.GetILGenerator();

					Label fail = ilgen.DefineLabel();
					bool hasfail = false;

					if(!shadowType.IsSealed)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, typeBuilder);
						ilgen.Emit(OpCodes.Brtrue_S, fail);
						hasfail = true;
					}

					if(shadowType == typeof(object))
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, typeof(Array));
						ilgen.Emit(OpCodes.Brtrue_S, fail);
						hasfail = true;
					}

					foreach(RemapperTypeWrapper r in remappedTypes)
					{
						if(!r.shadowType.IsInterface && r.shadowType.IsSubclassOf(shadowType))
						{
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Isinst, r.shadowType);
							ilgen.Emit(OpCodes.Brtrue_S, fail);
							hasfail = true;
						}
					}
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Castclass, shadowType);
					ilgen.Emit(OpCodes.Ret);

					if(hasfail)
					{
						ilgen.MarkLabel(fail);
						ilgen.ThrowException(typeof(InvalidCastException));
					}
				}

				internal override MethodBase LinkMethod(MethodWrapper mw)
				{
					return ((RemappedMethodBaseWrapper)mw).DoLink();
				}

				internal override TypeWrapper DeclaringTypeWrapper
				{
					get
					{
						// at the moment we don't support nested remapped types
						return null;
					}
				}

				internal override void Finish(bool forDebugSave)
				{
				}

				internal override TypeWrapper[] InnerClasses
				{
					get
					{
						return null;
					}
				}

				internal override TypeWrapper[] Interfaces
				{
					get
					{
						return interfaceWrappers;
					}
				}

				internal override Type TypeAsTBD
				{
					get
					{
						return shadowType;
					}
				}

				internal override Type TypeAsBaseType
				{
					get
					{
						return typeBuilder;
					}
				}

				internal override TypeBuilder TypeAsBuilder
				{
					get
					{
						return typeBuilder;
					}
				}

				internal override bool IsMapUnsafeException
				{
					get
					{
						// any remapped exceptions are automatically unsafe
						return shadowType == typeof(Exception) || shadowType.IsSubclassOf(typeof(Exception));
					}
				}
			}

			internal void EmitRemappedTypes(IKVM.Internal.MapXml.Root map)
			{
				Tracer.Info(Tracer.Compiler, "Emit remapped types");

				// 1st pass, put all types in remapped to make them loadable
				foreach(IKVM.Internal.MapXml.Class c in map.assembly)
				{
					if(c.Shadows != null)
					{
						remapped.Add(c.Name, new RemapperTypeWrapper(this, c, map));
					}
				}

				DynamicTypeWrapper.SetupGhosts(map);

				// 2nd pass, resolve interfaces, publish methods/fields
				foreach(IKVM.Internal.MapXml.Class c in map.assembly)
				{
					if(c.Shadows != null)
					{
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)remapped[c.Name];
						typeWrapper.Process2ndPass(map);
					}
				}
			}

			internal void FinishRemappedTypes()
			{
				// 3rd pass, link the methods. Note that a side effect of the linking is the
				// twiddling with the overriders array in the base methods, so we need to do this
				// as a separate pass before we compile the methods
				foreach(RemapperTypeWrapper typeWrapper in remapped.Values)
				{
					typeWrapper.Process3rdPass();
				}
				// 4th pass, implement methods/fields and bake the type
				foreach(RemapperTypeWrapper typeWrapper in remapped.Values)
				{
					typeWrapper.Process4thPass(remapped.Values);
				}
			}
		}

		public class CompilerOptions
		{
			public string path;
			public string keyfilename;
			public string keycontainer;
			public string version;
			public bool targetIsModule;
			public string assembly;
			public string mainClass;
			public ApartmentState apartment;
			public PEFileKinds target;
			public bool guessFileKind;
			public byte[][] classes;
			public string[] references;
			public bool nojni;
			public Hashtable resources;
			public string[] classesToExclude;
			public string remapfile;
			public Hashtable props;
			public bool noglobbing;
			public bool nostacktraceinfo;
			public bool removeUnusedFields;
			public bool monoBugWorkaround;
		}

		public static int Compile(CompilerOptions options)
		{
			Tracer.Info(Tracer.Compiler, "JVM.Compile path: {0}, assembly: {1}", options.path, options.assembly);
			isStaticCompiler = true;
			noJniStubs = options.nojni;
			noStackTraceInfo = options.nostacktraceinfo;
			monoBugWorkaround = options.monoBugWorkaround;
			foreach(string r in options.references)
			{
				try
				{
					Assembly reference = Assembly.LoadFrom(r);
					if(reference == null)
					{
						Console.Error.WriteLine("Error: reference not found: {0}", r);
						return 1;
					}
					Tracer.Info(Tracer.Compiler, "Loaded reference assembly: {0}", reference.FullName);
				}
				catch(Exception x)
				{
					Console.Error.WriteLine("Error: invalid reference: {0} ({1})", r, x.Message);
					return 1;
				}
			}
			Hashtable h = new Hashtable();
			Tracer.Info(Tracer.Compiler, "Parsing class files");
			for(int i = 0; i < options.classes.Length; i++)
			{
				ClassFile f;
				try
				{
					f = new ClassFile(options.classes[i], 0, options.classes[i].Length, null, true);
				}
				catch(UnsupportedClassVersionError x)
				{
					Console.Error.WriteLine("Error: unsupported class file version: {0}", x.Message);
					return 1;
				}
				catch(ClassFormatError x)
				{
					Console.Error.WriteLine("Error: invalid class file: {0}", x.Message);
					return 1;
				}
				string name = f.Name;
				bool excluded = false;
				for(int j = 0; j < options.classesToExclude.Length; j++)
				{
					if(Regex.IsMatch(name, options.classesToExclude[j]))
					{
						excluded = true;
						break;
					}
				}
				if(h.ContainsKey(name))
				{
					Console.Error.WriteLine("Warning: duplicate class name: {0}", name);
					excluded = true;
				}
				if(!excluded)
				{
					if(options.removeUnusedFields)
					{
						f.RemoveUnusedFields();
					}
					h[name] = f;
					if(options.mainClass == null && (options.guessFileKind || options.target != PEFileKinds.Dll))
					{
						foreach(ClassFile.Method m in f.Methods)
						{
							if(m.IsPublic && m.IsStatic && m.Name == "main" && m.Signature == "([Ljava.lang.String;)V")
							{
								Console.Error.WriteLine("Note: found main method in class: {0}", f.Name);
								options.mainClass = f.Name;
								break;
							}
						}
					}
				}
			}

			if(options.guessFileKind && options.mainClass == null)
			{
				options.target = PEFileKinds.Dll;
			}

			if(options.target == PEFileKinds.Dll && options.mainClass != null)
			{
				Console.Error.WriteLine("Error: main class cannot be specified for library or module");
				return 1;
			}

			if(options.target != PEFileKinds.Dll && options.mainClass == null)
			{
				Console.Error.WriteLine("Error: no main method found");
				return 1;
			}

			if(options.target == PEFileKinds.Dll && options.props.Count != 0)
			{
				Console.Error.WriteLine("Error: properties cannot be specified for library or module");
				return 1;
			}

			if(options.path == null)
			{
				if(options.target == PEFileKinds.Dll)
				{
					if(options.targetIsModule)
					{
						options.path = options.assembly + ".netmodule";
					}
					else
					{
						options.path = options.assembly + ".dll";
					}
				}
				else
				{
					options.path = options.assembly + ".exe";
				}
				Console.Error.WriteLine("Note: output file is: {0}", options.path);
			}

			if(options.targetIsModule)
			{
				// TODO if we're overwriting a user specified assembly name, we need to emit a warning
				options.assembly = new FileInfo(options.path).Name;
			}

			if(options.target == PEFileKinds.Dll && !options.path.ToLower().EndsWith(".dll") && !options.targetIsModule)
			{
				Console.Error.WriteLine("Error: library output file must end with .dll");
				return 1;
			}

			if(options.target != PEFileKinds.Dll && !options.path.ToLower().EndsWith(".exe"))
			{
				Console.Error.WriteLine("Error: executable output file must end with .exe");
				return 1;
			}

			// make sure all inner classes have a reference to their outer class
			// note that you cannot use the InnerClasses attribute in the inner class for this, because
			// anonymous inner classes do not have a reference to their outer class
			foreach(ClassFile classFile in h.Values)
			{
				// don't handle inner classes for ikvmstub types
				if(classFile.IKVMAssemblyAttribute == null)
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
										return 1;
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

			Tracer.Info(Tracer.Compiler, "Constructing compiler");
			CompilerClassLoader loader = new CompilerClassLoader(options.path, options.keyfilename, options.keycontainer, options.version, options.targetIsModule, options.assembly, h);
			ClassLoaderWrapper.SetBootstrapClassLoader(loader);
			compilationPhase1 = true;
			IKVM.Internal.MapXml.Root map = null;
			if(options.remapfile != null)
			{
				Tracer.Info(Tracer.Compiler, "Loading remapped types (1) from {0}", options.remapfile);
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(IKVM.Internal.MapXml.Root));
				ser.UnknownElement += new System.Xml.Serialization.XmlElementEventHandler(ser_UnknownElement);
				ser.UnknownAttribute += new System.Xml.Serialization.XmlAttributeEventHandler(ser_UnknownAttribute);
				using(FileStream fs = File.Open(options.remapfile, FileMode.Open))
				{
					XmlTextReader rdr = new XmlTextReader(fs);
					IKVM.Internal.MapXml.Root.xmlReader = rdr;
					IKVM.Internal.MapXml.Root.filename = new FileInfo(fs.Name).Name;
					map = (IKVM.Internal.MapXml.Root)ser.Deserialize(rdr);
				}
				loader.EmitRemappedTypes(map);
			}
			// Do a sanity check to make sure some of the bootstrap classes are available
			if(loader.LoadClassByDottedNameFast("java.lang.Object") == null)
			{
				Assembly classpath = Assembly.LoadWithPartialName("IKVM.GNU.Classpath");
				if(classpath == null)
				{
					Console.Error.WriteLine("Error: bootstrap classes missing and IKVM.GNU.Classpath.dll not found");
					return 1;
				}
				Console.Error.WriteLine("Warning: bootstrap classes are missing, automatically adding reference to {0}", classpath.Location);
				Console.Error.WriteLine("  (to avoid this warning add \"-reference:{0}\" to the command line)", classpath.Location);
				// we need to scan again for remapped types, now that we've loaded the core library
				ClassLoaderWrapper.LoadRemappedTypes();
			}

			ClassLoaderWrapper.PublishLibraryImplementationHelperType(typeof(gnu.classpath.RawData));
			ClassLoaderWrapper.PublishLibraryImplementationHelperType(typeof(ikvm.@internal.LibraryVMInterface));

			Tracer.Info(Tracer.Compiler, "Compiling class files (1)");
			ArrayList allwrappers = new ArrayList();
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
					else
					{
						allwrappers.Add(wrapper);
					}
				}
				catch(Exception x)
				{
					Console.Error.WriteLine("Loading class {0} failed due to:", s);
					Console.Error.WriteLine(x);
				}
			}
			if(options.mainClass != null)
			{
				TypeWrapper wrapper = loader.LoadClassByDottedNameFast(options.mainClass);
				if(wrapper == null)
				{
					Console.Error.WriteLine("Error: main class not found");
					return 1;
				}
				MethodWrapper mw = wrapper.GetMethodWrapper("main", "([Ljava.lang.String;)V", false);
				if(mw == null)
				{
					Console.Error.WriteLine("Error: main method not found");
					return 1;
				}
				mw.Link();
				MethodInfo method = mw.GetMethod() as MethodInfo;
				if(method == null)
				{
					Console.Error.WriteLine("Error: redirected main method not supported");
					return 1;
				}
				if(method.DeclaringType.Assembly != loader.ModuleBuilder.Assembly
					&& (!method.IsPublic || !method.DeclaringType.IsPublic))
				{
					Console.Error.WriteLine("Error: external main method must be public and in a public class");
					return 1;
				}
				Type apartmentAttributeType = null;
				if(options.apartment == ApartmentState.STA)
				{
					apartmentAttributeType = typeof(STAThreadAttribute);
				}
				else if(options.apartment == ApartmentState.MTA)
				{
					apartmentAttributeType = typeof(MTAThreadAttribute);
				}
				loader.SetMain(method, options.target, options.props, options.noglobbing, apartmentAttributeType);
			}
			compilationPhase1 = false;
			if(map != null)
			{
				DynamicTypeWrapper.LoadMappedExceptions(map);
				// mark all exceptions that are unsafe for mapping with a custom attribute,
				// so that at runtime we can quickly assertain if an exception type can be
				// caught without filtering
				foreach(TypeWrapper tw in allwrappers)
				{
					if(!tw.IsInterface && tw.IsMapUnsafeException)
					{
						tw.TypeAsBuilder.SetCustomAttribute(typeof(ExceptionIsUnsafeForMappingAttribute).GetConstructor(Type.EmptyTypes), new byte[0]);
					}
				}
				DynamicTypeWrapper.LoadNativeMethods(map);
				Tracer.Info(Tracer.Compiler, "Loading remapped types (2)");
				loader.FinishRemappedTypes();
			}
			Tracer.Info(Tracer.Compiler, "Compiling class files (2)");
			loader.AddResources(options.resources);
			loader.Save();
			return 0;
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
			try
			{
				Tracer.Error(Tracer.Runtime, "CRITICAL FAILURE: {0}", message);
				// NOTE we use reflection to invoke MessageBox.Show, to make sure we run on Mono as well
				Assembly winForms = IsUnix ? null : Assembly.LoadWithPartialName("System.Windows.Forms");
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
					x != null ? new StackTrace(x, true).ToString() : "",
					new StackTrace(true));
				if(messageBox != null)
				{
					try
					{
						messageBox.InvokeMember("Show", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new object[] { message, "IKVM.NET Critical Failure" });
					}
					catch
					{
						Console.Error.WriteLine(message);
					}
				}
				else
				{
					Console.Error.WriteLine(message);
				}
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine(ex);
			}
			finally
			{
				Environment.Exit(666);
			}
		}

		private static void ser_UnknownElement(object sender, System.Xml.Serialization.XmlElementEventArgs e)
		{
			Console.Error.WriteLine("Unknown element {0} in XML mapping file, line {1}, column {2}", e.Element.Name, e.LineNumber, e.LinePosition);
			Environment.Exit(1);
		}

		private static void ser_UnknownAttribute(object sender, System.Xml.Serialization.XmlAttributeEventArgs e)
		{
			Console.Error.WriteLine("Unknown attribute {0} in XML mapping file, line {1}, column {2}", e.Attr.Name, e.LineNumber, e.LinePosition);
			Environment.Exit(1);
		}
	}
}
