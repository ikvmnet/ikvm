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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Runtime.Serialization;
using SystemArray = System.Array;
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;
using AssemblyClassLoader_ = IKVM.Internal.AssemblyClassLoader;
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
	public class Handler
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
			TypeWrapper tw = ClassLoaderWrapper.GetAssemblyClassLoader(asm).LoadClassByDottedNameFast(className);
			if(tw != null)
			{
				return tw.ClassObject;
			}
			return null;
		}

		public static Assembly LoadAssembly(string name)
		{
			if(name.EndsWith("[ReflectionOnly]"))
			{
				return Assembly.ReflectionOnlyLoad(name.Substring(0, name.Length - 16));
			}
			return Assembly.Load(name);
		}

		public static object GetGenericClassLoaderById(int id)
		{
			return ClassLoaderWrapper.GetGenericClassLoaderById(id).GetJavaClassLoader();
		}
	}
}

namespace IKVM.NativeCode.gnu.classpath
{
	public class VMSystemProperties
	{
		public static string getVersion()
		{
			try
			{
				return JVM.SafeGetAssemblyVersion(typeof(VMSystemProperties).Assembly).ToString();
			}
			catch(Exception)
			{
				return "(unknown)";
			}
		}
	}
}

namespace IKVM.NativeCode.ikvm.@internal
{
	namespace stubgen
	{
		public class StubGenerator
		{
			public static string getAssemblyName(object c)
			{
				ClassLoaderWrapper loader = TypeWrapper.FromClass(c).GetClassLoader();
				IKVM.Internal.AssemblyClassLoader acl = loader as IKVM.Internal.AssemblyClassLoader;
				if(acl != null)
				{
					return acl.Assembly.FullName;
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
					return AttributeHelper.IsDefined(fi, typeof(ObsoleteAttribute));
				}
				GetterFieldWrapper getter = fieldWrapper as GetterFieldWrapper;
				if(getter != null)
				{
					return AttributeHelper.IsDefined(getter.GetProperty(), typeof(ObsoleteAttribute));
				}
				return false;
			}

			public static bool isMethodDeprecated(object method)
			{
				MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(method);
				MethodBase mb = mw.GetMethod();
				return mb != null && AttributeHelper.IsDefined(mb, typeof(ObsoleteAttribute));
			}

			public static bool isClassDeprecated(object clazz)
			{
				Type type = TypeWrapper.FromClass(clazz).TypeAsTBD;
				// we need to check type for null, because ReflectionOnly
				// generated delegate inner interfaces don't really exist
				return type != null && AttributeHelper.IsDefined(type, typeof(ObsoleteAttribute));
			}
		}
	}
}

namespace IKVM.NativeCode.ikvm.runtime
{
	public class AssemblyClassLoader
	{
		public static object LoadClass(object classLoader, Assembly assembly, string name)
		{
			try
			{
				TypeWrapper tw = null;
				if(classLoader == null)
				{
					tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(name);
				}
				else if(assembly != null)
				{
					AssemblyClassLoader_ acl = ClassLoaderWrapper.GetAssemblyClassLoader(assembly);
					tw = acl.GetLoadedClass(name);
					if(tw == null)
					{
						tw = acl.LoadGenericClass(name);
					}
					if(tw == null)
					{
						tw = acl.LoadReferenced(name);
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
				Tracer.Info(Tracer.ClassLoading, "Loaded class \"{0}\" from {1}", name, classLoader == null ? "boot class loader" : classLoader);
				return tw.ClassObject;
			}
			catch(RetargetableJavaException x)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, classLoader == null ? "boot class loader" : classLoader);
				throw x.ToJava();
			}
		}

		public static Assembly[] FindResourceAssemblies(Assembly assembly, string name, bool firstOnly)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = ClassLoaderWrapper.GetAssemblyClassLoader(assembly);
			Assembly[] assemblies = wrapper.FindResourceAssemblies(name, firstOnly);
			if(assemblies == null || assemblies.Length == 0)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to find resource \"{0}\" in {1}", name, wrapper.Assembly.FullName);
				return null;
			}
			foreach(Assembly asm in assemblies)
			{
				Tracer.Info(Tracer.ClassLoading, "Found resource \"{0}\" in {1}", name, asm.FullName);
			}
			return assemblies;
		}

		public static Assembly GetAssemblyFromClassLoader(object classLoader)
		{
			AssemblyClassLoader_ acl = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader) as AssemblyClassLoader_;
			return acl != null ? acl.Assembly : null;
		}

		// NOTE the array may contain duplicates!
		public static string[] GetPackages(Assembly assembly)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = ClassLoaderWrapper.GetAssemblyClassLoader(assembly);
			string[] packages = new string[0];
			foreach(Module m in wrapper.Assembly.GetModules(false))
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

		public static bool IsReflectionOnly(Assembly asm)
		{
#if WHIDBEY
			return asm.ReflectionOnly;
#else
			return false;
#endif
		}

		public static int GetGenericClassLoaderId(object classLoader)
		{
			return ClassLoaderWrapper.GetGenericClassLoaderId((ClassLoaderWrapper)JVM.Library.getWrapperFromClassLoader(classLoader));
		}

		public static Assembly GetBootClassLoaderAssembly()
		{
			return ClassLoaderWrapper.GetBootstrapClassLoader().Assembly;
		}

		public static string GetGenericClassLoaderName(object classLoader)
		{
			return ((GenericClassLoader)JVM.Library.getWrapperFromClassLoader(classLoader)).GetName();
		}
	}

	public class AppDomainAssemblyClassLoader
	{
		public static object loadClassFromAssembly(Assembly asm, string className)
		{
			if(asm is System.Reflection.Emit.AssemblyBuilder)
			{
				return null;
			}
			if(asm.Equals(DynamicClassLoader.Instance.ModuleBuilder.Assembly))
			{
				// this can happen on Orcas, where an AssemblyBuilder has a corresponding Assembly
				return null;
			}
			TypeWrapper tw = ClassLoaderWrapper.GetAssemblyClassLoader(asm).DoLoad(className);
			return tw != null ? tw.ClassObject : null;
		}

		public static bool findResourceInAssembly(Assembly asm, string resourceName)
		{
			if(asm is System.Reflection.Emit.AssemblyBuilder)
			{
				return false;
			}
			if(asm.Equals(DynamicClassLoader.Instance.ModuleBuilder.Assembly))
			{
				// this can happen on Orcas, where an AssemblyBuilder has a corresponding Assembly
				return false;
			}
			return asm.GetManifestResourceInfo(JVM.MangleResourceName(resourceName)) != null;
		}
	}

	public class Util
	{
		private static Type enumEnumType = JVM.CoreAssembly.GetType("ikvm.internal.EnumEnum");
		private static FieldInfo enumEnumTypeField = enumEnumType.GetField("typeWrapper", BindingFlags.Instance | BindingFlags.NonPublic);

		// we don't want "beforefieldinit"
		static Util() {}

		public static object getClassFromObject(object o)
		{
			return GetTypeWrapperFromObject(o).ClassObject;
		}

		internal static TypeWrapper GetTypeWrapperFromObject(object o)
		{
			Type t = o.GetType();
			if(t.IsPrimitive || (ClassLoaderWrapper.IsRemappedType(t) && !t.IsSealed))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t);
			}
			if(t == enumEnumType)
			{
				return (TypeWrapper)enumEnumTypeField.GetValue(o);
			}
			return ClassLoaderWrapper.GetWrapperFromType(t);
		}

		public static object getClassFromTypeHandle(RuntimeTypeHandle handle)
		{
			Type t = Type.GetTypeFromHandle(handle);
			if(t.IsPrimitive || ClassLoaderWrapper.IsRemappedType(t) || t == typeof(void))
			{
				return DotNetTypeWrapper.GetWrapperFromDotNetType(t).ClassObject;
			}
			if(Whidbey.ContainsGenericParameters(t))
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

		public static object getFriendlyClassFromType(Type type)
		{
			if(Whidbey.ContainsGenericParameters(type))
			{
				return null;
			}
			int rank = 0;
			while(type.IsArray)
			{
				type = type.GetElementType();
				rank++;
			}
			if(type.DeclaringType != null
				&& AttributeHelper.IsGhostInterface(type.DeclaringType))
			{
				type = type.DeclaringType;
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

		public static Type getInstanceTypeFromClass(object clazz)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
			if(wrapper.IsDynamicOnly)
			{
				return null;
			}
			if(wrapper.IsRemapped && wrapper.IsFinal)
			{
				return wrapper.TypeAsTBD;
			}
			return wrapper.TypeAsBaseType;
		}
	}
}
