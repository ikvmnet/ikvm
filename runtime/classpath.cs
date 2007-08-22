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
#if !OPENJDK
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;
#if !FIRST_PASS
using NegativeArraySizeException = java.lang.NegativeArraySizeException;
using IllegalArgumentException = java.lang.IllegalArgumentException;
using IllegalAccessException = java.lang.IllegalAccessException;
using NumberFormatException = java.lang.NumberFormatException;
using jlNoClassDefFoundError = java.lang.NoClassDefFoundError;
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
							return System.Array.CreateInstance(wrapper.TypeAsArrayType, dim);
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

				public static int GetRealModifiers(object clazz)
				{
					return (int)TypeWrapper.FromClass(clazz).Modifiers;
				}
				
				public static object GetReturnType(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					TypeWrapper retType = wrapper.ReturnType;
					retType = retType.EnsureLoadable(wrapper.DeclaringType.GetClassLoader());
					return retType.ClassObject;
				}

				public static object[] GetParameterTypes(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					TypeWrapper[] parameters = wrapper.GetParameters();
					object[] parameterClasses = new object[parameters.Length];
					for(int i = 0; i < parameters.Length; i++)
					{
						TypeWrapper paramType = parameters[i].EnsureLoadable(wrapper.DeclaringType.GetClassLoader());
						parameterClasses[i] = paramType.ClassObject;
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

				public static string GetSignature(object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					return wrapper.DeclaringType.GetGenericMethodSignature(wrapper);
				}

				public static object GetDefaultValue(Object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					return wrapper.DeclaringType.GetAnnotationDefault(wrapper);
				}

				public static object[] GetDeclaredAnnotations(Object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					wrapper.DeclaringType.Finish();
					return wrapper.DeclaringType.GetMethodAnnotations(wrapper);
				}

				public static object[][] GetParameterAnnotations(Object methodCookie)
				{
					MethodWrapper wrapper = (MethodWrapper)methodCookie;
					wrapper.DeclaringType.Finish();
					object[][] annotations = wrapper.DeclaringType.GetParameterAnnotations(wrapper);
					if(annotations == null)
					{
						annotations = new object[wrapper.GetParameters().Length][];
					}
					return annotations;
				}

				[HideFromJava]
				public static object Invoke(object methodCookie, object o, object[] args)
				{
#if !FIRST_PASS
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
									throw new IllegalArgumentException("primitive wrapper null");
								}
								argsCopy[i] = JVM.Library.unbox(args[i]);
								// NOTE we depend on the fact that the .NET reflection parameter type
								// widening rules are the same as in Java, but to have this work for byte
								// we need to convert byte to sbyte.
								if(argsCopy[i] is byte && argWrappers[i] != PrimitiveTypeWrapper.BYTE)
								{
									argsCopy[i] = (sbyte)(byte)argsCopy[i];
								}
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
						object retval;
						try
						{
							retval = mw.Invoke(o, argsCopy, false);
						}
						catch(MethodAccessException x)
						{
							// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
							throw new IllegalAccessException().initCause(x);
						}
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
#else
					return null;
#endif
				}
			}

			public class VMFieldImpl
			{
				public static string GetName(object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					return wrapper.Name;
				}

				public static int GetModifiers(object memberCookie)
				{
					MemberWrapper wrapper = (MemberWrapper)memberCookie;
					return (int)wrapper.Modifiers;
				}

				public static object GetDeclaringClass(object memberCookie)
				{
					MemberWrapper wrapper = (MemberWrapper)memberCookie;
					return wrapper.DeclaringType.ClassObject;
				}

				public static object GetFieldType(object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					TypeWrapper fieldType = wrapper.FieldTypeWrapper;
					fieldType = fieldType.EnsureLoadable(wrapper.DeclaringType.GetClassLoader());
					return fieldType.ClassObject;
				}

				public static string GetSignature(object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					return wrapper.DeclaringType.GetGenericFieldSignature(wrapper);
				}

				public static object[] GetDeclaredAnnotations(Object fieldCookie)
				{
					FieldWrapper wrapper = (FieldWrapper)fieldCookie;
					wrapper.DeclaringType.Finish();
					return wrapper.DeclaringType.GetFieldAnnotations(wrapper);
				}

				public static void RunClassInit(object clazz)
				{
					TypeWrapper.FromClass(clazz).RunClassInit();
				}

				public static bool CheckAccess(object memberCookie, object instance, object callerClass)
				{
					MemberWrapper member = (MemberWrapper)memberCookie;
					if(callerClass != null)
					{
						TypeWrapper instanceTypeWrapper;
						if(member.IsStatic || instance == null)
						{
							instanceTypeWrapper = member.DeclaringType;
						}
						else
						{
							instanceTypeWrapper = IKVM.NativeCode.ikvm.runtime.Util.GetTypeWrapperFromObject(instance);
						}
						return member.IsAccessibleFrom(member.DeclaringType, TypeWrapper.FromClass(callerClass), instanceTypeWrapper);
					}
					else
					{
						return member.IsPublic && member.DeclaringType.IsPublic;
					}
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
						try
						{
							return wrapper.GetValue(o);
						}
#if !COMPACT_FRAMEWORK && !FIRST_PASS
						catch(FieldAccessException x)
						{
							// this can happen if we're accessing a non-public field and the call stack doesn't have ReflectionPermission.MemberAccess
							throw new IllegalAccessException().initCause(x);
						}
#endif
						finally
						{
						}
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
						// if the field is an interface field, we must explicitly run <clinit>,
						// because .NET reflection doesn't
						if(wrapper.DeclaringType.IsInterface)
						{
							wrapper.DeclaringType.RunClassInit();
						}
						try
						{
							wrapper.SetValue(o, v);
						}
#if !COMPACT_FRAMEWORK && !FIRST_PASS
						catch(FieldAccessException x)
						{
							// this can happen if we're accessing a non-public field and the call stack doesn't have ReflectionPermission.MemberAccess
							throw new IllegalAccessException().initCause(x);
						}
#endif
						finally
						{
						}
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
			public static object loadClass(string name, bool resolve)
			{
				try
				{
					TypeWrapper type = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
					if(type != null)
					{
						return type.ClassObject;
					}
					return null;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

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

			public static object defineClassImpl(object classLoader, string name, byte[] data, int offset, int length, object protectionDomain)
			{
				Profiler.Enter("ClassLoader.defineClass");
				try
				{
					try
					{
						ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader);
						ClassFileParseOptions cfp = ClassFileParseOptions.LineNumberTable;
						if(classLoaderWrapper.EmitDebugInfo)
						{
							cfp |= ClassFileParseOptions.LocalVariableTable;
						}
						ClassFile classFile = new ClassFile(data, offset, length, name, cfp);
						if(name != null && classFile.Name != name)
						{
#if !FIRST_PASS
							throw new jlNoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
#endif
						}
						TypeWrapper type = classLoaderWrapper.DefineClass(classFile, protectionDomain);
						return type.ClassObject;
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

			public static object findLoadedClass(object javaClassLoader, string name)
			{
				ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderWrapper(javaClassLoader);
				TypeWrapper wrapper = loader.GetLoadedClass(name);
				if(wrapper != null)
				{
					return wrapper.ClassObject;
				}
				return null;
			}

			public static object getAssemblyClassLoader(Assembly asm)
			{
				return ClassLoaderWrapper.GetAssemblyClassLoader(asm).GetJavaClassLoader();
			}
		}

		public class VMClass
		{
			public static void throwException(Exception e)
			{
				throw e;
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
					return baseWrapper.ClassObject;
				}
				return null;
			}

			public static object getComponentClassFromWrapper(object wrapper)
			{
				TypeWrapper typeWrapper = (TypeWrapper)wrapper;
				if(typeWrapper.IsArray)
				{
					return typeWrapper.ElementTypeWrapper.ClassObject;
				}
				return null;
			}

			public static object forName0(string name, bool initialize, object classLoader)
			{
				try
				{
					ClassLoaderWrapper classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader);
					TypeWrapper type = classLoaderWrapper.LoadClassByDottedName(name);
					if(initialize && !type.IsArray)
					{
						type.Finish();
						type.RunClassInit();
					}
					return type.ClassObject;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object getClassFromType(Type type)
			{
				return IKVM.NativeCode.ikvm.runtime.Util.getClassFromTypeHandle(type.TypeHandle);
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
	
			public static object getClassLoader0(object wrapper)
			{
				TypeWrapper tw = (TypeWrapper)wrapper;
				return tw.GetClassLoader().GetJavaClassLoader();
			}

			public static object[] GetDeclaredMethods(object cwrapper, bool getMethods, bool publicOnly)
			{
				Profiler.Enter("VMClass.GetDeclaredMethods");
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					wrapper.Finish();
					if(wrapper.HasVerifyError)
					{
						// TODO we should get the message from somewhere
						throw new VerifyError();
					}
					if(wrapper.HasClassFormatError)
					{
						// TODO we should get the message from somewhere
						throw new ClassFormatError(wrapper.Name);
					}
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
								if(!JVM.EnableReflectionOnMethodsWithUnloadableTypeParameters)
								{
									methods[i].ReturnType.EnsureLoadable(wrapper.GetClassLoader());
									TypeWrapper[] args = methods[i].GetParameters();
									for(int j = 0; j < args.Length; j++)
									{
										args[j].EnsureLoadable(wrapper.GetClassLoader());
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
					ArrayList list = new ArrayList();
					for(int i = 0; i < fields.Length; i++)
					{
						if(fields[i].IsHideFromReflection)
						{
							// skip
						}
						else if(publicOnly && !fields[i].IsPublic)
						{
							// caller is only asking for public field, so we don't return this non-public field
						}
						else
						{
							fields[i].FieldTypeWrapper.EnsureLoadable(wrapper.GetClassLoader());
							list.Add(fields[i]);
						}
					}
					return (FieldWrapper[])list.ToArray(typeof(FieldWrapper));
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
#if !FIRST_PASS
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
								throw new jlNoClassDefFoundError(wrappers[i].Name);
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
							throw new jlNoClassDefFoundError(wrappers[i].Name);
						}
						if(!wrappers[i].IsAccessibleFrom(wrapper))
						{
							throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", wrappers[i].Name, wrapper.Name));
						}
						innerclasses[i] = wrappers[i].ClassObject;
					}
					return innerclasses;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
#else
				return null;
#endif
			}

			public static object GetDeclaringClass(object cwrapper)
			{
#if !FIRST_PASS
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
						throw new jlNoClassDefFoundError(declaring.Name);
					}
					if(!declaring.IsAccessibleFrom(wrapper))
					{
						throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", declaring.Name, wrapper.Name));
					}
					declaring.Finish();
					foreach(TypeWrapper tw in declaring.InnerClasses)
					{
						if(tw == wrapper)
						{
							return declaring.ClassObject;
						}
					}
					throw new IncompatibleClassChangeError(string.Format("{0} and {1} disagree on InnerClasses attribute", declaring.Name, wrapper.Name));
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
#else
				return null;
#endif
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
						interfaces[i] = interfaceWrappers[i].ClassObject;
					}
					return interfaces;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static int GetModifiers(Object cwrapper, bool ignoreInnerClassesAttribute)
			{
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					if(ignoreInnerClassesAttribute)
					{
						// NOTE GNU Classpath's Class gets it wrong and sets the ignoreInnerClassesAttribute
						// when it wants to find out about Enum, Annotation, etc. so we treat the flag instead
						// to mean that we should return the real accessibility flags, but otherwise return the
						// flags from the InnerClass attribute.
						return (int)((wrapper.ReflectiveModifiers & ~Modifiers.AccessMask) | (wrapper.Modifiers & Modifiers.AccessMask));
					}
					return (int)wrapper.ReflectiveModifiers;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static string GetClassSignature(object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				return wrapper.GetGenericSignature();
			}

			public static object GetEnclosingClass(object cwrapper)
			{
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					wrapper.Finish();
					string[] enclosing = wrapper.GetEnclosingMethod();
					if(enclosing != null)
					{
						TypeWrapper enclosingClass = wrapper.GetClassLoader().LoadClassByDottedNameFast(enclosing[0]);
						if(enclosingClass == null)
						{
#if !FIRST_PASS
							throw new jlNoClassDefFoundError(enclosing[0]);
#endif
						}
						return enclosingClass.ClassObject;
					}
					return null;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object GetEnclosingMethod(object cwrapper)
			{
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					string[] enclosing = wrapper.GetEnclosingMethod();
					if(enclosing != null && enclosing[1] != null && enclosing[1] != "<init>")
					{
						TypeWrapper enclosingClass = wrapper.GetClassLoader().LoadClassByDottedNameFast(enclosing[0]);
						if(enclosingClass == null)
						{
#if !FIRST_PASS
							throw new jlNoClassDefFoundError(enclosing[0]);
#endif
						}
						MethodWrapper mw = enclosingClass.GetMethodWrapper(enclosing[1], enclosing[2], false);
						if(mw != null && !mw.IsHideFromReflection)
						{
							return JVM.Library.newMethod(mw.DeclaringType.ClassObject, mw);
						}
					}
					return null;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object GetEnclosingConstructor(object cwrapper)
			{
				try
				{
					TypeWrapper wrapper = (TypeWrapper)cwrapper;
					string[] enclosing = wrapper.GetEnclosingMethod();
					if(enclosing != null && enclosing[1] == "<init>")
					{
						TypeWrapper enclosingClass = wrapper.GetClassLoader().LoadClassByDottedNameFast(enclosing[0]);
						if(enclosingClass == null)
						{
#if !FIRST_PASS
							throw new jlNoClassDefFoundError(enclosing[0]);
#endif
						}
						MethodWrapper mw = enclosingClass.GetMethodWrapper(enclosing[1], enclosing[2], false);
						if(mw != null && !mw.IsHideFromReflection)
						{
							return JVM.Library.newConstructor(mw.DeclaringType.ClassObject, mw);
						}
					}
					return null;
				}
				catch(RetargetableJavaException x)
				{
					throw x.ToJava();
				}
			}

			public static object[] GetDeclaredAnnotations(object cwrapper)
			{
				TypeWrapper wrapper = (TypeWrapper)cwrapper;
				wrapper.Finish();
				return wrapper.GetDeclaredAnnotations();
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
					throw x.ToJava();
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
					MethodWrapper mw = (MethodWrapper)JVM.Library.getWrapperFromMethodOrConstructor(constructor);
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

namespace IKVM.NativeCode.gnu.classpath
{
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
			return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
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
		object getWrapperFromField(object field);
		object getWrapperFromMethodOrConstructor(object methodOrConstructor);

		object getWrapperFromClassLoader(object classLoader);
		void setWrapperForClassLoader(object classLoader, object wrapper);

		object box(object val);
		object unbox(object val);

		Exception mapException(Exception t);

		void jniWaitUntilLastThread();
		void jniDetach();
		void setThreadGroup(object group);

		object newConstructor(object clazz, object wrapper);
		object newMethod(object clazz, object wrapper);
		object newField(object clazz, object wrapper);

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
#endif // !OPENJDK
