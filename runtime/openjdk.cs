/*
  Copyright (C) 2007 Jeroen Frijters

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
using StackFrame = System.Diagnostics.StackFrame;
using IKVM.Internal;
#if !FIRST_PASS
using jlClass = java.lang.Class;
using jlClassNotFoundException = java.lang.ClassNotFoundException;
using jlIllegalAccessException = java.lang.IllegalAccessException;
using jlIllegalArgumentException = java.lang.IllegalArgumentException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
using jlSecurityManager = java.lang.SecurityManager;
using jlStackTraceElement = java.lang.StackTraceElement;
using jlSystem = java.lang.System;
using jlRuntimePermission = java.lang.RuntimePermission;
using jlBoolean = java.lang.Boolean;
using jlByte = java.lang.Byte;
using jlShort = java.lang.Short;
using jlCharacter = java.lang.Character;
using jlInteger = java.lang.Integer;
using jlFloat = java.lang.Float;
using jlLong = java.lang.Long;
using jlDouble = java.lang.Double;
using jlVoid = java.lang.Void;
using jlNumber = java.lang.Number;
using jlrConstructor = java.lang.reflect.Constructor;
using jlrMethod = java.lang.reflect.Method;
using jlrField = java.lang.reflect.Field;
using jlrModifier = java.lang.reflect.Modifier;
using ProtectionDomain = java.security.ProtectionDomain;
using srMethodAccessor = sun.reflect.MethodAccessor;
using srConstantPool = sun.reflect.ConstantPool;
using srConstructorAccessor = sun.reflect.ConstructorAccessor;
using srFieldAccessor = sun.reflect.FieldAccessor;
using srLangReflectAccess = sun.reflect.LangReflectAccess;
using srReflection = sun.reflect.Reflection;
using srReflectionFactory = sun.reflect.ReflectionFactory;
using jnByteBuffer = java.nio.ByteBuffer;
using StubGenerator = ikvm.@internal.stubgen.StubGenerator;
using IConstantPoolWriter = ikvm.@internal.stubgen.StubGenerator.IConstantPoolWriter;
using Annotation = java.lang.annotation.Annotation;
#endif

namespace IKVM.NativeCode.java
{
	namespace lang
	{
#if !FIRST_PASS
		sealed class ConstantPoolWriter : IConstantPoolWriter
		{
			private ArrayList items = new ArrayList();

			public short AddUtf8(String str)
			{
				return (short)items.Add(str);
			}

			public short AddInt(int i)
			{
				return (short)items.Add(i);
			}

			public short AddLong(long l)
			{
				return (short)items.Add(l);
			}

			public short AddFloat(float f)
			{
				return (short)items.Add(f);
			}

			public short AddDouble(double d)
			{
				return (short)items.Add(d);
			}

			internal string GetUtf8(int index)
			{
				return (string)items[index];
			}

			internal int GetInt(int index)
			{
				return (int)items[index];
			}

			internal long GetLong(int index)
			{
				return (long)items[index];
			}

			internal float GetFloat(int index)
			{
				return (float)items[index];
			}

			internal double GetDouble(int index)
			{
				return (double)items[index];
			}
		}
#endif

		public sealed class Class
		{
			private Class() { }

			private static System.Reflection.FieldInfo signersField;
			private static System.Reflection.FieldInfo pdField;
			private static System.Reflection.FieldInfo constantPoolField;
			private static System.Reflection.FieldInfo constantPoolOopField;

			public static void registerNatives()
			{
#if !FIRST_PASS
				signersField = typeof(jlClass).GetField("signers", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				pdField = typeof(jlClass).GetField("pd", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				constantPoolField = typeof(jlClass).GetField("constantPool", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				constantPoolOopField = typeof(srConstantPool).GetField("constantPoolOop", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

				// HACK force LangHelper static initializer to run to register JavaLangAccess with SharedSecrets.
				System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(jlClass).Assembly.GetType("java.lang.LangHelper").TypeHandle);
#endif
			}

			public static object forName0(string name, bool initialize, object loader)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper tw = null;
				if (name.IndexOf(',') > 0)
				{
					// we essentially require full trust before allowing arbitrary types to be loaded,
					// hence we do the "createClassLoader" permission check
					jlSecurityManager sm = jlSystem.getSecurityManager();
					if (sm != null)
						sm.checkPermission(new jlRuntimePermission("createClassLoader"));
					Type type = Type.GetType(name);
					if (type != null)
					{
						tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
					}
					if (tw == null)
					{
						throw new jlClassNotFoundException(name);
					}
				}
				else
				{
					try
					{
						ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(loader);
						tw = classLoaderWrapper.LoadClassByDottedName(name);
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}
				if (initialize && !tw.IsArray)
				{
					tw.Finish();
					tw.RunClassInit();
				}
				return tw.ClassObject;
#endif
			}

			public static bool isInstance(object thisClass, object obj)
			{
				return obj != null && IKVM.NativeCode.ikvm.runtime.Util.GetTypeWrapperFromObject(obj).IsAssignableTo(TypeWrapper.FromClass(thisClass));
			}

			public static bool isAssignableFrom(object thisClass, object otherClass)
			{
				return TypeWrapper.FromClass(otherClass).IsAssignableTo(TypeWrapper.FromClass(thisClass));
			}

			public static bool isInterface(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsInterface;
			}

			public static bool isArray(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsArray;
			}

			public static bool isPrimitive(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).IsPrimitive;
			}

			public static string getName0(object thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				if (tw.IsPrimitive)
				{
					if (tw == PrimitiveTypeWrapper.BYTE)
					{
						return "byte";
					}
					else if (tw == PrimitiveTypeWrapper.CHAR)
					{
						return "char";
					}
					else if (tw == PrimitiveTypeWrapper.DOUBLE)
					{
						return "double";
					}
					else if (tw == PrimitiveTypeWrapper.FLOAT)
					{
						return "float";
					}
					else if (tw == PrimitiveTypeWrapper.INT)
					{
						return "int";
					}
					else if (tw == PrimitiveTypeWrapper.LONG)
					{
						return "long";
					}
					else if (tw == PrimitiveTypeWrapper.SHORT)
					{
						return "short";
					}
					else if (tw == PrimitiveTypeWrapper.BOOLEAN)
					{
						return "boolean";
					}
					else if (tw == PrimitiveTypeWrapper.VOID)
					{
						return "void";
					}
				}
				return tw.Name;
			}

			public static object getClassLoader0(object thisClass)
			{
				return TypeWrapper.FromClass(thisClass).GetClassLoader().GetJavaClassLoader();
			}

			public static object getSuperclass(object thisClass)
			{
				TypeWrapper super = TypeWrapper.FromClass(thisClass).BaseTypeWrapper;
				return super != null ? super.ClassObject : null;
			}

			public static object getInterfaces(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper[] ifaces = TypeWrapper.FromClass(thisClass).Interfaces;
				jlClass[] interfaces = new jlClass[ifaces.Length];
				for (int i = 0; i < ifaces.Length; i++)
				{
					interfaces[i] = (jlClass)ifaces[i].ClassObject;
				}
				return interfaces;
#endif
			}

			public static object getComponentType(object thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				return tw.IsArray ? tw.ElementTypeWrapper.ClassObject : null;
			}

			public static int getModifiers(object thisClass)
			{
				return (int)TypeWrapper.FromClass(thisClass).ReflectiveModifiers;
			}

			public static object[] getSigners(object thisClass)
			{
				return (object[])signersField.GetValue(thisClass);
			}

			public static void setSigners(object thisClass, object[] signers)
			{
				signersField.SetValue(thisClass, signers);
			}

			public static object[] getEnclosingMethod0(object thisClass)
			{
				TypeWrapper tw = TypeWrapper.FromClass(thisClass);
				string[] enc = tw.GetEnclosingMethod();
				if (enc == null)
				{
					return null;
				}
				try
				{
					return new object[] { tw.GetClassLoader().LoadClassByDottedName(enc[0]).ClassObject, enc[1], enc[2] == null ? null : enc[2].Replace('.', '/') };
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object getDeclaringClass(object thisClass)
			{
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					TypeWrapper decl = wrapper.DeclaringTypeWrapper;
					if (decl == null)
					{
						return null;
					}
					if (!decl.IsAccessibleFrom(wrapper))
					{
						throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", decl.Name, wrapper.Name));
					}
					decl.Finish();
					if (Array.IndexOf(decl.InnerClasses, wrapper) == -1)
					{
						throw new IncompatibleClassChangeError(string.Format("{0} and {1} disagree on InnerClasses attribute", decl.Name, wrapper.Name));
					}
					return decl.ClassObject;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object getProtectionDomain0(object thisClass)
			{
				TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
				while (wrapper.IsArray)
				{
					wrapper = wrapper.ElementTypeWrapper;
				}
				return pdField.GetValue(wrapper.ClassObject);
			}

			public static void setProtectionDomain0(object thisClass, object pd)
			{
				pdField.SetValue(thisClass, pd);
			}

			public static object getPrimitiveClass(string name)
			{
				switch (name)
				{
					case "byte":
						return PrimitiveTypeWrapper.BYTE.ClassObject;
					case "char":
						return PrimitiveTypeWrapper.CHAR.ClassObject;
					case "double":
						return PrimitiveTypeWrapper.DOUBLE.ClassObject;
					case "float":
						return PrimitiveTypeWrapper.FLOAT.ClassObject;
					case "int":
						return PrimitiveTypeWrapper.INT.ClassObject;
					case "long":
						return PrimitiveTypeWrapper.LONG.ClassObject;
					case "short":
						return PrimitiveTypeWrapper.SHORT.ClassObject;
					case "boolean":
						return PrimitiveTypeWrapper.BOOLEAN.ClassObject;
					case "void":
						return PrimitiveTypeWrapper.VOID.ClassObject;
					default:
						throw new ArgumentException(name);
				}
			}

			public static string getGenericSignature(object thisClass)
			{
				string sig = TypeWrapper.FromClass(thisClass).GetGenericSignature();
				if (sig == null)
				{
					return null;
				}
				return sig.Replace('.', '/');
			}

			public static byte[] getRawAnnotations(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
				wrapper.Finish();
				object[] objAnn = wrapper.GetDeclaredAnnotations();
				if (objAnn == null)
				{
					return null;
				}
				ArrayList ann = new ArrayList();
				foreach (object obj in objAnn)
				{
					if (obj is Annotation)
					{
						ann.Add(obj);
					}
				}
				IConstantPoolWriter cp = (IConstantPoolWriter)constantPoolOopField.GetValue(getConstantPool(thisClass));
				return StubGenerator.writeAnnotations(cp, (Annotation[])ann.ToArray(typeof(Annotation)));
#endif
			}

#if !FIRST_PASS
			internal static IConstantPoolWriter GetConstantPoolWriter(TypeWrapper wrapper)
			{
				return (IConstantPoolWriter)constantPoolOopField.GetValue(getConstantPool(wrapper.ClassObject));
			}
#endif

			public static object getConstantPool(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				lock (thisClass)
				{
					object cp = constantPoolField.GetValue(thisClass);
					if (cp == null)
					{
						cp = new srConstantPool();
						constantPoolOopField.SetValue(cp, new ConstantPoolWriter());
						constantPoolField.SetValue(thisClass, cp);
					}
					return cp;
				}
#endif
			}

			public static object getDeclaredFields0(object thisClass, bool publicOnly)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("Class.getDeclaredFields0");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					// we need to finish the type otherwise all fields will not be in the field map yet
					wrapper.Finish();
					FieldWrapper[] fields = wrapper.GetFields();
					ArrayList list = new ArrayList();
					for (int i = 0; i < fields.Length; i++)
					{
						if (fields[i].IsHideFromReflection)
						{
							// skip
						}
						else if (publicOnly && !fields[i].IsPublic)
						{
							// caller is only asking for public field, so we don't return this non-public field
						}
						else
						{
							fields[i].FieldTypeWrapper.EnsureLoadable(wrapper.GetClassLoader());
							list.Add(fields[i].ToField(false));
						}
					}
					return (jlrField[])list.ToArray(typeof(jlrField));
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("Class.getDeclaredFields0");
				}
#endif
			}

			public static object getDeclaredMethods0(object thisClass, bool publicOnly)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("Class.getDeclaredMethods0");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					wrapper.Finish();
					if (wrapper.HasVerifyError)
					{
						// TODO we should get the message from somewhere
						throw new VerifyError();
					}
					if (wrapper.HasClassFormatError)
					{
						// TODO we should get the message from somewhere
						throw new ClassFormatError(wrapper.Name);
					}
					// we need to look through the array for unloadable types, because we may not let them
					// escape into the 'wild'
					MethodWrapper[] methods = wrapper.GetMethods();
					ArrayList list = new ArrayList();
					for (int i = 0; i < methods.Length; i++)
					{
						// we don't want to expose "hideFromReflection" methods (one reason is that it would
						// mess up the serialVersionUID computation)
						if (!methods[i].IsHideFromReflection
							&& methods[i].Name != "<clinit>" && methods[i].Name != "<init>"
							&& (!publicOnly || methods[i].IsPublic))
						{
							methods[i].ReturnType.EnsureLoadable(wrapper.GetClassLoader());
							TypeWrapper[] args = methods[i].GetParameters();
							for (int j = 0; j < args.Length; j++)
							{
								args[j].EnsureLoadable(wrapper.GetClassLoader());
							}
							list.Add(methods[i].ToMethodOrConstructor(false));
						}
					}
					return (jlrMethod[])list.ToArray(typeof(jlrMethod));
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("Class.getDeclaredMethods0");
				}
#endif
			}

			public static object getDeclaredConstructors0(object thisClass, bool publicOnly)
			{
#if FIRST_PASS
				return null;
#else
				Profiler.Enter("Class.getDeclaredConstructors0");
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					wrapper.Finish();
					if (wrapper.HasVerifyError)
					{
						// TODO we should get the message from somewhere
						throw new VerifyError();
					}
					if (wrapper.HasClassFormatError)
					{
						// TODO we should get the message from somewhere
						throw new ClassFormatError(wrapper.Name);
					}
					// we need to look through the array for unloadable types, because we may not let them
					// escape into the 'wild'
					MethodWrapper[] methods = wrapper.GetMethods();
					ArrayList list = new ArrayList();
					for (int i = 0; i < methods.Length; i++)
					{
						// we don't want to expose "hideFromReflection" methods (one reason is that it would
						// mess up the serialVersionUID computation)
						if (!methods[i].IsHideFromReflection)
						{
							if (methods[i].Name == "<init>")
							{
								TypeWrapper[] args = methods[i].GetParameters();
								for (int j = 0; j < args.Length; j++)
								{
									args[j].EnsureLoadable(wrapper.GetClassLoader());
								}
								list.Add(methods[i].ToMethodOrConstructor(false));
							}
						}
					}
					return (jlrConstructor[])list.ToArray(typeof(jlrConstructor));
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				finally
				{
					Profiler.Leave("Class.getDeclaredConstructors0");
				}
#endif
			}

			public static object getDeclaredClasses0(object thisClass)
			{
#if FIRST_PASS
				return null;
#else
				try
				{
					TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
					// NOTE to get at the InnerClasses we need to finish the type
					wrapper.Finish();
					TypeWrapper[] wrappers = wrapper.InnerClasses;
					jlClass[] innerclasses = new jlClass[wrappers.Length];
					for (int i = 0; i < innerclasses.Length; i++)
					{
						wrappers[i].Finish();
						if (wrappers[i].IsUnloadable)
						{
							throw new jlNoClassDefFoundError(wrappers[i].Name);
						}
						if (!wrappers[i].IsAccessibleFrom(wrapper))
						{
							throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", wrappers[i].Name, wrapper.Name));
						}
						innerclasses[i] = (jlClass)wrappers[i].ClassObject;
					}
					return innerclasses;
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
#endif
			}

			public static bool desiredAssertionStatus0(object clazz)
			{
				// TODO
				return false;
			}
		}

		public sealed class ClassLoader
		{
#if !FIRST_PASS
			private static jlClassNotFoundException classNotFoundException;
#endif

			private ClassLoader() { }

			public static void registerNatives()
			{
			}

			public static object defineClass0(object thisClassLoader, string name, byte[] b, int off, int len, object pd)
			{
				return defineClass1(thisClassLoader, name, b, off, len, pd, null);
			}

			public static object defineClass1(object thisClassLoader, string name, byte[] b, int off, int len, object pd, string source)
			{
				// it appears the source argument is only used for trace messages in HotSpot. We'll just ignore it for now.
				Profiler.Enter("ClassLoader.defineClass");
				try
				{
					try
					{
						ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
						ClassFileParseOptions cfp = ClassFileParseOptions.LineNumberTable;
						if (classLoaderWrapper.EmitDebugInfo)
						{
							cfp |= ClassFileParseOptions.LocalVariableTable;
						}
						ClassFile classFile = new ClassFile(b, off, len, name, cfp);
						if (name != null && classFile.Name != name)
						{
#if !FIRST_PASS
							throw new jlNoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
#endif
						}
						TypeWrapper type = classLoaderWrapper.DefineClass(classFile, pd);
						return type.ClassObject;
					}
					catch (RetargetableJavaException x)
					{
						throw x.ToJava();
					}
				}
				finally
				{
					Profiler.Leave("ClassLoader.defineClass");
				}
			}

			public static object defineClass2(object thisClassLoader, string name, object b, int off, int len, object pd, string source)
			{
#if FIRST_PASS
				return null;
#else
				jnByteBuffer bb = (jnByteBuffer)b;
				byte[] buf = new byte[bb.remaining()];
				bb.get(buf);
				return defineClass1(thisClassLoader, name, buf, 0, buf.Length, pd, source);
#endif
			}

			public static void resolveClass0(object thisClassLoader, object clazz)
			{
				// no-op
			}

			public static object findBootstrapClass(object thisClassLoader, string name)
			{
#if FIRST_PASS
				return null;
#else
				TypeWrapper tw;
				try
				{
					tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
				}
				catch (RetargetableJavaException x)
				{
					throw x.ToJava();
				}
				if (tw == null)
				{
					// HACK for efficiency, we don't allocate a new exception here
					// (as this exception is thrown for *every* non-boot class that we load and
					// the exception is thrown away by our caller anyway)
					if (classNotFoundException == null)
					{
						jlClassNotFoundException ex = new jlClassNotFoundException(null, null);
						ex.setStackTrace(new jlStackTraceElement[] { new jlStackTraceElement("java.lang.ClassLoader", "findBootstrapClass", null, -2) });
						classNotFoundException = ex;
					}
					throw classNotFoundException;
				}
				return tw.ClassObject;
#endif
			}

			public static object findLoadedClass0(object thisClassLoader, string name)
			{
				ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
				TypeWrapper tw = loader.GetLoadedClass(name);
				return tw != null ? tw.ClassObject : null;
			}

			public sealed class NativeLibrary
			{
				private NativeLibrary() { }

				public static void load(object thisNativeLibrary, string name)
				{
					// TODO
					throw new NotImplementedException();
				}

				public static long find(object thisNativeLibrary, string name)
				{
					// TODO
					throw new NotImplementedException();
				}

				public static void unload(object thisNativeLibrary)
				{
					// TODO
					throw new NotImplementedException();
				}
			}

			public static object retrieveDirectives()
			{
				// TODO
				throw new NotImplementedException();
			}
		}

		public sealed class Package
		{
			private Package() { }

			public static string getSystemPackage0(string name)
			{
				// this method is not implemented because we redirect Package.getSystemPackage() to our implementation in LangHelper
				throw new NotImplementedException();
			}

			public static string[] getSystemPackages0()
			{
				// this method is not implemented because we redirect Package.getSystemPackages() to our implementation in LangHelper
				throw new NotImplementedException();
			}
		}

		namespace reflect
		{
			public sealed class Proxy
			{
				private Proxy() { }

				public static object defineClass0(object classLoader, string name, byte[] b, int off, int len)
				{
					return ClassLoader.defineClass1(classLoader, name, b, off, len, null, null);
				}
			}
		}
	}
}

namespace IKVM.NativeCode.sun.misc
{
	public sealed class VM
	{
		private VM() { }

		public static void getThreadStateValues(int[][] vmThreadStateValues, string[][] vmThreadStateNames)
		{
			// TODO
		}

		public static void initialize()
		{
		}
	}
}

namespace IKVM.NativeCode.sun.reflect
{
	public sealed class Reflection
	{
		private Reflection() { }

		// this method will return null for global methods on .NET 1.1
		// (.NET 1.1 doesn't have a way to go from MethodBase to Module or Assembly)
		private static System.Reflection.Assembly GetAssemblyFromMethodBase(System.Reflection.MethodBase mb)
		{
#if WHIDBEY
			return mb.Module.Assembly;
#else
			Type type = mb.DeclaringType;
			return type == null ? null : type.Assembly;
#endif
		}

		public static object getCallerClass(int realFramesToSkip)
		{
#if FIRST_PASS
			return null;
#else
			// HACK compensate for not inlining (or no tail-call optimization) of the native method stub
			if (new StackFrame(1, false).GetMethod().DeclaringType == typeof(srReflection))
			{
				realFramesToSkip++;
			}
			StackFrame frame;
			// skip reflection frames
			while (GetAssemblyFromMethodBase((frame = new StackFrame(realFramesToSkip, false)).GetMethod()) == typeof(object).Assembly)
			{
				for (;;)
				{
					Type type = new StackFrame(realFramesToSkip++, false).GetMethod().DeclaringType;
					if (type == typeof(jlrMethod) || type == typeof(jlrConstructor))
					{
						break;
					}
					if (type == typeof(IKVM.Runtime.JNIEnv))
					{
						while (new StackFrame(realFramesToSkip, false).GetMethod().DeclaringType == typeof(IKVM.Runtime.JNIEnv))
						{
							realFramesToSkip++;
						}
						break;
					}
				}
			}
			return ClassLoaderWrapper.GetWrapperFromType(frame.GetMethod().DeclaringType).ClassObject;
#endif
		}

		public static int getClassAccessFlags(object clazz)
		{
			return (int)TypeWrapper.FromClass(clazz).Modifiers;
		}

		public static bool checkInternalAccess(object currentClass, object memberClass)
		{
			TypeWrapper current = TypeWrapper.FromClass(currentClass);
			TypeWrapper member = TypeWrapper.FromClass(memberClass);
			return member.IsInternal && member.GetClassLoader() == current.GetClassLoader();
		}
	}

	public sealed class ReflectionFactory
	{
		private ReflectionFactory() { }

#if !FIRST_PASS
		private static object[] ConvertArgs(TypeWrapper[] argumentTypes, object[] args)
		{
			object[] nargs = new object[args == null ? 0 : args.Length];
			for (int i = 0; i < nargs.Length; i++)
			{
				if (argumentTypes[i].IsPrimitive)
				{
					if (args[i] == null)
					{
						throw new jlIllegalArgumentException("primitive wrapper null");
					}
					nargs[i] = JVM.Library.unbox(args[i]);
					// NOTE we depend on the fact that the .NET reflection parameter type
					// widening rules are the same as in Java, but to have this work for byte
					// we need to convert byte to sbyte.
					if (nargs[i] is byte && argumentTypes[i] != PrimitiveTypeWrapper.BYTE)
					{
						nargs[i] = (sbyte)(byte)nargs[i];
					}
				}
				else
				{
					nargs[i] = args[i];
				}
			}
			return nargs;
		}

		private sealed class MethodAccessorImpl : srMethodAccessor
		{
			private readonly MethodWrapper mw;

			internal MethodAccessorImpl(jlrMethod method)
			{
				mw = MethodWrapper.FromMethodOrConstructor(method);
			}

			[IKVM.Attributes.HideFromJava]
			public object invoke(object obj, object[] args)
			{
				args = ConvertArgs(mw.GetParameters(), args);
				object retval;
				try
				{
					retval = mw.Invoke(obj, args, false);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
				if (mw.ReturnType.IsPrimitive && mw.ReturnType != PrimitiveTypeWrapper.VOID)
				{
					retval = JVM.Library.box(retval);
				}
				return retval;
			}
		}

		private sealed class ConstructorAccessorImpl : srConstructorAccessor
		{
			private readonly MethodWrapper mw;

			internal ConstructorAccessorImpl(jlrConstructor constructor)
			{
				mw = MethodWrapper.FromMethodOrConstructor(constructor);
			}

			[IKVM.Attributes.HideFromJava]
			public object newInstance(object[] args)
			{
				args = ConvertArgs(mw.GetParameters(), args);
				try
				{
					return mw.Invoke(null, args, false);
				}
				catch (MethodAccessException x)
				{
					// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
					throw new jlIllegalAccessException().initCause(x);
				}
			}
		}

		private abstract class FieldAccessorImplBase : srFieldAccessor
		{
			private readonly FieldWrapper fw;
			private readonly bool isFinal;

			private FieldAccessorImplBase(jlrField field, bool overrideAccessCheck)
			{
				fw = FieldWrapper.FromField(field);
				isFinal = (!overrideAccessCheck || fw.IsStatic) && fw.IsFinal;
			}

			public virtual bool getBoolean(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual byte getByte(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual char getChar(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual short getShort(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual int getInt(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual long getLong(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual float getFloat(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual double getDouble(object obj)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setBoolean(object obj, bool z)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setByte(object obj, byte b)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setChar(object obj, char c)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setShort(object obj, short s)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setInt(object obj, int i)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setLong(object obj, long l)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setFloat(object obj, float f)
			{
				throw new jlIllegalArgumentException();
			}

			public virtual void setDouble(object obj, double d)
			{
				throw new jlIllegalArgumentException();
			}

			public abstract object get(object obj);
			public abstract void set(object obj, object value);
			
			private sealed class ObjectField : FieldAccessorImplBase
			{
				private readonly jlClass fieldType;

				internal ObjectField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
					fieldType = field.getType();
				}

				public override object get(object obj)
				{
					return fw.GetValue(obj);
				}

				public override void set(object obj, object value)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					if (value != null && !fieldType.isInstance(value))
					{
						throw new jlIllegalArgumentException();
					}
					fw.SetValue(obj, value);
				}
			}

			private sealed class ByteField : FieldAccessorImplBase
			{
				internal ByteField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override byte getByte(object obj)
				{
					return (byte)fw.GetValue(obj);
				}

				public override short getShort(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override int getInt(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override long getLong(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override float getFloat(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override double getDouble(object obj)
				{
					return (sbyte)getByte(obj);
				}

				public override object get(object obj)
				{
					return jlByte.valueOf(getByte(obj));
				}

				public override void set(object obj, object val)
				{
					if (!(val is jlByte))
					{
						throw new jlIllegalArgumentException();
					}
					setByte(obj, ((jlByte)val).byteValue());
				}

				public override void setByte(object obj, byte b)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, b);
				}
			}

			private sealed class BooleanField : FieldAccessorImplBase
			{
				internal BooleanField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override bool getBoolean(object obj)
				{
					return (bool)fw.GetValue(obj);
				}

				public override object get(object obj)
				{
					return jlBoolean.valueOf(getBoolean(obj));
				}

				public override void set(object obj, object val)
				{
					if (!(val is jlBoolean))
					{
						throw new jlIllegalArgumentException();
					}
					setBoolean(obj, ((jlBoolean)val).booleanValue());
				}

				public override void setBoolean(object obj, bool b)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, b);
				}
			}

			private sealed class CharField : FieldAccessorImplBase
			{
				internal CharField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override char getChar(object obj)
				{
					return (char)fw.GetValue(obj);
				}

				public override int getInt(object obj)
				{
					return getChar(obj);
				}

				public override long getLong(object obj)
				{
					return getChar(obj);
				}

				public override float getFloat(object obj)
				{
					return getChar(obj);
				}

				public override double getDouble(object obj)
				{
					return getChar(obj);
				}

				public override object get(object obj)
				{
					return jlCharacter.valueOf(getChar(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlCharacter)
						setChar(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setChar(object obj, char c)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, c);
				}
			}

			private sealed class ShortField : FieldAccessorImplBase
			{
				internal ShortField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override short getShort(object obj)
				{
					return (short)fw.GetValue(obj);
				}

				public override int getInt(object obj)
				{
					return getShort(obj);
				}

				public override long getLong(object obj)
				{
					return getShort(obj);
				}

				public override float getFloat(object obj)
				{
					return getShort(obj);
				}

				public override double getDouble(object obj)
				{
					return getShort(obj);
				}

				public override object get(object obj)
				{
					return jlShort.valueOf(getShort(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlByte
						|| val is jlShort)
						setShort(obj, ((jlNumber)val).shortValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setShort(obj, (sbyte)b);
				}

				public override void setShort(object obj, short s)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, s);
				}
			}

			private sealed class IntField : FieldAccessorImplBase
			{
				internal IntField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override int getInt(object obj)
				{
					return (int)fw.GetValue(obj);
				}

				public override long getLong(object obj)
				{
					return getInt(obj);
				}

				public override float getFloat(object obj)
				{
					return getInt(obj);
				}

				public override double getDouble(object obj)
				{
					return getInt(obj);
				}

				public override object get(object obj)
				{
					return jlInteger.valueOf(getInt(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlByte
						|| val is jlShort
						|| val is jlInteger)
						setInt(obj, ((jlNumber)val).intValue());
					else if (val is jlCharacter)
						setInt(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setInt(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setInt(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setInt(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, i);
				}
			}

			private sealed class FloatField : FieldAccessorImplBase
			{
				internal FloatField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override float getFloat(object obj)
				{
					return (float)fw.GetValue(obj);
				}

				public override double getDouble(object obj)
				{
					return getFloat(obj);
				}

				public override object get(object obj)
				{
					return jlFloat.valueOf(getFloat(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlFloat
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger
						|| val is jlLong)
						setFloat(obj, ((jlNumber)val).floatValue());
					else if (val is jlCharacter)
						setFloat(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setFloat(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setFloat(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setFloat(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setFloat(obj, i);
				}

				public override void setLong(object obj, long l)
				{
					setFloat(obj, l);
				}

				public override void setFloat(object obj, float f)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, f);
				}
			}

			private sealed class LongField : FieldAccessorImplBase
			{
				internal LongField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override long getLong(object obj)
				{
					return (long)fw.GetValue(obj);
				}

				public override float getFloat(object obj)
				{
					return getLong(obj);
				}

				public override double getDouble(object obj)
				{
					return getLong(obj);
				}

				public override object get(object obj)
				{
					return jlLong.valueOf(getLong(obj));
				}

				public override void setLong(object obj, long l)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, l);
				}

				public override void set(object obj, object val)
				{
					if (val is jlLong
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger)
						setLong(obj, ((jlNumber)val).longValue());
					else if (val is jlCharacter)
						setLong(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setLong(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setLong(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setLong(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setLong(obj, i);
				}
			}

			private sealed class DoubleField : FieldAccessorImplBase
			{
				internal DoubleField(jlrField field, bool overrideAccessCheck)
					: base(field, overrideAccessCheck)
				{
				}

				public override double getDouble(object obj)
				{
					return (double)fw.GetValue(obj);
				}

				public override object get(object obj)
				{
					return jlDouble.valueOf(getDouble(obj));
				}

				public override void set(object obj, object val)
				{
					if (val is jlDouble
						|| val is jlFloat
						|| val is jlByte
						|| val is jlShort
						|| val is jlInteger
						|| val is jlLong)
						setDouble(obj, ((jlNumber)val).doubleValue());
					else if (val is jlCharacter)
						setDouble(obj, ((jlCharacter)val).charValue());
					else
						throw new jlIllegalArgumentException();
				}

				public override void setByte(object obj, byte b)
				{
					setDouble(obj, (sbyte)b);
				}

				public override void setChar(object obj, char c)
				{
					setDouble(obj, c);
				}

				public override void setShort(object obj, short s)
				{
					setDouble(obj, s);
				}

				public override void setInt(object obj, int i)
				{
					setDouble(obj, i);
				}

				public override void setLong(object obj, long l)
				{
					setDouble(obj, l);
				}

				public override void setFloat(object obj, float f)
				{
					setDouble(obj, f);
				}

				public override void setDouble(object obj, double d)
				{
					if (isFinal)
					{
						throw new jlIllegalAccessException();
					}
					fw.SetValue(obj, d);
				}
			}

			internal static FieldAccessorImplBase Create(jlrField field, bool overrideAccessCheck)
			{
				jlClass type = field.getType();
				if (type.isPrimitive())
				{
					if (type == jlByte.TYPE)
					{
						return new ByteField(field, overrideAccessCheck);
					}
					if (type == jlBoolean.TYPE)
					{
						return new BooleanField(field, overrideAccessCheck);
					}
					if (type == jlCharacter.TYPE)
					{
						return new CharField(field, overrideAccessCheck);
					}
					if (type == jlShort.TYPE)
					{
						return new ShortField(field, overrideAccessCheck);
					}
					if (type == jlInteger.TYPE)
					{
						return new IntField(field, overrideAccessCheck);
					}
					if (type == jlFloat.TYPE)
					{
						return new FloatField(field, overrideAccessCheck);
					}
					if (type == jlLong.TYPE)
					{
						return new LongField(field, overrideAccessCheck);
					}
					if (type == jlDouble.TYPE)
					{
						return new DoubleField(field, overrideAccessCheck);
					}
					throw new InvalidOperationException("field type: " + type);
				}
				else
				{
					return new ObjectField(field, overrideAccessCheck);
				}
			}
		}
#endif

		public static object newFieldAccessor(object thisFactory, object field, bool overrideAccessCheck)
		{
#if FIRST_PASS
			return null;
#else
			return FieldAccessorImplBase.Create((jlrField)field, overrideAccessCheck);
#endif
		}

		public static object newMethodAccessor(object thisFactory, object method)
		{
#if FIRST_PASS
			return null;
#else
			return new MethodAccessorImpl((jlrMethod)method);
#endif
		}

		public static object newConstructorAccessor0(object thisFactory, object constructor)
		{
#if FIRST_PASS
			return null;
#else
			return new ConstructorAccessorImpl((jlrConstructor)constructor);
#endif
		}

		public static object newConstructorAccessorForSerialization(object classToInstantiate, object constructorToCall)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class ConstantPool
	{
		private ConstantPool() { }

		public static int getSize0(object thisConstantPool, object constantPoolOop)
		{
			throw new NotImplementedException();
		}

		public static object getClassAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getClassAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getMethodAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getMethodAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getFieldAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static object getFieldAtIfLoaded0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static string[] getMemberRefInfoAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static int getIntAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetInt(index);
#endif
		}

		public static long getLongAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetLong(index);
#endif
		}

		public static float getFloatAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetFloat(index);
#endif
		}

		public static double getDoubleAt0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return 0;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetDouble(index);
#endif
		}

		public static string getStringAt0(object thisConstantPool, object constantPoolOop, int index)
		{
			throw new NotImplementedException();
		}

		public static string getUTF8At0(object thisConstantPool, object constantPoolOop, int index)
		{
#if FIRST_PASS
			return null;
#else
			return ((IKVM.NativeCode.java.lang.ConstantPoolWriter)constantPoolOop).GetUtf8(index);
#endif
		}
	}
}
