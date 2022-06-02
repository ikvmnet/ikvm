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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

using IKVM.Internal;

static class Java_sun_misc_Unsafe
{
	public static java.lang.reflect.Field createFieldAndMakeAccessible(java.lang.Class c, string fieldName)
	{
#if FIRST_PASS
		return null;
#else
        // we pass in ReflectAccess.class as the field type (which isn't used)
        // to make Field.toString() return something "meaningful" instead of crash
        java.lang.reflect.Field field = new java.lang.reflect.Field(c, fieldName, ikvm.@internal.ClassLiteral<java.lang.reflect.ReflectAccess>.Value, 0, -1, null, null);
        field.@override = true;
        return field;
#endif
	}

	public static java.lang.reflect.Field copyFieldAndMakeAccessible(java.lang.reflect.Field field)
	{
#if FIRST_PASS
		return null;
#else
		field = new java.lang.reflect.Field(field.getDeclaringClass(), field.getName(), field.getType(), field.getModifiers() & ~java.lang.reflect.Modifier.FINAL, field._slot(), null, null);
		field.@override = true;
		return field;
#endif
	}

	private static void CheckArrayBounds(object obj, long offset, int accessLength)
	{
		// NOTE we rely on the fact that Buffer.ByteLength() requires a primitive array
		int arrayLength = Buffer.ByteLength((Array)obj);
		if (offset < 0 || offset > arrayLength - accessLength || accessLength > arrayLength)
		{
			throw new IndexOutOfRangeException();
		}
	}

	[SecuritySafeCritical]
	public static short ReadInt16(object obj, long offset)
	{
		Stats.Log("ReadInt16");
		CheckArrayBounds(obj, offset, 2);
		GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
		short value = Marshal.ReadInt16((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset));
		handle.Free();
		return value;
	}

	[SecuritySafeCritical]
	public static int ReadInt32(object obj, long offset)
	{
		Stats.Log("ReadInt32");
		CheckArrayBounds(obj, offset, 4);
		GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
		int value = Marshal.ReadInt32((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset));
		handle.Free();
		return value;
	}

	[SecuritySafeCritical]
	public static long ReadInt64(object obj, long offset)
	{
		Stats.Log("ReadInt64");
		CheckArrayBounds(obj, offset, 8);
		GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
		long value = Marshal.ReadInt64((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset));
		handle.Free();
		return value;
	}

	[SecuritySafeCritical]
	public static void WriteInt16(object obj, long offset, short value)
	{
		Stats.Log("WriteInt16");
		CheckArrayBounds(obj, offset, 2);
		GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
		Marshal.WriteInt16((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset), value);
		handle.Free();
	}

	[SecuritySafeCritical]
	public static void WriteInt32(object obj, long offset, int value)
	{
		Stats.Log("WriteInt32");
		CheckArrayBounds(obj, offset, 4);
		GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
		Marshal.WriteInt32((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset), value);
		handle.Free();
	}

	[SecuritySafeCritical]
	public static void WriteInt64(object obj, long offset, long value)
	{
		Stats.Log("WriteInt64");
		CheckArrayBounds(obj, offset, 8);
		GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
		Marshal.WriteInt64((IntPtr)(handle.AddrOfPinnedObject().ToInt64() + offset), value);
		handle.Free();
	}

	public static void throwException(object thisUnsafe, Exception x)
	{
		throw x;
	}

	public static bool shouldBeInitialized(object thisUnsafe, java.lang.Class clazz)
	{
		return TypeWrapper.FromClass(clazz).HasStaticInitializer;
	}

	public static void ensureClassInitialized(object thisUnsafe, java.lang.Class clazz)
	{
		TypeWrapper tw = TypeWrapper.FromClass(clazz);
		if (!tw.IsArray)
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

	[SecurityCritical]
	public static object allocateInstance(object thisUnsafe, java.lang.Class clazz)
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
		return FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
	}

	public static java.lang.Class defineClass(object thisUnsafe, string name, byte[] buf, int offset, int length, java.lang.ClassLoader cl, java.security.ProtectionDomain pd)
	{
		return Java_java_lang_ClassLoader.defineClass1(cl, name.Replace('/', '.'), buf, offset, length, pd, null);
	}

	public static java.lang.Class defineClass(object thisUnsafe, string name, byte[] buf, int offset, int length, ikvm.@internal.CallerID callerID)
	{
#if FIRST_PASS
		return null;
#else
		return defineClass(thisUnsafe, name, buf, offset, length, callerID.getCallerClassLoader(), callerID.getCallerClass().pd);
#endif
	}

	public static java.lang.Class defineAnonymousClass(object thisUnsafe, java.lang.Class host, byte[] data, object[] cpPatches)
	{
#if FIRST_PASS
		return null;
#else
		try
		{
			ClassLoaderWrapper loader = TypeWrapper.FromClass(host).GetClassLoader();
			ClassFile classFile = new ClassFile(data, 0, data.Length, "<Unknown>", loader.ClassFileParseOptions, cpPatches);
			if (classFile.IKVMAssemblyAttribute != null)
			{
				// if this happens, the OpenJDK is probably trying to load an OpenJDK class file as a resource,
				// make sure the build process includes the original class file as a resource in that case
				throw new java.lang.ClassFormatError("Trying to define anonymous class based on stub class: " + classFile.Name);
			}
			return loader.GetTypeWrapperFactory().DefineClassImpl(null, TypeWrapper.FromClass(host), classFile, loader, host.pd).ClassObject;
		}
		catch (RetargetableJavaException x)
		{
			throw x.ToJava();
		}
#endif
	}

	public static bool compareAndSwapInt(object thisUnsafe, object obj, long offset, int expect, int update)
	{
#if FIRST_PASS
		return false;
#else
		int[] array = obj as int[];
		if (array != null && (offset & 3) == 0)
		{
			Stats.Log("compareAndSwapInt.array");
			return Interlocked.CompareExchange(ref array[offset / 4], update, expect) == expect;
		}
		else if (obj is Array)
		{
			Stats.Log("compareAndSwapInt.unaligned");
			// unaligned or not the right array type, so we can't be atomic
			lock (thisUnsafe)
			{
				if (ReadInt32(obj, offset) == expect)
				{
					WriteInt32(obj, offset, update);
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
				cacheCompareExchangeInt32[offset] = (CompareExchangeInt32)CreateCompareExchange(offset);
			}
			Stats.Log("compareAndSwapInt.", offset);
			return cacheCompareExchangeInt32[offset](obj, update, expect) == expect;
		}
#endif
	}

    public static bool compareAndSwapLong(object thisUnsafe, object obj, long offset, long expect, long update)
	{
#if FIRST_PASS
		return false;
#else
		long[] array = obj as long[];
		if (array != null && (offset & 7) == 0)
		{
			Stats.Log("compareAndSwapLong.array");
			return Interlocked.CompareExchange(ref array[offset / 8], update, expect) == expect;
		}
		else if (obj is Array)
		{
			Stats.Log("compareAndSwapLong.unaligned");
			// unaligned or not the right array type, so we can't be atomic
			lock (thisUnsafe)
			{
				if (ReadInt64(obj, offset) == expect)
				{
					WriteInt64(obj, offset, update);
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
		FieldWrapper fw = FieldWrapper.FromField(sun.misc.Unsafe.getField(offset));
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
	}

	abstract class Atomic
	{
		// NOTE we don't care that we keep the Type alive, because Unsafe should only be used inside the core class libraries
		private static Dictionary<Type, Atomic> impls = new Dictionary<Type, Atomic>();

		internal static object CompareExchange(object[] array, int index, object value, object comparand)
		{
			return GetImpl(array.GetType().GetElementType()).CompareExchangeImpl(array, index, value, comparand);
		}

		private static Atomic GetImpl(Type type)
		{
			Atomic impl;
			if (!impls.TryGetValue(type, out impl))
			{
				impl = (Atomic)Activator.CreateInstance(typeof(Impl<>).MakeGenericType(type));
				Dictionary<Type, Atomic> curr = impls;
				Dictionary<Type, Atomic> copy = new Dictionary<Type, Atomic>(curr);
				copy[type] = impl;
				Interlocked.CompareExchange(ref impls, copy, curr);
			}
			return impl;
		}

		protected abstract object CompareExchangeImpl(object[] array, int index, object value, object comparand);

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
