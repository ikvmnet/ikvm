/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Security.Permissions;
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;

using NetSystem = System;

namespace IKVM.NativeCode.java
{
	namespace lang
	{
		namespace reflect
		{
			public class Proxy
			{
				// NOTE not used, only here to shut up ikvmc during compilation of IKVM.GNU.Classpath.dll
				public static object getProxyClass0(object o1, object o2)
				{
					throw new InvalidOperationException();
				}
				
				// NOTE not used, only here to shut up ikvmc during compilation of IKVM.GNU.Classpath.dll
				public static object getProxyData0(object o1, object o2)
				{
					throw new InvalidOperationException();
				}
				
				// NOTE not used, only here to shut up ikvmc during compilation of IKVM.GNU.Classpath.dll
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
						TypeWrapper wrapper = VMClass.getWrapperFromClass(clazz);
						wrapper.Finish();
						return NetSystem.Array.CreateInstance(wrapper.TypeAsArrayType, dim);
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
					TypeWrapper retType = wrapper.ReturnType;
					// HACK we should have a better plan than this
					if(retType.IsUnloadable)
					{
						retType = wrapper.DeclaringType.GetClassLoader().FieldTypeWrapperFromSig(retType.SigName);
					}
					return VMClass.getClassFromWrapper(retType);
				}

				public static object[] GetParameterTypes(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					TypeWrapper[] parameters = wrapper.GetParameters();
					object[] parameterClasses = new object[parameters.Length];
					for(int i = 0; i < parameters.Length; i++)
					{
						TypeWrapper paramType = parameters[i];
						// HACK we should have a better plan than this
						if(paramType.IsUnloadable)
						{
							paramType = wrapper.DeclaringType.GetClassLoader().FieldTypeWrapperFromSig(paramType.SigName);
						}
						parameterClasses[i] = VMClass.getClassFromWrapper(paramType);
					}
					return parameterClasses;
				}

				public static string[] GetExceptionTypes(object methodCookie)
				{
					try
					{
						MethodWrapper wrapper = (MethodWrapper)methodCookie;
						wrapper.DeclaringType.Finish();
						return wrapper.GetExceptions();
					}
					catch(RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}

				[HideFromJava]
				public static object Invoke(object methodCookie, object o, object[] args)
				{
					try
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
								argsCopy[i] = JVM.Library.unbox(args[i]);
							}
							else
							{
								argsCopy[i] = args[i];
							}
						}
						// if the method is an interface method, we must explicitly run <clinit>,
						// because .NET reflection doesn't
						if(mw.DeclaringType.IsInterface)
						{
							mw.DeclaringType.RunClassInit();
						}
						object retval = mw.Invoke(o, argsCopy, false);
						if(mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
						{
							retval = JVM.Library.box(retval);
						}
						return retval;
					}
					catch(RetargetableJavaException x)
					{
						throw x.ToJava();
					}
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
					TypeWrapper fieldType = wrapper.FieldTypeWrapper;
					// HACK we should have a better plan than this
					if(fieldType.IsUnloadable)
					{
						fieldType = wrapper.DeclaringType.GetClassLoader().FieldTypeWrapperFromSig(fieldType.SigName);
					}
					return VMClass.getClassFromWrapper(fieldType);
				}

				public static bool isSamePackage(object a, object b)
				{
					return VMClass.getWrapperFromClass(a).IsInSamePackageAs(VMClass.getWrapperFromClass(b));
				}

				public static object getClassFromFrame(NetSystem.Diagnostics.StackFrame frame)
				{
					Type type = frame.GetMethod().DeclaringType;
					if(type == null)
					{
						return null;
					}
					return VMClass.getClassFromType(type);
				}

				public static object GetValue(object fieldCookie, object o)
				{
					Profiler.Enter("Field.GetValue");
					try
					{
						FieldWrapper wrapper = (FieldWrapper)fieldCookie;
						// if the field is an interface field, we must explicitly run <clinit>,
						// because .NET reflection doesn't
						if(wrapper.DeclaringType.IsInterface)
						{
							wrapper.DeclaringType.RunClassInit();
						}
						object val = wrapper.GetValue(o);
						if(wrapper.FieldTypeWrapper.IsPrimitive)
						{
							val = JVM.Library.box(val);
						}
						return val;
					}
					finally
					{
						Profiler.Leave("Field.GetValue");
					}
				}

				public static void SetValue(object fieldCookie, object o, object v)
				{
					Profiler.Enter("Field.SetValue");
					try
					{
						FieldWrapper wrapper = (FieldWrapper)fieldCookie;
						if(wrapper.IsFinal)
						{
							// NOTE Java runs the class initializer when trying to set a final field
							wrapper.DeclaringType.RunClassInit();
							// NOTE even if the caller is the class itself, it still isn't legal
							throw JavaException.IllegalAccessException("Field is final");
						}
						if(wrapper.FieldTypeWrapper.IsPrimitive)
						{
							v = JVM.Library.unbox(v);
						}
						// if the field is an interface field, we must explicitly run <clinit>,
						// because .NET reflection doesn't
						if(wrapper.DeclaringType.IsInterface)
						{
							wrapper.DeclaringType.RunClassInit();
						}
						wrapper.SetValue(o, v);
					}
					finally
					{
						Profiler.Leave("Field.SetValue");
					}
				}
			}
		}

		public class VMRuntime
		{
			public static string getVersion()
			{
				return typeof(VMRuntime).Assembly.GetName().Version.ToString();
			}

			public static int nativeLoad(string filename)
			{
				return IKVM.Runtime.JniHelper.LoadLibrary(filename);
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
					if(s.Trim() == "+Infinity")
					{
						return double.PositiveInfinity;
					}
					// TODO I doubt that this is correct
					return double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
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
					// make sure -0.0 renders correctly
					if(d == 0.0 && BitConverter.DoubleToInt64Bits(d) < 0)
					{
						return "-" + s;
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
						Type type = frame.GetMethod().DeclaringType;
						if(type != null)
						{
							ar.Add(VMClass.getClassFromType(type));
						}
					}
				}
				return ar.ToArray(CoreClasses.java.lang.Class.Wrapper.TypeAsArrayType);
			}

			public static object currentClassLoader()
			{
				// TODO handle PrivilegedAction
				NetSystem.Diagnostics.StackTrace st = new NetSystem.Diagnostics.StackTrace();
				for(int i = 0; i < st.FrameCount; i++)
				{
					NetSystem.Diagnostics.StackFrame frame = st.GetFrame(i);
					Type type = frame.GetMethod().DeclaringType;
					if(type != null)
					{
						TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
						if(wrapper != null && wrapper.GetClassLoader().GetJavaClassLoader() != null)
						{
							return wrapper.GetClassLoader().GetJavaClassLoader();
						}
					}
				}
				return null;
			}
		}

		public class VMSystem
		{
			public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
			{
				ByteCodeHelper.arraycopy(src, srcStart, dest, destStart, len);
			}

			public static void setErr(object printStream)
			{
				TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical("java.lang.System");
				FieldWrapper fw = tw.GetFieldWrapper("err", "Ljava.io.PrintStream;");
				fw.SetValue(null, printStream);
			}

			public static void setIn(object inputStream)
			{
				TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical("java.lang.System");
				FieldWrapper fw = tw.GetFieldWrapper("in", "Ljava.io.InputStream;");
				fw.SetValue(null, inputStream);
			}

			public static void setOut(object printStream)
			{
				TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical("java.lang.System");
				FieldWrapper fw = tw.GetFieldWrapper("out", "Ljava.io.PrintStream;");
				fw.SetValue(null, printStream);
			}
		}

		public class VMClassLoader
		{
			public static Assembly findResourceAssembly(string name)
			{
				name = JVM.MangleResourceName(name);
				foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					if(!(asm is NetSystem.Reflection.Emit.AssemblyBuilder))
					{
						if(asm.GetManifestResourceInfo(name) != null)
						{
							return asm;
						}
					}
				}
				return null;
			}

			public static Assembly[] findResourceAssemblies(string name)
			{
				name = JVM.MangleResourceName(name);
				ArrayList list = new ArrayList();
				foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					if(!(asm is NetSystem.Reflection.Emit.AssemblyBuilder))
					{
						if(asm.GetManifestResourceInfo(name) != null)
						{
							list.Add(asm);
						}
					}
				}
				return (Assembly[])list.ToArray(typeof(Assembly));
			}

			public static object loadClass(string name, bool resolve)
			{
				try
				{
					TypeWrapper type = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
					if(type != null)
					{
						return VMClass.getClassFromWrapper(type);
					}
					return null;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object getBootstrapClassLoader()
			{
				return ClassLoaderWrapper.GetJavaBootstrapClassLoader();
			}

			public static object getPrimitiveClass(char type)
			{
				switch(type)
				{
					case 'Z':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.BOOLEAN);
					case 'B':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.BYTE);
					case 'C':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.CHAR);
					case 'D':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.DOUBLE);
					case 'F':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.FLOAT);
					case 'I':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.INT);
					case 'J':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.LONG);
					case 'S':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.SHORT);
					case 'V':
						return VMClass.getClassFromWrapper(PrimitiveTypeWrapper.VOID);
					default:
						throw new InvalidOperationException();
				}
			}

			public static object defineClass(object classLoader, string name, byte[] data, int offset, int length, object protectionDomain)
			{
				Profiler.Enter("ClassLoader.defineClass");
				try
				{
					try
					{
						ClassFile classFile = new ClassFile(data, offset, length, name, false);
						if(name != null && classFile.Name != name)
						{
							throw new NoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
						}
						lock(VMClass.LockObject)
						{
							TypeWrapper type = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader).DefineClass(classFile);
							try
							{
								return VMClass.CreateClassInstance(type, protectionDomain);
							}
							catch(Exception x)
							{
								// this is a critical failure, because otherwise we would open the window to a situation
								// where a Type exists without a Class and then if getClass() was called on an instance
								// of that type, a new Class would be created without the proper protectionDomain
								JVM.CriticalFailure("Unable to create Class", x);
								throw;
							}
						}
					}
					catch(RetargetableJavaException x)
					{
						throw x.ToJava();
					}
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

			internal static object LockObject
			{
				get
				{
					return map.SyncRoot;
				}
			}

			public static void throwException(Exception e)
			{
				throw e;
			}

			// NOTE when you call this method, you must own the VMClass.LockObject monitor
			internal static object CreateClassInstance(TypeWrapper wrapper, object protectionDomain)
			{
				object clazz = JVM.Library.newClass(wrapper, protectionDomain);
				map.Add(wrapper, clazz);
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
				if(typeWrapper.IsArray)
				{
					return getClassFromWrapper(typeWrapper.ElementTypeWrapper);
				}
				return null;
			}

			public static object loadArrayClass(string name, object classLoader)
			{
				try
				{
					ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader);
					TypeWrapper type = classLoaderWrapper.LoadClassByDottedName(name);
					return VMClass.getClassFromWrapper(type);
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			internal static TypeWrapper getWrapperFromClass(object clazz)
			{
				return (TypeWrapper)JVM.Library.getWrapperFromClass(clazz);
			}

			internal static object getClassFromWrapper(TypeWrapper wrapper)
			{
				NetSystem.Diagnostics.Debug.Assert(!wrapper.IsUnloadable);
				lock(VMClass.LockObject)
				{
					object clazz = map[wrapper];
					if(clazz == null)
					{
						clazz = CreateClassInstance(wrapper, null);
					}
					return clazz;
				}
			}

			internal static object getClassFromType(Type type)
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
	
			public static void initialize(object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				wrapper.Finish();
				wrapper.RunClassInit();
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
				Profiler.Enter("VMClass.GetDeclaredMethods");
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					// we need to look through the array for unloadable types, because we may not let them
					// escape into the 'wild'
					MethodWrapper[] methods = wrapper.GetMethods();
					ArrayList list = new ArrayList();
					for(int i = 0; i < methods.Length; i++)
					{
						methods[i].Link();
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
									// HACK we should have a better plan than this
									if(wrapper.GetClassLoader().LoadClassByDottedNameFast(methods[i].ReturnType.Name) == null)
									{
										throw JavaException.NoClassDefFoundError(methods[i].ReturnType.Name);
									}
								}
								TypeWrapper[] args = methods[i].GetParameters();
								for(int j = 0; j < args.Length; j++)
								{
									if(args[j].IsUnloadable)
									{
										// HACK we should have a better plan than this
										if(wrapper.GetClassLoader().LoadClassByDottedNameFast(args[j].Name) == null)
										{
											throw JavaException.NoClassDefFoundError(args[j].Name);
										}
									}
								}
								list.Add(methods[i]);
							}
						}
					}
					return (MethodWrapper[])list.ToArray(typeof(MethodWrapper));
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("VMClass.GetDeclaredMethods");
				}
			}

			public static object[] GetDeclaredFields(object cwrapper, bool publicOnly)
			{
				Profiler.Enter("VMClass.GetDeclaredFields");
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					// we need to finish the type otherwise all fields will not be in the field map yet
					wrapper.Finish();
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
					// we need to look through the array for unloadable types, because we may not let them
					// escape into the 'wild'
					for(int i = 0; i < fields.Length; i++)
					{
						if(fields[i].FieldTypeWrapper.IsUnloadable)
						{
							// HACK we should have a better plan than this
							if(wrapper.GetClassLoader().LoadClassByDottedNameFast(fields[i].FieldTypeWrapper.Name) == null)
							{
								throw JavaException.NoClassDefFoundError(fields[i].FieldTypeWrapper.Name);
							}
						}
					}
					return fields;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("VMClass.GetDeclaredFields");
				}
			}

			public static object[] GetDeclaredClasses(object cwrapper, bool publicOnly)
			{
				try
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
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object GetDeclaringClass(object cwrapper)
			{
				try
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
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object[] GetInterfaces(object cwrapper)
			{
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					TypeWrapper[] interfaceWrappers = wrapper.Interfaces;
					object[] interfaces = new object[interfaceWrappers.Length];
					for(int i = 0; i < interfaces.Length; i++)
					{
						interfaces[i] = getClassFromWrapper(interfaceWrappers[i]);
					}
					return interfaces;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static int GetModifiers(Object cwrapper)
			{
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					// NOTE we don't return the modifiers from the TypeWrapper, because for inner classes
					// the reflected modifiers are different from the physical ones
					Modifiers modifiers = wrapper.ReflectiveModifiers;
					// only returns public, protected, private, final, static, abstract and interface (as per
					// the documentation of Class.getModifiers())
					Modifiers mask = Modifiers.Public | Modifiers.Protected | Modifiers.Private | Modifiers.Final |
						Modifiers.Static | Modifiers.Abstract | Modifiers.Interface;
					return (int)(modifiers & mask);
				}				
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
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
				TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
				wrapper.Finish();
				Type type = wrapper.TypeAsTBD;
				if(!type.IsArray && type.TypeInitializer != null)
				{
					return !AttributeHelper.IsHideFromJava(type.TypeInitializer);
				}
				return false;
			}

			private static FieldWrapper GetFieldWrapperFromField(object field)
			{
				return (FieldWrapper)JVM.Library.getWrapperFromField(field);
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

			public static void setByteNative(object field, object obj, sbyte val)
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
				Profiler.Enter("ObjectInputStream.allocateObject");
				try
				{
					TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
					// if we're trying to deserialize a string as a TC_OBJECT, just return an emtpy string (Sun does the same)
					if(wrapper == CoreClasses.java.lang.String.Wrapper)
					{
						return "";
					}
					wrapper.Finish();
					// TODO do we need error handling? (e.g. when trying to instantiate an interface or abstract class)
					return NetSystem.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
				}
				finally
				{
					Profiler.Leave("ObjectInputStream.allocateObject");
				}
			}

			public static void callConstructor(object ois, object clazz, object obj)
			{
				Profiler.Enter("ObjectInputStream.callConstructor");
				try
				{
					TypeWrapper type = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
					// if we're trying to deserialize a string as a TC_OBJECT, we already have an initialized emtpy string
					// so there is no need to call the constructor (which wouldn't work anyway).
					if(!(obj is string))
					{
						MethodWrapper mw = type.GetMethodWrapper(new MethodDescriptor("<init>", "()V"), false);
						if(mw == null)
						{
							// TODO what should we do here?
							throw new NotImplementedException();
						}
						// TODO instead of calling link, we should probably Finish the wrapper
						mw.Link();
						// TODO what about exceptions? (should they be unwrapped?)
						mw.Invoke(obj, null, false);
					}
				}
				finally
				{
					Profiler.Leave("ObjectInputStream.callConstructor");
				}
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
					// FXBUG .NET framework bug
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

	namespace security
	{
		public class VMAccessController
		{
			public static object getClassFromFrame(NetSystem.Diagnostics.StackFrame frame)
			{
				return NativeCode.java.lang.VMClass.getClassFromType(frame.GetMethod().DeclaringType);
			}
		}
	}
}

namespace IKVM.NativeCode.gnu.java.net.protocol.ikvmres
{
	public class IkvmresURLConnection
	{
		public static string MangleResourceName(string name)
		{
			return JVM.MangleResourceName(name);
		}
	}
}

namespace IKVM.NativeCode.gnu.java.nio.channels
{
	public class FileChannelImpl
	{
		private static readonly MethodInfo mono_1_0_Flush;
		private static readonly MethodInfo mono_1_1_Flush;

		static FileChannelImpl()
		{
			try
			{
				FlushFileBuffers(new IntPtr(-1));
			}
			catch(TypeLoadException)
			{
				// If we end up here, we're not running on Windows, so we'll try two Mono specific methods.
				// The first one is using Mono.Posix, this is part of the documented public Mono API.
				Type t = Type.GetType("Mono.Posix.Syscall, Mono.Posix");
				if(t != null)
				{
					mono_1_1_Flush = t.GetMethod("fsync", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(int) }, new ParameterModifier[0]);
				}
				// HACK when we're running on Mono 1.0 we have to use an undocument internal method to flush the handle
				if(mono_1_1_Flush == null)
				{
					t = Type.GetType("System.IO.MonoIO");
					if(t != null)
					{
						mono_1_0_Flush = t.GetMethod("Flush", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(IntPtr), Type.GetType("System.IO.MonoIOError&") }, new ParameterModifier[0]);
					}
				}
			}
		}

		// NOTE this method implements a platform specific way to flush the underlying OS file buffers to disk
		public static bool flush(FileStream fs)
		{
			if(mono_1_0_Flush != null)
			{
				object[] args = new object[] { fs.Handle, null };
				mono_1_0_Flush.Invoke(null, args);
				return ((IConvertible)args[1]).ToInt32(null) == 0;
			}
			else if(mono_1_1_Flush != null)
			{
				object[] args = new object[] { fs.Handle.ToInt32() };
				return (int)mono_1_1_Flush.Invoke(null, args) == 0;
			}
			else
			{
				try
				{
					return FlushFileBuffers(fs.Handle);
				}
				catch(TypeLoadException)
				{
					// we're apparently running on a alternate runtime, so we'll just pretend that flush succeeded
					Tracer.Warning(Tracer.Runtime, "FlushFileBuffers/fsync not supported on this runtime");
					return true;
				}
			}
		}

		[DllImport("kernel32")]
		private extern static bool FlushFileBuffers(IntPtr handle);
	}
}

namespace gnu.classpath
{
	// This type lives here, because we don't want unverifiable code in IKVM.GNU.Classpath
	// (as that would prevents us from verifying it during the build process).
	public unsafe sealed class RawData
	{
		[HideFromJava]
		private sbyte* pb;

		public RawData(IntPtr p)
		{
			this.pb = (sbyte*)p;
		}

		public IntPtr p()
		{
			return new IntPtr(pb);
		}

		// NOTE the IKVM.Runtime doesn't have the AllowPartiallyTrustedCallersAttribute so this
		// security attribute isn't really needed, but to be extra safe we add the explicit link 
		// demand to these dangerous methods.
		[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public sbyte ReadByte(int index)
		{
			return pb[index];
		}

		[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void WriteByte(int index, sbyte b)
		{
			pb[index] = b;
		}

		[SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void MoveMemory(int dst_offset, int src_offset, int count)
		{
			if(dst_offset < src_offset)
			{
				while(count-- > 0)
					pb[dst_offset++] = pb[src_offset++];
			}
			else
			{
				dst_offset += count;
				src_offset += count;
				while(count-- > 0)
					pb[--dst_offset] = pb[--src_offset];
			}
		}
	}
}

namespace ikvm.@internal
{
	public interface LibraryVMInterface
	{
		object loadClass(object classLoader, string name);
		object newClass(object wrapper, object protectionDomain);
		object getWrapperFromClass(object clazz);
		object getWrapperFromField(object field);
		object getWrapperFromMethodOrConstructor(object methodOrConstructor);

		object getSystemClassLoader();

		object box(object val);
		object unbox(object val);

		Exception mapException(Exception t);
		void printStackTrace(Exception t);

		void jniWaitUntilLastThread();
		void jniDetach();

		object newConstructor(object clazz, object wrapper);
		object newMethod(object clazz, object wrapper);
		object newField(object clazz, object wrapper);

		object newDirectByteBuffer(IntPtr address, int capacity);
		IntPtr getDirectBufferAddress(object buffer);
		int getDirectBufferCapacity(object buffer);

		Exception newIllegalAccessError(string msg);
		Exception newIllegalAccessException(string msg);
		Exception newIncompatibleClassChangeError(string msg);
		Exception newLinkageError(string msg);
		Exception newVerifyError(string msg);
		Exception newClassCircularityError(string msg);
		Exception newClassFormatError(string msg);
		Exception newUnsupportedClassVersionError(string msg);
		Exception newNoClassDefFoundError(string msg);
		Exception newClassNotFoundException(string msg);
		Exception newUnsatisfiedLinkError(string msg);
		Exception newIllegalArgumentException(string msg);
		Exception newNegativeArraySizeException();
		Exception newArrayStoreException();
		Exception newIndexOutOfBoundsException(string msg);
		Exception newStringIndexOutOfBoundsException();
		Exception newInvocationTargetException(Exception t);
		Exception newUnknownHostException(string msg);
		Exception newArrayIndexOutOfBoundsException();
		Exception newNumberFormatException(string msg);
		Exception newNullPointerException();
		Exception newClassCastException(string msg);
		Exception newNoSuchFieldError(string msg);
		Exception newNoSuchMethodError(string msg);
		Exception newInstantiationError(string msg);
		Exception newInstantiationException(string msg);
		Exception newInterruptedException();
		Exception newIllegalMonitorStateException();
	}
}
