using System;
using System.Linq;
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

        /// <summary>
        /// Holds a reference to the various delegates that facilitate unsafe operations. References are kept alive and
        /// associated with a <see cref="FieldInfo"/> by a conditional weak table. When the <see cref="FieldInfo"/>
        /// becomes eligible for collection, so does the <see cref="FieldDelegateRef"/>. Each instance maintains a
        /// <see cref="GCHandle"/> that provides a fixed <see cref="IntPtr"/> sized value which can be handed off as an
        /// offset value.
        /// </summary>
        class FieldDelegateRef
        {

            readonly FieldWrapper field;
            readonly Lazy<Delegate> getter;
            readonly Lazy<Delegate> putter;
            readonly Lazy<Delegate> volatileGetter;
            readonly Lazy<Delegate> volatilePutter;
            readonly Lazy<Delegate> compareExchange;
            readonly GCHandle handle;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="field"></param>
            public FieldDelegateRef(FieldWrapper field)
            {
                this.field = field ?? throw new ArgumentNullException(nameof(field));

                getter = new Lazy<Delegate>(() => CreateGetFieldDelegate(field), true);
                putter = new Lazy<Delegate>(() => CreatePutFieldDelegate(field), true);
                volatileGetter = new Lazy<Delegate>(() => CreateGetFieldVolatileDelegate(field), true);
                volatilePutter = new Lazy<Delegate>(() => CreatePutFieldVolatileDelegate(field), true);
                compareExchange = new Lazy<Delegate>(() => CreateCompareExchangeFieldDelegate(field), true);
                handle = GCHandle.Alloc(this, GCHandleType.WeakTrackResurrection);
            }

            /// <summary>
            /// Gets a delegate capable of implementing the get logic. This value is a <see cref="Func{object, TField}" />
            /// </summary>
            public Delegate Getter => getter.Value;

            /// <summary>
            /// Gets a delegate capable of implementing the put logic. This value is an <see cref="Action{object, TField}" />
            /// </summary>
            public Delegate Putter => putter.Value;

            /// <summary>
            /// Gets a delegate capable of implementing the volatile get logic. This value is a <see cref="Func{object, TField}" />
            /// </summary>
            public Delegate VolatileGetter => volatileGetter.Value;

            /// <summary>
            /// Gets a delegate capable of implemetning the volatile put logic. This value is an <see cref="Action{object, TField}" />
            /// </summary>
            public Delegate VolatilePutter => volatilePutter.Value;

            /// <summary>
            /// Gets a delegate capable of implemetning the volatile put logic. This value is an <see cref="Func{object, TField, TField, TField}" />
            /// </summary>
            public Delegate CompareExchange => compareExchange.Value;

            /// <summary>
            /// Maintains a handle that can serve as the 'offset' value.
            /// </summary>
            public GCHandle Handle => handle;

        }

        /// <summary>
        /// Holds a reference to the various delegates that facilitate unsafe operations for arrays. References are kept
        /// alive and associated with the element type of the array in a conditional weak table.
        /// </summary>
        class ArrayDelegateRef
        {

            readonly TypeWrapper type;
            readonly Lazy<Delegate> volatileGetter;
            readonly Lazy<Delegate> volatilePutter;
            readonly Lazy<Delegate> compareExchange;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            public ArrayDelegateRef(TypeWrapper type)
            {
                this.type = type ?? throw new ArgumentNullException(nameof(type));

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

        // TODO: NET6+ replace with DependentHandle
        static readonly ConditionalWeakTable<FieldWrapper, FieldDelegateRef> fieldRefCache = new ConditionalWeakTable<FieldWrapper, FieldDelegateRef>();

        /// <summary>
        /// Cache of delegates for array operations.
        /// </summary>
        static readonly ConditionalWeakTable<TypeWrapper, ArrayDelegateRef> arrayRefCache = new ConditionalWeakTable<TypeWrapper, ArrayDelegateRef>();

        /// <summary>
        /// Generic CompareExchange method.
        /// </summary>
        static readonly MethodInfo compareExchangeMethodInfo = typeof(Unsafe).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(i => i.Name == nameof(CompareExchange) && i.IsGenericMethodDefinition && i.GetGenericArguments().Length == 1)
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
        /// Creates a delegate capable of accessing a field of a specific type.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        static Delegate CreateGetFieldDelegate(FieldWrapper fw)
        {
            fw.ResolveField();
            var ft = fw.FieldTypeWrapper.IsPrimitive ? fw.FieldTypeWrapper.TypeAsTBD : typeof(object);
            var dm = new DynamicMethod($"__<UnsafeGetField>__{fw.DeclaringType.Name.Replace(".", "_")}__{fw.Name}", ft, new[] { typeof(object) }, fw.DeclaringType.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            if (fw.IsStatic && fw.IsFinal)
            {
                // we obtain a reference to the field and do an indirect load here to avoid JIT optimizations that inline static readonly fields
                il.Emit(OpCodes.Ldsflda, fw.GetField());
                EmitLdind(il, ft);
            }
            else if (fw.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, fw.GetField());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, fw.GetField());
            }

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(object), ft));
        }

        /// <summary>
        /// Creates a delegate capable of accessing a field of a specific type.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        static Delegate CreatePutFieldDelegate(FieldWrapper fw)
        {
            fw.ResolveField();
            var ft = fw.FieldTypeWrapper.IsPrimitive ? fw.FieldTypeWrapper.TypeAsTBD : typeof(object);
            var dm = new DynamicMethod($"__<UnsafePutField>__{fw.DeclaringType.Name.Replace(".", "_")}__{fw.Name}", typeof(void), new[] { typeof(object), ft }, fw.DeclaringType.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            if (fw.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stsfld, fw.GetField());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, fw.GetField());
            }

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(object), ft));
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
            try
            {
                return ((Func<object, T>)((FieldDelegateRef)GCHandle.FromIntPtr((IntPtr)offset).Target).Getter)(o);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements the logic to set a field by offset.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static void PutField<TField>(object o, long offset, TField value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                ((Action<object, TField>)((FieldDelegateRef)GCHandle.FromIntPtr((IntPtr)offset).Target).Putter)(o, value);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
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
            return o switch
            {
                object[] array => array[offset],
                _ => GetField<object>(o, offset)
            };
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
            switch (o)
            {
                case object[] array:
                    array[offset] = x;
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => Buffer.GetByte(array, (int)offset) != 0,
                _ => GetField<bool>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    Buffer.SetByte(array, (int)offset, x ? (byte)1 : (byte)0);
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => Buffer.GetByte(array, (int)offset),
                _ => GetField<byte>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    Buffer.SetByte(array, (int)offset, x);
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => ReadInt16(array, offset),
                _ => GetField<short>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt16(array, offset, x);
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => (char)ReadInt16(array, offset),
                _ => GetField<char>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt16(array, offset, (short)x);
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => ReadInt32(array, offset),
                _ => GetField<int>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt32(array, offset, x);
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => ReadInt64(array, offset),
                _ => GetField<long>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt64(array, offset, x);
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => global::java.lang.Float.intBitsToFloat(ReadInt32(array, offset)),
                _ => GetField<float>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt32(array, offset, global::java.lang.Float.floatToRawIntBits(x));
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => global::java.lang.Double.longBitsToDouble(ReadInt64(array, offset)),
                _ => GetField<double>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt64(array, offset, global::java.lang.Double.doubleToRawLongBits(x));
                    break;
                default:
                    PutField(o, offset, x);
                    break;
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
            return Marshal.ReadByte((IntPtr)address);
        }

        /// <summary>
        /// Implementation of native method 'putByte'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putByte(object self, long address, byte x)
        {
            Marshal.WriteByte((IntPtr)address, x);
        }

        /// <summary>
        /// Implementation of native method 'getShort'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static short getShort(object self, long address)
        {
            return Marshal.ReadInt16((IntPtr)address);
        }

        /// <summary>
        /// Implementation of native method 'putShort'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putShort(object self, long address, short x)
        {
            Marshal.WriteInt16((IntPtr)address, x);
        }

        /// <summary>
        /// Implementation of native method 'getChar'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static char getChar(object self, long address)
        {
            return (char)Marshal.ReadInt16((IntPtr)address);
        }

        /// <summary>
        /// Implementation of native method 'putChar'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putChar(object self, long address, char x)
        {
            Marshal.WriteInt16((IntPtr)address, (short)x);
        }

        /// <summary>
        /// Implementation of native method 'getInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int getInt(object self, long address)
        {
            return Marshal.ReadInt32((IntPtr)address);
        }

        /// <summary>
        /// Implementation of native method 'putInt'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putInt(object self, long address, int x)
        {
            Marshal.WriteInt32((IntPtr)address, x);
        }

        /// <summary>
        /// Implementation of native method 'getLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static long getLong(object self, long address)
        {
            return Marshal.ReadInt64((IntPtr)address);
        }

        /// <summary>
        /// Implementation of native method 'putLong'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
        public static void putLong(object self, long address, long x)
        {
            Marshal.WriteInt64((IntPtr)address, x);
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
            return global::java.lang.Float.intBitsToFloat(getInt(self, address));
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
            return global::java.lang.Double.longBitsToDouble(getLong(self, address));
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
            return Marshal.ReadIntPtr((IntPtr)address).ToInt64();
        }

        /// <summary>
        /// Implementation of native method 'putAddress'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="address"></param>
        /// <param name="x"></param>
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
                return 0;

            try
            {
                return Marshal.AllocHGlobal((IntPtr)bytes).ToInt64();
            }
            catch (OutOfMemoryException e)
            {
                throw new global::java.lang.OutOfMemoryError(e.Message).initCause(e);
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
                throw new global::java.lang.OutOfMemoryError(e.Message).initCause(e);
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
            var fr = fieldRefCache.GetValue(FieldWrapper.FromField(f), _ => new FieldDelegateRef(_));
            return (long)GCHandle.ToIntPtr(fr.Handle);
        }

        /// <summary>
        /// Implementation of native method 'objectFieldOffset'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static long objectFieldOffset(object self, global::java.lang.reflect.Field f)
        {
            var fr = fieldRefCache.GetValue(FieldWrapper.FromField(f), _ => new FieldDelegateRef(_));
            return (long)GCHandle.ToIntPtr(fr.Handle);
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

            if (ac == typeof(int[]) || ac == typeof(float[]) || ac == typeof(object[]))
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
        /// Creates a delegate capable of accessing a field of a specific type as a volatile.
        /// </summary>
        /// <param name="fw"></param>
        /// <returns></returns>
        static Delegate CreateGetFieldVolatileDelegate(FieldWrapper fw)
        {
            fw.ResolveField();
            var ft = fw.FieldTypeWrapper.IsPrimitive ? fw.FieldTypeWrapper.TypeAsTBD : typeof(object);
            var dm = new DynamicMethod($"__<UnsafeGetFieldVolatile>__{fw.DeclaringType.Name.Replace(".", "_")}__{fw.Name}", ft, new[] { typeof(object) }, fw.DeclaringType.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            if (fw.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fw.GetField());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldflda, fw.GetField());
            }

            if (fw.FieldTypeWrapper.IsWidePrimitive == false)
            {
                il.Emit(OpCodes.Volatile);
                EmitLdind(il, fw.FieldTypeWrapper.TypeAsLocalOrStackType);
            }
            else
            {
                // Java volatile semantics require atomicity, CLR volatile semantics do not
                var mi = typeof(Unsafe).GetMethod(nameof(InterlockedRead), BindingFlags.NonPublic | BindingFlags.Static, null, new[] { fw.FieldTypeWrapper.TypeAsTBD.MakeByRefType() }, null);
                il.Emit(OpCodes.Call, mi);
            }

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(object), ft));
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
            try
            {
                return ((Func<object, T>)((FieldDelegateRef)GCHandle.FromIntPtr((IntPtr)offset).Target).VolatileGetter)(o);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing a field of a specific type.
        /// </summary>
        /// <param name="fw"></param>
        /// <returns></returns>
        static Delegate CreatePutFieldVolatileDelegate(FieldWrapper fw)
        {
            fw.ResolveField();
            var ft = fw.FieldTypeWrapper.IsPrimitive ? fw.FieldTypeWrapper.TypeAsTBD : typeof(object);
            var dm = new DynamicMethod($"__<UnsafePutFieldVolatile>__{fw.DeclaringType.Name.Replace(".", "_")}__{fw.Name}", typeof(void), new[] { typeof(object), ft }, fw.DeclaringType.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            // load reference to field
            if (fw.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fw.GetField());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldflda, fw.GetField());
            }

            il.Emit(OpCodes.Ldarg_1);

            if (fw.FieldTypeWrapper.IsWidePrimitive == false)
            {
                il.Emit(OpCodes.Volatile);
                EmitStind(il, fw.FieldTypeWrapper.TypeAsLocalOrStackType);
            }
            else
            {
                // Java volatile semantics require atomicity, CLR volatile semantics do not
                var mi = typeof(Unsafe).GetMethod(nameof(InterlockedWrite), BindingFlags.NonPublic | BindingFlags.Static, null, new[] { fw.FieldTypeWrapper.TypeAsTBD.MakeByRefType(), fw.FieldTypeWrapper.TypeAsTBD }, null);
                il.Emit(OpCodes.Call, mi);
            }

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(object), ft));
        }

        /// <summary>
        /// Implements the logic to set a field by offset using volatile.
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <exception cref="global::java.lang.InternalError"></exception>
        static void PutFieldVolatile<TField>(object o, long offset, TField value)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                ((Action<object, TField>)((FieldDelegateRef)GCHandle.FromIntPtr((IntPtr)offset).Target).VolatilePutter)(o, value);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing an index of a specific type.
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        static Delegate CreateGetArrayVolatileDelegate(TypeWrapper tw)
        {
            var et = tw.IsPrimitive ? tw.TypeAsTBD : typeof(object);
            var dm = new DynamicMethod($"UnsafeGetArrayVolatile__{tw.Name.Replace(".", "_")}", et, new[] { typeof(object[]), typeof(long) }, tw.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            // load reference to element
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
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
            try
            {
                return ((Func<object[], long, object>)arrayRefCache.GetValue(ClassLoaderWrapper.GetWrapperFromType(array.GetType().GetElementType()), _ => new ArrayDelegateRef(_)).VolatileGetter)(array, offset);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing an index of a specific type.
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        static Delegate CreatePutArrayVolatileDelegate(TypeWrapper tw)
        {
            var et = tw.IsPrimitive ? tw.TypeAsTBD : typeof(object);
            var dm = new DynamicMethod($"UnsafePutArrayVolatile__{tw.Name.Replace(".", "_")}", typeof(void), new[] { typeof(object[]), typeof(long), et }, tw.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            // load reference to element
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
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
            try
            {
                ((Action<object[], long, object>)arrayRefCache.GetValue(ClassLoaderWrapper.GetWrapperFromType(array.GetType().GetElementType()), _ => new ArrayDelegateRef(_)).VolatilePutter)(array, offset, value);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
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
            switch (o)
            {
                case object[] array when array.GetType() == typeof(object[]):
                    return Volatile.Read(ref array[offset]);
                case object[] array:
                    return GetArrayObjectVolatile(array, offset);
                default:
                    return GetFieldVolatile<object>(o, offset);
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
            switch (o)
            {
                case object[] array when array.GetType() == typeof(object[]):
                    Volatile.Write(ref array[offset], x);
                    break;
                case object[] array:
                    PutArrayObjectVolatile(array, offset, x);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => ReadInt32Volatile(array, offset),
                _ => GetFieldVolatile<int>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt32Volatile(array, offset, x);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            switch (o)
            {
                case Array array:
                    WriteByteVolatile(array, offset, x ? (byte)1 : (byte)0);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            switch (o)
            {
                case Array array:
                    WriteByteVolatile(array, offset, x);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => ReadInt16Volatile(array, offset),
                _ => GetFieldVolatile<short>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt16Volatile(array, offset, x);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => (char)ReadInt16Volatile(array, offset),
                _ => GetFieldVolatile<char>(o, offset)
            };
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
            switch (o)
            {
                case Array array:
                    WriteInt16Volatile(array, offset, (short)x);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => ReadInt64Volatile(array, offset),
                _ => GetFieldVolatile<long>(o, offset)
            };
#endif
        }

        /// <summary>
        /// Implementation of native method 'putLongVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putLongVolatile(object self, object o, long offset, long x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            switch (o)
            {
                case Array array:
                    WriteInt64Volatile(array, offset, x);
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => global::java.lang.Float.intBitsToFloat(ReadInt32Volatile(array, offset)),
                _ => GetFieldVolatile<float>(o, offset)
            };
#endif
        }

        /// <summary>
        /// Implementation of native method 'putFloatVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putFloatVolatile(object self, object o, long offset, float x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            switch (o)
            {
                case Array array:
                    WriteInt32Volatile(array, offset, global::java.lang.Float.floatToRawIntBits(x));
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
            return o switch
            {
                Array array => global::java.lang.Double.longBitsToDouble(ReadInt64Volatile(array, offset)),
                _ => GetFieldVolatile<double>(o, offset)
            };
#endif
        }

        /// <summary>
        /// Implementation of native method 'putDoubleVolatile'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="x"></param>
        public static void putDoubleVolatile(object self, object o, long offset, double x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            switch (o)
            {
                case Array array:
                    WriteInt64Volatile(array, offset, global::java.lang.Double.doubleToRawLongBits(x));
                    break;
                default:
                    PutFieldVolatile(o, offset, x);
                    break;
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
        /// Creates a delegate capable of accessing a field of a specific type as a volatile.
        /// </summary>
        /// <param name="fw"></param>
        /// <returns></returns>
        static Delegate CreateCompareExchangeFieldDelegate(FieldWrapper fw)
        {
            var ft = fw.FieldTypeWrapper.IsPrimitive ? fw.FieldTypeWrapper.TypeAsTBD : typeof(object);
            var mi = typeof(Interlocked)
                .GetMethods()
                .Where(i => i.Name == nameof(Interlocked.CompareExchange))
                .Where(i => i.GetParameters().Length > 0 && i.GetParameters()[0].ParameterType == ft.MakeByRefType())
                .FirstOrDefault();
            if (mi == null)
                throw new InvalidOperationException();

            var dm = new DynamicMethod($"UnsafeCompareExchangeFieldDelegate__{fw.DeclaringType.Name.Replace(".", "_")}__{fw.Name}", ft, new[] { typeof(object), ft, ft }, fw.DeclaringType.TypeAsTBD.Module, true);
            var il = dm.GetILGenerator();

            if (fw.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, fw.GetField());
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldflda, fw.GetField());
            }

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Call, mi);

            il.Emit(OpCodes.Ret);
            return dm.CreateDelegate(typeof(Func<,,,>).MakeGenericType(typeof(object), ft, ft, ft));
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
        static T CompareExchangeField<T>(object o, long offset, T value, T expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return ((Func<object, T, T, T>)((FieldDelegateRef)GCHandle.FromIntPtr((IntPtr)offset).Target).CompareExchange)(o, value, expected);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Creates a delegate capable of accessing an index of a specific type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static Delegate CreateCompareExchangeArrayDelegate(TypeWrapper tw)
        {
            var p = Expression.Parameter(typeof(object[]));
            var i = Expression.Parameter(typeof(long));
            var v = Expression.Parameter(typeof(object));
            var e = Expression.Parameter(typeof(object));
            return Expression.Lambda<Func<object[], long, object, object, object>>(
                Expression.Call(
                    compareExchangeMethodInfo.MakeGenericMethod(tw.TypeAsTBD),
                    Expression.Convert(p, tw.MakeArrayType(1).TypeAsTBD),
                    i,
                    Expression.Convert(v, tw.TypeAsTBD),
                    Expression.Convert(e, tw.TypeAsTBD)),
                p, i, v, e)
                .Compile();
        }

        /// <summary>
        /// Implements CompareExchange for a typed array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="comparand"></param>
        /// <returns></returns>
        static object CompareExchange<T>(T[] o, long offset, T value, T comparand)
            where T : class
        {
            return Interlocked.CompareExchange<T>(ref o[offset], value, comparand);
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
        static object CompareExchangeObject(object[] o, long offset, object value, object expected)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return ((Func<object[], long, object, object, object>)arrayRefCache.GetValue(ClassLoaderWrapper.GetWrapperFromType(o.GetType().GetElementType()), _ => new ArrayDelegateRef(_)).CompareExchange)(o, offset, value, expected);
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
            }
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
        /// <exception cref="InvalidOperationException"></exception>
        public static bool compareAndSwapObject(object self, object o, long offset, object expected, object x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return o switch
            {
                object[] array when array.GetType() == typeof(object[]) => Interlocked.CompareExchange(ref array[offset], x, expected) == expected,
                object[] array => CompareExchangeObject(array, offset, x, expected) == expected,
                _ => CompareExchangeField(o, offset, x, expected) == expected
            };
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
                return CompareExchangeField<int>(o, offset, x, expected) == expected;
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
                return CompareExchangeField<long>(o, offset, x, expected) == expected;
            }
#endif
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
        /// Gets the reflective Field instance from the specified offset.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        static FieldInfo GetFieldInfo(long offset)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var fw = FieldWrapper.FromCookie((IntPtr)offset);
                fw.Link();
                fw.ResolveField();
                var fi = fw.GetField();
                return fi;
            }
            catch (Exception e)
            {
                throw new global::java.lang.InternalError(e);
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
        }

        /// <summary>
        /// Reads an <see cref="short"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe short ReadInt16Volatile(Array obj, long offset)
        {
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
        }

        /// <summary>
        /// Reads an <see cref="int"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe int ReadInt32(Array obj, long offset)
        {
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
        }

        /// <summary>
        /// Reads an <see cref="int"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe int ReadInt32Volatile(Array obj, long offset)
        {
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
        }

        /// <summary>
        /// Reads an <see cref="long"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe long ReadInt64(Array obj, long offset)
        {
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
        }

        /// <summary>
        /// Reads an <see cref="long"/> from the array at the specific byte offset.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static unsafe long ReadInt64Volatile(Array obj, long offset)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="byte"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteByte(Array obj, long offset, byte value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="byte"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteByteVolatile(Array obj, long offset, byte value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="short"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt16(Array obj, long offset, short value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="short"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt16Volatile(Array obj, long offset, short value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="int"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt32(Array obj, long offset, int value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="int"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt32Volatile(Array obj, long offset, int value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="long"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt64(Array obj, long offset, long value)
        {
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
        }

        /// <summary>
        /// Writes a <see cref="long"/> to the specified byte offset within the array.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        static unsafe void WriteInt64Volatile(Array obj, long offset, long value)
        {
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
            GCHandle h = new();

            try
            {
                h = GCHandle.Alloc(obj, GCHandleType.Pinned);
                var r = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.AsRef<long>((byte*)h.AddrOfPinnedObject() + offset), value, expected);
                return r;
            }
            finally
            {
                h.Free();
            }
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
        }

        /// <summary>
        /// Implements an Interlocked.Read method for long.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static long InterlockedRead(ref long location)
        {
            return Interlocked.Read(ref location);
        }

        /// <summary>
        /// Implements an Interlocked.Read method for double.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static double InterlockedRead(ref double location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        /// <summary>
        /// Implements an Interlocked.Read method for long.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static void InterlockedWrite(ref long location, long value)
        {
            Interlocked.Exchange(ref location, value);
        }

        /// <summary>
        /// Implements an Interlocked.Read method for double.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        static void InterlockedWrite(ref double location, double value)
        {
            Interlocked.Exchange(ref location, value);
        }

    }

}
