/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Security.Permissions;
using System.Security;
using System.Runtime.CompilerServices;
using ICSharpCode.SharpZipLib.Zip;
using IKVM.Attributes;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;

namespace IKVM.Internal
{
	sealed class CompilerClassLoader : ClassLoaderWrapper
	{
		private Dictionary<string, Jar.Item> classes;
		private Dictionary<string, RemapperTypeWrapper> remapped = new Dictionary<string, RemapperTypeWrapper>();
		private string assemblyName;
		private string assemblyFile;
		private string assemblyDir;
		private bool targetIsModule;
		private AssemblyBuilder assemblyBuilder;
		private IKVM.Internal.MapXml.Attribute[] assemblyAttributes;
		private CompilerOptions options;
		private AssemblyClassLoader[] referencedAssemblies;
		private Dictionary<string, string> nameMappings = new Dictionary<string, string>();
		private Packages packages;
		private Dictionary<string, List<TypeWrapper>> ghosts;
		private TypeWrapper[] mappedExceptions;
		private bool[] mappedExceptionsAllSubClasses;
		private Dictionary<string, IKVM.Internal.MapXml.Class> mapxml_Classes;
		private Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList> mapxml_MethodBodies;
		private Dictionary<MethodKey, IKVM.Internal.MapXml.ReplaceMethodCall[]> mapxml_ReplacedMethods;
		private Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList> mapxml_MethodPrologues;
		private IKVM.Internal.MapXml.Root map;
		private List<string> classesToCompile;
		private List<CompilerClassLoader> peerReferences = new List<CompilerClassLoader>();
		private Dictionary<string, string> peerLoading = new Dictionary<string, string>();
		private List<ClassLoaderWrapper> internalsVisibleTo = new List<ClassLoaderWrapper>();
		private List<TypeWrapper> dynamicallyImportedTypes = new List<TypeWrapper>();
		private List<string> jarList = new List<string>();
		private List<TypeWrapper> allwrappers;
		private bool compilingCoreAssembly;

		internal CompilerClassLoader(AssemblyClassLoader[] referencedAssemblies, CompilerOptions options, FileInfo assemblyPath, bool targetIsModule, string assemblyName, Dictionary<string, Jar.Item> classes, bool compilingCoreAssembly)
			: base(options.codegenoptions, null)
		{
			this.referencedAssemblies = referencedAssemblies;
			this.options = options;
			this.classes = classes;
			this.assemblyName = assemblyName;
			this.assemblyFile = assemblyPath.Name;
			this.assemblyDir = assemblyPath.DirectoryName;
			this.targetIsModule = targetIsModule;
			this.compilingCoreAssembly = compilingCoreAssembly;
			Tracer.Info(Tracer.Compiler, "Instantiate CompilerClassLoader for {0}", assemblyName);
		}

		internal bool ReserveName(string javaName)
		{
			return !classes.ContainsKey(javaName) && GetTypeWrapperFactory().ReserveName(javaName);
		}

		internal void AddNameMapping(string javaName, string typeName)
		{
			nameMappings.Add(javaName, typeName);
		}

		internal void AddReference(AssemblyClassLoader acl)
		{
			referencedAssemblies = ArrayUtil.Concat(referencedAssemblies, acl);
		}

		internal void AddReference(CompilerClassLoader ccl)
		{
			peerReferences.Add(ccl);
		}

		internal AssemblyName GetAssemblyName()
		{
			return assemblyBuilder.GetName();
		}

		private static PermissionSet Combine(PermissionSet p1, PermissionSet p2)
		{
			if (p1 == null)
			{
				return p2;
			}
			if (p2 == null)
			{
				return p1;
			}
			return p1.Union(p2);
		}

		internal ModuleBuilder CreateModuleBuilder()
		{
			AssemblyName name = new AssemblyName();
			name.Name = assemblyName;
			if (options.keyPair != null)
			{
				name.KeyPair = options.keyPair;
			}
			else if (options.publicKey != null)
			{
				name.SetPublicKey(options.publicKey);
			}
			name.Version = options.version;
			assemblyBuilder = 
				StaticCompiler.Universe
					.DefineDynamicAssembly(name, AssemblyBuilderAccess.ReflectionOnly, assemblyDir);
			ModuleBuilder moduleBuilder;
			moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyFile, this.EmitDebugInfo);
			if(this.EmitStackTraceInfo)
			{
				AttributeHelper.SetSourceFile(moduleBuilder, null);
			}
			if(this.EmitDebugInfo || this.EmitStackTraceInfo)
			{
				CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(JVM.Import(typeof(DebuggableAttribute)).GetConstructor(new Type[] { Types.Boolean, Types.Boolean }), new object[] { true, this.EmitDebugInfo });
				assemblyBuilder.SetCustomAttribute(debugAttr);
			}
			AttributeHelper.SetRuntimeCompatibilityAttribute(assemblyBuilder);
			if(options.baseAddress != 0)
			{
				moduleBuilder.__ImageBase = options.baseAddress;
			}
			if(options.fileAlignment != 0)
			{
				moduleBuilder.__FileAlignment = options.fileAlignment;
			}
			if(options.highentropyva)
			{
				moduleBuilder.__DllCharacteristics |= DllCharacteristics.HighEntropyVA;
			}
			// allow the runtime to "inject" dynamic classes into the assembly
			string mainAssemblyName = options.sharedclassloader != null && options.sharedclassloader[0] != this
				? options.sharedclassloader[0].assemblyName
				: assemblyName;
			if(!DisableDynamicBinding)
			{
				AttributeHelper.SetInternalsVisibleToAttribute(assemblyBuilder, mainAssemblyName + DynamicClassLoader.DynamicAssemblySuffixAndPublicKey);
			}
			return moduleBuilder;
		}

		public override string ToString()
		{
			return "CompilerClassLoader:" + options.assembly;
		}

		protected override TypeWrapper LoadClassImpl(string name, LoadMode mode)
		{
			foreach(AssemblyClassLoader acl in referencedAssemblies)
			{
				TypeWrapper tw = acl.DoLoad(name);
				if(tw != null)
				{
					return tw;
				}
			}
			if(!peerLoading.ContainsKey(name))
			{
				peerLoading.Add(name, null);
				try
				{
					foreach(CompilerClassLoader ccl in peerReferences)
					{
						TypeWrapper tw = ccl.PeerLoad(name);
						if(tw != null)
						{
							return tw;
						}
					}
					if(options.sharedclassloader != null && options.sharedclassloader[0] != this)
					{
						TypeWrapper tw = options.sharedclassloader[0].PeerLoad(name);
						if(tw != null)
						{
							return tw;
						}
					}
				}
				finally
				{
					peerLoading.Remove(name);
				}
			}
			TypeWrapper tw1 = GetTypeWrapperCompilerHook(name);
			if(tw1 != null)
			{
				return tw1;
			}
			// HACK the peer loading mess above may have indirectly loaded the classes without returning it,
			// so we try once more here
			tw1 = FindLoadedClass(name);
			if(tw1 != null)
			{
				return tw1;
			}
			return FindOrLoadGenericClass(name, mode);
		}

		private TypeWrapper PeerLoad(string name)
		{
			// To keep the performance acceptable in cases where we're compiling many targets, we first check if the load can
			// possibly succeed on this class loader, otherwise we'll end up doing a lot of futile recursive loading attempts.
			if(classes.ContainsKey(name) || remapped.ContainsKey(name) || FindLoadedClass(name) != null)
			{
				TypeWrapper tw = LoadClassByDottedNameFast(name);
				// HACK we don't want to load classes referenced by peers, hence the "== this" check
				if(tw != null && tw.GetClassLoader() == this)
				{
					return tw;
				}
			}
			if(options.sharedclassloader != null && options.sharedclassloader[0] == this)
			{
				foreach(CompilerClassLoader ccl in options.sharedclassloader)
				{
					if(ccl != this)
					{
						TypeWrapper tw = ccl.PeerLoad(name);
						if(tw != null)
						{
							return tw;
						}
					}
				}
			}
			return null;
		}

		private TypeWrapper GetTypeWrapperCompilerHook(string name)
		{
			RemapperTypeWrapper rtw;
			if(remapped.TryGetValue(name, out rtw))
			{
				return rtw;
			}
			else
			{
				Jar.Item itemRef;
				if(classes.TryGetValue(name, out itemRef))
				{
					classes.Remove(name);
					ClassFile f;
					try
					{
						byte[] buf = itemRef.GetData();
						f = new ClassFile(buf, 0, buf.Length, name, ClassFileParseOptions, null);
					}
					catch(ClassFormatError x)
					{
						StaticCompiler.SuppressWarning(options, Message.ClassNotFound, name);
						StaticCompiler.IssueMessage(options, Message.ClassFormatError, name, x.Message);
						return null;
					}
					if(f.Name != name)
					{
						StaticCompiler.SuppressWarning(options, Message.ClassNotFound, name);
						StaticCompiler.IssueMessage(options, Message.WrongClassName, name, f.Name);
						return null;
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
					if(f.IsPublic && options.publicPackages != null)
					{
						bool found = false;
						foreach(string package in options.publicPackages)
						{
							if(f.Name.StartsWith(package))
							{
								found = true;
								break;
							}
						}
						if(!found)
						{
							f.SetInternal();
						}
					}
					if(f.SourceFileAttribute != null)
					{
						FileInfo path = itemRef.Path;
						if(path != null)
						{
							string sourceFile = Path.GetFullPath(Path.Combine(path.DirectoryName, f.SourceFileAttribute));
							if(File.Exists(sourceFile))
							{
								f.SourcePath = sourceFile;
							}
						}
						if(f.SourcePath == null)
						{
							if (options.sourcepath != null)
							{
								string package = f.Name;
								int index = package.LastIndexOf('.');
								package = index == -1 ? "" : package.Substring(0, index).Replace('.', '/');
								f.SourcePath = Path.GetFullPath(Path.Combine(options.sourcepath + "/" + package, f.SourceFileAttribute));
							}
							else
							{
								f.SourcePath = f.SourceFileAttribute;
							}
						}
					}
					try
					{
						TypeWrapper tw = DefineClass(f, null);
						// we successfully created the type, so we don't need to include the class as a resource
						if (options.nojarstubs)
						{
							itemRef.Remove();
						}
						else
						{
							itemRef.MarkAsStub();
						}
						int pos = f.Name.LastIndexOf('.');
						if (pos != -1)
						{
							string manifestJar = options.IsClassesJar(itemRef.Jar) ? null : itemRef.Jar.Name;
							packages.DefinePackage(f.Name.Substring(0, pos), manifestJar);
						}
						return tw;
					}
					catch (ClassFormatError x)
					{
						StaticCompiler.IssueMessage(options, Message.ClassFormatError, name, x.Message);
					}
					catch (IllegalAccessError x)
					{
						StaticCompiler.IssueMessage(options, Message.IllegalAccessError, name, x.Message);
					}
					catch (VerifyError x)
					{
						StaticCompiler.IssueMessage(options, Message.VerificationError, name, x.Message);
					}
					catch (NoClassDefFoundError x)
					{
						if ((options.codegenoptions & CodeGenOptions.DisableDynamicBinding) != 0)
						{
							StaticCompiler.IssueMessage(options, Message.NoClassDefFoundError, name, x.Message);
						}
						StaticCompiler.IssueMessage(options, Message.ClassNotFound, x.Message);
					}
					catch (RetargetableJavaException x)
					{
						StaticCompiler.IssueMessage(options, Message.GenericUnableToCompileError, name, x.GetType().Name, x.Message);
					}
					StaticCompiler.SuppressWarning(options, Message.ClassNotFound, name);
					return null;
				}
				else
				{
					return null;
				}
			}
		}

		// HACK when we're compiling multiple targets with -sharedclassloader, each target will have its own CompilerClassLoader,
		// so we need to consider them equivalent (because they represent the same class loader).
		internal bool IsEquivalentTo(ClassLoaderWrapper other)
		{
			if (this == other)
			{
				return true;
			}
			CompilerClassLoader ccl = other as CompilerClassLoader;
			if (ccl != null && options.sharedclassloader != null && options.sharedclassloader.Contains(ccl))
			{
				if (!internalsVisibleTo.Contains(ccl))
				{
					AddInternalsVisibleToAttribute(ccl);
				}
				return true;
			}
			return false;
		}

		internal override bool InternalsVisibleToImpl(TypeWrapper wrapper, TypeWrapper friend)
		{
			Debug.Assert(wrapper.GetClassLoader() == this);
			ClassLoaderWrapper other = friend.GetClassLoader();
			// TODO ideally we should also respect InternalsVisibleToAttribute.Annotation here
			if (this == other || internalsVisibleTo.Contains(other))
			{
				return true;
			}
			CompilerClassLoader ccl = other as CompilerClassLoader;
			if (ccl != null)
			{
				AddInternalsVisibleToAttribute(ccl);
				return true;
			}
			return false;
		}

		private void AddInternalsVisibleToAttribute(CompilerClassLoader ccl)
		{
			internalsVisibleTo.Add(ccl);
			AssemblyBuilder asm = ccl.assemblyBuilder;
			AssemblyName asmName = asm.GetName();
			string name = asmName.Name;
			byte[] pubkey = asmName.GetPublicKey();
			if (pubkey == null && asmName.KeyPair != null)
			{
				pubkey = asmName.KeyPair.PublicKey;
			}
			if (pubkey != null && pubkey.Length != 0)
			{
				StringBuilder sb = new StringBuilder(name);
				sb.Append(", PublicKey=");
				foreach (byte b in pubkey)
				{
					sb.AppendFormat("{0:X2}", b);
				}
				name = sb.ToString();
			}
			AttributeHelper.SetInternalsVisibleToAttribute(this.assemblyBuilder, name);
		}

		internal void SetMain(MethodInfo m, PEFileKinds target, Dictionary<string, string> props, bool noglobbing, Type apartmentAttributeType)
		{
			MethodBuilder mainStub = this.GetTypeWrapperFactory().ModuleBuilder.DefineGlobalMethod("main", MethodAttributes.Public | MethodAttributes.Static, Types.Int32, new Type[] { Types.String.MakeArrayType() });
			if(apartmentAttributeType != null)
			{
				mainStub.SetCustomAttribute(new CustomAttributeBuilder(apartmentAttributeType.GetConstructor(Type.EmptyTypes), new object[0]));
			}
			CodeEmitter ilgen = CodeEmitter.Create(mainStub);
			CodeEmitterLocal rc = ilgen.DeclareLocal(Types.Int32);
			TypeWrapper startupType = LoadClassByDottedName("ikvm.runtime.Startup");
			if(props.Count > 0)
			{
				ilgen.Emit(OpCodes.Newobj, JVM.Import(typeof(System.Collections.Generic.Dictionary<string, string>)).GetConstructor(Type.EmptyTypes));
				foreach(KeyValuePair<string, string> kv in props)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Ldstr, kv.Key);
					ilgen.Emit(OpCodes.Ldstr, kv.Value);
					if(kv.Value.IndexOf('%') < kv.Value.LastIndexOf('%'))
					{
						ilgen.Emit(OpCodes.Call, JVM.Import(typeof(Environment)).GetMethod("ExpandEnvironmentVariables", new Type[] { Types.String }));
					}
					ilgen.Emit(OpCodes.Callvirt, JVM.Import(typeof(System.Collections.Generic.Dictionary<string, string>)).GetMethod("Add"));
				}
				startupType.GetMethodWrapper("setProperties", "(Lcli.System.Collections.IDictionary;)V", false).EmitCall(ilgen);
			}
			ilgen.BeginExceptionBlock();
			startupType.GetMethodWrapper("enterMainThread", "()V", false).EmitCall(ilgen);
			ilgen.Emit(OpCodes.Ldarg_0);
			if (!noglobbing)
			{
				ilgen.Emit(OpCodes.Ldc_I4_0);
				startupType.GetMethodWrapper("glob", "([Ljava.lang.String;I)[Ljava.lang.String;", false).EmitCall(ilgen);
			}
			ilgen.Emit(OpCodes.Call, m);
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.EmitLeave(label);
			ilgen.BeginCatchBlock(Types.Exception);
			LoadClassByDottedName("ikvm.runtime.Util").GetMethodWrapper("mapException", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false).EmitCall(ilgen);
			CodeEmitterLocal exceptionLocal = ilgen.DeclareLocal(Types.Exception);
			ilgen.Emit(OpCodes.Stloc, exceptionLocal);
			TypeWrapper threadTypeWrapper = ClassLoaderWrapper.LoadClassCritical("java.lang.Thread");
			CodeEmitterLocal threadLocal = ilgen.DeclareLocal(threadTypeWrapper.TypeAsLocalOrStackType);
			threadTypeWrapper.GetMethodWrapper("currentThread", "()Ljava.lang.Thread;", false).EmitCall(ilgen);
			ilgen.Emit(OpCodes.Stloc, threadLocal);
			ilgen.Emit(OpCodes.Ldloc, threadLocal);
			threadTypeWrapper.GetMethodWrapper("getThreadGroup", "()Ljava.lang.ThreadGroup;", false).EmitCallvirt(ilgen);
			ilgen.Emit(OpCodes.Ldloc, threadLocal);
			ilgen.Emit(OpCodes.Ldloc, exceptionLocal);
			ClassLoaderWrapper.LoadClassCritical("java.lang.ThreadGroup").GetMethodWrapper("uncaughtException", "(Ljava.lang.Thread;Ljava.lang.Throwable;)V", false).EmitCallvirt(ilgen);
			ilgen.Emit(OpCodes.Ldc_I4_1);
			ilgen.Emit(OpCodes.Stloc, rc);
			ilgen.EmitLeave(label);
			ilgen.BeginFinallyBlock();
			startupType.GetMethodWrapper("exitMainThread", "()V", false).EmitCall(ilgen);
			ilgen.Emit(OpCodes.Endfinally);
			ilgen.EndExceptionBlock();
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Ldloc, rc);
			ilgen.Emit(OpCodes.Ret);
			ilgen.DoEmit();
			assemblyBuilder.SetEntryPoint(mainStub, target);
		}

		private void PrepareSave()
		{
			((DynamicClassLoader)this.GetTypeWrapperFactory()).FinishAll();
		}

		private void Save()
		{
			ModuleBuilder mb = GetTypeWrapperFactory().ModuleBuilder;
			if(targetIsModule)
			{
				// HACK force all referenced assemblies to end up as references in the assembly
				// (even if they are otherwise unused), to make sure that the assembly class loader
				// delegates to them at runtime.
				// NOTE now we only do this for modules, when we're an assembly we store the exported
				// assemblies in the ikvm.exports resource.
				for(int i = 0;i < referencedAssemblies.Length; i++)
				{
					Type[] types = referencedAssemblies[i].MainAssembly.GetExportedTypes();
					if(types.Length > 0)
					{
						mb.GetTypeToken(types[0]);
					}
				}
			}
			mb.CreateGlobalFunctions();

			AddJavaModuleAttribute(mb);

			// add a package list and export map
			if(options.sharedclassloader == null || options.sharedclassloader[0] == this)
			{
				ConstructorInfo packageListAttributeCtor = JVM.LoadType(typeof(PackageListAttribute)).GetConstructor(new Type[] { Types.String, Types.String.MakeArrayType() });
				foreach(object[] args in packages.ToArray())
				{
					args[1] = UnicodeUtil.EscapeInvalidSurrogates((string[])args[1]);
					mb.SetCustomAttribute(new CustomAttributeBuilder(packageListAttributeCtor, args));
				}
				// We can't add the resource when we're a module, because a multi-module assembly has a single resource namespace
				// and since you cannot combine -target:module with -sharedclassloader we don't need an export map
				// (the wildcard exports have already been added above, by making sure that we statically reference the assemblies).
				if(!targetIsModule)
				{
					WriteExportMap();
				}
			}

			if(targetIsModule)
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving {0} in {1}", assemblyFile, assemblyDir);
				try
				{
					GetTypeWrapperFactory().ModuleBuilder.__Save(options.pekind, options.imageFileMachine);
				}
				catch(IOException x)
				{
					throw new FatalCompilerErrorException(Message.ErrorWritingFile, GetTypeWrapperFactory().ModuleBuilder.FullyQualifiedName, x.Message);
				}
				catch(UnauthorizedAccessException x)
				{
					throw new FatalCompilerErrorException(Message.ErrorWritingFile, GetTypeWrapperFactory().ModuleBuilder.FullyQualifiedName, x.Message);
				}
			}
			else
			{
				Tracer.Info(Tracer.Compiler, "CompilerClassLoader saving {0} in {1}", assemblyFile, assemblyDir);
				try
				{
					assemblyBuilder.Save(assemblyFile, options.pekind, options.imageFileMachine);
				}
				catch(IOException x)
				{
					throw new FatalCompilerErrorException(Message.ErrorWritingFile, Path.Combine(assemblyDir, assemblyFile), x.Message);
				}
				catch(UnauthorizedAccessException x)
				{
					throw new FatalCompilerErrorException(Message.ErrorWritingFile, Path.Combine(assemblyDir, assemblyFile), x.Message);
				}
			}
		}

		private void AddJavaModuleAttribute(ModuleBuilder mb)
		{
			Type typeofJavaModuleAttribute = JVM.LoadType(typeof(JavaModuleAttribute));
			PropertyInfo[] propInfos = new PropertyInfo[] {
				typeofJavaModuleAttribute.GetProperty("Jars")
			};
			object[] propValues = new object[] {
				jarList.ToArray()
			};
			if (nameMappings.Count > 0)
			{
				string[] list = new string[nameMappings.Count * 2];
				int i = 0;
				foreach (KeyValuePair<string, string> kv in nameMappings)
				{
					list[i++] = kv.Key;
					list[i++] = kv.Value;
				}
				list = UnicodeUtil.EscapeInvalidSurrogates(list);
				CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofJavaModuleAttribute.GetConstructor(new Type[] { JVM.Import(typeof(string[])) }), new object[] { list }, propInfos, propValues);
				mb.SetCustomAttribute(cab);
			}
			else
			{
				CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofJavaModuleAttribute.GetConstructor(Type.EmptyTypes), new object[0], propInfos, propValues);
				mb.SetCustomAttribute(cab);
			}
		}

		private static void AddExportMapEntry(Dictionary<string, List<string>> map, CompilerClassLoader ccl, string name)
		{
			string assemblyName = ccl.assemblyBuilder.FullName;
			List<string> list;
			if (!map.TryGetValue(assemblyName, out list))
			{
				list = new List<string>();
				map.Add(assemblyName, list);
			}
			if (list != null) // if list is null, we already have a wildcard export for this assembly
			{
				list.Add(name);
			}
		}

		private void AddWildcardExports(Dictionary<string, List<string>> exportedNamesPerAssembly)
		{
			foreach (AssemblyClassLoader acl in referencedAssemblies)
			{
				exportedNamesPerAssembly[acl.MainAssembly.FullName] = null;
			}
		}

		private void WriteExportMap()
		{
			Dictionary<string, List<string>> exportedNamesPerAssembly = new Dictionary<string, List<string>>();
			AddWildcardExports(exportedNamesPerAssembly);
			foreach (TypeWrapper tw in dynamicallyImportedTypes)
			{
				AddExportMapEntry(exportedNamesPerAssembly, (CompilerClassLoader)tw.GetClassLoader(), tw.Name);
			}
			if (options.sharedclassloader == null)
			{
				foreach (CompilerClassLoader ccl in peerReferences)
				{
					exportedNamesPerAssembly[ccl.assemblyBuilder.FullName] = null;
				}
			}
			else
			{
				foreach (CompilerClassLoader ccl in options.sharedclassloader)
				{
					if (ccl != this)
					{
						ccl.AddWildcardExports(exportedNamesPerAssembly);
						foreach (Jar jar in ccl.options.jars)
						{
							foreach (Jar.Item item in jar)
							{
								if (!item.IsStub)
								{
									AddExportMapEntry(exportedNamesPerAssembly, ccl, item.Name);
								}
							}
						}
						if (ccl.options.externalResources != null)
						{
							foreach (string name in ccl.options.externalResources.Keys)
							{
								AddExportMapEntry(exportedNamesPerAssembly, ccl, name);
							}
						}
					}
				}
			}
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);
			bw.Write(exportedNamesPerAssembly.Count);
			foreach (KeyValuePair<string, List<string>> kv in exportedNamesPerAssembly)
			{
				bw.Write(kv.Key);
				if (kv.Value == null)
				{
					// wildcard export
					bw.Write(0);
				}
				else
				{
					Debug.Assert(kv.Value.Count != 0);
					bw.Write(kv.Value.Count);
					foreach (string name in kv.Value)
					{
						bw.Write(JVM.PersistableHash(name));
					}
				}
			}
			ms.Position = 0;
			this.GetTypeWrapperFactory().ModuleBuilder.DefineManifestResource("ikvm.exports", ms, ResourceAttributes.Public);
		}

		private void WriteResources()
		{
			Tracer.Info(Tracer.Compiler, "CompilerClassLoader adding resources...");

			// BUG we need to call GetTypeWrapperFactory() to make sure that the assemblyBuilder is created (when building an empty target)
			ModuleBuilder moduleBuilder = this.GetTypeWrapperFactory().ModuleBuilder;

			for (int i = 0; i < options.jars.Count; i++)
			{
				bool hasEntries = false;
				MemoryStream mem = new MemoryStream();
				using (ZipOutputStream zip = new ZipOutputStream(mem))
				{
					if (!string.IsNullOrEmpty(options.jars[i].Comment))
					{
						zip.SetComment(options.jars[i].Comment);
					}
					zip.SetLevel(9);
					List<string> stubs = new List<string>();
					foreach (Jar.Item item in options.jars[i])
					{
						if (item.IsStub)
						{
							// we don't want stub class pseudo resources for classes loaded from the file system
							if (i != options.classesJar)
							{
								stubs.Add(item.Name);
							}
							continue;
						}
						ZipEntry zipEntry = item.ZipEntry;
						if (options.compressedResources || zipEntry.CompressionMethod != CompressionMethod.Stored)
						{
							zipEntry.CompressionMethod = CompressionMethod.Deflated;
						}
						zip.PutNextEntry(zipEntry);
						byte[] data = item.GetData();
						zip.Write(data, 0, data.Length);
						zip.CloseEntry();
						hasEntries = true;
					}
					if (stubs.Count != 0)
					{
						// generate the --ikvm-classes-- file in the jar
						ZipEntry zipEntry = new ZipEntry(JVM.JarClassList);
						zipEntry.CompressionMethod = CompressionMethod.Deflated;
						zip.PutNextEntry(zipEntry);
						BinaryWriter bw = new BinaryWriter(zip);
						bw.Write(stubs.Count);
						foreach (string classFile in stubs)
						{
							bw.Write(classFile);
						}
						bw.Flush();
						zip.CloseEntry();
						hasEntries = true;
					}
				}
				// don't include empty classes.jar
				if (i != options.classesJar || hasEntries)
				{
					mem = new MemoryStream(mem.ToArray());
					string name = options.jars[i].Name;
					if (options.targetIsModule)
					{
						name = Path.GetFileNameWithoutExtension(name) + "-" + moduleBuilder.ModuleVersionId.ToString("N") + Path.GetExtension(name);
					}
					jarList.Add(name);
					moduleBuilder.DefineManifestResource(name, mem, ResourceAttributes.Public);
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

		private sealed class RemapperTypeWrapper : TypeWrapper
		{
			private CompilerClassLoader classLoader;
			private TypeBuilder typeBuilder;
			private TypeBuilder helperTypeBuilder;
			private Type shadowType;
			private IKVM.Internal.MapXml.Class classDef;
			private TypeWrapper baseTypeWrapper;
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
				: base(TypeFlags.None, (Modifiers)c.Modifiers, c.Name)
			{
				this.classLoader = classLoader;
				this.baseTypeWrapper = GetBaseWrapper(c);
				classDef = c;
				bool baseIsSealed = false;
				shadowType = StaticCompiler.Universe.GetType(c.Shadows, true);
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
				typeBuilder = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(name, attrs, baseIsSealed ? Types.Object : baseType);
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
					AttributeHelper.SetSourceFile(typeBuilder, classLoader.options.remapfile.Name);
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

			internal sealed override TypeWrapper BaseTypeWrapper
			{
				get { return baseTypeWrapper; }
			}

			internal void LoadInterfaces(IKVM.Internal.MapXml.Class c)
			{
				if (c.Interfaces != null)
				{
					interfaceWrappers = new TypeWrapper[c.Interfaces.Length];
					for (int i = 0; i < c.Interfaces.Length; i++)
					{
						interfaceWrappers[i] = classLoader.LoadClassByDottedName(c.Interfaces[i].Name);
					}
				}
				else
				{
					interfaceWrappers = TypeWrapper.EmptyArray;
				}
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
					ilgen.Emit(OpCodes.Call, GetMethod());
				}

				internal override void EmitNewobj(CodeEmitter ilgen)
				{
					if(mbHelper != null)
					{
						ilgen.Emit(OpCodes.Call, mbHelper);
					}
					else
					{
						ilgen.Emit(OpCodes.Newobj, GetMethod());
					}
				}

				internal override MethodBase DoLink()
				{
					MethodAttributes attr = MapMethodAccessModifiers(m.Modifiers);
					RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
					Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);

					MethodBuilder cbCore = null;

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
						cbCore = ReflectUtil.DefineConstructor(typeWrapper.typeBuilder, attr, paramTypes);
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

					MethodBuilder cbCore = GetMethod() as MethodBuilder;

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
								ilgen.EmitLdarg(i + 1);
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
						ilgen.DoEmit();
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
								ilgen.EmitLdarg(i);
							}
							ilgen.Emit(OpCodes.Newobj, baseCon);
							ilgen.Emit(OpCodes.Ret);
						}
						ilgen.DoEmit();
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
					if(!IsStatic && IsFinal)
					{
						// When calling a final instance method on a remapped type from a class derived from a .NET class (i.e. a cli.System.Object or cli.System.Exception derived base class)
						// then we can't call the java.lang.Object or java.lang.Throwable methods and we have to go through the instancehelper_ method. Note that since the method
						// is final, this won't affect the semantics.
						EmitCallvirt(ilgen);
					}
					else
					{
						ilgen.Emit(OpCodes.Call, (MethodInfo)GetMethod());
					}
				}

				internal override void EmitCallvirt(CodeEmitter ilgen)
				{
					EmitCallvirtImpl(ilgen, this.IsProtected && !mbHelper.IsPublic);
				}

				private void EmitCallvirtImpl(CodeEmitter ilgen, bool cloneOrFinalizeHack)
				{
					if(mbHelper != null && !cloneOrFinalizeHack)
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
						string[] throws;
						if (m.throws == null)
						{
							throws = new string[0];
						}
						else
						{
							throws = new string[m.throws.Length];
							for (int i = 0; i < throws.Length; i++)
							{
								throws[i] = m.throws[i].Class;
							}
						}
						AttributeHelper.SetRemappedInterfaceMethod(typeWrapper.typeBuilder, m.Name, m.@override.Name, throws);
						MethodBuilder helper = null;
						if(specialCases != null)
						{
							CodeEmitter ilgen;
							Type[] argTypes = ArrayUtil.Concat(typeWrapper.shadowType, typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig));
							if(typeWrapper.helperTypeBuilder == null)
							{
								typeWrapper.helperTypeBuilder = typeWrapper.typeBuilder.DefineNestedType("__Helper", TypeAttributes.NestedPublic | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract);
								AttributeHelper.HideFromJava(typeWrapper.helperTypeBuilder);
							}
							helper = typeWrapper.helperTypeBuilder.DefineMethod(m.Name, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig, LoadMode.LoadOrThrow).TypeAsSignatureType, argTypes);
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
								ilgen.EmitBrfalse(label);
								for(int i = 1; i < argTypes.Length; i++)
								{
									ilgen.EmitLdarg(i);
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
								ilgen.EmitLdarg(i);
							}
							ilgen.Emit(OpCodes.Callvirt, interfaceMethod);
							ilgen.Emit(OpCodes.Ret);
							ilgen.DoEmit();
						}
						mbHelper = helper;
						return interfaceMethod;
					}
					else
					{
						MethodBuilder mbCore = null;
						Type[] paramTypes = typeWrapper.GetClassLoader().ArgTypeListFromSig(m.Sig);
						Type retType = typeWrapper.GetClassLoader().RetTypeWrapperFromSig(m.Sig, LoadMode.LoadOrThrow).TypeAsSignatureType;

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
							MethodAttributes attr = m.MethodAttributes | MapMethodAccessModifiers(m.Modifiers) | MethodAttributes.HideBySig;
							if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) != 0)
							{
								attr |= MethodAttributes.Static;
							}
							else if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Private) == 0 && (m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Final) == 0)
							{
								attr |= MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
								if(!typeWrapper.shadowType.IsSealed)
								{
									MethodInfo autoOverride = typeWrapper.shadowType.GetMethod(m.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
									if(autoOverride != null && autoOverride.ReturnType == retType && !autoOverride.IsFinal)
									{
										// the method we're processing is overriding a method in its shadowType (which is the actual base type)
										attr &= ~MethodAttributes.NewSlot;
									}
								}
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
							mbCore = GetDefineMethodHelper().DefineMethod(DeclaringType.GetClassLoader().GetTypeWrapperFactory(), typeWrapper.typeBuilder, m.Name, attr);
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

						if((m.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) == 0 && !IsHideFromJava(m))
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
							mbHelper = typeWrapper.typeBuilder.DefineMethod("instancehelper_" + m.Name, attr, CallingConventions.Standard, retType, ArrayUtil.Concat(typeWrapper.shadowType, paramTypes));
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
							mbHelper.SetCustomAttribute(new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(new Type[] { Types.String }), new object[] { "This function will be removed from future versions. Please use extension methods from ikvm.extensions namespace instead." }));
						}
						return mbCore;
					}
				}

				private static bool IsHideFromJava(IKVM.Internal.MapXml.Method m)
				{
					if (m.Attributes != null)
					{
						foreach (MapXml.Attribute attr in m.Attributes)
						{
							if (attr.Type == "IKVM.Attributes.HideFromJavaAttribute")
							{
								return true;
							}
						}
					}
					return m.Name.StartsWith("__<", StringComparison.Ordinal);
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
								ilgen.EmitLdarg(i + thisOffset);
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
						ilgen.DoEmit();
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
							ilgen.EmitNullCheck();
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
							ilgen.EmitBrfalse(skip);
							for(int i = 0; i < paramTypes.Length; i++)
							{
								ilgen.EmitLdarg(i + 1);
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
								ilgen.EmitBrfalse(skip);
								for(int i = 0; i < paramTypes.Length; i++)
								{
									ilgen.EmitLdarg(i + 1);
								}
								mw.Link();
								mw.EmitCallvirtImpl(ilgen, false);
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
								ilgen.EmitLdarg(i);
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
						ilgen.DoEmit();
						if(this.DeclaringType.GetClassLoader().EmitStackTraceInfo)
						{
							ilgen.EmitLineNumberTable(mbHelper);
						}
					}

					// do we need a helper for non-virtual reflection invocation?
					if(m.nonvirtualAlternateBody != null || (m.@override != null && overriders.Count > 0))
					{
						RemapperTypeWrapper typeWrapper = (RemapperTypeWrapper)DeclaringType;
						MethodBuilder mb = typeWrapper.typeBuilder.DefineMethod("nonvirtualhelper/" + this.Name, MethodAttributes.Private | MethodAttributes.Static,
							ReturnTypeForDefineMethod, ArrayUtil.Concat(typeWrapper.TypeAsSignatureType, GetParametersForDefineMethod()));
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
								ilgen.EmitLdarg(i + 1);
							}
							ilgen.Emit(OpCodes.Call, baseMethod);
							ilgen.Emit(OpCodes.Ret);
						}
						ilgen.DoEmit();
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
						Type type = m.redirect.Class == null ? baseType : StaticCompiler.Universe.GetType(m.redirect.Class, true);
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

			internal void Process2ndPassStep1()
			{
				if (!shadowType.IsSealed)
				{
					foreach (TypeWrapper ifaceTypeWrapper in interfaceWrappers)
					{
						typeBuilder.AddInterfaceImplementation(ifaceTypeWrapper.TypeAsBaseType);
					}
				}
				AttributeHelper.SetImplementsAttribute(typeBuilder, interfaceWrappers);
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
						{
							FieldAttributes attr = MapFieldAccessModifiers(f.Modifiers);
							if(f.Constant != null)
							{
								attr |= FieldAttributes.Literal;
							}
							else if((f.Modifiers & IKVM.Internal.MapXml.MapModifiers.Final) != 0)
							{
								attr |= FieldAttributes.InitOnly;
							}
							if((f.Modifiers & IKVM.Internal.MapXml.MapModifiers.Static) != 0)
							{
								attr |= FieldAttributes.Static;
							}
							FieldBuilder fb = tb.DefineField(f.Name, GetClassLoader().FieldTypeWrapperFromSig(f.Sig, LoadMode.LoadOrThrow).TypeAsSignatureType, attr);
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
								fields.Add(new ConstantFieldWrapper(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig, LoadMode.LoadOrThrow), f.Name, f.Sig, (Modifiers)f.Modifiers, fb, constant, MemberFlags.None));
							}
							else
							{
								fields.Add(FieldWrapper.Create(this, GetClassLoader().FieldTypeWrapperFromSig(f.Sig, LoadMode.LoadOrThrow), fb, f.Name, f.Sig, new ExModifiers((Modifiers)f.Modifiers, false)));
							}
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

			internal void Process4thPass(ICollection<RemapperTypeWrapper> remappedTypes)
			{
				foreach(RemappedMethodBaseWrapper m in GetMethods())
				{
					m.Finish();
				}

				if(classDef.Clinit != null)
				{
					MethodBuilder cb = ReflectUtil.DefineTypeInitializer(typeBuilder, classLoader);
					CodeEmitter ilgen = CodeEmitter.Create(cb);
					// TODO emit code to make sure super class is initialized
					classDef.Clinit.body.Emit(classLoader, ilgen);
					ilgen.DoEmit();
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
					foreach(MethodWrapper mw in GetMethods())
					{
						MethodBuilder mb = mw.GetMethod() as MethodBuilder;
						if(mb != null)
						{
							methods.Add(MakeMethodKey(mb), mb);
						}
					}
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
								ilgen.EmitLdarg(i);
							}
							if(!mi.IsStatic)
							{
								ilgen.EmitLdarg(paramTypes.Length);
								ilgen.Emit(OpCodes.Callvirt, mi);
							}
							else
							{
								ilgen.Emit(OpCodes.Call, mi);
							}
							ilgen.Emit(OpCodes.Ret);
							ilgen.DoEmit();
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
						if(pi.GetGetMethod() != null)
						{
							pb.SetGetMethod(methods[MakeMethodKey(pi.GetGetMethod())]);
						}
						if(pi.GetSetMethod() != null)
						{
							pb.SetSetMethod(methods[MakeMethodKey(pi.GetSetMethod())]);
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
				foreach (CustomAttributeData cad in CustomAttributeData.__GetDeclarativeSecurity(mi))
				{
					if (cad.ConstructorArguments.Count == 0 || (int)cad.ConstructorArguments[0].Value == (int)SecurityAction.LinkDemand)
					{
						mb.__AddDeclarativeSecurity(cad.__ToBuilder());
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

			private void CreateShadowInstanceOf(ICollection<RemapperTypeWrapper> remappedTypes)
			{
				// FXBUG .NET 1.1 doesn't allow static methods on interfaces
				if(typeBuilder.IsInterface)
				{
					return;
				}
				MethodAttributes attr = MethodAttributes.SpecialName | MethodAttributes.Public | MethodAttributes.Static;
				MethodBuilder mb = typeBuilder.DefineMethod("__<instanceof>", attr, Types.Boolean, new Type[] { Types.Object });
				AttributeHelper.HideFromJava(mb);
				AttributeHelper.SetEditorBrowsableNever(mb);
				CodeEmitter ilgen = CodeEmitter.Create(mb);

				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.Emit(OpCodes.Isinst, shadowType);
				CodeEmitterLabel retFalse = ilgen.DefineLabel();
				ilgen.EmitBrfalse(retFalse);

				if(!shadowType.IsSealed)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Isinst, typeBuilder);
					ilgen.EmitBrtrue(retFalse);
				}

				if(shadowType == Types.Object)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Isinst, Types.Array);
					ilgen.EmitBrtrue(retFalse);
				}

				foreach(RemapperTypeWrapper r in remappedTypes)
				{
					if(!r.shadowType.IsInterface && r.shadowType.IsSubclassOf(shadowType))
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, r.shadowType);
						ilgen.EmitBrtrue(retFalse);
					}
				}
				ilgen.Emit(OpCodes.Ldc_I4_1);
				ilgen.Emit(OpCodes.Ret);

				ilgen.MarkLabel(retFalse);
				ilgen.Emit(OpCodes.Ldc_I4_0);
				ilgen.Emit(OpCodes.Ret);

				ilgen.DoEmit();
			}

			private void CreateShadowCheckCast(ICollection<RemapperTypeWrapper> remappedTypes)
			{
				// FXBUG .NET 1.1 doesn't allow static methods on interfaces
				if(typeBuilder.IsInterface)
				{
					return;
				}
				MethodAttributes attr = MethodAttributes.SpecialName | MethodAttributes.Public | MethodAttributes.Static;
				MethodBuilder mb = typeBuilder.DefineMethod("__<checkcast>", attr, shadowType, new Type[] { Types.Object });
				AttributeHelper.HideFromJava(mb);
				AttributeHelper.SetEditorBrowsableNever(mb);
				CodeEmitter ilgen = CodeEmitter.Create(mb);

				CodeEmitterLabel fail = ilgen.DefineLabel();
				bool hasfail = false;

				if(!shadowType.IsSealed)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Isinst, typeBuilder);
					ilgen.EmitBrtrue(fail);
					hasfail = true;
				}

				if(shadowType == Types.Object)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Isinst, Types.Array);
					ilgen.EmitBrtrue(fail);
					hasfail = true;
				}

				foreach(RemapperTypeWrapper r in remappedTypes)
				{
					if(!r.shadowType.IsInterface && r.shadowType.IsSubclassOf(shadowType))
					{
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, r.shadowType);
						ilgen.EmitBrtrue(fail);
						hasfail = true;
					}
				}
				ilgen.Emit(OpCodes.Ldarg_0);
				ilgen.EmitCastclass(shadowType);
				ilgen.Emit(OpCodes.Ret);

				if(hasfail)
				{
					ilgen.MarkLabel(fail);
					ilgen.ThrowException(JVM.Import(typeof(InvalidCastException)));
				}

				ilgen.DoEmit();
			}

			internal override MethodBase LinkMethod(MethodWrapper mw)
			{
				return ((RemappedMethodBaseWrapper)mw).DoLink();
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

			internal override bool IsMapUnsafeException
			{
				get
				{
					// any remapped exceptions are automatically unsafe
					return shadowType == Types.Exception || shadowType.IsSubclassOf(Types.Exception);
				}
			}

			internal override bool IsFastClassLiteralSafe
			{
				get { return true; }
			}
		}

		internal static void AddDeclaredExceptions(MethodBuilder mb, IKVM.Internal.MapXml.Throws[] throws)
		{
			if (throws != null)
			{
				string[] exceptions = new string[throws.Length];
				for (int i = 0; i < exceptions.Length; i++)
				{
					exceptions[i] = throws[i].Class;
				}
				AttributeHelper.SetThrowsAttribute(mb, exceptions);
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
						if(classes.ContainsKey(c.Name))
						{
							StaticCompiler.IssueMessage(Message.DuplicateClassName, c.Name);
						}
						remapped.Add(c.Name, new RemapperTypeWrapper(this, c, map));
						hasRemappedTypes = true;
					}
				}

				if(hasRemappedTypes)
				{
					SetupGhosts(map);
					foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
					{
						if(c.Shadows != null)
						{
							remapped[c.Name].LoadInterfaces(c);
						}
					}
				}
			}
		}

		internal void EmitRemappedTypes2ndPass()
		{
			if (map != null && map.assembly != null && map.assembly.Classes != null)
			{
				// 2nd pass, resolve interfaces, publish methods/fields
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if(c.Shadows != null)
					{
						RemapperTypeWrapper typeWrapper = remapped[c.Name];
						typeWrapper.Process2ndPassStep1();
					}
				}
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if(c.Shadows != null)
					{
						RemapperTypeWrapper typeWrapper = remapped[c.Name];
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
				ilgen.Emit(OpCodes.Callvirt, Compiler.getTypeMethod);
				for(int i = 0; i < map.Length; i++)
				{
					ilgen.Emit(OpCodes.Dup);
					ilgen.Emit(OpCodes.Ldtoken, StaticCompiler.Universe.GetType(map[i].src, true));
					ilgen.Emit(OpCodes.Call, Compiler.getTypeFromHandleMethod);
					ilgen.Emit(OpCodes.Ceq);
					CodeEmitterLabel label = ilgen.DefineLabel();
					ilgen.EmitBrfalse(label);
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
				mapxml_MethodPrologues = new Dictionary<MethodKey, IKVM.Internal.MapXml.InstructionList>();
				foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					// if it is not a remapped type, it must be a container for native, patched or augmented methods
					if(c.Shadows == null)
					{
						string className = c.Name;
						mapxml_Classes.Add(className, c);
						AddMapXmlMethods(className, c.Constructors);
						AddMapXmlMethods(className, c.Methods);
						if (c.Clinit != null)
						{
							AddMapXmlMethod(className, c.Clinit);
						}
					}
				}
			}
		}

		private void AddMapXmlMethods(string className, IKVM.Internal.MapXml.MethodBase[] methods)
		{
			if(methods != null)
			{
				foreach(IKVM.Internal.MapXml.MethodBase method in methods)
				{
					AddMapXmlMethod(className, method);
				}
			}
		}

		private void AddMapXmlMethod(string className, IKVM.Internal.MapXml.MethodBase method)
		{
			if(method.body != null)
			{
				mapxml_MethodBodies.Add(method.ToMethodKey(className), method.body);
			}
			if(method.ReplaceMethodCalls != null)
			{
				mapxml_ReplacedMethods.Add(method.ToMethodKey(className), method.ReplaceMethodCalls);
			}
			if (method.prologue != null)
			{
				mapxml_MethodPrologues.Add(method.ToMethodKey(className), method.prologue);
			}
		}

		internal IKVM.Internal.MapXml.InstructionList GetMethodPrologue(MethodKey method)
		{
			if(mapxml_MethodPrologues == null)
			{
				return null;
			}
			IKVM.Internal.MapXml.InstructionList prologue;
			mapxml_MethodPrologues.TryGetValue(method, out prologue);
			return prologue;
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
					TypeWrapper typeWrapper = FindLoadedClass(c.Name);
					foreach(IKVM.Internal.MapXml.Interface iface in c.Interfaces)
					{
						TypeWrapper ifaceWrapper = FindLoadedClass(iface.Name);
						if(ifaceWrapper == null || !ifaceWrapper.TypeAsTBD.IsAssignableFrom(typeWrapper.TypeAsTBD))
						{
							AddGhost(iface.Name, typeWrapper);
						}
					}
				}
			}
			// we manually add the array ghost interfaces
			TypeWrapper array = ClassLoaderWrapper.GetWrapperFromType(Types.Array);
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

		internal static bool IsCoreAssembly(Assembly asm)
		{
			return asm.IsDefined(StaticCompiler.GetRuntimeType("IKVM.Attributes.RemappedClassAttribute"), false);
		}

		private bool CheckCompilingCoreAssembly()
		{
			if (map != null && map.assembly != null && map.assembly.Classes != null)
			{
				foreach (IKVM.Internal.MapXml.Class c in map.assembly.Classes)
				{
					if (c.Shadows != null && c.Name == "java.lang.Object")
					{
						return compilingCoreAssembly = true;
					}
				}
			}
			return false;
		}

		internal static int Compile(string runtimeAssembly, List<CompilerOptions> optionsList)
		{
			try
			{
				if(runtimeAssembly == null)
				{
					// we assume that the runtime is in the same directory as the compiler
					runtimeAssembly = Path.Combine(typeof(CompilerClassLoader).Assembly.Location, ".." + Path.DirectorySeparatorChar + "IKVM.Runtime.dll");
				}
				StaticCompiler.runtimeAssembly = StaticCompiler.LoadFile(runtimeAssembly);
				StaticCompiler.runtimeJniAssembly = StaticCompiler.LoadFile(Path.Combine(StaticCompiler.runtimeAssembly.Location, ".." + Path.DirectorySeparatorChar + "IKVM.Runtime.JNI.dll"));
			}
			catch(FileNotFoundException)
			{
				if(StaticCompiler.runtimeAssembly == null)
				{
					throw new FatalCompilerErrorException(Message.RuntimeNotFound);
				}
				StaticCompiler.IssueMessage(Message.NoJniRuntime);
			}
			Tracer.Info(Tracer.Compiler, "Loaded runtime assembly: {0}", StaticCompiler.runtimeAssembly.FullName);
			bool compilingCoreAssembly = false;
			List<CompilerClassLoader> compilers = new List<CompilerClassLoader>();
			foreach (CompilerOptions options in optionsList)
			{
				CompilerClassLoader compiler = null;
				int rc = CreateCompiler(options, ref compiler, ref compilingCoreAssembly);
				if(rc != 0)
				{
					return rc;
				}
				compilers.Add(compiler);
				if(options.sharedclassloader != null)
				{
					options.sharedclassloader.Add(compiler);
				}
			}
			foreach (CompilerClassLoader compiler1 in compilers)
			{
				foreach (CompilerClassLoader compiler2 in compilers)
				{
					if (compiler1 != compiler2
						&& (compiler1.options.crossReferenceAllPeers || (compiler1.options.peerReferences != null && Array.IndexOf(compiler1.options.peerReferences, compiler2.options.assembly) != -1)))
					{
						compiler1.AddReference(compiler2);
					}
				}
			}
			foreach (CompilerClassLoader compiler in compilers)
			{
				compiler.CompilePass0();
			}
			Dictionary<CompilerClassLoader, Type> mainAssemblyTypes = new Dictionary<CompilerClassLoader, Type>();
			foreach (CompilerClassLoader compiler in compilers)
			{
				if (compiler.options.sharedclassloader != null)
				{
					Type mainAssemblyType;
					if (!mainAssemblyTypes.TryGetValue(compiler.options.sharedclassloader[0], out mainAssemblyType))
					{
						TypeBuilder tb = compiler.options.sharedclassloader[0].GetTypeWrapperFactory().ModuleBuilder.DefineType("__<MainAssembly>", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.SpecialName);
						AttributeHelper.HideFromJava(tb);
						mainAssemblyType = tb.CreateType();
						mainAssemblyTypes.Add(compiler.options.sharedclassloader[0], mainAssemblyType);
					}
					if (compiler.options.sharedclassloader[0] != compiler)
					{
						((AssemblyBuilder)compiler.GetTypeWrapperFactory().ModuleBuilder.Assembly).__AddTypeForwarder(mainAssemblyType);
					}
				}
				compiler.CompilePass1();
			}
			foreach (CompilerClassLoader compiler in compilers)
			{
				compiler.CompilePass2();
			}
			if (compilingCoreAssembly)
			{
				RuntimeHelperTypes.Create(compilers[0]);
				foreach (CompilerClassLoader compiler in compilers)
				{
					compiler.EmitRemappedTypes2ndPass();
				}
			}
			foreach (CompilerClassLoader compiler in compilers)
			{
				int rc = compiler.CompilePass3();
				if (rc != 0)
				{
					return rc;
				}
			}
			Tracer.Info(Tracer.Compiler, "CompilerClassLoader.Save...");
			foreach (CompilerClassLoader compiler in compilers)
			{
				compiler.PrepareSave();
			}
			if (StaticCompiler.errorCount > 0)
			{
				return 1;
			}
			foreach (CompilerClassLoader compiler in compilers)
			{
				compiler.Save();
			}
			return StaticCompiler.errorCount == 0 ? 0 : 1;
		}

		private static int CreateCompiler(CompilerOptions options, ref CompilerClassLoader loader, ref bool compilingCoreAssembly)
		{
			Tracer.Info(Tracer.Compiler, "JVM.Compile path: {0}, assembly: {1}", options.path, options.assembly);
			AssemblyName runtimeAssemblyName = StaticCompiler.runtimeAssembly.GetName();
			bool allReferencesAreStrongNamed = IsSigned(StaticCompiler.runtimeAssembly);
			List<Assembly> references = new List<Assembly>();
			foreach(Assembly reference in options.references ?? new Assembly[0])
			{
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
							// TODO we really should support binding redirects here to allow different revisions to be mixed
							if(asmref.FullName != runtimeAssemblyName.FullName)
							{
								throw new FatalCompilerErrorException(Message.RuntimeMismatch, reference.Location, runtimeAssemblyName.FullName, asmref.FullName);
							}
						}
						else
						{
							if(asmref.GetPublicKeyToken() != null && asmref.GetPublicKeyToken().Length != 0)
							{
								throw new FatalCompilerErrorException(Message.RuntimeMismatch, reference.Location, runtimeAssemblyName.FullName, asmref.FullName);
							}
						}
					}
				}
			}
			Tracer.Info(Tracer.Compiler, "Parsing class files");
			// map the class names to jar entries
			Dictionary<string, Jar.Item> h = new Dictionary<string, Jar.Item>();
			List<string> classNames = new List<string>();
			foreach (Jar jar in options.jars)
			{
				if (options.IsResourcesJar(jar))
				{
					continue;
				}
				foreach (Jar.Item item in jar)
				{
					string name = item.Name;
					if (name.EndsWith(".class", StringComparison.Ordinal)
						&& name.Length > 6
						&& name.IndexOf('.') == name.Length - 6)
					{
						string className = name.Substring(0, name.Length - 6).Replace('/', '.');
						if (h.ContainsKey(className))
						{
							StaticCompiler.IssueMessage(Message.DuplicateClassName, className);
							Jar.Item itemRef = h[className];
							if ((options.classesJar != -1 && itemRef.Jar == options.jars[options.classesJar]) || jar != itemRef.Jar)
							{
								// the previous class stays, because it was either in an earlier jar or we're processing the classes.jar
								// which contains the classes loaded from the file system (where the first encountered class wins)
								continue;
							}
							else
							{
								// we have a jar that contains multiple entries with the same name, the last one wins
								h.Remove(className);
								classNames.Remove(className);
							}
						}
						h.Add(className, item);
						classNames.Add(className);
					}
				}
			}

			if (options.assemblyAttributeAnnotations == null)
			{
				// look for "assembly" type that acts as a placeholder for assembly attributes
				Jar.Item assemblyType;
				if (h.TryGetValue("assembly", out assemblyType))
				{
					try
					{
						byte[] buf = assemblyType.GetData();
						ClassFile f = new ClassFile(buf, 0, buf.Length, null, ClassFileParseOptions.None, null);
						// NOTE the "assembly" type in the unnamed package is a magic type
						// that acts as the placeholder for assembly attributes
						if (f.Name == "assembly" && f.Annotations != null)
						{
							options.assemblyAttributeAnnotations = f.Annotations;
							// HACK remove "assembly" type that exists only as a placeholder for assembly attributes
							h.Remove(f.Name);
							assemblyType.Remove();
							StaticCompiler.IssueMessage(Message.LegacyAssemblyAttributesFound);
						}
					}
					catch (ClassFormatError) { }
				}
			}

			// now look for a main method
			if (options.mainClass == null && (options.guessFileKind || options.target != PEFileKinds.Dll))
			{
				foreach (string className in classNames)
				{
					try
					{
						byte[] buf = h[className].GetData();
						ClassFile f = new ClassFile(buf, 0, buf.Length, null, ClassFileParseOptions.None, null);
						if (f.Name == className)
						{
							foreach (ClassFile.Method m in f.Methods)
							{
								if (m.IsPublic && m.IsStatic && m.Name == "main" && m.Signature == "([Ljava.lang.String;)V")
								{
									StaticCompiler.IssueMessage(Message.MainMethodFound, f.Name);
									options.mainClass = f.Name;
									goto break_outer;
								}
							}
						}
					}
					catch (ClassFormatError) { }
				}
			break_outer: ;
			}

			if(options.guessFileKind && options.mainClass == null)
			{
				options.target = PEFileKinds.Dll;
			}

			if(options.target == PEFileKinds.Dll && options.mainClass != null)
			{
				throw new FatalCompilerErrorException(Message.MainClassRequiresExe);
			}

			if(options.target != PEFileKinds.Dll && options.mainClass == null)
			{
				throw new FatalCompilerErrorException(Message.ExeRequiresMainClass);
			}

			if(options.target == PEFileKinds.Dll && options.props.Count != 0)
			{
				throw new FatalCompilerErrorException(Message.PropertiesRequireExe);
			}

			if(options.path == null)
			{
				if(options.target == PEFileKinds.Dll)
				{
					if(options.targetIsModule)
					{
						options.path = IkvmcCompiler.GetFileInfo(options.assembly + ".netmodule");
					}
					else
					{
						options.path = IkvmcCompiler.GetFileInfo(options.assembly + ".dll");
					}
				}
				else
				{
					options.path = IkvmcCompiler.GetFileInfo(options.assembly + ".exe");
				}
				StaticCompiler.IssueMessage(Message.OutputFileIs, options.path.ToString());
			}

			if(options.targetIsModule)
			{
				if(options.classLoader != null)
				{
					throw new FatalCompilerErrorException(Message.ModuleCannotHaveClassLoader);
				}
				// TODO if we're overwriting a user specified assembly name, we need to emit a warning
				options.assembly = options.path.Name;
			}

			Tracer.Info(Tracer.Compiler, "Constructing compiler");
			AssemblyClassLoader[] referencedAssemblies = new AssemblyClassLoader[references.Count];
			for(int i = 0; i < references.Count; i++)
			{
				AssemblyClassLoader acl = AssemblyClassLoader.FromAssembly(references[i]);
				if (Array.IndexOf(referencedAssemblies, acl) != -1)
				{
					StaticCompiler.IssueMessage(options, Message.DuplicateAssemblyReference, acl.MainAssembly.FullName);
				}
				referencedAssemblies[i] = acl;
			}
			loader = new CompilerClassLoader(referencedAssemblies, options, options.path, options.targetIsModule, options.assembly, h, compilingCoreAssembly);
			loader.classesToCompile = new List<string>(h.Keys);
			if(options.remapfile != null)
			{
				Tracer.Info(Tracer.Compiler, "Loading remapped types (1) from {0}", options.remapfile);
				System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(IKVM.Internal.MapXml.Root));
				ser.UnknownElement += new System.Xml.Serialization.XmlElementEventHandler(ser_UnknownElement);
				ser.UnknownAttribute += new System.Xml.Serialization.XmlAttributeEventHandler(ser_UnknownAttribute);
				FileStream fs;
				try
				{
					fs = options.remapfile.OpenRead();
				}
				catch(Exception x)
				{
					throw new FatalCompilerErrorException(Message.ErrorReadingFile, options.remapfile, x.Message);
				}
				try
				{
					XmlTextReader rdr = new XmlTextReader(fs);
					IKVM.Internal.MapXml.Root.xmlReader = rdr;
					IKVM.Internal.MapXml.Root map;
					try
					{
						map = (IKVM.Internal.MapXml.Root)ser.Deserialize(rdr);
					}
					catch(InvalidOperationException x)
					{
						throw new FatalCompilerErrorException(Message.ErrorParsingMapFile, options.remapfile, x.Message);
					}
					if(!loader.ValidateAndSetMap(map))
					{
						return 1;
					}
				}
				finally
				{
					fs.Close();
				}
				if(loader.CheckCompilingCoreAssembly())
				{
					compilingCoreAssembly = true;
					ClassLoaderWrapper.SetBootstrapClassLoader(loader);
				}
			}
			// If we do not yet have a reference to the core assembly and we are not compiling the core assembly,
			// try to find the core assembly by looking at the assemblies that the runtime references
			if(JVM.CoreAssembly == null && !compilingCoreAssembly)
			{
				foreach(AssemblyName name in StaticCompiler.runtimeAssembly.GetReferencedAssemblies())
				{
					Assembly asm = null;
					try
					{
						asm = LoadReferencedAssembly(StaticCompiler.runtimeAssembly.Location + "/../" + name.Name + ".dll");
					}
					catch(FileNotFoundException)
					{
					}
					if(asm != null && IsCoreAssembly(asm))
					{
						AssemblyClassLoader.PreloadExportedAssemblies(asm);
						JVM.CoreAssembly = asm;
						break;
					}
				}
				if(JVM.CoreAssembly == null)
				{
					throw new FatalCompilerErrorException(Message.BootstrapClassesMissing);
				}
				// we need to scan again for remapped types, now that we've loaded the core library
				ClassLoaderWrapper.LoadRemappedTypes();
			}

			if(!compilingCoreAssembly)
			{
				allReferencesAreStrongNamed &= IsSigned(JVM.CoreAssembly);
				loader.AddReference(AssemblyClassLoader.FromAssembly(JVM.CoreAssembly));
			}

			if((options.keyPair != null || options.publicKey != null) && !allReferencesAreStrongNamed)
			{
				throw new FatalCompilerErrorException(Message.StrongNameRequiresStrongNamedRefs);
			}

			if(loader.map != null)
			{
				loader.LoadMapXml();
			}

			if(!compilingCoreAssembly)
			{
				FakeTypes.Load(JVM.CoreAssembly);
			}
			return 0;
		}

		private static Assembly LoadReferencedAssembly(string r)
		{
			Assembly asm = StaticCompiler.LoadFile(r);
			return asm;
		}

		private void CompilePass0()
		{
			if(options.sharedclassloader != null && options.sharedclassloader[0] != this)
			{
				packages = options.sharedclassloader[0].packages;
			}
			else
			{
				packages = new Packages();
			}
		}

		private void CompilePass1()
		{
			Tracer.Info(Tracer.Compiler, "Compiling class files (1)");
			if(CheckCompilingCoreAssembly())
			{
				EmitRemappedTypes();
			}
			// if we're compiling the core class library, generate the "fake" generic types
			// that represent the not-really existing types (i.e. the Java enums that represent .NET enums,
			// the Method interface for delegates and the Annotation annotation for custom attributes)
			if(map != null && CheckCompilingCoreAssembly())
			{
				FakeTypes.Create(GetTypeWrapperFactory().ModuleBuilder, this);
			}
			allwrappers = new List<TypeWrapper>();
			foreach(string s in classesToCompile)
			{
				TypeWrapper wrapper = LoadClassByDottedNameFast(s);
				if(wrapper != null)
				{
					ClassLoaderWrapper loader = wrapper.GetClassLoader();
					if(loader != this)
					{
						if(loader is AssemblyClassLoader)
						{
							StaticCompiler.IssueMessage(options, Message.SkippingReferencedClass, s, ((AssemblyClassLoader)loader).GetAssembly(wrapper).FullName);
						}
						continue;
					}
					if(options.sharedclassloader != null && options.sharedclassloader[0] != this)
					{
						options.sharedclassloader[0].dynamicallyImportedTypes.Add(wrapper);
					}
					allwrappers.Add(wrapper);
				}
			}
		}

		private void CompilePass2()
		{
			Tracer.Info(Tracer.Compiler, "Compiling class files (2)");
			foreach(TypeWrapper tw in allwrappers)
			{
				DynamicTypeWrapper dtw = tw as DynamicTypeWrapper;
				if(dtw != null)
				{
					dtw.CreateStep2();
				}
			}
		}

		private int CompilePass3()
		{
			Tracer.Info(Tracer.Compiler, "Compiling class files (3)");
			if(map != null && CheckCompilingCoreAssembly())
			{
				FakeTypes.Finish(this);
			}
			foreach(string proxy in options.proxies)
			{
				ProxyGenerator.Create(this, proxy);
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
					throw new FatalCompilerErrorException(Message.MainClassNotFound);
				}
				MethodWrapper mw = wrapper.GetMethodWrapper("main", "([Ljava.lang.String;)V", false);
				if(mw == null || !mw.IsStatic)
				{
					throw new FatalCompilerErrorException(Message.MainMethodNotFound);
				}
				mw.Link();
				MethodInfo method = mw.GetMethod() as MethodInfo;
				if(method == null)
				{
					throw new FatalCompilerErrorException(Message.UnsupportedMainMethod);
				}
				if(!ReflectUtil.IsFromAssembly(method.DeclaringType, assemblyBuilder)
					&& (!method.IsPublic || !method.DeclaringType.IsPublic))
				{
					throw new FatalCompilerErrorException(Message.ExternalMainNotAccessible);
				}
				Type apartmentAttributeType = null;
				if(options.apartment == ApartmentState.STA)
				{
					apartmentAttributeType = JVM.Import(typeof(STAThreadAttribute));
				}
				else if(options.apartment == ApartmentState.MTA)
				{
					apartmentAttributeType = JVM.Import(typeof(MTAThreadAttribute));
				}
				SetMain(method, options.target, options.props, options.noglobbing, apartmentAttributeType);
			}
			if(map != null)
			{
				LoadMappedExceptions(map);
				Tracer.Info(Tracer.Compiler, "Loading remapped types (2)");
				try
				{
					FinishRemappedTypes();
				}
				catch (IKVM.Reflection.MissingMemberException x)
				{
					StaticCompiler.IssueMissingTypeMessage((Type)x.MemberInfo);
					return 1;
				}
			}
			Tracer.Info(Tracer.Compiler, "Compiling class files (2)");
			WriteResources();
			if(options.externalResources != null)
			{
				foreach(KeyValuePair<string, string> kv in options.externalResources)
				{
					assemblyBuilder.AddResourceFile(JVM.MangleResourceName(kv.Key), kv.Value);
				}
			}
			if(options.fileversion != null)
			{
				CustomAttributeBuilder filever = new CustomAttributeBuilder(JVM.Import(typeof(System.Reflection.AssemblyFileVersionAttribute)).GetConstructor(new Type[] { Types.String }), new object[] { options.fileversion });
				assemblyBuilder.SetCustomAttribute(filever);
			}
			if(options.assemblyAttributeAnnotations != null)
			{
				foreach(object[] def in options.assemblyAttributeAnnotations)
				{
					Annotation annotation = Annotation.LoadAssemblyCustomAttribute(this, def);
					if(annotation != null)
					{
						annotation.Apply(this, assemblyBuilder, def);
					}
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
					throw new FatalCompilerErrorException(Message.ClassLoaderNotFound);
				}
				if(!wrapper.IsPublic && !ReflectUtil.IsFromAssembly(wrapper.TypeAsBaseType, assemblyBuilder))
				{
					throw new FatalCompilerErrorException(Message.ClassLoaderNotAccessible);
				}
				if(wrapper.IsAbstract)
				{
					throw new FatalCompilerErrorException(Message.ClassLoaderIsAbstract);
				}
				if(!wrapper.IsAssignableTo(ClassLoaderWrapper.LoadClassCritical("java.lang.ClassLoader")))
				{
					throw new FatalCompilerErrorException(Message.ClassLoaderNotClassLoader);
				}
				MethodWrapper mw = wrapper.GetMethodWrapper("<init>", "(Lcli.System.Reflection.Assembly;)V", false);
				if(mw == null)
				{
					throw new FatalCompilerErrorException(Message.ClassLoaderConstructorMissing);
				}
				ConstructorInfo ci = JVM.LoadType(typeof(CustomAssemblyClassLoaderAttribute)).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { Types.Type }, null);
				assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(ci, new object[] { wrapper.TypeAsTBD }));
				// TODO it would be better to do this for all assemblies in a shared class loader group (because options.classloader is relevant only for the main assembly),
				// but since it is probably common to specify the custom assembly class loader at the group level, it hopefully won't make much difference in practice.
				MethodWrapper mwModuleInit = wrapper.GetMethodWrapper("InitializeModule", "(Lcli.System.Reflection.Module;)V", false);
				if(mwModuleInit != null && !mwModuleInit.IsStatic)
				{
					MethodBuilder moduleInitializer = GetTypeWrapperFactory().ModuleBuilder.DefineGlobalMethod(".cctor", MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, null, Type.EmptyTypes);
					ILGenerator ilgen = moduleInitializer.GetILGenerator();
					ilgen.Emit(OpCodes.Ldtoken, moduleInitializer);
					ilgen.Emit(OpCodes.Call, JVM.Import(typeof(System.Reflection.MethodBase)).GetMethod("GetMethodFromHandle", new Type[] { JVM.Import(typeof(RuntimeMethodHandle)) }));
					ilgen.Emit(OpCodes.Callvirt, JVM.Import(typeof(System.Reflection.MemberInfo)).GetMethod("get_Module"));
					ilgen.Emit(OpCodes.Call, StaticCompiler.GetRuntimeType("IKVM.Runtime.ByteCodeHelper").GetMethod("InitializeModule"));
					ilgen.Emit(OpCodes.Ret);
				}
			}
			if (options.iconfile != null)
			{
				assemblyBuilder.__DefineIconResource(IkvmcCompiler.ReadAllBytes(options.iconfile));
			}
			if (options.manifestFile != null)
			{
				assemblyBuilder.__DefineManifestResource(IkvmcCompiler.ReadAllBytes(options.manifestFile));
			}
			assemblyBuilder.DefineVersionInfoResource();
			return 0;
		}

		private static void ser_UnknownElement(object sender, System.Xml.Serialization.XmlElementEventArgs e)
		{
			StaticCompiler.IssueMessage(Message.UnknownElementInMapFile, e.Element.Name, e.LineNumber.ToString(), e.LinePosition.ToString());
		}

		private static void ser_UnknownAttribute(object sender, System.Xml.Serialization.XmlAttributeEventArgs e)
		{
			StaticCompiler.IssueMessage(Message.UnknownAttributeInMapFile, e.Attr.Name, e.LineNumber.ToString(), e.LinePosition.ToString());
		}

		private bool ValidateAndSetMap(IKVM.Internal.MapXml.Root map)
		{
			bool valid = true;
			if (map.assembly != null)
			{
				if (map.assembly.Classes != null)
				{
					foreach (IKVM.Internal.MapXml.Class c in map.assembly.Classes)
					{
						if (c.Fields != null)
						{
							foreach (IKVM.Internal.MapXml.Field f in c.Fields)
							{
								ValidateNameSig("field", c.Name, f.Name, f.Sig, ref valid, true);
							}
						}
						if (c.Methods != null)
						{
							foreach (IKVM.Internal.MapXml.Method m in c.Methods)
							{
								ValidateNameSig("method", c.Name, m.Name, m.Sig, ref valid, false);
							}
						}
						if (c.Constructors != null)
						{
							foreach (IKVM.Internal.MapXml.Constructor ctor in c.Constructors)
							{
								ValidateNameSig("constructor", c.Name, "<init>", ctor.Sig, ref valid, false);
							}
						}
						if (c.Properties != null)
						{
							foreach (IKVM.Internal.MapXml.Property prop in c.Properties)
							{
								ValidateNameSig("property", c.Name, prop.Name, prop.Sig, ref valid, false);
								ValidatePropertyGetterSetter("getter", c.Name, prop.Name, prop.getter, ref valid);
								ValidatePropertyGetterSetter("setter", c.Name, prop.Name, prop.setter, ref valid);
							}
						}
					}
				}
			}
			this.map = map;
			return valid;
		}

		private static void ValidateNameSig(string member, string clazz, string name, string sig, ref bool valid, bool field)
		{
			if (!IsValidName(name))
			{
				valid = false;
				StaticCompiler.IssueMessage(Message.InvalidMemberNameInMapFile, member, name, clazz);
			}
			if (!IsValidSig(sig, field))
			{
				valid = false;
				StaticCompiler.IssueMessage(Message.InvalidMemberSignatureInMapFile, member, clazz, name, sig);
			}
		}

		private static void ValidatePropertyGetterSetter(string getterOrSetter, string clazz, string property, IKVM.Internal.MapXml.Method method, ref bool valid)
		{
			if (method != null)
			{
				if (!IsValidName(method.Name))
				{
					valid = false;
					StaticCompiler.IssueMessage(Message.InvalidPropertyNameInMapFile, getterOrSetter, clazz, property, method.Name);
				}
				if (!ClassFile.IsValidMethodSig(method.Sig))
				{
					valid = false;
					StaticCompiler.IssueMessage(Message.InvalidPropertySignatureInMapFile, getterOrSetter, clazz, property, method.Sig);
				}
			}
		}

		private static bool IsValidName(string name)
		{
			return name != null && name.Length != 0;
		}

		private static bool IsValidSig(string sig, bool field)
		{
			return sig != null && (field ? ClassFile.IsValidFieldSig(sig) : ClassFile.IsValidMethodSig(sig));
		}

		internal Type GetTypeFromReferencedAssembly(string name)
		{
			foreach (AssemblyClassLoader acl in referencedAssemblies)
			{
				Type type = acl.MainAssembly.GetType(name, false);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		internal override void IssueMessage(Message msgId, params string[] values)
		{
			StaticCompiler.IssueMessage(options, msgId, values);
		}

		internal bool TryEnableUnmanagedExports()
		{
			// we only support -platform:x86 and -platform:x64
			// (currently IKVM.Reflection doesn't support unmanaged exports for ARM)
			if ((options.imageFileMachine == ImageFileMachine.I386 && (options.pekind & PortableExecutableKinds.Required32Bit) != 0)
				|| options.imageFileMachine == ImageFileMachine.AMD64)
			{
				// when you add unmanaged exports, the ILOnly flag MUST NOT be set or the DLL will fail to load
				options.pekind &= ~PortableExecutableKinds.ILOnly;
				return true;
			}
			else
			{
				StaticCompiler.IssueMessage(options, Message.DllExportRequiresSupportedPlatform);
				return false;
			}
		}

		internal override bool WarningLevelHigh
		{
			get { return options.warningLevelHigh; }
		}

		internal override bool NoParameterReflection
		{
			get { return options.noParameterReflection; }
		}

		protected override void CheckProhibitedPackage(string className)
		{
			if (!compilingCoreAssembly)
			{
				base.CheckProhibitedPackage(className);
			}
		}
	}

	sealed class Jar
	{
		internal readonly string Name;
		internal readonly string Comment;
		private readonly List<JarItem> Items = new List<JarItem>();

		internal Jar(string name, string comment)
		{
			this.Name = name;
			this.Comment = comment;
		}

		internal Jar Copy()
		{
			Jar newJar = new Jar(Name, Comment);
			newJar.Items.AddRange(Items);
			return newJar;
		}

		internal void Add(ZipEntry ze, byte[] data)
		{
			Items.Add(new JarItem(ze, data, null));
		}

		internal void Add(string name, byte[] data, FileInfo fileInfo)
		{
			ZipEntry zipEntry = new ZipEntry(name);
			zipEntry.DateTime = fileInfo.LastWriteTimeUtc;
			zipEntry.CompressionMethod = CompressionMethod.Stored;
			Items.Add(new JarItem(zipEntry, data, fileInfo));
		}

		private struct JarItem
		{
			internal readonly ZipEntry zipEntry;
			internal readonly byte[] data;
			internal readonly FileInfo path;			// path of the original file, if it was individual file (used to construct source file path)

			internal JarItem(ZipEntry zipEntry, byte[] data, FileInfo path)
			{
				this.zipEntry = zipEntry;
				this.data = data;
				this.path = path;
			}
		}

		public struct Item
		{
			internal readonly Jar Jar;
			private readonly int Index;

			internal Item(Jar jar, int index)
			{
				this.Jar = jar;
				this.Index = index;
			}

			internal string Name
			{
				get { return Jar.Items[Index].zipEntry.Name; }
			}

			internal byte[] GetData()
			{
				return Jar.Items[Index].data;
			}

			internal FileInfo Path
			{
				get { return Jar.Items[Index].path; }
			}

			internal ZipEntry ZipEntry
			{
				get
				{
					ZipEntry org = Jar.Items[Index].zipEntry;
					ZipEntry zipEntry = new ZipEntry(org.Name);
					zipEntry.Comment = org.Comment;
					zipEntry.CompressionMethod = org.CompressionMethod;
					zipEntry.DosTime = org.DosTime;
					zipEntry.ExternalFileAttributes = org.ExternalFileAttributes;
					zipEntry.ExtraData = org.ExtraData;
					zipEntry.Flags = org.Flags;
					return zipEntry;
				}
			}

			internal void Remove()
			{
				Jar.Items[Index] = new JarItem();
			}

			internal void MarkAsStub()
			{
				Jar.Items[Index] = new JarItem(Jar.Items[Index].zipEntry, null, null);
			}

			internal bool IsStub
			{
				get { return Jar.Items[Index].data == null; }
			}
		}

		internal struct JarEnumerator
		{
			private readonly Jar jar;
			private int index;

			internal JarEnumerator(Jar jar)
			{
				this.jar = jar;
				this.index = -1;
			}

			public Item Current
			{
				get { return new Item(jar, index); }
			}

			public bool MoveNext()
			{
				while (index + 1 < jar.Items.Count)
				{
					if (jar.Items[++index].zipEntry != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		public JarEnumerator GetEnumerator()
		{
			return new JarEnumerator(this);
		}
	}

	sealed class CompilerOptions
	{
		internal List<Jar> jars = new List<Jar>();
		private Dictionary<string, int> jarMap = new Dictionary<string, int>();
		internal int classesJar = -1;
		internal int resourcesJar = -1;
		internal bool nojarstubs;
		internal FileInfo path;
		internal FileInfo keyfile;
		internal string keycontainer;
		internal bool delaysign;
		internal byte[] publicKey;
		internal StrongNameKeyPair keyPair;
		internal Version version;
		internal string fileversion;
		internal FileInfo iconfile;
		internal FileInfo manifestFile;
		internal bool targetIsModule;
		internal string assembly;
		internal string mainClass;
		internal ApartmentState apartment;
		internal PEFileKinds target;
		internal bool guessFileKind;
		internal string[] unresolvedReferences;	// only used during command line parsing
		internal Dictionary<string, string> legacyStubReferences = new Dictionary<string,string>();	// only used during command line parsing
		internal Assembly[] references;
		internal string[] peerReferences;
		internal bool crossReferenceAllPeers = true;
		internal string[] classesToExclude;		// only used during command line parsing
		internal FileInfo remapfile;
		internal Dictionary<string, string> props;
		internal bool noglobbing;
		internal CodeGenOptions codegenoptions;
		internal bool compressedResources;
		internal string[] privatePackages;
		internal string[] publicPackages;
		internal string sourcepath;
		internal Dictionary<string, string> externalResources;
		internal string classLoader;
		internal PortableExecutableKinds pekind = PortableExecutableKinds.ILOnly;
		internal ImageFileMachine imageFileMachine = ImageFileMachine.I386;
		internal long baseAddress;
		internal int fileAlignment;
		internal bool highentropyva;
		internal List<CompilerClassLoader> sharedclassloader; // should *not* be deep copied in Copy(), because we want the list of all compilers that share a class loader
		internal Dictionary<string, string> suppressWarnings = new Dictionary<string, string>();
		internal Dictionary<string, string> errorWarnings = new Dictionary<string, string>();	// treat specific warnings as errors
		internal bool warnaserror; // treat all warnings as errors
		internal FileInfo writeSuppressWarningsFile;
		internal List<string> proxies = new List<string>();
		internal object[] assemblyAttributeAnnotations;
		internal bool warningLevelHigh;
		internal bool noParameterReflection;

		internal CompilerOptions Copy()
		{
			CompilerOptions copy = (CompilerOptions)MemberwiseClone();
			copy.jars = Copy(jars);
			copy.jarMap = new Dictionary<string, int>(jarMap);
			if (props != null)
			{
				copy.props = new Dictionary<string, string>(props);
			}
			if (externalResources != null)
			{
				copy.externalResources = new Dictionary<string, string>(externalResources);
			}
			copy.suppressWarnings = new Dictionary<string, string>(suppressWarnings);
			copy.errorWarnings = new Dictionary<string, string>(errorWarnings);
			return copy;
		}

		private static List<Jar> Copy(List<Jar> jars)
		{
			List<Jar> newJars = new List<Jar>();
			foreach (Jar jar in jars)
			{
				newJars.Add(jar.Copy());
			}
			return newJars;
		}

		internal Jar GetJar(ZipFile zf)
		{
			int existingJar;
			if (jarMap.TryGetValue(zf.Name, out existingJar))
			{
				return jars[existingJar];
			}
			jarMap.Add(zf.Name, jars.Count);
			return CreateJar(Path.GetFileName(zf.Name), zf.ZipFileComment);
		}

		private Jar CreateJar(string jarName, string comment)
		{
			int count = 0;
			string name = jarName;
		retry:
			foreach (Jar jar in jars)
			{
				if (jar.Name == name)
				{
					name = Path.GetFileNameWithoutExtension(jarName) + "-" + (++count) + Path.GetExtension(jarName);
					goto retry;
				}
			}
			Jar newJar = new Jar(name, comment);
			jars.Add(newJar);
			return newJar;
		}

		internal Jar GetClassesJar()
		{
			if (classesJar == -1)
			{
				classesJar = jars.Count;
				CreateJar("classes.jar", null);
			}
			return jars[classesJar];
		}

		internal bool IsClassesJar(Jar jar)
		{
			return classesJar != -1 && jars[classesJar] == jar;
		}

		internal Jar GetResourcesJar()
		{
			if (resourcesJar == -1)
			{
				resourcesJar = jars.Count;
				CreateJar("resources.jar", null);
			}
			return jars[resourcesJar];
		}

		internal bool IsResourcesJar(Jar jar)
		{
			return resourcesJar != -1 && jars[resourcesJar] == jar;
		}

		internal bool IsExcludedClass(string className)
		{
			if (classesToExclude != null)
			{
				for (int i = 0; i < classesToExclude.Length; i++)
				{
					if (Regex.IsMatch(className, classesToExclude[i]))
					{
						return true;
					}
				}
			}
			return false;
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
		SkippingReferencedClass = 109,
		NoJniRuntime= 110,
		EmittedNoClassDefFoundError = 111,
		EmittedIllegalAccessError = 112,
		EmittedInstantiationError = 113,
		EmittedIncompatibleClassChangeError = 114,
		EmittedNoSuchFieldError = 115,
		EmittedAbstractMethodError = 116,
		EmittedNoSuchMethodError = 117,
		EmittedLinkageError = 118,
		EmittedVerificationError = 119,
		EmittedClassFormatError = 120,
		InvalidCustomAttribute = 121,
		IgnoredCustomAttribute = 122,
		AssumeAssemblyVersionMatch = 123,
		InvalidDirectoryInLibOptionPath = 124,
		InvalidDirectoryInLibEnvironmentPath = 125,
		LegacySearchRule = 126,
		AssemblyLocationIgnored = 127,
		InterfaceMethodCantBeInternal = 128,
		DllExportMustBeStaticMethod = 129,
		DllExportRequiresSupportedPlatform = 130,
		DuplicateAssemblyReference = 132,
		UnableToResolveType = 133,
		StubsAreDeprecated = 134,
		WrongClassName = 135,
		ReflectionCallerClassRequiresCallerID = 136,
		LegacyAssemblyAttributesFound = 137,
		UnableToCreateLambdaFactory = 138,
		UnknownWarning = 999,
		// This is where the errors start
		StartErrors = 4000,
		UnableToCreateProxy = 4001,
		DuplicateProxy = 4002,
		MapXmlUnableToResolveOpCode = 4003,
		MapXmlError = 4004,
		InputFileNotFound = 4005,
		UnknownFileType = 4006,
		UnknownElementInMapFile = 4007,
		UnknownAttributeInMapFile = 4008,
		InvalidMemberNameInMapFile = 4009,
		InvalidMemberSignatureInMapFile = 4010,
		InvalidPropertyNameInMapFile = 4011,
		InvalidPropertySignatureInMapFile = 4012,
		NonPrimaryAssemblyReference = 4013,
		MissingType = 4014,
		MissingReference = 4015,
		CallerSensitiveOnUnsupportedMethod = 4016,
		// Fatal errors
		ResponseFileDepthExceeded = 5000,
		ErrorReadingFile = 5001,
		NoTargetsFound = 5002,
		FileFormatLimitationExceeded = 5003,
		CannotSpecifyBothKeyFileAndContainer = 5004,
		DelaySignRequiresKey = 5005,
		InvalidStrongNameKeyPair = 5006,
		ReferenceNotFound = 5007,
		OptionsMustPreceedChildLevels = 5008,
		UnrecognizedTargetType = 5009,
		UnrecognizedPlatform = 5010,
		UnrecognizedApartment = 5011,
		MissingFileSpecification = 5012,
		PathTooLong = 5013,
		PathNotFound = 5014,
		InvalidPath = 5015,
		InvalidOptionSyntax = 5016,
		ExternalResourceNotFound = 5017,
		ExternalResourceNameInvalid = 5018,
		InvalidVersionFormat = 5019,
		InvalidFileAlignment = 5020,
		ErrorWritingFile = 5021,
		UnrecognizedOption = 5022,
		NoOutputFileSpecified = 5023,
		SharedClassLoaderCannotBeUsedOnModuleTarget = 5024,
		RuntimeNotFound = 5025,
		MainClassRequiresExe = 5026,
		ExeRequiresMainClass = 5027,
		PropertiesRequireExe = 5028,
		ModuleCannotHaveClassLoader = 5029,
		ErrorParsingMapFile = 5030,
		BootstrapClassesMissing = 5031,
		StrongNameRequiresStrongNamedRefs = 5032,
		MainClassNotFound = 5033,
		MainMethodNotFound = 5034,
		UnsupportedMainMethod = 5035,
		ExternalMainNotAccessible = 5036,
		ClassLoaderNotFound = 5037,
		ClassLoaderNotAccessible = 5038,
		ClassLoaderIsAbstract = 5039,
		ClassLoaderNotClassLoader = 5040,
		ClassLoaderConstructorMissing = 5041,
		MapFileTypeNotFound = 5042,
		MapFileClassNotFound = 5043,
		MaximumErrorCountReached = 5044,
		LinkageError = 5045,
		RuntimeMismatch = 5046,
		RuntimeMismatchStrongName = 5047,
		CoreClassesMissing = 5048,
		CriticalClassNotFound = 5049,
		AssemblyContainsDuplicateClassNames = 5050,
		CallerIDRequiresHasCallerIDAnnotation = 5051,
		UnableToResolveInterface = 5052,
		MissingBaseType = 5053,
		MissingBaseTypeReference = 5054,
		FileNotFound = 5055,
		RuntimeMethodMissing = 5056,
		MapFileFieldNotFound = 5057,
		GhostInterfaceMethodMissing = 5058,
	}

	static class StaticCompiler
	{
		private static Universe universe;
		internal static Assembly runtimeAssembly;
		internal static Assembly runtimeJniAssembly;
		internal static CompilerOptions toplevel;
		internal static int errorCount;

		internal static Universe Universe
		{
			get
			{
				Debug.Assert(universe != null);
				return universe;
			}
		}

		internal static void Init(bool emitSymbols)
		{
			UniverseOptions options = UniverseOptions.ResolveMissingMembers | UniverseOptions.EnableFunctionPointers;
			if (!emitSymbols)
			{
				options |= UniverseOptions.DeterministicOutput;
			}
			universe = new Universe(options);
			universe.ResolvedMissingMember += ResolvedMissingMember;
		}

		private static void ResolvedMissingMember(Module requestingModule, MemberInfo member)
		{
			if (requestingModule != null && member is Type)
			{
				IssueMessage(Message.UnableToResolveType, requestingModule.Name, ((Type)member).FullName, member.Module.FullyQualifiedName);
			}
		}

		internal static Assembly Load(string assemblyString)
		{
			Assembly asm = Universe.Load(assemblyString);
			if (asm.__IsMissing)
			{
				throw new FileNotFoundException(assemblyString);
			}
			return asm;
		}

		internal static Assembly LoadFile(string path)
		{
			return Universe.LoadFile(path);
		}

		internal static Type GetRuntimeType(string name)
		{
			Type type = runtimeAssembly.GetType(name);
			if (type != null)
			{
				return type;
			}
			if (runtimeJniAssembly != null)
			{
				return runtimeJniAssembly.GetType(name, true);
			}
			else
			{
				throw new TypeLoadException(name);
			}
		}

		internal static Type GetTypeForMapXml(ClassLoaderWrapper loader, string name)
		{
			Type type = GetType(loader, name);
			if (type == null)
			{
				throw new FatalCompilerErrorException(Message.MapFileTypeNotFound, name);
			}
			return type;
		}

		internal static TypeWrapper GetClassForMapXml(ClassLoaderWrapper loader, string name)
		{
			TypeWrapper tw = loader.LoadClassByDottedNameFast(name);
			if (tw == null)
			{
				throw new FatalCompilerErrorException(Message.MapFileClassNotFound, name);
			}
			return tw;
		}

		internal static FieldWrapper GetFieldForMapXml(ClassLoaderWrapper loader, string clazz, string name, string sig)
		{
			FieldWrapper fw = GetClassForMapXml(loader, clazz).GetFieldWrapper(name, sig);
			if (fw == null)
			{
				throw new FatalCompilerErrorException(Message.MapFileFieldNotFound, name, clazz);
			}
			fw.Link();
			return fw;
		}

		internal static Type GetType(ClassLoaderWrapper loader, string name)
		{
			CompilerClassLoader ccl = (CompilerClassLoader)loader;
			return ccl.GetTypeFromReferencedAssembly(name);
		}

		internal static void IssueMessage(Message msgId, params string[] values)
		{
			IssueMessage(toplevel, msgId, values);
		}

		internal static void IssueMessage(CompilerOptions options, Message msgId, params string[] values)
		{
			if (errorCount != 0 && msgId < Message.StartErrors && !options.warnaserror)
			{
				// don't display any warnings after we've emitted an error message
				return;
			}
			string key = ((int)msgId).ToString();
			for (int i = 0; ; i++)
			{
				if (options.suppressWarnings.ContainsKey(key))
				{
					return;
				}
				if (i == values.Length)
				{
					break;
				}
				key += ":" + values[i];
			}
			options.suppressWarnings.Add(key, key);
			if(options.writeSuppressWarningsFile != null)
			{
				File.AppendAllText(options.writeSuppressWarningsFile.FullName, "-nowarn:" + key + Environment.NewLine);
			}
			string msg;
			switch(msgId)
			{
				case Message.MainMethodFound:
					msg = "Found main method in class \"{0}\"";
					break;
				case Message.OutputFileIs:
					msg = "Output file is \"{0}\"";
					break;
				case Message.AutoAddRef:
					msg = "Automatically adding reference to \"{0}\"";
					break;
				case Message.MainMethodFromManifest:
					msg = "Using main class \"{0}\" based on jar manifest";
					break;
				case Message.ClassNotFound:
					msg = "Class \"{0}\" not found";
					break;
				case Message.ClassFormatError:
					msg = "Unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (class format error \"{1}\")";
					break;
				case Message.DuplicateClassName:
					msg = "Duplicate class name: \"{0}\"";
					break;
				case Message.IllegalAccessError:
					msg = "Unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (illegal access error \"{1}\")";
					break;
				case Message.VerificationError:
					msg = "Unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (verification error \"{1}\")";
					break;
				case Message.NoClassDefFoundError:
					msg = "Unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (missing class \"{1}\")";
					break;
				case Message.GenericUnableToCompileError:
					msg = "Unable to compile class \"{0}\"" + Environment.NewLine + 
						"    (\"{1}\": \"{2}\")";
					break;
				case Message.DuplicateResourceName:
					msg = "Skipping resource (name clash): \"{0}\"";
					break;
				case Message.SkippingReferencedClass:
					msg = "Skipping class: \"{0}\"" + Environment.NewLine +
						"    (class is already available in referenced assembly \"{1}\")";
					break;
				case Message.NoJniRuntime:
					msg = "Unable to load runtime JNI assembly";
					break;
				case Message.EmittedNoClassDefFoundError:
					msg = "Emitted java.lang.NoClassDefFoundError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedIllegalAccessError:
					msg = "Emitted java.lang.IllegalAccessError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedInstantiationError:
					msg = "Emitted java.lang.InstantiationError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedIncompatibleClassChangeError:
					msg = "Emitted java.lang.IncompatibleClassChangeError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedNoSuchFieldError:
					msg = "Emitted java.lang.NoSuchFieldError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedAbstractMethodError:
					msg = "Emitted java.lang.AbstractMethodError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedNoSuchMethodError:
					msg = "Emitted java.lang.NoSuchMethodError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedLinkageError:
					msg = "Emitted java.lang.LinkageError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedVerificationError:
					msg = "Emitted java.lang.VerificationError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.EmittedClassFormatError:
					msg = "Emitted java.lang.ClassFormatError in \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.InvalidCustomAttribute:
					msg = "Error emitting \"{0}\" custom attribute" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.IgnoredCustomAttribute:
					msg = "Custom attribute \"{0}\" was ignored" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.AssumeAssemblyVersionMatch:
					msg = "Assuming assembly reference \"{0}\" matches \"{1}\", you may need to supply runtime policy";
					break;
				case Message.InvalidDirectoryInLibOptionPath:
					msg = "Directory \"{0}\" specified in -lib option is not valid";
					break;
				case Message.InvalidDirectoryInLibEnvironmentPath:
					msg = "Directory \"{0}\" specified in LIB environment is not valid";
					break;
				case Message.LegacySearchRule:
					msg = "Found assembly \"{0}\" using legacy search rule, please append '.dll' to the reference";
					break;
				case Message.AssemblyLocationIgnored:
					msg = "Assembly \"{0}\" is ignored as previously loaded assembly \"{1}\" has the same identity \"{2}\"";
					break;
				case Message.InterfaceMethodCantBeInternal:
					msg = "Ignoring @ikvm.lang.Internal annotation on interface method" + Environment.NewLine +
						"    (\"{0}.{1}{2}\")";
					break;
				case Message.DllExportMustBeStaticMethod:
					msg = "Ignoring @ikvm.lang.DllExport annotation on non-static method" + Environment.NewLine +
						"    (\"{0}.{1}{2}\")";
					break;
				case Message.DllExportRequiresSupportedPlatform:
					msg = "Ignoring @ikvm.lang.DllExport annotation due to unsupported target platform";
					break;
				case Message.NonPrimaryAssemblyReference:
					msg = "Referenced assembly \"{0}\" is not the primary assembly of a shared class loader group, please reference primary assembly \"{1}\" instead";
					break;
				case Message.MissingType:
					msg = "Reference to type \"{0}\" claims it is defined in \"{1}\", but it could not be found";
					break;
				case Message.MissingReference:
					msg = "The type '{0}' is defined in an assembly that is not referenced. You must add a reference to assembly '{1}'";
					break;
				case Message.DuplicateAssemblyReference:
					msg = "Duplicate assembly reference \"{0}\"";
					break;
				case Message.UnableToResolveType:
					msg = "Reference in \"{0}\" to type \"{1}\" claims it is defined in \"{2}\", but it could not be found";
					break;
				case Message.StubsAreDeprecated:
					msg = "Compiling stubs is deprecated. Please add a reference to assembly \"{0}\" instead.";
					break;
				case Message.WrongClassName:
					msg = "Unable to compile \"{0}\" (wrong name: \"{1}\")";
					break;
				case Message.ReflectionCallerClassRequiresCallerID:
					msg = "Reflection.getCallerClass() called from non-CallerID method" + Environment.NewLine +
						"    (\"{0}.{1}{2}\")";
					break;
				case Message.LegacyAssemblyAttributesFound:
					msg = "Legacy assembly attributes container found. Please use the -assemblyattributes:<file> option.";
					break;
				case Message.UnableToCreateLambdaFactory:
					msg = "Unable to create static lambda factory.";
					break;
				case Message.UnableToCreateProxy:
					msg = "Unable to create proxy \"{0}\"" + Environment.NewLine +
						"    (\"{1}\")";
					break;
				case Message.DuplicateProxy:
					msg = "Duplicate proxy \"{0}\"";
					break;
				case Message.MapXmlUnableToResolveOpCode:
					msg = "Unable to resolve opcode in remap file: {0}";
					break;
				case Message.MapXmlError:
					msg = "Error in remap file: {0}";
					break;
				case Message.InputFileNotFound:
					msg = "Source file '{0}' not found";
					break;
				case Message.UnknownFileType:
					msg = "Unknown file type: {0}";
					break;
				case Message.UnknownElementInMapFile:
					msg = "Unknown element {0} in remap file, line {1}, column {2}";
					break;
				case Message.UnknownAttributeInMapFile:
					msg = "Unknown attribute {0} in remap file, line {1}, column {2}";
					break;
				case Message.InvalidMemberNameInMapFile:
					msg = "Invalid {0} name '{1}' in remap file in class {2}";
					break;
				case Message.InvalidMemberSignatureInMapFile:
					msg = "Invalid {0} signature '{3}' in remap file for {0} {1}.{2}";
					break;
				case Message.InvalidPropertyNameInMapFile:
					msg = "Invalid property {0} name '{3}' in remap file for property {1}.{2}";
					break;
				case Message.InvalidPropertySignatureInMapFile:
					msg = "Invalid property {0} signature '{3}' in remap file for property {1}.{2}";
					break;
				case Message.UnknownWarning:
					msg = "{0}";
					break;
				case Message.CallerSensitiveOnUnsupportedMethod:
					msg = "CallerSensitive annotation on unsupported method" + Environment.NewLine +
						"    (\"{0}.{1}{2}\")";
					break;
				default:
					throw new InvalidProgramException();
			}
			bool error = msgId >= Message.StartErrors
				|| (options.warnaserror && msgId >= Message.StartWarnings)
				|| options.errorWarnings.ContainsKey(key)
				|| options.errorWarnings.ContainsKey(((int)msgId).ToString());
			Console.Error.Write("{0} IKVMC{1:D4}: ", error ? "error" : msgId < Message.StartWarnings ? "note" : "warning", (int)msgId);
			if (error && Message.StartWarnings <= msgId && msgId < Message.StartErrors)
			{
				Console.Error.Write("Warning as Error: ");
			}
			Console.Error.WriteLine(msg, values);
			if(options != toplevel && options.path != null)
			{
				Console.Error.WriteLine("    (in {0})", options.path);
			}
			if(error)
			{
				if (++errorCount == 100)
				{
					throw new FatalCompilerErrorException(Message.MaximumErrorCountReached);
				}
			}
		}

		internal static void LinkageError(string msg, TypeWrapper actualType, TypeWrapper expectedType, params object[] values)
		{
			object[] args = new object[values.Length + 2];
			values.CopyTo(args, 2);
			args[0] = AssemblyQualifiedName(actualType);
			args[1] = AssemblyQualifiedName(expectedType);
			string str = string.Format(msg, args);
			if (actualType is UnloadableTypeWrapper && (expectedType is CompiledTypeWrapper || expectedType is DotNetTypeWrapper))
			{
				str += string.Format("\n\t(Please add a reference to {0})", expectedType.TypeAsBaseType.Assembly.Location);
			}
			throw new FatalCompilerErrorException(Message.LinkageError, str);
		}

		private static string AssemblyQualifiedName(TypeWrapper tw)
		{
			ClassLoaderWrapper loader = tw.GetClassLoader();
			AssemblyClassLoader acl = loader as AssemblyClassLoader;
			if(acl != null)
			{
				return tw.Name + ", " + acl.GetAssembly(tw).FullName;
			}
			CompilerClassLoader ccl = loader as CompilerClassLoader;
			if(ccl != null)
			{
				return tw.Name + ", " + ccl.GetTypeWrapperFactory().ModuleBuilder.Assembly.FullName;
			}
			return tw.Name + " (unknown assembly)";
		}

		internal static void IssueMissingTypeMessage(Type type)
		{
			type = ReflectUtil.GetMissingType(type);
			StaticCompiler.IssueMessage(type.Assembly.__IsMissing ? Message.MissingReference : Message.MissingType, type.FullName, type.Assembly.FullName);
		}

		internal static void SuppressWarning(CompilerOptions options, Message message, string name)
		{
			options.suppressWarnings[(int)message + ":" + name] = null;
		}
	}

	sealed class Packages
	{
		private readonly List<string> packages = new List<string>();
		private readonly Dictionary<string, string> packagesSet = new Dictionary<string, string>();

		internal void DefinePackage(string packageName, string jar)
		{
			if (!packagesSet.ContainsKey(packageName))
			{
				packages.Add(packageName);
				packagesSet.Add(packageName, jar);
			}
		}

		// returns an array of PackageListAttribute constructor argument arrays
		internal object[][] ToArray()
		{
			List<object[]> list = new List<object[]>();
			// we use an empty string to indicate we don't yet have a jar,
			// because null is used for packages that were defined from
			// the file system (i.e. don't have a jar to load a manifest from)
			string currentJar = "";
			List<string> currentList = new List<string>();
			foreach (string package in packages)
			{
				string jar = packagesSet[package];
				if (jar != currentJar)
				{
					if (currentList.Count != 0)
					{
						list.Add(new object[] { currentJar, currentList.ToArray() });
						currentList.Clear();
					}
					currentJar = jar;
				}
				currentList.Add(package);
			}
			if (currentList.Count != 0)
			{
				list.Add(new object[] { currentJar, currentList.ToArray() });
			}
			return list.ToArray();
		}
	}
}
