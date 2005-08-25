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
using System.Reflection.Emit;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Internal
{
	class IdentityProvider : IHashCodeProvider, IComparer
	{
		public int GetHashCode(object obj)
		{
			return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
		}

		public int Compare(object x, object y)
		{
			return x == y ? 0 : 1;
		}
	}

	class ClassLoaderWrapper
	{
		private static bool arrayConstructionHack;
		private static readonly object arrayConstructionLock = new object();
		private static readonly IdentityProvider identityProvider = new IdentityProvider();
		private static readonly Hashtable javaClassLoaderToClassLoaderWrapper = new Hashtable(identityProvider, identityProvider);
		private static readonly Hashtable dynamicTypes = Hashtable.Synchronized(new Hashtable());
		private static readonly Hashtable typeToTypeWrapper = Hashtable.Synchronized(new Hashtable(identityProvider, identityProvider));
		private static ClassLoaderWrapper bootstrapClassLoader;
		private static ClassLoaderWrapper systemClassLoader;
		private object javaClassLoader;
		private Hashtable types = new Hashtable();
		private ArrayList nativeLibraries;
		// FXBUG moduleBuilder is static, because multiple dynamic assemblies is broken (TypeResolve doesn't fire)
		// so for the time being, we share one dynamic assembly among all classloaders
		private static ModuleBuilder moduleBuilder;
		private static bool saveDebugImage;
		private static Hashtable nameClashHash = new Hashtable();
		private static Assembly coreAssembly;	// this is the assembly that contains the remapped and core classes
		private static Hashtable remappedTypes = new Hashtable();
		private static int instanceCounter = 0;
		private static ArrayList saveDebugAssemblies;
		private int instanceId = System.Threading.Interlocked.Increment(ref instanceCounter);

		// HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
		internal static void SetBootstrapClassLoader(ClassLoaderWrapper bootstrapClassLoader)
		{
			Debug.Assert(ClassLoaderWrapper.bootstrapClassLoader == null);

			ClassLoaderWrapper.bootstrapClassLoader = bootstrapClassLoader;
		}

		static ClassLoaderWrapper()
		{
			// TODO AppDomain.TypeResolve requires ControlAppDomain permission, but if we don't have that,
			// we should handle that by disabling dynamic class loading
			AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(OnTypeResolve);
			typeToTypeWrapper[PrimitiveTypeWrapper.BOOLEAN.TypeAsTBD] = PrimitiveTypeWrapper.BOOLEAN;
			typeToTypeWrapper[PrimitiveTypeWrapper.BYTE.TypeAsTBD] = PrimitiveTypeWrapper.BYTE;
			typeToTypeWrapper[PrimitiveTypeWrapper.CHAR.TypeAsTBD] = PrimitiveTypeWrapper.CHAR;
			typeToTypeWrapper[PrimitiveTypeWrapper.DOUBLE.TypeAsTBD] = PrimitiveTypeWrapper.DOUBLE;
			typeToTypeWrapper[PrimitiveTypeWrapper.FLOAT.TypeAsTBD] = PrimitiveTypeWrapper.FLOAT;
			typeToTypeWrapper[PrimitiveTypeWrapper.INT.TypeAsTBD] = PrimitiveTypeWrapper.INT;
			typeToTypeWrapper[PrimitiveTypeWrapper.LONG.TypeAsTBD] = PrimitiveTypeWrapper.LONG;
			typeToTypeWrapper[PrimitiveTypeWrapper.SHORT.TypeAsTBD] = PrimitiveTypeWrapper.SHORT;
			typeToTypeWrapper[PrimitiveTypeWrapper.VOID.TypeAsTBD] = PrimitiveTypeWrapper.VOID;
			LoadRemappedTypes();
		}

		internal static void LoadRemappedTypes()
		{
			Debug.Assert(coreAssembly == null);

			// if we're compiling the core, impl will be null
			object impl = JVM.Library;

			if(impl != null)
			{
				coreAssembly = impl.GetType().Assembly;

				object[] remapped = coreAssembly.GetCustomAttributes(typeof(RemappedClassAttribute), false);
				if(remapped.Length > 0)
				{
					foreach(RemappedClassAttribute r in remapped)
					{
						Tracer.Info(Tracer.Runtime, "Remapping type {0} to {1}", r.RemappedType, r.Name);
						remappedTypes.Add(r.RemappedType, r.Name);
					}
				}
				else
				{
					JVM.CriticalFailure("Failed to find core classes in core library", null);
				}
			}
		}

		internal static bool IsCoreAssemblyType(Type type)
		{
			return type.Assembly == coreAssembly;
		}

		private static Assembly OnTypeResolve(object sender, ResolveEventArgs args)
		{
			lock(arrayConstructionLock)
			{
				Tracer.Info(Tracer.ClassLoading, "OnTypeResolve: {0} (arrayConstructionHack = {1})", args.Name, arrayConstructionHack);
				if(arrayConstructionHack)
				{
					return null;
				}
			}
			TypeWrapper type = (TypeWrapper)dynamicTypes[args.Name];
			if(type == null)
			{
				return null;
			}
			// During static compilation, a TypeResolve event should never trigger a finish.
			if(JVM.IsStaticCompilerPhase1)
			{
				JVM.CriticalFailure("Finish triggered during phase 1 of compilation.", null);
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
			//dynamicTypes.Remove(args.Name);
			return type.TypeAsTBD.Assembly;
		}

		internal ClassLoaderWrapper(object javaClassLoader)
		{
			SetJavaClassLoader(javaClassLoader);
		}

		internal void SetJavaClassLoader(object javaClassLoader)
		{
			this.javaClassLoader = javaClassLoader;
		}

		internal static bool IsRemappedType(Type type)
		{
			return remappedTypes.ContainsKey(type);
		}

		internal void SetRemappedType(Type type, TypeWrapper tw)
		{
			Debug.Assert(!types.ContainsKey(tw.Name));
			types.Add(tw.Name, tw);
			Debug.Assert(!typeToTypeWrapper.ContainsKey(type));
			typeToTypeWrapper.Add(type, tw);
			remappedTypes.Add(type, type);
		}

		// HACK return the TypeWrapper if it is already loaded
		// (this exists solely for DynamicTypeWrapper.SetupGhosts and VMClassLoader.findLoadedClass)
		internal TypeWrapper GetLoadedClass(string name)
		{
			lock(types.SyncRoot)
			{
				return (TypeWrapper)types[name];
			}
		}

		internal TypeWrapper RegisterInitiatingLoader(TypeWrapper tw)
		{
			if(tw == null || tw.IsUnloadable || tw.IsPrimitive)
				return tw;

			lock(types.SyncRoot)
			{
				object existing = types[tw.Name];
				if(existing != tw)
				{
					if(existing != null)
					{
						throw new LinkageError("duplicate class definition: " + tw.Name);
					}
					if(tw.IsArray)
					{
						TypeWrapper elem = tw;
						// TODO there should be a way to get the ultimate element type
						// without creating all the intermediate types
						while(elem.IsArray)
						{
							elem = elem.ElementTypeWrapper;
						}
						RegisterInitiatingLoader(elem);
					}
					// NOTE if types.ContainsKey(tw.Name) is true (i.e. the value is null),
					// we currently have a DefineClass in progress on another thread and we've
					// beaten that thread to the punch by loading the class from a parent class
					// loader instead. This is ok as DefineClass will throw a LinkageError when
					// it is done.
					types[tw.Name] = tw;
				}
			}
			return tw;
		}

		// FXBUG This mangles type names, to enable different class loaders loading classes with the same names.
		// We used to support this by using an assembly per class loader instance, but because
		// of the CLR TypeResolve bug, we put all types in a single assembly for now.
		internal string MangleTypeName(string name)
		{
			lock(nameClashHash.SyncRoot)
			{
				// FXBUG the 1.1 CLR doesn't like type names that end with a period.
				if(nameClashHash.ContainsKey(name) || name.EndsWith("."))
				{
					if(JVM.IsStaticCompiler)
					{
						Tracer.Warning(Tracer.Compiler, "Class name clash: {0}", name);
					}
					return name + "/" + instanceId;
				}
				else
				{
					nameClashHash.Add(name, name);
					return name;
				}
			}
		}

		internal TypeWrapper LoadClassByDottedName(string name)
		{
			TypeWrapper type = LoadClassByDottedNameFast(name, true);
			if(type != null)
			{
				return type;
			}
			throw new ClassNotFoundException(name);
		}

		internal TypeWrapper LoadClassByDottedNameFast(string name)
		{
			return LoadClassByDottedNameFast(name, false);
		}

		private TypeWrapper LoadClassByDottedNameFast(string name, bool throwClassNotFoundException)
		{
			// .NET 1.1 has a limit of 1024 characters for type names
			if(name.Length >= 1024)
			{
				return null;
			}
			Profiler.Enter("LoadClassByDottedName");
			try
			{
				TypeWrapper type;
				lock(types.SyncRoot)
				{
					type = (TypeWrapper)types[name];
					if(type == null && types.ContainsKey(name))
					{
						// NOTE this can also happen if we (incorrectly) trigger a load of this class during
						// the loading of the base class, so we print a trace message here.
						Tracer.Error(Tracer.ClassLoading, "**** ClassCircularityError: {0} ****", name);
						throw new ClassCircularityError(name);
					}
				}
				if(type != null)
				{
					return type;
				}
				if(name.Length > 1 && name[0] == '[')
				{
					int dims = 1;
					while(name[dims] == '[')
					{
						dims++;
					}
					if(name[dims] == 'L')
					{
						if(!name.EndsWith(";") || name.Length <= dims + 2 || name[dims + 1] == '[')
						{
							// malformed class name
							return null;
						}
						string elemClass = name.Substring(dims + 1, name.Length - dims - 2);
						type = LoadClassByDottedNameFast(elemClass);
						if(type != null)
						{
							type = type.GetClassLoader().CreateArrayType(name, type, dims);
						}
						return RegisterInitiatingLoader(type);
					}
					if(name.Length != dims + 1)
					{
						// malformed class name
						return null;
					}
					switch(name[dims])
					{
						case 'B':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BYTE, dims));
						case 'C':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.CHAR, dims));
						case 'D':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.DOUBLE, dims));
						case 'F':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.FLOAT, dims));
						case 'I':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.INT, dims));
						case 'J':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.LONG, dims));
						case 'S':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.SHORT, dims));
						case 'Z':
							return RegisterInitiatingLoader(GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BOOLEAN, dims));
						default:
							return null;
					}
				}
				if(this == GetBootstrapClassLoader())
				{
					// HACK if the name contains a comma, we assume it is an assembly qualified name
					if(name.IndexOf(',') != -1)
					{
						// NOTE even though we search all loaded assemblies below, we still need to do this,
						// because this call might actually trigger the load of an assembly.
						Type t = Type.GetType(name);
						if(t == null)
						{
							// HACK we explicitly try all loaded assemblies, to support assemblies
							// that aren't loaded in the "Load" context.
							string typeName = name.Substring(0, name.IndexOf(','));
							foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
							{
								t = asm.GetType(typeName);
								if(t != null && t.AssemblyQualifiedName == name)
								{
									break;
								}
								t = null;
							}
						}
						if(t != null)
						{
							if(t.Module.IsDefined(typeof(JavaModuleAttribute), false))
							{
								return RegisterInitiatingLoader(GetWrapperFromType(t));
							}
							else
							{
								// HACK weird way to load the .NET type wrapper that always works
								// (for remapped types as well, because netexp uses this way of
								// loading types, we need the remapped types to appear in their
								// .NET "warped" form).
								return LoadClassByDottedNameFast(DotNetTypeWrapper.GetName(t), throwClassNotFoundException);
							}
						}
					}
					// TODO why is this check here and not at the top of the method?
					if(name != "")
					{
						Type t = GetBootstrapTypeRaw(name);
						if(t != null)
						{
							return RegisterInitiatingLoader(GetWrapperFromBootstrapType(t));
						}
						type = DotNetTypeWrapper.CreateDotNetTypeWrapper(name);
						if(type != null)
						{
							Debug.Assert(type.Name == name, type.Name + " != " + name);
							lock(types.SyncRoot)
							{
								// another thread may have beaten us to it and in that
								// case we don't want to overwrite the previous one
								TypeWrapper race = (TypeWrapper)types[name];
								if(race == null)
								{
									types[name] = type;
								}
								else
								{
									type = race;
								}
							}
							return type;
						}
						// NOTE it is important that this is done last, because otherwise we will
						// load the netexp generated fake types (e.g. delegate inner interface) instead
						// of having DotNetTypeWrapper generating it.
						type = GetTypeWrapperCompilerHook(name);
						if(type != null)
						{
							return type;
						}
					}
					if(javaClassLoader == null)
					{
						return null;
					}
				}
				// NOTE just like Java does (I think), we take the classloader lock before calling the loadClass method
				lock(javaClassLoader)
				{
					Profiler.Enter("ClassLoader.loadClass");
					try
					{
						type = (TypeWrapper)JVM.Library.loadClass(javaClassLoader, name);
					}
					catch(Exception x)
					{
						if(!throwClassNotFoundException
							&& LoadClassCritical("java.lang.ClassNotFoundException").TypeAsBaseType.IsInstanceOfType(x))
						{
							return null;
						}
						throw new ClassLoadingException(IKVM.Runtime.Util.MapException(x));
					}
					finally
					{
						Profiler.Leave("ClassLoader.loadClass");
					}
					// NOTE to be safe, we register the initiating loader,
					// while we're holding the lock on the class loader object
					return RegisterInitiatingLoader(type);
				}
			}
			finally
			{
				Profiler.Leave("LoadClassByDottedName");
			}
		}

		private TypeWrapper GetWrapperFromBootstrapType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromBootstrapType: {0}", type.FullName);
			Debug.Assert(GetWrapperFromTypeFast(type) == null, "GetWrapperFromTypeFast(type) == null", type.FullName);
			Debug.Assert(!type.IsArray, "!type.IsArray", type.FullName);
			Debug.Assert(!(type.Assembly is AssemblyBuilder), "!(type.Assembly is AssemblyBuilder)", type.FullName);
			// only the bootstrap classloader can own compiled types
			Debug.Assert(this == GetBootstrapClassLoader(), "this == GetBootstrapClassLoader()", type.FullName);
			bool javaType = type.Module.IsDefined(typeof(JavaModuleAttribute), false);
			string name;
			if(javaType)
			{
				name = CompiledTypeWrapper.GetName(type);
			}
			else
			{
				name = DotNetTypeWrapper.GetName(type);
			}
			TypeWrapper wrapper;
			lock(types.SyncRoot)
			{
				wrapper = (TypeWrapper)types[name];
			}
			if(wrapper != null)
			{
				if(wrapper.TypeAsTBD != type)
				{
					string msg = String.Format("\nTypename \"{0}\" is imported from multiple assemblies:\n{1}\n{2}\n", type.FullName, wrapper.Assembly.FullName, type.Assembly.FullName);
					JVM.CriticalFailure(msg, null);
				}
			}
			else
			{
				if(javaType)
				{
					// since this type was compiled from Java source, we have to look for our
					// attributes
					wrapper = CompiledTypeWrapper.newInstance(name, type);
				}
				else
				{
					// since this type was not compiled from Java source, we don't need to
					// look for our attributes, but we do need to filter unrepresentable
					// stuff (and transform some other stuff)
					wrapper = new DotNetTypeWrapper(type);
				}
				Debug.Assert(wrapper.Name == name, "wrapper.Name == name", type.FullName);
				lock(types.SyncRoot)
				{
					// another thread may have beaten us to it and in that
					// case we don't want to overwrite the previous one
					TypeWrapper race = (TypeWrapper)types[name];
					if(race == null)
					{
						types.Add(name, wrapper);
						typeToTypeWrapper.Add(type, wrapper);
					}
					else
					{
						wrapper = race;
					}
				}
			}
			return wrapper;
		}

		// NOTE this method only sees pre-compiled Java classes
		internal Type GetBootstrapTypeRaw(string name)
		{
			foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				if(!(a is AssemblyBuilder))
				{
					Type t = GetJavaTypeFromAssembly(a, name);
					if(t != null)
					{
						return t;
					}
				}
			}
			return null;
		}

		private static Type GetJavaTypeFromAssembly(Assembly a, string name)
		{
			try
			{
				Type t = a.GetType(name);
				if(t != null
					&& t.Module.IsDefined(typeof(JavaModuleAttribute), false)
					&& !AttributeHelper.IsHideFromJava(t))
				{
					return t;
				}
				// HACK we might be looking for an inner classes
				t = a.GetType(name.Replace('$', '+'));
				if(t != null
					&& t.Module.IsDefined(typeof(JavaModuleAttribute), false)
					&& !AttributeHelper.IsHideFromJava(t))
				{
					return t;
				}
			}
			catch(ArgumentException x)
			{
				// we can end up here because we replace the $ with a plus sign
				// (or client code did a Class.forName() on an invalid name)
				Tracer.Info(Tracer.Runtime, x.Message);
			}
			return null;
		}

		internal virtual TypeWrapper GetTypeWrapperCompilerHook(string name)
		{
			return null;
		}

		// NOTE this method can actually return null if the resulting array type name would be too long
		// for .NET to handle.
		private TypeWrapper CreateArrayType(string name, TypeWrapper elementTypeWrapper, int dims)
		{
			Debug.Assert(new String('[', dims) + elementTypeWrapper.SigName == name);
			Debug.Assert(!elementTypeWrapper.IsUnloadable && !elementTypeWrapper.IsVerifierType && !elementTypeWrapper.IsArray);
			Debug.Assert(dims >= 1);
			Type elementType = elementTypeWrapper.TypeAsArrayType;
			TypeWrapper wrapper;
			lock(types.SyncRoot)
			{
				wrapper = (TypeWrapper)types[name];
			}
			if(wrapper == null)
			{
				String netname = elementType.FullName + "[]";
				for(int i = 1; i < dims; i++)
				{
					netname += "[]";
				}
				// .NET 1.1 has a limit of 1024 characters for type names
				if(netname.Length >= 1024)
				{
					return null;
				}
				Type array;
				if(elementType.Module is ModuleBuilder)
				{
					// FXBUG ModuleBuilder.GetType() is broken (I think), it fires a TypeResolveEvent when
					// you try to construct an array type from an unfinished type. I don't think it should
					// do that. We have to work around that by setting a global flag (yuck) to prevent us
					// from responding to the TypeResolveEvent.
					lock(arrayConstructionLock)
					{
						arrayConstructionHack = true;
						try
						{
							array = ((ModuleBuilder)elementType.Module).GetType(netname);
						}
						finally
						{
							arrayConstructionHack = false;
						}
					}
				}
				else
				{
					array = elementType.Assembly.GetType(netname, true);
				}
				Modifiers modifiers = Modifiers.Final | Modifiers.Abstract;
				Modifiers reflectiveModifiers = modifiers;
				modifiers |= elementTypeWrapper.Modifiers & Modifiers.Public;
				reflectiveModifiers |= elementTypeWrapper.ReflectiveModifiers & Modifiers.AccessMask;
				wrapper = new ArrayTypeWrapper(array, modifiers, reflectiveModifiers, name, this);
				lock(types.SyncRoot)
				{
					// another thread may have beaten us to it and in that
					// case we don't want to overwrite the previous one
					TypeWrapper race = (TypeWrapper)types[name];
					if(race == null)
					{
						types.Add(name, wrapper);
						if(!(elementType is TypeBuilder) && !wrapper.IsGhostArray)
						{
							Debug.Assert(!typeToTypeWrapper.ContainsKey(array), name);
							typeToTypeWrapper.Add(array, wrapper);
						}
					}
					else
					{
						wrapper = race;
					}
				}
			}
			return wrapper;
		}

		internal TypeWrapper DefineClass(ClassFile f, object protectionDomain)
		{
			string dotnetAssembly = f.IKVMAssemblyAttribute;
			if(dotnetAssembly != null)
			{
				// HACK only the bootstrap classloader can define .NET types (but for convenience, we do
				// allow other class loaders to call DefineClass for them)
				// TODO reconsider this, it might be a better idea to only allow netexp generated jars on the bootclasspath
				return GetBootstrapClassLoader().DefineNetExpType(f.Name, dotnetAssembly);
			}
			lock(types.SyncRoot)
			{
				if(types.ContainsKey(f.Name))
				{
					throw new LinkageError("duplicate class definition: " + f.Name);
				}
				// mark the type as "loading in progress", so that we can detect circular dependencies.
				types.Add(f.Name, null);
			}
			try
			{
				TypeWrapper type = CreateDynamicTypeWrapper(f, this, protectionDomain);
				lock(types.SyncRoot)
				{
					// in very extreme conditions another thread may have beaten us to it
					// and loaded a class with the same name, in that case we'll leak
					// the defined DynamicTypeWrapper (or rather the Reflection.Emit
					// defined type).
					TypeWrapper race = (TypeWrapper)types[f.Name];
					if(race == null)
					{
						Debug.Assert(!dynamicTypes.ContainsKey(type.TypeAsTBD.FullName));
						dynamicTypes.Add(type.TypeAsTBD.FullName, type);
						types[f.Name] = type;
					}
					else
					{
						throw new LinkageError("duplicate class definition: " + f.Name);
					}
				}
				return type;
			}
			catch
			{
				lock(types.SyncRoot)
				{
					if(types[f.Name] == null)
					{
						// if loading the class fails, we remove the indicator that we're busy loading the class,
						// because otherwise we get a ClassCircularityError if we try to load the class again.
						types.Remove(f.Name);
					}
				}
				throw;
			}
		}

		protected virtual TypeWrapper CreateDynamicTypeWrapper(ClassFile f, ClassLoaderWrapper loader, object protectionDomain)
		{
			return new DynamicTypeWrapper(f, loader, protectionDomain);
		}

		private TypeWrapper DefineNetExpType(string name, string assemblyName)
		{
			Debug.Assert(this == GetBootstrapClassLoader());
			TypeWrapper type;
			lock(types.SyncRoot)
			{
				// we need to check if we've already got it, because other classloaders than the bootstrap classloader may
				// "define" NetExp types, there is a potential race condition if multiple classloaders try to define the
				// same type simultaneously.
				type = (TypeWrapper)types[name];
				if(type != null)
				{
					return type;
				}
			}
			// The sole purpose of the netexp class is to let us load the assembly that the class lives in,
			// once we've done that, all types in it become visible.
			Assembly asm;
			try
			{
				asm = Assembly.Load(assemblyName);
			}
			catch(Exception x)
			{
				throw new NoClassDefFoundError(name + " (" + x.Message + ")");
			}
			// pre-compiled Java types can also live in a netexp referenced assembly,
			// so we have to explicitly check for those
			// (DotNetTypeWrapper.CreateDotNetTypeWrapper will refuse to return Java types).
			Type t = GetJavaTypeFromAssembly(asm, name);
			if(t != null)
			{
				return GetWrapperFromBootstrapType(t);
			}
			type = DotNetTypeWrapper.CreateDotNetTypeWrapper(name);
			if(type == null)
			{
				throw new NoClassDefFoundError(name + " not found in " + assemblyName);
			}
			lock(types.SyncRoot)
			{
				TypeWrapper race = (TypeWrapper)types[name];
				if(race == null)
				{
					types.Add(name, type);
				}
				else
				{
					type = race;
				}
			}
			return type;
		}

		internal object GetJavaClassLoader()
		{
			return (this == GetBootstrapClassLoader()) ? null : javaClassLoader;
		}

		// When -Xbootclasspath is specified, we use a URLClassLoader as an
		// additional bootstrap class loader (this is not visible to the Java code).
		// We need to access this to be able to load resources.
		internal static object GetJavaBootstrapClassLoader()
		{
			return GetBootstrapClassLoader().javaClassLoader;
		}

		internal static void PrepareForSaveDebugImage()
		{
			Debug.Assert(moduleBuilder == null);
			saveDebugImage = true;
		}

		internal static bool IsSaveDebugImage
		{
			get
			{
				return saveDebugImage;
			}
		}

		internal static void FinishAll()
		{
			JVM.FinishingForDebugSave = true;
			while(dynamicTypes.Count > 0)
			{
				ArrayList l = new ArrayList(dynamicTypes.Values);
				foreach(TypeWrapper tw in l)
				{
					string name = tw.TypeAsTBD.FullName;
					Tracer.Info(Tracer.Runtime, "Finishing {0}", name);
					tw.Finish();
					dynamicTypes.Remove(name);
				}
			}
		}

		internal static void SaveDebugImage(object mainClass)
		{
			FinishAll();
			// HACK use reflection to get the type from the class
			TypeWrapper mainTypeWrapper = IKVM.NativeCode.java.lang.VMClass.getWrapperFromClass(mainClass);
			mainTypeWrapper.Finish();
			Type mainType = mainTypeWrapper.TypeAsTBD;
			MethodInfo main = mainType.GetMethod("main", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(string[]) }, null);
			AssemblyBuilder asm = ((AssemblyBuilder)moduleBuilder.Assembly);
			asm.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
			asm.Save("ikvmdump.exe");
			if(saveDebugAssemblies != null)
			{
				foreach(AssemblyBuilder ab in saveDebugAssemblies)
				{
					ab.Save(ab.GetName().Name + ".dll");
				}
			}
		}

		internal static void RegisterForSaveDebug(AssemblyBuilder ab)
		{
			if(saveDebugAssemblies == null)
			{
				saveDebugAssemblies = new ArrayList();
			}
			saveDebugAssemblies.Add(ab);
		}

		internal ModuleBuilder ModuleBuilder
		{
			get
			{
				lock(this)
				{
					if(moduleBuilder == null)
					{
						moduleBuilder = CreateModuleBuilder();
					}
					return moduleBuilder;
				}
			}
		}

		protected virtual ModuleBuilder CreateModuleBuilder()
		{
			AssemblyName name = new AssemblyName();
			if(saveDebugImage)
			{
				name.Name = "ikvmdump";
			}
			else
			{
				name.Name = "ikvm_dynamic_assembly__" + instanceId + "__" + (uint)Environment.TickCount;
			}
			DateTime now = DateTime.Now;
			name.Version = new Version(now.Year, (now.Month * 100) + now.Day, (now.Hour * 100) + now.Minute, (now.Second * 1000) + now.Millisecond);
			AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, saveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run, null, null, null, null, null, true);
			CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, JVM.Debug });
			assemblyBuilder.SetCustomAttribute(debugAttr);
			return saveDebugImage ? assemblyBuilder.DefineDynamicModule("ikvmdump.exe", "ikvmdump.exe", JVM.Debug) : assemblyBuilder.DefineDynamicModule(name.Name, JVM.Debug);
		}

		internal TypeWrapper ExpressionTypeWrapper(string type)
		{
			Debug.Assert(!type.StartsWith("Lret;"));
			Debug.Assert(type != "Lnull");

			int index = 0;
			return SigDecoderWrapper(ref index, type);
		}

		// NOTE this exposes potentially unfinished types
		internal Type[] ArgTypeListFromSig(string sig)
		{
			if(sig[1] == ')')
			{
				return Type.EmptyTypes;
			}
			TypeWrapper[] wrappers = ArgTypeWrapperListFromSig(sig);
			Type[] types = new Type[wrappers.Length];
			for(int i = 0; i < wrappers.Length; i++)
			{
				types[i] = wrappers[i].TypeAsSignatureType;
			}
			return types;
		}

		// NOTE: this will ignore anything following the sig marker (so that it can be used to decode method signatures)
		private TypeWrapper SigDecoderWrapper(ref int index, string sig)
		{
			switch(sig[index++])
			{
				case 'B':
					return PrimitiveTypeWrapper.BYTE;
				case 'C':
					return PrimitiveTypeWrapper.CHAR;
				case 'D':
					return PrimitiveTypeWrapper.DOUBLE;
				case 'F':
					return PrimitiveTypeWrapper.FLOAT;
				case 'I':
					return PrimitiveTypeWrapper.INT;
				case 'J':
					return PrimitiveTypeWrapper.LONG;
				case 'L':
				{
					int pos = index;
					index = sig.IndexOf(';', index) + 1;
					return LoadClassByDottedName(sig.Substring(pos, index - pos - 1));
				}
				case 'S':
					return PrimitiveTypeWrapper.SHORT;
				case 'Z':
					return PrimitiveTypeWrapper.BOOLEAN;
				case 'V':
					return PrimitiveTypeWrapper.VOID;
				case '[':
				{
					// TODO this can be optimized
					string array = "[";
					while(sig[index] == '[')
					{
						index++;
						array += "[";
					}
					switch(sig[index])
					{
						case 'L':
						{
							int pos = index;
							index = sig.IndexOf(';', index) + 1;
							return LoadClassByDottedName(array + sig.Substring(pos, index - pos));
						}
						case 'B':
						case 'C':
						case 'D':
						case 'F':
						case 'I':
						case 'J':
						case 'S':
						case 'Z':
							return LoadClassByDottedName(array + sig[index++]);
						default:
							throw new InvalidOperationException(sig.Substring(index));
					}
				}
				default:
					throw new InvalidOperationException(sig.Substring(index));
			}
		}

		internal TypeWrapper FieldTypeWrapperFromSig(string sig)
		{
			int index = 0;
			return SigDecoderWrapper(ref index, sig);
		}

		internal TypeWrapper RetTypeWrapperFromSig(string sig)
		{
			int index = sig.IndexOf(')') + 1;
			return SigDecoderWrapper(ref index, sig);
		}

		internal TypeWrapper[] ArgTypeWrapperListFromSig(string sig)
		{
			if(sig[1] == ')')
			{
				return TypeWrapper.EmptyArray;
			}
			ArrayList list = new ArrayList();
			for(int i = 1; sig[i] != ')';)
			{
				list.Add(SigDecoderWrapper(ref i, sig));
			}
			TypeWrapper[] types = new TypeWrapper[list.Count];
			list.CopyTo(types);
			return types;
		}

		internal static ClassLoaderWrapper GetBootstrapClassLoader()
		{
			lock(typeof(ClassLoaderWrapper))
			{
				if(bootstrapClassLoader == null)
				{
					bootstrapClassLoader = new ClassLoaderWrapper(null);
				}
				return bootstrapClassLoader;
			}
		}

		internal static ClassLoaderWrapper GetSystemClassLoader()
		{
			// during static compilation, we don't have a system class loader
			if(JVM.IsStaticCompiler)
			{
				return GetBootstrapClassLoader();
			}
			if(systemClassLoader == null)
			{
				systemClassLoader = GetClassLoaderWrapper(JVM.Library.getSystemClassLoader());
			}
			return systemClassLoader;
		}
	
		internal static ClassLoaderWrapper GetClassLoaderWrapper(object javaClassLoader)
		{
			if(javaClassLoader == null || GetBootstrapClassLoader().javaClassLoader == javaClassLoader)
			{
				return GetBootstrapClassLoader();
			}
			lock(javaClassLoaderToClassLoaderWrapper.SyncRoot)
			{
				ClassLoaderWrapper wrapper = (ClassLoaderWrapper)javaClassLoaderToClassLoaderWrapper[javaClassLoader];
				if(wrapper == null)
				{
					wrapper = new ClassLoaderWrapper(javaClassLoader);
					javaClassLoaderToClassLoaderWrapper[javaClassLoader] = wrapper;
				}
				return wrapper;
			}
		}

		// This only returns the wrapper for a Type if that wrapper has already been created, otherwise
		// it returns null
		// If the wrapper doesn't exist, that means that the type is either a .NET type or a pre-compiled Java class
		internal static TypeWrapper GetWrapperFromTypeFast(Type type)
		{
			TypeWrapper.AssertFinished(type);
			TypeWrapper wrapper = (TypeWrapper)typeToTypeWrapper[type];
			if(wrapper == null)
			{
				string name = (string)remappedTypes[type];
				if(name != null)
				{
					return LoadClassCritical(name);
				}
			}
			return wrapper;
		}

		internal static TypeWrapper GetWrapperFromType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromType: {0}", type.AssemblyQualifiedName);
			TypeWrapper.AssertFinished(type);
			TypeWrapper wrapper = GetWrapperFromTypeFast(type);
			if(wrapper == null)
			{
				Debug.Assert(type != typeof(object) && type != typeof(string));
				if(type.IsArray)
				{
					// it might be an array of a dynamically compiled Java type
					int rank = 1;
					Type elem = type.GetElementType();
					while(elem.IsArray)
					{
						rank++;
						elem = elem.GetElementType();
					}
					wrapper = GetWrapperFromType(elem);
					return wrapper.MakeArrayType(rank);
				}
				// if the wrapper doesn't already exist, that must mean that the type
				// is a .NET type (or a pre-compiled Java class), which means that it
				// was "loaded" by the bootstrap classloader
				// TODO think up a scheme to deal with .NET types that have the same name. Since all .NET types
				// appear in the boostrap classloader, we need to devise a scheme to mangle the class name
				return GetBootstrapClassLoader().GetWrapperFromBootstrapType(type);
			}
			return wrapper;
		}

		internal static void SetWrapperForType(Type type, TypeWrapper wrapper)
		{
			TypeWrapper.AssertFinished(type);
			Debug.Assert(!typeToTypeWrapper.ContainsKey(type));
			typeToTypeWrapper.Add(type, wrapper);
		}

		internal static void PublishLibraryImplementationHelperType(Type type)
		{
			CompiledTypeWrapper typeWrapper = CompiledTypeWrapper.newInstance(type.FullName, type);
			SetWrapperForType(type, typeWrapper);
			GetBootstrapClassLoader().types[type.FullName] = typeWrapper;
		}

		internal static TypeWrapper LoadClassCritical(string name)
		{
			try
			{
				return GetBootstrapClassLoader().LoadClassByDottedName(name);
			}
			catch(Exception x)
			{
				JVM.CriticalFailure("Loading of critical class failed", x);
				return null;
			}
		}

		internal void RegisterNativeLibrary(IntPtr p)
		{
			lock(this)
			{
				if(nativeLibraries == null)
				{
					nativeLibraries = new ArrayList();
				}
				nativeLibraries.Add(p);
			}
		}

		internal IntPtr[] GetNativeLibraries()
		{
			lock(this)
			{
				if(nativeLibraries ==  null)
				{
					return new IntPtr[0];
				}
				return (IntPtr[])nativeLibraries.ToArray(typeof(IntPtr));
			}
		}

		public override string ToString()
		{
			if(javaClassLoader == null)
			{
				return "null";
			}
			return String.Format("{0}@{1:X}", GetWrapperFromType(javaClassLoader.GetType()).Name, javaClassLoader.GetHashCode());
		}
	}
}
