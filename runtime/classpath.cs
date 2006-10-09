/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
using IKVM.Attributes;
using IKVM.Runtime;
using IKVM.Internal;

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
						catch(MethodAccessException)
						{
							// this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
							throw JavaException.IllegalAccessException("System.MethodAccessException for {0}.{1}", mw.DeclaringType.Name, mw.Name);
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

				public static bool CheckAccess(object memberCookie, object instance, object callerTypeWrapper)
				{
					MemberWrapper member = (MemberWrapper)memberCookie;
					if(callerTypeWrapper != null)
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
						return member.IsAccessibleFrom(member.DeclaringType, (TypeWrapper)callerTypeWrapper, instanceTypeWrapper);
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
#if !COMPACT_FRAMEWORK
						catch(FieldAccessException)
						{
							// this can happen if we're accessing a non-public field and the call stack doesn't have ReflectionPermission.MemberAccess
							throw JavaException.IllegalAccessException("System.FieldAccessException for {0}.{1}", wrapper.DeclaringType.Name, wrapper.Name);
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
#if !COMPACT_FRAMEWORK
						catch(FieldAccessException)
						{
							// this can happen if we're accessing a non-public field and the call stack doesn't have ReflectionPermission.MemberAccess
							throw JavaException.IllegalAccessException("System.FieldAccessException for {0}.{1}", wrapper.DeclaringType.Name, wrapper.Name);
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

		public class VMDouble
		{
			public static double parseDouble(string s)
			{
				if(s.Length == 0) goto error;
				int first = 0;
				while(s[first] <= ' ')
				{
					first++;
					if(first == s.Length) goto error;
				}
				int last = s.Length - 1;
				while(s[last] <= ' ')
				{
					last--;
					if(first > last) goto error;
				}
				bool sign = false;
				if(s[first] == '-')
				{
					sign = true;
					first++;
					if(first > last) goto error;
				}
				else if(s[first] == '+')
				{
					first++;
					if(first > last) goto error;
				}
				if(last - first == 7 && string.CompareOrdinal(s, first, "Infinity", 0, 8) == 0)
				{
					return sign ? double.NegativeInfinity : double.PositiveInfinity;
				}
				if(last - first == 2 && string.CompareOrdinal(s, first, "NaN", 0, 3) == 0)
				{
					return double.NaN;
				}
				// Java allows 'f' or 'd' at the end
				if("dfDF".IndexOf(s[last]) >= 0)
				{
					last--;
					if(first > last) goto error;
				}
				bool dot = false;
				bool exp = false;
				for(int i = first; i <= last; i++)
				{
					char c = s[i];
					if(c >= '0' && c <= '9')
					{
						// ok
					}
					else if(c == '.')
					{
						if(dot || exp) goto error;
						dot = true;
					}
					else if(c == 'e' || c == 'E')
					{
						if(i == first || i == last || exp) goto error;
						if(s[i + 1] == '-' || s[i + 1] == '+')
						{
							i++;
							if(i == last) goto error;
						}
						exp = true;
					}
					else goto error;
				}
				if(first != 0 || last != s.Length - 1)
				{
					s = s.Substring(first, last - first + 1);
				}
				try
				{
					// I doubt that this is fully correct, but since we don't implement FP properly,
					// we can probably get away with this as well.
					double d = double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
					return sign ? -d : d;
				}
				catch(OverflowException)
				{
					return sign ? double.NegativeInfinity : double.PositiveInfinity;
				}
				catch(FormatException x)
				{
					// this can't happen, since we already validated the format of the string
					System.Diagnostics.Debug.Fail(x.ToString());
				}
				error:
				throw JavaException.NumberFormatException("For input string: \"{0}\"", s);
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
							throw new NoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
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
						if(!wrappers[i].IsAccessibleFrom(wrapper))
						{
							throw JavaException.IllegalAccessError("tried to access class {0} from class {1}", wrappers[i].Name, wrapper.Name);
						}
						innerclasses[i] = wrappers[i].ClassObject;
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
					if(!declaring.IsAccessibleFrom(wrapper))
					{
						throw JavaException.IllegalAccessError("tried to access class {0} from class {1}", declaring.Name, wrapper.Name);
					}
					declaring.Finish();
					foreach(TypeWrapper tw in declaring.InnerClasses)
					{
						if(tw == wrapper)
						{
							return declaring.ClassObject;
						}
					}
					throw JavaException.IncompatibleClassChangeError("{0} and {1} disagree on InnerClasses attribute", declaring.Name, wrapper.Name);
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
							throw JavaException.NoClassDefFoundError(enclosing[0]);
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
							throw JavaException.NoClassDefFoundError(enclosing[0]);
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
							throw JavaException.NoClassDefFoundError(enclosing[0]);
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
#if WHIDBEY
			Stream s = asm.GetManifestResourceStream(JVM.MangleResourceName(resource));
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
			using(Stream s = asm.GetManifestResourceStream(JVM.MangleResourceName(resource)))
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
			return mb.IsDefined(typeof(HideFromJavaAttribute), false)
				|| mb.IsDefined(typeof(HideFromReflectionAttribute), false);
		}
	}
}

namespace gnu.classpath
{
	// This type lives here, because we don't want unverifiable code in IKVM.GNU.Classpath
	// (as that would prevents us from verifying it during the build process).
#if !COMPACT_FRAMEWORK
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
#endif
	public unsafe sealed class Pointer
	{
		// NOTE during static compilation this type is represented by a CompiledTypeWrapper,
		// so we need to hide this field, because Java types cannot have pointer types.
		// Note that at runtime this type is represented by a DotNetTypeWrapper and that
		// DotNetTypeWrapper.LazyPublishMembers has a hack to prevent it from exposing any
		// of the members of this type (because that would be a security problem).
		[HideFromJava]
		private byte* pb;

		public Pointer(IntPtr p)
		{
			this.pb = (byte*)p;
		}

		public IntPtr p()
		{
			return new IntPtr(pb);
		}

		public byte ReadByte(int index)
		{
			return pb[index];
		}

		public void WriteByte(int index, byte b)
		{
			pb[index] = b;
		}

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

		public static Assembly FindResourceAssembly(object classLoader, string name)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = classLoader == null ? ClassLoaderWrapper.GetBootstrapClassLoader() : (JVM.Library.getWrapperFromClassLoader(classLoader) as IKVM.Internal.AssemblyClassLoader);
			if(wrapper == null)
			{
				// must be a GenericClassLoader
				return null;
			}
			if(wrapper.Assembly.GetManifestResourceInfo(JVM.MangleResourceName(name)) != null)
			{
				Tracer.Info(Tracer.ClassLoading, "Found resource \"{0}\" in {1}", name, wrapper.Assembly.FullName);
				return wrapper.Assembly;
			}
			Tracer.Info(Tracer.ClassLoading, "Failed to find resource \"{0}\" in {1}", name, wrapper.Assembly.FullName);
			return null;
		}

		public static Assembly[] FindResourceAssemblies(object classLoader, string name)
		{
			Assembly asm = FindResourceAssembly(classLoader, name);
			if(asm != null)
			{
				return new Assembly[] { asm };
			}
			return null;
		}

		public static Assembly GetClassAssembly(object clazz)
		{
			return ((IKVM.Internal.AssemblyClassLoader)TypeWrapper.FromClass(clazz).GetClassLoader()).Assembly;
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

		public static Assembly GetAssembly(object classLoader)
		{
			IKVM.Internal.AssemblyClassLoader wrapper = JVM.Library.getWrapperFromClassLoader(classLoader) as IKVM.Internal.AssemblyClassLoader;
			if(wrapper == null)
			{
				// must be a GenericClassLoader
				return null;
			}
			return wrapper.Assembly;
		}

		public static bool IsReflectionOnly(Assembly asm)
		{
#if WHIDBEY
			return asm.ReflectionOnly;
#else
			return false;
#endif
		}
	}

	namespace stubgen
	{
		public class StubGenerator
		{
			public static string getAssemblyName(object c)
			{
				return ((IKVM.Internal.AssemblyClassLoader)TypeWrapper.FromClass(c).GetClassLoader()).Assembly.FullName;
			}

			public static object getFieldConstantValue(object fieldWrapper)
			{
				return ((FieldWrapper)fieldWrapper).GetConstant();
			}

			public static bool isFieldDeprecated(object fieldWrapper)
			{
				FieldInfo fi = ((FieldWrapper)fieldWrapper).GetField();
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

			public static bool isMethodDeprecated(object methodWrapper)
			{
				MethodBase mb = ((MethodWrapper)methodWrapper).GetMethod();
				return mb != null && AttributeHelper.IsDefined(mb, typeof(ObsoleteAttribute));
			}

			public static bool isClassDeprecated(object wrapper)
			{
				Type type = ((TypeWrapper)wrapper).TypeAsTBD;
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

		public static Type GetInstanceTypeFromTypeWrapper(object wrapperObject)
		{
			TypeWrapper wrapper = (TypeWrapper)wrapperObject;
			if(wrapper.IsRemapped && wrapper.IsFinal)
			{
				return wrapper.TypeAsTBD;
			}
			return wrapper.TypeAsBaseType;
		}
	}
}

namespace ikvm.@internal
{
	public interface LibraryVMInterface
	{
		object loadClass(object classLoader, string name);
		object newClass(object wrapper, object protectionDomain, object classLoader);
		object getWrapperFromClass(object clazz);
		object getWrapperFromField(object field);
		object getWrapperFromMethodOrConstructor(object methodOrConstructor);

		object getWrapperFromClassLoader(object classLoader);
		void setWrapperForClassLoader(object classLoader, object wrapper);

		object box(object val);
		object unbox(object val);

		Exception mapException(Exception t);
		void printStackTrace(Exception t);

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

		object newAssemblyClassLoader();
	}
}
