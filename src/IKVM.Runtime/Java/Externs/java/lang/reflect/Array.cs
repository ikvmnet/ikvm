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

using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;

namespace IKVM.Java.Externs.java.lang.reflect
{

    static class Array
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

        public static object newArray(global::java.lang.Class componentType, int length)
        {
            return null;
        }

        public static object multiNewArray(global::java.lang.Class componentType, int[] dimensions)
        {
            return null;
        }
#else
        private static global::System.Array CheckArray(object arrayObj)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            global::System.Array arr = arrayObj as global::System.Array;
            if (arr != null)
            {
                return arr;
            }
            throw new global::java.lang.IllegalArgumentException("Argument is not an array");
        }

        public static int getLength(object arrayObj)
        {
            return CheckArray(arrayObj).Length;
        }

        public static object get(object arrayObj, int index)
        {
            global::System.Array arr = CheckArray(arrayObj);
            if (index < 0 || index >= arr.Length)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
            // We need to look at the actual type here, because "is" or "as"
            // will convert enums to their underlying type and unsigned integral types
            // to their signed counter parts.
            Type type = arrayObj.GetType();
            if (type == typeof(bool[]))
            {
                return global::java.lang.Boolean.valueOf(((bool[])arr)[index]);
            }
            if (type == typeof(byte[]))
            {
                return global::java.lang.Byte.valueOf(((byte[])arr)[index]);
            }
            if (type == typeof(short[]))
            {
                return global::java.lang.Short.valueOf(((short[])arr)[index]);
            }
            if (type == typeof(char[]))
            {
                return global::java.lang.Character.valueOf(((char[])arr)[index]);
            }
            if (type == typeof(int[]))
            {
                return global::java.lang.Integer.valueOf(((int[])arr)[index]);
            }
            if (type == typeof(float[]))
            {
                return global::java.lang.Float.valueOf(((float[])arr)[index]);
            }
            if (type == typeof(long[]))
            {
                return global::java.lang.Long.valueOf(((long[])arr)[index]);
            }
            if (type == typeof(double[]))
            {
                return global::java.lang.Double.valueOf(((double[])arr)[index]);
            }
            return arr.GetValue(index);
        }

        public static bool getBoolean(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            bool[] arr = arrayObj as bool[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            throw new global::java.lang.IllegalArgumentException("argument type mismatch");
        }

        public static byte getByte(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            byte[] arr = arrayObj as byte[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            throw new global::java.lang.IllegalArgumentException("argument type mismatch");
        }

        public static char getChar(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            char[] arr = arrayObj as char[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            throw new global::java.lang.IllegalArgumentException("argument type mismatch");
        }

        public static short getShort(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            short[] arr = arrayObj as short[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            return (sbyte)getByte(arrayObj, index);
        }

        public static int getInt(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            int[] arr1 = arrayObj as int[];
            if (arr1 != null)
            {
                if (index < 0 || index >= arr1.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr1[index];
            }
            char[] arr2 = arrayObj as char[];
            if (arr2 != null)
            {
                if (index < 0 || index >= arr2.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr2[index];
            }
            return getShort(arrayObj, index);
        }

        public static float getFloat(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            float[] arr = arrayObj as float[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            return getLong(arrayObj, index);
        }

        public static long getLong(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            long[] arr = arrayObj as long[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            return getInt(arrayObj, index);
        }

        public static double getDouble(object arrayObj, int index)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            double[] arr = arrayObj as double[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                return arr[index];
            }
            return getFloat(arrayObj, index);
        }

        public static void set(object arrayObj, int index, object value)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            Type type = arrayObj.GetType();
            if (ReflectUtil.IsVector(type) && JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(type.GetElementType()).IsPrimitive)
            {
                global::java.lang.Boolean booleanValue = value as global::java.lang.Boolean;
                if (booleanValue != null)
                {
                    setBoolean(arrayObj, index, booleanValue.booleanValue());
                    return;
                }
                global::java.lang.Byte byteValue = value as global::java.lang.Byte;
                if (byteValue != null)
                {
                    setByte(arrayObj, index, byteValue.byteValue());
                    return;
                }
                global::java.lang.Character charValue = value as global::java.lang.Character;
                if (charValue != null)
                {
                    setChar(arrayObj, index, charValue.charValue());
                    return;
                }
                global::java.lang.Short shortValue = value as global::java.lang.Short;
                if (shortValue != null)
                {
                    setShort(arrayObj, index, shortValue.shortValue());
                    return;
                }
                global::java.lang.Integer intValue = value as global::java.lang.Integer;
                if (intValue != null)
                {
                    setInt(arrayObj, index, intValue.intValue());
                    return;
                }
                global::java.lang.Float floatValue = value as global::java.lang.Float;
                if (floatValue != null)
                {
                    setFloat(arrayObj, index, floatValue.floatValue());
                    return;
                }
                global::java.lang.Long longValue = value as global::java.lang.Long;
                if (longValue != null)
                {
                    setLong(arrayObj, index, longValue.longValue());
                    return;
                }
                global::java.lang.Double doubleValue = value as global::java.lang.Double;
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
                throw new global::java.lang.IllegalArgumentException("argument type mismatch");
            }
            catch (IndexOutOfRangeException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
        }

        public static void setBoolean(object arrayObj, int index, bool value)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            bool[] arr = arrayObj as bool[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                arr[index] = value;
            }
            else
            {
                throw new global::java.lang.IllegalArgumentException("argument type mismatch");
            }
        }

        public static void setByte(object arrayObj, int index, byte value)
        {
            if (arrayObj == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            byte[] arr = arrayObj as byte[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
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
                throw new global::java.lang.NullPointerException();
            }
            char[] arr = arrayObj as char[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
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
                throw new global::java.lang.NullPointerException();
            }
            short[] arr = arrayObj as short[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
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
                throw new global::java.lang.NullPointerException();
            }
            int[] arr = arrayObj as int[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
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
                throw new global::java.lang.NullPointerException();
            }
            float[] arr = arrayObj as float[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
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
                throw new global::java.lang.NullPointerException();
            }
            long[] arr = arrayObj as long[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
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
                throw new global::java.lang.NullPointerException();
            }
            double[] arr = arrayObj as double[];
            if (arr != null)
            {
                if (index < 0 || index >= arr.Length)
                {
                    throw new global::java.lang.ArrayIndexOutOfBoundsException();
                }
                arr[index] = value;
            }
            else
            {
                throw new global::java.lang.IllegalArgumentException("argument type mismatch");
            }
        }

        public static object newArray(global::java.lang.Class componentType, int length)
        {
            if (componentType == null)
                throw new global::java.lang.NullPointerException();
            if (componentType == global::java.lang.Void.TYPE)
                throw new global::java.lang.IllegalArgumentException();
            if (length < 0)
                throw new global::java.lang.NegativeArraySizeException();

            try
            {
                var wrapper = RuntimeJavaType.FromClass(componentType);
                wrapper.Finish();
                object obj = global::System.Array.CreateInstance(wrapper.TypeAsArrayType, length);
                if (wrapper.IsGhost || wrapper.IsGhostArray)
                    GhostTag.SetTag(obj, wrapper.MakeArrayType(1));

                return obj;
            }
            catch (NotSupportedException x)
            {
                // This happens when you try to create an array from TypedReference, ArgIterator, ByRef,
                // RuntimeArgumentHandle or an open generic type.
                throw new global::java.lang.IllegalArgumentException(x.Message);
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
        }

        public static object multiNewArray(global::java.lang.Class componentType, int[] dimensions)
        {
            if (componentType == null || dimensions == null)
                throw new global::java.lang.NullPointerException();
            if (componentType == global::java.lang.Void.TYPE)
                throw new global::java.lang.IllegalArgumentException();
            if (dimensions.Length == 0 || dimensions.Length > 255)
                throw new global::java.lang.IllegalArgumentException();

            try
            {
                var wrapper = RuntimeJavaType.FromClass(componentType).MakeArrayType(dimensions.Length);
                wrapper.Finish();

                var obj = ByteCodeHelper.multianewarray(wrapper.TypeAsArrayType.TypeHandle, dimensions);
                if (wrapper.IsGhostArray)
                    GhostTag.SetTag(obj, wrapper);

                return obj;
            }
            catch (NotSupportedException e)
            {
                // This happens when you try to create an array from TypedReference, ArgIterator, ByRef,
                // RuntimeArgumentHandle or an open generic type.
                throw new global::java.lang.IllegalArgumentException(e.Message);
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
        }

#endif // FIRST_PASS

    }

}
