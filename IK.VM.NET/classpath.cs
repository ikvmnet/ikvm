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

namespace NativeCode.java
{
	namespace lang
	{
		namespace reflect
		{
			public class Proxy
			{
				// NOTE not used, only here to shut up ikvmc during compilation of classpath.dll
				public static object getProxyClass0(object o1, object o2)
				{
					throw new InvalidOperationException();
				}
				
				// NOTE not used, only here to shut up ikvmc during compilation of classpath.dll
				public static object getProxyData0(object o1, object o2)
				{
					throw new InvalidOperationException();
				}
				
				// NOTE not used, only here to shut up ikvmc during compilation of classpath.dll
				public static object generateProxyClass0(object o1, object o2)
				{
					throw new InvalidOperationException();
				}
			}

			public class Array
			{
				public static object createObjectArray(object clazz, int dim)
				{
					if(dim >= 0)
					{
						// TODO handle ghost types
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
					else if(o is byte)
					{
						return Activator.CreateInstance(java_lang_Byte, new object[] { (sbyte)(byte)o });
					}
					else if(o is bool)
					{
						return Activator.CreateInstance(java_lang_Boolean, new object[] { o });
					}
					else if(o is short)
					{
						return Activator.CreateInstance(java_lang_Short, new object[] { o });
					}
					else if(o is ushort)
					{
						return Activator.CreateInstance(java_lang_Short, new object[] { (short)(ushort)o });
					}
					else if(o is char)
					{
						return Activator.CreateInstance(java_lang_Character, new object[] { o });
					}
					else if(o is int)
					{
						return Activator.CreateInstance(java_lang_Integer, new object[] { o });
					}
					else if(o is uint)
					{
						return Activator.CreateInstance(java_lang_Integer, new object[] { (int)(uint)o });
					}
					else if(o is long)
					{
						return Activator.CreateInstance(java_lang_Long, new object[] { o });
					}
					else if(o is ulong)
					{
						return Activator.CreateInstance(java_lang_Long, new object[] { (long)(ulong)o });
					}
					else if(o is float)
					{
						return Activator.CreateInstance(java_lang_Float, new object[] { o });
					}
					else if(o is double)
					{
						return Activator.CreateInstance(java_lang_Double, new object[] { o });
					}
					else if(o is Enum)
					{
						Type enumType = Enum.GetUnderlyingType(o.GetType());
						if(enumType == typeof(byte) || enumType == typeof(sbyte))
						{
							return JavaWrapper.Box((sbyte)((IConvertible)o).ToInt32(null));
						}
						else if(enumType == typeof(short) || enumType == typeof(ushort))
						{
							return JavaWrapper.Box((short)((IConvertible)o).ToInt32(null));
						}
						else if(enumType == typeof(int))
						{
							return JavaWrapper.Box(((IConvertible)o).ToInt32(null));
						}
						else if(enumType == typeof(uint))
						{
							return JavaWrapper.Box(unchecked((int)((IConvertible)o).ToUInt32(null)));
						}
						else if(enumType == typeof(long))
						{
							return JavaWrapper.Box(((IConvertible)o).ToInt64(null));
						}
						else if(enumType == typeof(ulong))
						{
							return JavaWrapper.Box(unchecked((long)((IConvertible)o).ToUInt64(null)));
						}
						else
						{
							throw new InvalidOperationException();
						}
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
					wrapper.DeclaringType.Finish();
					TypeWrapper[] exceptions = wrapper.GetExceptions();
					object[] exceptionClasses = new object[exceptions.Length];
					for(int i = 0; i < exceptions.Length; i++)
					{
						// TODO check for unloadable types
						exceptionClasses[i] = VMClass.getClassFromWrapper(exceptions[i]);
					}
					return exceptionClasses;
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
				// HACK this is used by netexp to query the constant value of a field
				public static object getConstant(object field)
				{
					// HACK we use reflection to extract the fieldCookie from the java.lang.reflect.Field object
					FieldWrapper wrapper = (FieldWrapper)field.GetType().GetField("fieldCookie", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(field);
					return wrapper.GetConstant();
				}

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
					return VMClass.getClassFromWrapper(wrapper.FieldTypeWrapper);
				}

				public static object GetValue(object fieldCookie, object o)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					object val = wrapper.GetValue(o);
					if(wrapper.FieldTypeWrapper.IsPrimitive)
					{
						val = JavaWrapper.Box(val);
					}
					return val;
				}

				public static void SetValue(object fieldCookie, object o, object v)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					if(wrapper.FieldTypeWrapper.IsPrimitive)
					{
						v = JavaWrapper.Unbox(v);
					}
					wrapper.SetValue(o, v);
				}
			}
		}

		public class Runtime
		{
			public static void insertSystemProperties(object properties)
			{
				MethodInfo m = properties.GetType().GetMethod("setProperty");
				// TODO set all these properties to something useful
				m.Invoke(properties, new string[] { "java.version", "1.4" });
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
				m.Invoke(properties, new string[] { "java.specification.version", "1.4" });
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
				if(isFloat)
				{
					float f = (float)d;
					// TODO this is not correct, we need to use the Java algorithm of converting a float to string
					if(float.IsNaN(f))
					{
						return "NaN";
					}
					if(float.IsNegativeInfinity(f))
					{
						return "-Infinity";
					}
					if(float.IsPositiveInfinity(f))
					{
						return "Infinity";
					}
					// HACK really lame hack to apprioximate the Java behavior a little bit
					string s = f.ToString(System.Globalization.CultureInfo.InvariantCulture);
					if(s.IndexOf('.') == -1)
					{
						s += ".0";
					}
					return s;
				}
				else
				{
					StringBuilder sb = new StringBuilder();
					DoubleToString.append(sb, d);
					return sb.ToString();
				}
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
					throw JavaException.NullPointerException();
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
					throw JavaException.NullPointerException();
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
					throw JavaException.NullPointerException();
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
					throw JavaException.NullPointerException();
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
					// NOTE side effect of GetTypeHandle call is null check for src and dest (it
					// throws an ArgumentNullException)
					// Since constructing a Type object is expensive, we use Type.GetTypeHandle and
					// hope that it is implemented in a such a way that it is more efficient than
					// Object.GetType()
					try
					{
						RuntimeTypeHandle type_src = Type.GetTypeHandle(src);
						RuntimeTypeHandle type_dst = Type.GetTypeHandle(dest);
						if(type_src.Value != type_dst.Value)
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
					catch(ArgumentNullException)
					{
						throw JavaException.NullPointerException();
					}
				}
				try 
				{
					Array.Copy((Array)src, srcStart, (Array)dest, destStart, len);
				}
				catch(ArgumentNullException)
				{
					throw JavaException.NullPointerException();
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

			public static int identityHashCode(object o)
			{
				return RuntimeHelpers.GetHashCode(o);
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

			public static object getPrimitiveClass(char type)
			{
				switch(type)
				{
					case 'Z':
						return VMClass.getClassFromType(typeof(bool));
					case 'B':
						return VMClass.getClassFromType(typeof(sbyte));
					case 'C':
						return VMClass.getClassFromType(typeof(char));
					case 'D':
						return VMClass.getClassFromType(typeof(double));
					case 'F':
						return VMClass.getClassFromType(typeof(float));
					case 'I':
						return VMClass.getClassFromType(typeof(int));
					case 'J':
						return VMClass.getClassFromType(typeof(long));
					case 'S':
						return VMClass.getClassFromType(typeof(short));
					case 'V':
						return VMClass.getClassFromType(typeof(void));
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
					object clazz = VMClass.CreateClassInstance(type);
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
			private delegate object CreateClassDelegate(object typeWrapper);
			private static CreateClassDelegate CreateClass;
			private static MethodInfo getWrapper;

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

			internal static object CreateClassInstance(TypeWrapper wrapper)
			{
				if(CreateClass == null)
				{
					CreateClass = (CreateClassDelegate)Delegate.CreateDelegate(typeof(CreateClassDelegate), ClassLoaderWrapper.GetType("java.lang.VMClass").GetMethod("createClass", BindingFlags.Static | BindingFlags.Public));
					// HACK to make sure we don't run into any problems creating class objects for classes that
					// participate in the VMClass static initialization, we first do a bogus call to initialize
					// the machinery (I ran into this when running netexp on classpath.dll)
					CreateClass(null);
					lock(map.SyncRoot)
					{
						object o = map[wrapper];
						if(o != null)
						{
							return o;
						}
					}
				}
				object clazz = CreateClass(wrapper);
				lock(map.SyncRoot)
				{
					if(wrapper != null)
					{
						map.Add(wrapper, clazz);
					}
				}
				return clazz;
			}

			public static bool IsAssignableFrom(object w1, object w2)
			{
				return ((TypeWrapper)w2).IsAssignableTo((TypeWrapper)w1);
			}

			public static bool IsInterface(object wrapper)
			{
				return ((TypeWrapper)wrapper).IsInterface;
			}

			public static bool IsArray(object wrapper)
			{
				return ((TypeWrapper)wrapper).IsArray;
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
				return type;
			}

			public static object getWrapperFromType(Type t)
			{
				return ClassLoaderWrapper.GetWrapperFromType(t);
			}

			public static Type getType(object clazz)
			{
				if(getWrapper == null)
				{
					getWrapper = ClassLoaderWrapper.GetType("java.lang.VMClass").GetMethod("getWrapperFromClass", BindingFlags.NonPublic | BindingFlags.Static);
				}
				TypeWrapper wrapper = (TypeWrapper)getWrapper.Invoke(null, new object[] { clazz });
				wrapper.Finish();
				return wrapper.Type;
			}

			internal static object getClassFromWrapper(TypeWrapper wrapper)
			{
				lock(map.SyncRoot)
				{
					object clazz = map[wrapper];
					if(clazz == null)
					{
						clazz = CreateClassInstance(wrapper);
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
				return getClassFromWrapper(ClassLoaderWrapper.GetWrapperFromType(type));
			}

			public static string GetName(object wrapper)
			{
				TypeWrapper typeWrapper = (TypeWrapper)wrapper;
				if(typeWrapper.IsPrimitive)
				{
					if(typeWrapper == PrimitiveTypeWrapper.VOID)
					{
						return "void";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.BYTE)
					{
						return "byte";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.BOOLEAN)
					{
						return "boolean";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.SHORT)
					{
						return "short";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.CHAR)
					{
						return "char";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.INT)
					{
						return "int";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.LONG)
					{
						return "long";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.FLOAT)
					{
						return "float";
					}
					else if(typeWrapper == PrimitiveTypeWrapper.DOUBLE)
					{
						return "double";
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				return typeWrapper.Name;
			}
	
			internal static string getName(Type type)
			{
				TypeWrapper wrapperType = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
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
						// HACK we're assuming for the time being that Java code cannot define new value types
						return DotNetTypeWrapper.GetName(type);
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
							// HACK we're assuming for the time being that Java code cannot define new value types
							sb.Append(DotNetTypeWrapper.GetName(type));
						}
					}
					else
					{
						sb.Append('L').Append(getName(type)).Append(';');
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
					if(type.Assembly is System.Reflection.Emit.AssemblyBuilder || type.Assembly.IsDefined(typeof(JavaAssemblyAttribute), false))
					{
						return type.FullName;
					}
					else
					{
						return DotNetTypeWrapper.GetName(type);
					}
				}
			}

			[StackTraceInfo(Hidden = true)]
			public static void initialize(object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				wrapper.Finish();
				RuntimeHelpers.RunClassConstructor(wrapper.Type.TypeHandle);
			}

			public static object getClassLoader0(object wrapper)
			{
				return ((TypeWrapper)wrapper).GetClassLoader().GetJavaClassLoader();
			}

			public static object getClassLoaderFromType(Type type)
			{
				return ClassLoaderWrapper.GetWrapperFromType(type).GetClassLoader().GetJavaClassLoader();
			}

			public static object[] GetDeclaredMethods(object cwrapper, bool getMethods, bool publicOnly)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
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

			public static object[] GetDeclaredFields(object cwrapper, bool publicOnly)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
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

			public static object[] GetDeclaredClasses(object cwrapper, bool publicOnly)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
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

			public static object GetDeclaringClass(object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
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

			public static object[] GetInterfaces(object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				// we need to finish the type otherwise all fields will not be in the field map yet
				// TODO this should not be needed (make sure it isn't and remove)
				wrapper.Finish();
				TypeWrapper[] interfaceWrappers = wrapper.Interfaces;
				object[] interfaces = new object[interfaceWrappers.Length];
				for(int i = 0; i < interfaces.Length; i++)
				{
					interfaces[i] = getClassFromWrapper(interfaceWrappers[i]);
				}
				return interfaces;
			}

			public static int GetModifiers(Object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
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

			public static string getLocalHostname()
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
						byte[] address = addresses[i].GetAddressBytes();
						sbyte[] sb = new sbyte[address.Length];
						for(int j = 0; j < sb.Length; j++)
						{
							sb[j] = (sbyte)address[j];
						}
						list[i] = sb;
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
		}
	}

	namespace nio
	{
		namespace channels
		{
			// HACK this is a rubbish implementation
			public class FileChannelImpl
			{
				private static Stream GetStream(object o)
				{
					object fd = o.GetType().GetField("fd", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(o);
					return (Stream)fd.GetType().GetField("stream", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(fd);
				}

				public static long implPosition(object thiz)
				{
					// TODO map exceptions
					return GetStream(thiz).Position;
				}

				public static object implPosition(object thiz, long newPosition)
				{
					// TODO map exceptions
					GetStream(thiz).Position = newPosition;
					// why are we returning thiz?
					return thiz;
				}

				public static object implTruncate(object thiz, long size)
				{
					throw new NotImplementedException();
				}
  
				public static IntPtr nio_mmap_file(object thiz, long pos, long size, int mode)
				{
					throw new NotImplementedException();
				}

				public static void nio_unmmap_file(object thiz, IntPtr map_address, int size)
				{
					throw new NotImplementedException();
				}

				public static void nio_msync(object thiz, IntPtr map_address, int length)
				{
				}

				public static long size(object thiz)
				{
					// TODO map exceptions
					return GetStream(thiz).Length;
				}

				public static int implRead(object thiz, byte[] buffer, int offset, int length)
				{
					// TODO map exceptions
					return GetStream(thiz).Read(buffer, offset, length);
				}

				public static int implWrite(object thiz, byte[] buffer, int offset, int length)
				{
					// TODO map exceptions
					GetStream(thiz).Write(buffer, offset, length);
					return length;
				}
			}
		}
	}
}
