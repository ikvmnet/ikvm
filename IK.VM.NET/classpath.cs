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

				public static object Invoke(object methodCookie, object o, object[] args)
				{
					// TODO this is a very lame implementation, no where near correct
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					wrapper.DeclaringType.Finish();
					TypeWrapper[] argWrappers = wrapper.GetParameters();
					Type[] argTypes = new Type[argWrappers.Length];
					for(int i = 0; i < argTypes.Length; i++)
					{
						argWrappers[i].Finish();
						argTypes[i] = argWrappers[i].Type;
						if(argTypes[i].IsPrimitive)
						{
							if(argTypes[i] == typeof(int))
							{
								args[i] = ClassLoaderWrapper.GetType("java.lang.Integer").GetMethod("intValue").Invoke(args[i], new object[0]);
							}
							else if(argTypes[i] == typeof(bool))
							{
								args[i] = ClassLoaderWrapper.GetType("java.lang.Boolean").GetMethod("booleanValue").Invoke(args[i], new object[0]);
							}
							else if(argTypes[i] == typeof(short))
							{
								args[i] = ClassLoaderWrapper.GetType("java.lang.Short").GetMethod("shortValue").Invoke(args[i], new object[0]);
							}
							else
							{
								throw new NotImplementedException("argtype: " + argTypes[i].FullName);
							}
						}
					}
					try
					{
						if(wrapper.Name == "<init>")
						{
							if(o == null)
							{
								return wrapper.DeclaringType.Type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, argTypes, null).Invoke(args);
							}
							else
							{
								throw new NotImplementedException("invoking constructor on existing instance");
							}
						}
						else
						{
							MethodInfo mi;
							if(wrapper.GetMethod() is NetSystem.Reflection.Emit.MethodBuilder || wrapper.GetMethod() == null)
							{
								mi = wrapper.DeclaringType.Type.GetMethod(wrapper.Name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, argTypes, null);
							}
							else
							{
								mi = (MethodInfo)wrapper.GetMethod();
							}
							if(mi == null)
							{
								throw new InvalidOperationException("Method not found: " + wrapper.DeclaringType.Name + "." + wrapper.Name + wrapper.Descriptor.Signature);
							}
							object retval = mi.Invoke(o, args);
							if(wrapper.ReturnType.Type.IsValueType)
							{
								if(wrapper.ReturnType.Type == typeof(int))
								{
									retval = Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.Integer"), new object[] { (int)retval });
								}
								else if(wrapper.ReturnType.Type == typeof(bool))
								{
									retval = Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.Boolean"), new object[] { (bool)retval });
								}
								else if(wrapper.ReturnType.Type == typeof(void))
								{
									// nothing to do
								}
								else
								{
									throw new NotImplementedException("rettype: " + wrapper.ReturnType.Type.FullName);
								}
							}
							return retval;
						}
					}
					catch(TargetInvocationException x)
					{
						throw JavaException.InvocationTargetException(ExceptionHelper.MapException(x.InnerException, typeof(Exception)));
					}
				}

				// TODO remove this, it isn't used anymore
				public static Exception mapException(Exception x)
				{
					return ExceptionHelper.MapException(x, typeof(Exception));
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
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					wrapper.DeclaringType.Finish();
					FieldInfo fi = wrapper.DeclaringType.Type.GetField(wrapper.Name, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if(fi.FieldType.IsValueType)
					{
						if(fi.FieldType == typeof(long))
						{
							return Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.Long"), new object[] { fi.GetValue(o) });
						}
						else
						{
							throw new NotImplementedException("GetValue: " + fi.FieldType.FullName);
						}
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
						throw new NotImplementedException("SetValue: " + fi.FieldType.FullName);
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
				m.Invoke(properties, new string[] { "java.version", "1.1" });
				m.Invoke(properties, new string[] { "java.vendor", "Jeroen Frijters" });
				m.Invoke(properties, new string[] { "java.vendor.url", "http://jeroen.nu/" });
				m.Invoke(properties, new string[] { "java.home", "" });
				m.Invoke(properties, new string[] { "java.vm.specification.version", "" });
				m.Invoke(properties, new string[] { "java.vm.specification.vendor", "" });
				m.Invoke(properties, new string[] { "java.vm.specification.name", "" });
				m.Invoke(properties, new string[] { "java.vm.version", "" });
				m.Invoke(properties, new string[] { "java.vm.vendor", "" });
				m.Invoke(properties, new string[] { "java.vm.name", "" });
				m.Invoke(properties, new string[] { "java.specification.version", "" });
				m.Invoke(properties, new string[] { "java.specification.vendor", "" });
				m.Invoke(properties, new string[] { "java.specification.name", "" });
				m.Invoke(properties, new string[] { "java.class.version", "" });
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
				m.Invoke(properties, new string[] { "os.name", "Windows" });
				m.Invoke(properties, new string[] { "os.arch", "" });
				m.Invoke(properties, new string[] { "os.version", Environment.OSVersion.ToString() });
				m.Invoke(properties, new string[] { "file.separator", Path.DirectorySeparatorChar.ToString() });
				m.Invoke(properties, new string[] { "path.separator", Path.PathSeparator.ToString() });
				m.Invoke(properties, new string[] { "line.separator", Environment.NewLine });
				m.Invoke(properties, new string[] { "user.name", Environment.UserName });
				m.Invoke(properties, new string[] { "user.home", "" });
				m.Invoke(properties, new string[] { "user.dir", Environment.CurrentDirectory });
				m.Invoke(properties, new string[] { "awt.toolkit", "ikvm.awt.NetToolkit, awt, Version=1.0, Culture=neutral, PublicKeyToken=null" });
			}

			public static string nativeGetLibname(string pathname, string libname)
			{
				// TODO
				return libname;
			}

			public static int nativeLoad(object obj, string filename)
			{
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
				// TODO
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
				// TODO I doubt that this is correct
				return double.Parse(s);
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

		/* not used anymore
		public class System
		{
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

			public static void setErr0(object printStream)
			{
				ClassLoaderWrapper.GetType("java.lang.System").GetField("err", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, printStream);
			}

			public static void setIn0(object inputStream)
			{
				ClassLoaderWrapper.GetType("java.lang.System").GetField("in", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, inputStream);
			}

			public static void setOut0(object printStream)
			{
				ClassLoaderWrapper.GetType("java.lang.System").GetField("out", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, printStream);
			}
		}
		*/

		public class VMSystem
		{
			public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
			{
				// TODO
				Array.Copy((Array)src, srcStart, (Array)dest, destStart, len);
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
				// TODO handle errors
				ClassFile classFile = new ClassFile(data, offset, length, name);
				if(name != null && classFile.Name.Replace('/', '.') != name)
				{
					throw JavaException.NoClassDefFoundError("{0} (wrong name: {1})", name, classFile.Name);
				}
//				if(classFile.Name == "org/eclipse/core/internal/boot/InternalBootLoader")
//				{
//					using(FileStream fs = File.Create("internalbootloader.class"))
//					{
//						fs.Write(data, offset, length);
//					}
//				}
				TypeWrapper type = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader).DefineClass(classFile);
				object clazz = Class.CreateInstance(null, type);
				if(protectionDomain != null)
				{
					// TODO cache the FieldInfo
					clazz.GetType().GetField("pd", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(clazz, protectionDomain);
				}
				return clazz;
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

			public static Type getTypeFromWrapper(object clazz, object wrapper)
			{
				((TypeWrapper)wrapper).Finish();
				Type type = ((TypeWrapper)wrapper).Type;
				lock(map.SyncRoot)
				{
					// NOTE since this method can be called multiple times (or after getClassFromType has added
					// the Class to the map), we don't use Add() here, but the indexer because that can handle
					// "overwriting" the existing association (which should always be the same as the new one)
					map[type] = clazz;
				}
				return type;
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
				if(type == null)
				{
					return null;
				}
				if(type is NetSystem.Reflection.Emit.TypeBuilder)
				{
					throw new InvalidOperationException();
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
							if(type.IsDefined(typeof(OverrideStubTypeAttribute), false))
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
					// TODO look for our custom attribute (which doesn't exist yet), that contains the real name of the type
					TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
					if(wrapper != null)
					{
						return wrapper.Name.Replace('/', '.');
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

			public static object[] GetDeclaredMethods(Type type, object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				if(wrapper == null)
				{
					wrapper = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				// we need to finish the type otherwise all methods will not be in the method map yet
				wrapper.Finish();
				return wrapper.GetMethods();
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
				return wrapper.GetFields();
			}

			public static object[] GetDeclaredClasses(Type type, object cwrapper)
			{
				// TODO
				return new object[0];
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
				// only returns public, protected, private, final, static, abstract and interface (as per
				// the documentation of Class.getModifiers())
				Modifiers mask = Modifiers.Public | Modifiers.Protected | Modifiers.Private | Modifiers.Final |
					Modifiers.Static | Modifiers.Abstract | Modifiers.Interface;
				return (int)(wrapper.Modifiers & mask);
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

		public class ObjectInputStream
		{
			public static object currentClassLoader(object sm)
			{
				// TODO calling currentClassLoader in SecurityManager results in null being returned, so we use our own
				// version for now, don't know what the security implications of this are
				// SECURITY
				return NativeCode.java.lang.VMSecurityManager.currentClassLoader();
			}

			public static void callReadMethod(object ois, object obj, object clazz)
			{
				Type type = NativeCode.java.lang.Class.getType(clazz);
				MethodInfo mi = type.GetMethod("readObject", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { ois.GetType() }, null);
				mi.Invoke(obj, new object[] { ois });
			}

			public static object allocateObject(object ois, object clazz)
			{
				Type type = NativeCode.java.lang.Class.getType(clazz);
				return NetSystem.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
			}

			public static void callConstructor(object ois, object clazz, object obj)
			{
				Type type = NativeCode.java.lang.Class.getType(clazz);
				type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null).Invoke(obj, null);
			}

			public static void setBooleanField(object ois, object obj, object clazz, string field_name, bool val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setByteField(object ois, object obj, object clazz, string field_name, sbyte val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setCharField(object ois, object obj, object clazz, string field_name, char val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setDoubleField(object ois, object obj, object clazz, string field_name, double val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setFloatField(object ois, object obj, object clazz, string field_name, float val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setIntField(object ois, object obj, object clazz, string field_name, int val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setLongField(object ois, object obj, object clazz, string field_name, long val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setShortField(object ois, object obj, object clazz, string field_name, short val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			public static void setObjectField(object ois, object obj, object clazz, string field_name, string type_code, object val)
			{
				SetFieldValue(obj, clazz, field_name, val);
			}

			private static void SetFieldValue(object obj, object clazz, string field_name, object val)
			{
				// TODO support overloaded field name
				Type type = NativeCode.java.lang.Class.getType(clazz);
//				while(type != null)
				{
					FieldInfo fi = type.GetField(field_name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if(fi != null)
					{
						fi.SetValue(obj, val);
						return;
					}
					// NOTE if not found, we're moving up the hierarchy, even though I'd expect GetField to do that, it doesn't, at least
					// not for private fields
//					type = type.BaseType;
				}
				throw new InvalidOperationException("SetFieldValue: field not found, field_name = " + field_name + ", obj = " + obj);
			}
		}

		public class ObjectOutputStream
		{
			public static void callWriteMethod(object oos, object obj)
			{
				Type type = obj.GetType();
				MethodInfo mi = type.GetMethod("writeObject", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { oos.GetType() }, null);
				mi.Invoke(obj, new object[] { oos });
			}

			public static bool getBooleanField(object oos, object obj, object clazz, string field_name)
			{
				return (bool)GetFieldValue(obj, clazz, field_name);
			}

			public static sbyte getByteField(object oos, object obj, object clazz, string field_name)
			{
				return (sbyte)GetFieldValue(obj, clazz, field_name);
			}

			public static char getCharField(object oos, object obj, object clazz, string field_name)
			{
				return (char)GetFieldValue(obj, clazz, field_name);
			}

			public static double getDoubleField(object oos, object obj, object clazz, string field_name)
			{
				return (double)GetFieldValue(obj, clazz, field_name);
			}

			public static float getFloatField(object oos, object obj, object clazz, string field_name)
			{
				return (float)GetFieldValue(obj, clazz, field_name);
			}

			public static int getIntField(object oos, object obj, object clazz, string field_name)
			{
				return (int)GetFieldValue(obj, clazz, field_name);
			}

			public static long getLongField(object oos, object obj, object clazz, string field_name)
			{
				return (long)GetFieldValue(obj, clazz, field_name);
			}

			public static short getShortField(object oos, object obj, object clazz, string field_name)
			{
				return (short)GetFieldValue(obj, clazz, field_name);
			}

			public static object getObjectField(object oos, object obj, object clazz, string field_name, string type_code)
			{
				return GetFieldValue(obj, clazz, field_name);
			}

			private static object GetFieldValue(object obj, object clazz, string field_name)
			{
				// TODO support overloaded field name
				Type type = NativeCode.java.lang.Class.getType(clazz);
//				while(type != null)
				{
					FieldInfo fi = type.GetField(field_name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if(fi != null)
					{
						return fi.GetValue(obj);
					}
					// NOTE if not found, we're moving up the hierarchy, even though I'd expect GetField to do that, it doesn't, at least
					// not for private fields
//					type = type.BaseType;
				}
				throw new InvalidOperationException("GetFieldValue: field not found, field_name = " + field_name + ", obj = " + obj);
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
