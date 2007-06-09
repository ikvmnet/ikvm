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
#if !FIRST_PASS
using NegativeArraySizeException = java.lang.NegativeArraySizeException;
using IllegalArgumentException = java.lang.IllegalArgumentException;
using IllegalAccessException = java.lang.IllegalAccessException;
using NumberFormatException = java.lang.NumberFormatException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
using jlrConstructor = java.lang.reflect.Constructor;
using jlrField = java.lang.reflect.Field;
#endif

namespace IKVM.NativeCode.java
{
	namespace lang
	{
		namespace reflect
		{
			public class VMArray
			{
				public static object createObjectArray(object clazz, int dim)
				{
					if(dim >= 0)
					{
						try
						{
							TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
							wrapper.Finish();
							return SystemArray.CreateInstance(wrapper.TypeAsArrayType, dim);
						}
						catch(RetargetableJavaException x)
						{
							throw x.ToJava();
						}
					}
#if !FIRST_PASS
					throw new NegativeArraySizeException();
#else
					return null;
#endif
				}
			}
		}

		public class VMRuntime
		{
			public static int nativeLoad(string filename, object classLoader)
			{
#if !COMPACT_FRAMEWORK
				return IKVM.Runtime.JniHelper.LoadLibrary(filename, ClassLoaderWrapper.GetClassLoaderWrapper(classLoader));
#else
				return 0;
#endif
			}
		}

		public class VMSystem
		{
			public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
			{
				ByteCodeHelper.arraycopy(src, srcStart, dest, destStart, len);
			}
		}

		public class VMClassLoader
		{
			public static object getPrimitiveClass(char type)
			{
				switch(type)
				{
					case 'Z':
						return PrimitiveTypeWrapper.BOOLEAN.ClassObject;
					case 'B':
						return PrimitiveTypeWrapper.BYTE.ClassObject;
					case 'C':
						return PrimitiveTypeWrapper.CHAR.ClassObject;
					case 'D':
						return PrimitiveTypeWrapper.DOUBLE.ClassObject;
					case 'F':
						return PrimitiveTypeWrapper.FLOAT.ClassObject;
					case 'I':
						return PrimitiveTypeWrapper.INT.ClassObject;
					case 'J':
						return PrimitiveTypeWrapper.LONG.ClassObject;
					case 'S':
						return PrimitiveTypeWrapper.SHORT.ClassObject;
					case 'V':
						return PrimitiveTypeWrapper.VOID.ClassObject;
					default:
						throw new InvalidOperationException();
				}
			}
		}
	}

	namespace io
	{
		public class VMObjectStreamClass
		{
			public static bool hasClassInitializer(object clazz)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
				try
				{
					wrapper.Finish();
				}
				catch(RetargetableJavaException x)
				{
					x.ToJava();
				}
				Type type = wrapper.TypeAsTBD;
				if(!type.IsArray && type.TypeInitializer != null)
				{
					wrapper.RunClassInit();
					return !AttributeHelper.IsHideFromJava(type.TypeInitializer);
				}
				return false;
			}

			private static FieldWrapper GetFieldWrapperFromField(object field)
			{
#if FIRST_PASS
				return null;
#else
				return FieldWrapper.FromField(field);
#endif
			}

			public static void setDoubleNative(object field, object obj, double val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setFloatNative(object field, object obj, float val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setLongNative(object field, object obj, long val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setIntNative(object field, object obj, int val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setShortNative(object field, object obj, short val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setCharNative(object field, object obj, char val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setByteNative(object field, object obj, byte val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setBooleanNative(object field, object obj, bool val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}

			public static void setObjectNative(object field, object obj, object val)
			{
				GetFieldWrapperFromField(field).SetValue(obj, val);
			}
		}

		public class VMObjectInputStream
		{
			public static object allocateObject(object clazz, object constructor_clazz, object constructor)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("ObjectInputStream.allocateObject");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
					// if we're trying to deserialize a string as a TC_OBJECT, just return an emtpy string (Sun does the same)
					if(wrapper == CoreClasses.java.lang.String.Wrapper)
					{
						return "";
					}
					wrapper.Finish();
					// TODO do we need error handling? (e.g. when trying to instantiate an interface or abstract class)
					object obj = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
					MethodWrapper mw = MethodWrapper.FromMethodOrConstructor(constructor);
					// TODO do we need error handling?
					mw.Invoke(obj, null, false);
					return obj;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("ObjectInputStream.allocateObject");
				}
#endif
			}
		}
	}

#if !COMPACT_FRAMEWORK
	namespace security
	{
		public class VMAccessController
		{
			public static object getClassFromFrame(System.Diagnostics.StackFrame frame)
			{
				return gnu.classpath.VMStackWalker.getClassFromType(frame.GetMethod().DeclaringType);
			}
		}
	}
#endif
}

namespace IKVM.NativeCode.gnu.java.lang.management
{
	public class VMClassLoadingMXBeanImpl
	{
		public static int getLoadedClassCount()
		{
			// we don't really have a number of classes loaded, but we'll
			// return something anyway
			return ClassLoaderWrapper.GetLoadedClassCount();
		}
	}
}

namespace IKVM.NativeCode.gnu.java.net.protocol.ikvmres
{
#if !WHIDBEY
	class LZInputStream : Stream 
	{
		private Stream inp;
		private int[] ptr_tbl;
		private int[] char_tbl;
		private int[] stack;
		private int table_size;
		private int count;
		private int bitoff;
		private int bitbuf;
		private int prev = -1;
		private int bits;
		private int cc;
		private int fc;
		private int sp;

		public LZInputStream(Stream inp)
		{
			this.inp = inp;
			bitoff = 0;
			count = 0;
			table_size = 256;
			bits = 9;
			ptr_tbl = new int[table_size];
			char_tbl = new int[table_size];
			stack = new int[table_size];
			sp = 0;
			cc = prev = incode();
			stack[sp++] = cc;
		}

		private int read()
		{
			if (sp == 0) 
			{
				if (stack.Length != table_size) 
				{
					stack = new int[table_size];
				}
				int ic = cc = incode();
				if (cc == -1) 
				{
					return -1;
				}
				if (count >= 0 && cc >= count + 256) 
				{
					stack[sp++] = fc;
					cc = prev;
					ic = find(prev, fc);
				}
				while (cc >= 256) 
				{
					stack[sp++] = char_tbl[cc - 256];
					cc = ptr_tbl[cc - 256];
				}
				stack[sp++] = cc;
				fc = cc;
				if (count >= 0) 
				{
					ptr_tbl[count] = prev;
					char_tbl[count] = fc;
				}
				count++;
				if (count == table_size) 
				{
					count = -1;
					if (bits == 12)
					{
						table_size = 256;
						bits = 9;
					}
					else
					{
						bits++;
						table_size = (1 << bits) - 256;
					}
					ptr_tbl = null;
					char_tbl = null;
					ptr_tbl = new int[table_size];
					char_tbl= new int[table_size];
				}
				prev = ic;
			}
			return stack[--sp] & 0xFF;
		}

		private int find(int p, int c) 
		{
			int i;
			for (i = 0; i < count; i++) 
			{
				if (ptr_tbl[i] == p && char_tbl[i] == c) 
				{
					break;
				}
			}
			return i + 256;
		}

		private int incode()
		{
			while (bitoff < bits) 
			{
				int v = inp.ReadByte();
				if (v == -1) 
				{
					return -1;
				}
				bitbuf |= (v & 0xFF) << bitoff;
				bitoff += 8;
			}
			bitoff -= bits;
			int result = bitbuf;
			bitbuf >>= bits;
			result -= bitbuf << bits;
			return result;
		}

		public override int Read(byte[] b, int off, int len)
		{
			int i = 0;
			for (; i < len ; i++)
			{
				int r = read();
				if(r == -1)
				{
					break;
				}
				b[off + i] = (byte)r;
			}
			return i;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
#endif // !WHIDBEY

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
#if WHIDBEY
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
#else
			using(Stream s = asm.GetManifestResourceStream(mangledName))
			{
				if(s == null)
				{
					Tracer.Warning(Tracer.ClassLoading, "Resource \"{0}\" not found in {1}", resource, asm.FullName);
					throw new FileNotFoundException("resource " + resource + " not found in assembly " + asm.FullName);
				}
				using(System.Resources.ResourceReader r = new System.Resources.ResourceReader(s))
				{
					foreach(DictionaryEntry de in r)
					{
						if((string)de.Key == "lz")
						{
							Tracer.Info(Tracer.ClassLoading, "Reading compressed resource \"{0}\" from {1}", resource, asm.FullName);
							return new LZInputStream(new MemoryStream((byte[])de.Value));
						}
						else if((string)de.Key == "ikvm")
						{
							Tracer.Info(Tracer.ClassLoading, "Reading resource \"{0}\" from {1}", resource, asm.FullName);
							return new MemoryStream((byte[])de.Value);
						}
						else
						{
							Tracer.Error(Tracer.ClassLoading, "Resource \"{0}\" in {1} has an unsupported encoding", resource, asm.FullName);
							throw new IOException("Unsupported resource encoding " + de.Key + " for resource " + resource + " found in assembly " + asm.FullName);
						}
					}
					Tracer.Error(Tracer.ClassLoading, "Resource \"{0}\" in {1} is invalid", resource, asm.FullName);
					throw new IOException("Invalid resource " + resource + " found in assembly " + asm.FullName);
				}
			}
#endif
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
#if WHIDBEY
			if(name.EndsWith("[ReflectionOnly]"))
			{
				return Assembly.ReflectionOnlyLoad(name.Substring(0, name.Length - 16));
			}
#endif
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

	public class VMStackWalker
	{
		private static readonly Hashtable isHideFromJavaCache = Hashtable.Synchronized(new Hashtable());

		public static object getClassFromType(Type type)
		{
			TypeWrapper.AssertFinished(type);
			if(type == null)
			{
				return null;
			}
			TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
			if(tw == null)
			{
				return null;
			}
			return tw.ClassObject;
		}

		public static object getClassLoaderFromType(Type type)
		{
			// global methods have no type
			if(type == null)
			{
				return null;
			}
			else if(type.Module is System.Reflection.Emit.ModuleBuilder)
			{
				return ClassLoaderWrapper.GetWrapperFromType(type).GetClassLoader().GetJavaClassLoader();
			}
			else
			{
				return ClassLoaderWrapper.GetAssemblyClassLoader(type.Assembly).GetJavaClassLoader();
			}
		}

		public static Type getJNIEnvType()
		{
#if COMPACT_FRAMEWORK
			return null;
#else
			return typeof(IKVM.Runtime.JNIEnv);
#endif
		}

		public static bool isHideFromJava(MethodBase mb)
		{
			// TODO on .NET 2.0 isHideFromJavaCache should be a Dictionary<RuntimeMethodHandle, bool>
			object cached = isHideFromJavaCache[mb];
			if(cached == null)
			{
				cached = mb.IsDefined(typeof(HideFromJavaAttribute), false)
					|| mb.IsDefined(typeof(HideFromReflectionAttribute), false);
				isHideFromJavaCache[mb] = cached;
			}
			return (bool)cached;
		}
	}
}

namespace IKVM.NativeCode.ikvm.@internal
{
	public class AssemblyClassLoader
	{
		public static object LoadClass(object classLoader, string name)
		{
			try
			{
				ClassLoaderWrapper wrapper = classLoader == null ? ClassLoaderWrapper.GetBootstrapClassLoader() : (ClassLoaderWrapper)JVM.Library.getWrapperFromClassLoader(classLoader);
				TypeWrapper tw = wrapper.LoadClassByDottedName(name);
				Tracer.Info(Tracer.ClassLoading, "Loaded class \"{0}\" from {1}", name, classLoader == null ? "boot class loader" : classLoader);
				return tw.ClassObject;
			}
			catch(RetargetableJavaException x)
			{
				Tracer.Info(Tracer.ClassLoading, "Failed to load class \"{0}\" from {1}", name, classLoader == null ? "boot class loader" : classLoader);
				throw x.ToJava();
			}
		}

		public static Assembly[] FindResourceAssemblies(object classLoader, string name, bool firstOnly)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = classLoader == null ? ClassLoaderWrapper.GetBootstrapClassLoader() : (JVM.Library.getWrapperFromClassLoader(classLoader) as IKVM.Internal.AssemblyClassLoader);
			if(wrapper == null)
			{
				// must be a GenericClassLoader
				Tracer.Info(Tracer.ClassLoading, "Failed to find resource \"{0}\" in generic class loader", name);
				return null;
			}
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

		// NOTE the array may contain duplicates!
		public static string[] GetPackages(object classLoader)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = classLoader == null ? ClassLoaderWrapper.GetBootstrapClassLoader() : (JVM.Library.getWrapperFromClassLoader(classLoader) as IKVM.Internal.AssemblyClassLoader);
			if(wrapper == null)
			{
				// must be a GenericClassLoader
				return null;
			}
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
			return ((IKVM.Internal.AssemblyClassLoader)ClassLoaderWrapper.GetBootstrapClassLoader()).Assembly;
		}

		public static string GetGenericClassLoaderName(object classLoader)
		{
			return ((GenericClassLoader)JVM.Library.getWrapperFromClassLoader(classLoader)).GetName();
		}
	}

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

namespace IKVM.NativeCode.sun.misc
{
	public sealed class Unsafe
	{
		private Unsafe() { }

		public static void throwException(object thisUnsafe, Exception x)
		{
			throw x;
		}

		public static void ensureClassInitialized(object thisUnsafe, object clazz)
		{
			TypeWrapper tw = TypeWrapper.FromClass(clazz);
			if (!tw.IsArray)
			{
				tw.Finish();
				tw.RunClassInit();
			}
		}

		public static object allocateInstance(object thisUnsafe, object clazz)
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
			try
			{
				wrapper.Finish();
			}
			catch (RetargetableJavaException x)
			{
				throw x.ToJava();
			}
			return FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
		}
	}
}

#if FIRST_PASS
namespace ikvm.@internal
{
	public interface LibraryVMInterface
	{
		object newClass(object wrapper, object protectionDomain, object classLoader);
		object getWrapperFromClass(object clazz);

		object getWrapperFromClassLoader(object classLoader);
		void setWrapperForClassLoader(object classLoader, object wrapper);

		object box(object val);
		object unbox(object val);

		Exception mapException(Exception t);

		object newDirectByteBuffer(IntPtr address, int capacity);
		IntPtr getDirectBufferAddress(object buffer);
		int getDirectBufferCapacity(object buffer);

		void setProperties(System.Collections.Hashtable props);

		bool runFinalizersOnExit();

		object newAnnotation(object classLoader, object definition);
		object newAnnotationElementValue(object classLoader, object expectedClass, object definition);

		object newAssemblyClassLoader(Assembly asm);
	}
}
#endif // !FIRST_PASS
