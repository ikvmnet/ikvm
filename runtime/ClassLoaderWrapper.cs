/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
#if !COMPACT_FRAMEWORK
using System.Reflection.Emit;
#endif
using System.IO;
using System.Collections;
using System.Diagnostics;
using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Internal
{
	abstract class ClassLoaderWrapper
	{
		private static readonly object wrapperLock = new object();
		private static readonly Hashtable typeToTypeWrapper = Hashtable.Synchronized(new Hashtable());
		private static ClassLoaderWrapper bootstrapClassLoader;
#if WHIDBEY && !STATIC_COMPILER
		private static readonly Hashtable reflectionOnlyClassLoaders = new Hashtable();
#endif
		private readonly object javaClassLoader;
		protected Hashtable types = new Hashtable();
		private ArrayList nativeLibraries;
		private static Hashtable remappedTypes = new Hashtable();

		// HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
		internal static void SetBootstrapClassLoader(ClassLoaderWrapper bootstrapClassLoader)
		{
			Debug.Assert(ClassLoaderWrapper.bootstrapClassLoader == null);

			ClassLoaderWrapper.bootstrapClassLoader = bootstrapClassLoader;
		}

		static ClassLoaderWrapper()
		{
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
			// if we're compiling the core, coreAssembly will be null
			Assembly coreAssembly = JVM.CoreAssembly;
			if(coreAssembly != null)
			{
				Tracer.Info(Tracer.Runtime, "Core assembly: {0}", coreAssembly.Location);
				RemappedClassAttribute[] remapped = AttributeHelper.GetRemappedClasses(coreAssembly);
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

		internal static int GetLoadedClassCount()
		{
			return typeToTypeWrapper.Count;
		}

		internal static bool IsCoreAssemblyType(Type type)
		{
			return type.Assembly == JVM.CoreAssembly;
		}

		internal ClassLoaderWrapper(object javaClassLoader)
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

		protected TypeWrapper RegisterInitiatingLoader(TypeWrapper tw)
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
						// HACK elem.ElementTypeWrapper as an evil side effect registers the initiating loader
						// so if we're the same loader, don't do it again
						if(!elem.IsPrimitive && !types.ContainsKey(elem.Name))
						{
							RegisterInitiatingLoader(elem);
						}
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

#if !COMPACT_FRAMEWORK
		// HACK the proxyClassLoader is used to dynamically load classes in the boot class loader
		// (e.g. when a Proxy is defined a boot class)
		private static DynamicClassLoader proxyClassLoader;

		internal virtual TypeWrapper DefineClass(ClassFile f, object protectionDomain)
		{
			lock(wrapperLock)
			{
				if(proxyClassLoader == null)
				{
					proxyClassLoader = new DynamicClassLoader(null);
				}
			}
			return proxyClassLoader.DefineClass(f, protectionDomain);
		}
#endif

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
			if(name.Length >= 1024 || name.Length == 0)
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
#if STATIC_COMPILER
					// NOTE it is important that this is done last, because otherwise we will
					// load the netexp generated fake types (e.g. delegate inner interface) instead
					// of having DotNetTypeWrapper generating it.
					type = GetTypeWrapperCompilerHook(name);
					if(type != null)
					{
						return type;
					}
#endif // STATIC_COMPILER
					if(javaClassLoader == null)
					{
						return null;
					}
				}
#if !COMPACT_FRAMEWORK
				// if we're here, we're not the bootstrap class loader and don't have a java class loader,
				// that must mean that we're either the proxyClassLoader or a ReflectionOnly class loader
				// and we should delegate to the bootstrap class loader
				if(javaClassLoader == null)
				{
					return GetBootstrapClassLoader().LoadClassByDottedNameFast(name, throwClassNotFoundException);
				}
#endif
#if !STATIC_COMPILER
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
#else
				return null;
#endif
			}
			finally
			{
				Profiler.Leave("LoadClassByDottedName");
			}
		}

		private TypeWrapper GetWrapperFromBootstrapType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromBootstrapType: {0}", type.FullName);
			Debug.Assert(!type.IsArray, "!type.IsArray", type.FullName);
#if !COMPACT_FRAMEWORK
			Debug.Assert(!(type.Assembly is AssemblyBuilder), "!(type.Assembly is AssemblyBuilder)", type.FullName);
#endif
			// only the bootstrap classloader can own compiled types
			Debug.Assert(this == GetBootstrapClassLoader(), "this == GetBootstrapClassLoader()", type.FullName);
			bool javaType = AttributeHelper.IsJavaModule(type.Module);
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
					string msg = String.Format("\nTypename \"{0}\" is imported from multiple assemblies:\n{1}\n{2}\n", type.FullName, wrapper.TypeAsTBD.Assembly.FullName, type.Assembly.FullName);
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
					if(!DotNetTypeWrapper.IsAllowedOutside(type))
					{
						return null;
					}
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
		private Type GetBootstrapTypeRaw(string name)
		{
#if COMPACT_FRAMEWORK
			// TODO figure this out
			return GetJavaTypeFromAssembly(JVM.CoreAssembly, name);
#else
			Assembly[] assemblies;
#if WHIDBEY
			if(JVM.IsStaticCompiler)
			{
				assemblies = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies();
			}
			else
			{
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			}
#else
			assemblies = AppDomain.CurrentDomain.GetAssemblies();
#endif
			foreach(Assembly a in assemblies)
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
#endif
		}

		private static Type GetJavaTypeFromAssembly(Assembly a, string name)
		{
			try
			{
				Type t = a.GetType(name);
				if(t != null
					&& AttributeHelper.IsJavaModule(t.Module)
					&& !AttributeHelper.IsHideFromJava(t)
					&& !t.IsArray
					&& !t.IsPointer
					&& !t.IsByRef)
				{
					return t;
				}
				// HACK we might be looking for an inner classes
				t = a.GetType(name.Replace('$', '+'));
				if(t != null
					&& AttributeHelper.IsJavaModule(t.Module)
					&& !AttributeHelper.IsHideFromJava(t)
					&& !t.IsArray
					&& !t.IsPointer
					&& !t.IsByRef)
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
			catch(FileLoadException x)
			{
				// this can only happen if the assembly was loaded in the ReflectionOnly
				// context and the requested type references a type in another assembly
				// that cannot be found in the ReflectionOnly context
				// TODO figure out what other exceptions Assembly.GetType() can throw
				Tracer.Info(Tracer.Runtime, x.Message);
			}
			return null;
		}

#if STATIC_COMPILER
		internal virtual TypeWrapper GetTypeWrapperCompilerHook(string name)
		{
			return null;
		}
#endif // STATIC_COMPILER

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
				// .NET 1.1 has a limit of 1024 characters for type names
				if(elementType.FullName.Length >= 1024 - dims * 2)
				{
					return null;
				}
				Type array = ArrayTypeWrapper.MakeArrayType(elementType, dims);
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
#if COMPACT_FRAMEWORK
						if(!wrapper.IsGhostArray)
						{
							Debug.Assert(!typeToTypeWrapper.ContainsKey(array), name);
							typeToTypeWrapper.Add(array, wrapper);
						}
#else
						if(!(elementType is TypeBuilder) && !wrapper.IsGhostArray)
						{
							Debug.Assert(!typeToTypeWrapper.ContainsKey(array), name);
							typeToTypeWrapper.Add(array, wrapper);
						}
#endif
					}
					else
					{
						wrapper = race;
					}
				}
			}
			return wrapper;
		}

		internal object GetJavaClassLoader()
		{
			return javaClassLoader;
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
#if COMPACT_FRAMEWORK
				return new Type[0];
#else
				return Type.EmptyTypes;
#endif
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
			lock(wrapperLock)
			{
				if(bootstrapClassLoader == null)
				{
					bootstrapClassLoader = new BootstrapClassLoader();
				}
				return bootstrapClassLoader;
			}
		}

#if !STATIC_COMPILER
		internal static ClassLoaderWrapper GetClassLoaderWrapper(object javaClassLoader)
		{
			if(javaClassLoader == null)
			{
				return GetBootstrapClassLoader();
			}
			lock(wrapperLock)
			{
				ClassLoaderWrapper wrapper = (ClassLoaderWrapper)JVM.Library.getWrapperFromClassLoader(javaClassLoader);
				if(wrapper == null)
				{
					wrapper = new DynamicClassLoader(javaClassLoader);
					JVM.Library.setWrapperForClassLoader(javaClassLoader, wrapper);
				}
				return wrapper;
			}
		}
#endif

		// This only returns the wrapper for a Type if that wrapper has already been created, otherwise
		// it returns null
		// If the wrapper doesn't exist, that means that the type is either a .NET type or a pre-compiled Java class
		private static TypeWrapper GetWrapperFromTypeFast(Type type)
		{
			TypeWrapper.AssertFinished(type);
			Debug.Assert(!Whidbey.ContainsGenericParameters(type));
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
			Debug.Assert(!Whidbey.ContainsGenericParameters(type));
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
				// was "loaded" by an assembly classloader
				return GetAssemblyClassLoader(type.Assembly).GetWrapperFromBootstrapType(type);
			}
			return wrapper;
		}

		// this method only supports .NET or pre-compiled Java assemblies
		internal static ClassLoaderWrapper GetAssemblyClassLoader(Assembly assembly)
		{
			// TODO this assertion fires when compiling the core library (at least on Whidbey)
			// I need to find out why...
			Debug.Assert(!(assembly is AssemblyBuilder));

#if WHIDBEY && !STATIC_COMPILER
			if(assembly.ReflectionOnly)
			{
				lock(reflectionOnlyClassLoaders)
				{
					ClassLoaderWrapper loader = (ClassLoaderWrapper)reflectionOnlyClassLoaders[assembly];
					if(loader == null)
					{
						loader = new ReflectionOnlyClassLoader();
						reflectionOnlyClassLoaders[assembly] = loader;
					}
					return loader;
				}
			}
#endif
			return GetBootstrapClassLoader();
		}

		internal static void SetWrapperForType(Type type, TypeWrapper wrapper)
		{
			TypeWrapper.AssertFinished(type);
			Debug.Assert(!typeToTypeWrapper.ContainsKey(type));
			typeToTypeWrapper.Add(type, wrapper);
		}

#if STATIC_COMPILER
		internal static void PublishLibraryImplementationHelperType(Type type)
		{
			CompiledTypeWrapper typeWrapper = CompiledTypeWrapper.newInstance(type.FullName, type);
			SetWrapperForType(type, typeWrapper);
			GetBootstrapClassLoader().types[type.FullName] = typeWrapper;
		}
#endif // STATIC_COMPILER

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

	class BootstrapClassLoader : ClassLoaderWrapper
	{
		internal BootstrapClassLoader()
			: base(null)
		{
		}
	}

	class ReflectionOnlyClassLoader : ClassLoaderWrapper
	{
		internal ReflectionOnlyClassLoader()
			: base(null)
		{
		}
	}
}
