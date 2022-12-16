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
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void registerNatives()
        {

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
                    return GetField(offset).getInt(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).setInt(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).get(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).set(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).getBoolean(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).setBoolean(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).getByte(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).setByte(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).getShort(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).setShort(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).getChar(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).setChar(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).getLong(o);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    GetField(offset).setLong(o, x);
                }
                catch (global::java.lang.IllegalAccessException e)
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
                    return GetField(offset).getFloat(o);
                }
                catch (global::java.lang.IllegalAccessException e)
                {
                    throw (global::java.lang.InternalError)new global::java.lang.InternalError().initCause(e);
                }
            }
#endif
        }

        public static void putFloat(object self, object o, long offset, float x)
        {
#if FIRST_PASS
		    return null;
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
                    throw new global::java.lang.ClassFormatError("Trying to define anonymous class based on stub class: " + classFile.Name);
                }
                return loader.GetTypeWrapperFactory().DefineClassImpl(null, TypeWrapper.FromClass(host), classFile, loader, host.pd).ClassObject;
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
#endif
        }

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
                if (offset >= cacheCompareExchangeInt32.Length || cacheCompareExchangeInt32[offset] == null)
                {
                    InterlockedResize(ref cacheCompareExchangeInt32, (int)offset + 1);
                    cacheCompareExchangeInt32[offset] = (CompareExchangeInt32)CreateCompareExchangeDelegate(offset);
                }

                return cacheCompareExchangeInt32[offset](o, x, expected) == expected;
            }
#endif
        }

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
                if (offset >= cacheCompareExchangeInt64.Length || cacheCompareExchangeInt64[offset] == null)
                {
                    InterlockedResize(ref cacheCompareExchangeInt64, (int)offset + 1);
                    cacheCompareExchangeInt64[offset] = (CompareExchangeInt64)CreateCompareExchange(offset);
                }
                Stats.Log("compareAndSwapLong.", offset);
                return cacheCompareExchangeInt64[offset](obj, update, expect) == expect;
            }
#endif
        }

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
                var field = GetField(offset);

        private delegate int CompareExchangeInt32(object obj, int value, int comparand);
        private delegate long CompareExchangeInt64(object obj, long value, long comparand);
        private delegate object CompareExchangeObject(object obj, object value, object comparand);
        private static CompareExchangeInt32[] cacheCompareExchangeInt32 = new CompareExchangeInt32[0];
        private static CompareExchangeInt64[] cacheCompareExchangeInt64 = new CompareExchangeInt64[0];
        private static CompareExchangeObject[] cacheCompareExchangeObject = new CompareExchangeObject[0];

        private static void InterlockedResize<T>(ref T[] array, int newSize)
        {
            for (; ; )
            {
                T[] oldArray = array;
                T[] newArray = oldArray;
                if (oldArray.Length >= newSize)
                {
                    return;
                }
                Array.Resize(ref newArray, newSize);
                if (Interlocked.CompareExchange(ref array, newArray, oldArray) == oldArray)
                {
                    return;
                }
            }
        }

#if !FIRST_PASS
        private static Delegate CreateCompareExchange(long fieldOffset)
        {
            FieldInfo field = GetFieldInfo(fieldOffset);
            bool primitive = field.FieldType.IsPrimitive;
            Type signatureType = primitive ? field.FieldType : typeof(object);
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
            DynamicMethod dm = new DynamicMethod("CompareExchange", signatureType, new Type[] { typeof(object), signatureType, signatureType }, field.DeclaringType);
            ILGenerator ilgen = dm.GetILGenerator();
            // note that we don't bother will special casing static fields, because it is legal to use ldflda on a static field
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Castclass, field.DeclaringType);
            ilgen.Emit(OpCodes.Ldflda, field);
            ilgen.Emit(OpCodes.Ldarg_1);
            if (!primitive)
            {
                ilgen.Emit(OpCodes.Castclass, field.FieldType);
            }
            ilgen.Emit(OpCodes.Ldarg_2);
            if (!primitive)
            {
                ilgen.Emit(OpCodes.Castclass, field.FieldType);
            }
            ilgen.Emit(OpCodes.Call, compareExchange);
            ilgen.Emit(OpCodes.Ret);
            return dm.CreateDelegate(delegateType);
        }

        private static FieldInfo GetFieldInfo(long offset)
        {
            FieldWrapper fw = FieldWrapper.FromField(global::sun.misc.Unsafe.getField(offset));
            fw.Link();
            fw.ResolveField();
            return fw.GetField();
        }
#endif

        public static bool compareAndSwapObject(object thisUnsafe, object obj, long offset, object expect, object update)
        {
#if FIRST_PASS
		return false;
#else
            object[] array = obj as object[];
            if (array != null)
            {
                Stats.Log("compareAndSwapObject.array");
                return Atomic.CompareExchange(array, (int)offset, update, expect) == expect;
            }
            else
            {
                if (offset >= cacheCompareExchangeObject.Length || cacheCompareExchangeObject[offset] == null)
                {
                    InterlockedResize(ref cacheCompareExchangeObject, (int)offset + 1);
                    cacheCompareExchangeObject[offset] = (CompareExchangeObject)CreateCompareExchange(offset);
                }
                Stats.Log("compareAndSwapObject.", offset);
                return cacheCompareExchangeObject[offset](obj, update, expect) == expect;
            }
#endif

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
            // NOTE we don't care that we keep the Type alive, because Unsafe should only be used inside the core class libraries
            private static Dictionary<Type, Atomic> impls = new Dictionary<Type, Atomic>();

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

            sealed class Impl<T> : Atomic
                where T : class
            {
                protected override object CompareExchangeImpl(object[] array, int index, object value, object comparand)
                {
                    return Interlocked.CompareExchange<T>(ref ((T[])array)[index], (T)value, (T)comparand);
                }
            }
        }

        static class Stats
        {
#if !FIRST_PASS && UNSAFE_STATISTICS
		    private static readonly Dictionary<string, int> dict = new Dictionary<string, int>();

		    static Stats()
		    {
			    java.lang.Runtime.getRuntime().addShutdownHook(new DumpStats());
		    }

		    sealed class DumpStats : java.lang.Thread
		    {
			    public override void run()
			    {
				    List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(dict);
				    list.Sort(delegate(KeyValuePair<string, int> kv1, KeyValuePair<string, int> kv2) { return kv1.Value.CompareTo(kv2.Value); });
				    foreach (KeyValuePair<string, int> kv in list)
				    {
					    Console.WriteLine("{0,10}: {1}", kv.Value, kv.Key);
				    }
			    }
		    }
#endif

            [Conditional("UNSAFE_STATISTICS")]
            internal static void Log(string key)
            {
#if !FIRST_PASS && UNSAFE_STATISTICS
				lock (dict)
				{
					int count;
					dict.TryGetValue(key, out count);
					dict[key] = count + 1;
				}
#endif
            }

            [Conditional("UNSAFE_STATISTICS")]
            internal static void Log(string key, long offset)
            {
#if !FIRST_PASS && UNSAFE_STATISTICS
				FieldWrapper field = FieldWrapper.FromField(sun.misc.Unsafe.getField(offset));
				key += field.DeclaringType.Name + "::" + field.Name;
				Log(key);
#endif
            }
        }

    }

}
