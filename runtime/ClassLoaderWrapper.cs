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
using System.Reflection.Emit;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;

class ClassLoaderWrapper
{
	private delegate object LoadClassDelegate(object classLoader, string className);
	private static LoadClassDelegate loadClassDelegate;
	private static bool arrayConstructionHack;
	private static object arrayConstructionLock = new object();
	private static Hashtable javaClassLoaderToClassLoaderWrapper = new Hashtable();
	private static Hashtable dynamicTypes = Hashtable.Synchronized(new Hashtable());
	// TODO typeToTypeWrapper should be an identity hashtable
	private static Hashtable typeToTypeWrapper = Hashtable.Synchronized(new Hashtable());
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

		// HACK we need to find the "core" library, to figure out the remapped types
		// TODO this approach fails if the core library was compiled as a module (ikvmc always generates an assembly
		// and the assembly attributes end up on the assemblies main module that is deleted when ikvmc finishes)
		foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
		{
			object[] remapped = asm.GetCustomAttributes(typeof(RemappedClassAttribute), false);
			if(remapped.Length > 0)
			{
				coreAssembly = asm;
				foreach(RemappedClassAttribute r in remapped)
				{
					Tracer.Info(Tracer.Runtime, "Remapping type {0} to {1}", r.RemappedType, r.Name);
					remappedTypes.Add(r.RemappedType, r.Name);
				}
				break;
			}
		}
		if(coreAssembly == null)
		{
			Tracer.Info(Tracer.Compiler, "Unable to find core library");
			if(!JVM.IsStaticCompiler)
			{
				JVM.CriticalFailure("Unable to find core library", null);
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
		if(javaClassLoader != null)
		{
			if(loadClassDelegate == null)
			{
				TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical("java.lang.VMClass");
				tw.Finish();
				loadClassDelegate = (LoadClassDelegate)Delegate.CreateDelegate(typeof(LoadClassDelegate), tw.TypeAsTBD, "loadClassHelper");
			}
		}
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
	// (this exists solely for DynamicTypeWrapper.SetupGhosts)
	internal TypeWrapper GetLoadedClass(string name)
	{
		lock(types.SyncRoot)
		{
			return (TypeWrapper)types[name];
		}
	}

	// FXBUG This mangles type names, to enable different class loaders loading classes with the same names.
	// We used to support this by using an assembly per class loader instance, but because
	// of the CLR TypeResolve bug, we put all types in a single assembly for now.
	internal string MangleTypeName(string name)
	{
		lock(nameClashHash.SyncRoot)
		{
			if(nameClashHash.ContainsKey(name))
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
		TypeWrapper type = LoadClassByDottedNameFast(name);
		if(type != null)
		{
			return type;
		}
		throw new ClassNotFoundException(name);
	}

	internal TypeWrapper LoadClassByDottedNameFast(string name)
	{
		if(name == null)
		{
			throw new NullReferenceException();
		}
		Profiler.Enter("LoadClassByDottedName");
		try
		{
			TypeWrapper type;
			lock(types.SyncRoot)
			{
				type = (TypeWrapper)types[name];
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
				switch(name[dims])
				{
					case 'L':
					{
						type = LoadClassByDottedNameFast(name.Substring(dims + 1, name.IndexOf(';', dims) - dims - 1));
						if(type != null)
						{
							type = type.GetClassLoader().CreateArrayType(name, type.TypeAsArrayType, dims);
						}
						return type;
					}
					case 'B':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BYTE.TypeAsArrayType, dims);
					case 'C':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.CHAR.TypeAsArrayType, dims);
					case 'D':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.DOUBLE.TypeAsArrayType, dims);
					case 'F':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.FLOAT.TypeAsArrayType, dims);
					case 'I':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.INT.TypeAsArrayType, dims);
					case 'J':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.LONG.TypeAsArrayType, dims);
					case 'S':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.SHORT.TypeAsArrayType, dims);
					case 'Z':
						return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BOOLEAN.TypeAsArrayType, dims);
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
							return GetWrapperFromType(t);
						}
						else
						{
							// HACK weird way to load the .NET type wrapper that always works
							// (for remapped types as well, because netexp uses this way of
							// loading types, we need the remapped types to appear in their
							// .NET "warped" form).
							return LoadClassByDottedName(DotNetTypeWrapper.GetName(t));
						}
					}
				}
				// TODO why is this check here and not at the top of the method?
				if(name != "")
				{
					Type t = GetBootstrapTypeRaw(name);
					if(t != null)
					{
						return GetWrapperFromBootstrapType(t);
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
					type = (TypeWrapper)loadClassDelegate(javaClassLoader, name);
				}
				finally
				{
					Profiler.Leave("ClassLoader.loadClass");
				}
			}
			// NOTE we're caching types loaded by parent classloaders as well!
			// TODO this isn't correct, but it gives us a huge perf gain,
			// if we don't do this, Eclipse startup time goes from 52 seconds to 1 minute 30 seconds!
			if(type.GetClassLoader() != this)
			{
				lock(types.SyncRoot)
				{
					if(!types.ContainsKey(name))
					{
						types.Add(name, type);
					}
				}
			}
			return type;
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
		TypeWrapper wrapper = null;
		if(type.Module.IsDefined(typeof(JavaModuleAttribute), false))
		{
			lock(types.SyncRoot)
			{
				string name = CompiledTypeWrapper.GetName(type);
				wrapper = (TypeWrapper)types[name];
				if(wrapper == null)
				{
					// since this type was compiled from Java source, we have to look for our
					// attributes
					wrapper = new CompiledTypeWrapper(name, type);
					Debug.Assert(wrapper.Name == name, "wrapper.Name == name", type.FullName);
					Debug.Assert(!types.ContainsKey(wrapper.Name), wrapper.Name, type.FullName);
					types.Add(wrapper.Name, wrapper);
					Debug.Assert(!typeToTypeWrapper.ContainsKey(type), "!typeToTypeWrapper.ContainsKey(type)", type.FullName);
					typeToTypeWrapper.Add(type, wrapper);
				}
			}
		}
		else
		{
			lock(types.SyncRoot)
			{
				string name = DotNetTypeWrapper.GetName(type);
				wrapper = (TypeWrapper)types[name];
				if(wrapper == null)
				{
					// since this type was not compiled from Java source, we don't need to
					// look for our attributes, but we do need to filter unrepresentable
					// stuff (and transform some other stuff)
					wrapper = new DotNetTypeWrapper(type);
					Debug.Assert(wrapper.Name == name, "wrapper.Name == name", type.FullName);
					Debug.Assert(!types.ContainsKey(wrapper.Name), wrapper.Name, type.FullName);
					types.Add(wrapper.Name, wrapper);
					Debug.Assert(!typeToTypeWrapper.ContainsKey(type), "!typeToTypeWrapper.ContainsKey(type)", type.FullName);
					typeToTypeWrapper.Add(type, wrapper);
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
				Type t = a.GetType(name);
				if(t != null && t.Module.IsDefined(typeof(JavaModuleAttribute), false))
				{
					return t;
				}
				// HACK we might be looking for an inner classes
				t = a.GetType(name.Replace('$', '+'));
				if(t != null && t.Module.IsDefined(typeof(JavaModuleAttribute), false))
				{
					return t;
				}
			}
		}
		return null;
	}

	internal virtual TypeWrapper GetTypeWrapperCompilerHook(string name)
	{
		return null;
	}

	private TypeWrapper CreateArrayType(string name, Type elementType, int dims)
	{
		Debug.Assert(!elementType.IsArray);
		lock(types.SyncRoot)
		{
			TypeWrapper wrapper = (TypeWrapper)types[name];
			if(wrapper == null)
			{
				String netname = "[]";
				for(int i = 1; i < dims; i++)
				{
					netname += "[]";
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
							array = ((ModuleBuilder)elementType.Module).GetType(elementType.FullName + netname);
						}
						finally
						{
							arrayConstructionHack = false;
						}
					}
				}
				else
				{
					array = elementType.Assembly.GetType(elementType.FullName + netname, true);
				}
				Modifiers modifiers = Modifiers.Final | Modifiers.Abstract;
				// TODO taking the visibility from the .NET isn't 100% correct, we really should look at the wrapper
				if(DotNetTypeWrapper.IsVisible(elementType))
				{
					modifiers |= Modifiers.Public;
				}
				wrapper = new ArrayTypeWrapper(array, modifiers, name, this);
				Debug.Assert(!types.ContainsKey(name));
				types.Add(name, wrapper);
				if(!(elementType is TypeBuilder) && !wrapper.IsGhostArray)
				{
					Debug.Assert(!typeToTypeWrapper.ContainsKey(array), name);
					typeToTypeWrapper.Add(array, wrapper);
				}
			}
			return wrapper;
		}
	}

	internal TypeWrapper DefineClass(ClassFile f)
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
				if(types[f.Name] == null)
				{
					// NOTE this can also happen if we (incorrectly) trigger a load of this class during
					// the loading of the base class, so we print a warning here.
					Tracer.Warning(Tracer.ClassLoading, "**** ClassCircularityError: {0} ****", f.Name);
					throw new ClassCircularityError(f.Name);
				}
				throw new LinkageError("duplicate class definition: " + f.Name);
			}
			// mark the type as "loading in progress", so that we can detect circular dependencies.
			types.Add(f.Name, null);
			try
			{
				TypeWrapper type = new DynamicTypeWrapper(f, this);
				Debug.Assert(!dynamicTypes.ContainsKey(type.TypeAsTBD.FullName));
				dynamicTypes.Add(type.TypeAsTBD.FullName, type);
				Debug.Assert(types[f.Name] == null);
				types[f.Name] = type;
				return type;
			}
			catch
			{
				if(types[f.Name] == null)
				{
					// if loading the class fails, we remove the indicator that we're busy loading the class,
					// because otherwise we get a ClassCircularityError if we try to load the class again.
					types.Remove(f.Name);
				}
				throw;
			}
		}
	}

	private TypeWrapper DefineNetExpType(string name, string assembly)
	{
		Debug.Assert(this == GetBootstrapClassLoader());
		lock(types.SyncRoot)
		{
			// we need to check if we've already got it, because other classloaders than the bootstrap classloader may
			// "define" NetExp types, there is a potential race condition if multiple classloaders try to define the
			// same type simultaneously.
			TypeWrapper type = (TypeWrapper)types[name];
			if(type != null)
			{
				return type;
			}
			// The sole purpose of the netexp class is to let us load the assembly that the class lives in,
			// once we've done that, all types in it become visible.
			try
			{
				Assembly.Load(assembly);
			}
			catch(Exception x)
			{
				throw new NoClassDefFoundError(name + " (" + x.Message + ")");
			}
			type = DotNetTypeWrapper.CreateDotNetTypeWrapper(name);
			if(type == null)
			{
				throw new NoClassDefFoundError(name + " not found in " + assembly);
			}
			types.Add(name, type);
			return type;
		}
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
		while(dynamicTypes.Count > 0)
		{
			ArrayList l = new ArrayList(dynamicTypes.Values);
			foreach(TypeWrapper tw in l)
			{
				Tracer.Info(Tracer.Runtime, "Finishing {0} for debug image", tw.TypeAsTBD.FullName);
				tw.Finish(true);
				dynamicTypes.Remove(tw.TypeAsTBD.FullName);
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
			name.Name = "ikvm_dynamic_assembly__" + (this == GetBootstrapClassLoader() ? "bootstrap" : javaClassLoader);
		}
		AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, saveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run, null, null, null, null, null, true);
		ModuleBuilder moduleBuilder = saveDebugImage ? assemblyBuilder.DefineDynamicModule(name.Name, "ikvmdump.exe", JVM.Debug) : assemblyBuilder.DefineDynamicModule(name.Name, JVM.Debug);
		CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, JVM.Debug });
		assemblyBuilder.SetCustomAttribute(debugAttr);
		return moduleBuilder;
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
			types[i] = wrappers[i].TypeAsParameterType;
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
		lock(typeof(ClassLoaderWrapper))
		{
			if(systemClassLoader == null)
			{
				TypeWrapper tw = LoadClassCritical("java.lang.System");
				// We directly access the systemClassLoader field, because calling ClassLoader.getSystemClassLoader
				// would cause a security check (and would require to be wrapped in a AccessController.doPriviledged()).
				FieldWrapper fw = tw.GetFieldWrapper("systemClassLoader", "Ljava.lang.ClassLoader;");
				systemClassLoader = GetClassLoaderWrapper(fw.GetValue(null));
			}
			return systemClassLoader;
		}
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
				// HACK BYTE[]
				//if(elem == typeof(byte))
				//{
				//	elem = typeof(sbyte);
				//}
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
}
