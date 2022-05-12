/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;
using ProtectionDomain = java.security.ProtectionDomain;
#endif
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Runtime.CompilerServices;
using IKVM.Attributes;

namespace IKVM.Internal
{
	[Flags]
	enum CodeGenOptions
	{
		None = 0,
		Debug = 1,
		NoStackTraceInfo = 2,
		StrictFinalFieldSemantics = 4,
		NoJNI = 8,
		RemoveAsserts = 16,
		NoAutomagicSerialization = 32,
		DisableDynamicBinding = 64,
		NoRefEmitHelpers = 128,
		RemoveUnusedFields = 256,
	}

	[Flags]
	enum LoadMode
	{
		// These are the modes that should be used
		Find					= ReturnNull,
		LoadOrNull				= Load | ReturnNull,
		LoadOrThrow				= Load | ThrowClassNotFound,
		Link					= Load | ReturnUnloadable | SuppressExceptions,

		// call into Java class loader
		Load					= 0x0001,

		// return value
		DontReturnUnloadable	= 0x0002,	// This is used with a bitwise OR to disable returning unloadable
		ReturnUnloadable		= 0x0004,
		ReturnNull				= 0x0004 | DontReturnUnloadable,
		ThrowClassNotFound		= 0x0008 | DontReturnUnloadable,
		MaskReturn				= ReturnUnloadable | ReturnNull | ThrowClassNotFound,

		// exceptions (not ClassNotFoundException)
		SuppressExceptions		= 0x0010,

		// warnings
		WarnClassNotFound		= 0x0020,
	}

#if !STUB_GENERATOR
	abstract class TypeWrapperFactory
	{
		internal abstract ModuleBuilder ModuleBuilder { get; }
		internal abstract TypeWrapper DefineClassImpl(Dictionary<string, TypeWrapper> types, TypeWrapper host, ClassFile f, ClassLoaderWrapper classLoader, ProtectionDomain protectionDomain);
		internal abstract bool ReserveName(string name);
		internal abstract string AllocMangledName(DynamicTypeWrapper tw);
		internal abstract Type DefineUnloadable(string name);
		internal abstract Type DefineDelegate(int parameterCount, bool returnVoid);
		internal abstract bool HasInternalAccess { get; }
#if CLASSGC
		internal abstract void AddInternalsVisibleTo(Assembly friend);
#endif
	}
#endif // !STUB_GENERATOR

	class ClassLoaderWrapper
	{
		private static readonly object wrapperLock = new object();
		private static readonly Dictionary<Type, TypeWrapper> globalTypeToTypeWrapper = new Dictionary<Type, TypeWrapper>();
#if STATIC_COMPILER || STUB_GENERATOR
		private static ClassLoaderWrapper bootstrapClassLoader;
#else
		private static AssemblyClassLoader bootstrapClassLoader;
#endif
		private static List<GenericClassLoaderWrapper> genericClassLoaders;
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		protected java.lang.ClassLoader javaClassLoader;
#endif
#if !STUB_GENERATOR
		private TypeWrapperFactory factory;
#endif // !STUB_GENERATOR
		private readonly Dictionary<string, TypeWrapper> types = new Dictionary<string, TypeWrapper>();
		private readonly Dictionary<string, Thread> defineClassInProgress = new Dictionary<string, Thread>();
		private List<IntPtr> nativeLibraries;
		private readonly CodeGenOptions codegenoptions;
#if CLASSGC
		private Dictionary<Type, TypeWrapper> typeToTypeWrapper;
		private static ConditionalWeakTable<Assembly, ClassLoaderWrapper> dynamicAssemblies;
#endif
		private static readonly Dictionary<Type, string> remappedTypes = new Dictionary<Type, string>();

#if STATIC_COMPILER || STUB_GENERATOR
		// HACK this is used by the ahead-of-time compiler to overrule the bootstrap classloader
		// when we're compiling the core class libraries and by ikvmstub with the -bootstrap option
		internal static void SetBootstrapClassLoader(ClassLoaderWrapper bootstrapClassLoader)
		{
			Debug.Assert(ClassLoaderWrapper.bootstrapClassLoader == null);

			ClassLoaderWrapper.bootstrapClassLoader = bootstrapClassLoader;
		}
#endif

		static ClassLoaderWrapper()
		{
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.BOOLEAN.TypeAsTBD] = PrimitiveTypeWrapper.BOOLEAN;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.BYTE.TypeAsTBD] = PrimitiveTypeWrapper.BYTE;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.CHAR.TypeAsTBD] = PrimitiveTypeWrapper.CHAR;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.DOUBLE.TypeAsTBD] = PrimitiveTypeWrapper.DOUBLE;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.FLOAT.TypeAsTBD] = PrimitiveTypeWrapper.FLOAT;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.INT.TypeAsTBD] = PrimitiveTypeWrapper.INT;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.LONG.TypeAsTBD] = PrimitiveTypeWrapper.LONG;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.SHORT.TypeAsTBD] = PrimitiveTypeWrapper.SHORT;
			globalTypeToTypeWrapper[PrimitiveTypeWrapper.VOID.TypeAsTBD] = PrimitiveTypeWrapper.VOID;
			LoadRemappedTypes();
		}

		internal static void LoadRemappedTypes()
		{
			// if we're compiling the core, coreAssembly will be null
			Assembly coreAssembly = JVM.CoreAssembly;
			if(coreAssembly != null && remappedTypes.Count ==0)
			{
				RemappedClassAttribute[] remapped = AttributeHelper.GetRemappedClasses(coreAssembly);
				if(remapped.Length > 0)
				{
					foreach(RemappedClassAttribute r in remapped)
					{
						remappedTypes.Add(r.RemappedType, r.Name);
					}
				}
				else
				{
#if STATIC_COMPILER
					throw new FatalCompilerErrorException(Message.CoreClassesMissing);
#else
					JVM.CriticalFailure("Failed to find core classes in core library", null);
#endif
				}
			}
		}

		internal ClassLoaderWrapper(CodeGenOptions codegenoptions, object javaClassLoader)
		{
			this.codegenoptions = codegenoptions;
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			this.javaClassLoader = (java.lang.ClassLoader)javaClassLoader;
#endif
		}

		internal static bool IsRemappedType(Type type)
		{
			return remappedTypes.ContainsKey(type);
		}

#if STATIC_COMPILER || STUB_GENERATOR
		internal void SetRemappedType(Type type, TypeWrapper tw)
		{
			lock(types)
			{
				types.Add(tw.Name, tw);
			}
			lock(globalTypeToTypeWrapper)
			{
				globalTypeToTypeWrapper.Add(type, tw);
			}
			remappedTypes.Add(type, tw.Name);
		}
#endif

		// return the TypeWrapper if it is already loaded, this exists for DynamicTypeWrapper.SetupGhosts
		// and implements ClassLoader.findLoadedClass()
		internal TypeWrapper FindLoadedClass(string name)
		{
			if (name.Length > 1 && name[0] == '[')
			{
				return FindOrLoadArrayClass(name, LoadMode.Find);
			}
			TypeWrapper tw;
			lock (types)
			{
				types.TryGetValue(name, out tw);
			}
			return tw ?? FindLoadedClassLazy(name);
		}

		protected virtual TypeWrapper FindLoadedClassLazy(string name)
		{
			return null;
		}

		internal TypeWrapper RegisterInitiatingLoader(TypeWrapper tw)
		{
			Debug.Assert(tw != null);
			Debug.Assert(!tw.IsUnloadable);
			Debug.Assert(!tw.IsPrimitive);

			try
			{
				// critical code in the finally block to avoid Thread.Abort interrupting the thread
			}
			finally
			{
				tw = RegisterInitiatingLoaderCritical(tw);
			}
			return tw;
		}

		private TypeWrapper RegisterInitiatingLoaderCritical(TypeWrapper tw)
		{
			lock(types)
			{
				TypeWrapper existing;
				types.TryGetValue(tw.Name, out existing);
				if(existing != tw)
				{
					if(existing != null)
					{
						// another thread beat us to it, discard the new TypeWrapper and
						// return the previous one
						return existing;
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

		internal bool RemoveAsserts
		{
			get
			{
				return (codegenoptions & CodeGenOptions.RemoveAsserts) != 0;
			}
		}

		internal bool NoAutomagicSerialization
		{
			get
			{
				return (codegenoptions & CodeGenOptions.NoAutomagicSerialization) != 0;
			}
		}

		internal bool DisableDynamicBinding
		{
			get
			{
				return (codegenoptions & CodeGenOptions.DisableDynamicBinding) != 0;
			}
		}

		internal bool EmitNoRefEmitHelpers
		{
			get
			{
				return (codegenoptions & CodeGenOptions.NoRefEmitHelpers) != 0;
			}
		}

		internal bool RemoveUnusedFields
		{
			get
			{
				return (codegenoptions & CodeGenOptions.RemoveUnusedFields) != 0;
			}
		}

		internal bool WorkaroundAbstractMethodWidening
		{
			get
			{
				// pre-Roslyn C# compiler doesn't like widening access to abstract methods
				return true;
			}
		}

		internal bool WorkaroundInterfaceFields
		{
			get
			{
				// pre-Roslyn C# compiler doesn't allow access to interface fields
				return true;
			}
		}

		internal bool WorkaroundInterfacePrivateMethods
		{
			get
			{
				// pre-Roslyn C# compiler doesn't like interfaces that have non-public methods
				return true;
			}
		}

		internal bool WorkaroundInterfaceStaticMethods
		{
			get
			{
				// pre-Roslyn C# compiler doesn't allow access to interface static methods
				return true;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal bool RelaxedClassNameValidation
		{
			get
			{
#if FIRST_PASS
				return true;
#else
				return JVM.relaxedVerification && (javaClassLoader == null || java.lang.ClassLoader.isTrustedLoader(javaClassLoader));
#endif
			}
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		protected virtual void CheckProhibitedPackage(string className)
		{
			if (className.StartsWith("java.", StringComparison.Ordinal))
			{
				throw new JavaSecurityException("Prohibited package name: " + className.Substring(0, className.LastIndexOf('.')));
			}
		}

#if !STUB_GENERATOR
		internal TypeWrapper DefineClass(ClassFile f, ProtectionDomain protectionDomain)
		{
#if !STATIC_COMPILER
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
#endif
			CheckProhibitedPackage(f.Name);
			// check if the class already exists if we're an AssemblyClassLoader
			if(FindLoadedClassLazy(f.Name) != null)
			{
				throw new LinkageError("duplicate class definition: " + f.Name);
			}
			TypeWrapper def;
			try
			{
				// critical code in the finally block to avoid Thread.Abort interrupting the thread
			}
			finally
			{
				def = DefineClassCritical(f, protectionDomain);
			}
			return def;
		}

		private TypeWrapper DefineClassCritical(ClassFile f, ProtectionDomain protectionDomain)
		{
			lock(types)
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
				return GetTypeWrapperFactory().DefineClassImpl(types, null, f, this, protectionDomain);
			}
			finally
			{
				lock(types)
				{
					if(types[f.Name] == null)
					{
						// if loading the class fails, we remove the indicator that we're busy loading the class,
						// because otherwise we get a ClassCircularityError if we try to load the class again.
						types.Remove(f.Name);
					}
					defineClassInProgress.Remove(f.Name);
					Monitor.PulseAll(types);
				}
			}
		}

		internal TypeWrapperFactory GetTypeWrapperFactory()
		{
			if(factory == null)
			{
				lock(this)
				{
					try
					{
						// critical code in the finally block to avoid Thread.Abort interrupting the thread
					}
					finally
					{
						if(factory == null)
						{
#if CLASSGC
							if(dynamicAssemblies == null)
							{
								Interlocked.CompareExchange(ref dynamicAssemblies, new ConditionalWeakTable<Assembly, ClassLoaderWrapper>(), null);
							}
							typeToTypeWrapper = new Dictionary<Type, TypeWrapper>();
							DynamicClassLoader instance = DynamicClassLoader.Get(this);
							dynamicAssemblies.Add(instance.ModuleBuilder.Assembly.ManifestModule.Assembly, this);
							this.factory = instance;
#else
							factory = DynamicClassLoader.Get(this);
#endif
						}
					}
				}
			}
			return factory;
		}
#endif // !STUB_GENERATOR

		internal TypeWrapper LoadClassByDottedName(string name)
		{
			return LoadClass(name, LoadMode.LoadOrThrow);
		}

		internal TypeWrapper LoadClassByDottedNameFast(string name)
		{
			return LoadClass(name, LoadMode.LoadOrNull);
		}

		internal TypeWrapper LoadClass(string name, LoadMode mode)
		{
			Profiler.Enter("LoadClass");
			try
			{
				TypeWrapper tw = LoadRegisteredOrPendingClass(name);
				if (tw != null)
				{
					return tw;
				}
				if (name.Length > 1 && name[0] == '[')
				{
					tw = FindOrLoadArrayClass(name, mode);
				}
				else
				{
					tw = LoadClassImpl(name, mode);
				}
				if (tw != null)
				{
					return RegisterInitiatingLoader(tw);
				}
#if STATIC_COMPILER
				if (!(name.Length > 1 && name[0] == '[') && ((mode & LoadMode.WarnClassNotFound) != 0) || WarningLevelHigh)
				{
					IssueMessage(Message.ClassNotFound, name);
				}
#else
				if (!(name.Length > 1 && name[0] == '['))
				{
					Tracer.Error(Tracer.ClassLoading, "Class not found: {0}", name);
				}
#endif
				switch (mode & LoadMode.MaskReturn)
				{
					case LoadMode.ReturnNull:
						return null;
					case LoadMode.ReturnUnloadable:
						return new UnloadableTypeWrapper(name);
					case LoadMode.ThrowClassNotFound:
						throw new ClassNotFoundException(name);
					default:
						throw new InvalidOperationException();
				}
			}
			finally
			{
				Profiler.Leave("LoadClass");
			}
		}

		private TypeWrapper LoadRegisteredOrPendingClass(string name)
		{
			TypeWrapper tw;
			lock (types)
			{
				if (types.TryGetValue(name, out tw) && tw == null)
				{
					Thread defineThread;
					if (defineClassInProgress.TryGetValue(name, out defineThread))
					{
						if (Thread.CurrentThread == defineThread)
						{
							throw new ClassCircularityError(name);
						}
						// the requested class is currently being defined by another thread,
						// so we have to wait on that
						while (defineClassInProgress.ContainsKey(name))
						{
							Monitor.Wait(types);
						}
						// the defineClass may have failed, so we need to use TryGetValue
						types.TryGetValue(name, out tw);
					}
				}
			}
			return tw;
		}

		private TypeWrapper FindOrLoadArrayClass(string name, LoadMode mode)
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
				TypeWrapper type = LoadClass(elemClass, mode | LoadMode.DontReturnUnloadable);
				if(type != null)
				{
					type = CreateArrayType(name, type, dims);
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
					return CreateArrayType(name, PrimitiveTypeWrapper.BYTE, dims);
				case 'C':
					return CreateArrayType(name, PrimitiveTypeWrapper.CHAR, dims);
				case 'D':
					return CreateArrayType(name, PrimitiveTypeWrapper.DOUBLE, dims);
				case 'F':
					return CreateArrayType(name, PrimitiveTypeWrapper.FLOAT, dims);
				case 'I':
					return CreateArrayType(name, PrimitiveTypeWrapper.INT, dims);
				case 'J':
					return CreateArrayType(name, PrimitiveTypeWrapper.LONG, dims);
				case 'S':
					return CreateArrayType(name, PrimitiveTypeWrapper.SHORT, dims);
				case 'Z':
					return CreateArrayType(name, PrimitiveTypeWrapper.BOOLEAN, dims);
				default:
					return null;
			}
		}

		internal TypeWrapper FindOrLoadGenericClass(string name, LoadMode mode)
		{
			// we don't want to expose any failures to load any of the component types
			mode = (mode & LoadMode.MaskReturn) | LoadMode.ReturnNull;

			// we need to handle delegate methods here (for generic delegates)
			// (note that other types with manufactured inner classes such as Attribute and Enum can't be generic)
			if (name.EndsWith(DotNetTypeWrapper.DelegateInterfaceSuffix))
			{
				TypeWrapper outer = FindOrLoadGenericClass(name.Substring(0, name.Length - DotNetTypeWrapper.DelegateInterfaceSuffix.Length), mode);
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
			TypeWrapper def = LoadClass(name.Substring(0, pos), mode);
			if (def == null || !def.TypeAsTBD.IsGenericTypeDefinition)
			{
				return null;
			}
			Type type = def.TypeAsTBD;
			List<string> typeParamNames = new List<string>();
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
				TypeWrapper tw;
				switch(s[dims])
				{
					case 'L':
						tw = LoadClass(s.Substring(dims + 1), mode);
						if(tw == null)
						{
							return null;
						}
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
					default:
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
				type = type.MakeGenericType(typeArguments);
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

		protected virtual TypeWrapper LoadClassImpl(string name, LoadMode mode)
		{
			TypeWrapper tw = FindOrLoadGenericClass(name, mode);
			if(tw != null)
			{
				return tw;
			}
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			if((mode & LoadMode.Load) == 0)
			{
				return null;
			}
			Profiler.Enter("ClassLoader.loadClass");
			try
			{
				java.lang.Class c = GetJavaClassLoader().loadClassInternal(name);
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
				if((mode & LoadMode.MaskReturn) == LoadMode.ThrowClassNotFound)
				{
					throw new ClassLoadingException(ikvm.runtime.Util.mapException(x), name);
				}
				return null;
			}
			catch(java.lang.ThreadDeath)
			{
				throw;
			}
			catch(Exception x)
			{
				if((mode & LoadMode.SuppressExceptions) == 0)
				{
					throw new ClassLoadingException(ikvm.runtime.Util.mapException(x), name);
				}
				if(Tracer.ClassLoading.TraceError)
				{
					java.lang.ClassLoader cl = GetJavaClassLoader();
					if(cl != null)
					{
						System.Text.StringBuilder sb = new System.Text.StringBuilder();
						string sep = "";
						while(cl != null)
						{
							sb.Append(sep).Append(cl);
							sep = " -> ";
							cl = cl.getParent();
						}
						Tracer.Error(Tracer.ClassLoading, "ClassLoader chain: {0}", sb);
					}
					Exception m = ikvm.runtime.Util.mapException(x);
					Tracer.Error(Tracer.ClassLoading, m.ToString() + Environment.NewLine + m.StackTrace);
				}
				return null;
			}
			finally
			{
				Profiler.Leave("ClassLoader.loadClass");
			}
#else
			return null;
#endif
		}

		private static TypeWrapper CreateArrayType(string name, TypeWrapper elementTypeWrapper, int dims)
		{
			Debug.Assert(new String('[', dims) + elementTypeWrapper.SigName == name);
			Debug.Assert(!elementTypeWrapper.IsUnloadable && !elementTypeWrapper.IsVerifierType && !elementTypeWrapper.IsArray);
			Debug.Assert(dims >= 1);
			return elementTypeWrapper.GetClassLoader().RegisterInitiatingLoader(new ArrayTypeWrapper(elementTypeWrapper, name));
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal virtual java.lang.ClassLoader GetJavaClassLoader()
		{
#if FIRST_PASS
			return null;
#else
			return javaClassLoader;
#endif
		}
#endif

		// NOTE this exposes potentially unfinished types
		internal Type[] ArgTypeListFromSig(string sig)
		{
			if(sig[1] == ')')
			{
				return Type.EmptyTypes;
			}
			TypeWrapper[] wrappers = ArgTypeWrapperListFromSig(sig, LoadMode.LoadOrThrow);
			Type[] types = new Type[wrappers.Length];
			for(int i = 0; i < wrappers.Length; i++)
			{
				types[i] = wrappers[i].TypeAsSignatureType;
			}
			return types;
		}

		// NOTE: this will ignore anything following the sig marker (so that it can be used to decode method signatures)
		private TypeWrapper SigDecoderWrapper(ref int index, string sig, LoadMode mode)
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
					return LoadClass(sig.Substring(pos, index - pos - 1), mode);
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
							return LoadClass(array + sig.Substring(pos, index - pos), mode);
						}
						case 'B':
						case 'C':
						case 'D':
						case 'F':
						case 'I':
						case 'J':
						case 'S':
						case 'Z':
							return LoadClass(array + sig[index++], mode);
						default:
							throw new InvalidOperationException(sig.Substring(index));
					}
				}
				default:
					throw new InvalidOperationException(sig.Substring(index));
			}
		}

		internal TypeWrapper FieldTypeWrapperFromSig(string sig, LoadMode mode)
		{
			int index = 0;
			return SigDecoderWrapper(ref index, sig, mode);
		}

		internal TypeWrapper RetTypeWrapperFromSig(string sig, LoadMode mode)
		{
			int index = sig.IndexOf(')') + 1;
			return SigDecoderWrapper(ref index, sig, mode);
		}

		internal TypeWrapper[] ArgTypeWrapperListFromSig(string sig, LoadMode mode)
		{
			if(sig[1] == ')')
			{
				return TypeWrapper.EmptyArray;
			}
			List<TypeWrapper> list = new List<TypeWrapper>();
			for(int i = 1; sig[i] != ')';)
			{
				list.Add(SigDecoderWrapper(ref i, sig, mode));
			}
			return list.ToArray();
		}

#if STATIC_COMPILER || STUB_GENERATOR
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

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal static ClassLoaderWrapper GetClassLoaderWrapper(java.lang.ClassLoader javaClassLoader)
		{
			if(javaClassLoader == null)
			{
				return GetBootstrapClassLoader();
			}
			lock(wrapperLock)
			{
#if FIRST_PASS
				ClassLoaderWrapper wrapper = null;
#else
				ClassLoaderWrapper wrapper = 
#if __MonoCS__
					// MONOBUG the redundant cast to ClassLoaderWrapper is to workaround an mcs bug
					(ClassLoaderWrapper)(object)
#endif
					javaClassLoader.wrapper;
#endif
				if(wrapper == null)
				{
					CodeGenOptions opt = CodeGenOptions.None;
					if(JVM.EmitSymbols)
					{
						opt |= CodeGenOptions.Debug;
					}
#if NET_4_0
					if (!AppDomain.CurrentDomain.IsFullyTrusted)
					{
						opt |= CodeGenOptions.NoAutomagicSerialization;
					}
#endif
					wrapper = new ClassLoaderWrapper(opt, javaClassLoader);
					SetWrapperForClassLoader(javaClassLoader, wrapper);
				}
				return wrapper;
			}
		}
#endif

#if CLASSGC
		internal static ClassLoaderWrapper GetClassLoaderForDynamicJavaAssembly(Assembly asm)
		{
			ClassLoaderWrapper loader;
			dynamicAssemblies.TryGetValue(asm, out loader);
			return loader;
		}
#endif // CLASSGC

		internal static TypeWrapper GetWrapperFromType(Type type)
		{
#if STATIC_COMPILER
			if (type.__ContainsMissingType)
			{
				return new UnloadableTypeWrapper(type);
			}
#endif
			//Tracer.Info(Tracer.Runtime, "GetWrapperFromType: {0}", type.AssemblyQualifiedName);
#if !STATIC_COMPILER
			TypeWrapper.AssertFinished(type);
#endif
			Debug.Assert(!type.IsPointer);
			Debug.Assert(!type.IsByRef);
			TypeWrapper wrapper;
			lock(globalTypeToTypeWrapper)
			{
				globalTypeToTypeWrapper.TryGetValue(type, out wrapper);
			}
			if(wrapper != null)
			{
				return wrapper;
			}
#if STUB_GENERATOR
			if(type.__IsMissing || type.__ContainsMissingType)
			{
				wrapper = new UnloadableTypeWrapper("Missing/" + type.Assembly.FullName);
				globalTypeToTypeWrapper.Add(type, wrapper);
				return wrapper;
			}
#endif
			string remapped;
			if(remappedTypes.TryGetValue(type, out remapped))
			{
				wrapper = LoadClassCritical(remapped);
			}
			else if(ReflectUtil.IsVector(type))
			{
				// it might be an array of a dynamically compiled Java type
				int rank = 1;
				Type elem = type.GetElementType();
				while(ReflectUtil.IsVector(elem))
				{
					rank++;
					elem = elem.GetElementType();
				}
				wrapper = GetWrapperFromType(elem).MakeArrayType(rank);
			}
			else
			{
				Assembly asm = type.Assembly;
#if CLASSGC
				ClassLoaderWrapper loader = null;
				if(dynamicAssemblies != null && dynamicAssemblies.TryGetValue(asm, out loader))
				{
					lock(loader.typeToTypeWrapper)
					{
						TypeWrapper tw;
						if(loader.typeToTypeWrapper.TryGetValue(type, out tw))
						{
							return tw;
						}
						// it must be an anonymous type then
						Debug.Assert(AnonymousTypeWrapper.IsAnonymous(type));
					}
				}
#endif
#if !STATIC_COMPILER && !STUB_GENERATOR
				if(AnonymousTypeWrapper.IsAnonymous(type))
				{
					Dictionary<Type, TypeWrapper> typeToTypeWrapper;
#if CLASSGC
					typeToTypeWrapper = loader != null ? loader.typeToTypeWrapper : globalTypeToTypeWrapper;
#else
					typeToTypeWrapper = globalTypeToTypeWrapper;
#endif
					TypeWrapper tw = new AnonymousTypeWrapper(type);
					lock(typeToTypeWrapper)
					{
						if(!typeToTypeWrapper.TryGetValue(type, out wrapper))
						{
							typeToTypeWrapper.Add(type, wrapper = tw);
						}
					}
					return wrapper;
				}
				if(ReflectUtil.IsReflectionOnly(type))
				{
					// historically we've always returned null for types that don't have a corresponding TypeWrapper (or java.lang.Class)
					return null;
				}
#endif
				// if the wrapper doesn't already exist, that must mean that the type
				// is a .NET type (or a pre-compiled Java class), which means that it
				// was "loaded" by an assembly classloader
				wrapper = AssemblyClassLoader.FromAssembly(asm).GetWrapperFromAssemblyType(type);
			}
#if CLASSGC
			if(type.Assembly.IsDynamic)
			{
				// don't cache types in dynamic assemblies, because they might live in a RunAndCollect assembly
				// TODO we also shouldn't cache generic type instances that have a GCable type parameter
				return wrapper;
			}
#endif
			lock(globalTypeToTypeWrapper)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					globalTypeToTypeWrapper[type] = wrapper;
				}
			}
			return wrapper;
		}

		internal static ClassLoaderWrapper GetGenericClassLoader(TypeWrapper wrapper)
		{
			Type type = wrapper.TypeAsTBD;
			Debug.Assert(type.IsGenericType);
			Debug.Assert(!type.ContainsGenericParameters);

			List<ClassLoaderWrapper> list = new List<ClassLoaderWrapper>();
			list.Add(AssemblyClassLoader.FromAssembly(type.Assembly));
			foreach(Type arg in type.GetGenericArguments())
			{
				ClassLoaderWrapper loader = GetWrapperFromType(arg).GetClassLoader();
				if(!list.Contains(loader) && loader != bootstrapClassLoader)
				{
					list.Add(loader);
				}
			}
			ClassLoaderWrapper[] key = list.ToArray();
			ClassLoaderWrapper matchingLoader = GetGenericClassLoaderByKey(key);
			matchingLoader.RegisterInitiatingLoader(wrapper);
			return matchingLoader;
		}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		internal static object DoPrivileged(java.security.PrivilegedAction action)
		{
			return java.security.AccessController.doPrivileged(action, ikvm.@internal.CallerID.create(typeof(java.lang.ClassLoader).TypeHandle));
		}
#endif

		private static ClassLoaderWrapper GetGenericClassLoaderByKey(ClassLoaderWrapper[] key)
		{
			lock(wrapperLock)
			{
				if(genericClassLoaders == null)
				{
					genericClassLoaders = new List<GenericClassLoaderWrapper>();
				}
				foreach(GenericClassLoaderWrapper loader in genericClassLoaders)
				{
					if(loader.Matches(key))
					{
						return loader;
					}
				}
#if STATIC_COMPILER || STUB_GENERATOR || FIRST_PASS
				GenericClassLoaderWrapper newLoader = new GenericClassLoaderWrapper(key, null);
#else
				java.lang.ClassLoader javaClassLoader = new ikvm.runtime.GenericClassLoader();
				GenericClassLoaderWrapper newLoader = new GenericClassLoaderWrapper(key, javaClassLoader);
				SetWrapperForClassLoader(javaClassLoader, newLoader);
#endif
				genericClassLoaders.Add(newLoader);
				return newLoader;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		protected internal static void SetWrapperForClassLoader(java.lang.ClassLoader javaClassLoader, ClassLoaderWrapper wrapper)
		{
#if __MonoCS__ || FIRST_PASS
			typeof(java.lang.ClassLoader).GetField("wrapper", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(javaClassLoader, wrapper);
#else
			javaClassLoader.wrapper = wrapper;
#endif
		}
#endif

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal static ClassLoaderWrapper GetGenericClassLoaderByName(string name)
		{
			Debug.Assert(name.StartsWith("[[") && name.EndsWith("]]"));
			Stack<List<ClassLoaderWrapper>> stack = new Stack<List<ClassLoaderWrapper>>();
			List<ClassLoaderWrapper> list = null;
			for(int i = 0; i < name.Length; i++)
			{
				if(name[i] == '[')
				{
					if(name[i + 1] == '[')
					{
						stack.Push(list);
						list = new List<ClassLoaderWrapper>();
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
					ClassLoaderWrapper loader = GetGenericClassLoaderByKey(list.ToArray());
					list = stack.Pop();
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
			return AssemblyClassLoader.FromAssembly(Assembly.Load(name));
		}
#endif

		internal static int GetGenericClassLoaderId(ClassLoaderWrapper wrapper)
		{
			lock(wrapperLock)
			{
				return genericClassLoaders.IndexOf(wrapper as GenericClassLoaderWrapper);
			}
		}

		internal static ClassLoaderWrapper GetGenericClassLoaderById(int id)
		{
			lock(wrapperLock)
			{
				return genericClassLoaders[id];
			}
		}

		internal void SetWrapperForType(Type type, TypeWrapper wrapper)
		{
#if !STATIC_COMPILER
			TypeWrapper.AssertFinished(type);
#endif
			Dictionary<Type, TypeWrapper> dict;
#if CLASSGC
			dict = typeToTypeWrapper ?? globalTypeToTypeWrapper;
#else
			dict = globalTypeToTypeWrapper;
#endif
			lock (dict)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					dict.Add(type, wrapper);
				}
			}
		}

		internal static TypeWrapper LoadClassCritical(string name)
		{
#if STATIC_COMPILER
			TypeWrapper wrapper = GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
			if (wrapper == null)
			{
				throw new FatalCompilerErrorException(Message.CriticalClassNotFound, name);
			}
			return wrapper;
#else
			try
			{
				return GetBootstrapClassLoader().LoadClassByDottedName(name);
			}
			catch(Exception x)
			{
				JVM.CriticalFailure("Loading of critical class failed", x);
				return null;
			}
#endif
		}

		internal void RegisterNativeLibrary(IntPtr p)
		{
			lock(this)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					if(nativeLibraries == null)
					{
						nativeLibraries = new List<IntPtr>();
					}
					nativeLibraries.Add(p);
				}
			}
		}

		internal void UnregisterNativeLibrary(IntPtr p)
		{
			lock(this)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					nativeLibraries.Remove(p);
				}
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
				return nativeLibraries.ToArray();
			}
		}

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		public override string ToString()
		{
			object javaClassLoader = GetJavaClassLoader();
			if(javaClassLoader == null)
			{
				return "null";
			}
			return String.Format("{0}@{1:X}", GetWrapperFromType(javaClassLoader.GetType()).Name, javaClassLoader.GetHashCode());
		}
#endif

		internal virtual bool InternalsVisibleToImpl(TypeWrapper wrapper, TypeWrapper friend)
		{
			Debug.Assert(wrapper.GetClassLoader() == this);
			return this == friend.GetClassLoader();
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		// this method is used by IKVM.Runtime.JNI
		internal static ClassLoaderWrapper FromCallerID(ikvm.@internal.CallerID callerID)
		{
#if FIRST_PASS
			return null;
#else
			return GetClassLoaderWrapper(callerID.getCallerClassLoader());
#endif
		}
#endif

#if STATIC_COMPILER
		internal virtual void IssueMessage(Message msgId, params string[] values)
		{
			// it's not ideal when we end up here (because it means we're emitting a warning that is not associated with a specific output target),
			// but it happens when we're decoding something in a referenced assembly that either doesn't make sense or contains an unloadable type
			StaticCompiler.IssueMessage(msgId, values);
		}
#endif

		internal void CheckPackageAccess(TypeWrapper tw, ProtectionDomain pd)
		{
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			if (javaClassLoader != null)
			{
				javaClassLoader.checkPackageAccess(tw.ClassObject, pd);
			}
#endif
		}

#if !STUB_GENERATOR
		internal ClassFileParseOptions ClassFileParseOptions
		{
			get
			{
#if STATIC_COMPILER
				ClassFileParseOptions cfp = ClassFileParseOptions.LocalVariableTable;
				if (EmitStackTraceInfo)
				{
					cfp |= ClassFileParseOptions.LineNumberTable;
				}
				if (bootstrapClassLoader is CompilerClassLoader)
				{
					cfp |= ClassFileParseOptions.TrustedAnnotations;
				}
				if (RemoveAsserts)
				{
					cfp |= ClassFileParseOptions.RemoveAssertions;
				}
				return cfp;
#else
				ClassFileParseOptions cfp = ClassFileParseOptions.LineNumberTable;
				if (EmitDebugInfo)
				{
					cfp |= ClassFileParseOptions.LocalVariableTable;
				}
				if (RelaxedClassNameValidation)
				{
					cfp |= ClassFileParseOptions.RelaxedClassNameValidation;
				}
				if (this == bootstrapClassLoader)
				{
					cfp |= ClassFileParseOptions.TrustedAnnotations;
				}
				return cfp;
#endif
			}
		}
#endif

#if STATIC_COMPILER
		internal virtual bool WarningLevelHigh
		{
			get { return false; }
		}

		internal virtual bool NoParameterReflection
		{
			get { return false; }
		}
#endif
	}

	sealed class GenericClassLoaderWrapper : ClassLoaderWrapper
	{
		private readonly ClassLoaderWrapper[] delegates;

		internal GenericClassLoaderWrapper(ClassLoaderWrapper[] delegates, object javaClassLoader)
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

		protected override TypeWrapper FindLoadedClassLazy(string name)
		{
			TypeWrapper tw1 = FindOrLoadGenericClass(name, LoadMode.Find);
			if (tw1 != null)
			{
				return tw1;
			}
			foreach (ClassLoaderWrapper loader in delegates)
			{
				TypeWrapper tw = loader.FindLoadedClass(name);
				if (tw != null && tw.GetClassLoader() == loader)
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
				GenericClassLoaderWrapper gcl = loader as GenericClassLoaderWrapper;
				if(gcl != null)
				{
					sb.Append(gcl.GetName());
				}
				else
				{
					sb.Append(((AssemblyClassLoader)loader).MainAssembly.FullName);
				}
				sb.Append(']');
			}
			sb.Append(']');
			return sb.ToString();
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal java.util.Enumeration GetResources(string name)
		{
#if FIRST_PASS
			return null;
#else
			java.util.Vector v = new java.util.Vector();
			foreach (java.net.URL url in GetBootstrapClassLoader().GetResources(name))
			{
				v.add(url);
			}
			if (name.EndsWith(".class", StringComparison.Ordinal) && name.IndexOf('.') == name.Length - 6)
			{
				TypeWrapper tw = FindLoadedClass(name.Substring(0, name.Length - 6).Replace('/', '.'));
				if (tw != null && !tw.IsArray && !tw.IsDynamic)
				{
					ClassLoaderWrapper loader = tw.GetClassLoader();
					if (loader is GenericClassLoaderWrapper)
					{
						v.add(new java.net.URL("ikvmres", "gen", ClassLoaderWrapper.GetGenericClassLoaderId(loader), "/" + name));
					}
					else if (loader is AssemblyClassLoader)
					{
						foreach (java.net.URL url in ((AssemblyClassLoader)loader).FindResources(name))
						{
							v.add(url);
						}
					}
				}
			}
			return v.elements();
#endif
		}

		internal java.net.URL FindResource(string name)
		{
#if !FIRST_PASS
			if (name.EndsWith(".class", StringComparison.Ordinal) && name.IndexOf('.') == name.Length - 6)
			{
				TypeWrapper tw = FindLoadedClass(name.Substring(0, name.Length - 6).Replace('/', '.'));
				if (tw != null && tw.GetClassLoader() == this && !tw.IsArray && !tw.IsDynamic)
				{
					return new java.net.URL("ikvmres", "gen", ClassLoaderWrapper.GetGenericClassLoaderId(this), "/" + name);
				}
			}
#endif
			return null;
		}
#endif
	}
}
