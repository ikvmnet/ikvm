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
using System.Resources;
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
	private static IJniProvider jniProvider;
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

	public static IJniProvider JniProvider
	{
		get
		{
			if(jniProvider == null)
			{
				Type provider;
				string providerAssembly = Environment.GetEnvironmentVariable("IKVM_JNI_PROVIDER");
				if(providerAssembly != null)
				{
					Tracer.Info(Tracer.Runtime, "Loading environment specified JNI provider: {0}", providerAssembly);
					provider = Assembly.LoadFrom(providerAssembly).GetType("JNI", true);
				}
				else
				{
					if(IsUnix)
					{
						Tracer.Info(Tracer.Runtime, "Loading JNI provider: Mono.IKVM.JNI");
						provider = Assembly.Load("Mono.IKVM.JNI").GetType("JNI", true);
					}
					else
					{
						Tracer.Info(Tracer.Runtime, "Loading JNI provider: ik.vm.jni");
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
			CustomAttributeBuilder ikvmModuleAttr = new CustomAttributeBuilder(typeof(JavaModuleAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			ModuleBuilder moduleBuilder;
			moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFile, JVM.Debug);
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
			Tracer.Info(Tracer.Compiler, "CompilerClassLoader.Save...");
			FinishAll();

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
					IResourceWriter writer = moduleBuilder.DefineResource("ikvm:" + d.Key, "");
					writer.AddResource("ikvm", buf);
				}
			}
		}

		private static MethodAttributes MapMethodAccessModifiers(MapXml.MapModifiers mod)
		{
			const MapXml.MapModifiers access = MapXml.MapModifiers.Public | MapXml.MapModifiers.Protected | MapXml.MapModifiers.Private;
			switch(mod & access)
			{
				case MapXml.MapModifiers.Public:
					return MethodAttributes.Public;
				case MapXml.MapModifiers.Protected:
					return MethodAttributes.FamORAssem;
				case MapXml.MapModifiers.Private:
					return MethodAttributes.Private;
				default:
					return MethodAttributes.Assembly;
			}
		}

		private static FieldAttributes MapFieldAccessModifiers(MapXml.MapModifiers mod)
		{
			const MapXml.MapModifiers access = MapXml.MapModifiers.Public | MapXml.MapModifiers.Protected | MapXml.MapModifiers.Private;
			switch(mod & access)
			{
				case MapXml.MapModifiers.Public:
					return FieldAttributes.Public;
				case MapXml.MapModifiers.Protected:
					return FieldAttributes.FamORAssem;
				case MapXml.MapModifiers.Private:
					return FieldAttributes.Private;
				default:
					return FieldAttributes.Assembly;
			}
		}

		private static void EmitRedirect(Type baseType, MethodDescriptor md, MapXml.Method m, ILGenerator ilgen)
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
			MethodDescriptor redir = MethodDescriptor.FromNameSig(ClassLoaderWrapper.GetBootstrapClassLoader(), redirName, redirSig);
			// HACK if the class name contains a comma, we assume it is a .NET type
			if(m.redirect.Class == null || m.redirect.Class.IndexOf(',') >= 0)
			{
				// TODO better error handling
				Type type = m.redirect.Class == null ? baseType : Type.GetType(m.redirect.Class, true);
				Type[] redirParamTypes = redir.ArgTypesForDefineMethod;
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
				mw.EmitCall.Emit(ilgen);
			}
			if(!redir.RetTypeWrapper.IsAssignableTo(md.RetTypeWrapper))
			{
				// NOTE we're passing a null context, this is safe because the return type
				// should always be loadable
				Debug.CompareTo(!md.RetTypeWrapper.IsUnloadable);
				md.RetTypeWrapper.EmitCheckcast(null, ilgen);
			}
		}

		private class RemapperTypeWrapper : TypeWrapper
		{
			private TypeBuilder typeBuilder;
			private Type equivalencyType;
			private MapXml.Class classDef;
			private TypeWrapper[] interfaceWrappers;

			internal override bool IsRemapped
			{
				get
				{
					return true;
				}
			}

			private static TypeWrapper GetBaseWrapper(MapXml.Class c)
			{
				if((c.Modifiers & MapXml.MapModifiers.Interface) != 0)
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

			internal RemapperTypeWrapper(CompilerClassLoader classLoader, MapXml.Class c)
				: base((Modifiers)c.Modifiers, c.Name, GetBaseWrapper(c), classLoader)
			{
				classDef = c;
				bool baseIsSealed = false;
				equivalencyType = Type.GetType(c.Type, true);
				classLoader.SetRemappedType(equivalencyType, this);
				Type baseType = equivalencyType;
				Type baseInterface = null;
				if(baseType.IsInterface)
				{
					baseInterface = baseType;
				}
				TypeAttributes attrs = TypeAttributes.Public;
				if((c.Modifiers & MapXml.MapModifiers.Interface) == 0)
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
				if((c.Modifiers & MapXml.MapModifiers.Abstract) != 0)
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

				if(!c.OneWay)
				{
					// FXBUG we would like to emit an attribute with a Type argument here, but that doesn't work because
					// of a bug in SetCustomAttribute that causes type arguments to be serialized incorrectly (if the type
					// is in the same assembly). Normally we use AttributeHelper.FreezeDry to get around this, but that doesn't
					// work in this case (to attribute is emitted at all). So we work around by emitting a string instead
					ConstructorInfo remappedClassAttribute = typeof(RemappedClassAttribute).GetConstructor(new Type[] { typeof(string), typeof(Type) });
					classLoader.assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute, new object[] { name, equivalencyType }));

					ConstructorInfo remappedTypeAttribute = typeof(RemappedTypeAttribute).GetConstructor(new Type[] { typeof(Type) });
					typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute, new object[] { equivalencyType }));
					AttributeHelper.HideFromReflection(typeBuilder);
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
			}

			class RemappedMethodWrapper : MethodWrapper
			{
				private MapXml.Method xmlMethod;
				private MethodBuilder mbCore;
				private MethodBuilder mbHelper;
				private MapXml.Constructor xmlConstructor;
				private ConstructorBuilder cbCore;
				private MethodBuilder cbHelper;
				private MethodInfo interfaceMethod;
				private ArrayList overriders = new ArrayList();

				private RemappedMethodWrapper(TypeWrapper typeWrapper, MethodDescriptor md, MethodBuilder mbCore, MethodBuilder mbHelper, Modifiers modifiers, MapXml.Method m)
					: base(typeWrapper, md, /*TODO*/mbCore, mbCore, modifiers, false)
				{
					this.xmlMethod = m;
					this.mbCore = mbCore;
					this.mbHelper = mbHelper;

					this.EmitNewobj = CodeEmitter.InternalError;
					if(mbCore != null)
					{
						this.EmitCall = CodeEmitter.Create(OpCodes.Call, mbCore);
					}
					else
					{
						this.EmitCall = CodeEmitter.InternalError;
					}
					if(mbHelper != null)
					{
						this.EmitCallvirt = CodeEmitter.Create(OpCodes.Call, mbHelper);
					}
					else
					{
						this.EmitCallvirt = CodeEmitter.InternalError;
					}
				}

				private RemappedMethodWrapper(RemapperTypeWrapper typeWrapper, MethodDescriptor md, ConstructorBuilder cbCore, MethodBuilder cbHelper, Modifiers modifiers, MapXml.Constructor m)
					: base(typeWrapper, md, /*TODO*/cbCore, cbCore, modifiers, false)
				{
					this.xmlConstructor = m;
					this.cbCore = cbCore;
					this.cbHelper = cbHelper;

					this.EmitCallvirt = CodeEmitter.InternalError;
					if(typeWrapper.equivalencyType.IsSealed)
					{
						this.EmitCall = CodeEmitter.InternalError;
						this.EmitNewobj = CodeEmitter.Create(OpCodes.Call, cbHelper);
					}
					else
					{
						this.EmitCall = CodeEmitter.Create(OpCodes.Call, cbCore);
						this.EmitNewobj = CodeEmitter.Create(OpCodes.Newobj, cbCore);
					}
				}

				private RemappedMethodWrapper(RemapperTypeWrapper typeWrapper, MethodDescriptor md, MethodInfo interfaceMethod, Modifiers modifiers)
					: base(typeWrapper, md, interfaceMethod, null, modifiers, false)
				{
					this.interfaceMethod = interfaceMethod;
					this.EmitCallvirt = CodeEmitter.Create(OpCodes.Callvirt, interfaceMethod);
					this.EmitCall = CodeEmitter.InternalError;
					this.EmitNewobj = CodeEmitter.InternalError;
				}

				internal static RemappedMethodWrapper Create(RemapperTypeWrapper typeWrapper, MapXml.Constructor m)
				{
					ConstructorBuilder cbCore = null;
					MethodBuilder cbHelper = null;
					MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers);
					MethodDescriptor md = MethodDescriptor.FromNameSig(typeWrapper.GetClassLoader(), "<init>", m.Sig);
					Type[] paramTypes = md.ArgTypesForDefineMethod;

					if(typeWrapper.equivalencyType.IsSealed)
					{
						cbHelper = typeWrapper.typeBuilder.DefineMethod("newhelper", attr | MethodAttributes.Static, CallingConventions.Standard, typeWrapper.equivalencyType, paramTypes);
						AddDeclaredExceptions(cbHelper, m.throws);
					}
					else
					{
						cbCore = typeWrapper.typeBuilder.DefineConstructor(attr, CallingConventions.Standard, paramTypes);
						AddDeclaredExceptions(cbCore, m.throws);
					}
					return new RemappedMethodWrapper(typeWrapper, md, cbCore, cbHelper, (Modifiers)m.Modifiers, m);
				}

				internal static RemappedMethodWrapper Create(RemapperTypeWrapper typeWrapper, MapXml.Method m)
				{
					MethodDescriptor md = MethodDescriptor.FromNameSig(typeWrapper.GetClassLoader(), m.Name, m.Sig);

					if(typeWrapper.IsInterface)
					{
						if(m.@override == null)
						{
							throw new InvalidOperationException(typeWrapper.Name + "." + m.Name + m.Sig);
						}
						MethodInfo interfaceMethod = typeWrapper.equivalencyType.GetMethod(m.@override.Name, md.ArgTypesForDefineMethod);
						if(interfaceMethod == null)
						{
							throw new InvalidOperationException(typeWrapper.Name + "." + m.Name + m.Sig);
						}
						if(m.throws != null)
						{
							// TODO we need a place to stick the declared exceptions
							throw new NotImplementedException();
						}
						CustomAttributeBuilder cab = new CustomAttributeBuilder(typeof(RemappedInterfaceMethodAttribute).GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { m.Name, m.@override.Name } );
						typeWrapper.typeBuilder.SetCustomAttribute(cab);
						return new RemappedMethodWrapper(typeWrapper, md, interfaceMethod, (Modifiers)m.Modifiers);
					}

					MethodBuilder mbCore = null;
					MethodBuilder mbHelper = null;
					Type[] paramTypes = md.ArgTypesForDefineMethod;
					Type retType = md.RetTypeForDefineMethod;

					if(typeWrapper.equivalencyType.IsSealed && (m.Modifiers & MapXml.MapModifiers.Static) == 0)
					{
						// skip instance methods in sealed types, but we do need to add them to the overriders
						if(typeWrapper.BaseTypeWrapper != null && (m.Modifiers & MapXml.MapModifiers.Private) == 0)
						{
							RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(md, true) as RemappedMethodWrapper;
							if(baseMethod != null &&
								!baseMethod.IsFinal &&
								!baseMethod.IsPrivate &&
								(baseMethod.xmlMethod.@override != null ||
								baseMethod.xmlMethod.redirect != null ||
								baseMethod.xmlMethod.body != null ||
								baseMethod.xmlMethod.alternateBody != null))
							{
								baseMethod.overriders.Add(typeWrapper);
							}
						}
					}
					else
					{
						MethodInfo overrideMethod = null;
						MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers);
						if((m.Modifiers & MapXml.MapModifiers.Static) != 0)
						{
							attr |= MethodAttributes.Static;
						}
						else if((m.Modifiers & MapXml.MapModifiers.Private) == 0 && (m.Modifiers & MapXml.MapModifiers.Final) == 0)
						{
							attr |= MethodAttributes.Virtual | MethodAttributes.NewSlot;
							if(typeWrapper.BaseTypeWrapper != null)
							{
								RemappedMethodWrapper baseMethod = typeWrapper.BaseTypeWrapper.GetMethodWrapper(md, true) as RemappedMethodWrapper;
								if(baseMethod != null)
								{
									baseMethod.overriders.Add(typeWrapper);
									if(baseMethod.xmlMethod.@override != null)
									{
										overrideMethod = typeWrapper.BaseTypeWrapper.TypeAsTBD.GetMethod(baseMethod.xmlMethod.@override.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
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

					if((m.Modifiers & MapXml.MapModifiers.Static) == 0)
					{
						// instance methods must have an instancehelper method
						MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.Static;
						Type[] exParamTypes = new Type[paramTypes.Length + 1];
						Array.Copy(paramTypes, 0, exParamTypes, 1, paramTypes.Length);
						exParamTypes[0] = typeWrapper.equivalencyType;
						mbHelper = typeWrapper.typeBuilder.DefineMethod("instancehelper_" + m.Name, attr, CallingConventions.Standard, retType, exParamTypes);
						AddDeclaredExceptions(mbHelper, m.throws);
					}

					return new RemappedMethodWrapper(typeWrapper, md, mbCore, mbHelper, (Modifiers)m.Modifiers, m);
				}

				private static void AddDeclaredExceptions(MethodBase mb, MapXml.Throws[] throws)
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

				internal void Finish()
				{
					// TODO we should insert method tracing (if enabled)
					if(xmlConstructor != null)
					{
						System.Diagnostics.Debug.Assert(xmlMethod == null);

						MapXml.Constructor m = xmlConstructor;
						Type[] paramTypes = this.Descriptor.ArgTypesForDefineMethod;

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

						if(cbHelper != null)
						{
							ILGenerator ilgen = cbHelper.GetILGenerator();
							if(m.redirect != null)
							{
								if(m.redirect.Type != "static" || m.redirect.Class == null || m.redirect.Name == null || m.redirect.Sig == null)
								{
									throw new NotImplementedException();
								}
								MethodDescriptor redir = MethodDescriptor.FromNameSig(ClassLoaderWrapper.GetBootstrapClassLoader(), m.redirect.Name, m.redirect.Sig);
								Type[] redirParamTypes = redir.ArgTypesForDefineMethod;
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
									mw.EmitCall.Emit(ilgen);
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
					else if(xmlMethod != null)
					{
						MapXml.Method m = xmlMethod;
						MethodDescriptor md = this.Descriptor;
						Type[] paramTypes = md.ArgTypesForDefineMethod;

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
								foreach(MapXml.Instruction instr in m.body.invoke)
								{
									if(instr is MapXml.Ret)
									{
										md.RetTypeWrapper.EmitConvStackToParameterType(ilgen, null);
									}
									instr.Generate(context, ilgen);
								}
							}
							else
							{
								int thisOffset = 0;
								if((m.Modifiers & MapXml.MapModifiers.Static) == 0)
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
									EmitRedirect(DeclaringType.TypeAsTBD, md, m, ilgen);
								}
								else
								{
									if(baseMethod == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									ilgen.Emit(OpCodes.Call, baseMethod);
								}
								md.RetTypeWrapper.EmitConvStackToParameterType(ilgen, null);
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
								(m.Modifiers & MapXml.MapModifiers.Private) == 0 && (m.Modifiers & MapXml.MapModifiers.Final) == 0)
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
								md.RetTypeWrapper.EmitConvStackToParameterType(ilgen, null);
								ilgen.Emit(OpCodes.Ret);
								ilgen.MarkLabel(skip);
								ilgen.Emit(OpCodes.Pop);
							}
							foreach(RemapperTypeWrapper overrider in overriders)
							{
								RemappedMethodWrapper mw = (RemappedMethodWrapper)overrider.GetMethodWrapper(md, false);
								if(mw.xmlMethod.redirect == null && mw.xmlMethod.body == null && mw.xmlMethod.alternateBody == null)
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
									mw.EmitCallvirt.Emit(ilgen);
									md.RetTypeWrapper.EmitConvStackToParameterType(ilgen, null);
									ilgen.Emit(OpCodes.Ret);
									ilgen.MarkLabel(skip);
									ilgen.Emit(OpCodes.Pop);
								}
							}
							if(m.body != null || m.alternateBody != null)
							{
								MapXml.InstructionList body = m.alternateBody == null ? m.body : m.alternateBody;
								// we manually walk the instruction list, because we need to special case the ret instructions
								Hashtable context = new Hashtable();
								foreach(MapXml.Instruction instr in body.invoke)
								{
									if(instr is MapXml.Ret)
									{
										md.RetTypeWrapper.EmitConvStackToParameterType(ilgen, null);
									}
									instr.Generate(context, ilgen);
								}
							}
							else
							{
								Type equivalencyType = ((RemapperTypeWrapper)DeclaringType).equivalencyType;
								for(int i = 0; i < paramTypes.Length + 1; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)i);
								}
								if(m.redirect != null)
								{
									EmitRedirect(equivalencyType, md, m, ilgen);
								}
								else if(m.@override != null)
								{
									MethodInfo baseMethod = equivalencyType.GetMethod(m.@override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
									if(baseMethod == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									ilgen.Emit(OpCodes.Callvirt, baseMethod);
								}
								else
								{
									RemappedMethodWrapper baseMethod = DeclaringType.BaseTypeWrapper.GetMethodWrapper(md, true) as RemappedMethodWrapper;
									if(baseMethod == null || baseMethod.xmlMethod.@override == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									MethodInfo overrideMethod = equivalencyType.GetMethod(baseMethod.xmlMethod.@override.Name, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
									if(overrideMethod == null)
									{
										throw new InvalidOperationException(DeclaringType.Name + "." + m.Name + m.Sig);
									}
									ilgen.Emit(OpCodes.Callvirt, overrideMethod);
								}
								md.RetTypeWrapper.EmitConvStackToParameterType(ilgen, null);
								ilgen.Emit(OpCodes.Ret);
							}
						}
					}
				}
			}

			internal void Process2ndPass()
			{
				MapXml.Class c = classDef;
				TypeBuilder tb = typeBuilder;
				bool baseIsSealed = equivalencyType.IsSealed;

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
						AttributeHelper.ImplementsAttribute(tb, ifaceTypeWrapper);
					}
				}
				else
				{
					interfaceWrappers = TypeWrapper.EmptyArray;
				}

				if(c.Constructors != null)
				{
					foreach(MapXml.Constructor m in c.Constructors)
					{
						AddMethod(RemappedMethodWrapper.Create(this, m));
					}
				}

				if(c.Methods != null)
				{
					// TODO we should also add methods from our super classes (e.g. Throwable should have Object's methods)
					foreach(MapXml.Method m in c.Methods)
					{
						AddMethod(RemappedMethodWrapper.Create(this, m));
					}
				}

				if(c.Fields != null)
				{
					foreach(MapXml.Field f in c.Fields)
					{
						if(f.redirect != null)
						{
							TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical(f.redirect.Class);
							MethodDescriptor redir = MethodDescriptor.FromNameSig(GetClassLoader(), f.redirect.Name, f.redirect.Sig);
							MethodWrapper method = tw.GetMethodWrapper(redir, false);
							if(method == null || !method.IsStatic)
							{
								// TODO better error handling
								throw new InvalidOperationException("remapping field: " + f.Name + f.Sig + " not found");
							}
							// TODO emit an static helper method that enables access to the field at runtime
							AddField(FieldWrapper.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), f.Name, f.Sig, (Modifiers)f.Modifiers, null, method.EmitCall, CodeEmitter.InternalError));
						}
						else if((f.Modifiers & MapXml.MapModifiers.Static) != 0)
						{
							FieldAttributes attr = MapFieldAccessModifiers(f.Modifiers) | FieldAttributes.Static;
							if(f.Constant != null)
							{
								attr |= FieldAttributes.Literal;
							}
							else if((f.Modifiers & MapXml.MapModifiers.Final) != 0)
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
								CodeEmitter getter = CodeEmitter.CreateLoadConstant(constant);
								AddField(FieldWrapper.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), f.Name, f.Sig, (Modifiers)f.Modifiers, fb, getter, CodeEmitter.InternalError, constant));
							}
							else
							{
								AddField(FieldWrapper.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), fb, f.Sig, (Modifiers)f.Modifiers));
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
				foreach(RemappedMethodWrapper m in GetMethods())
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
					foreach(MapXml.Interface iface in classDef.Interfaces)
					{
						GetClassLoader().LoadClassByDottedName(iface.Name).Finish();
					}
				}

				typeBuilder.CreateType();
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					// at the moment we don't support nested remapped types
					return null;
				}
			}

			internal override void Finish()
			{
			}

			protected override FieldWrapper GetFieldImpl(string fieldName, TypeWrapper fieldType)
			{
				// we don't resolve fields lazily
				return null;
			}

			protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
			{
				// we don't resolve methods lazily
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
					return equivalencyType;
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
					return equivalencyType == typeof(Exception) || equivalencyType.IsSubclassOf(typeof(Exception));
				}
			}
		}

		internal void EmitRemappedTypes(MapXml.Root map)
		{
			Tracer.Info(Tracer.Compiler, "Emit remapped types");

			// 1st pass, put all types in remapped to make them loadable
			foreach(MapXml.Class c in map.remappings)
			{
				remapped.Add(c.Name, new RemapperTypeWrapper(this, c));
			}

			DynamicTypeWrapper.SetupGhosts(map);

			// 2nd pass, resolve interfaces, publish methods/fields
			foreach(MapXml.Class c in map.remappings)
			{
				RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)remapped[c.Name];
				typeWrapper.Process2ndPass();
			}
		}

		internal void FinishRemappedTypes()
		{
			// 3rd pass, implement methods/fields and bake the type
			foreach(RemapperTypeWrapper typeWrapper in remapped.Values)
			{
				typeWrapper.Process3rdPass();
			}
		}
	}

	public static void Compile(string path, string keyfilename, string version, bool targetIsModule, string assembly, string mainClass, PEFileKinds target, bool guessFileKind, byte[][] classes, string[] references, bool nojni, Hashtable resources, string[] classesToExclude, string remapfile)
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
				f = new ClassFile(classes[i], 0, classes[i].Length, null);
			}
			catch(ClassFile.UnsupportedClassVersionError x)
			{
				Console.Error.WriteLine("Error: unsupported class file version: {0}", x.Message);
				return;
			}
			catch(ClassFile.ClassFormatError x)
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
				Console.Error.WriteLine("Error: duplicate class name: {0}", name);
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

		Tracer.Info(Tracer.Compiler, "Constructing compiler");
		CompilerClassLoader loader = new CompilerClassLoader(path, keyfilename, version, targetIsModule, assembly, h);
		ClassLoaderWrapper.SetBootstrapClassLoader(loader);
		compilationPhase1 = true;
		MapXml.Root map = null;
		if(remapfile != null)
		{
			Tracer.Info(Tracer.Compiler, "Loading remapped types (1) from {0}", remapfile);
			System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(MapXml.Root));
			using(FileStream fs = File.Open(remapfile, FileMode.Open))
			{
				map = (MapXml.Root)ser.Deserialize(fs);
			}
			loader.EmitRemappedTypes(map);
		}
		// Do a sanity check to make sure some of the bootstrap classes are available
		if(loader.LoadClassByDottedNameFast("java.lang.Object") == null)
		{
			Assembly classpath = Assembly.LoadWithPartialName("classpath");
			if(classpath == null)
			{
				Console.Error.WriteLine("Error: bootstrap classes missing and classpath.dll not found");
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
			messageBox.InvokeMember("Show", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null, new object[] { message, "IKVM.NET Critical Failure" });
		}
		else
		{
			Console.Error.WriteLine(message);
		}
		Environment.Exit(1);
	}
}
