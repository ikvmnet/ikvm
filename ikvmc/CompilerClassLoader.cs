/*
  Copyright (C) 2002-2008 Jeroen Frijters

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
using System.Reflection.Emit;
using System.Resources;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using IKVM.Attributes;

using System.Security.Permissions;
using System.Security;

namespace IKVM.Internal
{
	class CompilerClassLoader : ClassLoaderWrapper
	{
		private Dictionary<string, byte[]> classes;
		private Dictionary<string, TypeWrapper> remapped = new Dictionary<string, TypeWrapper>();
		private string assemblyName;
		private string assemblyFile;
		private string assemblyDir;
		private string keyfilename;
		private string keycontainer;
		private string version;
		private bool targetIsModule;
		private AssemblyBuilder assemblyBuilder;
		private IKVM.Internal.MapXml.Attribute[] assemblyAttributes;
		private CompilerOptions options;
		private AssemblyClassLoader[] referencedAssemblies;
		private Dictionary<string, string> nameMappings = new Dictionary<string, string>();
		private Dictionary<string, string> packages = new Dictionary<string, string>();
		private Dictionary<string, List<TypeWrapper>> ghosts;
		private TypeWrapper[] mappedExceptions;
		private bool[] mappedExceptionsAllSubClasses;
		private Dictionary<string, IKVM.Internal.MapXml.Class> mapxml_Classes;
		private Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList> mapxml_MethodBodies;
		private Dictionary<MethodKey, IKVM.Internal.MapXml.ReplaceMethodCall[]> mapxml_ReplacedMethods;
		private Dictionary<string, string> baseClasses;
		private IKVM.Internal.MapXml.Root map;
		private bool compilingCoreAssembly;
		private List<object> assemblyAnnotations;
		private List<string> classesToCompile;
		private List<CompilerClassLoader> peerReferences = new List<CompilerClassLoader>();
		[ThreadStatic]
		private bool peerHack;

		internal CompilerClassLoader(AssemblyClassLoader[] referencedAssemblies, CompilerOptions options, string path, string keyfilename, string keycontainer, string version, bool targetIsModule, string assemblyName, Dictionary<string, byte[]> classes)
			: base(options.codegenoptions, null)
		{
			this.referencedAssemblies = referencedAssemblies;
			this.options = options;
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

		internal void AddNameMapping(string javaName, string typeName)
		{
			nameMappings.Add(javaName, typeName);
		}

		internal void AddReference(AssemblyClassLoader acl)
		{
			AssemblyClassLoader[] temp = new AssemblyClassLoader[referencedAssemblies.Length + 1];
			Array.Copy(referencedAssemblies, 0, temp, 0, referencedAssemblies.Length);
			temp[temp.Length - 1] = acl;
			referencedAssemblies = temp;
		}

		internal void AddReference(CompilerClassLoader ccl)
		{
			peerReferences.Add(ccl);
		}

		internal override string SourcePath
		{
			get
			{
				return options.sourcepath;
			}
		}

		internal AssemblyName GetAssemblyName()
		{
			return assemblyBuilder.GetName();
		}

		internal ModuleBuilder CreateModuleBuilder()
		{
			AssemblyName name = new AssemblyName();
			name.Name = assemblyName;
			if(keyfilename != null) 
			{
				name.KeyPair = new StrongNameKeyPair(File.ReadAllBytes(keyfilename));
			}
			if(keycontainer != null)
			{
				name.KeyPair = new StrongNameKeyPair(keycontainer);
			}
			name.Version = new Version(version);
			assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.ReflectionOnly, assemblyDir);
			ModuleBuilder moduleBuilder;
			moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFile, this.EmitDebugInfo);
			if(this.EmitStackTraceInfo)
			{
				AttributeHelper.SetSourceFile(moduleBuilder, null);
			}
			if(this.EmitDebugInfo || this.EmitStackTraceInfo)
			{
				CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, this.EmitDebugInfo });
				assemblyBuilder.SetCustomAttribute(debugAttr);
			}
			return moduleBuilder;
		}

		protected override TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			foreach(AssemblyClassLoader acl in referencedAssemblies)
			{
				TypeWrapper tw = acl.DoLoad(name);
				if(tw != null)
				{
					return tw;
				}
			}
			if(!peerHack)
			{
				peerHack = true;
				try
				{
					foreach(CompilerClassLoader ccl in peerReferences)
					{
						TypeWrapper tw = ccl.LoadClassByDottedNameFast(name);
						if(tw != null && tw.GetClassLoader() == ccl)
						{
							return tw;
						}
					}
				}
				finally
				{
					peerHack = false;
				}
			}
			return GetTypeWrapperCompilerHook(name);
		}

		private TypeWrapper GetTypeWrapperCompilerHook(string name)
		{
			TypeWrapper type = null;
			if(type == null)
			{
				if(remapped.TryGetValue(name, out type))
				{
					return type;
				}
				byte[] classdef;
				if(classes.TryGetValue(name, out classdef))
				{
					classes.Remove(name);
					ClassFile f;
					try
					{
						ClassFileParseOptions cfp = ClassFileParseOptions.LocalVariableTable;
						if(this.EmitStackTraceInfo)
						{
							cfp |= ClassFileParseOptions.LineNumberTable;
						}
						f = new ClassFile(classdef, 0, classdef.Length, name, cfp);
					}
					catch(ClassFormatError x)
					{
						StaticCompiler.IssueMessage(Message.ClassFormatError, name, x.Message);
						return null;
					}
					if(options.removeUnusedFields)
					{
						f.RemoveUnusedFields();
					}
					if(f.IsPublic && options.privatePackages != null)
					{
						foreach(string p in options.privatePackages)
						{
							if(f.Name.StartsWith(p))
							{
								f.SetInternal();
								break;
							}
						}
					}
					if(!f.IsInterface
						&& !f.IsAbstract
						&& !f.IsPublic
						&& !f.IsFinal
						&& !baseClasses.ContainsKey(f.Name)
						&& !options.targetIsModule)
					{
						f.SetEffectivelyFinal();
					}
					try
					{
						type = DefineClass(f, null);
					}
					catch (ClassFormatError x)
					{
						StaticCompiler.IssueMessage(Message.ClassFormatError, name, x.Message);
						return null;
					}
					catch (IllegalAccessError x)
					{
						StaticCompiler.IssueMessage(Message.IllegalAccessError, name, x.Message);
						return null;
					}
					catch (VerifyError x)
					{
						StaticCompiler.IssueMessage(Message.VerificationError, name, x.Message);
						return null;
					}
					catch (NoClassDefFoundError x)
					{
						StaticCompiler.IssueMessage(Message.NoClassDefFoundError, name, x.Message);
						return null;
					}
					catch (RetargetableJavaException x)
					{
						StaticCompiler.IssueMessage(Message.GenericUnableToCompileError, name, x.GetType().Name, x.Message);
						return null;
					}
				}
			}
			return type;
		}

		internal override Type GetType(string name)
		{
			foreach(AssemblyClassLoader loader in referencedAssemblies)
			{
				Type type = loader.GetType(name);
				if(type != null)
				{
					return type;
				}
			}
			return null;
		}

		internal void SetMain(MethodInfo m, PEFileKinds target, Dictionary<string, string> props, bool noglobbing, Type apartmentAttributeType)
		{
			Type[] args = Type.EmptyTypes;
			if(noglobbing)
			{
				args = new Type[] { typeof(string[]) };
			}
			MethodBuilder mainStub = this.GetTypeWrapperFactory().ModuleBuilder.DefineGlobalMethod("main", MethodAttributes.Public | MethodAttributes.Static, typeof(int), args);
			if(apartmentAttributeType != null)
			{
				mainStub.SetCustomAttribute(new CustomAttributeBuilder(apartmentAttributeType.GetConstructor(Type.EmptyTypes), new object[0]));
			}
			CodeEmitter ilgen = CodeEmitter.Create(mainStub);
			LocalBuilder rc = ilgen.DeclareLocal(typeof(int));
			TypeWrapper startupType = LoadClassByDottedName("ikvm.runtime.Startup");
			if(props.Count > 0)
			{
				ilgen.Emit(OpCodes.Newobj, typeof(System.Collections.Hashtable).GetConstructor(Type.EmptyTypes));
				foreach(KeyValuePair<string, string> kv in props)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Ldstr, kv.Key);
					ilgen.Emit(OpCodes.Ldstr, kv.Value);
					ilgen.Emit(OpCodes.Callvirt, typeof(System.Collections.Hashtable).GetMethod("Add"));
				}
				startupType.GetMethodWrapper("setProperties", "(Lcli.System.Collections.Hashtable;)V", false).EmitCall(ilgen);
			}
			ilgen.BeginExceptionBlock();
			startupType.GetMethodWrapper("enterMainThread", "()V", false).EmitCall(ilgen);
			if(noglobbing)
			{
				ilgen.Emit(OpCodes.Ldarg_0);
			}
			else
			{
				startupType.GetMethodWrapper("glob", "()[Ljava.lang.String;", false).EmitCall(ilgen);
			}
			ilgen.Emit(OpCodes.Call, m);
			ilgen.BeginCatchBlock(typeof(Exception));
			LoadClassByDottedName("ikvm.runtime.Util").GetMethodWrapper("mapException", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false).EmitCall(ilgen);
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
			ClassLoaderWrapper.LoadClassCritical("java.lang.ThreadGroup").GetMethodWrapper("uncaughtException", "(Ljava.lang.Thread;Ljava.lang.Throwable;)V", false).EmitCallvirt(ilgen);
			ilgen.Emit(OpCodes.Ldc_I4_1);
			ilgen.Emit(OpCodes.Stloc, rc);
			ilgen.BeginFinallyBlock();
			startupType.GetMethodWrapper("exitMainThread", "()V", false).EmitCall(ilgen);
			ilgen.EndExceptionBlock();
			ilgen.Emit(OpCodes.Ldloc, rc);
			ilgen.Emit(OpCodes.Ret);
			assemblyBuilder.SetEntryPoint(mainStub, target);
		}

		internal void Save()
		{
			Tracer.Info(Tracer.Compiler, "CompilerClassLoader.Save...");
			((DynamicClassLoader)this.GetTypeWrapperFactory()).FinishAll();

			ModuleBuilder mb = GetTypeWrapperFactory().ModuleBuilder;
			// HACK force all referenced assemblies to end up as references in the assembly
			// (even if they are otherwise unused), to make sure that the assembly class loader
			// delegates to them at runtime.
			for(int i = 0;i < referencedAssemblies.Length; i++)
			{
				Type[] types = referencedAssemblies[i].Assembly.GetExportedTypes();
				if(types.Length > 0)
				{
					mb.GetTypeToken(types[0]);
				}
			}
			mb.CreateGlobalFunctions();

			// add a class.map resource, if needed.
			if(nameMappings.Count > 0)
			{
				string[] list = new string[nameMappings.Count * 2];
				int i = 0;
				foreach(KeyValuePair<string, string> kv in nameMappings)
				{
					list[i++] = kv.Key;
					list[i++] = kv.Value;
				}
				CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(JavaModuleAttribute)).GetConstructor(new Type[] { typeof(string[]) }), new object[] { list });
				mb.SetCustomAttribute(cab);
			}
			else
			{
				CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(JavaModuleAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
				mb.SetCustomAttribute(cab);
			}

			// add a package list
			if(true)
			{
				string[] list = new string[packages.Count];
				packages.Keys.CopyTo(list, 0);
				mb.SetCustomAttribute(new CustomAttributeBuilder(JVM.LoadType(typeof(PackageListAttribute)).GetConstructor(new Type[] { typeof(string[]) }), new object[] { list }));
			}

			if(targetIsModule)
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving temp.$$$ in {0}", assemblyDir);
				string manifestAssembly = "temp.$$$";
				assemblyBuilder.Save(manifestAssembly, options.pekind, options.imageFileMachine);
				File.Delete(assemblyDir + manifestAssembly);
			}
			else
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving {0} in {1}", assemblyFile, assemblyDir);
				assemblyBuilder.Save(assemblyFile, options.pekind, options.imageFileMachine);
			}
		}

		internal void AddResources(Dictionary<string, byte[]> resources, bool compressedResources)
		{
			Tracer.Info(Tracer.Compiler, "CompilerClassLoader adding resources...");
			ModuleBuilder moduleBuilder = this.GetTypeWrapperFactory().ModuleBuilder;
			foreach(KeyValuePair<string, byte[]> kv in resources)
			{
				byte[] buf = kv.Value;
				string name = JVM.MangleResourceName(kv.Key);
				MemoryStream mem = new MemoryStream();
				if(compressedResources)
				{
					mem.WriteByte(1);
					using(System.IO.Compression.DeflateStream def = new System.IO.Compression.DeflateStream(mem, System.IO.Compression.CompressionMode.Compress, true))
					{
						def.Write(buf, 0, buf.Length);
					}
				}
				else
				{
					mem.WriteByte(0);
					mem.Write(buf, 0, buf.Length);
				}
				mem.Position = 0;
				moduleBuilder.DefineManifestResource(name, mem, ResourceAttributes.Public);
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
			private CompilerClassLoader classLoader;
			private TypeBuilder typeBuilder;
			private TypeBuilder helperTypeBuilder;
			private Type shadowType;
			private IKVM.Internal.MapXml.Class classDef;
			private TypeWrapper[] interfaceWrappers;

			internal override ClassLoaderWrapper GetClassLoader()
			{
				return classLoader;
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
				: base((Modifiers)c.Modifiers, c.Name, GetBaseWrapper(c))
			{
				this.classLoader = classLoader;
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
						attrs |= TypeAttributes.Abstract | TypeAttributes.Sealed;
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
				typeBuilder = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(name, attrs, baseIsSealed ? typeof(object) : baseType);
				if(c.Attributes != null)
				{
					foreach(IKVM.Internal.MapXml.Attribute custattr in c.Attributes)
					{
						AttributeHelper.SetCustomAttribute(classLoader, typeBuilder, custattr);
					}
				}
				if(baseInterface != null)
				{
					typeBuilder.AddInterfaceImplementation(baseInterface);
				}
				if(classLoader.EmitStackTraceInfo)
				{
					AttributeHelper.SetSourceFile(typeBuilder, IKVM.Internal.MapXml.Root.filename);
				}

				if(baseIsSealed)
				{
					AttributeHelper.SetModifiers(typeBuilder, (Modifiers)c.Modifiers, false);
				}

				if(c.scope == IKVM.Internal.MapXml.Scope.Public)
				{
					// FXBUG we would like to emit an attribute with a Type argument here, but that doesn't work because
					// of a bug in SetCustomAttribute that causes type arguments to be serialized incorrectly (if the type
					// is in the same assembly). Normally we use AttributeHelper.FreezeDry to get around this, but that doesn't
					// work in this case (no attribute is emitted at all). So we work around by emitting a string instead
					AttributeHelper.SetRemappedClass(classLoader.assemblyBuilder, name, shadowType);
						
					AttributeHelper.SetRemappedType(typeBuilder, shadowType);
				}

				List<MethodWrapper> methods = new List<MethodWrapper>();

				if(c.Constructors != null)
				{
					foreach(IKVM.Internal.MapXml.Constructor m in c.Constructors)
					{
						methods.Add(new RemappedConstructorWrapper(this, m));
					}
				}

				if(c.Methods != null)
				{
					foreach(IKVM.Internal.MapXml.Method m in c.Methods)
					{
						methods.Add(new RemappedMethodWrapper(this, m, map, false));
					}
				}
				// add methods from our super classes (e.g. Throwable should have Object's methods)
				if(!this.IsFinal && !this.IsInterface && this.BaseTypeWrapper != null)
				{
					foreach(MethodWrapper mw in BaseTypeWrapper.GetMethods())
					{
						RemappedMethodWrapper rmw = mw as RemappedMethodWrapper;
						if(rmw != null && (rmw.IsPublic || rmw.IsProtected))
						{
							if(!FindMethod(methods, rmw.Name, rmw.Signature))
							{
								methods.Add(new RemappedMethodWrapper(this, rmw.XmlMethod, map, true));
							}
						}
					}
				}

				SetMethods(methods.ToArray());
			}

			private static bool FindMethod(List<MethodWrapper> methods, string name, string sig)
			{
				foreach(MethodWrapper mw in methods)
				{
					if(mw.Name == name && mw.Signature == sig)
					{
						return true;
					}
				}
				return false;
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

				internal override void EmitCall(CodeEmitter ilgen)
				{
					ilgen.Emit(OpCodes.Call, (ConstructorInfo)GetMethod());
				}

				internal override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
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
						if(m.Attributes != null)
						{
							foreach(IKVM.Internal.MapXml.Attribute custattr in m.Attributes)
							{
								AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mbHelper, custattr);
							}
						}
						SetParameters(DeclaringType.GetClassLoader(), mbHelper, m.Params);
						AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers, false);
						AttributeHelper.SetNameSig(mbHelper, "<init>", m.Sig);
						AddDeclaredExceptions(mbHelper, m.throws);
					}
					else
					{
						cbCore = typeWrapper.typeBuilder.DefineConstructor(attr, CallingConventions.Standard, paramTypes);
						if(m.Attributes != null)
						{
							foreach(IKVM.Internal.MapXml.Attribute custattr in m.Attributes)
							{
								AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), cbCore, custattr);
							}
						}
						SetParameters(DeclaringType.GetClassLoader(), cbCore, m.Params);
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
						CodeEmitter ilgen = CodeEmitter.Create(cbCore);
						// TODO we need to support ghost (and other funky?) parameter types
						if(m.body != null)
						{
							// TODO do we need return type conversion here?
							m.body.Emit(DeclaringType.GetClassLoader(), ilgen);
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
						if(this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
						{
							ilgen.EmitLineNumberTable(cbCore);
						}
					}

					if(mbHelper != null)
					{
						CodeEmitter ilgen = CodeEmitter.Create(mbHelper);
						if(m.redirect != null)
						{
							m.redirect.Emit(DeclaringType.GetClassLoader(), ilgen);
						}
						else if(m.alternateBody != null)
						{
							m.alternateBody.Emit(DeclaringType.GetClassLoader(), ilgen);
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
						if(this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
						{
							ilgen.EmitLineNumberTable(mbHelper);
						}
					}
				}
			}

			sealed class RemappedMethodWrapper : RemappedMethodBaseWrapper
			{
				private IKVM.Internal.MapXml.Method m;
				private IKVM.Internal.MapXml.Root map;
				private MethodBuilder mbHelper;
				private List<RemapperTypeWrapper> overriders = new List<RemapperTypeWrapper>();
				private bool inherited;

				internal RemappedMethodWrapper(RemapperTypeWrapper typeWrapper, IKVM.Internal.MapXml.Method m, IKVM.Internal.MapXml.Root map, bool inherited)
					: base(typeWrapper, m.Name, m.Sig, (Modifiers)m.Modifiers)
				{
					this.m = m;
					this.map = map;
					this.inherited = inherited;
				}

				internal IKVM.Internal.MapXml.Method XmlMethod
				{
					get
					{
						return m;
					}
				}

				internal override void EmitCall(CodeEmitter ilgen)
				{
					ilgen.Emit(OpCodes.Call, (MethodInfo)GetMethod());
				}

				internal override void EmitCallvirt(CodeEmitter ilgen)
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
						List<IKVM.Internal.MapXml.Class> specialCases = null;
						foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
						{
							if(c.Methods != null)
							{
								foreach(IKVM.Internal.MapXml.Method mm in c.Methods)
								{
									if(mm.Name == m.Name && mm.Sig == m.Sig && mm.body != null)
									{
										if(specialCases == null)
										{
											specialCases = new List<IKVM.Internal.MapXml.Class>();
										}
										specialCases.Add(c);
										break;
									}
								}
							}
						}
						AttributeHelper.SetRemappedInterfaceMethod(typeWrapper.typeBuilder, m.Name, m.@override.Name);
						MethodBuilder helper = null;
						if(specialCases != null)
						{
							CodeEmitter ilgen;
							Type[] temp = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);
							Type[] argTypes = new Type[temp.Length + 1];
							temp.CopyTo(argTypes, 1);
							argTypes[0] = typeWrapper.shadowType;
							if(typeWrapper.helperTypeBuilder == null)
							{
								// FXBUG we use a nested helper class, because Reflection.Emit won't allow us to add a static method to the interface
								// TODO now that we're on Whidbey we can remove this workaround
								typeWrapper.helperTypeBuilder = typeWrapper.typeBuilder.DefineNestedType("__Helper", TypeAttributes.NestedPublic | TypeAttributes.Class | TypeAttributes.Sealed);
								ilgen = CodeEmitter.Create(typeWrapper.helperTypeBuilder.DefineConstructor(MethodAttributes.Private, CallingConventions.Standard, Type.EmptyTypes));
								ilgen.Emit(OpCodes.Ldnull);
								ilgen.Emit(OpCodes.Throw);
								AttributeHelper.HideFromJava(typeWrapper.helperTypeBuilder);
							}
							helper = typeWrapper.helperTypeBuilder.DefineMethod(m.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig).TypeAsSignatureType, argTypes);
							if(m.Attributes != null)
							{
								foreach(IKVM.Internal.MapXml.Attribute custattr in m.Attributes)
								{
									AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), helper, custattr);
								}
							}
							SetParameters(DeclaringType.GetClassLoader(), helper, m.Params);
							ilgen = CodeEmitter.Create(helper);
							foreach(IKVM.Internal.MapXml.Class c in specialCases)
							{
								TypeWrapper tw = typeWrapper.GetClassLoader().LoadClassByDottedName(c.Name);
								ilgen.Emit(OpCodes.Ldarg_0);
								ilgen.Emit(OpCodes.Isinst, tw.TypeAsTBD);
								ilgen.Emit(OpCodes.Dup);
								CodeEmitterLabel label = ilgen.DefineLabel();
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
						Type retType = typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig).TypeAsSignatureType;

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
							MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.HideBySig;
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
							if(m.Attributes != null)
							{
								foreach(IKVM.Internal.MapXml.Attribute custattr in m.Attributes)
								{
									AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mbCore, custattr);
								}
							}
							SetParameters(DeclaringType.GetClassLoader(), mbCore, m.Params);
							if(overrideMethod != null && !inherited)
							{
								typeWrapper.typeBuilder.DefineMethodOverride(mbCore, overrideMethod);
							}
							if(inherited)
							{
								AttributeHelper.HideFromReflection(mbCore);
							}
							AddDeclaredExceptions(mbCore, m.throws);
						}

						if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) == 0)
						{
							// instance methods must have an instancehelper method
							MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.HideBySig | MethodAttributes.Static;
							// NOTE instancehelpers for protected methods are made internal
							// and special cased in DotNetTypeWrapper.LazyPublishMembers
							if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Protected) != 0)
							{
								attr &= ~MethodAttributes.MemberAccessMask;
								attr |= MethodAttributes.Assembly;
							}
							Type[] exParamTypes = new Type[paramTypes.Length + 1];
							Array.Copy(paramTypes, 0, exParamTypes, 1, paramTypes.Length);
							exParamTypes[0] = typeWrapper.shadowType;
							mbHelper = typeWrapper.typeBuilder.DefineMethod("instancehelper_" + m.Name, attr, CallingConventions.Standard, retType, exParamTypes);
							if(m.Attributes != null)
							{
								foreach(IKVM.Internal.MapXml.Attribute custattr in m.Attributes)
								{
									AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mbHelper, custattr);
								}
							}
							IKVM.Internal.MapXml.Param[] parameters;
							if(m.Params == null)
							{
								parameters = new IKVM.Internal.MapXml.Param[1];
							}
							else
							{
								parameters = new IKVM.Internal.MapXml.Param[m.Params.Length + 1];
								m.Params.CopyTo(parameters, 1);
							}
							parameters[0] = new IKVM.Internal.MapXml.Param();
							parameters[0].Name = "this";
							SetParameters(DeclaringType.GetClassLoader(), mbHelper, parameters);
							if(!typeWrapper.IsFinal)
							{
								AttributeHelper.SetEditorBrowsableNever(mbHelper);
							}
							AttributeHelper.SetModifiers(mbHelper, (Modifiers)m.Modifiers, false);
							AttributeHelper.SetNameSig(mbHelper, m.Name, m.Sig);
							AddDeclaredExceptions(mbHelper, m.throws);
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
						CodeEmitter ilgen = CodeEmitter.Create(mbCore);
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
							IKVM.Internal.MapXml.CodeGenContext context = new IKVM.Internal.MapXml.CodeGenContext(DeclaringType.GetClassLoader());
							foreach(IKVM.Internal.MapXml.Instruction instr in m.body.invoke)
							{
								if(instr is IKVM.Internal.MapXml.Ret)
								{
									this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
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
							this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
							ilgen.Emit(OpCodes.Ret);
						}
						if(this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
						{
							ilgen.EmitLineNumberTable(mbCore);
						}
					}

					// NOTE static methods don't have helpers
					// NOTE for interface helpers we don't have to do anything,
					// because they've already been generated in DoLink
					// (currently this only applies to Comparable.compareTo).
					if(mbHelper != null && !this.DeclaringType.IsInterface)
					{
						CodeEmitter ilgen = CodeEmitter.Create(mbHelper);
						// check "this" for null
						if(m.@override != null && m.redirect == null && m.body == null && m.alternateBody == null)
						{
							// we're going to be calling the overridden version, so we don't need the null check
						}
						else if(!m.NoNullCheck)
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
							CodeEmitterLabel skip = ilgen.DefineLabel();
							ilgen.Emit(OpCodes.Brfalse_S, skip);
							for(int i = 0; i < paramTypes.Length; i++)
							{
								ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
							}
							ilgen.Emit(OpCodes.Callvirt, mbCore);
							this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
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
								CodeEmitterLabel skip = ilgen.DefineLabel();
								ilgen.Emit(OpCodes.Brfalse_S, skip);
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, (short)(i + 1));
								}
								mw.Link();
								mw.EmitCallvirt(ilgen);
								this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
								ilgen.Emit(OpCodes.Ret);
								ilgen.MarkLabel(skip);
								ilgen.Emit(OpCodes.Pop);
							}
						}
						if(m.body != null || m.alternateBody != null)
						{
							IKVM.Internal.MapXml.InstructionList body = m.alternateBody == null ? m.body : m.alternateBody;
							// we manually walk the instruction list, because we need to special case the ret instructions
							IKVM.Internal.MapXml.CodeGenContext context = new IKVM.Internal.MapXml.CodeGenContext(DeclaringType.GetClassLoader());
							foreach(IKVM.Internal.MapXml.Instruction instr in body.invoke)
							{
								if(instr is IKVM.Internal.MapXml.Ret)
								{
									this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
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
							this.ReturnType.EmitConvStackTypeToSignatureType(ilgen, null);
							ilgen.Emit(OpCodes.Ret);
						}
						if(this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
						{
							ilgen.EmitLineNumberTable(mbHelper);
						}
					}

					// do we need a helper for non-virtual reflection invocation?
					if(m.nonvirtualAlternateBody != null || (m.@override != null && overriders.Count > 0))
					{
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
						Type[] argTypes = new Type[paramTypes.Length + 1];
						argTypes[0] = typeWrapper.TypeAsSignatureType;
						this.GetParametersForDefineMethod().CopyTo(argTypes, 1);
						MethodBuilder mb = typeWrapper.typeBuilder.DefineMethod("nonvirtualhelper/" + this.Name, MethodAttributes.Private | MethodAttributes.Static, this.ReturnTypeForDefineMethod, argTypes);
						if(m.Attributes != null)
						{
							foreach(IKVM.Internal.MapXml.Attribute custattr in m.Attributes)
							{
								AttributeHelper.SetCustomAttribute(DeclaringType.GetClassLoader(), mb, custattr);
							}
						}
						SetParameters(DeclaringType.GetClassLoader(), mb, m.Params);
						AttributeHelper.HideFromJava(mb);
						CodeEmitter ilgen = CodeEmitter.Create(mb);
						if(m.nonvirtualAlternateBody != null)
						{
							m.nonvirtualAlternateBody.Emit(DeclaringType.GetClassLoader(), ilgen);
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

				private void EmitRedirect(Type baseType, CodeEmitter ilgen)
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
					ClassLoaderWrapper classLoader = DeclaringType.GetClassLoader();
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
						TypeWrapper tw = classLoader.LoadClassByDottedName(m.redirect.Class);
						MethodWrapper mw = tw.GetMethodWrapper(redirName, redirSig, false);
						if(mw == null)
						{
							throw new InvalidOperationException("Missing redirect method: " + tw.Name + "." + redirName + redirSig);
						}
						mw.Link();
						mw.EmitCall(ilgen);
					}
				}
			}

			private static void SetParameters(ClassLoaderWrapper loader, MethodBuilder mb, IKVM.Internal.MapXml.Param[] parameters)
			{
				if(parameters != null)
				{
					for(int i = 0; i < parameters.Length; i++)
					{
						ParameterBuilder pb = mb.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
						if(parameters[i].Attributes != null)
						{
							for(int j = 0; j < parameters[i].Attributes.Length; j++)
							{
								AttributeHelper.SetCustomAttribute(loader, pb, parameters[i].Attributes[j]);
							}
						}
					}
				}
			}

			private static void SetParameters(ClassLoaderWrapper loader, ConstructorBuilder cb, IKVM.Internal.MapXml.Param[] parameters)
			{
				if(parameters != null)
				{
					for(int i = 0; i < parameters.Length; i++)
					{
						ParameterBuilder pb = cb.DefineParameter(i + 1, ParameterAttributes.None, parameters[i].Name);
						if(parameters[i].Attributes != null)
						{
							for(int j = 0; j < parameters[i].Attributes.Length; j++)
							{
								AttributeHelper.SetCustomAttribute(loader, pb, parameters[i].Attributes[j]);
							}
						}
					}
				}
			}

			internal void Process2ndPassStep1(IKVM.Internal.MapXml.Root map)
			{
				IKVM.Internal.MapXml.Class c = classDef;
				TypeBuilder tb = typeBuilder;
				bool baseIsSealed = shadowType.IsSealed;

				if(c.Interfaces != null)
				{
					interfaceWrappers = new TypeWrapper[c.Interfaces.Length];
					for(int i = 0; i < c.Interfaces.Length; i++)
					{
						TypeWrapper ifaceTypeWrapper = classLoader.LoadClassByDottedName(c.Interfaces[i].Name);
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
			}

			internal void Process2ndPassStep2(IKVM.Internal.MapXml.Root map)
			{
				IKVM.Internal.MapXml.Class c = classDef;
				TypeBuilder tb = typeBuilder;

				List<FieldWrapper> fields = new List<FieldWrapper>();

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
							fields.Add(new GetterFieldWrapper(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), null, f.Name, f.Sig, new ExModifiers((Modifiers)f.Modifiers, false), (MethodInfo)method.GetMethod(), null));
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
							FieldBuilder fb = tb.DefineField(f.Name, GetClassLoader().FieldTypeWrapperFromSig(f.Sig).TypeAsSignatureType, attr);
							if(f.Attributes != null)
							{
								foreach(IKVM.Internal.MapXml.Attribute custattr in f.Attributes)
								{
									AttributeHelper.SetCustomAttribute(classLoader, fb, custattr);
								}
							}
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
								fields.Add(new ConstantFieldWrapper(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), f.Name, f.Sig, (Modifiers)f.Modifiers, fb, constant, MemberFlags.None));
							}
							else
							{
								fields.Add(FieldWrapper.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig), fb, f.Name, f.Sig, new ExModifiers((Modifiers)f.Modifiers, false)));
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
				SetFields(fields.ToArray());
			}

			internal void Process3rdPass()
			{
				foreach(RemappedMethodBaseWrapper m in GetMethods())
				{
					m.Link();
				}
			}

			internal void Process4thPass(ICollection<TypeWrapper> remappedTypes)
			{
				foreach(RemappedMethodBaseWrapper m in GetMethods())
				{
					m.Finish();
				}

				if(classDef.Clinit != null)
				{
					ConstructorBuilder cb = typeBuilder.DefineTypeInitializer();
					CodeEmitter ilgen = CodeEmitter.Create(cb);
					// TODO emit code to make sure super class is initialized
					classDef.Clinit.body.Emit(classLoader, ilgen);
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
					// For all inherited methods, we emit a method that hides the inherited method and
					// annotate it with EditorBrowsableAttribute(EditorBrowsableState.Never) to make
					// sure the inherited methods don't show up in Intellisense.
					Dictionary<string, MethodBuilder> methods = new Dictionary<string, MethodBuilder>();
					foreach(MethodInfo mi in typeBuilder.BaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy))
					{
						string key = MakeMethodKey(mi);
						if(!methods.ContainsKey(key))
						{
							ParameterInfo[] paramInfo = mi.GetParameters();
							Type[] paramTypes = new Type[paramInfo.Length];
							for(int i = 0; i < paramInfo.Length; i++)
							{
								paramTypes[i] = paramInfo[i].ParameterType;
							}
							MethodBuilder mb = typeBuilder.DefineMethod(mi.Name, mi.Attributes & (MethodAttributes.MemberAccessMask | MethodAttributes.SpecialName | MethodAttributes.Static), mi.ReturnType, paramTypes);
							AttributeHelper.HideFromJava(mb);
							AttributeHelper.SetEditorBrowsableNever(mb);
							CopyLinkDemands(mb, mi);
							CodeEmitter ilgen = CodeEmitter.Create(mb);
							for(int i = 0; i < paramTypes.Length; i++)
							{
								ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
							}
							if(!mi.IsStatic)
							{
								ilgen.Emit(OpCodes.Ldarg_S, (byte)paramTypes.Length);
								ilgen.Emit(OpCodes.Callvirt, mi);
							}
							else
							{
								ilgen.Emit(OpCodes.Call, mi);
							}
							ilgen.Emit(OpCodes.Ret);
							methods[key] = mb;
						}
					}
					foreach(PropertyInfo pi in typeBuilder.BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
					{
						ParameterInfo[] paramInfo = pi.GetIndexParameters();
						Type[] paramTypes = new Type[paramInfo.Length];
						for(int i = 0; i < paramInfo.Length; i++)
						{
							paramTypes[i] = paramInfo[i].ParameterType;
						}
						PropertyBuilder pb = typeBuilder.DefineProperty(pi.Name, PropertyAttributes.None, pi.PropertyType, paramTypes);
						if(pi.CanRead)
						{
							pb.SetGetMethod((MethodBuilder)methods[MakeMethodKey(pi.GetGetMethod())]);
						}
						if(pi.CanWrite)
						{
							pb.SetSetMethod((MethodBuilder)methods[MakeMethodKey(pi.GetSetMethod())]);
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

			private static void CopyLinkDemands(MethodBuilder mb, MethodInfo mi)
			{
				foreach (object attr in mi.GetCustomAttributes(false))
				{
					CodeAccessSecurityAttribute cas = attr as CodeAccessSecurityAttribute;
					if (cas != null)
					{
						if (cas.Action == SecurityAction.LinkDemand)
						{
							PermissionSet pset = new PermissionSet(PermissionState.None);
							pset.AddPermission(cas.CreatePermission());
							mb.AddDeclarativeSecurity(SecurityAction.LinkDemand, pset);
						}
					}
				}
			}

			private static string MakeMethodKey(MethodInfo method)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append(method.ReturnType.AssemblyQualifiedName).Append(":").Append(method.Name);
				ParameterInfo[] paramInfo = method.GetParameters();
				Type[] paramTypes = new Type[paramInfo.Length];
				for(int i = 0; i < paramInfo.Length; i++)
				{
					paramTypes[i] = paramInfo[i].ParameterType;
					sb.Append(":").Append(paramInfo[i].ParameterType.AssemblyQualifiedName);
				}
				return sb.ToString();
			}

			private void CreateShadowInstanceOf(ICollection<TypeWrapper> remappedTypes)
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
				CodeEmitter ilgen = CodeEmitter.Create(mb);

				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Isinst, shadowType);
				CodeEmitterLabel retFalse = ilgen.DefineLabel();
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

			private void CreateShadowCheckCast(ICollection<TypeWrapper> remappedTypes)
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
				CodeEmitter ilgen = CodeEmitter.Create(mb);

				CodeEmitterLabel fail = ilgen.DefineLabel();
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
				EmitHelper.Castclass(ilgen, shadowType);
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

			internal override void Finish()
			{
				if(BaseTypeWrapper != null)
				{
					BaseTypeWrapper.Finish();
				}
				foreach(TypeWrapper iface in Interfaces)
				{
					iface.Finish();
				}
				foreach(MethodWrapper m in GetMethods())
				{
					m.Link();
				}
				foreach(FieldWrapper f in GetFields())
				{
					f.Link();
				}
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					return TypeWrapper.EmptyArray;
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

			internal override string GetGenericSignature()
			{
				return null;
			}

			internal override string GetGenericMethodSignature(MethodWrapper mw)
			{
				return null;
			}

			internal override string GetGenericFieldSignature(FieldWrapper fw)
			{
				return null;
			}

			internal override string[] GetEnclosingMethod()
			{
				return null;
			}
		}

		internal void EmitRemappedTypes()
		{
			Tracer.Info(Tracer.Compiler, "Emit remapped types");

			assemblyAttributes = map.assembly.Attributes;

			if(map.assembly.Classes != null)
			{
				// 1st pass, put all types in remapped to make them loadable
				bool hasRemappedTypes = false;
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if(c.Shadows != null)
					{
						remapped.Add(c.Name, new RemapperTypeWrapper(this, c, map));
						hasRemappedTypes = true;
					}
				}

				if(hasRemappedTypes)
				{
					SetupGhosts(map);
				}

				// 2nd pass, resolve interfaces, publish methods/fields
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if(c.Shadows != null)
					{
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)remapped[c.Name];
						typeWrapper.Process2ndPassStep1(map);
					}
				}
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if(c.Shadows != null)
					{
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)remapped[c.Name];
						typeWrapper.Process2ndPassStep2(map);
					}
				}
			}
		}

		internal bool IsMapUnsafeException(TypeWrapper tw)
		{
			if(mappedExceptions != null)
			{
				for(int i = 0; i < mappedExceptions.Length; i++)
				{
					if(mappedExceptions[i].IsSubTypeOf(tw) ||
						(mappedExceptionsAllSubClasses[i] && tw.IsSubTypeOf(mappedExceptions[i])))
					{
						return true;
					}
				}
			}
			return false;
		}

		internal void LoadMappedExceptions(IKVM.Internal.MapXml.Root map)
		{
			if(map.exceptionMappings != null)
			{
				mappedExceptionsAllSubClasses = new bool[map.exceptionMappings.Length];
				mappedExceptions = new TypeWrapper[map.exceptionMappings.Length];
				for(int i = 0; i < mappedExceptions.Length; i++)
				{
					string dst = map.exceptionMappings[i].dst;
					if(dst[0] == '*')
					{
						mappedExceptionsAllSubClasses[i] = true;
						dst = dst.Substring(1);
					}
					mappedExceptions[i] = LoadClassByDottedName(dst);
				}
				// HACK we need to find the <exceptionMapping /> element and bind it
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if(c.Methods != null)
					{
						foreach(IKVM.Internal.MapXml.Method m in c.Methods)
						{
							if(m.body != null && m.body.invoke != null)
							{
								foreach(IKVM.Internal.MapXml.Instruction instr in m.body.invoke)
								{
									IKVM.Internal.MapXml.EmitExceptionMapping eem = instr as IKVM.Internal.MapXml.EmitExceptionMapping;
									if(eem != null)
									{
										eem.mapping = map.exceptionMappings;
									}
								}
							}
						}
					}
				}
			}
		}

		internal sealed class ExceptionMapEmitter
		{
			private IKVM.Internal.MapXml.ExceptionMapping[] map;

			internal ExceptionMapEmitter(IKVM.Internal.MapXml.ExceptionMapping[] map)
			{
				this.map = map;
			}

			internal void Emit(IKVM.Internal.MapXml.CodeGenContext context, CodeEmitter ilgen)
			{
				MethodWrapper mwSuppressFillInStackTrace = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper("__<suppressFillInStackTrace>", "()V", false);
				mwSuppressFillInStackTrace.Link();
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Callvirt, typeof(Object).GetMethod("GetType"));
				MethodInfo GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle");
				for(int i = 0; i < map.Length; i++)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Ldtoken, Type.GetType(map[i].src));
					ilgen.Emit(OpCodes.Call, GetTypeFromHandle);
					ilgen.Emit(OpCodes.Ceq);
					CodeEmitterLabel label = ilgen.DefineLabel();
					ilgen.Emit(OpCodes.Brfalse_S, label);
					ilgen.Emit(OpCodes.Pop);
					if(map[i].code != null)
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						if(map[i].code.invoke != null)
						{
							foreach(MapXml.Instruction instr in map[i].code.invoke)
							{
								MapXml.NewObj newobj = instr as MapXml.NewObj;
								if(newobj != null
									&& newobj.Class != null
									&& context.ClassLoader.LoadClassByDottedName(newobj.Class).IsSubTypeOf(CoreClasses.java.lang.Throwable.Wrapper))
								{
									mwSuppressFillInStackTrace.EmitCall(ilgen);
								}
								instr.Generate(context, ilgen);
							}
						}
						ilgen.Emit(OpCodes.Ret);
					}
					else
					{
						TypeWrapper tw = context.ClassLoader.LoadClassByDottedName(map[i].dst);
						MethodWrapper mw = tw.GetMethodWrapper("<init>", "()V", false);
						mw.Link();
						mwSuppressFillInStackTrace.EmitCall(ilgen);
						mw.EmitNewobj(ilgen);
						ilgen.Emit(OpCodes.Ret);
					}
					ilgen.MarkLabel(label);
				}
				ilgen.Emit(OpCodes.Pop);
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Ret);
			}
		}

		internal void LoadMapXml()
		{
			if(map.assembly.Classes != null)
			{
				mapxml_Classes = new Dictionary<string, IKVM.Internal.MapXml.Class>();
				mapxml_MethodBodies = new Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList>();
				mapxml_ReplacedMethods = new Dictionary<MethodKey, IKVM.Internal.MapXml.ReplaceMethodCall[]>();
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					// HACK if it is not a remapped type, we assume it is a container for native methods
					if(c.Shadows == null)
					{
						string className = c.Name;
						mapxml_Classes.Add(className, c);
						if(c.Methods != null)
						{
							foreach(IKVM.Internal.MapXml.Method method in c.Methods)
							{
								if(method.body != null)
								{
									string methodName = method.Name;
									string methodSig = method.Sig;
									mapxml_MethodBodies.Add(new MethodKey(className, methodName, methodSig), method.body);
								}
								if(method.ReplaceMethodCalls != null)
								{
									mapxml_ReplacedMethods.Add(new MethodKey(className, method.Name, method.Sig), method.ReplaceMethodCalls);
								}
							}
						}
					}
				}
			}
		}

		internal IKVM.Internal.MapXml.ReplaceMethodCall[] GetReplacedMethodsFor(MethodWrapper mw)
		{
			if(mapxml_ReplacedMethods == null)
			{
				return null;
			}
			IKVM.Internal.MapXml.ReplaceMethodCall[] rmc;
			mapxml_ReplacedMethods.TryGetValue(new MethodKey(mw.DeclaringType.Name, mw.Name, mw.Signature), out rmc);
			return rmc;
		}

		internal Dictionary<string, IKVM.Internal.MapXml.Class> GetMapXmlClasses()
		{
			return mapxml_Classes;
		}

		internal Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList> GetMapXmlMethodBodies()
		{
			return mapxml_MethodBodies;
		}

		internal IKVM.Internal.MapXml.Param[] GetXmlMapParameters(string classname, string method, string sig)
		{
			if(mapxml_Classes != null)
			{
				IKVM.Internal.MapXml.Class clazz;
				if(mapxml_Classes.TryGetValue(classname, out clazz))
				{
					if(method == "<init>" && clazz.Constructors != null)
					{
						for(int i = 0; i < clazz.Constructors.Length; i++)
						{
							if(clazz.Constructors[i].Sig == sig)
							{
								return clazz.Constructors[i].Params;
							}
						}
					}
					else if(clazz.Methods != null)
					{
						for(int i = 0; i < clazz.Methods.Length; i++)
						{
							if(clazz.Methods[i].Name == method && clazz.Methods[i].Sig == sig)
							{
								return clazz.Methods[i].Params;
							}
						}
					}
				}
			}
			return null;
		}

		internal bool IsGhost(TypeWrapper tw)
		{
			return ghosts != null && tw.IsInterface && ghosts.ContainsKey(tw.Name);
		}

		private void SetupGhosts(IKVM.Internal.MapXml.Root map)
		{
			ghosts = new Dictionary<string, List<TypeWrapper>>();

			// find the ghost interfaces
			foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
			{
				if(c.Shadows != null && c.Interfaces != null)
				{
					// NOTE we don't support interfaces that inherit from other interfaces
					// (actually, if they are explicitly listed it would probably work)
					TypeWrapper typeWrapper = GetLoadedClass(c.Name);
					foreach(IKVM.Internal.MapXml.Interface iface in c.Interfaces)
					{
						TypeWrapper ifaceWrapper = GetLoadedClass(iface.Name);
						if(ifaceWrapper == null || !ifaceWrapper.TypeAsTBD.IsAssignableFrom(typeWrapper.TypeAsTBD))
						{
							AddGhost(iface.Name, typeWrapper);
						}
					}
				}
			}
			// we manually add the array ghost interfaces
			TypeWrapper array = ClassLoaderWrapper.GetWrapperFromType(typeof(Array));
			AddGhost("java.io.Serializable", array);
			AddGhost("java.lang.Cloneable", array);
		}

		private void AddGhost(string interfaceName, TypeWrapper implementer)
		{
			List<TypeWrapper> list;
			if(!ghosts.TryGetValue(interfaceName, out list))
			{
				list = new List<TypeWrapper>();
				ghosts[interfaceName] = list;
			}
			list.Add(implementer);
		}

		internal TypeWrapper[] GetGhostImplementers(TypeWrapper wrapper)
		{
			List<TypeWrapper> list;
			if (!ghosts.TryGetValue(wrapper.Name, out list))
			{
				return TypeWrapper.EmptyArray;
			}
			return list.ToArray();
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

			if(assemblyAttributes != null)
			{
				foreach(IKVM.Internal.MapXml.Attribute attr in assemblyAttributes)
				{
					AttributeHelper.SetCustomAttribute(this, assemblyBuilder, attr);
				}
			}
		}

		private static bool IsSigned(Assembly asm)
		{
			byte[] key = asm.GetName().GetPublicKey();
			return key != null && key.Length != 0;
		}

		private static bool IsCoreAssembly(Assembly asm)
		{
			return AttributeHelper.IsDefined(asm, StaticCompiler.GetType("IKVM.Attributes.RemappedClassAttribute"));
		}

		private bool CheckCompilingCoreAssembly()
		{
			if (map.assembly.Classes != null)
			{
				foreach (IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if (c.Shadows != null && c.Name == "java.lang.Object")
					{
						return true;
					}
				}
			}
			return false;
		}

		internal static int Compile(List<CompilerOptions> optionsList)
		{
			List<CompilerClassLoader> compilers = new List<CompilerClassLoader>();
			foreach (CompilerOptions options in optionsList)
			{
				CompilerClassLoader compiler = null;
				int rc = CreateCompiler(options, ref compiler);
				if(rc != 0)
				{
					return rc;
				}
				compilers.Add(compiler);
			}
			bool compilingCoreAssembly = false;
			foreach (CompilerClassLoader compiler1 in compilers)
			{
				if (compiler1.compilingCoreAssembly)
				{
					compilingCoreAssembly = true;
				}
				foreach (CompilerClassLoader compiler2 in compilers)
				{
					if (compiler1 != compiler2)
					{
						compiler1.AddReference(compiler2);
					}
				}
			}
			if (!compilingCoreAssembly)
			{
				ClassLoaderWrapper.SetBootstrapClassLoader(ClassLoaderWrapper.GetAssemblyClassLoader(JVM.CoreAssembly));
			}
			foreach (CompilerClassLoader compiler in compilers)
			{
				int rc = compiler.Compile();
				if (rc != 0)
				{
					return rc;
				}
			}
			return 0;
		}

		private static int CreateCompiler(CompilerOptions options, ref CompilerClassLoader loader)
		{
			Tracer.Info(Tracer.Compiler, "JVM.Compile path: {0}, assembly: {1}", options.path, options.assembly);
			try
			{
				if(options.runtimeAssembly == null)
				{
					// HACK based on our assembly name we create the default runtime assembly name
					Assembly compilerAssembly = typeof(CompilerClassLoader).Assembly;
					StaticCompiler.runtimeAssembly = Assembly.ReflectionOnlyLoad(compilerAssembly.FullName.Replace(compilerAssembly.GetName().Name, "IKVM.Runtime"));
					StaticCompiler.runtimeJniAssembly = Assembly.ReflectionOnlyLoad(compilerAssembly.FullName.Replace(compilerAssembly.GetName().Name, "IKVM.Runtime.JNI"));
				}
				else
				{
					StaticCompiler.runtimeAssembly = Assembly.ReflectionOnlyLoadFrom(options.runtimeAssembly);
					StaticCompiler.runtimeJniAssembly = Assembly.ReflectionOnlyLoadFrom(Path.Combine(StaticCompiler.runtimeAssembly.CodeBase, ".." + Path.DirectorySeparatorChar + "IKVM.Runtime.JNI.dll"));
				}
			}
			catch(FileNotFoundException)
			{
				if(StaticCompiler.runtimeAssembly == null)
				{
					Console.Error.WriteLine("Error: unable to load runtime assembly");
					return 1;
				}
				StaticCompiler.IssueMessage(Message.NoJniRuntime);
			}
			Tracer.Info(Tracer.Compiler, "Loaded runtime assembly: {0}", StaticCompiler.runtimeAssembly.FullName);
			AssemblyName runtimeAssemblyName = StaticCompiler.runtimeAssembly.GetName();
			bool allReferencesAreStrongNamed = IsSigned(StaticCompiler.runtimeAssembly);
			List<Assembly> references = new List<Assembly>();
			foreach(string r in options.references)
			{
				try
				{
					Assembly reference = Assembly.ReflectionOnlyLoadFrom(r);
					if(IsCoreAssembly(reference))
					{
						JVM.CoreAssembly = reference;
					}
					if(reference == null)
					{
						Console.Error.WriteLine("Error: reference not found: {0}", r);
						return 1;
					}
					references.Add(reference);
					allReferencesAreStrongNamed &= IsSigned(reference);
					Tracer.Info(Tracer.Compiler, "Loaded reference assembly: {0}", reference.FullName);
					// if it's an IKVM compiled assembly, make sure that it was compiled
					// against same version of the runtime
					foreach(AssemblyName asmref in reference.GetReferencedAssemblies())
					{
						if(asmref.Name == runtimeAssemblyName.Name)
						{
							if(IsSigned(StaticCompiler.runtimeAssembly))
							{
								if(asmref.FullName != runtimeAssemblyName.FullName)
								{
									Console.Error.WriteLine("Error: referenced assembly {0} was compiled with an incompatible IKVM.Runtime version ({1})", r, asmref.Version);
									Console.Error.WriteLine("   Current runtime: {0}", runtimeAssemblyName.FullName);
									Console.Error.WriteLine("   Referenced assembly runtime: {0}", asmref.FullName);
									return 1;
								}
							}
							else
							{
								if(asmref.GetPublicKeyToken() != null && asmref.GetPublicKeyToken().Length != 0)
								{
									Console.Error.WriteLine("Error: referenced assembly {0} was compiled with an incompatible (signed) IKVM.Runtime version", r);
									Console.Error.WriteLine("   Current runtime: {0}", runtimeAssemblyName.FullName);
									Console.Error.WriteLine("   Referenced assembly runtime: {0}", asmref.FullName);
									return 1;
								}
							}
						}
					}
				}
				catch(Exception x)
				{
					Console.Error.WriteLine("Error: invalid reference: {0} ({1})", r, x.Message);
					return 1;
				}
			}
			bool err = false;
			foreach(Assembly reference in references)
			{
				try
				{
					reference.GetTypes();
				}
				catch(ReflectionTypeLoadException x)
				{
					err = true;
					foreach(Exception n in x.LoaderExceptions)
					{
						FileNotFoundException f = n as FileNotFoundException;
						if(f != null)
						{
							Console.Error.WriteLine("Error: referenced assembly {0} has a missing dependency: {1}", reference.GetName().Name, f.FileName);
							goto next;
						}
					}
					Console.Error.WriteLine("Error: referenced assembly produced the following loader exceptions:");
					foreach(Exception n in x.LoaderExceptions)
					{
						Console.WriteLine(n.Message);
					}
				}
			next:;
			}
			if(err)
			{
				return 1;
			}
			// If the "System" assembly wasn't explicitly referenced, load it automatically
			bool systemIsLoaded = false;
			foreach(Assembly asm in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
			{
				if(asm.GetType("System.ComponentModel.EditorBrowsableAttribute", false, false) != null)
				{
					systemIsLoaded = true;
					break;
				}
			}
			if(!systemIsLoaded)
			{
				Assembly.ReflectionOnlyLoadFrom(typeof(System.ComponentModel.EditorBrowsableAttribute).Assembly.Location);
			}
			List<object> assemblyAnnotations = new List<object>();
			Dictionary<string, string> baseClasses = new Dictionary<string, string>();
			Tracer.Info(Tracer.Compiler, "Parsing class files");
			foreach(KeyValuePair<string, byte[]> kv in options.classes)
			{
				ClassFile f;
				try
				{
					byte[] buf = kv.Value;
					f = new ClassFile(buf, 0, buf.Length, null, ClassFileParseOptions.None);
					if(!f.IsInterface && f.SuperClass != null)
					{
						baseClasses[f.SuperClass] = f.SuperClass;
					}
					// NOTE the "assembly" type in the unnamed package is a magic type
					// that acts as the placeholder for assembly attributes
					if(f.Name == "assembly" && f.Annotations != null)
					{
						assemblyAnnotations.AddRange(f.Annotations);
					}
				}
				catch(ClassFormatError)
				{
					continue;
				}
				if(options.mainClass == null && (options.guessFileKind || options.target != PEFileKinds.Dll))
				{
					foreach(ClassFile.Method m in f.Methods)
					{
						if(m.IsPublic && m.IsStatic && m.Name == "main" && m.Signature == "([Ljava.lang.String;)V")
						{
							StaticCompiler.IssueMessage(Message.MainMethodFound, f.Name);
							options.mainClass = f.Name;
							break;
						}
					}
				}
			}
			Dictionary<string, byte[]> h = new Dictionary<string, byte[]>();
			// HACK remove "assembly" type that exists only as a placeholder for assembly attributes
			options.classes.Remove("assembly");
			foreach(KeyValuePair<string, byte[]> kv in options.classes)
			{
				string name = kv.Key;
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
					StaticCompiler.IssueMessage(Message.DuplicateClassName, name);
					excluded = true;
				}
				if(!excluded)
				{
					h[name] = kv.Value;
				}
			}
			options.classes = null;

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
				StaticCompiler.IssueMessage(Message.OutputFileIs, options.path);
			}

			if(options.targetIsModule)
			{
				if(options.classLoader != null)
				{
					Console.Error.WriteLine("Error: cannot specify assembly class loader for modules");
					return 1;
				}
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

			Tracer.Info(Tracer.Compiler, "Constructing compiler");
			AssemblyClassLoader[] referencedAssemblies = new AssemblyClassLoader[references.Count];
			for(int i = 0; i < references.Count; i++)
			{
				referencedAssemblies[i] = ClassLoaderWrapper.GetAssemblyClassLoader(references[i]);
			}
			loader = new CompilerClassLoader(referencedAssemblies, options, options.path, options.keyfilename, options.keycontainer, options.version, options.targetIsModule, options.assembly, h);
			loader.baseClasses = baseClasses;
			loader.assemblyAnnotations = assemblyAnnotations;
			loader.classesToCompile = new List<string>(h.Keys);
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
					loader.map = (IKVM.Internal.MapXml.Root)ser.Deserialize(rdr);
				}
				if(loader.CheckCompilingCoreAssembly())
				{
					loader.compilingCoreAssembly = true;
					ClassLoaderWrapper.SetBootstrapClassLoader(loader);
					loader.EmitRemappedTypes();
				}
			}
			// If we do not yet have a reference to the core assembly and we are not compiling the core assembly,
			// try to find the core assembly by looking at the assemblies that the runtime references
			if(JVM.CoreAssembly == null && !loader.compilingCoreAssembly)
			{
				foreach(AssemblyName name in StaticCompiler.runtimeAssembly.GetReferencedAssemblies())
				{
					Assembly asm = null;
					try
					{
						asm = Assembly.ReflectionOnlyLoad(name.FullName);
					}
					catch(FileNotFoundException)
					{
						try
						{
							asm = Assembly.ReflectionOnlyLoadFrom(StaticCompiler.runtimeAssembly.CodeBase + "\\..\\" + name.Name + ".dll");
						}
						catch(FileNotFoundException)
						{
						}
					}
					if(asm != null && IsCoreAssembly(asm))
					{
						JVM.CoreAssembly = asm;
						break;
					}
				}
				if(JVM.CoreAssembly == null)
				{
					Console.Error.WriteLine("Error: bootstrap classes missing and core assembly not found");
					return 1;
				}
				// we need to scan again for remapped types, now that we've loaded the core library
				ClassLoaderWrapper.LoadRemappedTypes();
			}

			if(!loader.compilingCoreAssembly)
			{
				allReferencesAreStrongNamed &= IsSigned(JVM.CoreAssembly);
				loader.AddReference(ClassLoaderWrapper.GetAssemblyClassLoader(JVM.CoreAssembly));
			}

			if((options.keycontainer != null || options.keyfilename != null) && !allReferencesAreStrongNamed)
			{
				Console.Error.WriteLine("Error: all referenced assemblies must be strong named, to be able to sign the output assembly");
				return 1;
			}

			if(loader.map != null)
			{
				loader.LoadMapXml();
			}

			if(!loader.compilingCoreAssembly)
			{
				FakeTypes.Load(JVM.CoreAssembly);
			}
			else
			{
				FakeTypes.CreatePre(loader.GetTypeWrapperFactory().ModuleBuilder);
			}
			return 0;
		}

		private int Compile()
		{
			Tracer.Info(Tracer.Compiler, "Compiling class files (1)");
			List<TypeWrapper> allwrappers = new List<TypeWrapper>();
			foreach(string s in classesToCompile)
			{
				TypeWrapper wrapper = LoadClassByDottedNameFast(s);
				if(wrapper != null)
				{
					ClassLoaderWrapper loader = wrapper.GetClassLoader();
					if(loader != this)
					{
						if(!(loader is GenericClassLoader || loader is CompilerClassLoader))
						{
							StaticCompiler.IssueMessage(Message.SkippingReferencedClass, s, ((AssemblyClassLoader)loader).Assembly.FullName);
						}
						continue;
					}
					if(map == null)
					{
						wrapper.Finish();
					}
					int pos = wrapper.Name.LastIndexOf('.');
					if(pos != -1)
					{
						packages[wrapper.Name.Substring(0, pos)] = "";
					}
					allwrappers.Add(wrapper);
				}
			}
			if(options.mainClass != null)
			{
				TypeWrapper wrapper = null;
				try
				{
					wrapper = LoadClassByDottedNameFast(options.mainClass);
				}
				catch(RetargetableJavaException)
				{
				}
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
				if(!method.DeclaringType.Assembly.Equals(assemblyBuilder)
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
				SetMain(method, options.target, options.props, options.noglobbing, apartmentAttributeType);
			}
			if(map != null)
			{
				LoadMappedExceptions(map);
				// mark all exceptions that are unsafe for mapping with a custom attribute,
				// so that at runtime we can quickly assertain if an exception type can be
				// caught without filtering
				foreach(TypeWrapper tw in allwrappers)
				{
					if(!tw.IsInterface && tw.IsMapUnsafeException)
					{
						AttributeHelper.SetExceptionIsUnsafeForMapping(tw.TypeAsBuilder);
					}
				}
				Tracer.Info(Tracer.Compiler, "Loading remapped types (2)");
				FinishRemappedTypes();
				// if we're compiling the core class library, generate the "fake" generic types
				// that represent the not-really existing types (i.e. the Java enums that represent .NET enums,
				// the Method interface for delegates and the Annotation annotation for custom attributes)
				if(compilingCoreAssembly)
				{
					FakeTypes.Create(GetTypeWrapperFactory().ModuleBuilder, this);
				}
			}
			Tracer.Info(Tracer.Compiler, "Compiling class files (2)");
			AddResources(options.resources, options.compressedResources);
			if(options.externalResources != null)
			{
				foreach(KeyValuePair<string, string> kv in options.externalResources)
				{
					assemblyBuilder.AddResourceFile(JVM.MangleResourceName(kv.Key), kv.Value);
				}
			}
			if(options.fileversion != null)
			{
				CustomAttributeBuilder filever = new CustomAttributeBuilder(typeof(AssemblyFileVersionAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { options.fileversion });
				assemblyBuilder.SetCustomAttribute(filever);
			}
			foreach(object[] def in assemblyAnnotations)
			{
				Annotation annotation = Annotation.Load(this, def);
				if(annotation != null)
				{
					annotation.Apply(this, assemblyBuilder, def);
				}
			}
			if(options.classLoader != null)
			{
				TypeWrapper wrapper = null;
				try
				{
					wrapper = LoadClassByDottedNameFast(options.classLoader);
				}
				catch(RetargetableJavaException)
				{
				}
				if(wrapper == null)
				{
					Console.Error.WriteLine("Error: custom assembly class loader class not found");
					return 1;
				}
				if(!wrapper.IsPublic && !wrapper.TypeAsBaseType.Assembly.Equals(assemblyBuilder))
				{
					Console.Error.WriteLine("Error: custom assembly class loader class is not accessible");
					return 1;
				}
				if(wrapper.IsAbstract)
				{
					Console.Error.WriteLine("Error: custom assembly class loader class is abstract");
					return 1;
				}
				if(!wrapper.IsAssignableTo(ClassLoaderWrapper.LoadClassCritical("java.lang.ClassLoader")))
				{
					Console.Error.WriteLine("Error: custom assembly class loader class does not extend java.lang.ClassLoader");
					return 1;
				}
				MethodWrapper mw = wrapper.GetMethodWrapper("<init>", "(Lcli.System.Reflection.Assembly;)V", false);
				if(mw == null)
				{
					Console.Error.WriteLine("Error: custom assembly class loader constructor is missing");
					return 1;
				}
				ConstructorInfo ci = JVM.LoadType(typeof(CustomAssemblyClassLoaderAttribute)).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(Type) }, null);
				assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(ci, new object[] { wrapper.TypeAsTBD }));
			}
			assemblyBuilder.DefineVersionInfoResource();
			Save();
			return 0;
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

	class CompilerOptions
	{
		internal string path;
		internal string keyfilename;
		internal string keycontainer;
		internal string version;
		internal string fileversion;
		internal bool targetIsModule;
		internal string assembly;
		internal string mainClass;
		internal ApartmentState apartment;
		internal PEFileKinds target;
		internal bool guessFileKind;
		internal Dictionary<string, byte[]> classes;
		internal string[] references;
		internal Dictionary<string, byte[]> resources;
		internal string[] classesToExclude;
		internal string remapfile;
		internal Dictionary<string, string> props;
		internal bool noglobbing;
		internal CodeGenOptions codegenoptions;
		internal bool removeUnusedFields;
		internal bool compressedResources;
		internal string runtimeAssembly;
		internal string[] privatePackages;
		internal string sourcepath;
		internal Dictionary<string, string> externalResources;
		internal string classLoader;
		internal PortableExecutableKinds pekind = PortableExecutableKinds.ILOnly;
		internal ImageFileMachine imageFileMachine = ImageFileMachine.I386;

		internal CompilerOptions Copy()
		{
			CompilerOptions copy = (CompilerOptions)MemberwiseClone();
			if (classes != null)
			{
				copy.classes = new Dictionary<string, byte[]>(classes);
			}
			if (resources != null)
			{
				copy.resources = new Dictionary<string, byte[]>(resources);
			}
			if (props != null)
			{
				copy.props = new Dictionary<string, string>(props);
			}
			if (externalResources != null)
			{
				copy.externalResources = new Dictionary<string, string>(externalResources);
			}
			return copy;
		}
	}

	enum Message
	{
		// These are the informational messages
		MainMethodFound = 1,
		OutputFileIs = 2,
		AutoAddRef = 3,
		MainMethodFromManifest = 4,
		// This is were the warnings start
		StartWarnings = 100,
		ClassNotFound = 100,
		ClassFormatError = 101,
		DuplicateClassName = 102,
		IllegalAccessError = 103,
		VerificationError = 104,
		NoClassDefFoundError = 105,
		GenericUnableToCompileError = 106,
		DuplicateResourceName = 107,
		NotAClassFile = 108,
		SkippingReferencedClass = 109,
		NoJniRuntime= 110,
	}

	class StaticCompiler
	{
		internal static Assembly runtimeAssembly;
		internal static Assembly runtimeJniAssembly;

		internal static Type GetType(string name)
		{
			return GetType(name, true);
		}

		internal static Type GetType(string name, bool throwOnError)
		{
			if(runtimeAssembly.GetType(name) != null)
			{
				return runtimeAssembly.GetType(name);
			}
			if(runtimeJniAssembly != null && runtimeJniAssembly.GetType(name) != null)
			{
				return runtimeJniAssembly.GetType(name);
			}
			foreach(Assembly asm in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
			{
				Type t = asm.GetType(name, false);
				if(t != null)
				{
					return t;
				}
			}
			// try mscorlib as well
			return typeof(object).Assembly.GetType(name, throwOnError);
		}

		private static Dictionary<string, string> suppressWarnings = new Dictionary<string, string>();
		private static Dictionary<string, string> errorWarnings = new Dictionary<string, string>();

		internal static void SuppressWarning(string key)
		{
			suppressWarnings[key] = key;
		}

		internal static void WarnAsError(string key)
		{
			errorWarnings[key] = key;
		}

		internal static void IssueMessage(Message msgId, params string[] values)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append((int)msgId);
			if(values.Length > 0)
			{
				sb.Append(':').Append(values[0]);
			}
			string key = sb.ToString();
			if(suppressWarnings.ContainsKey(key)
				|| suppressWarnings.ContainsKey(((int)msgId).ToString()))
			{
				return;
			}
			suppressWarnings.Add(key, key);
			string msg;
			switch(msgId)
			{
				case Message.MainMethodFound:
					msg = "found main method in class \"{0}\"";
					break;
				case Message.OutputFileIs:
					msg = "output file is \"{0}\"";
					break;
				case Message.AutoAddRef:
					msg = "automatically adding reference to \"{0}\"";
					break;
				case Message.MainMethodFromManifest:
					msg = "using main class \"{0}\" based on jar manifest";
					break;
				case Message.ClassNotFound:
					msg = "class \"{0}\" not found";
					break;
				case Message.ClassFormatError:
					msg = "unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (class format error \"{1}\")";
					break;
				case Message.DuplicateClassName:
					msg = "duplicate class name: \"{0}\"";
					break;
				case Message.IllegalAccessError:
					msg = "unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (illegal access error \"{1}\")";
					break;
				case Message.VerificationError:
					msg = "unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (verification error \"{1}\")";
					break;
				case Message.NoClassDefFoundError:
					msg = "unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (missing class \"{1}\")";
					break;
				case Message.GenericUnableToCompileError:
					msg = "unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (\"{1}\": \"{2}\")";
					break;
				case Message.DuplicateResourceName:
					msg = "skipping resource (name clash): \"{0}\"";
					break;
				case Message.NotAClassFile:
					msg = "not a class file \"{0}\", including it as resource" + Environment.NewLine +
						"    (class format error \"{1}\")";
					break;
				case Message.SkippingReferencedClass:
					msg = "skipping class: \"{0}\"" + Environment.NewLine +
						"    (class is already available in referenced assembly \"{1}\")";
					break;
				case Message.NoJniRuntime:
					msg = "unable to load runtime JNI assembly";
					break;
				default:
					throw new InvalidProgramException();
			}
			if(errorWarnings.ContainsKey(key)
				|| errorWarnings.ContainsKey(((int)msgId).ToString()))
			{
				Console.Error.Write("{0} IKVMC{1:D4}: ", "Error", (int)msgId);
				Console.Error.WriteLine(msg, values);
				Environment.Exit(1);
			}
			Console.Error.Write("{0} IKVMC{1:D4}: ", msgId < Message.StartWarnings ? "Note" : "Warning", (int)msgId);
			Console.Error.WriteLine(msg, values);
		}

		internal static void LinkageError(string msg, TypeWrapper actualType, TypeWrapper expectedType, params object[] values)
		{
			object[] args = new object[values.Length + 2];
			values.CopyTo(args, 2);
			args[0] = AssemblyQualifiedName(actualType);
			args[1] = AssemblyQualifiedName(expectedType);
			Console.Error.WriteLine("Link Error: " + msg, args);
			Environment.Exit(1);
		}

		private static string AssemblyQualifiedName(TypeWrapper tw)
		{
			ClassLoaderWrapper loader = tw.GetClassLoader();
			AssemblyClassLoader acl = loader as AssemblyClassLoader;
			if(acl != null)
			{
				return tw.Name + ", " + acl.Assembly.FullName;
			}
			CompilerClassLoader ccl = loader as CompilerClassLoader;
			if(ccl != null)
			{
				return tw.Name + ", " + ccl.GetTypeWrapperFactory().ModuleBuilder.Assembly.FullName;
			}
			return tw.Name + " (unknown assembly)";
		}
	}
}
