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
	private MethodInfo loadClassMethod;
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
		if(javaClassLoader != null)
		{
			loadClassMethod = javaClassLoader.GetType().GetMethod("loadClass", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, new Type[] { typeof(string) }, null);
			if(loadClassMethod == null)
			{
				throw new InvalidOperationException();
			}
		}
	}

	internal void LoadRemappedTypes()
	{
		nativeMethods = new Hashtable();
		// TODO interfaces have java/lang/Object as the base type (do they really?)
		types["java.lang.Cloneable"] = new RemappedTypeWrapper(ModifiersAttribute.GetModifiers(typeof(java.lang.Cloneable)), "java/lang/Cloneable", typeof(java.lang.Cloneable), new TypeWrapper[0], null);
		typeToTypeWrapper.Add(typeof(java.lang.Cloneable), types["java.lang.Cloneable"]);
		types["java.io.Serializable"] = new RemappedTypeWrapper(ModifiersAttribute.GetModifiers(typeof(java.io.Serializable)), "java/io/Serializable", typeof(java.io.Serializable), new TypeWrapper[0], null);
		typeToTypeWrapper.Add(typeof(java.io.Serializable), types["java.io.Serializable"]);
		MapXml.Root map = null;
		using(Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("map.xml"))
		{
			// TODO the XmlSerializer generates a bunch of C# code and compiles that. This is very slow, we probably
			// shouldn't use it.
			System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(MapXml.Root));
			map = (MapXml.Root)ser.Deserialize(s);
		}
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
			// TODO specify interfaces
			TypeWrapper tw = new RemappedTypeWrapper(modifiers, name.Replace('.', '/'), Type.GetType(c.Type, true), new TypeWrapper[0], baseWrapper);
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
			type = GetBootstrapType(name);
			if(type != null)
			{
				return type;
			}
			if(javaClassLoader == null)
			{
				throw JavaException.ClassNotFoundException(name);
			}
		}
		// OPTIMIZE this should be optimized
		try
		{
			object clazz;
			// NOTE just like Java does (I think), we take the classloader lock before calling the loadClass method
			lock(javaClassLoader)
			{
				clazz = loadClassMethod.Invoke(javaClassLoader, new object[] { name });
			}
			type = (TypeWrapper)clazz.GetType().GetField("wrapper", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(clazz);
			if(type == null)
			{
				Type t = (Type)clazz.GetType().GetField("type", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(clazz);
				ClassLoaderWrapper loader = GetClassLoader(t);
				type = (TypeWrapper)loader.types[name];
				if(type == null)
				{
					// this shouldn't be possible
					throw new InvalidOperationException(name + ", this = " + javaClassLoader);
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
		catch(TargetInvocationException x)
		{
			ExceptionHelper.MapExceptionFast(x);
			throw x.InnerException;
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
			if(type.BaseType == null)
			{
				baseType = LoadClassByDottedName("java.lang.Object");
			}
			else
			{
				baseType = GetWrapperFromType(type.BaseType);
			}
			wrapper = new CompiledTypeWrapper(name.Replace('.', '/'), type, baseType);
			types.Add(name, wrapper);
			// TODO shouldn't we add the <type,wrapper> to the typeToTypeWrapper hashtable?
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
			Modifiers modifiers = Modifiers.Final | Modifiers.Public;
			// TODO copy accessibility from element type
			wrapper = new RemappedTypeWrapper(modifiers, name, array, interfaces, GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Object"));
			MethodInfo clone = typeof(Array).GetMethod("Clone");
			MethodWrapper mw = new MethodWrapper(wrapper, mdClone, clone, Modifiers.Public);
			mw.EmitCall = CodeEmitter.Create(OpCodes.Callvirt, clone);
			mw.EmitCallvirt = CodeEmitter.Create(OpCodes.Callvirt, clone);
			wrapper.AddMethod(mw);
			types.Add(name, wrapper);
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
			type = new DynamicTypeWrapper(f.Name, f, baseType, this, nativeMethods);
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

	internal Type ExpressionType(string type)
	{
		// HACK to ease the burden of the compiler, we support the Lret pseudo type here
		if(type.StartsWith("Lret;"))
		{
			return typeof(int);
		}
		if(type == "Lnull")
		{
			throw new InvalidOperationException("ExpressionType for Lnull requested");
		}
		int index = 0;
		return SigDecoder(ref index, type);
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

	// subType and baseType are Java class name (e.g. java/lang/Object)
	internal bool IsSubType(string subType, string baseType)
	{
		return LoadClassBySlashedName(subType).IsSubTypeOf(LoadClassBySlashedName(baseType));
	}

	internal string FindCommonBaseType(string type1, string type2)
	{
		TypeWrapper t1 = LoadClassBySlashedName(type1);
		TypeWrapper t2 = LoadClassBySlashedName(type2);
		if(t1 == t2)
		{
			return type1;
		}
		if(t1.IsInterface || t2.IsInterface)
		{
			// TODO I don't know how finding the common base for interfaces is defined, but
			// for now I'm just doing the naive thing
			// UPDATE according to a paper by Alessandro Coglio & Allen Goldberg titled
			// "Type Safety in the JVM: Some Problems in Java 2 SDK 1.2 and Proposed Solutions"
			// the common base of two interfaces is java/lang/Object, and there is special
			// treatment for java/lang/Object types that allow it to be assigned to any interface
			// type, the JVM's typesafety then depends on the invokeinterface instruction to make
			// sure that the reference actually implements the interface.
			// So strictly speaking, the code below isn't correct, but it works, so for now it stays in.
			if(t1.ImplementsInterface(t2))
			{
				return t2.Name;
			}
			if(t2.ImplementsInterface(t1))
			{
				return t1.Name;
			}
			return "java/lang/Object";
		}
		Stack st1 = new Stack();
		Stack st2 = new Stack();
		while(t1 != null)
		{
			st1.Push(t1);
			t1 = t1.BaseTypeWrapper;
		}
		while(t2 != null)
		{
			st2.Push(t2);
			t2 = t2.BaseTypeWrapper;
		}
		TypeWrapper type = null;
		for(;;)
		{
			t1 = st1.Count > 0 ? (TypeWrapper)st1.Pop() : null;
			t2 = st2.Count > 0 ? (TypeWrapper)st2.Pop() : null;
			if(t1 != t2)
			{
				return type.Name;
			}
			type = t1;
		}
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
		return (TypeWrapper)typeToTypeWrapper[type];
	}

	internal static TypeWrapper GetWrapperFromType(Type type)
	{
		Debug.Assert(!(type is TypeBuilder));
		TypeWrapper wrapper = GetWrapperFromTypeFast(type);
		if(wrapper == null)
		{
			// if the wrapper doesn't already exist, that must mean that the type
			// is a .NET type (or a pre-compiled Java class), which means that it
			// was "loaded" by the bootstrap classloader
			// TODO think up a scheme to deal with .NET types that have the same name. Since all .NET types
			// appear in the boostrap classloader, we need to devise a scheme to mangle the class name
			if(type.IsPrimitive || type == typeof(void))
			{
				if(type == typeof(void))
				{
					return PrimitiveTypeWrapper.VOID;
				}
				else if(type == typeof(sbyte))
				{
					return PrimitiveTypeWrapper.BYTE;
				}
				else if(type == typeof(char))
				{
					return PrimitiveTypeWrapper.CHAR;
				}
				else if(type == typeof(double))
				{
					return PrimitiveTypeWrapper.DOUBLE;
				}
				else if(type == typeof(float))
				{
					return PrimitiveTypeWrapper.FLOAT;
				}
				else if(type == typeof(int))
				{
					return PrimitiveTypeWrapper.INT;
				}
				else if(type == typeof(long))
				{
					return PrimitiveTypeWrapper.LONG;
				}
				else if(type == typeof(short))
				{
					return PrimitiveTypeWrapper.SHORT;
				}
				else if(type == typeof(bool))
				{
					return PrimitiveTypeWrapper.BOOLEAN;
				}
			}
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
