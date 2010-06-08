/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
		// note that MangleNestedTypeName() assumes that there are less than 16 special characters
		private static readonly char[] specialCharacters = { '\\', '+', ',', '[', ']', '*', '&', '\u0000' };
		private static readonly string specialCharactersString = new String(specialCharacters);
#if !STATIC_COMPILER
		private static List<AssemblyBuilder> saveDebugAssemblies;
		private static List<DynamicClassLoader> saveClassLoaders;
#endif // !STATIC_COMPILER
		private readonly Dictionary<string, TypeWrapper> dynamicTypes = new Dictionary<string, TypeWrapper>();
		private ModuleBuilder moduleBuilder;
#if STATIC_COMPILER
		private TypeBuilder proxyHelperContainer;
		private List<TypeBuilder> proxyHelpers;
#endif // STATIC_COMPILER
		private Dictionary<string, TypeBuilder> unloadables;
		private TypeBuilder unloadableContainer;
#if !STATIC_COMPILER && !CLASSGC
		private static DynamicClassLoader instance = new DynamicClassLoader(CreateModuleBuilder());
#endif

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

		internal static string EscapeName(string name)
		{
			// TODO the escaping of special characters is not required on .NET 2.0
			// (but it doesn't really hurt that much either, the only overhead is the
			// extra InnerClassAttribute to record the real name of the class)
			// Note that even though .NET 2.0 automatically escapes the special characters,
			// the name that gets passed in ResolveEventArgs.Name of the TypeResolve event
			// contains the unescaped type name.
			if(name.IndexOfAny(specialCharacters) >= 0)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				foreach(char c in name)
				{
					if(specialCharactersString.IndexOf(c) >= 0)
					{
						if(c == 0)
						{
							// we can't escape the NUL character, so we replace it with a space.
							sb.Append(' ');
							continue;
						}
						sb.Append('\\');
					}
					sb.Append(c);
				}
				name = sb.ToString();
			}
			return name;
		}

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

		private string AllocMangledName(string mangledTypeName)
		{
			lock(dynamicTypes)
			{
				mangledTypeName = EscapeName(mangledTypeName);
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
				dynamicTypes.Add(mangledTypeName, null);
			}
			return mangledTypeName;
		}

		internal sealed override TypeWrapper DefineClassImpl(Dictionary<string, TypeWrapper> types, ClassFile f, ClassLoaderWrapper classLoader, object protectionDomain)
		{
			DynamicTypeWrapper type;
#if STATIC_COMPILER
			type = new AotTypeWrapper(f, (CompilerClassLoader)classLoader);
#else
			type = new DynamicTypeWrapper(f, classLoader);
#endif
			// this step can throw a retargettable exception, if the class is incorrect
			bool hasclinit;
			type.CreateStep1(out hasclinit);
			// now we can allocate the mangledTypeName, because the next step cannot fail
			string mangledTypeName = AllocMangledName(f.Name);
			// This step actually creates the TypeBuilder. It is not allowed to throw any exceptions,
			// if an exception does occur, it is due to a programming error in the IKVM or CLR runtime
			// and will cause a CriticalFailure and exit the process.
			type.CreateStep2NoFail(hasclinit, mangledTypeName);
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
					lock(dynamicTypes)
					{
						Debug.Assert(dynamicTypes.ContainsKey(mangledTypeName) && dynamicTypes[mangledTypeName] == null);
						dynamicTypes[mangledTypeName] = type;
					}
					types[f.Name] = type;
#if !STATIC_COMPILER && !FIRST_PASS
					java.lang.Class clazz = new java.lang.Class(null);
#if __MonoCS__
					TypeWrapper.SetTypeWrapperHack(ref clazz.typeWrapper, type);
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
			proxyHelpers.Add(proxyHelperContainer.DefineNestedType(MangleNestedTypeName(type.FullName), TypeAttributes.NestedPublic | TypeAttributes.Interface | TypeAttributes.Abstract, null, new Type[] { type }));
		}
#endif

		internal static string GetProxyHelperName(Type type)
		{
			return "__<Proxy>+" + MangleNestedTypeName(type.FullName);
		}

		private static string MangleNestedTypeName(string name)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (char c in name)
			{
				int index = specialCharactersString.IndexOf(c);
				if(c == '.')
				{
					sb.Append("_");
				}
				else if(c == '_')
				{
					sb.Append("^-");
				}
				else if(index == -1)
				{
					sb.Append(c);
					if(c == '^')
					{
						sb.Append(c);
					}
				}
				else
				{
					sb.Append('^').AppendFormat("{0:X1}", index);
				}
			}
			return sb.ToString();
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
				type = unloadableContainer.DefineNestedType(MangleNestedTypeName(name), TypeAttributes.NestedPrivate | TypeAttributes.Interface | TypeAttributes.Abstract);
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
