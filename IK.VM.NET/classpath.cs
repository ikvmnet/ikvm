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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using NetSystem = System;

namespace java.lang
{
	public interface Cloneable
	{
	}

	// TODO should be serializable
	public sealed class StackTraceElement
	{
		private static readonly long serialVersionUID = 6992337162326171013L;
		private string fileName;
		private int lineNumber;
		private string className;
		private string methodName;
		[NonSerialized]
		private bool isNative;

		internal long bogus_method_to_prevent_warning()
		{
			return serialVersionUID;
		}

		internal StackTraceElement(string fileName, int lineNumber, string className, string methodName, bool isNative)
		{
			this.fileName = fileName;
			this.lineNumber = lineNumber;
			this.className = className;
			this.methodName = methodName;
			this.isNative = isNative;
		}

		public string getFileName()
		{
			return fileName;
		}

		public int getLineNumber()
		{
			return lineNumber;
		}

		public string getClassName()
		{
			return className;
		}

		public string getMethodName()
		{
			return methodName;
		}

		public bool isNativeMethod()
		{
			return isNative;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if(className != null)
			{
				sb.Append(className);
				if(methodName != null)
				{
					sb.Append('.');
				}
			}
			if(methodName != null)
			{
				sb.Append(methodName);
			}
			sb.Append('(');
			if(fileName != null)
			{
				sb.Append(fileName);
			}
			else
			{
				sb.Append(isNative ? "Native Method" : "Unknown Source");
			}
			if(lineNumber >= 0)
			{
				sb.Append(':').Append(lineNumber);
			}
			sb.Append(')');
			return sb.ToString();
		}

		public override bool Equals(object o)
		{
			if(o != null && o.GetType() == GetType())
			{
				StackTraceElement ste = (StackTraceElement)o;
				return ste.className == className &&
					ste.fileName == fileName &&
					ste.lineNumber == lineNumber &&
					ste.methodName == methodName;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return GetHashCode(className) ^ GetHashCode(fileName) ^ GetHashCode(methodName) ^ lineNumber;
		}

		private static int GetHashCode(string o)
		{
			if(o == null)
			{
				return 0;
			}
			return o.GetHashCode();
		}
	}
}

namespace java.io
{
	public interface Serializable
	{
	}
}

namespace NativeCode.java
{
	namespace lang
	{
		namespace reflect
		{
			public class Array
			{
				public static object createObjectArray(object clazz, int dim)
				{
					if(dim >= 0)
					{
						return NetSystem.Array.CreateInstance(Class.getType(clazz), dim);
					}
					throw JavaException.NegativeArraySizeException();
				}
			}

			internal class JavaWrapper
			{
				private static Type java_lang_Byte = ClassLoaderWrapper.GetType("java.lang.Byte");
				private static Type java_lang_Boolean = ClassLoaderWrapper.GetType("java.lang.Boolean");
				private static Type java_lang_Short = ClassLoaderWrapper.GetType("java.lang.Short");
				private static Type java_lang_Character = ClassLoaderWrapper.GetType("java.lang.Character");
				private static Type java_lang_Integer = ClassLoaderWrapper.GetType("java.lang.Integer");
				private static Type java_lang_Long = ClassLoaderWrapper.GetType("java.lang.Long");
				private static Type java_lang_Float = ClassLoaderWrapper.GetType("java.lang.Float");
				private static Type java_lang_Double = ClassLoaderWrapper.GetType("java.lang.Double");

				internal static object Box(object o)
				{
					if(o is sbyte)
					{
						return Activator.CreateInstance(java_lang_Byte, new object[] { o });
					}
					else if(o is bool)
					{
						return Activator.CreateInstance(java_lang_Boolean, new object[] { o });
					}
					else if(o is short)
					{
						return Activator.CreateInstance(java_lang_Short, new object[] { o });
					}
					else if(o is char)
					{
						return Activator.CreateInstance(java_lang_Character, new object[] { o });
					}
					else if(o is int)
					{
						return Activator.CreateInstance(java_lang_Integer, new object[] { o });
					}
					else if(o is long)
					{
						return Activator.CreateInstance(java_lang_Long, new object[] { o });
					}
					else if(o is float)
					{
						return Activator.CreateInstance(java_lang_Float, new object[] { o });
					}
					else if(o is double)
					{
						return Activator.CreateInstance(java_lang_Double, new object[] { o });
					}
					else
					{
						throw new NotImplementedException(o.GetType().FullName);
					}
				}

				internal static object Unbox(object o)
				{
					Type type = o.GetType();
					if(type == java_lang_Byte)
					{
						return java_lang_Byte.GetMethod("byteValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Boolean)
					{
						return java_lang_Boolean.GetMethod("booleanValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Short)
					{
						return java_lang_Short.GetMethod("shortValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Character)
					{
						return java_lang_Character.GetMethod("charValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Integer)
					{
						return java_lang_Integer.GetMethod("intValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Long)
					{
						return java_lang_Long.GetMethod("longValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Float)
					{
						return java_lang_Float.GetMethod("floatValue").Invoke(o, new object[0]);
					}
					else if(type == java_lang_Double)
					{
						return java_lang_Double.GetMethod("doubleValue").Invoke(o, new object[0]);
					}
					else
					{
						throw new NotImplementedException(o.GetType().FullName);
					}
				}
			}

			public class Method
			{
				public static String GetName(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					return wrapper.Name;
				}
				
				public static int GetModifiers(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					return (int)wrapper.Modifiers;
				}

				public static object GetReturnType(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					return Class.getClassFromWrapper(wrapper.ReturnType);
				}

				public static object[] GetParameterTypes(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					TypeWrapper[] parameters = wrapper.GetParameters();
					object[] parameterClasses = new object[parameters.Length];
					for(int i = 0; i < parameters.Length; i++)
					{
						parameterClasses[i] = Class.getClassFromWrapper(parameters[i]);
					}
					return parameterClasses;
				}

				public static object[] GetExceptionTypes(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					// TODO
					return new object[0];
				}

				[StackTraceInfo(Hidden = true)]
				public static object Invoke(object methodCookie, object o, object[] args)
				{
					MethodWrapper mw = (MethodWrapper)methodCookie;
					mw.DeclaringType.Finish();
					TypeWrapper[] argWrappers = mw.GetParameters();
					for(int i = 0; i < argWrappers.Length; i++)
					{
						if(argWrappers[i].IsPrimitive)
						{
							args[i] = JavaWrapper.Unbox(args[i]);
						}
					}
					object retval = mw.Invoke(o, args, false);
					if(mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
					{
						retval = JavaWrapper.Box(retval);
					}
					return retval;
				}
			}

			public class Field
			{
				public static string GetName(object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					return wrapper.Name;
				}

				public static int GetModifiers(object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					return (int)wrapper.Modifiers;
				}

				public static object GetFieldType(object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					return Class.getClassFromType(wrapper.FieldType);
				}

				public static object GetValue(object fieldCookie, object o)
				{
					// TODO this is a very lame implementation, no where near correct
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					wrapper.DeclaringType.Finish();
					FieldInfo fi = wrapper.DeclaringType.Type.GetField(wrapper.Name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if(fi.FieldType.IsValueType)
					{
						return JavaWrapper.Box(fi.GetValue(o));
					}
					return fi.GetValue(o);
				}

				public static void SetValue(object fieldCookie, object o, object v)
				{
					// TODO this is a very lame implementation, no where near correct
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					wrapper.DeclaringType.Finish();
					FieldInfo fi = wrapper.DeclaringType.Type.GetField(wrapper.Name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if(fi.FieldType.IsValueType)
					{
						v = JavaWrapper.Unbox(v);
					}
					fi.SetValue(o, v);
				}
			}
		}

		public class Runtime
		{
			public static void insertSystemProperties(object properties)
			{
				MethodInfo m = properties.GetType().GetMethod("setProperty");
				// TODO set all these properties to something useful
				m.Invoke(properties, new string[] { "java.version", "1.3" });
				m.Invoke(properties, new string[] { "java.vendor", "Jeroen Frijters" });
				m.Invoke(properties, new string[] { "java.vendor.url", "http://ikvm.net/" });
				// HACK using the Assembly.Location property isn't correct
				m.Invoke(properties, new string[] { "java.home", new FileInfo(typeof(Runtime).Assembly.Location).DirectoryName });
				m.Invoke(properties, new string[] { "java.vm.specification.version", "1.0" });
				m.Invoke(properties, new string[] { "java.vm.specification.vendor", "Sun Microsystems Inc." });
				m.Invoke(properties, new string[] { "java.vm.specification.name", "Java Virtual Machine Specification" });
				m.Invoke(properties, new string[] { "java.vm.version", typeof(Runtime).Assembly.GetName().Version.ToString() });
				m.Invoke(properties, new string[] { "java.vm.vendor", "Jeroen Frijters" });
				m.Invoke(properties, new string[] { "java.vm.name", "IKVM.NET" });
				m.Invoke(properties, new string[] { "java.specification.version", "1.3" });
				m.Invoke(properties, new string[] { "java.specification.vendor", "Sun Microsystems Inc." });
				m.Invoke(properties, new string[] { "java.specification.name", "Java Platform API Specification" });
				m.Invoke(properties, new string[] { "java.class.version", "48.0" });
				string classpath = Environment.GetEnvironmentVariable("CLASSPATH");
				if(classpath == null)
				{
					classpath = ".";
				}
				m.Invoke(properties, new string[] { "java.class.path", classpath });
				m.Invoke(properties, new string[] { "java.library.path", "." });
				m.Invoke(properties, new string[] { "java.io.tmpdir", Path.GetTempPath() });
				m.Invoke(properties, new string[] { "java.compiler", "" });
				m.Invoke(properties, new string[] { "java.ext.dirs", "" });
				// NOTE os.name *must* contain "Windows" when running on Windows, because Classpath tests on that
				string osname = Environment.OSVersion.ToString();
				string osver = Environment.OSVersion.Version.ToString();
				// HACK if the osname contains the version, we remove it
				osname = osname.Replace(osver, "").Trim();
				m.Invoke(properties, new string[] { "os.name", osname });
				string arch = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
				if(arch == null)
				{
					// TODO get this info from somewhere else
					arch = "x86";
				}
				m.Invoke(properties, new string[] { "os.arch", arch });
				m.Invoke(properties, new string[] { "os.version", osver });
				m.Invoke(properties, new string[] { "file.separator", Path.DirectorySeparatorChar.ToString() });
				m.Invoke(properties, new string[] { "file.encoding", "8859_1" });
				m.Invoke(properties, new string[] { "path.separator", Path.PathSeparator.ToString() });
				m.Invoke(properties, new string[] { "line.separator", Environment.NewLine });
				m.Invoke(properties, new string[] { "user.name", Environment.UserName });
				string home = Environment.GetEnvironmentVariable("USERPROFILE");
				if(home == null)
				{
					// maybe we're on *nix
					home = Environment.GetEnvironmentVariable("HOME");
					if(home == null)
					{
						// TODO may be there is a better way
						// NOTE on MS .NET this doesn't return the correct path
						// (it returns "C:\Documents and Settings\username\My Documents", but we really need
						// "C:\Documents and Settings\username" to be compatible with Sun)
						home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					}
				}
				m.Invoke(properties, new string[] { "user.home", home });
				m.Invoke(properties, new string[] { "user.dir", Environment.CurrentDirectory });
				m.Invoke(properties, new string[] { "awt.toolkit", "ikvm.awt.NetToolkit, awt, Version=1.0, Culture=neutral, PublicKeyToken=null" });
				// HACK we assume that the type of the properties object is the classpath assembly
				m.Invoke(properties, new string[] { "gnu.classpath.home.url", "ikvmres:" + properties.GetType().Assembly.FullName + ":lib" });
			}

			public static string nativeGetLibname(string pathname, string libname)
			{
				// HACK this seems like a lame way of doing things, but in order to get Eclipse to work,
				// we have append .dll to the libname here
				if(!libname.ToUpper().EndsWith(".DLL"))
				{
					libname += ".dll";
				}
				return libname;
			}

			public static int nativeLoad(object obj, string filename)
			{
				// TODO native libraries somehow need to be scoped by class loader
				return JVM.JniProvider.LoadNativeLibrary(filename);
			}

			public static void gc(object obj)
			{
				NetSystem.GC.Collect();
			}

			public static void runFinalization(object obj)
			{
				NetSystem.GC.WaitForPendingFinalizers();
			}

			public static void runFinalizersOnExitInternal(bool b)
			{
				// the CLR always runs the finalizers, so we can ignore this
			}

			public static void exitInternal(object obj, int rc)
			{
				NetSystem.Environment.Exit(rc);
			}

			public static int availableProcessors(object obj)
			{
				string s = NetSystem.Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
				if(s != null)
				{
					return int.Parse(s);
				}
				return 1;
			}

			public static object execInternal(object obj, string[] cmd, string[] env, object dir)
			{
				// TODO this was moved to the Java class ikvm.lang.DotNetProcess
				throw new NotImplementedException();
			}

			public static long freeMemory(object obj)
			{
				// TODO figure out if there is anything meaningful we can return here
				return 10 * 1024 * 1024;
			}

			public static long maxMemory(object obj)
			{
				return long.MaxValue;
			}

			public static long totalMemory(object obj)
			{
				// NOTE this really is a bogus number, but we have to return something
				return NetSystem.GC.GetTotalMemory(false);
			}

			public static void traceInstructions(object obj, bool b)
			{
				// not supported
			}

			public static void traceMethodCalls(object obj, bool b)
			{
				// not supported
			}
		}

		public class Math
		{
			public static double pow(double x, double y)
			{
				return NetSystem.Math.Pow(x, y);
			}

			public static double exp(double d)
			{
				return NetSystem.Math.Exp(d);
			}

			public static double rint(double d)
			{
				return NetSystem.Math.Round(d);
			}

			public static double IEEEremainder(double f1, double f2)
			{
				return NetSystem.Math.IEEERemainder(f1, f2);
			}

			public static double sqrt(double d)
			{
				return NetSystem.Math.Sqrt(d);
			}

			public static double floor(double d)
			{
				return NetSystem.Math.Floor(d);
			}

			public static double ceil(double d)
			{
				return NetSystem.Math.Ceiling(d);
			}

			public static double log(double d)
			{
				return NetSystem.Math.Log(d);
			}

			public static double sin(double d)
			{
				return NetSystem.Math.Sin(d);
			}

			public static double asin(double d)
			{
				return NetSystem.Math.Asin(d);
			}

			public static double cos(double d)
			{
				return NetSystem.Math.Cos(d);
			}

			public static double acos(double d)
			{
				return NetSystem.Math.Acos(d);
			}

			public static double tan(double d)
			{
				return NetSystem.Math.Tan(d);
			}

			public static double atan(double d)
			{
				return NetSystem.Math.Atan(d);
			}

			public static double atan2(double y, double x)
			{
				return NetSystem.Math.Atan2(y, x);
			}
		}

		public class Double
		{
			public static void initIDs()
			{
			}

			public static double parseDouble(string s)
			{
				try
				{
					// TODO I doubt that this is correct
					return double.Parse(s);
				}
				catch(FormatException x)
				{
					throw JavaException.NumberFormatException(x.Message);
				}
			}

			public static long doubleToLongBits(double v)
			{
				if(double.IsNaN(v))
				{
					return 0x7ff8000000000000L;
				}
				return BitConverter.DoubleToInt64Bits(v);
			}

			public static long doubleToRawLongBits(double v)
			{
				return BitConverter.DoubleToInt64Bits(v);
			}

			public static double longBitsToDouble(long bits)
			{
				return BitConverter.Int64BitsToDouble(bits);
			}

			public static string toString(double d, bool isFloat)
			{
				return isFloat ? StringHelper.valueOf((float)d) : StringHelper.valueOf(d);
			}
		}

		public class Float
		{
			public static float intBitsToFloat(int v)
			{
				return BitConverter.ToSingle(BitConverter.GetBytes(v), 0);
			}

			public static int floatToIntBits(float v)
			{
				if(float.IsNaN(v))
				{
					return 0x7fc00000;
				}
				return BitConverter.ToInt32(BitConverter.GetBytes(v), 0);
			}

			public static int floatToRawIntBits(float v)
			{
				return BitConverter.ToInt32(BitConverter.GetBytes(v), 0);
			}
		}

		public class VMSecurityManager
		{
			public static object getClassContext()
			{
				ArrayList ar = new ArrayList();
				NetSystem.Diagnostics.StackTrace st = new NetSystem.Diagnostics.StackTrace();
				for(int i = 0; i < st.FrameCount; i++)
				{
					NetSystem.Diagnostics.StackFrame frame = st.GetFrame(i);
					// HACK very insecure
					// TODO handle reflection scenario
					if(frame.GetMethod().Name != "getClassContext")
					{
						ar.Add(Class.getClassFromType(frame.GetMethod().DeclaringType));
					}
				}
				return ar.ToArray(ClassLoaderWrapper.GetType("java.lang.Class"));
			}

			public static object currentClassLoader()
			{
				// TODO handle PrivilegedAction
				NetSystem.Diagnostics.StackTrace st = new NetSystem.Diagnostics.StackTrace();
				for(int i = 0; i < st.FrameCount; i++)
				{
					NetSystem.Diagnostics.StackFrame frame = st.GetFrame(i);
					TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(frame.GetMethod().DeclaringType);
					if(wrapper != null && wrapper.GetClassLoader().GetJavaClassLoader() != null)
					{
						return wrapper.GetClassLoader().GetJavaClassLoader();
					}
				}
				return null;
			}
		}

		public class VMSystem
		{
			public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
			{
				if ((src == null) || (dest == null))
					throw new NullReferenceException ();

				if (!(src is Array) || !(dest is Array))
					throw JavaException.ArrayStoreException ("source and destination must be an array");

				Type eltype_src = src.GetType().GetElementType();
				Type eltype_dst = dest.GetType().GetElementType();
				bool prim_src = eltype_src.IsPrimitive;
				bool prim_dst = eltype_dst.IsPrimitive;

				if (prim_src && !prim_dst)
					throw JavaException.ArrayStoreException ("source is an array of primitive type while destination is not");

				if (!prim_src && prim_dst)
					throw JavaException.ArrayStoreException ("destination is an array of primitive type while source is not");

				if (prim_src && prim_dst && (eltype_src != eltype_dst))
					throw JavaException.ArrayStoreException ("source and destination must be of the same primitive type");

				try {
					Array.Copy((Array)src, srcStart, (Array)dest, destStart, len);
				}
				catch (ArgumentOutOfRangeException) {
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch (ArgumentException) {
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch (InvalidCastException) {
					throw JavaException.ArrayStoreException ("cast failed");
				}
			}

			public static bool isWordsBigEndian()
			{
				return !BitConverter.IsLittleEndian;
			}

			private static long timebase = ((TimeZone.CurrentTimeZone.ToUniversalTime(DateTime.Now) - new DateTime(1970, 1, 1)).Ticks / 10000L) - Environment.TickCount;

			public static long currentTimeMillis()
			{
				// NOTE this wraps after 24.9 days, but it is much faster than calling DateTime.Now every time
				return timebase + Environment.TickCount;
			}

			public static void setErr(object printStream)
			{
				ClassLoaderWrapper.GetType("java.lang.System").GetField("err", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, printStream);
			}

			public static void setIn(object inputStream)
			{
				ClassLoaderWrapper.GetType("java.lang.System").GetField("in", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, inputStream);
			}

			public static void setOut(object printStream)
			{
				ClassLoaderWrapper.GetType("java.lang.System").GetField("out", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, printStream);
			}
		}

		public class VMClassLoader
		{
			public static Assembly findResourceAssembly(string name)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for(int i = 0; i < assemblies.Length; i++)
				{
					if(!(assemblies[i] is NetSystem.Reflection.Emit.AssemblyBuilder))
					{
						if(assemblies[i].GetLoadedModules()[0].GetField(name) != null)
						{
							return assemblies[i];
						}
					}
				}
				return null;
			}

			public static Type getPrimitiveType(char type)
			{
				switch(type)
				{
					case 'Z':
						return typeof(bool);
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
					case 'S':
						return typeof(short);
					case 'V':
						return typeof(void);
					default:
						throw new InvalidOperationException();
				}
			}

			public static object defineClass(object classLoader, string name, byte[] data, int offset, int length, object protectionDomain)
			{
				Profiler.Enter("ClassLoader.defineClass");
				try
				{
					// TODO handle errors
					ClassFile classFile = new ClassFile(data, offset, length, name);
					if(name != null && classFile.Name.Replace('/', '.') != name)
					{
						throw JavaException.NoClassDefFoundError("{0} (wrong name: {1})", name, classFile.Name);
					}
					TypeWrapper type = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader).DefineClass(classFile);
					object clazz = Class.CreateInstance(null, type);
					if(protectionDomain != null)
					{
						// TODO cache the FieldInfo
						clazz.GetType().GetField("pd", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(clazz, protectionDomain);
					}
					return clazz;
				}
				finally
				{
					Profiler.Leave("ClassLoader.defineClass");
				}
			}
		}

		public class Thread
		{
			public static void sleep(long millis, int nanos)
			{
				NetSystem.Threading.Thread.Sleep(new TimeSpan(millis * 10000 + (nanos + 99) / 100));
			}

			public static void joinInternal(NetSystem.Threading.Thread nativeThread, long millis, int nanos)
			{
				nativeThread.Join(new TimeSpan(millis * 10000 + (nanos + 99) / 100));
			}
		}

		public class Class
		{
			private static Hashtable map = new Hashtable();
			private static ConstructorInfo classConstructor;
			private static MethodInfo getTypeMethod;

			public static object loadArrayClass(string name, object classLoader)
			{
				ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader);
				TypeWrapper type = classLoaderWrapper.LoadClassByDottedName(name);
				return getClassFromWrapper(type);
			}

			public static object loadBootstrapClass(string name, bool initialize)
			{
				TypeWrapper type = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(name);
				type.Finish();
				if(initialize)
				{
					RuntimeHelpers.RunClassConstructor(type.Type.TypeHandle);
				}
				return getClassFromType(type.Type);
			}

			internal static object CreateInstance(Type type, TypeWrapper wrapper)
			{
				TypeWrapper.AssertFinished(type);
				if(classConstructor == null)
				{
					classConstructor = ClassLoaderWrapper.GetType("java.lang.Class").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, new Type[] { typeof(Type), typeof(object) }, null);
				}
				object clazz = classConstructor.Invoke(new object[] { type, wrapper });
				lock(map.SyncRoot)
				{
					if(type != null)
					{
						map.Add(type, clazz);
					}
					if(wrapper != null)
					{
						map.Add(wrapper, clazz);
					}
				}
				return clazz;
			}

			public static bool IsAssignableFrom(Object w1, Object w2)
			{
				return ((TypeWrapper)w2).IsAssignableTo((TypeWrapper)w1);
			}

			public static object GetSuperClassFromWrapper(object wrapper)
			{
				TypeWrapper baseWrapper = ((TypeWrapper)wrapper).BaseTypeWrapper;
				if(baseWrapper != null)
				{
					return getClassFromWrapper(baseWrapper);
				}
				return null;
			}

			public static object getComponentClassFromWrapper(object wrapper)
			{
				TypeWrapper typeWrapper = (TypeWrapper)wrapper;
				if(typeWrapper.ArrayRank > 0)
				{
					TypeWrapper elementWrapper = typeWrapper.ElementTypeWrapper;
					// TODO why are we finishing here? This shouldn't be necessary
					elementWrapper.Finish();
					return getClassFromWrapper(elementWrapper);
				}
				return null;
			}

			public static Type getTypeFromWrapper(object clazz, object wrapper)
			{
				((TypeWrapper)wrapper).Finish();
				Type type = ((TypeWrapper)wrapper).Type;
				TypeWrapper.AssertFinished(type);
				lock(map.SyncRoot)
				{
					// NOTE since this method can be called multiple times (or after getClassFromType has added
					// the Class to the map), we don't use Add() here, but the indexer because that can handle
					// "overwriting" the existing association (which should always be the same as the new one)
					map[type] = clazz;
				}
				return type;
			}

			public static object getWrapperFromType(Type t)
			{
				return ClassLoaderWrapper.GetWrapperFromType(t);
			}

			public static Type getType(object clazz)
			{
				if(getTypeMethod == null)
				{
					getTypeMethod = ClassLoaderWrapper.GetType("java.lang.Class").GetMethod("getType", BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
				}
				return (Type)getTypeMethod.Invoke(clazz, new object[0]);
			}

			internal static object getClassFromWrapper(TypeWrapper wrapper)
			{
				lock(map.SyncRoot)
				{
					object clazz = map[wrapper];
					if(clazz == null)
					{
						// Maybe the Class object was already constructed from the type
						clazz = map[wrapper.Type];
						if(clazz == null)
						{
							clazz = CreateInstance(null, wrapper);
						}
					}
					return clazz;
				}
			}

			public static object getClassFromType(Type type)
			{
				TypeWrapper.AssertFinished(type);
				if(type == null)
				{
					return null;
				}
				lock(map.SyncRoot)
				{
					object clazz = map[type];
					if(clazz == null)
					{
						// maybe the Class object was constructed from the wrapper
						TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
						if(wrapper != null)
						{
							clazz = map[wrapper];
							if(clazz != null)
							{
								map.Add(type, clazz);
								return clazz;
							}
						}
						// NOTE we need to get the bootstrap classloader to trigger its construction (if it
						// hasn't been created yet), because otherwise CreateInstance will do that and this
						// causes the same class object to be created multiple times)
						ClassLoaderWrapper.GetBootstrapClassLoader();
						clazz = map[type];
						if(clazz == null)
						{
							// if this type is an override stub (e.g. java.lang.Object), we need to return the
							// class object for the parent type
							// NOTE we first check if type isn't an array, because Type.IsDefined throws an exception
							// when called on an array type (?)
							if(!type.IsArray && type.IsDefined(typeof(OverrideStubTypeAttribute), false))
							{
								clazz = getClassFromType(type.BaseType);
								map.Add(type, clazz);
							}
							else
							{
								// TODO should we specify the wrapper?
								// NOTE CreateInstance adds the Class to the "map"
								clazz = CreateInstance(type, null);
							}
						}
					}
					return clazz;
				}
			}

			public static string getName(Type type)
			{
				return GetName(type, null);
			}
	
			public static string GetName(Type type, object wrapperType)
			{
				if(type == null)
				{
					string name = ((TypeWrapper)wrapperType).Name;
					// HACK name is null for primitives
					if(name != null)
					{
						return name.Replace('/', '.');
					}
					type = ((TypeWrapper)wrapperType).Type;
				}
				if(wrapperType == null)
				{
					wrapperType = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
				}
				if(wrapperType != null)
				{
					string name = ((TypeWrapper)wrapperType).Name;
					// HACK name is null for primitives
					if(name != null)
					{
						return name.Replace('/', '.');
					}
				}
				if(type.IsValueType)
				{
					if(type == typeof(void))
					{
						return "void";
					}
					else if(type == typeof(bool))
					{
						return "boolean";
					}
					else if(type == typeof(sbyte))
					{
						return "byte";
					}
					else if(type == typeof(char))
					{
						return "char";
					}
					else if(type == typeof(short))
					{
						return "short";
					}
					else if(type == typeof(int))
					{
						return "int";
					}
					else if(type == typeof(long))
					{
						return "long";
					}
					else if(type == typeof(float))
					{
						return "float";
					}
					else if(type == typeof(double))
					{
						return "double";
					}
					else
					{
						return type.FullName;
					}
				}
				else if(type.IsArray)
				{
					StringBuilder sb = new StringBuilder();
					while(type.IsArray)
					{
						sb.Append('[');
						type = type.GetElementType();
					}
					if(type.IsValueType)
					{
						if(type == typeof(void))
						{
							sb.Append('V');
						}
						else if(type == typeof(bool))
						{
							sb.Append('Z');
						}
						else if(type == typeof(sbyte))
						{
							sb.Append('B');
						}
						else if(type == typeof(char))
						{
							sb.Append('C');
						}
						else if(type == typeof(short))
						{
							sb.Append('S');
						}
						else if(type == typeof(int))
						{
							sb.Append('I');
						}
						else if(type == typeof(long))
						{
							sb.Append('J');
						}
						else if(type == typeof(float))
						{
							sb.Append('F');
						}
						else if(type == typeof(double))
						{
							sb.Append('D');
						}
						else
						{
							sb.Append(type.FullName);
						}
					}
					else
					{
						sb.Append('L').Append(GetName(type, null)).Append(';');
					}
					return sb.ToString();
				}
				else
				{
					while(type.IsDefined(typeof(OverrideStubTypeAttribute), false))
					{
						type = type.BaseType;
					}
					TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
					if(wrapper != null)
					{
						return wrapper.Name.Replace('/', '.');
					}
					// look for our custom attribute, that contains the real name of the type (for inner classes)
					Object[] attribs = type.GetCustomAttributes(typeof(ClassNameAttribute), false);
					if(attribs.Length == 1)
					{
						return ((ClassNameAttribute)attribs[0]).Name.Replace('/', '.');
					}
					return type.FullName;
				}
			}

			[StackTraceInfo(Hidden = true)]
			public static void initializeType(Type type)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}

			public static object getClassLoader0(Type type)
			{
				return ClassLoaderWrapper.GetClassLoader(type).GetJavaClassLoader();
			}

			public static object[] GetDeclaredMethods(Type type, object cwrapper, bool getMethods)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// we need to finish the type otherwise all methods will not be in the method map yet
				wrapper.Finish();
				// we need to look through the array for unloadable types, because we may not let them
				// escape into the 'wild'
				MethodWrapper[] methods = wrapper.GetMethods();
				ArrayList list = new ArrayList();
				for(int i = 0; i < methods.Length; i++)
				{
					// we don't want to expose synthetics methods (one reason is that it would
					// mess up the serialVersionUID computation)
					if((methods[i].Modifiers & Modifiers.Synthetic) == 0)
					{
						if(methods[i].ReturnType.IsUnloadable)
						{
							throw JavaException.NoClassDefFoundError(methods[i].ReturnType.Name);
						}
						TypeWrapper[] args = methods[i].GetParameters();
						for(int j = 0; j < args.Length; j++)
						{
							if(args[j].IsUnloadable)
							{
								throw JavaException.NoClassDefFoundError(args[j].Name);
							}
						}
						if(methods[i].Name == "<clinit>")
						{
							// not reported back
						}
						else if((methods[i].Name == "<init>") != getMethods)
						{
							list.Add(methods[i]);
						}
					}
				}
				return (MethodWrapper[])list.ToArray(typeof(MethodWrapper));
			}

			public static object[] GetDeclaredFields(Type type, object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// we need to finish the type otherwise all fields will not be in the field map yet
				wrapper.Finish();
				// we need to look through the array for unloadable types, because we may not let them
				// escape into the 'wild'
				FieldWrapper[] fields = wrapper.GetFields();
				for(int i = 0; i < fields.Length; i++)
				{
					if(fields[i].FieldTypeWrapper.IsUnloadable)
					{
						throw JavaException.NoClassDefFoundError(fields[i].FieldTypeWrapper.Name);
					}
				}
				return fields;
			}

			public static object[] GetDeclaredClasses(Type type, object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// NOTE to get at the InnerClasses we *don't* need to finish the type
				TypeWrapper[] wrappers = wrapper.InnerClasses;
				object[] innerclasses = new object[wrappers.Length];
				for(int i = 0; i < innerclasses.Length; i++)
				{
					if(wrappers[i].IsUnloadable)
					{
						throw JavaException.NoClassDefFoundError(wrappers[i].Name);
					}
					innerclasses[i] = getClassFromWrapper(wrappers[i]);
				}
				return innerclasses;
			}

			public static object GetDeclaringClass(Type type, object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				TypeWrapper declaring = wrapper.DeclaringTypeWrapper;
				if(declaring == null)
				{
					return null;
				}
				if(declaring.IsUnloadable)
				{
					throw JavaException.NoClassDefFoundError(declaring.Name);
				}
				return getClassFromWrapper(declaring);
			}

			public static object[] GetInterfaces(Type type, object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// we need to finish the type otherwise all fields will not be in the field map yet
				wrapper.Finish();
				TypeWrapper[] interfaceWrappers = wrapper.Interfaces;
				object[] interfaces = new object[interfaceWrappers.Length];
				for(int i = 0; i < interfaces.Length; i++)
				{
					interfaces[i] = getClassFromWrapper(interfaceWrappers[i]);
				}
				return interfaces;
			}

			public static int GetModifiers(Type type, Object cwrapper)
			{
				if(type == null)
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					wrapper.Finish();
					type = wrapper.Type;
				}
				// NOTE we don't return the modifiers from the TypeWrapper, because for inner classes
				// the reflected modifiers are different from the physical ones
				Modifiers modifiers = ModifiersAttribute.GetModifiers(type);
				// only returns public, protected, private, final, static, abstract and interface (as per
				// the documentation of Class.getModifiers())
				Modifiers mask = Modifiers.Public | Modifiers.Protected | Modifiers.Private | Modifiers.Final |
					Modifiers.Static | Modifiers.Abstract | Modifiers.Interface;
				return (int)(modifiers & mask);
			}
		}
	}

	namespace io
	{
		public class File
		{
			internal static string DemanglePath(string path)
			{
				//Console.WriteLine("Demangle: " + path);
				// HACK for some reason Java accepts: \c:\foo.txt
				// I don't know what else, but for now lets just support this
				if(path.Length > 3 && (path[0] == '\\' || path[0] == '/') && path[2] == ':')
				{
					path = path.Substring(1);
				}
				return path;
			}

			public static string[] listRootsInternal()
			{
				return System.IO.Directory.GetLogicalDrives();
			}

			public static bool existsInternal(object obj, string path)
			{
				path = DemanglePath(path);
				try
				{
					return NetSystem.IO.File.Exists(path) || NetSystem.IO.Directory.Exists(path);
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool isFileInternal(object obj, string path)
			{
				// TODO handle errors
				// TODO make sure semantics are the same
				try
				{
					return NetSystem.IO.File.Exists(DemanglePath(path));
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool isDirectoryInternal(object obj, string path)
			{
				// TODO handle errors
				// TODO make sure semantics are the same
				try
				{
					return NetSystem.IO.Directory.Exists(DemanglePath(path));
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static long lengthInternal(object obj, string path)
			{
				// TODO handle errors
				try
				{
					return new NetSystem.IO.FileInfo(DemanglePath(path)).Length;
				}
				catch(Exception)
				{
					return 0;
				}
			}

			public static bool mkdirInternal(object obj, string path)
			{
				path = DemanglePath(path);
				// TODO handle errors
				if (!NetSystem.IO.Directory.Exists(NetSystem.IO.Directory.GetParent(path).FullName) ||
					NetSystem.IO.Directory.Exists(path)) 
				{
					return false;
				}
				return NetSystem.IO.Directory.CreateDirectory(path) != null;
			}

			public static bool deleteInternal(object obj, string path) 
			{
				// TODO handle errors
				// TODO shouldn't we demangle the path?
				try
				{
					if (NetSystem.IO.Directory.Exists(path)) 
					{
						NetSystem.IO.Directory.Delete(path);
					} 
					else if (NetSystem.IO.File.Exists(path)) 
					{
						NetSystem.IO.File.Delete(path);
					} 
					else 
					{
						return false;
					}
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool createInternal(string path) 
			{
				// TODO handle errors
				// TODO shouldn't we demangle the path?
				try
				{
					NetSystem.IO.File.Open(path, FileMode.CreateNew).Close();
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}

			private static long DateTimeToJavaLongTime(DateTime datetime)
			{
				return (TimeZone.CurrentTimeZone.ToUniversalTime(datetime) - new DateTime(1970, 1, 1)).Ticks / 10000L;
			}

			public static DateTime JavaLongTimeToDateTime(long datetime)
			{
				return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(new DateTime(1970, 1, 1).Ticks + datetime * 10000L));
			}

			public static long lastModifiedInternal(object obj, string path)
			{
				try
				{
					return DateTimeToJavaLongTime(NetSystem.IO.File.GetLastWriteTime(DemanglePath(path)));
				}
				catch(Exception)
				{
					return 0;
				}
			}

			public static string[] listInternal(object obj, string dirname)
			{
				// TODO error handling
				try
				{
					string[] l = NetSystem.IO.Directory.GetFileSystemEntries(dirname);
					for(int i = 0; i < l.Length; i++)
					{
						int pos = l[i].LastIndexOf(Path.DirectorySeparatorChar);
						if(pos >= 0)
						{
							l[i] = l[i].Substring(pos + 1);
						}
					}
					return l;
				}
				catch(Exception)
				{
					return null;
				}
			}

			public static bool canReadInternal(object obj, string file)
			{
				try
				{
					// HACK if file refers to a directory, we always return true
					if(NetSystem.IO.Directory.Exists(file))
					{
						return true;
					}
					new FileInfo(file).Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite).Close();
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool canWriteInternal(object obj, string file)
			{
				try
				{
					// HACK if file refers to a directory, we always return true
					if(NetSystem.IO.Directory.Exists(file))
					{
						return true;
					}
					new FileInfo(file).Open(FileMode.Open, FileAccess.Write, FileShare.ReadWrite).Close();
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool renameToInternal(object obj, string oldName, string newName)
			{
				try
				{
					new FileInfo(oldName).MoveTo(newName);
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool setLastModifiedInternal(object obj, string file, long lastModified)
			{
				try
				{
					new FileInfo(file).LastWriteTime = JavaLongTimeToDateTime(lastModified);
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}

			public static bool setReadOnlyInternal(object obj, string file)
			{
				try
				{
					new FileInfo(file).Attributes |= FileAttributes.ReadOnly;
					return true;
				}
				catch(Exception)
				{
					return false;
				}
			}
		}

		public class VMObjectStreamClass
		{
			public static bool hasClassInitializer(object clazz)
			{
				Type type = NativeCode.java.lang.Class.getType(clazz);
				try
				{
					if(!type.IsArray && type.TypeInitializer != null)
					{
						if(!ModifiersAttribute.IsSynthetic(type.TypeInitializer))
						{
							return true;
						}
					}
					return false;
				}
				catch(Exception x)
				{
					Console.WriteLine(type.FullName);
					Console.WriteLine(x);
					return false;
				}
			}
		}

		public class ObjectInputStream
		{
			public static object currentClassLoader(object sm)
			{
				// TODO calling currentClassLoader in SecurityManager results in null being returned, so we use our own
				// version for now, don't know what the security implications of this are
				// SECURITY
				return NativeCode.java.lang.VMSecurityManager.currentClassLoader();
			}

			public static object allocateObject(object ois, object clazz)
			{
				Type type = NativeCode.java.lang.Class.getType(clazz);
				return NetSystem.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
			}

			public static void callConstructor(object ois, object clazz, object obj)
			{
				// TODO use TypeWrapper based reflection, instead of .NET reflection
				Type type = NativeCode.java.lang.Class.getType(clazz);
				type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(obj, null);
			}
		}
	}

	namespace util
	{
		public class TimeZone
		{
			public static string getDefaultTimeZoneId()
			{
				// HACK return null, classpath then assumes GMT, which is fine by me, for the time being
				return null;
			}
		}
	}

	namespace net
	{
		public class InetAddress
		{
			public static sbyte[] lookupInaddrAny()
			{
				return new sbyte[] { 0, 0, 0, 0 };
			}

			public static string getLocalHostName()
			{
				// TODO error handling
				return NetSystem.Net.Dns.GetHostName();
			}

			public static sbyte[][] getHostByName(string name)
			{
				// TODO error handling
				try
				{
					NetSystem.Net.IPHostEntry he = NetSystem.Net.Dns.GetHostByName(name);
					NetSystem.Net.IPAddress[] addresses = he.AddressList;
					sbyte[][] list = new sbyte[addresses.Length][];
					for(int i = 0; i < addresses.Length; i++)
					{
						list[i] = AddressToByteArray((int)addresses[i].Address);
					}
					return list;
				}
				catch(Exception x)
				{
					throw JavaException.UnknownHostException(x.Message);
				}
			}

			public static string getHostByAddr(byte[] address)
			{
				return NetSystem.Net.Dns.GetHostByAddress(string.Format("{0}.{1}.{2}.{3}", address[0], address[1], address[2], address[3])).HostName;
			}

			private static sbyte[] AddressToByteArray(int address)
			{
				// TODO check for correctness
				return new sbyte[] { (sbyte)address, (sbyte)(address >> 8), (sbyte)(address >> 16), (sbyte)(address >> 24) };
			}
		}

		public class PlainDatagramSocketImpl
		{
			// TODO this method lives here, because UdpClient.Receive has a ByRef parameter and NetExp doesn't support that
			// I have to figure out a way to support ref parameters from Java
			public static void receive(object obj, object packet)
			{
				sbyte[] data = (sbyte[])packet.GetType().InvokeMember("getData", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, packet, new object[0]);
				int length = (int)packet.GetType().InvokeMember("getLength", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, packet, new object[0]);
				object s = obj.GetType().GetField("socket", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
				NetSystem.Net.Sockets.UdpClient socket = (NetSystem.Net.Sockets.UdpClient)s;
				NetSystem.Net.IPEndPoint remoteEP = new NetSystem.Net.IPEndPoint(0, 0);
				byte[] buf = socket.Receive(ref remoteEP);
				for(int i = 0; i < Math.Min(length, buf.Length); i++)
				{
					data[i] = (sbyte)buf[i];
				}
				packet.GetType().InvokeMember("setLength", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, packet, new object[] { buf.Length });
				long remoteIP = remoteEP.Address.Address;
				string remote = (remoteIP & 0xff) + "." + ((remoteIP >> 8) & 0xff) + "." + ((remoteIP >> 16) & 0xff) + "." + ((remoteIP >> 24) & 0xff);
				object remoteAddress = ClassLoaderWrapper.GetType("java.net.InetAddress").GetMethod("getByName").Invoke(null, new object[] { remote });
				packet.GetType().InvokeMember("setAddress", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, packet, new object[] { remoteAddress });
				packet.GetType().InvokeMember("setPort", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, packet, new object[] { remoteEP.Port });
			}
		}
	}
}

namespace NativeCode.gnu.java.net.protocol.ikvmres
{
	public class IkvmresURLConnection
	{
		public static void InitArray(sbyte[] buf, FieldInfo field)
		{
			NetSystem.Runtime.CompilerServices.RuntimeHelpers.InitializeArray(buf, field.FieldHandle);
		}
	}
}
