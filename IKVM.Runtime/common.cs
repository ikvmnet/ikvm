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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
		public static byte[] GenerateStub(jlClass c)
		{
			MemoryStream mem = new MemoryStream();
#if !FIRST_PASS
			bool includeNonPublicInterfaces = !"true".Equals(global::java.lang.Props.props.getProperty("ikvm.stubgen.skipNonPublicInterfaces"), StringComparison.OrdinalIgnoreCase);
			IKVM.StubGen.StubGenerator.WriteClass(mem, TypeWrapper.FromClass(c), includeNonPublicInterfaces, false, false, true);
#endif
			return mem.ToArray();
		}

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
			return VirtualFileSystem.GetAssemblyClassesPath(JVM.CoreAssembly);
		}

		public static string getStdoutEncoding()
		{
			return IsWindowsConsole(true) ? GetConsoleEncoding() : null;
		}

		public static string getStderrEncoding()
		{
			return IsWindowsConsole(false) ? GetConsoleEncoding() : null;
		}

		public static FileVersionInfo getKernel32FileVersionInfo()
		{
			try
			{
				foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
				{
					if (string.Compare(module.ModuleName, "kernel32.dll", StringComparison.OrdinalIgnoreCase) == 0)
					{
						return module.FileVersionInfo;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		private static bool IsWindowsConsole(bool stdout)
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				return false;
			}
			// these properties are available starting with .NET 4.5
			PropertyInfo pi = typeof(Console).GetProperty(stdout ? "IsOutputRedirected" : "IsErrorRedirected");
			if (pi != null)
			{
				return !(bool)pi.GetValue(null, null);
			}
			const int STD_OUTPUT_HANDLE = -11;
			const int STD_ERROR_HANDLE = -12;
			IntPtr handle = GetStdHandle(stdout ? STD_OUTPUT_HANDLE : STD_ERROR_HANDLE);
			if (handle == IntPtr.Zero)
			{
				return false;
			}
			const int FILE_TYPE_CHAR = 2;
			return GetFileType(handle) == FILE_TYPE_CHAR;
		}

		private static string GetConsoleEncoding()
		{
			int codepage = Console.InputEncoding.CodePage;
			return codepage >= 847 && codepage <= 950
				? "ms" + codepage
				: "cp" + codepage;
		}

		[DllImport("kernel32")]
		private static extern int GetFileType(IntPtr hFile);

		[DllImport("kernel32")]
		private static extern IntPtr GetStdHandle(int nStdHandle);
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

	static class AnnotationAttributeBase
	{
		public static object[] unescapeInvalidSurrogates(object[] def)
		{
			return (object[])AnnotationDefaultAttribute.Unescape(def);
		}

		public static object newAnnotationInvocationHandler(jlClass type, object memberValues)
		{
#if FIRST_PASS
			return null;
#else
			return new global::sun.reflect.annotation.AnnotationInvocationHandler(type, (global::java.util.Map)memberValues);
#endif
		}

		public static object newAnnotationTypeMismatchExceptionProxy(string msg)
		{
#if FIRST_PASS
			return null;
#else
			return new global::sun.reflect.annotation.AnnotationTypeMismatchExceptionProxy(msg);
#endif
		}
	}
}

namespace IKVM.NativeCode.ikvm.runtime
{
	static class AssemblyClassLoader
	{
		public static void setWrapper(global::java.lang.ClassLoader _this, Assembly assembly)
		{
			ClassLoaderWrapper.SetWrapperForClassLoader(_this, IKVM.Internal.AssemblyClassLoader.FromAssembly(assembly));
		}

		public static global::java.lang.Class loadClass(global::java.lang.ClassLoader _this, string name, bool resolve)
		{
#if FIRST_PASS
			return null;
#else
			try
			{
				if (!global::java.lang.ClassLoader.checkName(name))
				{
					throw new ClassNotFoundException(name);
				}
				AssemblyClassLoader_ wrapper = (AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this);
				TypeWrapper tw = wrapper.LoadClass(name);
				if (tw == null)
				{
					Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, _this);
					global::java.lang.Throwable.suppressFillInStackTrace = true;
					throw new global::java.lang.ClassNotFoundException(name);
				}
				Tracer.Info(Tracer.ClassLoading, "Loaded class \"{0}\" from {1}", name, _this);
				return tw.ClassObject;
			}
			catch (ClassNotFoundException x)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, _this);
				throw new global::java.lang.ClassNotFoundException(x.Message);
			}
			catch (ClassLoadingException x)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, _this);
				throw x.InnerException;
			}
			catch (RetargetableJavaException x)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, _this);
				throw x.ToJava();
			}
#endif
		}

		public static global::java.net.URL getResource(global::java.lang.ClassLoader _this, string name)
		{
#if !FIRST_PASS
			AssemblyClassLoader_ wrapper = (AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this);
			foreach (global::java.net.URL url in wrapper.GetResources(name))
			{
				return url;
			}
#endif
			return null;
		}

		public static global::java.util.Enumeration getResources(global::java.lang.ClassLoader _this, string name)
		{
#if FIRST_PASS
			return null;
#else
			return new global::ikvm.runtime.EnumerationWrapper(((AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this)).GetResources(name));
#endif
		}

		public static global::java.net.URL findResource(global::java.lang.ClassLoader _this, string name)
		{
#if !FIRST_PASS
			AssemblyClassLoader_ wrapper = (AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this);
			foreach (global::java.net.URL url in wrapper.FindResources(name))
			{
				return url;
			}
#endif
			return null;
		}

		public static global::java.util.Enumeration findResources(global::java.lang.ClassLoader _this, string name)
		{
#if FIRST_PASS
			return null;
#else
			return new global::ikvm.runtime.EnumerationWrapper(((AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this)).FindResources(name));
#endif
		}

#if !FIRST_PASS
		private static global::java.net.URL GetCodeBase(Assembly assembly)
		{
			try
			{
				return new global::java.net.URL(assembly.CodeBase);
			}
			catch (NotSupportedException)
			{
			}
			catch (global::java.net.MalformedURLException)
			{
			}
			return null;
		}

		private static string GetAttributeValue(global::java.util.jar.Attributes.Name name, global::java.util.jar.Attributes first, global::java.util.jar.Attributes second)
		{
			string result = null;
			if (first != null)
			{
				result = first.getValue(name);
			}
			if (second != null && result == null)
			{
				result = second.getValue(name);
			}
			return result;
		}
#endif

		public static void lazyDefinePackages(global::java.lang.ClassLoader _this)
		{
#if !FIRST_PASS
			AssemblyClassLoader_ wrapper = (AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this);
			global::java.net.URL sealBase = GetCodeBase(wrapper.MainAssembly);
			foreach (KeyValuePair<string, string[]> packages in wrapper.GetPackageInfo())
			{
				global::java.util.jar.Manifest manifest = null;
				global::java.util.jar.Attributes attr = null;
				if (packages.Key != null)
				{
					global::java.util.jar.JarFile jarFile = new global::java.util.jar.JarFile(VirtualFileSystem.GetAssemblyResourcesPath(wrapper.MainAssembly) + packages.Key);
					manifest = jarFile.getManifest();
				}
				if (manifest != null)
				{
					attr = manifest.getMainAttributes();
				}
				foreach (string name in packages.Value)
				{
					if (_this.getPackage(name) == null)
					{
						global::java.util.jar.Attributes entryAttr = null;
						if (manifest != null)
						{
							entryAttr = manifest.getAttributes(name.Replace('.', '/') + '/');
						}
						_this.definePackage(name,
							GetAttributeValue(global::java.util.jar.Attributes.Name.SPECIFICATION_TITLE, entryAttr, attr),
							GetAttributeValue(global::java.util.jar.Attributes.Name.SPECIFICATION_VERSION, entryAttr, attr),
							GetAttributeValue(global::java.util.jar.Attributes.Name.SPECIFICATION_VENDOR, entryAttr, attr),
							GetAttributeValue(global::java.util.jar.Attributes.Name.IMPLEMENTATION_TITLE, entryAttr, attr),
							GetAttributeValue(global::java.util.jar.Attributes.Name.IMPLEMENTATION_VERSION, entryAttr, attr),
							GetAttributeValue(global::java.util.jar.Attributes.Name.IMPLEMENTATION_VENDOR, entryAttr, attr),
							"true".Equals(GetAttributeValue(global::java.util.jar.Attributes.Name.SEALED, entryAttr, attr), StringComparison.OrdinalIgnoreCase) ? sealBase : null);
					}
				}
			}
#endif
		}

		public static string toString(global::java.lang.ClassLoader _this)
		{
			return ((AssemblyClassLoader_)ClassLoaderWrapper.GetClassLoaderWrapper(_this)).MainAssembly.FullName;
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

	static class GenericClassLoader
	{
		public static string toString(global::java.lang.ClassLoader _this)
		{
			return ((GenericClassLoaderWrapper)ClassLoaderWrapper.GetClassLoaderWrapper(_this)).GetName();
		}

		public static global::java.util.Enumeration getResources(global::java.lang.ClassLoader _this, string name)
		{
			return ((GenericClassLoaderWrapper)ClassLoaderWrapper.GetClassLoaderWrapper(_this)).GetResources(name);
		}

		public static global::java.net.URL findResource(global::java.lang.ClassLoader _this, string name)
		{
			return ((GenericClassLoaderWrapper)ClassLoaderWrapper.GetClassLoaderWrapper(_this)).FindResource(name);
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
