/*
  Copyright (C) 2007-2015 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

using IKVM.Internal;

namespace IKVM.Java.Externs.sun.misc
{

    static class Unsafe
    {

        static readonly ConditionalWeakTable<FieldInfo, Delegate> getFieldCache = new ConditionalWeakTable<FieldInfo, Delegate>();
        static readonly ConditionalWeakTable<FieldInfo, Delegate> putFieldCache = new ConditionalWeakTable<FieldInfo, Delegate>();

        delegate int CompareExchangeInt32(object obj, int value, int comparand);
        delegate long CompareExchangeInt64(object obj, long value, long comparand);
        delegate object CompareExchangeObject(object obj, object value, object comparand);

        static readonly ConditionalWeakTable<FieldInfo, CompareExchangeInt32> compareExchangeInt32Cache = new ConditionalWeakTable<FieldInfo, CompareExchangeInt32>();
        static readonly ConditionalWeakTable<FieldInfo, CompareExchangeInt64> compareExchangeInt64Cache = new ConditionalWeakTable<FieldInfo, CompareExchangeInt64>();
        static readonly ConditionalWeakTable<FieldInfo, CompareExchangeObject> compareExchangeObjectCache = new ConditionalWeakTable<FieldInfo, CompareExchangeObject>();

        public static void registerNatives()
        {

        }

        /// <summary>
        /// Creates a delegate capable of accessing a field of a specific type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        static Func<object, TField> CreateGetFieldDelegate<TField>(FieldInfo f)
        {
            var p = Expression.Parameter(typeof(object));
            return Expression.Lambda<Func<object, TField>>(Expression.Convert(Expression.Field(Expression.Convert(p, f.DeclaringType), f), typeof(TField)), p).Compile();
        }

        /// <summary>
        /// Creates a delegate capable of accessing a field of a specific type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        static Func<object, TField> GetOrCreateGetFieldDelegate<TField>(FieldInfo f)
        {
            return (Func<object, TField>)getFieldCache.GetValue(f, _ => CreateGetFieldDelegate<TField>(_));
        }

        /// <summary>
        /// Creates a delegate capable of accessing a field of a specific type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        static Action<object, TField> CreatePutFieldDelegate<TField>(FieldInfo f)
        {
            var p = Expression.Parameter(typeof(object));
            var v = Expression.Parameter(typeof(TField));
            return Expression.Lambda<Action<object, TField>>(Expression.Assign(Expression.Field(Expression.Convert(p, f.DeclaringType), f), v), p, v).Compile();
        }

        /// <summary>
        /// Creates a delegate capable of assigning a field of a specific type.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        static Action<object, TField> GetOrCreatePutFieldDelegate<TField>(FieldInfo f)
        {
            return (Action<object, TField>)putFieldCache.GetValue(f, _ => CreatePutFieldDelegate<TField>(_));
        }

        public static int getInt(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return ReadInt32(array, offset);
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<int>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putInt(object self, object o, long offset, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                WriteInt32(array, offset, x);
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<int>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static object getObject(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is object[] array)
            {
                return array[(int)offset];
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<object>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putObject(object self, object o, long offset, object x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is object[] array)
            {
                array[(int)offset] = x;
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<object>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static bool getBoolean(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return Buffer.GetByte(array, (int)offset) != 0;
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<bool>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putBoolean(object self, object o, long offset, bool x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                Buffer.SetByte(array, (int)offset, x ? (byte)1 : (byte)0);
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<bool>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static byte getByte(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return Buffer.GetByte(array, (int)offset);
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<byte>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putByte(object self, object o, long offset, byte x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                Buffer.SetByte(array, (int)offset, x);
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<byte>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static short getShort(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return ReadInt16(array, offset);
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<short>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putShort(object self, object o, long offset, short x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                WriteInt16(array, offset, x);
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<short>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static char getChar(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return (char)ReadInt16(array, offset);
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<char>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putChar(object self, object o, long offset, char x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                WriteInt16(array, offset, (short)x);
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<char>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static long getLong(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return ReadInt64(array, offset);
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<long>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putLong(object self, object o, long offset, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                WriteInt64(array, offset, x);
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<long>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static float getFloat(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return global::java.lang.Float.intBitsToFloat(ReadInt32(array, offset));
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<float>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putFloat(object self, object o, long offset, float x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                WriteInt32(array, offset, global::java.lang.Float.floatToRawIntBits(x));
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<float>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static double getDouble(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                return global::java.lang.Double.longBitsToDouble(ReadInt64(array, offset));
            }
            else
            {
                try
                {
                    return GetOrCreateGetFieldDelegate<double>(GetFieldInfo(offset))(o);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putDouble(object self, object o, long offset, double x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                WriteInt64(array, offset, global::java.lang.Double.doubleToRawLongBits(x));
            }
            else
            {
                try
                {
                    GetOrCreatePutFieldDelegate<double>(GetFieldInfo(offset))(o, x);
                }
                catch (Exception e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static byte getByte(object self, long address)
        {
            return Marshal.ReadByte((IntPtr)address);
        }

        public static void putByte(object self, long address, byte x)
        {
            Marshal.WriteByte((IntPtr)address, x);
        }

        public static short getShort(object self, long address)
        {
            return Marshal.ReadInt16((IntPtr)address);
        }

        public static void putShort(object self, long address, short x)
        {
            Marshal.WriteInt16((IntPtr)address, x);
        }

        public static char getChar(object self, long address)
        {
            return (char)Marshal.ReadInt16((IntPtr)address);
        }

        public static void putChar(object self, long address, char x)
        {
            Marshal.WriteInt16((IntPtr)address, (short)x);
        }

        public static int getInt(object self, long address)
        {
            return Marshal.ReadInt32((IntPtr)address);
        }

        public static void putInt(object self, long address, int x)
        {
            Marshal.WriteInt32((IntPtr)address, x);
        }

        public static long getLong(object self, long address)
        {
            return Marshal.ReadInt64((IntPtr)address);
        }

        public static void putLong(object self, long address, long x)
        {
            Marshal.WriteInt64((IntPtr)address, x);
        }

        public static float getFloat(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return global::java.lang.Float.intBitsToFloat(getInt(self, address));
#endif
        }

        public static void putFloat(object self, long address, float x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            putInt(self, address, global::java.lang.Float.floatToIntBits(x));
#endif
        }

        public static double getDouble(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return global::java.lang.Double.longBitsToDouble(getLong(self, address));
#endif
        }

        public static void putDouble(object self, long address, double x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            putLong(self, address, global::java.lang.Double.doubleToLongBits(x));
#endif
        }

        public static long getAddress(object self, long address)
        {
            return Marshal.ReadIntPtr((IntPtr)address).ToInt64();
        }

        public static void putAddress(object self, long address, long x)
        {
            Marshal.WriteIntPtr((IntPtr)address, (IntPtr)x);
        }

        /// <summary>
        /// Implementation of native method 'allocateMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.OutOfMemoryError"></exception>
        public static long allocateMemory(object self, long bytes)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (bytes == 0)
            {
                return 0;
            }
            try
            {
                return Marshal.AllocHGlobal((IntPtr)bytes).ToInt64();
            }
            catch (OutOfMemoryException e)
            {
                throw new global::java.lang.OutOfMemoryError(e.Message);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'reallocateMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.OutOfMemoryError"></exception>
        public static long reallocateMemory(object self, long address, long bytes)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (bytes == 0)
            {
                freeMemory(self, address);
                return 0;
            }
            try
            {
                return Marshal.ReAllocHGlobal((IntPtr)address, (IntPtr)bytes).ToInt64();
            }
            catch (OutOfMemoryException e)
            {
                throw new global::java.lang.OutOfMemoryError(e.Message);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'setMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="bytes"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.IllegalArgumentException"></exception>
        public static void setMemory(object self, object o, long offset, long bytes, byte value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o == null)
                while (bytes-- > 0)
                    putByte(self, offset++, value);
            else if (o is byte[] array)
                ((Span<byte>)array).Slice((int)offset, (int)bytes).Fill(value);
            else if (o is Array df)
                for (int i = 0; i < bytes; i++)
                    Buffer.SetByte(df, (int)(offset + i), value);
            else
                throw new global::java.lang.IllegalArgumentException();
#endif
        }

        /// <summary>
        /// Implementation of native method 'copyMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="srcBase"></param>
        /// <param name="srcOffset"></param>
        /// <param name="destBase"></param>
        /// <param name="destOffset"></param>
        /// <param name="bytes"></param>
        /// <exception cref="global::java.lang.IllegalArgumentException"></exception>
        public static void copyMemory(object self, object srcBase, long srcOffset, object destBase, long destOffset, long bytes)
        {
            void copyMemory2(long srcAddress, long destAddress, long bytes)
            {
                while (bytes-- > 0)
                    putByte(self, destAddress++, getByte(self, srcAddress++));
            }

            if (srcBase == null)
            {
                if (destBase is byte[] byteArray)
                {
                    Marshal.Copy((IntPtr)(srcOffset), byteArray, (int)destOffset, (int)bytes);
                }
                else if (destBase is bool[])
                {
                    byte[] tmp = new byte[(int)bytes];
                    copyMemory(self, srcBase, srcOffset, tmp, 0, bytes);
                    copyMemory(self, tmp, 0, destBase, destOffset, bytes);
                }
                else if (destBase is short[] shortArray)
                {
                    Marshal.Copy((IntPtr)(srcOffset), shortArray, (int)(destOffset >> 1), (int)(bytes >> 1));
                }
                else if (destBase is char[] charArray)
                {
                    Marshal.Copy((IntPtr)(srcOffset), charArray, (int)(destOffset >> 1), (int)(bytes >> 1));
                }
                else if (destBase is int[] intArray)
                {
                    Marshal.Copy((IntPtr)srcOffset, intArray, (int)(destOffset >> 2), (int)(bytes >> 2));
                }
                else if (destBase is float[] floatArray)
                {
                    Marshal.Copy((IntPtr)(srcOffset), floatArray, (int)(destOffset >> 2), (int)(bytes >> 2));
                }
                else if (destBase is long[] longArray)
                {
                    Marshal.Copy((IntPtr)(srcOffset), longArray, (int)(destOffset >> 3), (int)(bytes >> 3));
                }
                else if (destBase is double[] doubleArray)
                {
                    Marshal.Copy((IntPtr)(srcOffset), doubleArray, (int)(destOffset >> 3), (int)(bytes >> 3));
                }
                else if (destBase == null)
                {
                    copyMemory2(srcOffset, destOffset, bytes);
                }
                else
                {
                    throw new global::java.lang.IllegalArgumentException();
                }
            }
            else if (srcBase is Array && destBase is Array)
            {
                Buffer.BlockCopy((Array)srcBase, (int)srcOffset, (Array)destBase, (int)destOffset, (int)bytes);
            }
            else
            {
                if (srcBase is byte[] byteArray)
                {
                    Marshal.Copy(byteArray, (int)srcOffset, (IntPtr)(destOffset), (int)bytes);
                }
                else if (srcBase is bool[])
                {
                    byte[] tmp = new byte[(int)bytes];
                    copyMemory(self, srcBase, srcOffset, tmp, 0, bytes);
                    copyMemory(self, tmp, 0, destBase, destOffset, bytes);
                }
                else if (srcBase is short[] shortArray)
                {
                    Marshal.Copy(shortArray, (int)(srcOffset >> 1), (IntPtr)(destOffset), (int)(bytes >> 1));
                }
                else if (srcBase is char[] charArray)
                {
                    Marshal.Copy(charArray, (int)(srcOffset >> 1), (IntPtr)(destOffset), (int)(bytes >> 1));
                }
                else if (srcBase is int[] intArray)
                {
                    Marshal.Copy(intArray, (int)(srcOffset >> 2), (IntPtr)(destOffset), (int)(bytes >> 2));
                }
                else if (srcBase is float[] floatArray)
                {
                    Marshal.Copy(floatArray, (int)(srcOffset >> 2), (IntPtr)(destOffset), (int)(bytes >> 2));
                }
                else if (srcBase is long[] longArray)
                {
                    Marshal.Copy(longArray, (int)(srcOffset >> 3), (IntPtr)(destOffset), (int)(bytes >> 3));
                }
                else if (srcBase is double[] doubleArray)
                {
                    Marshal.Copy(doubleArray, (int)(srcOffset >> 3), (IntPtr)(destOffset), (int)(bytes >> 3));
                }
                else
                {
                    throw new global::java.lang.IllegalArgumentException();
                }
            }
        }

        /// <summary>
        /// Implementation of native method 'freeMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        public static void freeMemory(object self, long address)
        {
            Marshal.FreeHGlobal((IntPtr)address);
        }

        /// <summary>
        /// Implementation of native method 'staticFieldOffset'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long staticFieldOffset(object self, global::java.lang.reflect.Field f)
        {
            var fw = FieldWrapper.FromField(f);
            return (long)fw.Cookie;
        }

        /// <summary>
        /// Implementation of native method 'objectFieldOffset'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long objectFieldOffset(object self, global::java.lang.reflect.Field f)
        {
            var fw = FieldWrapper.FromField(f);
            return (long)fw.Cookie;
        }

        /// <summary>
        /// Implementation of native method 'staticFieldBase'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static object staticFieldBase(object self, global::java.lang.reflect.Field f)
        {
            return null;
        }

        /// <summary>
        /// Implementation of native method 'shouldBeInitialized'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool shouldBeInitialized(object self, global::java.lang.Class c)
        {
            return TypeWrapper.FromClass(c).HasStaticInitializer;
        }

        /// <summary>
        /// Implementation of native method 'ensureClassInitialized'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="c"></param>
        public static void ensureClassInitialized(object self, global::java.lang.Class c)
        {
            var tw = TypeWrapper.FromClass(c);
            if (tw.IsArray == false)
            {
                try
                {
                    tw.Finish();
                }
                catch (RetargetableJavaException x)
                {
                    throw x.ToJava();
                }

                tw.RunClassInit();
            }
        }

        /// <summary>
        /// Implementation of native method 'arrayBaseOffset'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="arrayClass"></param>
        /// <returns></returns>
        public static int arrayBaseOffset(object self, global::java.lang.Class arrayClass)
        {
            return 0;
        }

        /// <summary>
        /// Implementation of native method 'arrayIndexScale'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="arrayClass"></param>
        /// <returns></returns>
        public static int arrayIndexScale(object self, global::java.lang.Class arrayClass)
        {
            var tw = TypeWrapper.FromClass(arrayClass);
            var ac = tw.TypeAsTBD;

            if (ac == typeof(byte[]) || ac == typeof(bool[]))
                return 1;

            if (ac == typeof(char[]) || ac == typeof(short[]))
                return 2;

            if (ac == typeof(int[]) || ac == typeof(float[]) || ac == typeof(Object[]))
                return 4;

            if (ac == typeof(long[]) || ac == typeof(double[]))
                return 8;

            // don't change this, the Unsafe intrinsics depend on this value
            return 1;
        }

        /// <summary>
        /// Implementation of native method 'addressSize'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int addressSize(object self)
        {
            return IntPtr.Size;
        }

        /// <summary>
        /// Implementation of native method 'pageSize'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int pageSize(object self)
        {
            return Environment.SystemPageSize;
        }

        /// <summary>
        /// Implementation of native method 'defineClass'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <param name="loader"></param>
        /// <param name="protectionDomain"></param>
        /// <returns></returns>
        public static global::java.lang.Class defineClass(object self, string name, byte[] b, int off, int len, global::java.lang.ClassLoader loader, global::java.security.ProtectionDomain protectionDomain)
        {
            return IKVM.Java.Externs.java.lang.ClassLoader.defineClass1(loader, name.Replace('/', '.'), b, off, len, protectionDomain, null);
        }

        /// <summary>
        /// Implementation of native method 'defineAnonymousClass'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="hostClass"></param>
        /// <param name="data"></param>
        /// <param name="cpPatches"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.ClassFormatError"></exception>
        public static global::java.lang.Class defineAnonymousClass(object self, global::java.lang.Class hostClass, byte[] data, object[] cpPatches)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var tw = TypeWrapper.FromClass(hostClass);
                var cl = tw.GetClassLoader();
                var cf = new ClassFile(data, 0, data.Length, "<Unknown>", cl.ClassFileParseOptions, cpPatches);
                if (cf.IKVMAssemblyAttribute != null)
                {
                    // if this happens, the OpenJDK is probably trying to load an OpenJDK class file as a resource,
                    // make sure the build process includes the original class file as a resource in that case
                    throw new global::java.lang.ClassFormatError("Trying to define anonymous class based on stub class: " + cf.Name);
                }

                return cl.GetTypeWrapperFactory().DefineClassImpl(null, tw, cf, cl, hostClass.pd).ClassObject;
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'allocateInstance'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public static object allocateInstance(object self, global::java.lang.Class cls)
        {
            var wrapper = TypeWrapper.FromClass(cls);
            try
            {
                wrapper.Finish();
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }

            return FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
        }

        /// <summary>
        /// Implementation of native method 'monitorEnter'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        public static void monitorEnter(object self, object o)
        {
            Monitor.Enter(o);
        }

        /// <summary>
        /// Implementation of native method 'monitorExit'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        public static void monitorExit(object self, object o)
        {
            Monitor.Exit(o);
        }

        /// <summary>
        /// Implementation of native method 'tryMonitorEnter'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool tryMonitorEnter(object self, object o)
        {
            return Monitor.TryEnter(o);
        }

        /// <summary>
        /// Implementation of native method 'throwException'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ee"></param>
        public static void throwException(object self, Exception ee)
        {
            throw ee;
        }

        /// <summary>
        /// Implementation of native method 'compareAndSwapObject'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="expected"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool compareAndSwapObject(object self, object o, long offset, object expected, object x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is object[] array)
            {
                return AtomicArray.CompareExchange(array, (int)offset, x, expected) == expected;
            }
            else
            {
                var field = GetFieldInfo(offset);
                if (field == null)
                    throw new InvalidOperationException();

                // get or create delegate for field
                var d = compareExchangeObjectCache.GetValue(field, _ => (CompareExchangeObject)CreateCompareExchangeDelegate(_));
                return d(o, x, expected) == expected;
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'compareAndSwapInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="expected"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool compareAndSwapInt(object self, object o, long offset, int expected, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is int[] array && (offset & 3) == 0)
            {
                return Interlocked.CompareExchange(ref array[offset / 4], x, expected) == expected;
            }
            else if (o is Array array1)
            {
                // unaligned or not the right array type, so we can't be atomic
                lock (self)
                {
                    if (ReadInt32(array1, offset) == expected)
                    {
                        WriteInt32(array1, offset, x);
                        return true;
                    }

                    return false;
                }
            }
            else
            {
                var field = GetFieldInfo(offset);
                if (field == null)
                    throw new InvalidOperationException();

                // get or create delegate for field
                var d = compareExchangeInt32Cache.GetValue(field, _ => (CompareExchangeInt32)CreateCompareExchangeDelegate(_));
                return d(o, x, expected) == expected;
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'compareAndSwapLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="expected"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool compareAndSwapLong(object self, object o, long offset, long expected, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is long[] array && (offset & 7) == 0)
            {
                return Interlocked.CompareExchange(ref array[offset / 8], x, expected) == expected;
            }
            else if (o is Array array1)
            {
                // unaligned or not the right array type, so we can't be atomic
                lock (self)
                {
                    if (ReadInt64(array1, offset) == expected)
                    {
                        WriteInt64(array1, offset, x);
                        return true;
                    }

                    return false;
                }
            }
            else
            {
                var field = GetFieldInfo(offset);
                if (field == null)
                    throw new InvalidOperationException();

                // get or create delegate for field
                var d = compareExchangeInt64Cache.GetValue(field, _ => (CompareExchangeInt64)CreateCompareExchangeDelegate(_));
                return d(o, x, expected) == expected;
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getObjectVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static object getObjectVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is object[] array)
            {
                lock (self)
                    return array[(int)offset];
            }
            else
            {
                return getObject(self, o, offset);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putObjectVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putObjectVolatile(object self, object o, long offset, object x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is object[] array)
            {
                lock (self)
                    array[(int)offset] = x;
            }
            else
            {
                putObject(self, o, offset, x);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getIntVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int getIntVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    return ReadInt32(array, offset);
            }
            else
            {
                return getInt(self, o, offset);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putIntVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putIntVolatile(object self, object o, long offset, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    putInt(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putInt(self, o, offset, x);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getBooleanVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool getBooleanVolatile(object self, object o, long offset)
        {
            return getBoolean(self, o, offset);
        }

        /// <summary>
        /// Implementation of native method 'putBooleanVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putBooleanVolatile(object self, object o, long offset, bool x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    putBoolean(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putBoolean(self, o, offset, x);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getByteVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte getByteVolatile(object self, object o, long offset)
        {
            return getByte(self, o, offset);
        }

        /// <summary>
        /// Implementation of native method 'putByteVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putByteVolatile(object self, object o, long offset, byte x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    putByte(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putByte(self, o, offset, x);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getShortVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static short getShortVolatile(object self, object o, long offset)
        {
            return getShort(self, o, offset);
        }

        /// <summary>
        /// Implementation of native method 'putShortVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putShortVolatile(object self, object o, long offset, short x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    putShort(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putShort(self, o, offset, x);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getCharVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static char getCharVolatile(object self, object o, long offset)
        {
            return getChar(self, o, offset);
        }

        /// <summary>
        /// Implementation of native method 'putCharVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putCharVolatile(object self, object o, long offset, char x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    putChar(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putChar(self, o, offset, x);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getLongVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static long getLongVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    return ReadInt64(array, offset);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    return getLong(self, o, offset);
            }
#endif
        }

        public static void putLongVolatile(object self, object o, long offset, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (o is Array array)
            {
                lock (self)
                    WriteInt64(array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putLong(self, o, offset, x);
            }
#endif
        }

        public static float getFloatVolatile(object self, object o, long offset)
        {
            return getFloat(self, o, offset);
        }

        public static void putFloatVolatile(object self, object o, long offset, float x)
        {
            if (o is Array array)
            {
                lock (self)
                    putFloat(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putFloat(self, o, offset, x);
            }
        }

        public static double getDoubleVolatile(object self, object o, long offset)
        {
            lock (self)
                return getDouble(self, o, offset);
        }

        public static void putDoubleVolatile(object self, object o, long offset, double x)
        {
            if (o is Array array)
            {
                lock (self)
                    putDouble(self, array, offset, x);
            }
            else
            {
                lock (GetFieldInfo(offset))
                    putDouble(self, o, offset, x);
            }
        }

        public static void putOrderedObject(object self, object o, long offset, object x)
        {
            putObjectVolatile(self, o, offset, x);
        }

        public static void putOrderedInt(object self, object o, long offset, int x)
        {
            putIntVolatile(self, o, offset, x);
        }

        public static void putOrderedLong(object self, object o, long offset, long x)
        {
            putLongVolatile(self, o, offset, x);
        }

        public static void unpark(object self, object thread)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            global::java.util.concurrent.locks.LockSupport.unpark((global::java.lang.Thread)thread);
#endif
        }

        public static void park(object self, bool isAbsolute, long time)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (isAbsolute)
            {
                global::java.util.concurrent.locks.LockSupport.parkUntil(time);
            }
            else
            {
                if (time == 0)
                    time = global::java.lang.Long.MAX_VALUE;

                global::java.util.concurrent.locks.LockSupport.parkNanos(time);
            }
#endif
        }

        public static int getLoadAverage(object self, double[] loadavg, int nelems)
        {
            return -1;
        }

        public static void loadFence(object self)
        {
            System.Threading.Thread.MemoryBarrier();
        }

        public static void storeFence(object self)
        {
            System.Threading.Thread.MemoryBarrier();
        }

        public static void fullFence(object self)
        {
            System.Threading.Thread.MemoryBarrier();
        }

        /// <summary>
        /// Gets the reflective Field instance from the specified offset.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        static FieldInfo GetFieldInfo(long offset)
        {
            var fw = FieldWrapper.FromCookie((IntPtr)offset);
            fw.Link();
            fw.ResolveField();
            var fi = fw.GetField();
            return fi;
        }

        /// <summary>
        /// Gets the reflective Field instance from the specified offset.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        static global::java.lang.reflect.Field GetField(long offset)
        {
            var fw = FieldWrapper.FromCookie((IntPtr)offset);
            fw.Link();
            fw.ResolveField();
            var fi = (global::java.lang.reflect.Field)fw.ToField(false);
            return fi;
        }

        static void CheckArrayBounds(Array array, long offset, int accessLength)
        {
            var arrayLength = Buffer.ByteLength(array);
            if (offset < 0 || offset > arrayLength - accessLength || accessLength > arrayLength)
                throw new IndexOutOfRangeException();
        }

        static short ReadInt16(Array obj, long offset)
        {
            CheckArrayBounds(obj, offset, 2);
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            var value = Marshal.ReadInt16((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset));
            handle.Free();
            return value;
        }

        static int ReadInt32(Array obj, long offset)
        {
            CheckArrayBounds(obj, offset, 4);
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            var value = Marshal.ReadInt32((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset));
            handle.Free();
            return value;
        }

        static long ReadInt64(Array obj, long offset)
        {
            CheckArrayBounds(obj, offset, 8);
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            var value = Marshal.ReadInt64((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset));
            handle.Free();
            return value;
        }

        static void WriteInt16(Array obj, long offset, short value)
        {
            CheckArrayBounds(obj, offset, 2);
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            Marshal.WriteInt16((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset), value);
            handle.Free();
        }

        static void WriteInt32(Array obj, long offset, int value)
        {
            CheckArrayBounds(obj, offset, 4);
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            Marshal.WriteInt32((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset), value);
            handle.Free();
        }

        static void WriteInt64(Array obj, long offset, long value)
        {
            CheckArrayBounds(obj, offset, 8);
            var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            Marshal.WriteInt64((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset), value);
            handle.Free();
        }

#if !FIRST_PASS

        /// <summary>
        /// Creates a dynamic method that implements the compare and exchange logic for a given field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        static Delegate CreateCompareExchangeDelegate(FieldInfo field)
        {
            var primitive = field.FieldType.IsPrimitive;
            var signatureType = primitive ? field.FieldType : typeof(object);

            MethodInfo compareExchange;
            Type delegateType;

            if (signatureType == typeof(int))
            {
                compareExchange = InterlockedMethods.CompareExchangeInt32;
                delegateType = typeof(CompareExchangeInt32);
            }
            else if (signatureType == typeof(long))
            {
                compareExchange = InterlockedMethods.CompareExchangeInt64;
                delegateType = typeof(CompareExchangeInt64);
            }
            else
            {
                compareExchange = InterlockedMethods.CompareExchangeOfT.MakeGenericMethod(field.FieldType);
                delegateType = typeof(CompareExchangeObject);
            }

            var dm = new DynamicMethod("CompareExchange", signatureType, new Type[] { typeof(object), signatureType, signatureType }, field.DeclaringType);
            var ilgen = dm.GetILGenerator();
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
            ilgen.Emit(OpCodes.Ldflda, field);
            ilgen.Emit(OpCodes.Ldarg_1);
            if (!primitive)
                ilgen.Emit(OpCodes.Castclass, field.FieldType);
            ilgen.Emit(OpCodes.Ldarg_2);
            if (!primitive)
                ilgen.Emit(OpCodes.Castclass, field.FieldType);
            ilgen.Emit(OpCodes.Call, compareExchange);
            ilgen.Emit(OpCodes.Ret);
            return dm.CreateDelegate(delegateType);
        }

#endif

        /// <summary>
        /// Base class for typed atomic instances. Type-specific instances are maintained in a weak table to allow collection of types.
        /// </summary>
        abstract class AtomicArray
        {

            readonly static ConditionalWeakTable<Type, AtomicArray> cache = new ConditionalWeakTable<Type, AtomicArray>();

            /// <summary>
            /// Provides a CompareExchange implementation that caches by type.
            /// </summary>
            /// <param name="array"></param>
            /// <param name="index"></param>
            /// <param name="value"></param>
            /// <param name="comparand"></param>
            /// <returns></returns>
            public static object CompareExchange(object[] array, int index, object value, object comparand)
            {
                return GetImpl(array.GetType().GetElementType()).CompareExchangeImpl(array, index, value, comparand);
            }

            /// <summary>
            /// Gets the <see cref="AtomicArray"/> instance for the specified <see cref="Type"/>.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            static AtomicArray GetImpl(Type type)
            {
                if (cache.TryGetValue(type, out var impl) == false)
                    cache.Add(type, impl = (AtomicArray)Activator.CreateInstance(typeof(Impl<>).MakeGenericType(type)));

                return impl;
            }

            protected abstract object CompareExchangeImpl(object[] array, int index, object value, object comparand);

            /// <summary>
            /// Type-specific implementation of Atomic.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            sealed class Impl<T> : AtomicArray
                where T : class
            {

                protected override object CompareExchangeImpl(object[] array, int index, object value, object comparand)
                {
                    return Interlocked.CompareExchange(ref ((T[])array)[index], (T)value, (T)comparand);
                }

            }
        }

    }

}
