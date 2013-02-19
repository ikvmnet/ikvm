/*
  Copyright (C) 2002-2007, 2010 Jeroen Frijters

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
using System.IO;
using System.Reflection;
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;
using AssemblyClassLoader_ = IKVM.Internal.AssemblyClassLoader;
using jlClass = java.lang.Class;
#if !FIRST_PASS
using NegativeArraySizeException = java.lang.NegativeArraySizeException;
using IllegalArgumentException = java.lang.IllegalArgumentException;
using IllegalAccessException = java.lang.IllegalAccessException;
using NumberFormatException = java.lang.NumberFormatException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
using jlrConstructor = java.lang.reflect.Constructor;
using jlrField = java.lang.reflect.Field;
#endif

namespace IKVM.NativeCode.gnu.java.net.protocol.ikvmres
{
	static class Handler
	{
		public static Stream ReadResourceFromAssemblyImpl(Assembly asm, string resource)
		{
			// chop off the leading slash
			resource = resource.Substring(1);
			string mangledName = JVM.MangleResourceName(resource);
			ManifestResourceInfo info = asm.GetManifestResourceInfo(mangledName);
			if(info != null && info.FileName != null)
			{
				return asm.GetManifestResourceStream(mangledName);
			}
			Stream s = asm.GetManifestResourceStream(mangledName);
			if(s == null)
			{
				Tracer.Warning(Tracer.ClassLoading, "Resource \"{0}\" not found in {1}", resource, asm.FullName);
				throw new FileNotFoundException("resource " + resource + " not found in assembly " + asm.FullName);
			}
			switch (s.ReadByte())
			{
				case 0:
					Tracer.Info(Tracer.ClassLoading, "Reading resource \"{0}\" from {1}", resource, asm.FullName);
					return s;
				case 1:
					Tracer.Info(Tracer.ClassLoading, "Reading compressed resource \"{0}\" from {1}", resource, asm.FullName);
					return new System.IO.Compression.DeflateStream(s, System.IO.Compression.CompressionMode.Decompress, false);
				default:
					Tracer.Error(Tracer.ClassLoading, "Resource \"{0}\" in {1} has an unsupported encoding", resource, asm.FullName);
					throw new IOException("Unsupported resource encoding for resource " + resource + " found in assembly " + asm.FullName);
			}
		}

		public static object LoadClassFromAssembly(Assembly asm, string className)
		{
			TypeWrapper tw = AssemblyClassLoader.FromAssembly(asm).LoadClassByDottedNameFast(className);
			if(tw != null)
			{
				return tw.ClassObject;
			}
			return null;
		}

		public static Assembly LoadAssembly(string name)
		{
			return Assembly.Load(name);
		}

		public static global::java.lang.ClassLoader GetGenericClassLoaderById(int id)
		{
			return ClassLoaderWrapper.GetGenericClassLoaderById(id).GetJavaClassLoader();
		}
	}
}

namespace IKVM.NativeCode.java.lang
{
	static class VMSystemProperties
	{
		public static string getVirtualFileSystemRoot()
		{
			return VirtualFileSystem.RootPath;
		}

		public static string getBootClassPath()
		{
#if FIRST_PASS
			return null;
#else
			return VirtualFileSystem.GetAssemblyClassesPath(JVM.CoreAssembly);
#endif
		}
	}
}

namespace IKVM.NativeCode.ikvm.@internal
{
	static class CallerID
	{
		public static jlClass GetClass(object obj)
		{
			return ClassLoaderWrapper.GetWrapperFromType(obj.GetType().DeclaringType).ClassObject;
		}

		public static global::java.lang.ClassLoader GetClassLoader(object obj)
		{
			return ClassLoaderWrapper.GetWrapperFromType(obj.GetType().DeclaringType).GetClassLoader().GetJavaClassLoader();
		}

		public static global::java.lang.ClassLoader GetAssemblyClassLoader(Assembly asm)
		{
			return AssemblyClassLoader.FromAssembly(asm).GetJavaClassLoader();
		}
	}

	namespace stubgen
	{
		static class StubGenerator
		{
			public static int getRealModifiers(jlClass c)
			{
				return (int)TypeWrapper.FromClass(c).Modifiers;
			}

			public static string getAssemblyName(jlClass c)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(c);
				ClassLoaderWrapper loader = wrapper.GetClassLoader();
				IKVM.Internal.AssemblyClassLoader acl = loader as IKVM.Internal.AssemblyClassLoader;
				if(acl != null)
				{
					return acl.GetAssembly(wrapper).FullName;
				}
				else
				{
					return ((IKVM.Internal.GenericClassLoader)loader).GetName();
				}
			}

			public static object getFieldConstantValue(object field)
			{
				return FieldWrapper.FromField(field).GetConstant();
			}

			public static bool isFieldDeprecated(object field)
			{
				FieldWrapper fieldWrapper = FieldWrapper.FromField(field);
				FieldInfo fi = fieldWrapper.GetField();
				if(fi != null)
				{
					return fi.IsDefined(typeof(ObsoleteAttribute), false);
				}
				return false;
			}

			public static bool isMethodDeprecated(object method)
			{
				MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(method);
				MethodBase mb = mw.GetMethod();
				return mb != null && mb.IsDefined(typeof(ObsoleteAttribute), false);
			}

			public static bool isClassDeprecated(jlClass clazz)
			{
				Type type = TypeWrapper.FromClass(clazz).TypeAsTBD;
				// we need to check type for null, because ReflectionOnly
				// generated delegate inner interfaces don't really exist
				return type != null && type.IsDefined(typeof(ObsoleteAttribute), false);
			}
		}
	}
}

namespace IKVM.NativeCode.ikvm.runtime
{
	static class AssemblyClassLoader
	{
		public static global::java.lang.Class LoadClass(global::java.lang.ClassLoader classLoader, Assembly assembly, string name)
		{
#if !FIRST_PASS
			if (!global::java.lang.ClassLoader.checkName(name))
			{
				throw new global::java.lang.ClassNotFoundException(name);
			}
#endif
			try
			{
				TypeWrapper tw = null;
				if(classLoader == null)
				{
					tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(name);
				}
				else if(assembly != null)
				{
					AssemblyClassLoader_ acl = global::IKVM.Internal.AssemblyClassLoader.FromAssembly(assembly);
					tw = acl.FindLoadedClass(name);
					if(tw == null)
					{
						tw = acl.FindOrLoadGenericClass(name, false);
					}
					if(tw == null)
					{
						tw = acl.LoadReferenced(name);
					}
					if(tw == null)
					{
						tw = acl.LoadDynamic(name);
					}
					if(tw == null)
					{
						throw new ClassNotFoundException(name);
					}
				}
				else
				{
					// this must be a GenericClassLoader
					tw = ((GenericClassLoader)ClassLoaderWrapper.GetClassLoaderWrapper(classLoader)).LoadClassByDottedName(name);
				}
				Tracer.Info(Tracer.ClassLoading, "Loaded class \"{0}\" from {1}", name, classLoader == null ? "boot class loader" : (object)classLoader);
				return tw.ClassObject;
			}
			catch(RetargetableJavaException x)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, classLoader == null ? "boot class loader" : (object)classLoader);
				throw x.ToJava();
			}
		}

		public static global::java.net.URL GetManifest(Assembly assembly)
		{
#if FIRST_PASS
			return null;
#else
			IKVM.Internal.AssemblyClassLoader wrapper = IKVM.Internal.AssemblyClassLoader.FromAssembly(assembly);
			foreach (global::java.net.URL url in wrapper.FindResources("META-INF/MANIFEST.MF"))
			{
				return url;
			}
			return null;
#endif
		}

		public static global::java.net.URL getResource(global::java.lang.ClassLoader classLoader, Assembly assembly, string name)
		{
#if FIRST_PASS
			return null;
#else
			if (assembly != null)
			{
				IKVM.Internal.AssemblyClassLoader wrapper = IKVM.Internal.AssemblyClassLoader.FromAssembly(assembly);
				foreach (global::java.net.URL url in wrapper.GetResources(name))
				{
					return url;
				}
			}
			return GetClassResource(classLoader, assembly, name);
#endif
		}

		public static global::java.util.Enumeration getResources(global::java.lang.ClassLoader classLoader, Assembly assembly, string name)
		{
#if FIRST_PASS
			return null;
#else
			global::java.util.Vector v = new global::java.util.Vector();
			if (assembly != null)
			{
				IKVM.Internal.AssemblyClassLoader wrapper = IKVM.Internal.AssemblyClassLoader.FromAssembly(assembly);
				foreach (global::java.net.URL url in wrapper.GetResources(name))
				{
					v.addElement(url);
				}
			}
			// we'll only generate a stub class if there isn't already a resource with this name
			if (v.isEmpty())
			{
				global::java.net.URL curl = GetClassResource(classLoader, assembly, name);
				if (curl != null)
				{
					v.addElement(curl);
				}
			}
			return v.elements();
#endif
		}

#if !FIRST_PASS
		private static global::java.net.URL GetClassResource(global::java.lang.ClassLoader classLoader, Assembly assembly, string name)
		{
			if (name.EndsWith(".class", StringComparison.Ordinal) && name.IndexOf('.') == name.Length - 6)
			{
				global::java.lang.Class c = null;
				try
				{
					c = LoadClass(classLoader, assembly, name.Substring(0, name.Length - 6).Replace('/', '.'));
				}
				catch (global::java.lang.ClassNotFoundException)
				{
				}
				catch (global::java.lang.LinkageError)
				{
				}
				if (c != null && !IsDynamic(c))
				{
					assembly = GetAssemblyFromClass(c);
					try
					{
						if (assembly != null)
						{
							return new global::java.io.File(VirtualFileSystem.GetAssemblyClassesPath(assembly) + name).toURI().toURL();
						}
						else
						{
							// HACK we use an index to identify the generic class loader in the url
							// TODO this obviously isn't persistable, we should use a list of assemblies instead.
							return new global::java.net.URL("ikvmres", "gen", GetGenericClassLoaderId(c.getClassLoader()), "/" + name);
						}
					}
					catch (global::java.net.MalformedURLException x)
					{
						throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(x);
					}
				}
			}
			return null;
		}
#endif

		private static Assembly GetAssemblyFromClass(jlClass clazz)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
			AssemblyClassLoader_ acl = wrapper.GetClassLoader() as AssemblyClassLoader_;
			return acl != null ? acl.GetAssembly(wrapper) : null;
		}

		private static bool IsDynamic(jlClass clazz)
		{
			return TypeWrapper.FromClass(clazz) is DynamicTypeWrapper;
		}

		// NOTE the array may contain duplicates!
		public static string[] GetPackages(Assembly assembly)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = IKVM.Internal.AssemblyClassLoader.FromAssembly(assembly);
			string[] packages = new string[0];
			foreach(Module m in wrapper.MainAssembly.GetModules(false))
			{
				object[] attr = m.GetCustomAttributes(typeof(PackageListAttribute), false);
				foreach(PackageListAttribute p in attr)
				{
					string[] mp = p.GetPackages();
					string[] tmp = new string[packages.Length + mp.Length];
					Array.Copy(packages, 0, tmp, 0, packages.Length);
					Array.Copy(mp, 0, tmp, packages.Length, mp.Length);
					packages = tmp;
				}
			}
			return packages;
		}

		public static int GetGenericClassLoaderId(global::java.lang.ClassLoader classLoader)
		{
#if FIRST_PASS
			return 0;
#else
			return ClassLoaderWrapper.GetGenericClassLoaderId(ClassLoaderWrapper.GetClassLoaderWrapper(classLoader));
#endif
		}

		public static string GetGenericClassLoaderName(global::java.lang.ClassLoader classLoader)
		{
#if FIRST_PASS
			return null;
#else
			return ((GenericClassLoader)ClassLoaderWrapper.GetClassLoaderWrapper(classLoader)).GetName();
#endif
		}

		public static global::java.lang.ClassLoader getAssemblyClassLoader(Assembly asm)
		{
			// note that we don't do a security check here, because if you have the Assembly object,
			// you can already get at all the types in it.
			return AssemblyClassLoader_.FromAssembly(asm).GetJavaClassLoader();
		}
	}

	static class AppDomainAssemblyClassLoader
	{
		public static object loadClassFromAssembly(Assembly asm, string className)
		{
			if(ReflectUtil.IsDynamicAssembly(asm))
			{
				return null;
			}
			TypeWrapper tw = IKVM.Internal.AssemblyClassLoader.FromAssembly(asm).DoLoad(className);
			return tw != null ? tw.ClassObject : null;
		}

		private static IEnumerable<global::java.net.URL> FindResources(string name)
		{
			List<IKVM.Internal.AssemblyClassLoader> done = new List<IKVM.Internal.AssemblyClassLoader>();
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (!ReflectUtil.IsDynamicAssembly(asm))
				{
					IKVM.Internal.AssemblyClassLoader acl = IKVM.Internal.AssemblyClassLoader.FromAssembly(asm);
					if (!done.Contains(acl))
					{
						done.Add(acl);
						foreach (global::java.net.URL url in acl.FindResources(name))
						{
							yield return url;
						}
					}
				}
			}
		}

		public static global::java.net.URL findResource(object thisObj, string name)
		{
			foreach (global::java.net.URL url in FindResources(name))
			{
				return url;
			}
			return null;
		}

		public static void getResources(global::java.util.Vector v, string name)
		{
#if !FIRST_PASS
			foreach (global::java.net.URL url in FindResources(name))
			{
				if (url != null && !v.contains(url))
				{
					v.add(url);
				}
			}
#endif
		}
	}

	static class Util
	{
		public static jlClass getClassFromObject(object o)
		{
			return GetTypeWrapperFromObject(o).ClassObject;
		}

		internal static TypeWrapper GetTypeWrapperFromObject(object o)
		{
			TypeWrapper ghostType = GhostTag.GetTag(o);
			if(ghostType != null)
			{
				return ghostType;
			}
			Type t = o.GetType();
			if(t.IsPrimitive || (ClassLoaderWrapper.IsRemappedType(t) && !t.IsSealed))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t);
			}
			for(; ; )
			{
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(t);
				// if GetWrapperFromType returns null (or if tw.IsAbstract), that
				// must mean that the Type of the object is an implementation helper class
				// (e.g. an AtomicReferenceFieldUpdater or ThreadLocal instrinsic subclass)
				if(tw != null && (!tw.IsAbstract || tw.IsArray))
				{
					return tw;
				}
				t = t.BaseType;
			}
		}

		public static jlClass getClassFromTypeHandle(RuntimeTypeHandle handle)
		{
			Type t = Type.GetTypeFromHandle(handle);
			if(t.IsPrimitive || ClassLoaderWrapper.IsRemappedType(t) || t == typeof(void))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).ClassObject;
			}
			if(!IsVisibleAsClass(t))
			{
				return null;
			}
			TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(t);
			if(tw != null)
			{
				return tw.ClassObject;
			}
			return null;
		}

		public static jlClass getClassFromTypeHandle(RuntimeTypeHandle handle, int rank)
		{
			Type t = Type.GetTypeFromHandle(handle);
			if(t.IsPrimitive || ClassLoaderWrapper.IsRemappedType(t) || t == typeof(void))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).MakeArrayType(rank).ClassObject;
			}
			if(!IsVisibleAsClass(t))
			{
				return null;
			}
			TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(t);
			if(tw != null)
			{
				return tw.MakeArrayType(rank).ClassObject;
			}
			return null;
		}

		public static jlClass getFriendlyClassFromType(Type type)
		{
			int rank = 0;
			while(ReflectUtil.IsVector(type))
			{
				type = type.GetElementType();
				rank++;
			}
			if(type.DeclaringType != null
				&& AttributeHelper.IsGhostInterface(type.DeclaringType))
			{
				type = type.DeclaringType;
			}
			if(!IsVisibleAsClass(type))
			{
				return null;
			}
			TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
			if(wrapper == null)
			{
				return null;
			}
			if(rank > 0)
			{
				wrapper = wrapper.MakeArrayType(rank);
			}
			return wrapper.ClassObject;
		}

		private static bool IsVisibleAsClass(Type type)
		{
			while (type.HasElementType)
			{
				if (type.IsPointer || type.IsByRef)
				{
					return false;
				}
				type = type.GetElementType();
			}
			if (type.ContainsGenericParameters && !type.IsGenericTypeDefinition)
			{
				return false;
			}
			System.Reflection.Emit.TypeBuilder tb = type as System.Reflection.Emit.TypeBuilder;
			if (tb != null && !tb.IsCreated())
			{
				return false;
			}
			return true;
		}

		public static Type getInstanceTypeFromClass(jlClass clazz)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
			if(wrapper.IsRemapped && wrapper.IsFinal)
			{
				return wrapper.TypeAsTBD;
			}
			return wrapper.TypeAsBaseType;
		}

		[HideFromJava]
		public static Exception mapException(Exception x)
		{
			return ExceptionHelper.MapException<Exception>(x, true, false);
		}

		public static Exception unmapException(Exception x)
		{
			return ExceptionHelper.UnmapException(x);
		}
	}
}
