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
using System.Collections.Generic;
using System.Diagnostics;
#if STATIC_COMPILER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Internal
{
	sealed class DynamicClassLoader : TypeWrapperFactory
	{
#if !STATIC_COMPILER
		private static List<AssemblyBuilder> saveDebugAssemblies;
		private static List<DynamicClassLoader> saveClassLoaders;
#endif // !STATIC_COMPILER
		private readonly Dictionary<string, TypeWrapper> dynamicTypes = new Dictionary<string, TypeWrapper>();
		private ModuleBuilder moduleBuilder;
#if STATIC_COMPILER
		private TypeBuilder proxyHelperContainer;
		private List<TypeBuilder> proxyHelpers;
		private TypeBuilder proxiesContainer;
		private List<TypeBuilder> proxies;
#endif // STATIC_COMPILER
		private Dictionary<string, TypeBuilder> unloadables;
		private TypeBuilder unloadableContainer;
#if !STATIC_COMPILER && !CLASSGC
		private static DynamicClassLoader instance = new DynamicClassLoader(CreateModuleBuilder());
#endif
#if CLASSGC
		private List<string> friends = new List<string>();
#endif

		[System.Security.SecuritySafeCritical]
		static DynamicClassLoader()
		{
#if !STATIC_COMPILER
			if(JVM.IsSaveDebugImage)
			{
#if !CLASSGC
				saveClassLoaders.Add(instance);
#endif
			}
			// TODO AppDomain.TypeResolve requires ControlAppDomain permission, but if we don't have that,
			// we should handle that by disabling dynamic class loading
			AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(OnTypeResolve);
#endif // !STATIC_COMPILER
		}

		internal DynamicClassLoader(ModuleBuilder moduleBuilder)
		{
			this.moduleBuilder = moduleBuilder;

			// Ref.Emit doesn't like the "<Module>" name for types
			// (since it already defines a pseudo-type named <Module> for global methods and fields)
			dynamicTypes.Add("<Module>", null);
		}

#if CLASSGC
		internal override void AddInternalsVisibleTo(Assembly friend)
		{
			string name = friend.GetName().Name;
			lock (friends)
			{
				if (!friends.Contains(name))
				{
					friends.Add(name);
					((AssemblyBuilder)moduleBuilder.Assembly).SetCustomAttribute(new CustomAttributeBuilder(typeof(System.Runtime.CompilerServices.InternalsVisibleToAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { name }));
				}
			}
		}
#endif // CLASSGC

#if !STATIC_COMPILER
		private static Assembly OnTypeResolve(object sender, ResolveEventArgs args)
		{
			TypeWrapper type;
#if CLASSGC
			DynamicClassLoader instance;
			ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderForDynamicJavaAssembly(args.RequestingAssembly);
			if(loader == null)
			{
				return null;
			}
			instance = (DynamicClassLoader)loader.GetTypeWrapperFactory();
#endif
			instance.dynamicTypes.TryGetValue(args.Name, out type);
			if(type == null)
			{
				return null;
			}
			try
			{
				type.Finish();
			}
			catch(RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			// NOTE We used to remove the type from the hashtable here, but that creates a race condition if
			// another thread also fires the OnTypeResolve event while we're baking the type.
			// I really would like to remove the type from the hashtable, but at the moment I don't see
			// any way of doing that that wouldn't cause this race condition.
			// UPDATE since we now also use the dynamicTypes hashtable to keep track of type names that
			// have been used already, we cannot remove the keys.
			return type.TypeAsTBD.Assembly;
		}
#endif // !STATIC_COMPILER

		internal override bool ReserveName(string name)
		{
			lock(dynamicTypes)
			{
				if(dynamicTypes.ContainsKey(name))
				{
					return false;
				}
				dynamicTypes.Add(name, null);
				return true;
			}
		}

		internal override string AllocMangledName(DynamicTypeWrapper tw)
		{
			string mangledTypeName;
			lock(dynamicTypes)
			{
				mangledTypeName = TypeNameUtil.EscapeName(tw.Name);
				// FXBUG the CLR (both 1.1 and 2.0) doesn't like type names that end with a single period,
				// it loses the trailing period in the name that gets passed in the TypeResolve event.
				if(dynamicTypes.ContainsKey(mangledTypeName) || mangledTypeName.EndsWith("."))
				{
#if STATIC_COMPILER
					Tracer.Warning(Tracer.Compiler, "Class name clash: {0}", mangledTypeName);
#endif
					// Java class names cannot contain slashes (since they are converted into periods),
					// so we take advantage of that fact to create a unique name.
					string baseName = mangledTypeName;
					int instanceId = 0;
					do
					{
						mangledTypeName = baseName + "/" + (++instanceId);
					} while(dynamicTypes.ContainsKey(mangledTypeName));
				}
				dynamicTypes.Add(mangledTypeName, tw);
			}
			return mangledTypeName;
		}

		internal sealed override TypeWrapper DefineClassImpl(Dictionary<string, TypeWrapper> types, ClassFile f, ClassLoaderWrapper classLoader, object protectionDomain)
		{
#if STATIC_COMPILER
			AotTypeWrapper type = new AotTypeWrapper(f, (CompilerClassLoader)classLoader);
			type.CreateStep1();
			types[f.Name] = type;
			return type;
#else
			// this step can throw a retargettable exception, if the class is incorrect
			DynamicTypeWrapper type = new DynamicTypeWrapper(f, classLoader);
			// This step actually creates the TypeBuilder. It is not allowed to throw any exceptions,
			// if an exception does occur, it is due to a programming error in the IKVM or CLR runtime
			// and will cause a CriticalFailure and exit the process.
			type.CreateStep1();
			type.CreateStep2();
			lock(types)
			{
				// in very extreme conditions another thread may have beaten us to it
				// and loaded (not defined) a class with the same name, in that case
				// we'll leak the the Reflection.Emit defined type. Also see the comment
				// in ClassLoaderWrapper.RegisterInitiatingLoader().
				TypeWrapper race;
				types.TryGetValue(f.Name, out race);
				if(race == null)
				{
					types[f.Name] = type;
#if !FIRST_PASS
					java.lang.Class clazz = new java.lang.Class(null);
#if __MonoCS__
					TypeWrapper.SetTypeWrapperHack(clazz, type);
#else
					clazz.typeWrapper = type;
#endif
					clazz.pd = (java.security.ProtectionDomain)protectionDomain;
					type.SetClassObject(clazz);
#endif
				}
				else
				{
					throw new LinkageError("duplicate class definition: " + f.Name);
				}
			}
			return type;
#endif // STATIC_COMPILER
		}

#if STATIC_COMPILER
		internal void DefineProxyHelper(Type type)
		{
			if(proxyHelperContainer == null)
			{
				proxyHelperContainer = moduleBuilder.DefineType("__<Proxy>", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
				AttributeHelper.HideFromJava(proxyHelperContainer);
				AttributeHelper.SetEditorBrowsableNever(proxyHelperContainer);
				proxyHelpers = new List<TypeBuilder>();
			}
			proxyHelpers.Add(proxyHelperContainer.DefineNestedType(TypeNameUtil.MangleNestedTypeName(type.FullName), TypeAttributes.NestedPublic | TypeAttributes.Interface | TypeAttributes.Abstract, null, new Type[] { type }));
		}

		internal TypeBuilder DefineProxy(TypeWrapper proxyClass, TypeWrapper[] interfaces)
		{
			if (proxiesContainer == null)
			{
				proxiesContainer = moduleBuilder.DefineType("__<Proxies>", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract);
				AttributeHelper.HideFromJava(proxiesContainer);
				AttributeHelper.SetEditorBrowsableNever(proxiesContainer);
				proxies = new List<TypeBuilder>();
			}
			Type[] ifaces = new Type[interfaces.Length];
			for (int i = 0; i < ifaces.Length; i++)
			{
				ifaces[i] = interfaces[i].TypeAsBaseType;
			}
			TypeBuilder tb = proxiesContainer.DefineNestedType(GetProxyNestedName(interfaces), TypeAttributes.NestedPublic | TypeAttributes.Class | TypeAttributes.Sealed, proxyClass.TypeAsBaseType, ifaces);
			proxies.Add(tb);
			return tb;
		}
#endif

		private static string GetProxyNestedName(TypeWrapper[] interfaces)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (TypeWrapper tw in interfaces)
			{
				sb.Append(tw.Name.Length).Append('|').Append(tw.Name);
			}
			return TypeNameUtil.MangleNestedTypeName(sb.ToString());
		}

		internal static string GetProxyName(TypeWrapper[] interfaces)
		{
			return "__<Proxies>+" + GetProxyNestedName(interfaces);
		}

		internal static string GetProxyHelperName(Type type)
		{
			return "__<Proxy>+" + TypeNameUtil.MangleNestedTypeName(type.FullName);
		}

		internal override Type DefineUnloadable(string name)
		{
			lock(this)
			{
				if(unloadables == null)
				{
					unloadables = new Dictionary<string, TypeBuilder>();
				}
				TypeBuilder type;
				if(unloadables.TryGetValue(name, out type))
				{
					return type;
				}
				if(unloadableContainer == null)
				{
					unloadableContainer = moduleBuilder.DefineType("__<Unloadable>", TypeAttributes.Interface | TypeAttributes.Abstract);
					AttributeHelper.HideFromJava(unloadableContainer);
				}
				type = unloadableContainer.DefineNestedType(TypeNameUtil.MangleNestedTypeName(name), TypeAttributes.NestedPrivate | TypeAttributes.Interface | TypeAttributes.Abstract);
				unloadables.Add(name, type);
				return type;
			}
		}

		internal void FinishAll()
		{
			Dictionary<TypeWrapper, TypeWrapper> done = new Dictionary<TypeWrapper, TypeWrapper>();
			bool more = true;
			while(more)
			{
				more = false;
				List<TypeWrapper> l = new List<TypeWrapper>(dynamicTypes.Values);
				foreach(TypeWrapper tw in l)
				{
					if(tw != null && !done.ContainsKey(tw))
					{
						more = true;
						done.Add(tw, tw);
						Tracer.Info(Tracer.Runtime, "Finishing {0}", tw.TypeAsTBD.FullName);
						tw.Finish();
					}
				}
			}
			if(unloadableContainer != null)
			{
				unloadableContainer.CreateType();
				foreach(TypeBuilder tb in unloadables.Values)
				{
					tb.CreateType();
				}
			}
#if STATIC_COMPILER
			if(proxyHelperContainer != null)
			{
				proxyHelperContainer.CreateType();
				foreach(TypeBuilder tb in proxyHelpers)
				{
					tb.CreateType();
				}
			}
			if(proxiesContainer != null)
			{
				proxiesContainer.CreateType();
				foreach(TypeBuilder tb in proxies)
				{
					tb.CreateType();
				}
			}
#endif // STATIC_COMPILER
		}

#if !STATIC_COMPILER
		internal static void SaveDebugImages()
		{
			JVM.FinishingForDebugSave = true;
			if (saveClassLoaders != null)
			{
				foreach (DynamicClassLoader instance in saveClassLoaders)
				{
					instance.FinishAll();
					AssemblyBuilder ab = (AssemblyBuilder)instance.ModuleBuilder.Assembly;
					ab.Save(ab.GetName().Name + ".dll");
				}
			}
			if (saveDebugAssemblies != null)
			{
				foreach (AssemblyBuilder ab in saveDebugAssemblies)
				{
					ab.Save(ab.GetName().Name + ".dll");
				}
			}
		}

		internal static void RegisterForSaveDebug(AssemblyBuilder ab)
		{
			if(saveDebugAssemblies == null)
			{
				saveDebugAssemblies = new List<AssemblyBuilder>();
			}
			saveDebugAssemblies.Add(ab);
		}
#endif

		internal sealed override ModuleBuilder ModuleBuilder
		{
			get
			{
				return moduleBuilder;
			}
		}

		[System.Security.SecuritySafeCritical]
		internal static DynamicClassLoader Get(ClassLoaderWrapper loader)
		{
#if STATIC_COMPILER
			DynamicClassLoader instance = new DynamicClassLoader(((CompilerClassLoader)loader).CreateModuleBuilder());
#elif CLASSGC
			DynamicClassLoader instance = new DynamicClassLoader(CreateModuleBuilder());
			if(saveClassLoaders != null)
			{
				saveClassLoaders.Add(instance);
			}
#endif
			return instance;
		}

#if !STATIC_COMPILER
		private static ModuleBuilder CreateModuleBuilder()
		{
			AssemblyName name = new AssemblyName();
			if(JVM.IsSaveDebugImage)
			{
				if(saveClassLoaders == null)
				{
					System.Threading.Interlocked.CompareExchange(ref saveClassLoaders, new List<DynamicClassLoader>(), null);
				}
				// we ignore the race condition (we could end up with multiple assemblies with the same name),
				// because it is pretty harmless (you'll miss one of the ikvmdump-xx.dll files)
				name.Name = "ikvmdump-" + saveClassLoaders.Count;
			}
			else
			{
				name.Name = "ikvm_dynamic_assembly__" + (uint)Environment.TickCount;
			}
			DateTime now = DateTime.Now;
			name.Version = new Version(now.Year, (now.Month * 100) + now.Day, (now.Hour * 100) + now.Minute, (now.Second * 1000) + now.Millisecond);
			List<CustomAttributeBuilder> attribs = new List<CustomAttributeBuilder>();
			AssemblyBuilderAccess access;
			if(JVM.IsSaveDebugImage)
			{
				access = AssemblyBuilderAccess.RunAndSave;
			}
#if CLASSGC
			else if(JVM.classUnloading
				// DefineDynamicAssembly(..., RunAndCollect, ...) does a demand for PermissionSet(Unrestricted), so we want to avoid that in partial trust scenarios
				&& AppDomain.CurrentDomain.IsFullyTrusted)
			{
				access = AssemblyBuilderAccess.RunAndCollect;
			}
#endif
			else
			{
				access = AssemblyBuilderAccess.Run;
			}
#if NET_4_0
			if(!AppDomain.CurrentDomain.IsFullyTrusted)
			{
				attribs.Add(new CustomAttributeBuilder(typeof(System.Security.SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
			}
#endif
			AssemblyBuilder assemblyBuilder =
#if NET_4_0
				AppDomain.CurrentDomain.DefineDynamicAssembly(name, access, null, true, attribs);
#else
				AppDomain.CurrentDomain.DefineDynamicAssembly(name, access, null, null, null, null, null, true, attribs);
#endif
			AttributeHelper.SetRuntimeCompatibilityAttribute(assemblyBuilder);
			bool debug = JVM.EmitSymbols;
			CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, debug });
			assemblyBuilder.SetCustomAttribute(debugAttr);
			ModuleBuilder moduleBuilder = JVM.IsSaveDebugImage ? assemblyBuilder.DefineDynamicModule(name.Name, name.Name + ".dll", debug) : assemblyBuilder.DefineDynamicModule(name.Name, debug);
			moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(IKVM.Attributes.JavaModuleAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
			return moduleBuilder;
		}
#endif // !STATIC_COMPILER
	}
}
