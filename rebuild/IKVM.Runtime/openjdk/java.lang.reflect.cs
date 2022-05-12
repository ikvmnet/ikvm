/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System.Collections.Generic;
using IKVM.Internal;

static class Java_java_lang_reflect_Array
{
#if FIRST_PASS
	public static int getLength(object arrayObj)
	{
		return 0;
	}

	public static object get(object arrayObj, int index)
	{
		return null;
	}

	public static bool getBoolean(object arrayObj, int index)
	{
		return false;
	}

	public static byte getByte(object arrayObj, int index)
	{
		return 0;
	}

	public static char getChar(object arrayObj, int index)
	{
		return '\u0000';
	}

	public static short getShort(object arrayObj, int index)
	{
		return 0;
	}

	public static int getInt(object arrayObj, int index)
	{
		return 0;
	}

	public static float getFloat(object arrayObj, int index)
	{
		return 0;
	}

	public static long getLong(object arrayObj, int index)
	{
		return 0;
	}

	public static double getDouble(object arrayObj, int index)
	{
		return 0;
	}

	public static void set(object arrayObj, int index, object value)
	{
	}

	public static void setBoolean(object arrayObj, int index, bool value)
	{
	}

	public static void setByte(object arrayObj, int index, byte value)
	{
	}

	public static void setChar(object arrayObj, int index, char value)
	{
	}

	public static void setShort(object arrayObj, int index, short value)
	{
	}

	public static void setInt(object arrayObj, int index, int value)
	{
	}

	public static void setFloat(object arrayObj, int index, float value)
	{
	}

	public static void setLong(object arrayObj, int index, long value)
	{
	}

	public static void setDouble(object arrayObj, int index, double value)
	{
	}

	public static object newArray(java.lang.Class componentType, int length)
	{
		return null;
	}

	public static object multiNewArray(java.lang.Class componentType, int[] dimensions)
	{
		return null;
	}
#else
	private static Array CheckArray(object arrayObj)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		Array arr = arrayObj as Array;
		if (arr != null)
		{
			return arr;
		}
		throw new java.lang.IllegalArgumentException("Argument is not an array");
	}

	public static int getLength(object arrayObj)
	{
		return CheckArray(arrayObj).Length;
	}

	public static object get(object arrayObj, int index)
	{
		Array arr = CheckArray(arrayObj);
		if (index < 0 || index >= arr.Length)
		{
			throw new java.lang.ArrayIndexOutOfBoundsException();
		}
		// We need to look at the actual type here, because "is" or "as"
		// will convert enums to their underlying type and unsigned integral types
		// to their signed counter parts.
		Type type = arrayObj.GetType();
		if (type == typeof(bool[]))
		{
			return java.lang.Boolean.valueOf(((bool[])arr)[index]);
		}
		if (type == typeof(byte[]))
		{
			return java.lang.Byte.valueOf(((byte[])arr)[index]);
		}
		if (type == typeof(short[]))
		{
			return java.lang.Short.valueOf(((short[])arr)[index]);
		}
		if (type == typeof(char[]))
		{
			return java.lang.Character.valueOf(((char[])arr)[index]);
		}
		if (type == typeof(int[]))
		{
			return java.lang.Integer.valueOf(((int[])arr)[index]);
		}
		if (type == typeof(float[]))
		{
			return java.lang.Float.valueOf(((float[])arr)[index]);
		}
		if (type == typeof(long[]))
		{
			return java.lang.Long.valueOf(((long[])arr)[index]);
		}
		if (type == typeof(double[]))
		{
			return java.lang.Double.valueOf(((double[])arr)[index]);
		}
		return arr.GetValue(index);
	}

	public static bool getBoolean(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		bool[] arr = arrayObj as bool[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		throw new java.lang.IllegalArgumentException("argument type mismatch");
	}

	public static byte getByte(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		byte[] arr = arrayObj as byte[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		throw new java.lang.IllegalArgumentException("argument type mismatch");
	}

	public static char getChar(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		char[] arr = arrayObj as char[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		throw new java.lang.IllegalArgumentException("argument type mismatch");
	}

	public static short getShort(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		short[] arr = arrayObj as short[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		return (sbyte)getByte(arrayObj, index);
	}

	public static int getInt(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		int[] arr1 = arrayObj as int[];
		if (arr1 != null)
		{
			if (index < 0 || index >= arr1.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr1[index];
		}
		char[] arr2 = arrayObj as char[];
		if (arr2 != null)
		{
			if (index < 0 || index >= arr2.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr2[index];
		}
		return getShort(arrayObj, index);
	}

	public static float getFloat(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		float[] arr = arrayObj as float[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		return getLong(arrayObj, index);
	}

	public static long getLong(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		long[] arr = arrayObj as long[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		return getInt(arrayObj, index);
	}

	public static double getDouble(object arrayObj, int index)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		double[] arr = arrayObj as double[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			return arr[index];
		}
		return getFloat(arrayObj, index);
	}

	public static void set(object arrayObj, int index, object value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		Type type = arrayObj.GetType();
		if (ReflectUtil.IsVector(type) && ClassLoaderWrapper.GetWrapperFromType(type.GetElementType()).IsPrimitive)
		{
			java.lang.Boolean booleanValue = value as java.lang.Boolean;
			if (booleanValue != null)
			{
				setBoolean(arrayObj, index, booleanValue.booleanValue());
				return;
			}
			java.lang.Byte byteValue = value as java.lang.Byte;
			if (byteValue != null)
			{
				setByte(arrayObj, index, byteValue.byteValue());
				return;
			}
			java.lang.Character charValue = value as java.lang.Character;
			if (charValue != null)
			{
				setChar(arrayObj, index, charValue.charValue());
				return;
			}
			java.lang.Short shortValue = value as java.lang.Short;
			if (shortValue != null)
			{
				setShort(arrayObj, index, shortValue.shortValue());
				return;
			}
			java.lang.Integer intValue = value as java.lang.Integer;
			if (intValue != null)
			{
				setInt(arrayObj, index, intValue.intValue());
				return;
			}
			java.lang.Float floatValue = value as java.lang.Float;
			if (floatValue != null)
			{
				setFloat(arrayObj, index, floatValue.floatValue());
				return;
			}
			java.lang.Long longValue = value as java.lang.Long;
			if (longValue != null)
			{
				setLong(arrayObj, index, longValue.longValue());
				return;
			}
			java.lang.Double doubleValue = value as java.lang.Double;
			if (doubleValue != null)
			{
				setDouble(arrayObj, index, doubleValue.doubleValue());
				return;
			}
		}
		try
		{
			CheckArray(arrayObj).SetValue(value, index);
		}
		catch (InvalidCastException)
		{
			throw new java.lang.IllegalArgumentException("argument type mismatch");
		}
		catch (IndexOutOfRangeException)
		{
			throw new java.lang.ArrayIndexOutOfBoundsException();
		}
	}

	public static void setBoolean(object arrayObj, int index, bool value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		bool[] arr = arrayObj as bool[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			throw new java.lang.IllegalArgumentException("argument type mismatch");
		}
	}

	public static void setByte(object arrayObj, int index, byte value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		byte[] arr = arrayObj as byte[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			setShort(arrayObj, index, (sbyte)value);
		}
	}

	public static void setChar(object arrayObj, int index, char value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		char[] arr = arrayObj as char[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			setInt(arrayObj, index, value);
		}
	}

	public static void setShort(object arrayObj, int index, short value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		short[] arr = arrayObj as short[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			setInt(arrayObj, index, value);
		}
	}

	public static void setInt(object arrayObj, int index, int value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		int[] arr = arrayObj as int[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			setLong(arrayObj, index, value);
		}
	}

	public static void setFloat(object arrayObj, int index, float value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		float[] arr = arrayObj as float[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			setDouble(arrayObj, index, value);
		}
	}

	public static void setLong(object arrayObj, int index, long value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		long[] arr = arrayObj as long[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			setFloat(arrayObj, index, value);
		}
	}

	public static void setDouble(object arrayObj, int index, double value)
	{
		if (arrayObj == null)
		{
			throw new java.lang.NullPointerException();
		}
		double[] arr = arrayObj as double[];
		if (arr != null)
		{
			if (index < 0 || index >= arr.Length)
			{
				throw new java.lang.ArrayIndexOutOfBoundsException();
			}
			arr[index] = value;
		}
		else
		{
			throw new java.lang.IllegalArgumentException("argument type mismatch");
		}
	}

	public static object newArray(java.lang.Class componentType, int length)
	{
		if (componentType == null)
		{
			throw new java.lang.NullPointerException();
		}
		if (componentType == java.lang.Void.TYPE)
		{
			throw new java.lang.IllegalArgumentException();
		}
		if (length < 0)
		{
			throw new java.lang.NegativeArraySizeException();
		}
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(componentType);
			wrapper.Finish();
			object obj = Array.CreateInstance(wrapper.TypeAsArrayType, length);
			if (wrapper.IsGhost || wrapper.IsGhostArray)
			{
				IKVM.Runtime.GhostTag.SetTag(obj, wrapper.MakeArrayType(1));
			}
			return obj;
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		catch (NotSupportedException x)
		{
			// This happens when you try to create an array from TypedReference, ArgIterator, ByRef,
			// RuntimeArgumentHandle or an open generic type.
			throw new java.lang.IllegalArgumentException(x.Message);
		}
	}

	public static object multiNewArray(java.lang.Class componentType, int[] dimensions)
	{
		if (componentType == null || dimensions == null)
		{
			throw new java.lang.NullPointerException();
		}
		if (componentType == java.lang.Void.TYPE)
		{
			throw new java.lang.IllegalArgumentException();
		}
		if (dimensions.Length == 0 || dimensions.Length > 255)
		{
			throw new java.lang.IllegalArgumentException();
		}
		try
		{
			TypeWrapper wrapper = TypeWrapper.FromClass(componentType).MakeArrayType(dimensions.Length);
			wrapper.Finish();
			object obj = IKVM.Runtime.ByteCodeHelper.multianewarray(wrapper.TypeAsArrayType.TypeHandle, dimensions);
			if (wrapper.IsGhostArray)
			{
				IKVM.Runtime.GhostTag.SetTag(obj, wrapper);
			}
			return obj;
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
		catch (NotSupportedException x)
		{
			// This happens when you try to create an array from TypedReference, ArgIterator, ByRef,
			// RuntimeArgumentHandle or an open generic type.
			throw new java.lang.IllegalArgumentException(x.Message);
		}
	}
#endif // FIRST_PASS
}

static class Java_java_lang_reflect_Proxy
{
	public static object defineClass0(java.lang.ClassLoader classLoader, string name, byte[] b, int off, int len)
	{
		return Java_java_lang_ClassLoader.defineClass1(classLoader, name, b, off, len, null, null);
	}

	public static java.lang.Class getPrecompiledProxy(java.lang.ClassLoader classLoader, string proxyName, java.lang.Class[] interfaces)
	{
		AssemblyClassLoader acl = ClassLoaderWrapper.GetClassLoaderWrapper(classLoader) as AssemblyClassLoader;
		if (acl == null)
		{
			return null;
		}
		TypeWrapper[] wrappers = new TypeWrapper[interfaces.Length];
		for (int i = 0; i < wrappers.Length; i++)
		{
			wrappers[i] = TypeWrapper.FromClass(interfaces[i]);
		}
		// TODO support multi assembly class loaders
		Type type = acl.MainAssembly.GetType(TypeNameUtil.GetProxyName(wrappers));
		if (type == null)
		{
			return null;
		}
		TypeWrapper tw = CompiledTypeWrapper.newInstance(proxyName, type);
		TypeWrapper tw2 = acl.RegisterInitiatingLoader(tw);
		if (tw != tw2)
		{
			return null;
		}
		// we need to explicitly register the type, because the type isn't visible by normal means
		tw.GetClassLoader().SetWrapperForType(type, tw);
		TypeWrapper[] wrappers2 = tw.Interfaces;
		if (wrappers.Length != wrappers.Length)
		{
			return null;
		}
		for (int i = 0; i < wrappers.Length; i++)
		{
			if (wrappers[i] != wrappers2[i])
			{
				return null;
			}
		}
		return tw.ClassObject;
	}
}

static class Java_java_lang_reflect_Executable
{
	public static object[] getParameters0(java.lang.reflect.Executable _this)
	{
#if FIRST_PASS
		return null;
#else
		MethodWrapper mw = MethodWrapper.FromExecutable(_this);
		MethodParametersEntry[] methodParameters = mw.DeclaringType.GetMethodParameters(mw);
		if (methodParameters == null)
		{
			return null;
		}
		if (methodParameters == MethodParametersEntry.Malformed)
		{
			throw new java.lang.reflect.MalformedParametersException("Invalid constant pool index");
		}
		java.lang.reflect.Parameter[] parameters = new java.lang.reflect.Parameter[methodParameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			parameters[i] = new java.lang.reflect.Parameter(methodParameters[i].name ?? "", methodParameters[i].flags, _this, i);
		}
		return parameters;
#endif
	}

	public static byte[] getTypeAnnotationBytes0(java.lang.reflect.Executable _this)
	{
		MethodWrapper mw = MethodWrapper.FromExecutable(_this);
		return mw.DeclaringType.GetMethodRawTypeAnnotations(mw);
	}

	public static object declaredAnnotationsImpl(java.lang.reflect.Executable executable)
	{
		MethodWrapper mw = MethodWrapper.FromExecutable(executable);
		return Java_java_lang_Class.AnnotationsToMap(mw.DeclaringType.GetClassLoader(), mw.DeclaringType.GetMethodAnnotations(mw));
	}

	public static object[][] sharedGetParameterAnnotationsImpl(java.lang.reflect.Executable executable)
	{
#if FIRST_PASS
		return null;
#else
		MethodWrapper mw = MethodWrapper.FromExecutable(executable);
		object[][] objAnn = mw.DeclaringType.GetParameterAnnotations(mw);
		if (objAnn == null)
		{
			return null;
		}
		java.lang.annotation.Annotation[][] ann = new java.lang.annotation.Annotation[objAnn.Length][];
		for (int i = 0; i < ann.Length; i++)
		{
			List<java.lang.annotation.Annotation> list = new List<java.lang.annotation.Annotation>();
			foreach (object obj in objAnn[i])
			{
				java.lang.annotation.Annotation a = obj as java.lang.annotation.Annotation;
				if (a != null)
				{
					list.Add(Java_java_lang_Class.FreezeOrWrapAttribute(a));
				}
				else if (obj is IKVM.Attributes.DynamicAnnotationAttribute)
				{
					a = (java.lang.annotation.Annotation)JVM.NewAnnotation(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), ((IKVM.Attributes.DynamicAnnotationAttribute)obj).Definition);
					if (a != null)
					{
						list.Add(a);
					}
				}
			}
			ann[i] = list.ToArray();
		}
		return ann;
#endif
	}
}

static class Java_java_lang_reflect_Field
{
	public static object getDeclaredAnnotationsImpl(java.lang.reflect.Field thisField)
	{
		FieldWrapper fw = FieldWrapper.FromField(thisField);
		return Java_java_lang_Class.AnnotationsToMap(fw.DeclaringType.GetClassLoader(), fw.DeclaringType.GetFieldAnnotations(fw));
	}

	public static byte[] getTypeAnnotationBytes0(java.lang.reflect.Field thisField)
	{
		FieldWrapper fw = FieldWrapper.FromField(thisField);
		return fw.DeclaringType.GetFieldRawTypeAnnotations(fw);
	}
}

static class Java_java_lang_reflect_Method
{
	public static object getDefaultValue(java.lang.reflect.Method thisMethod)
	{
		MethodWrapper mw = MethodWrapper.FromExecutable(thisMethod);
		return mw.DeclaringType.GetAnnotationDefault(mw);
	}
}
