#define DEBUG
/*
  Copyright (C) 2002 Jeroen Frijters

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

class ClassLoaderWrapper
{
	private delegate object LoadClassDelegate(object classLoader, string className);
	private static LoadClassDelegate loadClassDelegate;
	private static bool arrayConstructionHack;
	private static ArrayList ikvmAssemblies = new ArrayList();
	private static Hashtable assemblyToClassLoaderWrapper = new Hashtable();
	private static Hashtable javaClassLoaderToClassLoaderWrapper = new Hashtable();
	private static ArrayList classLoaders = new ArrayList();
	private static Hashtable dynamicTypes = new Hashtable();
	// TODO typeToTypeWrapper should be an identity hashtable
	private static Hashtable typeToTypeWrapper = new Hashtable();
	private static ClassLoaderWrapper bootstrapClassLoader;
	private object javaClassLoader;
	private Hashtable types = new Hashtable();
	private Hashtable nativeMethods;
	// HACK moduleBuilder is static, because multiple dynamic assemblies is broken (TypeResolve doesn't fire)
	// so for the time being, we share one dynamic assembly among all classloaders
	private static ModuleBuilder moduleBuilder;

	// HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
	internal static void SetBootstrapClassLoader(ClassLoaderWrapper bootstrapClassLoader)
	{
		if(ClassLoaderWrapper.bootstrapClassLoader != null)
		{
			throw new InvalidOperationException();
		}
		ClassLoaderWrapper.bootstrapClassLoader = bootstrapClassLoader;
	}

	static ClassLoaderWrapper()
	{
		AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(OnTypeResolve);
		AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoad);
		foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			if(a.IsDefined(typeof(IKVMAssemblyAttribute), false) && !(a is AssemblyBuilder))
			{
				ikvmAssemblies.Add(a);
			}
		}
	}

	private static void OnAssemblyLoad(object sender, AssemblyLoadEventArgs e)
	{
		if(e.LoadedAssembly.IsDefined(typeof(IKVMAssemblyAttribute), false) && !(e.LoadedAssembly is AssemblyBuilder))
		{
			ikvmAssemblies.Add(e.LoadedAssembly);
		}
	}

	private static Assembly OnTypeResolve(object sender, ResolveEventArgs args)
	{
		//Console.WriteLine("OnTypeResolve: " + args.Name);
		if(arrayConstructionHack)
		{
			return null;
		}
		try
		{
			TypeWrapper type = (TypeWrapper)dynamicTypes[args.Name];
			if(type == null)
			{
				return null;
			}
			type.Finish();
			return type.Type.Assembly;
		}
		catch(Exception x)
		{
			// TODO don't catch the exception here... But, the problem is that Type.GetType() swallows all exceptions
			// that occur here, unless throwOnError is set, but in some (most?) cases you don't want the exception if it only
			// means that the class cannot be found...
			Console.WriteLine(x);
			Console.WriteLine(new StackTrace(true));
			throw;
		}
	}

	internal ClassLoaderWrapper(object javaClassLoader)
	{
		SetJavaClassLoader(javaClassLoader);
		classLoaders.Add(this);
	}

	internal void SetJavaClassLoader(object javaClassLoader)
	{
		this.javaClassLoader = javaClassLoader;
		if(javaClassLoader != null && loadClassDelegate == null)
		{
			loadClassDelegate = (LoadClassDelegate)Delegate.CreateDelegate(typeof(LoadClassDelegate), GetType("java.lang.Class"), "__loadClassHelper");
		}
	}

	internal void LoadRemappedTypes()
	{
		nativeMethods = new Hashtable();
		// NOTE interfaces have *not* java/lang/Object as the base type (even though they do in the class file)
		types["java.lang.Cloneable"] = new RemappedTypeWrapper(this, ModifiersAttribute.GetModifiers(typeof(java.lang.Cloneable)), "java/lang/Cloneable", typeof(java.lang.Cloneable), new TypeWrapper[0], null);
		typeToTypeWrapper.Add(typeof(java.lang.Cloneable), types["java.lang.Cloneable"]);
		types["java.io.Serializable"] = new RemappedTypeWrapper(this, ModifiersAttribute.GetModifiers(typeof(java.io.Serializable)), "java/io/Serializable", typeof(java.io.Serializable), new TypeWrapper[0], null);
		typeToTypeWrapper.Add(typeof(java.io.Serializable), types["java.io.Serializable"]);
		MapXml.Root map = MapXmlGenerator.Generate();
		foreach(MapXml.Class c in map.remappings)
		{
			TypeWrapper baseWrapper = null;
			// HACK need to resolve the base type or put it in the XML
			if(c.Type != "System.Object")
			{
				baseWrapper = (TypeWrapper)types["java.lang.Object"];
			}
			string name = c.Name;
			Modifiers modifiers = (Modifiers)c.Modifiers;
			Type type = Type.GetType(c.Type, true);
			if(type.IsInterface)
			{
				baseWrapper = null;
			}
			TypeWrapper tw = new RemappedTypeWrapper(this, modifiers, name.Replace('.', '/'), type, new TypeWrapper[0], baseWrapper);
			types.Add(name, tw);
			typeToTypeWrapper.Add(tw.Type, tw);
		}
		foreach(MapXml.Class c in map.remappings)
		{
			((RemappedTypeWrapper)types[c.Name]).LoadRemappings(c);
		}
		// native methods
		foreach(MapXml.Class c in map.nativeMethods)
		{
			string className = c.Name;
			foreach(MapXml.Method method in c.Methods)
			{
				string methodName = method.Name;
				string methodSig = method.Sig;
				nativeMethods[className + "." + methodName + methodSig] = method;
			}
		}
	}

	internal TypeWrapper LoadClassBySlashedName(string name)
	{
		// OPTIMIZE
		return LoadClassByDottedName(name.Replace('/', '.'));
	}

	// TODO implement vmspec 5.3.4 Loading Constraints
	internal TypeWrapper LoadClassByDottedName(string name)
	{
		if(name == null)
		{
			throw new NullReferenceException();
		}
		Profiler.Enter("LoadClassByDottedName");
		try
		{
			TypeWrapper type = (TypeWrapper)types[name];
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
						type = LoadClassByDottedName(name.Substring(dims + 1, name.IndexOf(';', dims) - dims - 1));
						type = type.GetClassLoader().CreateArrayType(name, type.Type, dims);
						return type;
					}
					case 'B':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(sbyte), dims);
					case 'C':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(char), dims);
					case 'D':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(double), dims);
					case 'F':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(float), dims);
					case 'I':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(int), dims);
					case 'J':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(long), dims);
					case 'S':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(short), dims);
					case 'Z':
						return GetBootstrapClassLoader().CreateArrayType(name, typeof(bool), dims);
					default:
						// TODO I'm not sure this is the right exception here (instead we could throw a NoClassDefFoundError)
						throw JavaException.ClassNotFoundException(name);
				}
			}
			if(this == GetBootstrapClassLoader())
			{
				// HACK if the name contains a comma, we assume it is an assembly qualified name
				if(name.IndexOf(',') != -1)
				{
					Type t = Type.GetType(name);
					if(t != null)
					{
						return GetCompiledTypeWrapper(t);
					}
				}
				if(name != "")
				{
					type = GetBootstrapType(name);
				}
				if(type != null)
				{
					return type;
				}
				if(javaClassLoader == null)
				{
					throw JavaException.ClassNotFoundException(name);
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
			// TODO not sure if this is correct
			if(type.GetClassLoader() != this)
			{
				if(types[name] != type)
				{
					types.Add(name, type);
				}
			}
			return type;
		}
		finally
		{
			Profiler.Leave("LoadClassByDottedName");
		}
	}

	private TypeWrapper GetCompiledTypeWrapper(Type type)
	{
		Debug.Assert(!(type is TypeBuilder));
		// only the bootstrap classloader can own compiled types
		Debug.Assert(this == GetBootstrapClassLoader());
		string name = NativeCode.java.lang.Class.getName(type);
		TypeWrapper wrapper = (TypeWrapper)types[name];
		if(wrapper == null)
		{
			TypeWrapper baseType;
			if(type.IsInterface)
			{
				baseType = null;
			}
			else if(type.BaseType == null)
			{
				baseType = LoadClassByDottedName("java.lang.Object");
			}
			else
			{
				baseType = GetWrapperFromType(type.BaseType);
			}
			wrapper = new CompiledTypeWrapper(name.Replace('.', '/'), type, baseType);
			types.Add(name, wrapper);
			typeToTypeWrapper[type] = wrapper;
		}
		return wrapper;
	}

	internal virtual Type GetBootstrapTypeRaw(string name)
	{
		// TODO consider the thread safety aspects of this (if another thread triggers a load of an IKVM assembly,
		// the collection enumerator will throw a version exception)
		foreach(Assembly a in ikvmAssemblies)
		{
			Type t = a.GetType(name);
			if(t != null)
			{
				return t;
			}
			// HACK we might be looking for an inner classes
			t = a.GetType(name.Replace('$', '+'));
			if(t != null)
			{
				return t;
			}
		}
		// HACK for the time being we'll look into all loaded assemblies, this is to work around
		// a bug caused by the fact that SigDecoderWrapper is used to parse signatures that contain .NET exported
		// types (the Mauve test gnu.testlet.java.io.ObjectStreamClass.ProxyTest failed because
		// it did a getDeclaredMethods on java/lang/VMClassLoader which has a method that returns a System.Reflection.Assembly)
		foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			Type t = a.GetType(name);
			if(t != null)
			{
				return t;
			}
			// HACK we might be looking for an inner classes
			t = a.GetType(name.Replace('$', '+'));
			if(t != null)
			{
				return t;
			}
		}
		return null;
	}

	internal virtual TypeWrapper GetBootstrapType(string name)
	{
		Type t = GetBootstrapTypeRaw(name);
		if(t != null)
		{
			return GetCompiledTypeWrapper(t);
		}
		return null;
	}

	private TypeWrapper CreateArrayType(string name, Type elementType, int dims)
	{
		// TODO array accessibility should be the same as the elementType's accessibility
		// (and this should be enforced)
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
				// HACK ModuleBuilder.GetType() is broken (I think), it fires a TypeResolveEvent when
				// you try to construct an array type from an unfinished type. I don't think it should
				// do that. We have to work around that by setting a global flag (yuck) to prevent us
				// from responding to the TypeResolveEvent.
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
			else
			{
				array = elementType.Assembly.GetType(elementType.FullName + netname, true);
			}
			TypeWrapper[] interfaces = new TypeWrapper[2];
			interfaces[0] = GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Cloneable");
			interfaces[1] = GetBootstrapClassLoader().LoadClassByDottedName("java.io.Serializable");
			MethodDescriptor mdClone = new MethodDescriptor(GetBootstrapClassLoader(), "clone", "()Ljava/lang/Object;");
			Modifiers modifiers = Modifiers.Final | Modifiers.Abstract;
			// TODO taking the publicness from the .NET isn't 100% correct, we really should look at the wrapper
			if(elementType.IsPublic)
			{
				modifiers |= Modifiers.Public;
			}
			wrapper = new RemappedTypeWrapper(this, modifiers, name, array, interfaces, GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Object"));
			MethodInfo clone = typeof(Array).GetMethod("Clone");
			MethodWrapper mw = new MethodWrapper(wrapper, mdClone, clone, null, Modifiers.Public | Modifiers.Synthetic);
			mw.EmitCall = CodeEmitter.Create(OpCodes.Callvirt, clone);
			mw.EmitCallvirt = CodeEmitter.Create(OpCodes.Callvirt, clone);
			wrapper.AddMethod(mw);
			types.Add(name, wrapper);
			typeToTypeWrapper[array] = wrapper;
		}
		return wrapper;
	}

	// TODO make sure class isn't defined already
	// TODO check for circularity
	// TODO disallow anyone other than the bootstrap classloader defining classes in the "java." package
	internal TypeWrapper DefineClass(ClassFile f)
	{
		// TODO shouldn't this check be in ClassFile.cs?
		if(f.Name.Length == 0 || f.Name[0] == '[')
		{
			throw JavaException.ClassFormatError("Bad name");
		}
		TypeWrapper type;
		// TODO if the class doesn't exist, LoadClassBySlashedName throws a ClassNotFoundException, but
		// we need to catch that and throw a NoClassDefFoundError (because that is unchecked)
		// TODO also figure out what should happen if LoadClassBySlashedName throws another exception (custom class loaders
		// can throw whatever exception they want)
		TypeWrapper baseType = LoadClassBySlashedName(f.SuperClass);
		// if the base type isn't public, it must be in the same package
		if(!baseType.IsPublic)
		{
			if(baseType.GetClassLoader() != this || f.PackageName != baseType.PackageName)
			{
				throw JavaException.IllegalAccessError("Class {0} cannot access its superclass {1}", f.Name, baseType.Name);
			}
		}
		if(baseType.IsFinal)
		{
			throw JavaException.VerifyError("Cannot inherit from final class");
		}
		if(baseType.IsInterface)
		{
			throw JavaException.IncompatibleClassChangeError("Class {0} has interface {1} as superclass", f.Name, baseType.Name);
		}
		string dotnetType = f.NetExpTypeAttribute;
		if(dotnetType != null)
		{
			type = new NetExpTypeWrapper(f, dotnetType, baseType);
		}
		else
		{
			type = new DynamicTypeWrapper(f, this, nativeMethods);
			dynamicTypes.Add(f.Name.Replace('/', '.'), type);
		}
		types.Add(f.Name.Replace('/', '.'), type);
		return type;
	}

	internal object GetJavaClassLoader()
	{
		return (this == GetBootstrapClassLoader()) ? null : javaClassLoader;
	}

	internal static void SaveDebugImage(object mainClass)
	{
		// HACK we iterate 3 times, in the hopes that that will be enough. We really should let FinishAll return a boolean whether
		// anything was done, and continue iterating until all FinishAlls return false.
		for(int i = 0; i < 3; i++)
		{
			foreach(ClassLoaderWrapper wrapper in classLoaders)
			{
				wrapper.FinishAll();
			}
		}
		// HACK use reflection to get the type from the class
		Type mainType = NativeCode.java.lang.Class.getType(mainClass);
		MethodInfo main = mainType.GetMethod("main", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(string[]) }, null);
		AssemblyBuilder asm = ((AssemblyBuilder)moduleBuilder.Assembly);
		asm.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
		asm.Save(moduleBuilder.Name);
	}

	// this version isn't used at the moment, because multi assembly type references are broken in the CLR
	internal static void SaveDebugImage__MultiAssemblyVersion(object mainClass)
	{
		// HACK we iterate 3 times, in the hopes that that will be enough. We really should let FinishAll return a boolean whether
		// anything was done, and continue iterating until all FinishAlls return false.
		for(int i = 0; i < 3; i++)
		{
			foreach(DictionaryEntry entry in assemblyToClassLoaderWrapper)
			{
				AssemblyBuilder asm = (AssemblyBuilder)entry.Key;
				ClassLoaderWrapper loader = (ClassLoaderWrapper)entry.Value;
				loader.FinishAll();
			}
		}
		// HACK use reflection to get the type from the class
		Type mainType = NativeCode.java.lang.Class.getType(mainClass);
		MethodInfo main = mainType.GetMethod("main", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(string[]) }, null);
		foreach(DictionaryEntry entry in ClassLoaderWrapper.assemblyToClassLoaderWrapper)
		{
			AssemblyBuilder asm = (AssemblyBuilder)entry.Key;
			ClassLoaderWrapper loader = (ClassLoaderWrapper)entry.Value;
			if(mainType.Assembly.Equals(asm))
			{
				asm.SetEntryPoint(main, PEFileKinds.ConsoleApplication);
			}
			asm.Save(asm.GetName().Name);
		}
	}

	internal void FinishAll()
	{
		int prevCount = -1;
		while(prevCount != types.Count)
		{
			prevCount = types.Count;
			ArrayList l = new ArrayList();
			foreach(TypeWrapper t in types.Values)
			{
				l.Add(t);
			}
			foreach(TypeWrapper t in l)
			{
				t.Finish();
			}
		}
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
					lock(assemblyToClassLoaderWrapper.SyncRoot)
					{
						assemblyToClassLoaderWrapper[moduleBuilder.Assembly] = this;
					}
				}
				return moduleBuilder;
			}
		}
	}

	protected virtual ModuleBuilder CreateModuleBuilder()
	{
		AssemblyName name = new AssemblyName();
		name.Name = "ikvm_dynamic_assembly__" + (this == GetBootstrapClassLoader() ? "bootstrap" : javaClassLoader);
		AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
		ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name, JVM.Debug);
		if(JVM.Debug)
		{
			CustomAttributeBuilder debugAttr = new CustomAttributeBuilder(typeof(DebuggableAttribute).GetConstructor(new Type[] { typeof(bool), typeof(bool) }), new object[] { true, true });
			moduleBuilder.SetCustomAttribute(debugAttr);
		}
		return moduleBuilder;
	}

	internal TypeWrapper ExpressionTypeWrapper(string type)
	{
		if(type.StartsWith("Lret;"))
		{
			throw new InvalidOperationException("ExpressionTypeWrapper for Lret; requested");
		}
		if(type == "Lnull")
		{
			throw new InvalidOperationException("ExpressionTypeWrapper for Lnull requested");
		}
		int index = 0;
		return SigDecoderWrapper(ref index, type);
	}

	// NOTE: this will ignore anything following the sig marker (so that it can be used to decode method signatures)
	private Type SigDecoder(ref int index, string sig)
	{
		switch(sig[index++])
		{
			case 'B':
				return typeof(sbyte);
			case 'C':
				return typeof(char);
			case 'D':
				return typeof(double);
			case 'F':
				return typeof(float);
			case 'I':
				return typeof(int);
			case 'J':
				return typeof(long);
			case 'L':
			{
				int pos = index;
				index = sig.IndexOf(';', index) + 1;
				return LoadClassBySlashedName(sig.Substring(pos, index - pos - 1)).Type;
			}
			case 'S':
				return typeof(short);
			case 'Z':
				return typeof(bool);
			case 'V':
				return typeof(void);
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
						return LoadClassBySlashedName(array + sig.Substring(pos, index - pos)).Type;
					}
					case 'B':
					case 'C':
					case 'D':
					case 'F':
					case 'I':
					case 'J':
					case 'S':
					case 'Z':
						return LoadClassBySlashedName(array + sig[index++]).Type;
					default:
						throw new InvalidOperationException(sig.Substring(index));
				}
			}
			default:
				throw new InvalidOperationException("Invalid at " + index + " in " + sig);
		}
	}

	internal Type RetTypeFromSig(string sig)
	{
		int index = sig.IndexOf(')') + 1;
		return SigDecoder(ref index, sig);
	}

	internal Type[] ArgTypeListFromSig(string sig)
	{
		if(sig[1] == ')')
		{
			return Type.EmptyTypes;
		}
		ArrayList list = new ArrayList();
		for(int i = 1; sig[i] != ')';)
		{
			list.Add(SigDecoder(ref i, sig));
		}
		Type[] types = new Type[list.Count];
		list.CopyTo(types);
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
				return LoadClassBySlashedName(sig.Substring(pos, index - pos - 1));
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
						return LoadClassBySlashedName(array + sig.Substring(pos, index - pos));
					}
					case 'B':
					case 'C':
					case 'D':
					case 'F':
					case 'I':
					case 'J':
					case 'S':
					case 'Z':
						return LoadClassBySlashedName(array + sig[index++]);
					default:
						throw new InvalidOperationException(sig.Substring(index));
				}
			}
			default:
				throw new InvalidOperationException(sig.Substring(index));
		}
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
			return new TypeWrapper[0];
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
		if(bootstrapClassLoader == null)
		{
			bootstrapClassLoader = new ClassLoaderWrapper(null);
			bootstrapClassLoader.LoadRemappedTypes();
		}
		return bootstrapClassLoader;
	}
	
	internal static ClassLoaderWrapper GetClassLoaderWrapper(object javaClassLoader)
	{
		if(javaClassLoader == null || GetBootstrapClassLoader().javaClassLoader == javaClassLoader)
		{
			return GetBootstrapClassLoader();
		}
		ClassLoaderWrapper wrapper = (ClassLoaderWrapper)javaClassLoaderToClassLoaderWrapper[javaClassLoader];
		if(wrapper == null)
		{
			wrapper = new ClassLoaderWrapper(javaClassLoader);
			javaClassLoaderToClassLoaderWrapper[javaClassLoader] = wrapper;
		}
		return wrapper;
	}

	internal static ClassLoaderWrapper GetClassLoader(Type type)
	{
		Debug.Assert(!(type is TypeBuilder));
		TypeWrapper wrapper = GetWrapperFromTypeFast(type);
		if(wrapper != null)
		{
			return wrapper.GetClassLoader();
		}
		return GetBootstrapClassLoader();
//		ClassLoaderWrapper loader = (ClassLoaderWrapper)assemblyToClassLoaderWrapper[type.Assembly];
//		if(loader == null)
//		{
//			loader = GetBootstrapClassLoader();
//		}
//		return loader;
	}

	// This only returns the wrapper for a Type if that wrapper has already been created, otherwise
	// it returns null
	// If the wrapper doesn't exist, that means that the type is either a .NET type or a pre-compiled Java class
	internal static TypeWrapper GetWrapperFromTypeFast(Type type)
	{
		Debug.Assert(!(type is TypeBuilder));
		TypeWrapper wrapper = (TypeWrapper)typeToTypeWrapper[type];
		if(wrapper == null)
		{
			if(type.IsPrimitive)
			{
				if(type == typeof(sbyte))
				{
					wrapper = PrimitiveTypeWrapper.BYTE;
				}
				else if(type == typeof(char))
				{
					wrapper = PrimitiveTypeWrapper.CHAR;
				}
				else if(type == typeof(double))
				{
					wrapper = PrimitiveTypeWrapper.DOUBLE;
				}
				else if(type == typeof(float))
				{
					wrapper = PrimitiveTypeWrapper.FLOAT;
				}
				else if(type == typeof(int))
				{
					wrapper = PrimitiveTypeWrapper.INT;
				}
				else if(type == typeof(long))
				{
					wrapper = PrimitiveTypeWrapper.LONG;
				}
				else if(type == typeof(short))
				{
					wrapper = PrimitiveTypeWrapper.SHORT;
				}
				else if(type == typeof(bool))
				{
					wrapper = PrimitiveTypeWrapper.BOOLEAN;
				}
			}
			else if(type == typeof(void))
			{
				wrapper = PrimitiveTypeWrapper.VOID;
			}
			// if we found it, store it in the map
			if(wrapper != null)
			{
				typeToTypeWrapper[type] = wrapper;
			}
		}
		return wrapper;
	}

	internal static TypeWrapper GetWrapperFromType(Type type)
	{
		Debug.Assert(!(type is TypeBuilder));
		TypeWrapper wrapper = GetWrapperFromTypeFast(type);
		if(wrapper == null)
		{
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
				wrapper = (TypeWrapper)typeToTypeWrapper[elem];
				if(wrapper != null)
				{
					// HACK this is a lame way of creating the array wrapper
					if(elem.IsPrimitive)
					{
						string elemType;
						if(elem == typeof(sbyte))
						{
							elemType = "B";
						}
						else if(elem == typeof(bool))
						{
							elemType = "Z";
						}
						else if(elem == typeof(short))
						{
							elemType = "S";
						}
						else if(elem == typeof(char))
						{
							elemType = "C";
						}
						else if(elem == typeof(int))
						{
							elemType = "I";
						}
						else if(elem == typeof(long))
						{
							elemType = "J";
						}
						else if(elem == typeof(float))
						{
							elemType = "F";
						}
						else if(elem == typeof(double))
						{
							elemType = "D";
						}
						else
						{
							throw new InvalidOperationException();
						}
						wrapper = wrapper.GetClassLoader().LoadClassBySlashedName(new String('[', rank) + elemType);
					}
					else
					{
						wrapper = wrapper.GetClassLoader().LoadClassBySlashedName(new String('[', rank) + "L" + wrapper.Name + ";");
					}
				}
			}
			// if the wrapper doesn't already exist, that must mean that the type
			// is a .NET type (or a pre-compiled Java class), which means that it
			// was "loaded" by the bootstrap classloader
			// TODO think up a scheme to deal with .NET types that have the same name. Since all .NET types
			// appear in the boostrap classloader, we need to devise a scheme to mangle the class name
			wrapper = GetBootstrapClassLoader().GetCompiledTypeWrapper(type);
		}
		return wrapper;
	}

	internal static void SetWrapperForType(Type type, TypeWrapper wrapper)
	{
		Debug.Assert(!(type is TypeBuilder));
		typeToTypeWrapper.Add(type, wrapper);
	}

	// name is dot separated (e.g. java.lang.Object)
	internal static Type GetType(string name)
	{
		TypeWrapper wrapper = GetBootstrapClassLoader().LoadClassByDottedName(name);
		// TODO think about this Finish here
		wrapper.Finish();
		return wrapper.Type;
	}
}
