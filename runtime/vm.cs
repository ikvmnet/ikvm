/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Collections.Specialized;
using System.Xml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Runtime
{
	public class Startup
	{
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

		public static void SetProperties(System.Collections.Specialized.StringDictionary props)
		{
			Type vmruntime = Type.GetType("java.lang.VMRuntime, IKVM.GNU.Classpath");
			System.Collections.Hashtable h = new System.Collections.Hashtable();
			foreach(DictionaryEntry de in props)
			{
				h.Add(de.Key, de.Value);
			}
			vmruntime.GetField("props", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, h);
		}
	}
}

namespace IKVM.Internal
{
	public class JVM
	{
		private static bool debug = false;
		private static bool noJniStubs = false;
		private static bool isStaticCompiler = false;
		private static bool compilationPhase1;
		private static string sourcePath;

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
			private string version;
			private bool targetIsModule;
			private AssemblyBuilder assemblyBuilder;

			internal CompilerClassLoader(string path, string keyfilename, string version, bool targetIsModule, string assemblyName, Hashtable classes)
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
				name.Version = new Version(version);
				assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave, assemblyDir);
				ModuleBuilder moduleBuilder;
				moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFile, JVM.Debug);
				CustomAttributeBuilder ikvmModuleAttr = new CustomAttributeBuilder(typeof(JavaModuleAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
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
						type = DefineClass(f);
					}
				}
				return type;
			}

			internal void SetMain(MethodInfo m, PEFileKinds target, StringDictionary props, bool noglobbing, Type apartmentAttributeType)
			{
				if(noglobbing && props.Count == 0)
				{
					if(apartmentAttributeType != null)
					{
						((MethodBuilder)m).SetCustomAttribute(new CustomAttributeBuilder(apartmentAttributeType.GetConstructor(Type.EmptyTypes), new object[0]));
					}
					assemblyBuilder.SetEntryPoint(m, target);
				}
				else
				{
					Type[] args = Type.EmptyTypes;
					if(noglobbing)
					{
						args = new Type[] { typeof(string[]) };
					}
					MethodBuilder mainStub = this.ModuleBuilder.DefineGlobalMethod("main", MethodAttributes.Public | MethodAttributes.Static, typeof(void), args);
					if(apartmentAttributeType != null)
					{
						mainStub.SetCustomAttribute(new CustomAttributeBuilder(apartmentAttributeType.GetConstructor(Type.EmptyTypes), new object[0]));
					}
					ILGenerator ilgen = mainStub.GetILGenerator();
					if(props.Count > 0)
					{
						ilgen.Emit(OpCodes.Newobj, typeof(StringDictionary).GetConstructor(Type.EmptyTypes));
						foreach(DictionaryEntry de in props)
						{
							ilgen.Emit(OpCodes.Dup);
							ilgen.Emit(OpCodes.Ldstr, (string)de.Key);
							ilgen.Emit(OpCodes.Ldstr, (string)de.Value);
							ilgen.Emit(OpCodes.Callvirt, typeof(StringDictionary).GetMethod("Add"));
						}
						ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Startup).GetMethod("SetProperties"));
					}
					if(noglobbing)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
					}
					else
					{
						ilgen.Emit(OpCodes.Call, typeof(IKVM.Runtime.Startup).GetMethod("Glob", Type.EmptyTypes));
					}
					ilgen.Emit(OpCodes.Call, m);
					ilgen.Emit(OpCodes.Ret);
					assemblyBuilder.SetEntryPoint(mainStub, target);
				}
			}

			internal void Save()
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader.Save...");
				FinishAll();

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
					// NOTE we cannot use CoreClasses.java_lang_Object here, because that would trigger a load
					// of java.lang.String and java.lang.Throwable before we've got the remapping set up.
					return ClassLoaderWrapper.LoadClassCritical("java.lang.Object");
				}

				internal RemapperTypeWrapper(CompilerClassLoader classLoader, IKVM.Internal.MapXml.Class c, IKVM.Internal.MapXml.Root map)
					: base((Modifiers)c.Modifiers, c.Name, GetBaseWrapper(c), classLoader)
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
						AttributeHelper.HideFromJava(typeBuilder);
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

					if(c.Constructors != null)
					{
						foreach(IKVM.Internal.MapXml.Constructor m in c.Constructors)
						{
							AddMethod(new RemappedConstructorWrapper(this, m));
						}
					}

					if(c.Methods != null)
					{
						// TODO we should also add methods from our super classes (e.g. Throwable should have Object's methods)
						foreach(IKVM.Internal.MapXml.Method m in c.Methods)
						{
							AddMethod(new RemappedMethodWrapper(this, m, map));
						}
					}
				}

				abstract class RemappedMethodBaseWrapper : MethodWrapper
				{
					internal RemappedMethodBaseWrapper(RemapperTypeWrapper typeWrapper, MethodDescriptor md, Modifiers modifiers)
						: base(typeWrapper, md, null, null, null, modifiers, MemberFlags.None)
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
						: base(typeWrapper, new MethodDescriptor("<init>", m.Sig), (Modifiers)m.Modifiers)
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
						MethodDescriptor md = new MethodDescriptor("<init>", m.Sig);
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
						Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);

						ConstructorBuilder cbCore = null;

						if(typeWrapper.shadowType.IsSealed)
						{
							mbHelper = typeWrapper.typeBuilder.DefineMethod("newhelper", attr | MethodAttributes.Static, CallingConventions.Standard, typeWrapper.shadowType, paramTypes);
							AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers);
							AttributeHelper.SetNameSig(mbHelper, "<init>", m.Sig);
							AddDeclaredExceptions(mbHelper, m.throws);
						}
						else
						{
							cbCore = typeWrapper.typeBuilder.DefineConstructor(attr, CallingConventions.Standard, paramTypes);
							AddDeclaredExceptions(cbCore, m.throws);
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
								MethodDescriptor redir = new MethodDescriptor(m.redirect.Name, m.redirect.Sig);
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
									MethodWrapper mw = tw.GetMethodWrapper(redir, false);
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
						: base(typeWrapper, new MethodDescriptor(m.Name, m.Sig), (Modifiers)m.Modifiers)
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
						MethodDescriptor md = new MethodDescriptor(m.Name, m.Sig);

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
									MethodWrapper mw = tw.GetMethodWrapper(md, false);
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
									RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(md, true) as RemappedMethodWrapper;
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
										RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(md, true) as RemappedMethodWrapper;
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
								AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers);
								AttributeHelper.SetNameSig(mbHelper, m.Name, m.Sig);
								AddDeclaredExceptions(mbHelper, m.throws);
							}
							return mbCore;
						}
					}

					internal override void Finish()
					{
						// TODO we should insert method tracing (if enabled)
						MethodDescriptor md = this.Descriptor;
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
								RemappedMethodWrapper mw = (RemappedMethodWrapper)overrider.GetMethodWrapper(md, false);
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
									RemappedMethodWrapper baseMethod = DeclaringType.BaseTypeWrapper.GetMethodWrapper(md, true) as RemappedMethodWrapper;
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
						}

						// do we need a helper for non-virtual reflection invocation?
						if(m.nonvirtualAlternateBody != null || (m.@override != null && overriders.Count > 0))
						{
							RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
							Type[] argTypes = new Type[paramTypes.Length + 1];
							argTypes[0] = typeWrapper.TypeAsParameterType;
							this.GetParametersForDefineMethod().CopyTo(argTypes, 1);
							MethodBuilder mb = typeWrapper.typeBuilder.DefineMethod("nonvirtualhelper/" + this.Name, MethodAttributes.Private | MethodAttributes.Static, this.ReturnTypeForDefineMethod, argTypes);
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
						MethodDescriptor redir = new MethodDescriptor(redirName, redirSig);
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
							MethodWrapper mw = tw.GetMethodWrapper(redir, false);
							if(mw == null)
							{
								throw new InvalidOperationException("Missing redirect method: " + tw.Name + "." + redir.Name + redir.Signature);
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

					// TODO fields should be moved to the RemapperTypeWrapper constructor as well
					if(c.Fields != null)
					{
						foreach(IKVM.Internal.MapXml.Field f in c.Fields)
						{
							if(f.redirect != null)
							{
								TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(f.redirect.Class);
								MethodDescriptor redir = new MethodDescriptor(f.redirect.Name, f.redirect.Sig);
								MethodWrapper method = tw.GetMethodWrapper(redir, false);
								if(method == null || !method.IsStatic)
								{
									// TODO better error handling
									throw new InvalidOperationException("remapping field: " + f.Name + f.Sig + " not found");
								}
								// TODO emit an static helper method that enables access to the field at runtime
								method.Link();
								AddField(FieldWrapper.Create1(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), f.Name, f.Sig, (Modifiers)f.Modifiers, null, CodeEmitter.WrapCall(method), CodeEmitter.InternalError));
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
									AddField(new FieldWrapper.ConstantFieldWrapper(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), f.Name, f.Sig, (Modifiers)f.Modifiers, fb, constant));
								}
								else
								{
									AddField(FieldWrapper.Create3(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), fb, f.Sig, (Modifiers)f.Modifiers));
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
				}

				internal void Process3rdPass()
				{
					foreach(RemappedMethodBaseWrapper m in GetMethods())
					{
						m.Link();
					}
				}

				internal void Process4thPass()
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

					typeBuilder.CreateType();
					if(helperTypeBuilder != null)
					{
						helperTypeBuilder.CreateType();
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

				protected override FieldWrapper GetFieldImpl(string fieldName, TypeWrapper fieldType)
				{
					// we don't resolve fields lazily
					return null;
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
					typeWrapper.Process4thPass();
				}
			}
		}

		public static void Compile(string path, string keyfilename, string version, bool targetIsModule, string assembly, string mainClass, ApartmentState apartment, PEFileKinds target, bool guessFileKind, byte[][] classes, string[] references, bool nojni, Hashtable resources, string[] classesToExclude, string remapfile, StringDictionary props, bool noglobbing)
		{
			Tracer.Info(Tracer.Compiler, "JVM.Compile path: {0}, assembly: {1}", path, assembly);
			isStaticCompiler = true;
			noJniStubs = nojni;
			foreach(string r in references)
			{
				try
				{
					Assembly reference = Assembly.LoadFrom(r);
					if(reference == null)
					{
						Console.Error.WriteLine("Error: reference not found: {0}", r);
						return;
					}
					Tracer.Info(Tracer.Compiler, "Loaded reference assembly: {0}", reference.FullName);
				}
				catch(Exception x)
				{
					Console.Error.WriteLine("Error: invalid reference: {0} ({1})", r, x.Message);
					return;
				}
			}
			Hashtable h = new Hashtable();
			Tracer.Info(Tracer.Compiler, "Parsing class files");
			for(int i = 0; i < classes.Length; i++)
			{
				ClassFile f;
				try
				{
					f = new ClassFile(classes[i], 0, classes[i].Length, null, true);
				}
				catch(UnsupportedClassVersionError x)
				{
					Console.Error.WriteLine("Error: unsupported class file version: {0}", x.Message);
					return;
				}
				catch(ClassFormatError x)
				{
					Console.Error.WriteLine("Error: invalid class file: {0}", x.Message);
					return;
				}
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
					Console.Error.WriteLine("Warning: duplicate class name: {0}", name);
					excluded = true;
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

			if(target == PEFileKinds.Dll && mainClass != null)
			{
				Console.Error.WriteLine("Error: main class cannot be specified for library or module");
				return;
			}

			if(target != PEFileKinds.Dll && mainClass == null)
			{
				Console.Error.WriteLine("Error: no main method found");
				return;
			}

			if(target == PEFileKinds.Dll && props.Count != 0)
			{
				Console.Error.WriteLine("Error: properties cannot be specified for library or module");
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
				assembly = new FileInfo(path).Name;
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

			Tracer.Info(Tracer.Compiler, "Constructing compiler");
			CompilerClassLoader loader = new CompilerClassLoader(path, keyfilename, version, targetIsModule, assembly, h);
			ClassLoaderWrapper.SetBootstrapClassLoader(loader);
			compilationPhase1 = true;
			IKVM.Internal.MapXml.Root map = null;
			if(remapfile != null)
			{
				Tracer.Info(Tracer.Compiler, "Loading remapped types (1) from {0}", remapfile);
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(IKVM.Internal.MapXml.Root));
				ser.UnknownElement += new System.Xml.Serialization.XmlElementEventHandler(ser_UnknownElement);
				using(FileStream fs = File.Open(remapfile, FileMode.Open))
				{
					map = (IKVM.Internal.MapXml.Root)ser.Deserialize(fs);
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
					return;
				}
				Console.Error.WriteLine("Warning: bootstrap classes are missing, automatically adding reference to {0}", classpath.Location);
				Console.Error.WriteLine("  (to avoid this warning add \"-reference:{0}\" to the command line)", classpath.Location);
				// we need to scan again for remapped types, now that we've loaded the core library
				ClassLoaderWrapper.LoadRemappedTypes();
			}
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
				if(s == mainClass && wrapper != null)
				{
					MethodWrapper mw = wrapper.GetMethodWrapper(new MethodDescriptor("main", "([Ljava.lang.String;)V"), false);
					if(mw == null)
					{
						Console.Error.WriteLine("Error: main method not found");
						return;
					}
					mw.Link();
					MethodBuilder method = mw.GetMethod() as MethodBuilder;
					if(method == null)
					{
						Console.Error.WriteLine("Error: redirected main method not supported");
						return;
					}
					Type apartmentAttributeType = null;
					if(apartment == ApartmentState.STA)
					{
						apartmentAttributeType = typeof(STAThreadAttribute);
					}
					else if(apartment == ApartmentState.MTA)
					{
						apartmentAttributeType = typeof(MTAThreadAttribute);
					}
					loader.SetMain(method, target, props, noglobbing, apartmentAttributeType);
					mainClass = null;
				}
			}
			if(mainClass != null)
			{
				Console.Error.WriteLine("Error: main class not found");
				return;
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
	}
}
