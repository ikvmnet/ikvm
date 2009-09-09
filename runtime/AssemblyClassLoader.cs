/*
  Copyright (C) 2002-2009 Jeroen Frijters

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
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;
using FormatterServices = System.Runtime.Serialization.FormatterServices;
using IKVM.Attributes;

namespace IKVM.Internal
{
	class AssemblyClassLoader : ClassLoaderWrapper
	{
		private static readonly Dictionary<Assembly, AssemblyClassLoader> assemblyClassLoaders = new Dictionary<Assembly, AssemblyClassLoader>();
		private AssemblyLoader assemblyLoader;
		private string[] references;
		private AssemblyClassLoader[] delegates;
		private bool isReflectionOnly;
#if !STATIC_COMPILER
		private Thread initializerThread;
		private int initializerRecursion;
		private volatile object protectionDomain;
		private static Dictionary<string, string> customClassLoaderRedirects;
		private bool hasCustomClassLoader;
#endif
		private Dictionary<int, List<int>> exports;
		private string[] exportedAssemblyNames;
		private AssemblyLoader[] exportedAssemblies;
		private Dictionary<Assembly, AssemblyLoader> exportedLoaders;

		private sealed class AssemblyLoader
		{
			private readonly Assembly assembly;
			private bool[] isJavaModule;
			private Module[] modules;
			private Dictionary<string, string> nameMap;
			private bool hasDotNetModule;
			private AssemblyName[] internalsVisibleTo;

			internal AssemblyLoader(Assembly assembly)
			{
				this.assembly = assembly;
				modules = assembly.GetModules(false);
				isJavaModule = new bool[modules.Length];
				for (int i = 0; i < modules.Length; i++)
				{
					object[] attr;
					try
					{
						attr = AttributeHelper.GetJavaModuleAttributes(modules[i]);
					}
					catch (Exception x)
					{
						// HACK we handle exceptions here, because there is at least one obfuscator that produces
						// invalid assemblies that cause Module.GetCustomAttributes() to throw an exception
						JVM.CriticalFailure("Unexpected exception", x);
						throw null;
					}
					if (attr.Length > 0)
					{
						isJavaModule[i] = true;
						foreach (JavaModuleAttribute jma in attr)
						{
							string[] map = jma.GetClassMap();
							if (map != null)
							{
								if (nameMap == null)
								{
									nameMap = new Dictionary<string, string>();
								}
								for (int j = 0; j < map.Length; j += 2)
								{
									string key = map[j];
									string val = map[j + 1];
									// TODO if there is a name clash between modules, this will throw.
									// Figure out how to handle that.
									nameMap.Add(key, val);
								}
							}
						}
					}
					else
					{
						hasDotNetModule = true;
					}
				}
			}

			internal bool HasJavaModule
			{
				get
				{
					for (int i = 0; i < isJavaModule.Length; i++)
					{
						if (isJavaModule[i])
						{
							return true;
						}
					}
					return false;
				}
			}

			internal Assembly Assembly
			{
				get { return assembly; }
			}

			private Type GetType(string name)
			{
				try
				{
					return assembly.GetType(name);
				}
				catch (FileLoadException x)
				{
					// this can only happen if the assembly was loaded in the ReflectionOnly
					// context and the requested type references a type in another assembly
					// that cannot be found in the ReflectionOnly context
					// TODO figure out what other exceptions Assembly.GetType() can throw
					Tracer.Info(Tracer.Runtime, x.Message);
				}
				return null;
			}

			private Type GetType(Module mod, string name)
			{
				try
				{
					return mod.GetType(name);
				}
				catch (FileLoadException x)
				{
					// this can only happen if the assembly was loaded in the ReflectionOnly
					// context and the requested type references a type in another assembly
					// that cannot be found in the ReflectionOnly context
					// TODO figure out what other exceptions Assembly.GetType() can throw
					Tracer.Info(Tracer.Runtime, x.Message);
				}
				return null;
			}

			private Type GetJavaType(Module mod, string name)
			{
				try
				{
					string n = null;
					if (nameMap != null)
					{
						nameMap.TryGetValue(name, out n);
					}
					Type t = GetType(mod, n != null ? n : name);
					if (t == null)
					{
						n = name.Replace('$', '+');
						if (!ReferenceEquals(n, name))
						{
							t = GetType(n);
						}
					}
					if (t != null
						&& !AttributeHelper.IsHideFromJava(t)
						&& !t.IsArray
						&& !t.IsPointer
						&& !t.IsByRef)
					{
						return t;
					}
				}
				catch (ArgumentException x)
				{
					// we can end up here because we replace the $ with a plus sign
					// (or client code did a Class.forName() on an invalid name)
					Tracer.Info(Tracer.Runtime, x.Message);
				}
				return null;
			}

			internal TypeWrapper DoLoad(string name)
			{
				for (int i = 0; i < modules.Length; i++)
				{
					if (isJavaModule[i])
					{
						Type type = GetJavaType(modules[i], name);
						if (type != null)
						{
							// check the name to make sure that the canonical name was used
							if (CompiledTypeWrapper.GetName(type) == name)
							{
								return CompiledTypeWrapper.newInstance(name, type);
							}
						}
					}
					else
					{
						// TODO should we catch ArgumentException and prohibit array, pointer and byref here?
						Type type = GetType(modules[i], DotNetTypeWrapper.DemangleTypeName(name));
						if (type != null && DotNetTypeWrapper.IsAllowedOutside(type))
						{
							// check the name to make sure that the canonical name was used
							if (DotNetTypeWrapper.GetName(type) == name)
							{
								return DotNetTypeWrapper.Create(type, name);
							}
						}
					}
				}
				if (hasDotNetModule)
				{
					// for fake types, we load the declaring outer type (the real one) and
					// let that generated the manufactured nested classes
					TypeWrapper outer = null;
					if (name.EndsWith(DotNetTypeWrapper.DelegateInterfaceSuffix))
					{
						outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.DelegateInterfaceSuffix.Length));
					}
					else if (name.EndsWith(DotNetTypeWrapper.AttributeAnnotationSuffix))
					{
						outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationSuffix.Length));
					}
					else if (name.EndsWith(DotNetTypeWrapper.AttributeAnnotationReturnValueSuffix))
					{
						outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationReturnValueSuffix.Length));
					}
					else if (name.EndsWith(DotNetTypeWrapper.AttributeAnnotationMultipleSuffix))
					{
						outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationMultipleSuffix.Length));
					}
					else if (name.EndsWith(DotNetTypeWrapper.EnumEnumSuffix))
					{
						outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.EnumEnumSuffix.Length));
					}
					if (outer != null && outer.IsFakeTypeContainer)
					{
						foreach (TypeWrapper tw in outer.InnerClasses)
						{
							if (tw.Name == name)
							{
								return tw;
							}
						}
					}
				}
				return null;
			}

			internal TypeWrapper CreateWrapperForAssemblyType(Type type)
			{
				Module mod = type.Module;
				int moduleIndex = -1;
				for (int i = 0; i < modules.Length; i++)
				{
					if (modules[i] == mod)
					{
						moduleIndex = i;
						break;
					}
				}
				string name;
				if (isJavaModule[moduleIndex])
				{
					name = CompiledTypeWrapper.GetName(type);
				}
				else
				{
					name = DotNetTypeWrapper.GetName(type);
					if (name == null)
					{
						return null;
					}
				}
				if (isJavaModule[moduleIndex])
				{
					if (AttributeHelper.IsHideFromJava(type))
					{
						return null;
					}
					// since this type was compiled from Java source, we have to look for our
					// attributes
					return CompiledTypeWrapper.newInstance(name, type);
				}
				else
				{
					if (!DotNetTypeWrapper.IsAllowedOutside(type))
					{
						return null;
					}
					// since this type was not compiled from Java source, we don't need to
					// look for our attributes, but we do need to filter unrepresentable
					// stuff (and transform some other stuff)
					return DotNetTypeWrapper.Create(type, name);
				}
			}

			internal bool InternalsVisibleTo(AssemblyName otherName)
			{
				if (internalsVisibleTo == null)
				{
					internalsVisibleTo = AttributeHelper.GetInternalsVisibleToAttributes(assembly);
				}
				foreach (AssemblyName name in internalsVisibleTo)
				{
					if (AssemblyName.ReferenceMatchesDefinition(name, otherName))
					{
						return true;
					}
				}
				return false;
			}
		}

		internal AssemblyClassLoader(Assembly assembly)
			: this(assembly, null)
		{
#if !STATIC_COMPILER
			initializerThread = Thread.CurrentThread;
#endif
		}

		internal AssemblyClassLoader(Assembly assembly, string[] fixedReferences)
			: base(CodeGenOptions.None, null)
		{
			this.assemblyLoader = new AssemblyLoader(assembly);
			this.references = fixedReferences;
			this.isReflectionOnly = assembly.ReflectionOnly;
		}

		private void DoInitializeExports()
		{
			lock (this)
			{
				if (delegates == null)
				{
					if (!(ReflectUtil.IsDynamicAssembly(assemblyLoader.Assembly)) && assemblyLoader.Assembly.GetManifestResourceInfo("ikvm.exports") != null)
					{
						List<string> wildcardExports = new List<string>();
						using (Stream stream = assemblyLoader.Assembly.GetManifestResourceStream("ikvm.exports"))
						{
							BinaryReader rdr = new BinaryReader(stream);
							int assemblyCount = rdr.ReadInt32();
							exports = new Dictionary<int, List<int>>();
							exportedAssemblies = new AssemblyLoader[assemblyCount];
							exportedAssemblyNames = new string[assemblyCount];
							exportedLoaders = new Dictionary<Assembly, AssemblyLoader>();
							for (int i = 0; i < assemblyCount; i++)
							{
								exportedAssemblyNames[i] = rdr.ReadString();
								int typeCount = rdr.ReadInt32();
								if (typeCount == 0 && references == null)
								{
									wildcardExports.Add(exportedAssemblyNames[i]);
								}
								for (int j = 0; j < typeCount; j++)
								{
									int hash = rdr.ReadInt32();
									List<int> assemblies;
									if (!exports.TryGetValue(hash, out assemblies))
									{
										assemblies = new List<int>();
										exports.Add(hash, assemblies);
									}
									assemblies.Add(i);
								}
							}
						}
						if (references == null)
						{
							references = wildcardExports.ToArray();
						}
					}
					else
					{
						AssemblyName[] refNames = assemblyLoader.Assembly.GetReferencedAssemblies();
						references = new string[refNames.Length];
						for (int i = 0; i < references.Length; i++)
						{
							references[i] = refNames[i].FullName;
						}
					}
					delegates = new AssemblyClassLoader[references.Length];
				}
			}
		}

		private void LazyInitExports()
		{
			if (delegates == null)
			{
				DoInitializeExports();
			}
		}

		internal Assembly MainAssembly
		{
			get
			{
				return assemblyLoader.Assembly;
			}
		}

		internal Assembly GetAssembly(TypeWrapper wrapper)
		{
			Debug.Assert(wrapper.GetClassLoader() == this);
			while (wrapper.IsFakeNestedType)
			{
				wrapper = wrapper.DeclaringTypeWrapper;
			}
			return wrapper.TypeAsBaseType.Assembly;
		}

		internal override Type GetGenericTypeDefinition(string name)
		{
			try
			{
				// we only have to look in the main assembly, because only a .NET assembly can contain generic type definitions
				// and it cannot be part of a multi assembly sharedclassloader group
				Type type = assemblyLoader.Assembly.GetType(name);
				if (type != null && type.IsGenericTypeDefinition)
				{
					return type;
				}
			}
			catch (FileLoadException x)
			{
				// this can only happen if the assembly was loaded in the ReflectionOnly
				// context and the requested type references a type in another assembly
				// that cannot be found in the ReflectionOnly context
				// TODO figure out what other exceptions Assembly.GetType() can throw
				Tracer.Info(Tracer.Runtime, x.Message);
			}
			return null;
		}

		private Assembly LoadAssemblyOrClearName(ref string name)
		{
			if (name == null)
			{
				// previous load attemp failed
				return null;
			}
			try
			{
				if (isReflectionOnly)
				{
					return Assembly.ReflectionOnlyLoad(name);
				}
				else
				{
					return Assembly.Load(name);
				}
			}
			catch
			{
				// cache failure by clearing out the name the caller uses
				name = null;
				// should we issue a warning error (in ikvmc)?
				return null;
			}
		}

		internal TypeWrapper DoLoad(string name)
		{
			TypeWrapper tw = assemblyLoader.DoLoad(name);
			if (tw != null)
			{
				return RegisterInitiatingLoader(tw);
			}
			LazyInitExports();
			if (exports != null)
			{
				List<int> assemblies;
				if (exports.TryGetValue(JVM.PersistableHash(name), out assemblies))
				{
					foreach (int index in assemblies)
					{
						AssemblyLoader loader = exportedAssemblies[index];
						if (loader == null)
						{
							Assembly asm = LoadAssemblyOrClearName(ref exportedAssemblyNames[index]);
							if (asm == null)
							{
								continue;
							}
							loader = exportedAssemblies[index] = GetLoaderForExportedAssembly(asm);
						}
						tw = loader.DoLoad(name);
						if (tw != null)
						{
							return RegisterInitiatingLoader(tw);
						}
					}
				}
			}
			return null;
		}

		private AssemblyLoader GetLoader(Assembly assembly)
		{
			if (assemblyLoader.Assembly == assembly)
			{
				return assemblyLoader;
			}
			return GetLoaderForExportedAssembly(assembly);
		}

		private AssemblyLoader GetLoaderForExportedAssembly(Assembly assembly)
		{
			LazyInitExports();
			AssemblyLoader loader;
			lock (exportedLoaders)
			{
				exportedLoaders.TryGetValue(assembly, out loader);
			}
			if (loader == null)
			{
				loader = new AssemblyLoader(assembly);
				lock (exportedLoaders)
				{
					AssemblyLoader existing;
					if (exportedLoaders.TryGetValue(assembly, out existing))
					{
						// another thread beat us to it
						loader = existing;
					}
					else
					{
						exportedLoaders.Add(assembly, loader);
					}
				}
			}
			return loader;
		}

		internal virtual TypeWrapper GetWrapperFromAssemblyType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromAssemblyType: {0}", type.FullName);
			Debug.Assert(!type.Name.EndsWith("[]"), "!type.IsArray", type.FullName);
			Debug.Assert(AssemblyClassLoader.FromAssembly(type.Assembly) == this);

			TypeWrapper wrapper = GetLoader(type.Assembly).CreateWrapperForAssemblyType(type);
			if (wrapper != null)
			{
				wrapper = RegisterInitiatingLoader(wrapper);
				if (wrapper.TypeAsTBD != type && (!wrapper.IsRemapped || wrapper.TypeAsBaseType != type))
				{
					// this really shouldn't happen, it means that we have two different types in our assembly that both
					// have the same Java name
					string msg = String.Format("\nType \"{0}\" and \"{1}\" both map to the same name \"{2}\".\n", type.FullName, wrapper.TypeAsTBD.FullName, wrapper.Name);
					JVM.CriticalFailure(msg, null);
				}
				return wrapper;
			}
			return null;
		}

		protected override TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			TypeWrapper tw = DoLoad(name);
			if (tw != null)
			{
				return tw;
			}
#if !STATIC_COMPILER
			if (hasCustomClassLoader)
			{
				return base.LoadClassImpl(name, throwClassNotFoundException);
			}
#endif
			tw = LoadGenericClass(name);
			if (tw != null)
			{
				return tw;
			}
			return LoadReferenced(name);
		}

		internal TypeWrapper LoadReferenced(string name)
		{
			LazyInitExports();
			for (int i = 0; i < delegates.Length; i++)
			{
				if (delegates[i] == null)
				{
					Assembly asm = LoadAssemblyOrClearName(ref references[i]);
					if (asm != null)
					{
						delegates[i] = AssemblyClassLoader.FromAssembly(asm);
					}
				}
				if (delegates[i] != null)
				{
					TypeWrapper tw = delegates[i].DoLoad(name);
					if (tw != null)
					{
						return tw;
					}
				}
			}
			if (!assemblyLoader.HasJavaModule)
			{
				return GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
			}
			return null;
		}

#if !STATIC_COMPILER
		internal Assembly FindResourceAssembliesImpl(string unmangledName, string name, bool firstOnly, ref List<Assembly> list)
		{
			if (ReflectUtil.IsDynamicAssembly(assemblyLoader.Assembly))
			{
				return null;
			}
			if (assemblyLoader.Assembly.GetManifestResourceInfo(name) != null)
			{
				if (firstOnly)
				{
					return assemblyLoader.Assembly;
				}
				list = new List<Assembly>();
				list.Add(assemblyLoader.Assembly);
			}
			LazyInitExports();
			if (exports != null)
			{
				List<int> assemblies;
				if (exports.TryGetValue(JVM.PersistableHash(unmangledName), out assemblies))
				{
					foreach (int index in assemblies)
					{
						AssemblyLoader loader = exportedAssemblies[index];
						if (loader == null)
						{
							Assembly asm = LoadAssemblyOrClearName(ref exportedAssemblyNames[index]);
							if (asm == null)
							{
								continue;
							}
							loader = exportedAssemblies[index] = GetLoaderForExportedAssembly(asm);
						}
						if (loader.Assembly.GetManifestResourceInfo(name) != null)
						{
							if (firstOnly)
							{
								return loader.Assembly;
							}
							if (list == null)
							{
								list = new List<Assembly>();
							}
							list.Add(loader.Assembly);
						}
					}
				}
			}
			return null;
		}

		internal Assembly[] FindResourceAssemblies(string unmangledName, bool firstOnly)
		{
			List<Assembly> list = null;
			string name = JVM.MangleResourceName(unmangledName);
			Assembly first = FindResourceAssembliesImpl(unmangledName, name, firstOnly, ref list);
			if (first != null)
			{
				return new Assembly[] { first };
			}
			LazyInitExports();
			for (int i = 0; i < delegates.Length; i++)
			{
				if (delegates[i] == null)
				{
					Assembly asm = LoadAssemblyOrClearName(ref references[i]);
					if (asm != null)
					{
						delegates[i] = AssemblyClassLoader.FromAssembly(asm);
					}
				}
				if (delegates[i] != null)
				{
					first = delegates[i].FindResourceAssembliesImpl(unmangledName, name, firstOnly, ref list);
					if (first != null)
					{
						return new Assembly[] { first };
					}
				}
			}
			if (!assemblyLoader.HasJavaModule)
			{
				if (firstOnly)
				{
					return GetBootstrapClassLoader().FindResourceAssemblies(unmangledName, firstOnly);
				}
				else
				{
					Assembly[] assemblies = GetBootstrapClassLoader().FindResourceAssemblies(unmangledName, firstOnly);
					if (assemblies != null)
					{
						foreach (Assembly asm in assemblies)
						{
							if (list == null)
							{
								list = new List<Assembly>();
							}
							if (!list.Contains(asm))
							{
								list.Add(asm);
							}
						}
					}
				}
			}
			if (list == null)
			{
				return null;
			}
			return list.ToArray();
		}
#endif // !STATIC_COMPILER

		private void WaitInitDone()
		{
#if !STATIC_COMPILER && !FIRST_PASS
			if (initializerThread != null)
			{
				if (initializerThread == Thread.CurrentThread)
				{
					initializerRecursion++;
					try
					{
						InitializeJavaClassLoader();
					}
					finally
					{
						// We only publish the class loader from the outer most invocation, otherwise
						// an invocation of getClassLoader in the static initializer or constructor
						// of the custom class loader would result in prematurely publishing it.
						if (--initializerRecursion == 0)
						{
							lock (this)
							{
								initializerThread = null;
								Monitor.PulseAll(this);
							}
						}
					}
				}
				else
				{
					lock (this)
					{
						while (initializerThread != null)
						{
							Monitor.Wait(this);
						}
					}
				}
			}
#endif
		}

		internal virtual object GetProtectionDomain()
		{
#if STATIC_COMPILER || FIRST_PASS
			return null;
#else
			if (protectionDomain == null)
			{
				java.net.URL codebase;
				try
				{
					codebase = new java.net.URL(assemblyLoader.Assembly.CodeBase);
				}
				catch (NotSupportedException)
				{
					// dynamic assemblies don't have a codebase
					codebase = null;
				}
				catch (java.net.MalformedURLException)
				{
					codebase = null;
				}
				java.security.Permissions permissions = new java.security.Permissions();
				permissions.add(new java.security.AllPermission());
				object pd = new java.security.ProtectionDomain(new java.security.CodeSource(codebase, (java.security.cert.Certificate[])null), permissions, (java.lang.ClassLoader)GetJavaClassLoader(), null);
				lock (this)
				{
					if (protectionDomain == null)
					{
						protectionDomain = pd;
					}
				}
			}
			return protectionDomain;
#endif
		}

		protected override void CheckDefineClassAllowed(string className)
		{
			if (DoLoad(className) != null)
			{
				throw new LinkageError("duplicate class definition: " + className);
			}
		}

		internal override TypeWrapper GetLoadedClass(string name)
		{
			TypeWrapper tw = base.GetLoadedClass(name);
			return tw != null ? tw : DoLoad(name);
		}

		internal override bool InternalsVisibleToImpl(TypeWrapper wrapper, TypeWrapper friend)
		{
			ClassLoaderWrapper other = friend.GetClassLoader();
			if (this == other)
			{
				return true;
			}
			AssemblyName otherName;
#if STATIC_COMPILER
			CompilerClassLoader ccl = other as CompilerClassLoader;
			if (ccl == null)
			{
				return false;
			}
			otherName = ccl.GetAssemblyName();
#else
			AssemblyClassLoader acl = other as AssemblyClassLoader;
			if (acl == null)
			{
				return false;
			}
			otherName = acl.GetAssembly(friend).GetName();
#endif
			return GetLoaderForExportedAssembly(GetAssembly(wrapper)).InternalsVisibleTo(otherName);
		}

		// this method should not be used with dynamic Java assemblies
		internal static AssemblyClassLoader FromAssembly(Assembly assembly)
		{
			AssemblyClassLoader loader;
			lock (assemblyClassLoaders)
			{
				assemblyClassLoaders.TryGetValue(assembly, out loader);
			}
			if (loader == null)
			{
				loader = Create(assembly);
				lock (assemblyClassLoaders)
				{
					AssemblyClassLoader existing;
					if (assemblyClassLoaders.TryGetValue(assembly, out existing))
					{
						// another thread won the race to create the class loader
						loader = existing;
					}
					else
					{
						assemblyClassLoaders.Add(assembly, loader);
					}
				}
			}
			loader.WaitInitDone();
			return loader;
		}

		private static AssemblyClassLoader Create(Assembly assembly)
		{
			// If the assembly is a part of a multi-assembly shared class loader,
			// it will export the __<MainAssembly> type from the main assembly in the group.
			Type forwarder = assembly.GetType("__<MainAssembly>");
			if (forwarder != null)
			{
				Assembly mainAssembly = forwarder.Assembly;
				if (mainAssembly != assembly)
				{
					return FromAssembly(mainAssembly);
				}
			}
			if (assembly == JVM.CoreAssembly)
			{
				// This cast is necessary for ikvmc and a no-op for the runtime.
				// Note that the cast cannot fail, because ikvmc will only return a non AssemblyClassLoader
				// from GetBootstrapClassLoader() when compiling the core assembly and in that case JVM.CoreAssembly
				// will be null.
				return (AssemblyClassLoader)GetBootstrapClassLoader();
			}
			return new AssemblyClassLoader(assembly);
		}

#if !STATIC_COMPILER && !FIRST_PASS
		private void InitializeJavaClassLoader()
		{
			Assembly assembly = assemblyLoader.Assembly;
			if (!assembly.ReflectionOnly)
			{
				Type customClassLoaderClass = null;
				LoadCustomClassLoaderRedirects();
				string assemblyName = assembly.FullName;
				foreach (KeyValuePair<string, string> kv in customClassLoaderRedirects)
				{
					string asm = kv.Key;
					// We only support matching on the assembly's simple name,
					// because there appears to be no viable alternative.
					// There is AssemblyName.ReferenceMatchesDefinition()
					// but it is completely broken.
					if (assemblyName.StartsWith(asm + ","))
					{
						try
						{
							customClassLoaderClass = Type.GetType(kv.Value, true);
						}
						catch (Exception x)
						{
							Tracer.Error(Tracer.Runtime, "Unable to load custom class loader {0} specified in app.config for assembly {1}: {2}", kv.Value, assembly, x);
						}
						break;
					}
				}
				if (customClassLoaderClass == null)
				{
					object[] attribs = assembly.GetCustomAttributes(typeof(CustomAssemblyClassLoaderAttribute), false);
					if (attribs.Length == 1)
					{
						customClassLoaderClass = ((CustomAssemblyClassLoaderAttribute)attribs[0]).Type;
					}
				}
				if (customClassLoaderClass != null)
				{
					try
					{
						if (!customClassLoaderClass.IsPublic && !customClassLoaderClass.Assembly.Equals(assembly))
						{
							throw new Exception("Type not accessible");
						}
						ConstructorInfo customClassLoaderCtor = customClassLoaderClass.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(Assembly) }, null);
						if (customClassLoaderCtor == null)
						{
							throw new Exception("No constructor");
						}
						if (!customClassLoaderCtor.IsPublic && !customClassLoaderClass.Assembly.Equals(assembly))
						{
							customClassLoaderCtor = null;
							throw new Exception("Constructor not accessible");
						}
						hasCustomClassLoader = true;
						// NOTE we're creating an uninitialized instance of the custom class loader here, so that getClassLoader will return the proper object
						// when it is called during the construction of the custom class loader later on. This still doesn't make it safe to use the custom
						// class loader before it is constructed, but at least the object instance is available and should anyone cache it, they will get the
						// right object to use later on.
						// Note that creating the unitialized instance will (unfortunately) trigger the static initializer. The static initializer can
						// trigger a call to getClassLoader(), which means we can end up here recursively.
						java.lang.ClassLoader newJavaClassLoader = (java.lang.ClassLoader)FormatterServices.GetUninitializedObject(customClassLoaderClass);
						if (javaClassLoader == null) // check if we weren't invoked recursively and the nested invocation already did the work
						{
							javaClassLoader = newJavaClassLoader;
							SetWrapperForClassLoader(javaClassLoader, this);
							DoPrivileged(new CustomClassLoaderCtorCaller(customClassLoaderCtor, javaClassLoader, assembly));
							Tracer.Info(Tracer.Runtime, "Created custom assembly class loader {0} for assembly {1}", customClassLoaderClass.FullName, assembly);
						}
						else
						{
							// we didn't initialize the object, so there is no need to finalize it
							GC.SuppressFinalize(newJavaClassLoader);
						}
					}
					catch (Exception x)
					{
						Tracer.Error(Tracer.Runtime, "Unable to create custom assembly class loader {0} for {1}: {2}", customClassLoaderClass.FullName, assembly, x);
					}
				}
			}
			if (javaClassLoader == null)
			{
				javaClassLoader = (java.lang.ClassLoader)DoPrivileged(new CreateAssemblyClassLoader(assembly));
				SetWrapperForClassLoader(javaClassLoader, this);
			}
		}

		private static void LoadCustomClassLoaderRedirects()
		{
			if (customClassLoaderRedirects == null)
			{
				Dictionary<string, string> dict = new Dictionary<string, string>();
				try
				{
					foreach (string key in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
					{
						const string prefix = "ikvm-classloader:";
						if (key.StartsWith(prefix))
						{
							dict[key.Substring(prefix.Length)] = System.Configuration.ConfigurationManager.AppSettings.Get(key);
						}
					}
				}
				catch (Exception x)
				{
					Tracer.Error(Tracer.Runtime, "Error while reading custom class loader redirects: {0}", x);
				}
				finally
				{
					Interlocked.CompareExchange(ref customClassLoaderRedirects, dict, null);
				}
			}
		}

		internal sealed class CreateAssemblyClassLoader : java.security.PrivilegedAction
		{
			private Assembly assembly;

			internal CreateAssemblyClassLoader(Assembly assembly)
			{
				this.assembly = assembly;
			}

			public object run()
			{
				return new ikvm.runtime.AssemblyClassLoader(assembly);
			}
		}

		private sealed class CustomClassLoaderCtorCaller : java.security.PrivilegedAction
		{
			private ConstructorInfo ctor;
			private object classLoader;
			private Assembly assembly;

			internal CustomClassLoaderCtorCaller(ConstructorInfo ctor, object classLoader, Assembly assembly)
			{
				this.ctor = ctor;
				this.classLoader = classLoader;
				this.assembly = assembly;
			}

			public object run()
			{
				ctor.Invoke(classLoader, new object[] { assembly });
				return null;
			}
		}
#endif
	}

	sealed class BootstrapClassLoader : AssemblyClassLoader
	{
		internal BootstrapClassLoader()
			: base(JVM.CoreAssembly, new string[] {
				typeof(object).Assembly.FullName,		// mscorlib
				typeof(System.Uri).Assembly.FullName	// System
			})
		{
		}

		internal override TypeWrapper GetWrapperFromAssemblyType(Type type)
		{
			// we have to special case the fake types here
			if (type.IsGenericType)
			{
				TypeWrapper outer = ClassLoaderWrapper.GetWrapperFromType(type.GetGenericArguments()[0]);
				foreach (TypeWrapper inner in outer.InnerClasses)
				{
					if (inner.TypeAsTBD == type)
					{
						return inner;
					}
					foreach (TypeWrapper inner2 in inner.InnerClasses)
					{
						if (inner2.TypeAsTBD == type)
						{
							return inner2;
						}
					}
				}
				return null;
			}
			return base.GetWrapperFromAssemblyType(type);
		}

		internal override object GetProtectionDomain()
		{
			return null;
		}
	}
}
