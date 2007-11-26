/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006, 2007 Jeroen Frijters

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
using System.Threading;
using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Internal
{
	abstract class TypeWrapperFactory
	{
#if !COMPACT_FRAMEWORK
		internal abstract ModuleBuilder ModuleBuilder { get; }
#endif
		internal abstract TypeWrapper DefineClassImpl(Hashtable types, ClassFile f, ClassLoaderWrapper classLoader, object protectionDomain);
		internal abstract bool ReserveName(string name);
	}

	class ClassLoaderWrapper
	{
		private static readonly object wrapperLock = new object();
		private static readonly Hashtable typeToTypeWrapper = Hashtable.Synchronized(new Hashtable());
#if STATIC_COMPILER
		private static ClassLoaderWrapper bootstrapClassLoader;
		private TypeWrapperFactory factory;
#else
		private static AssemblyClassLoader bootstrapClassLoader;
#endif
		private static readonly Hashtable assemblyClassLoaders = new Hashtable();
		private static ArrayList genericClassLoaders;
#if !STATIC_COMPILER && !FIRST_PASS
		private readonly java.lang.ClassLoader javaClassLoader;
		private static bool customClassLoaderRedirectsLoaded;
		private static Hashtable customClassLoaderRedirects;
#endif
		protected Hashtable types = new Hashtable();
		private readonly Hashtable defineClassInProgress = new Hashtable();
		private ArrayList nativeLibraries;
		private CodeGenOptions codegenoptions;
		private static Hashtable remappedTypes = new Hashtable();

#if STATIC_COMPILER
		// HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
		internal static void SetBootstrapClassLoader(ClassLoaderWrapper bootstrapClassLoader)
		{
			Debug.Assert(ClassLoaderWrapper.bootstrapClassLoader == null);

			ClassLoaderWrapper.bootstrapClassLoader = bootstrapClassLoader;
		}
#endif

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

		internal ClassLoaderWrapper(CodeGenOptions codegenoptions, object javaClassLoader)
		{
			this.codegenoptions = codegenoptions;
#if !STATIC_COMPILER && !FIRST_PASS
			this.javaClassLoader = (java.lang.ClassLoader)javaClassLoader;
#endif
		}

		internal static bool IsDynamicType(Type type)
		{
			return typeToTypeWrapper[type] is DynamicTypeWrapper;
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

		// return the TypeWrapper if it is already loaded, this exists for DynamicTypeWrapper.SetupGhosts
		// and ClassLoader.findLoadedClass()
		internal virtual TypeWrapper GetLoadedClass(string name)
		{
			lock(types.SyncRoot)
			{
				return (TypeWrapper)types[name];
			}
		}

		internal TypeWrapper RegisterInitiatingLoader(TypeWrapper tw)
		{
			Debug.Assert(tw != null);
			Debug.Assert(!tw.IsUnloadable);
			Debug.Assert(!tw.IsPrimitive);

			lock(types.SyncRoot)
			{
				object existing = types[tw.Name];
				if(existing != tw)
				{
					if(existing != null)
					{
						// another thread beat us to it, discard the new TypeWrapper and
						// return the previous one
						return (TypeWrapper)existing;
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

		internal bool EmitDebugInfo
		{
			get
			{
				return (codegenoptions & CodeGenOptions.Debug) != 0;
			}
		}

		internal bool EmitStackTraceInfo
		{
			get
			{
				// NOTE we're negating the flag here!
				return (codegenoptions & CodeGenOptions.NoStackTraceInfo) == 0;
			}
		}

		internal bool StrictFinalFieldSemantics
		{
			get
			{
				return (codegenoptions & CodeGenOptions.StrictFinalFieldSemantics) != 0;
			}
		}

		internal bool NoJNI
		{
			get
			{
				return (codegenoptions & CodeGenOptions.NoJNI) != 0;
			}
		}

		internal virtual string SourcePath
		{
			get
			{
				return null;
			}
		}

		protected virtual void CheckDefineClassAllowed(string className)
		{
			// this hook exists so that AssemblyClassLoader can prevent DefineClass when the name is already present in the assembly
		}

		internal TypeWrapper DefineClass(ClassFile f, object protectionDomain)
		{
			string dotnetAssembly = f.IKVMAssemblyAttribute;
			if(dotnetAssembly != null)
			{
				// It's a stub class generated by ikvmstub (or generated by the runtime when getResource was
				// called on a statically compiled class).
				ClassLoaderWrapper loader;
				try
				{
					loader = ClassLoaderWrapper.GetAssemblyClassLoaderByName(dotnetAssembly);
				}
				catch(Exception x)
				{
					// TODO don't catch all exceptions here
					throw new NoClassDefFoundError(f.Name + " (" + x.Message + ")");
				}
				TypeWrapper tw = loader.LoadClassByDottedNameFast(f.Name);
				if(tw == null)
				{
					throw new NoClassDefFoundError(f.Name + " (type not found in " + dotnetAssembly + ")");
				}
				return RegisterInitiatingLoader(tw);
			}
			CheckDefineClassAllowed(f.Name);
			lock(types.SyncRoot)
			{
				if(types.ContainsKey(f.Name))
				{
					throw new LinkageError("duplicate class definition: " + f.Name);
				}
				// mark the type as "loading in progress", so that we can detect circular dependencies.
				types.Add(f.Name, null);
				defineClassInProgress.Add(f.Name, Thread.CurrentThread);
			}
			try
			{
				return GetTypeWrapperFactory().DefineClassImpl(types, f, this, protectionDomain);
			}
			finally
			{
				lock(types.SyncRoot)
				{
					if(types[f.Name] == null)
					{
						// if loading the class fails, we remove the indicator that we're busy loading the class,
						// because otherwise we get a ClassCircularityError if we try to load the class again.
						types.Remove(f.Name);
					}
					defineClassInProgress.Remove(f.Name);
					Monitor.PulseAll(types.SyncRoot);
				}
			}
		}

		internal TypeWrapperFactory GetTypeWrapperFactory()
		{
#if COMPACT_FRAMEWORK
			throw new NoClassDefFoundError("Class loading is not supported on the Compact Framework");
#elif STATIC_COMPILER
			if(factory == null)
			{
				factory = new DynamicClassLoader(((CompilerClassLoader)this).CreateModuleBuilder());
			}
			return factory;
#else
			return DynamicClassLoader.Instance;
#endif
		}

		internal TypeWrapper LoadClassByDottedName(string name)
		{
			TypeWrapper type = LoadClassByDottedNameFastImpl(name, true);
			if(type != null)
			{
				return RegisterInitiatingLoader(type);
			}
			throw new ClassNotFoundException(name);
		}

		internal TypeWrapper LoadClassByDottedNameFast(string name)
		{
			TypeWrapper type = LoadClassByDottedNameFastImpl(name, false);
			if(type != null)
			{
				return RegisterInitiatingLoader(type);
			}
			return null;
		}

		private TypeWrapper LoadClassByDottedNameFastImpl(string name, bool throwClassNotFoundException)
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
					if(type == null)
					{
						object defineThread = defineClassInProgress[name];
						if(defineThread != null)
						{
							if(Thread.CurrentThread == defineThread)
							{
								throw new ClassCircularityError(name);
							}
							// the requested class is currently being defined by another thread,
							// so we have to wait on that
							while(defineClassInProgress.ContainsKey(name))
							{
								Monitor.Wait(types.SyncRoot);
							}
							type = (TypeWrapper)types[name];
						}
					}
				}
				if(type != null)
				{
					return type;
				}
				if(name.Length > 1 && name[0] == '[')
				{
					return LoadArrayClass(name);
				}
				return LoadClassImpl(name, throwClassNotFoundException);
			}
			finally
			{
				Profiler.Leave("LoadClassByDottedName");
			}
		}

		private TypeWrapper LoadArrayClass(string name)
		{
			int dims = 1;
			while(name[dims] == '[')
			{
				dims++;
				if(dims == name.Length)
				{
					// malformed class name
					return null;
				}
			}
			if(name[dims] == 'L')
			{
				if(!name.EndsWith(";") || name.Length <= dims + 2 || name[dims + 1] == '[')
				{
					// malformed class name
					return null;
				}
				string elemClass = name.Substring(dims + 1, name.Length - dims - 2);
				// NOTE it's important that we're registered as the initiating loader
				// for the element type here
				TypeWrapper type = LoadClassByDottedNameFast(elemClass);
				if(type != null)
				{
					type = type.GetClassLoader().CreateArrayType(name, type, dims);
				}
				return type;
			}
			if(name.Length != dims + 1)
			{
				// malformed class name
				return null;
			}
			switch(name[dims])
			{
				case 'B':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BYTE, dims);
				case 'C':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.CHAR, dims);
				case 'D':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.DOUBLE, dims);
				case 'F':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.FLOAT, dims);
				case 'I':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.INT, dims);
				case 'J':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.LONG, dims);
				case 'S':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.SHORT, dims);
				case 'Z':
					return GetBootstrapClassLoader().CreateArrayType(name, PrimitiveTypeWrapper.BOOLEAN, dims);
				default:
					return null;
			}
		}

		internal TypeWrapper LoadGenericClass(string name)
		{
			// generic class name grammar:
			//
			// mangled(open_generic_type_name) "_$$$_" M(parameter_class_name) ( "_$$_" M(parameter_class_name) )* "_$$$$_"
			//
			// mangled() is the normal name mangling algorithm
			// M() is a replacement of "__" with "$$005F$$005F" followed by a replace of "." with "__"
			//
			int pos = name.IndexOf("_$$$_");
			if(pos <= 0 || !name.EndsWith("_$$$$_"))
			{
				return null;
			}
			Type type = GetType(DotNetTypeWrapper.DemangleTypeName(name.Substring(0, pos)));
			if(type == null || !Whidbey.IsGenericTypeDefinition(type))
			{
				return null;
			}
			ArrayList typeParamNames = new ArrayList();
			pos += 5;
			int start = pos;
			int nest = 0;
			for(;;)
			{
				pos = name.IndexOf("_$$", pos);
				if(pos == -1)
				{
					return null;
				}
				if(name.IndexOf("_$$_", pos, 4) == pos)
				{
					if(nest == 0)
					{
						typeParamNames.Add(name.Substring(start, pos - start));
						start = pos + 4;
					}
					pos += 4;
				}
				else if(name.IndexOf("_$$$_", pos, 5) == pos)
				{
					nest++;
					pos += 5;
				}
				else if(name.IndexOf("_$$$$_", pos, 6) == pos)
				{
					if(nest == 0)
					{
						if(pos + 6 != name.Length)
						{
							return null;
						}
						typeParamNames.Add(name.Substring(start, pos - start));
						break;
					}
					nest--;
					pos += 6;
				}
				else
				{
					pos += 3;
				}
			}
			Type[] typeArguments = new Type[typeParamNames.Count];
			for(int i = 0; i < typeArguments.Length; i++)
			{
				string s = (string)typeParamNames[i];
				// only do the unmangling for non-generic types (because we don't want to convert
				// the double underscores in two adjacent _$$$_ or _$$$$_ markers)
				if(s.IndexOf("_$$$_") == -1)
				{
					s = s.Replace("__", ".");
					s = s.Replace("$$005F$$005F", "__");
				}
				int dims = 0;
				while(s.Length > dims && s[dims] == 'A')
				{
					dims++;
				}
				if(s.Length == dims)
				{
					return null;
				}
				TypeWrapper tw = null;
				switch(s[dims])
				{
					case 'L':
						tw = LoadClassByDottedNameFast(s.Substring(dims + 1));
						tw.Finish();
						break;
					case 'Z':
						tw = PrimitiveTypeWrapper.BOOLEAN;
						break;
					case 'B':
						tw = PrimitiveTypeWrapper.BYTE;
						break;
					case 'S':
						tw = PrimitiveTypeWrapper.SHORT;
						break;
					case 'C':
						tw = PrimitiveTypeWrapper.CHAR;
						break;
					case 'I':
						tw = PrimitiveTypeWrapper.INT;
						break;
					case 'F':
						tw = PrimitiveTypeWrapper.FLOAT;
						break;
					case 'J':
						tw = PrimitiveTypeWrapper.LONG;
						break;
					case 'D':
						tw = PrimitiveTypeWrapper.DOUBLE;
						break;
				}
				if(tw == null)
				{
					return null;
				}
				if(dims > 0)
				{
					tw = tw.MakeArrayType(dims);
				}
				typeArguments[i] = tw.TypeAsSignatureType;
			}
			try
			{
				type = Whidbey.MakeGenericType(type, typeArguments);
			}
			catch(ArgumentException)
			{
				// one of the typeArguments failed to meet the constraints
				return null;
			}
			TypeWrapper wrapper = GetWrapperFromType(type);
			if(wrapper != null && wrapper.Name != name)
			{
				// the name specified was not in canonical form
				return null;
			}
			return wrapper;
		}

		protected virtual TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			TypeWrapper tw = LoadGenericClass(name);
			if(tw != null)
			{
				return tw;
			}
#if !STATIC_COMPILER && !FIRST_PASS
			Profiler.Enter("ClassLoader.loadClass");
			try
			{
				java.lang.Class c = javaClassLoader.loadClass(name);
				if(c == null)
				{
					return null;
				}
				TypeWrapper type = TypeWrapper.FromClass(c);
				if(type.Name != name)
				{
					// the class loader is trying to trick us
					return null;
				}
				return type;
			}
			catch(java.lang.ClassNotFoundException x)
			{
				if(throwClassNotFoundException)
				{
					throw new ClassLoadingException(ikvm.runtime.Util.mapException(x));
				}
				return null;
			}
			catch(Exception x)
			{
				throw new ClassLoadingException(ikvm.runtime.Util.mapException(x));
			}
			finally
			{
				Profiler.Leave("ClassLoader.loadClass");
			}
#else
			return null;
#endif
		}

		// NOTE this method can actually return null if the resulting array type name would be too long
		// for .NET to handle.
		private TypeWrapper CreateArrayType(string name, TypeWrapper elementTypeWrapper, int dims)
		{
			Debug.Assert(new String('[', dims) + elementTypeWrapper.SigName == name);
			Debug.Assert(!elementTypeWrapper.IsUnloadable && !elementTypeWrapper.IsVerifierType && !elementTypeWrapper.IsArray);
			Debug.Assert(dims >= 1);
			Type elementType = elementTypeWrapper.TypeAsArrayType;
			// .NET 1.1 has a limit of 1024 characters for type names
			if(elementType.FullName.Length >= 1024 - dims * 2)
			{
				return null;
			}
			return RegisterInitiatingLoader(new ArrayTypeWrapper(elementTypeWrapper, name));
		}

		internal object GetJavaClassLoader()
		{
#if FIRST_PASS || STATIC_COMPILER
			return null;
#else
			return javaClassLoader;
#endif
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

#if STATIC_COMPILER
		internal static ClassLoaderWrapper GetBootstrapClassLoader()
#else
		internal static AssemblyClassLoader GetBootstrapClassLoader()
#endif
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
					CodeGenOptions opt = CodeGenOptions.None;
					if(System.Diagnostics.Debugger.IsAttached)
					{
						opt |= CodeGenOptions.Debug;
					}
					wrapper = new ClassLoaderWrapper(opt, javaClassLoader);
					JVM.Library.setWrapperForClassLoader(javaClassLoader, wrapper);
				}
				return wrapper;
			}
		}
#endif

		internal static TypeWrapper GetWrapperFromType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromType: {0}", type.AssemblyQualifiedName);
			TypeWrapper.AssertFinished(type);
			Debug.Assert(!Whidbey.ContainsGenericParameters(type));
			Debug.Assert(!type.IsPointer);
			Debug.Assert(!type.IsByRef);
			TypeWrapper wrapper = (TypeWrapper)typeToTypeWrapper[type];
			if(wrapper != null)
			{
				return wrapper;
			}
			string remapped = (string)remappedTypes[type];
			if(remapped != null)
			{
				wrapper = LoadClassCritical(remapped);
			}
			else if(IsVector(type))
			{
				// it might be an array of a dynamically compiled Java type
				int rank = 1;
				Type elem = type.GetElementType();
				while(IsVector(elem))
				{
					rank++;
					elem = elem.GetElementType();
				}
				wrapper = GetWrapperFromType(elem).MakeArrayType(rank);
			}
			else
			{
				// if the wrapper doesn't already exist, that must mean that the type
				// is a .NET type (or a pre-compiled Java class), which means that it
				// was "loaded" by an assembly classloader
				wrapper = GetAssemblyClassLoader(type.Assembly).GetWrapperFromAssemblyType(type);
			}
			typeToTypeWrapper[type] = wrapper;
			return wrapper;
		}

		internal static bool IsVector(Type type)
		{
			// NOTE it looks like there's no API to distinguish an array of rank 1 from a vector,
			// so we check if the type name ends in [], which indicates it's a vector
			// (non-vectors will have [*] or [,]).
			return type.IsArray && type.Name.EndsWith("[]");
		}

		internal virtual Type GetType(string name)
		{
			return null;
		}

		internal static ClassLoaderWrapper GetGenericClassLoader(TypeWrapper wrapper)
		{
			Type type = wrapper.TypeAsTBD;
			Debug.Assert(Whidbey.IsGenericType(type));
			Debug.Assert(!Whidbey.ContainsGenericParameters(type));

			ArrayList list = new ArrayList();
			list.Add(GetAssemblyClassLoader(type.Assembly));
			foreach(Type arg in Whidbey.GetGenericArguments(type))
			{
				ClassLoaderWrapper loader = GetWrapperFromType(arg).GetClassLoader();
				if(!list.Contains(loader))
				{
					list.Add(loader);
				}
			}
			ClassLoaderWrapper[] key = (ClassLoaderWrapper[])list.ToArray(typeof(ClassLoaderWrapper));
			ClassLoaderWrapper matchingLoader = GetGenericClassLoaderByKey(key);
			matchingLoader.RegisterInitiatingLoader(wrapper);
			return matchingLoader;
		}

		private static ClassLoaderWrapper GetGenericClassLoaderByKey(ClassLoaderWrapper[] key)
		{
			lock(wrapperLock)
			{
				if(genericClassLoaders == null)
				{
					genericClassLoaders = new ArrayList();
				}
				foreach(GenericClassLoader loader in genericClassLoaders)
				{
					if(loader.Matches(key))
					{
						return loader;
					}
				}
				object javaClassLoader = null;
#if !STATIC_COMPILER
				javaClassLoader = JVM.Library.newAssemblyClassLoader(null);
#endif
				GenericClassLoader newLoader = new GenericClassLoader(key, javaClassLoader);
#if !STATIC_COMPILER
				JVM.Library.setWrapperForClassLoader(javaClassLoader, newLoader);
#endif
				genericClassLoaders.Add(newLoader);
				return newLoader;
			}
		}

		internal static ClassLoaderWrapper GetGenericClassLoaderByName(string name)
		{
			Debug.Assert(name.StartsWith("[[") && name.EndsWith("]]"));
			Stack stack = new Stack();
			ArrayList list = null;
			for(int i = 0; i < name.Length; i++)
			{
				if(name[i] == '[')
				{
					if(name[i + 1] == '[')
					{
						stack.Push(list);
						list = new ArrayList();
						if(name[i + 2] == '[')
						{
							i++;
						}
					}
					else
					{
						int start = i + 1;
						i = name.IndexOf(']', i);
						list.Add(ClassLoaderWrapper.GetAssemblyClassLoaderByName(name.Substring(start, i - start)));
					}
				}
				else if(name[i] == ']')
				{
					ClassLoaderWrapper loader = GetGenericClassLoaderByKey((ClassLoaderWrapper[])list.ToArray(typeof(ClassLoaderWrapper)));
					list = (ArrayList)stack.Pop();
					if(list == null)
					{
						return loader;
					}
					list.Add(loader);
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
			throw new InvalidOperationException();
		}

		internal static ClassLoaderWrapper GetAssemblyClassLoaderByName(string name)
		{
			if(name.StartsWith("[["))
			{
				return GetGenericClassLoaderByName(name);
			}
#if WHIDBEY && STATIC_COMPILER
			return ClassLoaderWrapper.GetAssemblyClassLoader(Assembly.ReflectionOnlyLoad(name));
#else
			return ClassLoaderWrapper.GetAssemblyClassLoader(Assembly.Load(name));
#endif
		}

		internal static int GetGenericClassLoaderId(ClassLoaderWrapper wrapper)
		{
			lock(wrapperLock)
			{
				return genericClassLoaders.IndexOf(wrapper);
			}
		}

		internal static ClassLoaderWrapper GetGenericClassLoaderById(int id)
		{
			lock(wrapperLock)
			{
				return (ClassLoaderWrapper)genericClassLoaders[id];
			}
		}

		// this method only supports .NET or pre-compiled Java assemblies
		internal static AssemblyClassLoader GetAssemblyClassLoader(Assembly assembly)
		{
#if !COMPACT_FRAMEWORK
			Debug.Assert(!(assembly is AssemblyBuilder));
#endif // !COMPACT_FRAMEWORK

			ConstructorInfo customClassLoaderCtor = null;
			AssemblyClassLoader loader;
			object javaClassLoader = null;
			lock(wrapperLock)
			{
				loader = (AssemblyClassLoader)assemblyClassLoaders[assembly];
				if(loader == null)
				{
#if !STATIC_COMPILER && !FIRST_PASS
					if(assembly == JVM.CoreAssembly)
					{
						return GetBootstrapClassLoader();
					}
					if(!Whidbey.ReflectionOnly(assembly))
					{
						Type customClassLoaderClass = null;
						LoadCustomClassLoaderRedirects();
						if(customClassLoaderRedirects != null)
						{
							string assemblyName = assembly.FullName;
							foreach(DictionaryEntry de in customClassLoaderRedirects)
							{
								string asm = (string)de.Key;
								// we only support matching on the assembly's simple name,
								// because there appears to be no viable alternative.
								// On .NET 2.0 there is AssemblyName.ReferenceMatchesDefinition()
								// but it is broken (and .NET 2.0 specific).
								if(assemblyName.StartsWith(asm + ","))
								{
									try
									{
										customClassLoaderClass = Type.GetType((string)de.Value, true);
									}
									catch(Exception x)
									{
										Tracer.Error(Tracer.Runtime, "Unable to load custom class loader {0} specified in app.config for assembly {1}: {2}", de.Value, assembly, x);
									}
									break;
								}
							}
						}
						if(customClassLoaderClass == null)
						{
							object[] attribs = assembly.GetCustomAttributes(typeof(CustomAssemblyClassLoaderAttribute), false);
							if(attribs.Length == 1)
							{
								customClassLoaderClass = ((CustomAssemblyClassLoaderAttribute)attribs[0]).Type;
							}
						}
						if(customClassLoaderClass != null)
						{
							try
							{
								if(!customClassLoaderClass.IsPublic && !customClassLoaderClass.Assembly.Equals(assembly))
								{
									throw new Exception("Type not accessible");
								}
								// NOTE we're creating an uninitialized instance of the custom class loader here, so that getClassLoader will return the proper object
								// when it is called during the construction of the custom class loader later on. This still doesn't make it safe to use the custom
								// class loader before it is constructed, but at least the object instance is valid and should anyone cache it, they will get the
								// right object to use later on.
								// Note also that we're not running the constructor here, because we don't want to run user code while holding a global lock.
								javaClassLoader = (java.lang.ClassLoader)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(customClassLoaderClass);
								customClassLoaderCtor = customClassLoaderClass.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(Assembly) }, null);
								if(customClassLoaderCtor == null)
								{
									javaClassLoader = null;
									throw new Exception("No constructor");
								}
								if(!customClassLoaderCtor.IsPublic && !customClassLoaderClass.Assembly.Equals(assembly))
								{
									javaClassLoader = null;
									throw new Exception("Constructor not accessible");
								}
								Tracer.Info(Tracer.Runtime, "Created custom assembly class loader {0} for assembly {1}", customClassLoaderClass.FullName, assembly);
							}
							catch(Exception x)
							{
								Tracer.Error(Tracer.Runtime, "Unable to create custom assembly class loader {0} for {1}: {2}", customClassLoaderClass.FullName, assembly, x);
							}
						}
					}
					if(javaClassLoader == null)
					{
						javaClassLoader = JVM.Library.newAssemblyClassLoader(assembly);
					}
#endif
					loader = new AssemblyClassLoader(assembly, javaClassLoader, customClassLoaderCtor != null);
					assemblyClassLoaders[assembly] = loader;
#if !STATIC_COMPILER
					if(customClassLoaderCtor != null)
					{
						loader.SetInitInProgress();
					}
					if(javaClassLoader != null)
					{
						JVM.Library.setWrapperForClassLoader(javaClassLoader, loader);
					}
#endif
				}
			}
#if !STATIC_COMPILER && !FIRST_PASS
			if(customClassLoaderCtor != null)
			{
				try
				{
					java.security.AccessController.doPrivileged(new CustomClassLoaderCtorCaller(customClassLoaderCtor, javaClassLoader, assembly));
				}
				finally
				{
					loader.SetInitDone();
				}
			}
			loader.WaitInitDone();
#endif
			return loader;
		}

#if !STATIC_COMPILER && !FIRST_PASS
		private static void LoadCustomClassLoaderRedirects()
		{
			// this method assumes that we hold a global lock
			if(!customClassLoaderRedirectsLoaded)
			{
				customClassLoaderRedirectsLoaded = true;
				try
				{
					foreach(string key in System.Configuration.ConfigurationSettings.AppSettings.AllKeys)
					{
						const string prefix = "ikvm-classloader:";
						if(key.StartsWith(prefix))
						{
							if(customClassLoaderRedirects == null)
							{
								customClassLoaderRedirects = new Hashtable();
							}
							customClassLoaderRedirects[key.Substring(prefix.Length)] = System.Configuration.ConfigurationManager.AppSettings.Get(key);
						}
					}
				}
				catch(Exception x)
				{
					Tracer.Error(Tracer.Runtime, "Error while reading custom class loader redirects: {0}", x);
				}
			}
		}

		sealed class CustomClassLoaderCtorCaller : java.security.PrivilegedAction
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

		internal static void SetWrapperForType(Type type, TypeWrapper wrapper)
		{
			TypeWrapper.AssertFinished(type);
			Debug.Assert(!typeToTypeWrapper.ContainsKey(type));
			typeToTypeWrapper.Add(type, wrapper);
		}

		internal static void ResetWrapperForType(Type type, TypeWrapper wrapper)
		{
			TypeWrapper.AssertFinished(type);
			typeToTypeWrapper[type] = wrapper;
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

#if !STATIC_COMPILER && !FIRST_PASS
		public override string ToString()
		{
			if(javaClassLoader == null)
			{
				return "null";
			}
			return String.Format("{0}@{1:X}", GetWrapperFromType(javaClassLoader.GetType()).Name, javaClassLoader.GetHashCode());
		}
#endif
	}

	class GenericClassLoader : ClassLoaderWrapper
	{
		private ClassLoaderWrapper[] delegates;

		internal GenericClassLoader(ClassLoaderWrapper[] delegates, object javaClassLoader)
			: base(CodeGenOptions.None, javaClassLoader)
		{
			this.delegates = delegates;
		}

		internal bool Matches(ClassLoaderWrapper[] key)
		{
			if(key.Length == delegates.Length)
			{
				for(int i = 0; i < key.Length; i++)
				{
					if(key[i] != delegates[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		internal override Type GetType(string name)
		{
			foreach(ClassLoaderWrapper loader in delegates)
			{
				Type t = loader.GetType(name);
				if(t != null)
				{
					return t;
				}
			}
			return null;
		}

		protected override TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			TypeWrapper tw = LoadGenericClass(name);
			if(tw != null)
			{
				return tw;
			}
			foreach(ClassLoaderWrapper loader in delegates)
			{
				tw = loader.LoadClassByDottedNameFast(name);
				if(tw != null)
				{
					return tw;
				}
			}
			return null;
		}

		internal string GetName()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('[');
			foreach(ClassLoaderWrapper loader in delegates)
			{
				sb.Append('[');
				GenericClassLoader gcl = loader as GenericClassLoader;
				if(gcl != null)
				{
					sb.Append(gcl.GetName());
				}
				else
				{
					sb.Append(((AssemblyClassLoader)loader).Assembly.FullName);
				}
				sb.Append(']');
			}
			sb.Append(']');
			return sb.ToString();
		}
	}

	class AssemblyClassLoader : ClassLoaderWrapper
	{
		private Assembly assembly;
		private AssemblyName[] references;
		private AssemblyClassLoader[] delegates;
#if WHIDBEY
		private bool isReflectionOnly;
#endif // WHIDBEY
		private bool[] isJavaModule;
		private Module[] modules;
		private Hashtable nameMap;
#if !STATIC_COMPILER
		private Thread initializerThread;
		private volatile object protectionDomain;
#endif
		private bool hasDotNetModule;
		private bool hasCustomClassLoader;

		internal AssemblyClassLoader(Assembly assembly, object javaClassLoader, bool hasCustomClassLoader)
			: base(CodeGenOptions.None, javaClassLoader)
		{
			this.assembly = assembly;
			modules = assembly.GetModules(false);
			isJavaModule = new bool[modules.Length];
			this.hasCustomClassLoader = hasCustomClassLoader;
#if WHIDBEY
			isReflectionOnly = assembly.ReflectionOnly;
#endif // WHIDBEY
			for(int i = 0; i < modules.Length; i++)
			{
				object[] attr = AttributeHelper.GetJavaModuleAttributes(modules[i]);
				if(attr.Length > 0)
				{
					isJavaModule[i] = true;
					foreach(JavaModuleAttribute jma in attr)
					{
						string[] map = jma.GetClassMap();
						if(map != null)
						{
							if(nameMap == null)
							{
								nameMap = new Hashtable();
							}
							for(int j = 0; j < map.Length; j += 2)
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
			references = assembly.GetReferencedAssemblies();
			delegates = new AssemblyClassLoader[references.Length];
		}

		internal Assembly Assembly
		{
			get
			{
				return assembly;
			}
		}

		internal override Type GetType(string name)
		{
			try
			{
				return assembly.GetType(name);
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

		private Type GetType(Module mod, string name)
		{
			try
			{
				return mod.GetType(name);
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

		private Type GetJavaType(Module mod, string name)
		{
			try
			{
				string n = null;
				if(nameMap != null)
				{
					n = (string)nameMap[name];
				}
				Type t = GetType(mod, n != null ? n : name);
				if(t == null)
				{
					n = name.Replace('$', '+');
					if(!ReferenceEquals(n, name))
					{
						t = GetType(n);
					}
				}
				if(t != null
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
			return null;
		}

		internal TypeWrapper DoLoad(string name)
		{
			for(int i = 0; i < modules.Length; i++)
			{
				if(isJavaModule[i])
				{
					Type type = GetJavaType(modules[i], name);
					if(type != null)
					{
						// check the name to make sure that the canonical name was used
						if(CompiledTypeWrapper.GetName(type) == name)
						{
							return RegisterInitiatingLoader(CompiledTypeWrapper.newInstance(name, type));
						}
					}
				}
				else
				{
					// TODO should we catch ArgumentException and prohibit array, pointer and byref here?
					Type type = GetType(modules[i], DotNetTypeWrapper.DemangleTypeName(name));
					if(type != null && DotNetTypeWrapper.IsAllowedOutside(type))
					{
						TypeWrapper tw = new DotNetTypeWrapper(type);
						// check the name to make sure that the canonical name was used
						if(tw.Name == name)
						{
							return RegisterInitiatingLoader(tw);
						}
					}
				}
			}
			if(hasDotNetModule)
			{
				// for manufactured types, we load the declaring outer type (the real one) and
				// let that generated the manufactured nested classes
				TypeWrapper outer = null;
				if(name.EndsWith(DotNetTypeWrapper.DelegateInterfaceSuffix))
				{
					outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.DelegateInterfaceSuffix.Length));
				}
				else if(name.EndsWith(DotNetTypeWrapper.AttributeAnnotationSuffix))
				{
					outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationSuffix.Length));
				}
				else if(name.EndsWith(DotNetTypeWrapper.AttributeAnnotationReturnValueSuffix))
				{
					outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationReturnValueSuffix.Length));
				}
				else if(name.EndsWith(DotNetTypeWrapper.AttributeAnnotationMultipleSuffix))
				{
					outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.AttributeAnnotationMultipleSuffix.Length));
				}
				else if(name.EndsWith(DotNetTypeWrapper.EnumEnumSuffix))
				{
					outer = DoLoad(name.Substring(0, name.Length - DotNetTypeWrapper.EnumEnumSuffix.Length));
				}
				if(outer != null && (outer is DotNetTypeWrapper || outer.IsDynamicOnly))
				{
					foreach(TypeWrapper tw in outer.InnerClasses)
					{
						if(tw.Name == name)
						{
							return RegisterInitiatingLoader(tw);
						}
					}
				}
			}
			return null;
		}

		internal TypeWrapper GetWrapperFromAssemblyType(Type type)
		{
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromAssemblyType: {0}", type.FullName);
			Debug.Assert(!type.Name.EndsWith("[]"), "!type.IsArray", type.FullName);
			Debug.Assert(type.Assembly == assembly);
#if !COMPACT_FRAMEWORK
			Debug.Assert(!(type.Assembly is AssemblyBuilder), "!(type.Assembly is AssemblyBuilder)", type.FullName);
#endif
			Module mod = type.Module;
			int moduleIndex = -1;
			for(int i = 0; i < modules.Length; i++)
			{
				if(modules[i] == mod)
				{
					moduleIndex = i;
					break;
				}
			}
			string name;
			if(isJavaModule[moduleIndex])
			{
				name = CompiledTypeWrapper.GetName(type);
			}
			else
			{
				name = DotNetTypeWrapper.GetName(type);
				if(name == null)
				{
					return null;
				}
			}
			TypeWrapper wrapper;
			lock(types.SyncRoot)
			{
				wrapper = (TypeWrapper)types[name];
			}
			if(wrapper != null)
			{
				if(wrapper.TypeAsTBD != type && (!wrapper.IsRemapped || wrapper.TypeAsBaseType != type))
				{
					// this really shouldn't happen, it means that we have two different types in our assembly that both
					// have the same Java name
					string msg = String.Format("\nType \"{0}\" and \"{1}\" both map to the same name \"{2}\".\n", type.FullName, wrapper.TypeAsTBD.FullName, name);
					JVM.CriticalFailure(msg, null);
				}
				return wrapper;
			}
			else
			{
				if(isJavaModule[moduleIndex])
				{
					if(AttributeHelper.IsHideFromJava(type))
					{
						return null;
					}
					// since this type was compiled from Java source, we have to look for our
					// attributes
					return RegisterInitiatingLoader(CompiledTypeWrapper.newInstance(name, type));
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
					return RegisterInitiatingLoader(new DotNetTypeWrapper(type));
				}
			}
		}

		protected override TypeWrapper LoadClassImpl(string name, bool throwClassNotFoundException)
		{
			TypeWrapper tw = DoLoad(name);
			if(tw != null)
			{
				return tw;
			}
			if(hasCustomClassLoader)
			{
				return base.LoadClassImpl(name, throwClassNotFoundException);
			}
			else
			{
				tw = LoadGenericClass(name);
				if(tw != null)
				{
					return tw;
				}
				return LoadReferenced(name);
			}
		}

		internal TypeWrapper LoadReferenced(string name)
		{
			for(int i = 0; i < delegates.Length; i++)
			{
				if(delegates[i] == null)
				{
					Assembly asm = null;
					try
					{
						// TODO consider throttling the Load attempts (or caching failure)
#if WHIDBEY
						if(isReflectionOnly)
						{
							asm = Assembly.ReflectionOnlyLoad(references[i].FullName);
						}
						else
#endif
						{
							asm = Assembly.Load(references[i]);
						}
					}
					catch
					{
					}
					if(asm != null)
					{
						delegates[i] = ClassLoaderWrapper.GetAssemblyClassLoader(asm);
					}
				}
				if(delegates[i] != null)
				{
					TypeWrapper tw = delegates[i].DoLoad(name);
					if(tw != null)
					{
						return tw;
					}
				}
			}
			bool isJava = false;
			for(int i = 0; i < isJavaModule.Length; i++)
			{
				if(isJavaModule[i])
				{
					isJava = true;
					break;
				}
			}
			if(!isJava)
			{
				return GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
			}
			return null;
		}

#if !STATIC_COMPILER
		internal Assembly[] FindResourceAssemblies(string name, bool firstOnly)
		{
			ArrayList list = null;
			name = JVM.MangleResourceName(name);
			if(assembly.GetManifestResourceInfo(name) != null)
			{
				if(firstOnly)
				{
					return new Assembly[] { assembly };
				}
				list = new ArrayList();
				list.Add(assembly);
			}
			for(int i = 0; i < delegates.Length; i++)
			{
				if(delegates[i] == null)
				{
					Assembly asm = null;
					try
					{
						// TODO consider throttling the Load attempts (or caching failure)
#if WHIDBEY
						if(isReflectionOnly)
						{
							asm = Assembly.ReflectionOnlyLoad(references[i].FullName);
						}
						else
#endif
					{
						asm = Assembly.Load(references[i]);
					}
					}
					catch
					{
					}
					if(asm != null)
					{
						delegates[i] = ClassLoaderWrapper.GetAssemblyClassLoader(asm);
					}
				}
				if(delegates[i] != null)
				{
					if(delegates[i].Assembly.GetManifestResourceInfo(name) != null)
					{
						if(firstOnly)
						{
							return new Assembly[] { delegates[i].Assembly };
						}
						if(list == null)
						{
							list = new ArrayList();
						}
						list.Add(delegates[i].Assembly);
					}
				}
			}
			bool isJava = false;
			for(int i = 0; i < isJavaModule.Length; i++)
			{
				if(isJavaModule[i])
				{
					isJava = true;
					break;
				}
			}
			if(!isJava)
			{
				Assembly asm = GetBootstrapClassLoader().Assembly;
				if(asm.GetManifestResourceInfo(name) != null)
				{
					if(firstOnly)
					{
						return new Assembly[] { asm };
					}
					if(list == null)
					{
						list = new ArrayList();
					}
					if(!list.Contains(asm))
					{
						list.Add(asm);
					}
				}
			}
			if(list == null)
			{
				return null;
			}
			return (Assembly[])list.ToArray(typeof(Assembly));
		}

		internal void SetInitInProgress()
		{
			initializerThread = Thread.CurrentThread;
		}

		internal void SetInitDone()
		{
			lock(this)
			{
				initializerThread = null;
				Monitor.PulseAll(this);
			}
		}

		internal void WaitInitDone()
		{
			lock(this)
			{
				if(initializerThread != Thread.CurrentThread)
				{
					while(initializerThread != null)
					{
						Monitor.Wait(this);
					}
				}
			}
		}
#endif // !STATIC_COMPILER

		internal virtual object GetProtectionDomain()
		{
#if STATIC_COMPILER || FIRST_PASS
			return null;
#else
			if(protectionDomain == null)
			{
				java.net.URL codebase;
				try
				{
					codebase = new java.net.URL(assembly.CodeBase);
				}
				catch(java.net.MalformedURLException)
				{
					codebase = null;
				}
				java.security.Permissions permissions = new java.security.Permissions();
				permissions.add(new java.security.AllPermission());
				object pd = new java.security.ProtectionDomain(new java.security.CodeSource(codebase, (java.security.cert.Certificate[])null), permissions, (java.lang.ClassLoader)GetJavaClassLoader(), null);
				lock(this)
				{
					if(protectionDomain == null)
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
			if(DoLoad(className) != null)
			{
				throw new LinkageError("duplicate class definition: " + className);
			}
		}

		internal override TypeWrapper GetLoadedClass(string name)
		{
			TypeWrapper tw = base.GetLoadedClass(name);
			return tw != null ? tw : DoLoad(name);
		}
	}

	class BootstrapClassLoader : AssemblyClassLoader
	{
		internal BootstrapClassLoader()
			: base(JVM.CoreAssembly, null, false)
		{
		}

		internal override object GetProtectionDomain()
		{
			return null;
		}
	}
}
