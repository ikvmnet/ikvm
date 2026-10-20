using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

using IKVM.ByteCode;
using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;

namespace IKVM.Java.Externs.sun.misc
{

    static class Unsafe
    {

        /// <summary>
        /// Holds a reference to the various delegates that facilitate unsafe operations for arrays. References are kept
        /// alive and associated with the element type of the array in a conditional weak table.
        /// </summary>
        class ArrayDelegateRef
        {

            readonly Lazy<Delegate> volatileGetter;
            readonly Lazy<Delegate> volatilePutter;
            readonly Lazy<Delegate> compareExchange;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            public ArrayDelegateRef(RuntimeJavaType type)
            {
                volatileGetter = new Lazy<Delegate>(() => CreateGetArrayVolatileDelegate(type), true);
                volatilePutter = new Lazy<Delegate>(() => CreatePutArrayVolatileDelegate(type), true);
                compareExchange = new Lazy<Delegate>(() => CreateCompareExchangeArrayDelegate(type), true);
            }

            /// <summary>
            /// Gets a delegate capable of implementing the volatile get logic. This value is a <see cref="Func{object, long, object}" />
            /// </summary>
            public Delegate VolatileGetter => volatileGetter.Value;

            /// <summary>
            /// Gets a delegate capable of implemetning the volatile put logic. This value is an <see cref="Action{object, long, object}" />
            /// </summary>
            public Delegate VolatilePutter => volatilePutter.Value;

            /// <summary>
            /// Gets a delegate capable of implemetning the compare and exchange logic. This value is an <see cref="Func{object[], long, object, object, object}" />
            /// </summary>
            public Delegate CompareExchange => compareExchange.Value;

        }

        /// <summary>
        /// Cache of delegates for array operations.
        /// </summary>
        static readonly ConditionalWeakTable<RuntimeJavaType, ArrayDelegateRef> arrayRefCache = new ConditionalWeakTable<RuntimeJavaType, ArrayDelegateRef>();

        /// <summary>
        /// Generic CompareExchange method.
        /// </summary>
        static readonly MethodInfo compareAndSwapArrayMethodInfo = typeof(Unsafe).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(i => i.Name == nameof(CompareAndSwapArray) && i.IsGenericMethodDefinition && i.GetGenericArguments().Length == 1)
            .FirstOrDefault();

        /// <summary>
        /// Emits the appropriate ldind opcode for the given type.
        /// </summary>
        /// <param name="il"></param>
        /// <param name="t"></param>
        /// <exception cref="InvalidOperationException"></exception>
        static void EmitLdind(ILGenerator il, Type t)
        {
            if (t == typeof(bool))
                il.Emit(OpCodes.Ldind_U1);
            else if (t == typeof(byte))
                il.Emit(OpCodes.Ldind_U1);
            else if (t == typeof(char))
                il.Emit(OpCodes.Ldind_U2);
            else if (t == typeof(short))
                il.Emit(OpCodes.Ldind_I2);
            else if (t == typeof(int))
                il.Emit(OpCodes.Ldind_I4);
            else if (t == typeof(long))
                il.Emit(OpCodes.Ldind_I8);
            else if (t == typeof(float))
                il.Emit(OpCodes.Ldind_R4);
            else if (t == typeof(double))
                il.Emit(OpCodes.Ldind_R8);
            else
                il.Emit(OpCodes.Ldind_Ref);
        }

        /// <summary>
        /// Emits the appropriate ldind opcode for the given type.
        /// </summary>
        /// <param name="il"></param>
        /// <param name="t"></param>
        /// <exception cref="InvalidOperationException"></exception>
        static void EmitStind(ILGenerator il, Type t)
        {
            if (t == typeof(bool))
                il.Emit(OpCodes.Stind_I1);
            else if (t == typeof(byte))
                il.Emit(OpCodes.Stind_I1);
            else if (t == typeof(char))
                il.Emit(OpCodes.Stind_I2);
            else if (t == typeof(short))
                il.Emit(OpCodes.Stind_I2);
            else if (t == typeof(int))
                il.Emit(OpCodes.Stind_I4);
            else if (t == typeof(long))
                il.Emit(OpCodes.Stind_I8);
            else if (t == typeof(float))
                il.Emit(OpCodes.Stind_R4);
            else if (t == typeof(double))
                il.Emit(OpCodes.Stind_R8);
            else
                il.Emit(OpCodes.Stind_Ref);
        }

        /// <summary>
        /// Implementation of native method 'registerNatives'.
        /// </summary>
        public static void registerNatives()
        {

        }

        /// <summary>
        /// Implements the logic to get a field by offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static T GetField<T>(object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var f = RuntimeJavaField.FromCookie((IntPtr)offset);
            if (o is RuntimeJavaType w)
            {
                if (w != f.DeclaringType)
                    throw new global::java.lang.IllegalArgumentException();

                return f.UnsafeGetValue<T>(null);
            }

            return f.UnsafeGetValue<T>(o);
#endif
        }

        /// <summary>
        /// Implements the logic to set a field by offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static void PutField<T>(object o, long offset, T value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var f = RuntimeJavaField.FromCookie((IntPtr)offset);
            if (o is RuntimeJavaType w)
            {
                if (w != f.DeclaringType)
                    throw new global::java.lang.IllegalArgumentException();

                f.UnsafeSetValue<T>(null, value);
            }

            f.UnsafeSetValue<T>(o, value);
#endif
        }

        /// <summary>
        /// Implementation of native method 'getObject'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static object getObject(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    RuntimeJavaType w => GetField<object>(null, offset),
                    object[] array => array[offset / IntPtr.Size],
                    object obj => GetField<object>(obj, offset),
                    _ => throw new global::java.lang.IllegalArgumentException(),
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putObject'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putObject(object self, object o, long offset, object x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case object[] array:
                        array[offset / IntPtr.Size] = x;
                        break;
                    case object obj:
                        PutField(obj, offset, x);
                        break;
                    default:
                        throw new global::java.lang.IllegalArgumentException();
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getBoolean'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool getBoolean(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Marshal.ReadByte((IntPtr)offset) != 0,
                    RuntimeJavaType w => GetField<bool>(null, offset),
                    Array array => Buffer.GetByte(array, (int)offset) != 0,
                    _ => GetField<bool>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putBoolean'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putBoolean(object self, object o, long offset, bool x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteByte((IntPtr)offset, x ? (byte)1 : (byte)0);
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        Buffer.SetByte(array, (int)offset, x ? (byte)1 : (byte)0);
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getByte'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte getByte(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Marshal.ReadByte((IntPtr)offset),
                    RuntimeJavaType w => GetField<byte>(null, offset),
                    Array array => Buffer.GetByte(array, (int)offset),
                    _ => GetField<byte>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putByte'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putByte(object self, object o, long offset, byte x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteByte((IntPtr)offset, x);
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        Buffer.SetByte(array, (int)offset, x);
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getShort'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static short getShort(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Marshal.ReadInt16((IntPtr)offset),
                    RuntimeJavaType w => GetField<short>(null, offset),
                    Array array => ReadInt16(array, offset),
                    _ => GetField<short>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putShort'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putShort(object self, object o, long offset, short x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteInt16((IntPtr)offset, x);
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        WriteInt16(array, offset, x);
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getChar'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static char getChar(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => (char)Marshal.ReadInt16((IntPtr)offset),
                    RuntimeJavaType w => GetField<char>(null, offset),
                    Array array => (char)ReadInt16(array, offset),
                    _ => GetField<char>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putChar'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putChar(object self, object o, long offset, char x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteInt16((IntPtr)offset, (short)x);
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        WriteInt16(array, offset, (short)x);
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int getInt(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Marshal.ReadInt32((IntPtr)offset),
                    RuntimeJavaType w => GetField<int>(null, offset),
                    Array array => ReadInt32(array, offset),
                    _ => GetField<int>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putInt(object self, object o, long offset, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteInt32((IntPtr)offset, x);
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        WriteInt32(array, offset, x);
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static long getLong(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Marshal.ReadInt64((IntPtr)offset),
                    RuntimeJavaType w => GetField<long>(null, offset),
                    Array array => ReadInt64(array, offset),
                    _ => GetField<long>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putLong(object self, object o, long offset, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteInt64((IntPtr)offset, x);
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        WriteInt64(array, offset, x);
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getFloat'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static float getFloat(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => global::java.lang.Float.intBitsToFloat(Marshal.ReadInt32((IntPtr)offset)),
                    RuntimeJavaType w => GetField<float>(null, offset),
                    Array array => global::java.lang.Float.intBitsToFloat(ReadInt32(array, offset)),
                    _ => GetField<float>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putFloat'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putFloat(object self, object o, long offset, float x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteInt32((IntPtr)offset, global::java.lang.Float.floatToRawIntBits(x));
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        WriteInt32(array, offset, global::java.lang.Float.floatToRawIntBits(x));
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getDouble'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static double getDouble(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => global::java.lang.Double.longBitsToDouble(Marshal.ReadInt64((IntPtr)offset)),
                    RuntimeJavaType w => GetField<double>(null, offset),
                    Array array => global::java.lang.Double.longBitsToDouble(ReadInt64(array, offset)),
                    _ => GetField<double>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putDouble'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putDouble(object self, object o, long offset, double x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Marshal.WriteInt64((IntPtr)offset, global::java.lang.Double.doubleToRawLongBits(x));
                        break;
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case Array array:
                        WriteInt64(array, offset, global::java.lang.Double.doubleToRawLongBits(x));
                        break;
                    default:
                        PutField(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getByte'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static byte getByte(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Marshal.ReadByte((IntPtr)address);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putByte'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putByte(object self, long address, byte x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Marshal.WriteByte((IntPtr)address, x);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getShort'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static short getShort(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Marshal.ReadInt16((IntPtr)address);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putShort'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putShort(object self, long address, short x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Marshal.WriteInt16((IntPtr)address, x);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getChar'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static char getChar(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return (char)Marshal.ReadInt16((IntPtr)address);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putChar'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putChar(object self, long address, char x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Marshal.WriteInt16((IntPtr)address, (short)x);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int getInt(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Marshal.ReadInt32((IntPtr)address);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putInt(object self, long address, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Marshal.WriteInt32((IntPtr)address, x);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static long getLong(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Marshal.ReadInt64((IntPtr)address);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putLong(object self, long address, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Marshal.WriteInt64((IntPtr)address, x);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getFloat'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static float getFloat(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return global::java.lang.Float.intBitsToFloat(getInt(self, address));
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putFloat'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putFloat(object self, long address, float x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            putInt(self, address, global::java.lang.Float.floatToIntBits(x));
#endif
        }

        /// <summary>
        /// Implementation of native method 'getDouble'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static double getDouble(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return global::java.lang.Double.longBitsToDouble(getLong(self, address));
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putDouble'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putDouble(object self, long address, double x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            putLong(self, address, global::java.lang.Double.doubleToLongBits(x));
#endif
        }

        /// <summary>
        /// Implementation of native method 'getAddress'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static long getAddress(object self, long address)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Marshal.ReadIntPtr((IntPtr)address).ToInt64();
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putAddress'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putAddress(object self, long address, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Marshal.WriteIntPtr((IntPtr)address, (IntPtr)x);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'allocateMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.OutOfMemoryError"></exception>
        public static unsafe long allocateMemory(object self, long bytes)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (bytes == 0)
                    return 0;

                var len = (IntPtr)bytes + sizeof(IntPtr);
                var ptr = Marshal.AllocHGlobal(len);
                Marshal.WriteIntPtr(ptr, len);
                System.GC.AddMemoryPressure((long)len);
                return (long)ptr + sizeof(IntPtr);
            }
            catch (OutOfMemoryException e)
            {
                throw new global::java.lang.OutOfMemoryError(e.Message).initCause(e);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static unsafe long reallocateMemory(object self, long address, long bytes)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (bytes == 0)
                {
                    freeMemory(self, address);
                    return 0;
                }

                // remove memory pressure
                var ptr = (IntPtr)address - sizeof(IntPtr);
                var len = Marshal.ReadIntPtr(ptr);
                System.GC.RemoveMemoryPressure((long)len);

                // reallocate and add memory pressure
                len = (IntPtr)bytes + sizeof(IntPtr);
                ptr = Marshal.ReAllocHGlobal(ptr, len);
                Marshal.WriteIntPtr(ptr, len);
                System.GC.AddMemoryPressure((long)len);
                return (long)ptr + sizeof(IntPtr);
            }
            catch (OutOfMemoryException e)
            {
                throw new global::java.lang.OutOfMemoryError(e.Message).initCause(e);
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static unsafe void setMemory(object self, object o, long offset, long bytes, byte value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (o == null)
                    new Span<byte>((void*)(IntPtr)offset, (int)bytes).Fill(value);
                else if (o is byte[] array)
                    ((Span<byte>)array).Slice((int)offset, (int)bytes).Fill(value);
                else if (o is Array array2)
                {
                    GCHandle h = new();

                    try
                    {
                        h = GCHandle.Alloc(array2, GCHandleType.Pinned);
                        new Span<byte>((byte*)h.AddrOfPinnedObject(), Buffer.ByteLength(array2)).Slice((int)offset, (int)bytes).Fill(value);
                    }
                    finally
                    {
                        if (h.IsAllocated)
                            h.Free();
                    }
                }
                else
                    throw new global::java.lang.IllegalArgumentException();
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
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
        public static unsafe void copyMemory(object self, object srcBase, long srcOffset, object destBase, long destOffset, long bytes)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (srcBase == null)
                {
                    if (destBase is byte[] byteArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, byteArray, (int)destOffset, (int)bytes);
                    }
                    else if (destBase is bool[])
                    {
                        var tmp = new byte[(int)bytes];
                        copyMemory(self, srcBase, srcOffset, tmp, 0, bytes);
                        copyMemory(self, tmp, 0, destBase, destOffset, bytes);
                    }
                    else if (destBase is short[] shortArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, shortArray, (int)(destOffset >> 1), (int)(bytes >> 1));
                    }
                    else if (destBase is char[] charArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, charArray, (int)(destOffset >> 1), (int)(bytes >> 1));
                    }
                    else if (destBase is int[] intArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, intArray, (int)(destOffset >> 2), (int)(bytes >> 2));
                    }
                    else if (destBase is float[] floatArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, floatArray, (int)(destOffset >> 2), (int)(bytes >> 2));
                    }
                    else if (destBase is long[] longArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, longArray, (int)(destOffset >> 3), (int)(bytes >> 3));
                    }
                    else if (destBase is double[] doubleArray)
                    {
                        Marshal.Copy((IntPtr)srcOffset, doubleArray, (int)(destOffset >> 3), (int)(bytes >> 3));
                    }
                    else if (destBase == null)
                    {
                        Buffer.MemoryCopy((void*)(IntPtr)srcOffset, (void*)(IntPtr)destOffset, long.MaxValue, bytes);
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
                        Marshal.Copy(byteArray, (int)srcOffset, (IntPtr)destOffset, (int)bytes);
                    }
                    else if (srcBase is bool[])
                    {
                        var tmp = new byte[(int)bytes];
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
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'freeMemory'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        public static unsafe void freeMemory(object self, long address)
        {
            var adr = (nint)address;
            if (adr == 0)
                return;

            var ptr = adr - sizeof(nint);
            var len = Marshal.ReadIntPtr(ptr);
            Marshal.FreeHGlobal(ptr);
            System.GC.RemoveMemoryPressure((long)len);
        }

        /// <summary>
        /// Implementation of native method 'staticFieldBase'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static object staticFieldBase(object self, global::java.lang.reflect.Field f)
        {
            var w = RuntimeJavaField.FromField(f);
            if (w.IsStatic == false)
                throw new global::java.lang.IllegalArgumentException();

            return w.DeclaringType;
        }

        /// <summary>
        /// Implementation of native method 'staticFieldOffset'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long staticFieldOffset(object self, global::java.lang.reflect.Field f)
        {
            var w = RuntimeJavaField.FromField(f);
            if (w.IsStatic == false)
                throw new global::java.lang.IllegalArgumentException();

            return (long)w.Cookie;
        }

        /// <summary>
        /// Implementation of native method 'objectFieldOffset'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long objectFieldOffset(object self, global::java.lang.reflect.Field f)
        {
            var w = RuntimeJavaField.FromField(f);
            if (w.IsStatic)
                throw new global::java.lang.IllegalArgumentException();

            return (long)w.Cookie;
        }

        /// <summary>
        /// Implementation of native method 'shouldBeInitialized'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool shouldBeInitialized(object self, global::java.lang.Class c)
        {
            return RuntimeJavaType.FromClass(c).HasStaticInitializer;
        }

        /// <summary>
        /// Implementation of native method 'ensureClassInitialized'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="c"></param>
        public static void ensureClassInitialized(object self, global::java.lang.Class c)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var tw = RuntimeJavaType.FromClass(c);
            if (tw.IsArray == false)
            {
                try
                {
                    tw.Finish();
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }

                tw.RunClassInit();
            }
#endif
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
        /// Determines the index scale for the specified array type.
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        static int ArrayIndexScale(RuntimeJavaType tw)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var context = tw.Context;

            var et = tw.ElementTypeWrapper;
            if (et == context.PrimitiveJavaTypeFactory.BYTE || et == context.PrimitiveJavaTypeFactory.BOOLEAN)
                return 1;
            else if (et == context.PrimitiveJavaTypeFactory.CHAR || et == context.PrimitiveJavaTypeFactory.SHORT)
                return 2;
            else if (et == context.PrimitiveJavaTypeFactory.INT || et == context.PrimitiveJavaTypeFactory.FLOAT)
                return 4;
            else if (et == context.PrimitiveJavaTypeFactory.LONG || et == context.PrimitiveJavaTypeFactory.DOUBLE)
                return 8;
            else if (et.IsPrimitive == false && et.IsNonPrimitiveValueType)
                return Marshal.SizeOf(et.TypeAsTBD);
            else if (et.IsPrimitive == false && et.IsNonPrimitiveValueType == false)
                return IntPtr.Size;
            else
                return 1;
#endif
        }

        /// <summary>
        /// Implementation of native method 'arrayIndexScale'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="arrayClass"></param>
        /// <returns></returns>
        public static int arrayIndexScale(object self, global::java.lang.Class arrayClass)
        {
            return ArrayIndexScale(RuntimeJavaType.FromClass(arrayClass));
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
                if (cpPatches is not null)
                {
                    var temp = new object[cpPatches.Length];

                    for (int i = 0; i < cpPatches.Length; i++)
                    {
                        temp[i] = cpPatches[i] switch
                        {
                            global::java.lang.Class clazz => RuntimeJavaType.FromClass(clazz),
                            global::java.lang.Integer integer_ => integer_.intValue(),
                            global::java.lang.Long long_ => long_.longValue(),
                            global::java.lang.Float float_ => float_.floatValue(),
                            global::java.lang.Double double_ => double_.doubleValue(),
                            _ => cpPatches[i],
                        };
                    }

                    cpPatches = temp;
                }

                var tw = RuntimeJavaType.FromClass(hostClass);
                var cl = tw.ClassLoader;
                var cf = new ClassFile(JVM.Context, ReadClass(data), "<Unknown>", cl.ClassFileParseOptions, cpPatches ?? []);

                // if this happens, the OpenJDK is probably trying to load an OpenJDK class file as a resource,
                // make sure the build process includes the original class file as a resource in that case
                if (cf.IKVMAssemblyAttribute != null)
                    throw new global::java.lang.ClassFormatError("Trying to define anonymous class based on stub class: " + cf.Name);

                return cl.GetTypeWrapperFactory().DefineClassImpl(null, tw, cf, cl, hostClass.pd).ClassObject;
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

#if FIRST_PASS == false

        /// <summary>
        /// Attempts to read the class, handling exceptions.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        static IKVM.ByteCode.Decoding.ClassFile ReadClass(byte[] buffer)
        {
            try
            {
                return IKVM.ByteCode.Decoding.ClassFile.Read(buffer);
            }
            catch (InvalidClassMagicException)
            {
                throw new global::java.lang.ClassFormatError("Incompatible magic value");
            }
            catch (ByteCodeException e)
            {
                throw new global::java.lang.ClassFormatError(e.Message);
            }
        }

#endif

        /// <summary>
        /// Implementation of native method 'allocateInstance'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public static object allocateInstance(object self, global::java.lang.Class cls)
        {
            var wrapper = RuntimeJavaType.FromClass(cls);
            try
            {
                wrapper.Finish();
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }

#if NETFRAMEWORK
            return FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
#else
            return RuntimeHelpers.GetUninitializedObject(wrapper.TypeAsBaseType);
#endif
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
        /// Implements the logic to get a field by offset using volatile.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static T GetFieldVolatile<T>(object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var f = RuntimeJavaField.FromCookie((IntPtr)offset);
            if (o is RuntimeJavaType w)
            {
                if (w != f.DeclaringType)
                    throw new global::java.lang.IllegalArgumentException();

                return f.UnsafeVolatileGet<T>(null);
            }

            return f.UnsafeVolatileGet<T>(o);
#endif
        }

        /// <summary>
        /// Implements the logic to set a field by offset using volatile.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static void PutFieldVolatile<T>(object o, long offset, T value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var f = RuntimeJavaField.FromCookie((IntPtr)offset);
            if (o is RuntimeJavaType w)
            {
                if (w != f.DeclaringType)
                    throw new global::java.lang.IllegalArgumentException();

                f.UnsafeVolatileSet<T>(null, value);
            }

            f.UnsafeVolatileSet<T>(o, value);
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing an index of a specific type.
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        static Delegate CreateGetArrayVolatileDelegate(RuntimeJavaType tw)
        {
            var et = tw.IsPrimitive ? tw.TypeAsTBD : typeof(object);
            var dm = DynamicMethodUtil.Create($"__<UnsafeGetArrayVolatile>__{tw.Name.Replace(".", "_")}", tw.TypeAsTBD, true, et, new[] { typeof(object[]), typeof(long) });
            var il = dm.GetILGenerator();

            // load reference to element
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Conv_Ovf_I);
            il.Emit(OpCodes.Ldc_I4, ArrayIndexScale(tw.MakeArrayType(1)));
            il.Emit(OpCodes.Div);
            il.Emit(OpCodes.Conv_Ovf_I);
            il.Emit(OpCodes.Ldelema, tw.TypeAsLocalOrStackType);

            if (tw.IsWidePrimitive == false)
            {
                il.Emit(OpCodes.Volatile);
                EmitLdind(il, tw.TypeAsLocalOrStackType);
            }
            else
            {
                // Java volatile semantics require atomicity, CLR volatile semantics do not
                var mi = typeof(Unsafe).GetMethod(nameof(InterlockedRead), BindingFlags.NonPublic | BindingFlags.Static, null, new[] { tw.TypeAsTBD.MakeByRefType() }, null);
                il.Emit(OpCodes.Call, mi);
            }

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Func<,,>).MakeGenericType(typeof(object[]), typeof(long), et));
        }

        /// <summary>
        /// Implements the logic to get an object array by offset using volatile.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static object GetArrayObjectVolatile(object[] array, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((Func<object[], long, object>)arrayRefCache.GetValue(JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(array.GetType().GetElementType()), _ => new ArrayDelegateRef(_)).VolatileGetter)(array, offset);
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing an index of a specific type.
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        static Delegate CreatePutArrayVolatileDelegate(RuntimeJavaType tw)
        {
            var et = tw.IsPrimitive ? tw.TypeAsTBD : typeof(object);
            var dm = DynamicMethodUtil.Create($"__<UnsafePutArrayVolatile>__{tw.Name.Replace(".", "_")}", tw.TypeAsTBD, true, typeof(void), new[] { typeof(object[]), typeof(long), et });
            var il = dm.GetILGenerator();

            // load reference to element
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Conv_Ovf_I);
            il.Emit(OpCodes.Ldc_I4, ArrayIndexScale(tw.MakeArrayType(1)));
            il.Emit(OpCodes.Div);
            il.Emit(OpCodes.Conv_Ovf_I);
            il.Emit(OpCodes.Ldelema, tw.TypeAsLocalOrStackType);

            il.Emit(OpCodes.Ldarg_2);

            if (tw.IsWidePrimitive == false)
            {
                il.Emit(OpCodes.Volatile);
                EmitStind(il, tw.TypeAsLocalOrStackType);
            }
            else
            {
                // Java volatile semantics require atomicity, CLR volatile semantics do not
                var mi = typeof(Interlocked).GetMethod(nameof(Interlocked.Exchange), new[] { tw.TypeAsTBD.MakeByRefType() });
                il.Emit(OpCodes.Call, mi);
            }

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Action<,,>).MakeGenericType(typeof(object[]), typeof(long), et));
        }

        /// <summary>
        /// Implements the logic to set an object array by offset using volatile.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static void PutArrayObjectVolatile(object[] array, long offset, object value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            ((Action<object[], long, object>)arrayRefCache.GetValue(JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(array.GetType().GetElementType()), _ => new ArrayDelegateRef(_)).VolatilePutter)(array, offset, value);
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
            try
            {
                return o switch
                {
                    RuntimeJavaType w => GetField<object>(null, offset),
                    object[] array when array.GetType() == typeof(object[]) => Volatile.Read(ref array[offset / IntPtr.Size]),
                    object[] array => GetArrayObjectVolatile(array, offset),
                    object obj => GetFieldVolatile<object>(obj, offset),
                    _ => throw new global::java.lang.IllegalArgumentException(),
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
            try
            {
                switch (o)
                {
                    case RuntimeJavaType w:
                        PutField(w, offset, x);
                        break;
                    case object[] array when array.GetType() == typeof(object[]):
                        Volatile.Write(ref array[offset / IntPtr.Size], x);
                        break;
                    case object[] array:
                        PutArrayObjectVolatile(array, offset, x);
                        break;
                    case object obj:
                        PutFieldVolatile(o, offset, x);
                        break;
                    default:
                        throw new global::java.lang.IllegalArgumentException();
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static unsafe int getIntVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Volatile.Read(ref System.Runtime.CompilerServices.Unsafe.AsRef<int>((void*)(IntPtr)offset)),
                    RuntimeJavaType w => GetFieldVolatile<int>(null, offset),
                    Array array => ReadInt32Volatile(array, offset),
                    _ => GetFieldVolatile<int>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static unsafe void putIntVolatile(object self, object o, long offset, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<int>((void*)(IntPtr)offset), x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteInt32Volatile(array, offset, x);
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static unsafe bool getBooleanVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Volatile.Read(ref System.Runtime.CompilerServices.Unsafe.AsRef<byte>((void*)(IntPtr)offset)) != 0,
                    RuntimeJavaType w => GetFieldVolatile<bool>(null, offset),
                    Array array => ReadByteVolatile(array, offset) != 0,
                    _ => GetFieldVolatile<bool>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putBooleanVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putBooleanVolatile(object self, object o, long offset, bool x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<byte>((void*)(IntPtr)offset), x ? (byte)1 : (byte)0);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteByteVolatile(array, offset, x ? (byte)1 : (byte)0);
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static unsafe byte getByteVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    null => Volatile.Read(ref System.Runtime.CompilerServices.Unsafe.AsRef<byte>((void*)(IntPtr)offset)),
                    RuntimeJavaType w => GetFieldVolatile<byte>(null, offset),
                    Array array => ReadByteVolatile(array, offset),
                    _ => GetFieldVolatile<byte>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putByteVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putByteVolatile(object self, object o, long offset, byte x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<byte>((void*)(IntPtr)offset), x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteByteVolatile(array, offset, x);
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    Array array => ReadInt16Volatile(array, offset),
                    _ => GetFieldVolatile<short>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putShortVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putShortVolatile(object self, object o, long offset, short x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<short>((void*)(IntPtr)offset), x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteInt16Volatile(array, offset, x);
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    Array array => (char)ReadInt16Volatile(array, offset),
                    _ => GetFieldVolatile<char>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putCharVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putCharVolatile(object self, object o, long offset, char x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<short>((void*)(IntPtr)offset), (short)x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteInt16Volatile(array, offset, (short)x);
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
            try
            {
                return o switch
                {
                    Array array => ReadInt64Volatile(array, offset),
                    _ => GetFieldVolatile<long>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putLongVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putLongVolatile(object self, object o, long offset, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<long>((void*)(IntPtr)offset), x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteInt64Volatile(array, offset, x);
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getFloatVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        public static float getFloatVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    Array array => global::java.lang.Float.intBitsToFloat(ReadInt32Volatile(array, offset)),
                    _ => GetFieldVolatile<float>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putFloatVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putFloatVolatile(object self, object o, long offset, float x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<float>((void*)(IntPtr)offset), x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteInt32Volatile(array, offset, global::java.lang.Float.floatToRawIntBits(x));
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'getDoubleVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        public static double getDoubleVolatile(object self, object o, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    Array array => global::java.lang.Double.longBitsToDouble(ReadInt64Volatile(array, offset)),
                    _ => GetFieldVolatile<double>(o, offset)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implementation of native method 'putDoubleVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static unsafe void putDoubleVolatile(object self, object o, long offset, double x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                switch (o)
                {
                    case null:
                        Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<double>((void*)(IntPtr)offset), x);
                        break;
                    case RuntimeJavaType w:
                        PutFieldVolatile(w, offset, x);
                        break;
                    case Array array:
                        WriteInt64Volatile(array, offset, global::java.lang.Double.doubleToRawLongBits(x));
                        break;
                    default:
                        PutFieldVolatile(o, offset, x);
                        break;
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
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

        /// <summary>
        /// Implements the logic to compare and swap an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static bool CompareAndSwapField<T>(object o, long offset, T expected, T value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return RuntimeJavaField.FromCookie((IntPtr)offset).UnsafeCompareAndSwap(o, expected, value);
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing an index of a specific type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static Delegate CreateCompareExchangeArrayDelegate(RuntimeJavaType tw)
        {
            var p = Expression.Parameter(typeof(object[]));
            var i = Expression.Parameter(typeof(long));
            var v = Expression.Parameter(typeof(object));
            var e = Expression.Parameter(typeof(object));
            return Expression.Lambda<Func<object[], long, object, object, object>>(
                Expression.Call(
                    compareAndSwapArrayMethodInfo.MakeGenericMethod(tw.TypeAsTBD),
                    Expression.Convert(p, tw.MakeArrayType(1).TypeAsTBD),
                    i,
                    Expression.Convert(v, tw.TypeAsTBD),
                    Expression.Convert(e, tw.TypeAsTBD)),
                p, i, v, e)
                .Compile();
        }

        /// <summary>
        /// Implements CompareAndSwap for a typed array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="comparand"></param>
        /// <returns></returns>
        static object CompareAndSwapArray<T>(T[] o, long offset, T value, T comparand)
            where T : class
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return Interlocked.CompareExchange<T>(ref o[offset / ArrayIndexScale(JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(o.GetType()))], value, comparand);
#endif
        }

        /// <summary>
        /// Implements the logic to compare and swap an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static object CompareAndSwapObjectArray(object[] o, long offset, object value, object expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ((Func<object[], long, object, object, object>)arrayRefCache.GetValue(JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(o.GetType().GetElementType()), _ => new ArrayDelegateRef(_)).CompareExchange)(o, offset, value, expected);
#endif
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
        public static bool compareAndSwapObject(object self, object o, long offset, object expected, object x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return o switch
                {
                    object[] array when array.GetType() == typeof(object[]) => Interlocked.CompareExchange(ref array[offset / IntPtr.Size], x, expected) == expected,
                    object[] array => CompareAndSwapObjectArray(array, offset, x, expected) == expected,
                    _ => CompareAndSwapField(o, offset, expected, x)
                };
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static bool compareAndSwapInt(object self, object o, long offset, int expected, int x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (o is int[] array && (offset % sizeof(int)) == 0)
                {
                    return Interlocked.CompareExchange(ref array[offset / sizeof(int)], x, expected) == expected;
                }
                else if (o is Array array1 && (offset % sizeof(int)) == 0)
                {
                    return CompareExchangeInt32(array1, offset, x, expected) == expected;
                }
                else if (o is Array array2)
                {
                    return CompareExchangeInt32Unaligned(array2, offset, x, expected) == expected;
                }
                else
                {
                    return CompareAndSwapField(o, offset, expected, x);
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
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
        public static bool compareAndSwapLong(object self, object o, long offset, long expected, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                if (o is long[] array && (offset % sizeof(long)) == 0)
                {
                    return Interlocked.CompareExchange(ref array[offset / sizeof(long)], x, expected) == expected;
                }
                else if (o is Array array1 && (offset % sizeof(long)) == 0)
                {
                    return CompareExchangeInt64(array1, offset, x, expected) == expected;
                }
                else if (o is Array array2)
                {
                    return CompareExchangeInt64Unaligned(array2, offset, x, expected) == expected;
                }
                else
                {
                    return CompareAndSwapField(o, offset, expected, x);
                }
            }
            catch (Exception e) when (e is not global::java.lang.Throwable)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        const int PARK_STATE_RUNNING = 0;
        const int PARK_STATE_PERMIT = 1;
        const int PARK_STATE_PARKED = 2;

        /// <summary>
        /// Implements the native method 'unpark'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="thread"></param>
        public static void unpark(object self, object thread)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var currentThread = (global::java.lang.Thread)thread;
            if (currentThread == null)
                throw new global::java.lang.IllegalStateException();

            if (currentThread != null)
            {
                if (Interlocked.CompareExchange(ref currentThread.parkState, PARK_STATE_PERMIT, PARK_STATE_RUNNING) == PARK_STATE_PARKED)
                {
                    if (Interlocked.CompareExchange(ref currentThread.parkState, PARK_STATE_RUNNING, PARK_STATE_PARKED) == PARK_STATE_PARKED)
                    {
                        // initialize lock
                        Interlocked.CompareExchange(ref currentThread.parkLock, new global::java.lang.Object(), null);

                        // thread is currently blocking, so we have to release it
                        lock (currentThread.parkLock)
                            Monitor.Pulse(currentThread.parkLock);
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'park'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isAbsolute"></param>
        /// <param name="time"></param>
        public static void park(object self, bool isAbsolute, long time)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var currentThread = global::java.lang.Thread.currentThread();
            if (currentThread == null)
                throw new global::java.lang.IllegalStateException();

            if (Interlocked.CompareExchange(ref currentThread.parkState, PARK_STATE_RUNNING, PARK_STATE_PERMIT) == PARK_STATE_PERMIT)
                return;

            // initialize lock
            Interlocked.CompareExchange(ref currentThread.parkLock, new global::java.lang.Object(), null);

            // lock thread
            lock (currentThread.parkLock)
            {
                if (Interlocked.CompareExchange(ref currentThread.parkState, PARK_STATE_PARKED, PARK_STATE_RUNNING) == PARK_STATE_PERMIT)
                {
                    Interlocked.CompareExchange(ref currentThread.parkState, PARK_STATE_RUNNING, PARK_STATE_PERMIT);
                    return;
                }

                if (isAbsolute)
                {
                    time *= 1000000;
                    time -= global::java.lang.System.currentTimeMillis() * 1000000;
                }

                if (time >= 0)
                {
                    try
                    {
                        ((global::java.lang.Object)currentThread.parkLock).wait(time / 1000000, (int)(time % 1000000));
                    }
                    catch (global::java.lang.InterruptedException)
                    {
                        currentThread.interrupt();
                    }
                }

                Interlocked.CompareExchange(ref currentThread.parkState, PARK_STATE_RUNNING, PARK_STATE_PARKED);
            }
#endif
        }

        public static int getLoadAverage(object self, double[] loadavg, int nelems)
        {
            return -1;
        }

        public static void loadFence(object self)
        {
            Thread.MemoryBarrier();
        }

        public static void storeFence(object self)
        {
            Thread.MemoryBarrier();
        }

        public static void fullFence(object self)
        {
            Thread.MemoryBarrier();
        }

        /// <summary>
        /// Reads an <see cref="byte"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe byte ReadByte(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var v = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<byte>((byte*)h.AddrOfPinnedObject() + offset);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="byte"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe byte ReadByteVolatile(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                ref var r = ref System.Runtime.CompilerServices.Unsafe.AsRef<byte>((byte*)h.AddrOfPinnedObject() + offset);
                var v = Volatile.Read(ref r);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="short"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe short ReadInt16(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var v = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<short>((byte*)h.AddrOfPinnedObject() + offset);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="short"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe short ReadInt16Volatile(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                ref var r = ref System.Runtime.CompilerServices.Unsafe.AsRef<short>((byte*)h.AddrOfPinnedObject() + offset);
                var v = Volatile.Read(ref r);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="int"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe int ReadInt32(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var v = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<int>((byte*)h.AddrOfPinnedObject() + offset);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="int"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe int ReadInt32Volatile(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                ref var r = ref System.Runtime.CompilerServices.Unsafe.AsRef<int>((byte*)h.AddrOfPinnedObject() + offset);
                var v = Volatile.Read(ref r);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="long"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe long ReadInt64(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var v = System.Runtime.CompilerServices.Unsafe.ReadUnaligned<long>((byte*)h.AddrOfPinnedObject() + offset);
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Reads an <see cref="long"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe long ReadInt64Volatile(Array obj, long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                ref var r = ref System.Runtime.CompilerServices.Unsafe.AsRef<long>((byte*)h.AddrOfPinnedObject() + offset);
                var v = Interlocked.Read(ref r); // java considers volatile to be atomic
                return v;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="byte"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteByte(Array obj, long offset, byte value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                System.Runtime.CompilerServices.Unsafe.WriteUnaligned((byte*)h.AddrOfPinnedObject() + offset, value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="byte"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteByteVolatile(Array obj, long offset, byte value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<byte>((byte*)h.AddrOfPinnedObject() + offset), value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="short"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt16(Array obj, long offset, short value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                System.Runtime.CompilerServices.Unsafe.WriteUnaligned((byte*)h.AddrOfPinnedObject() + offset, value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="short"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt16Volatile(Array obj, long offset, short value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<short>((byte*)h.AddrOfPinnedObject() + offset), value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="int"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt32(Array obj, long offset, int value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                System.Runtime.CompilerServices.Unsafe.WriteUnaligned((byte*)h.AddrOfPinnedObject() + offset, value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="int"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt32Volatile(Array obj, long offset, int value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.AsRef<int>((byte*)h.AddrOfPinnedObject() + offset), value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="long"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt64(Array obj, long offset, long value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                System.Runtime.CompilerServices.Unsafe.WriteUnaligned((byte*)h.AddrOfPinnedObject() + offset, value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Writes a <see cref="long"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt64Volatile(Array obj, long offset, long value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                Interlocked.Exchange(ref System.Runtime.CompilerServices.Unsafe.AsRef<long>((byte*)h.AddrOfPinnedObject() + offset), value);
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Executes CompareExchange against the specified byte offset with the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        static unsafe int CompareExchangeInt32(Array obj, long offset, int value, int expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var r = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.AsRef<int>((byte*)h.AddrOfPinnedObject() + offset), value, expected);
                return r;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Executes CompareExchange against the specified byte offset with the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        static unsafe int CompareExchangeInt32Unaligned(Array obj, long offset, int value, int expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var r = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.AsRef<int>((byte*)h.AddrOfPinnedObject() + offset), value, expected);
                return r;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Executes CompareExchange against the specified byte offset with the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        static unsafe long CompareExchangeInt64(Array obj, long offset, long value, long expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var r = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.AsRef<long>((byte*)h.AddrOfPinnedObject() + offset), value, expected);
                return r;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Executes CompareExchange against the specified byte offset with the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        static unsafe long CompareExchangeInt64Unaligned(Array obj, long offset, long value, long expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var r = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.AsRef<long>((byte*)h.AddrOfPinnedObject() + offset), value, expected);
                return r;
            }
            finally
            {
                if (h.IsAllocated)
                    h.Free();
            }
#endif
        }

        /// <summary>
        /// Implements an Interlocked.Read method for long.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static long InterlockedRead(ref long location)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.Read(ref location);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements an Interlocked.Read method for double.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static double InterlockedRead(ref double location)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, 0, 0);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements an Interlocked.Read method for long.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static void InterlockedWrite(ref long location, long value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Interlocked.Exchange(ref location, value);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements an Interlocked.Read method for double.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static void InterlockedWrite(ref double location, double value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Interlocked.Exchange(ref location, value);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

    }

}