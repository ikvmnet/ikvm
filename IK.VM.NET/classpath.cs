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
using OpenSystem.Java;
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
						return NetSystem.Array.CreateInstance(VMClass.getType(clazz), dim);
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
						throw JavaException.IllegalArgumentException(type.FullName);
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
					return VMClass.getClassFromWrapper(wrapper.ReturnType);
				}

				public static object[] GetParameterTypes(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					TypeWrapper[] parameters = wrapper.GetParameters();
					object[] parameterClasses = new object[parameters.Length];
					for(int i = 0; i < parameters.Length; i++)
					{
						parameterClasses[i] = VMClass.getClassFromWrapper(parameters[i]);
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
					object[] argsCopy = new Object[args != null ? args.Length : 0];
					MethodWrapper mw = (MethodWrapper)methodCookie;
					mw.DeclaringType.Finish();
					TypeWrapper[] argWrappers = mw.GetParameters();
					for(int i = 0; i < argWrappers.Length; i++)
					{
						if(argWrappers[i].IsPrimitive)
						{
							if(args[i] == null)
							{
								throw JavaException.IllegalArgumentException("primitive wrapper null");
							}
							argsCopy[i] = JavaWrapper.Unbox(args[i]);
						}
						else
						{
							argsCopy[i] = args[i];
						}
					}
					object retval = mw.Invoke(o, argsCopy, false);
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
					return VMClass.getClassFromType(wrapper.FieldType);
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
				string libraryPath = ".";
				if(Environment.OSVersion.ToString().IndexOf("Unix") >= 0)
				{
					string ldLibraryPath = Environment.GetEnvironmentVariable("LD_LIBRARY_PATH");
					if (ldLibraryPath != null)
					{
						libraryPath = ldLibraryPath;
					}
					else
					{
						libraryPath = "";
					}
				}
				m.Invoke(properties, new string[] { "java.library.path", libraryPath });
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
				if(Environment.OSVersion.ToString().IndexOf("Unix") >= 0)
				{
					return "lib" + libname + ".so";
				}

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

			public static long freeMemory(object obj)
			{
				// TODO figure out if there is anything meaningful we can return here
				return 10 * 1024 * 1024;
			}

			public static long maxMemory(object obj)
			{
				// spec says: If there is no inherent limit then the value Long.MAX_VALUE will be returned.
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
				if(double.IsInfinity(f2) && !double.IsInfinity(f1))
				{
					return f1;
				}
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
				if(double.IsInfinity(y) && double.IsInfinity(x))
				{
					if(double.IsPositiveInfinity(y))
					{
						if(double.IsPositiveInfinity(x))
						{
							return NetSystem.Math.PI / 4.0;
						}
						else
						{
							return NetSystem.Math.PI * 3.0 / 4.0;
						}
					}
					else
					{
						if(double.IsPositiveInfinity(x))
						{
							return - NetSystem.Math.PI / 4.0;
						}
						else
						{
							return - NetSystem.Math.PI * 3.0 / 4.0;
						}
					}
				}
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

			public static string toString(double d, bool isFloat)
			{
				StringBuilder sb = new StringBuilder();
				if(isFloat)
				{
					StringBufferHelper.append(sb, (float)d);
				}
				else
				{
					StringBufferHelper.append(sb, d);
				}
				return sb.ToString();
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
						ar.Add(VMClass.getClassFromType(frame.GetMethod().DeclaringType));
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
			public static void arraycopy_primitive_8(Array src, int srcStart, Array dest, int destStart, int len)
			{
				try 
				{
					checked
					{
						Buffer.BlockCopy(src, srcStart << 3, dest, destStart << 3, len << 3);
						return;
					}
				}
				catch(ArgumentNullException)
				{
					throw new NullReferenceException();
				}
				catch(OverflowException)
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch(ArgumentException) 
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
			}

			public static void arraycopy_primitive_4(Array src, int srcStart, Array dest, int destStart, int len)
			{
				try 
				{
					checked
					{
						Buffer.BlockCopy(src, srcStart << 2, dest, destStart << 2, len << 2);
						return;
					}
				}
				catch(ArgumentNullException)
				{
					throw new NullReferenceException();
				}
				catch(OverflowException)
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch(ArgumentException) 
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
			}

			public static void arraycopy_primitive_2(Array src, int srcStart, Array dest, int destStart, int len)
			{
				try 
				{
					checked
					{
						Buffer.BlockCopy(src, srcStart << 1, dest, destStart << 1, len << 1);
						return;
					}
				}
				catch(ArgumentNullException)
				{
					throw new NullReferenceException();
				}
				catch(OverflowException)
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch(ArgumentException) 
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
			}

			public static void arraycopy_primitive_1(Array src, int srcStart, Array dest, int destStart, int len)
			{
				try 
				{
					Buffer.BlockCopy(src, srcStart, dest, destStart, len);
					return;
				}
				catch(ArgumentNullException)
				{
					throw new NullReferenceException();
				}
				catch(OverflowException)
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch(ArgumentException) 
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
			}

			public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
			{
				if(src != dest)
				{
					// NOTE side effect is null check for src and dest
					Type type_src = src.GetType();
					Type type_dst = dest.GetType();
					if(type_src != type_dst)
					{
						if(len >= 0)
						{
							try
							{
								// since Java strictly defines what happens when an ArrayStoreException occurs during copying
								// and .NET doesn't, we have to do it by hand
								Object[] src1 = (Object[])src;
								Object[] dst1 = (Object[])dest;
								for(; len > 0; len--)
								{
									dst1[destStart++] = src1[srcStart++];
								}
								return;
							}
							catch(InvalidCastException)
							{
								throw JavaException.ArrayStoreException("cast failed");
							}
						}
						throw JavaException.ArrayIndexOutOfBoundsException();
					}
				}
				try 
				{
					Array.Copy((Array)src, srcStart, (Array)dest, destStart, len);
				}
				catch(ArgumentNullException)
				{
					throw new NullReferenceException();
				}
				catch(ArgumentException) 
				{
					throw JavaException.ArrayIndexOutOfBoundsException();
				}
				catch(InvalidCastException x)
				{
					if(!src.GetType().IsArray)
					{
						throw JavaException.ArrayStoreException("source is not an array");
					}
					if(!dest.GetType().IsArray)
					{
						throw JavaException.ArrayStoreException("destination is not an array");
					}
					// this shouldn't happen
					throw JavaException.ArrayStoreException(x.Message);
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
					if(name != null && classFile.Name != name)
					{
						throw JavaException.NoClassDefFoundError("{0} (wrong name: {1})", name, classFile.Name);
					}
					TypeWrapper type = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader).DefineClass(classFile);
					object clazz = VMClass.CreateInstance(null, type);
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

		public class VMClass
		{
			private static Hashtable map = new Hashtable();
			private static MethodInfo createClass;
			private static MethodInfo getTypeMethod;

			public static void throwException(Exception e)
			{
				throw e;
			}

			public static object loadArrayClass(string name, object classLoader)
			{
				ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader);
				TypeWrapper type = classLoaderWrapper.LoadClassByDottedName(name);
				return getClassFromWrapper(type);
			}

			public static object loadBootstrapClass(string name, bool initialize)
			{
				TypeWrapper type = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
				if(type != null)
				{
					if(initialize)
					{
						type.Finish();
						RuntimeHelpers.RunClassConstructor(type.Type.TypeHandle);
					}
					return getClassFromWrapper(type);
				}
				return null;
			}

			internal static object CreateInstance(Type type, TypeWrapper wrapper)
			{
				TypeWrapper.AssertFinished(type);
				if(createClass == null)
				{
					createClass = ClassLoaderWrapper.GetType("java.lang.VMClass").GetMethod("createClass", BindingFlags.Static | BindingFlags.NonPublic);
				}
				object clazz = createClass.Invoke(null, new object[] { type, wrapper });
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
					getTypeMethod = ClassLoaderWrapper.GetType("java.lang.VMClass").GetMethod("getTypeFromClass", BindingFlags.NonPublic | BindingFlags.Static);
				}
				return (Type)getTypeMethod.Invoke(null, new object[] { clazz });
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
							if(!type.IsArray && type.IsDefined(typeof(HideFromReflectionAttribute), false))
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
						return name;
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
						return name;
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
					while(type.IsDefined(typeof(HideFromReflectionAttribute), false))
					{
						type = type.BaseType;
					}
					TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
					if(wrapper != null)
					{
						return wrapper.Name;
					}
					// look for our custom attribute, that contains the real name of the type (for inner classes)
					Object[] attribs = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
					if(attribs.Length == 1)
					{
						return ((InnerClassAttribute)attribs[0]).InnerClassName;
					}
					return type.FullName;
				}
			}

			[StackTraceInfo(Hidden = true)]
			public static void initializeType(Type type)
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}

			public static object getClassLoader0(Type type, object wrapper)
			{
				if(wrapper != null)
				{
					return ((TypeWrapper)wrapper).GetClassLoader().GetJavaClassLoader();
				}
				return ClassLoaderWrapper.GetClassLoader(type).GetJavaClassLoader();
			}

			public static object[] GetDeclaredMethods(Type type, object cwrapper, bool getMethods, bool publicOnly)
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
					// we don't want to expose "hideFromReflection" methods (one reason is that it would
					// mess up the serialVersionUID computation)
					if(!methods[i].IsHideFromReflection)
					{
						if(methods[i].Name == "<clinit>")
						{
							// not reported back
						}
						else if(publicOnly && !methods[i].IsPublic)
						{
							// caller is only asking for public methods, so we don't return this non-public method
						}
						else if((methods[i].Name == "<init>") != getMethods)
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
							list.Add(methods[i]);
						}
					}
				}
				return (MethodWrapper[])list.ToArray(typeof(MethodWrapper));
			}

			public static object[] GetDeclaredFields(Type type, object cwrapper, bool publicOnly)
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
				if(publicOnly)
				{
					ArrayList list = new ArrayList();
					for(int i = 0; i < fields.Length; i++)
					{
						if(fields[i].IsPublic)
						{
							list.Add(fields[i]);
						}
					}
					fields = (FieldWrapper[])list.ToArray(typeof(FieldWrapper));
				}
				for(int i = 0; i < fields.Length; i++)
				{
					if(fields[i].FieldTypeWrapper.IsUnloadable)
					{
						throw JavaException.NoClassDefFoundError(fields[i].FieldTypeWrapper.Name);
					}
				}
				return fields;
			}

			public static object[] GetDeclaredClasses(Type type, object cwrapper, bool publicOnly)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// NOTE to get at the InnerClasses we need to finish the type
				wrapper.Finish();
				TypeWrapper[] wrappers = wrapper.InnerClasses;
				if(publicOnly)
				{
					ArrayList list = new ArrayList();
					for(int i = 0; i < wrappers.Length; i++)
					{
						if(wrappers[i].IsUnloadable)
						{
							throw JavaException.NoClassDefFoundError(wrappers[i].Name);
						}
						// because the VM lacks any support for nested visibility control, we
						// cannot rely on the publicness of the type here, but instead we have
						// to look at the reflective modifiers
						wrappers[i].Finish();
						if((wrappers[i].ReflectiveModifiers & Modifiers.Public) != 0)
						{
							list.Add(wrappers[i]);
						}
					}
					wrappers = (TypeWrapper[])list.ToArray(typeof(TypeWrapper));
				}
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
				// before we can call DeclaringTypeWrapper, we need to finish the type
				wrapper.Finish();
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
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// NOTE ReflectiveModifiers is only available for finished types
				wrapper.Finish();
				// NOTE we don't return the modifiers from the TypeWrapper, because for inner classes
				// the reflected modifiers are different from the physical ones
				Modifiers modifiers = wrapper.ReflectiveModifiers;
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
				Type type = NativeCode.java.lang.VMClass.getType(clazz);
				try
				{
					if(!type.IsArray && type.TypeInitializer != null)
					{
						return !AttributeHelper.IsHideFromReflection(type.TypeInitializer);
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
				Type type = NativeCode.java.lang.VMClass.getType(clazz);
				return NetSystem.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
			}

			public static void callConstructor(object ois, object clazz, object obj)
			{
				// TODO use TypeWrapper based reflection, instead of .NET reflection
				Type type = NativeCode.java.lang.VMClass.getType(clazz);
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
				NetSystem.TimeZone currentTimeZone = NetSystem.TimeZone.CurrentTimeZone;
				NetSystem.TimeSpan timeSpan = currentTimeZone.GetUtcOffset(DateTime.Now);

				int hours = timeSpan.Hours;
				int mins = timeSpan.Minutes;

				return "GMT" + hours + ":" + mins;
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
						list[i] = AddressToByteArray(addresses[i]);
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
				string s;
				try
				{
					s = NetSystem.Net.Dns.GetHostByAddress(string.Format("{0}.{1}.{2}.{3}", address[0], address[1], address[2], address[3])).HostName;
				}
				catch(NetSystem.Net.Sockets.SocketException x)
				{
					throw JavaException.UnknownHostException(x.Message);
				}
				try
				{
					NetSystem.Net.Dns.GetHostByName(s);
				}
				catch(NetSystem.Net.Sockets.SocketException)
				{
					// BUG .NET framework bug
					// HACK if GetHostByAddress returns a netbios name, it appends the default DNS suffix, but if the
					// machine's netbios name isn't the same as the DNS hostname, this might result in an unresolvable
					// name, if that happens we chop of the DNS suffix.
					int idx = s.IndexOf('.');
					if(idx > 0)
					{
						return s.Substring(0, idx);
					}
				}
				return s;
			}

			public static sbyte[] AddressToByteArray(NetSystem.Net.IPAddress ipaddress)
			{
				// TODO check for correctness
				int address = (int)ipaddress.Address;
				return new sbyte[] { (sbyte)address, (sbyte)(address >> 8), (sbyte)(address >> 16), (sbyte)(address >> 24) };
			}
		}
	}
}

namespace NativeCode.ikvm.lang
{
	// TODO instead of having these methods here, they should be defined as inlined CIL in map.xml
	public class CIL
	{
		public static int unbox_int(object o)
		{
			return (int)o;
		}
	}
}
