/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

sealed class JniHelper
{
	[DllImport("ikvm-native")]
	private static extern IntPtr ikvm_LoadLibrary(string filename);
	[DllImport("ikvm-native")]
	private static extern void ikvm_FreeLibrary(IntPtr handle);
	[DllImport("ikvm-native")]
	internal static extern IntPtr ikvm_GetProcAddress(IntPtr handle, string name, int argc);
	[DllImport("ikvm-native")]
	private unsafe static extern int ikvm_CallOnLoad(IntPtr method, void* jvm, void* reserved);
	[DllImport("ikvm-native")]
	internal unsafe static extern void** ikvm_GetJNIEnvVTable();
	[DllImport("ikvm-native")]
	internal unsafe static extern void* ikvm_MarshalDelegate(Delegate d);

	private static MethodBase FindCaller()
	{
		StackTrace st = new StackTrace();
		for(int i = 0; i < st.FrameCount; i++)
		{
			StackFrame frame = st.GetFrame(i);
			Type type = frame.GetMethod().DeclaringType;
			if(type != null)
			{
				// TODO we need a more robust algorithm to find the "caller" (note that in addition to native methods,
				// System.loadLibrary can also trigger executing native code)
				ClassLoaderWrapper loader = ClassLoaderWrapper.GetWrapperFromType(type).GetClassLoader();
				if(loader.GetJavaClassLoader() != null)
				{
					return frame.GetMethod();
				}
			}
		}
		return null;
	}

	private static ArrayList nativeLibraries = new ArrayList();
	internal static readonly object JniLock = new object();

	internal unsafe static int LoadLibrary(string filename)
	{
		MethodBase m = FindCaller();
		ClassLoaderWrapper loader;
		if(m != null)
		{
			loader = ClassLoaderWrapper.GetWrapperFromType(m.DeclaringType).GetClassLoader();
		}
		else
		{
			loader = ClassLoaderWrapper.GetBootstrapClassLoader();
		}
		lock(JniLock)
		{
			IntPtr p = ikvm_LoadLibrary(filename);
			if(p == IntPtr.Zero)
			{
				return 0;
			}
			try
			{
				foreach(IntPtr tmp in loader.GetNativeLibraries())
				{
					if(tmp == p)
					{
						// the library was already loaded by the current class loader,
						// no need to do anything
						ikvm_FreeLibrary(p);
						return 1;
					}
				}
				if(nativeLibraries.Contains(p))
				{
					throw JavaException.UnsatisfiedLinkError("Native library {0} already loaded in another classloader", filename);
				}
				IntPtr onload = ikvm_GetProcAddress(p, "JNI_OnLoad", IntPtr.Size * 2);
				if(onload != IntPtr.Zero)
				{
					JniFrame f = new JniFrame();
					f.Enter(m.MethodHandle);
					int version = ikvm_CallOnLoad(onload, JavaVM.pJavaVM, null);
					f.Leave();
					if(version != JNIEnv.JNI_VERSION_1_1 && version != JNIEnv.JNI_VERSION_1_2 && version != JNIEnv.JNI_VERSION_1_4)
					{
						throw JavaException.UnsatisfiedLinkError("Unsupported JNI version 0x{0:X} required by {1}", version, filename);
					}
				}
				nativeLibraries.Add(p);
				loader.RegisterNativeLibrary(p);
				return 1;
			}
			catch
			{
				ikvm_FreeLibrary(p);
				throw;
			}
		}
	}
}

[StructLayout(LayoutKind.Sequential)]
struct LocalRefCache
{
	internal object loc1;
	object loc2;
	object loc3;
	object loc4;
	object loc5;
	object loc6;
	object loc7;
	object loc8;
	object loc9;
	object loc10;
}

unsafe struct LocalRefListEntry
{
	internal const int STATIC_LIST_SIZE = 10;
	internal const int LOCAL_REF_SHIFT = 10;
	internal const int BUCKET_SIZE = (1 << LOCAL_REF_SHIFT);
	internal const int LOCAL_REF_MASK = (BUCKET_SIZE - 1);

	[StructLayout(LayoutKind.Explicit)]
	internal struct Union
	{
		[FieldOffset(0)]
		internal Object* static_list;
		[FieldOffset(0)]
		internal void* pv;
	}
	internal Union u;
	internal object[] dynamic_list;

	internal int MakeLocalRef(object o)
	{
		for(int i = 0; i < STATIC_LIST_SIZE; i++)
		{
			if(u.static_list[i] == null)
			{
				u.static_list[i] = o;
				return i;
			}
		}
		if(dynamic_list == null)
		{
			dynamic_list = new object[32 - STATIC_LIST_SIZE];
		}
		for(int i = 0; i < dynamic_list.Length; i++)
		{
			if(dynamic_list[i] == null)
			{
				dynamic_list[i] = o;
				return i + STATIC_LIST_SIZE;
			}
		}
		int newsize = (dynamic_list.Length + STATIC_LIST_SIZE) * 2 - STATIC_LIST_SIZE;
		if(newsize > BUCKET_SIZE)
		{
			return -1;
		}
		object[] tmp = dynamic_list;
		dynamic_list = new object[newsize];
		Array.Copy(tmp, 0, dynamic_list, 0, tmp.Length);
		dynamic_list[tmp.Length] = o;
		return tmp.Length + STATIC_LIST_SIZE;
	}

	internal void DeleteLocalRef(int i)
	{
		if(i < STATIC_LIST_SIZE)
		{
			u.static_list[i] = null;
		}
		else
		{
			dynamic_list[i - STATIC_LIST_SIZE] = null;
		}
	}

	internal object UnwrapLocalRef(int i)
	{
		if(i < STATIC_LIST_SIZE)
		{
			return u.static_list[i];
		}
		else
		{
			return dynamic_list[i - STATIC_LIST_SIZE];
		}
	}
}

class GlobalRefs
{
	internal static System.Collections.ArrayList globalRefs = new System.Collections.ArrayList();
}

unsafe class VtableBuilder
{
	delegate int pf_int_IntPtr(JNIEnv* pEnv, IntPtr p);
	delegate IntPtr pf_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p);
	delegate void pf_void_IntPtr(JNIEnv* pEnv, IntPtr p);
	delegate IntPtr pf_IntPtr(JNIEnv* pEnv);
	delegate void pf_void(JNIEnv* pEnv);
	delegate sbyte pf_sbyte(JNIEnv* pEnv);
	delegate IntPtr pf_IntPtr_pbyte(JNIEnv* pEnv, byte* p);
	delegate int pf_int(JNIEnv* pEnv);
	delegate IntPtr pf_IntPtr_pbyte_IntPtr_pbyte_IntPtr(JNIEnv* pEnv, byte* p1, IntPtr p2, byte* p3, int p4);
	delegate IntPtr pf_IntPtr_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate int pf_int_IntPtr_pbyte(JNIEnv* pEnv, IntPtr p1, byte* p2);
	delegate void pf_void_pbyte(JNIEnv* pEnv, byte* p1);
	delegate IntPtr pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv* pEnv, IntPtr p1, byte* p2, byte* p3);
	delegate int pf_int_IntPtr_pJNINativeMethod_int(JNIEnv* pEnv, IntPtr p1, JNIEnv.JNINativeMethod* p2, int p3);
	delegate int pf_int_ppJavaVM(JNIEnv* pEnv, JavaVM** ppJavaVM);
	delegate sbyte pf_sbyte_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate short pf_short_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate int pf_int_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate long pf_long_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate float pf_float_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate double pf_double_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate void pf_void_IntPtr_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3);
	delegate void pf_void_IntPtr_IntPtr_sbyte(JNIEnv* pEnv, IntPtr p1, IntPtr p2, sbyte p3);
	delegate void pf_void_IntPtr_IntPtr_short(JNIEnv* pEnv, IntPtr p1, IntPtr p2, short p3);
	delegate void pf_void_IntPtr_IntPtr_int(JNIEnv* pEnv, IntPtr p1, IntPtr p2, int p3);
	delegate void pf_void_IntPtr_IntPtr_long(JNIEnv* pEnv, IntPtr p1, IntPtr p2, long p3);
	delegate void pf_void_IntPtr_IntPtr_float(JNIEnv* pEnv, IntPtr p1, IntPtr p2, float p3);
	delegate void pf_void_IntPtr_IntPtr_double(JNIEnv* pEnv, IntPtr p1, IntPtr p2, double p3);
	delegate IntPtr pf_IntPtr_pchar_int(JNIEnv* pEnv, char* p1, int p2);
	delegate void pf_void_IntPtr_IntPtr(JNIEnv* pEnv, IntPtr p1, IntPtr p2);
	delegate IntPtr pf_IntPtr_int_IntPtr_IntPtr(JNIEnv* pEnv, int p1, IntPtr p2, IntPtr p3);
	delegate IntPtr pf_IntPtr_IntPtr_int(JNIEnv* pEnv, IntPtr p1, int p2);
	delegate void pf_void_IntPtr_int_IntPtr(JNIEnv* pEnv, IntPtr p1, int p2, IntPtr p3);
	delegate IntPtr pf_IntPtr_int(JNIEnv* pEnv, int p1);
	delegate void pf_void_IntPtr_int_int_IntPtr(JNIEnv* pEnv, IntPtr p1, int p2, int p3, IntPtr p4);
	delegate IntPtr pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate sbyte pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate short pf_short_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate int pf_int_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate long pf_long_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate float pf_float_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate double pf_double_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate void pf_void_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, JNIEnv.jvalue* p3);
	delegate IntPtr pf_IntPtr_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate sbyte pf_sbyte_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate short pf_short_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate int pf_int_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate long pf_long_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate float pf_float_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate double pf_double_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);
	delegate void pf_void_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv* pEnv, IntPtr p1, IntPtr p2, IntPtr p3, JNIEnv.jvalue* p4);

	internal static void* vtable;

	static VtableBuilder()
	{
		// JNIEnv
		void** pmcpp = JniHelper.ikvm_GetJNIEnvVTable();
		void** p = (void**)Marshal.AllocHGlobal(IntPtr.Size * vtableDelegates.Length);
		for(int i = 0; i < vtableDelegates.Length; i++)
		{
			if(vtableDelegates[i] != null)
			{
				p[i] = JniHelper.ikvm_MarshalDelegate(vtableDelegates[i]);
			}
			else
			{
				p[i] = pmcpp[i];
			}
		}
		vtable = p;
	}

	static Delegate[] vtableDelegates =
		{
			new pf_int_IntPtr_pbyte(JNIEnv.GetMethodArgs), //virtual void JNICALL reserved0();
			null, //virtual void JNICALL reserved1();
			null, //virtual void JNICALL reserved2();
			null, //virtual void JNICALL reserved3();

			new pf_int(JNIEnv.GetVersion), //virtual jint JNICALL GetVersion();

			new pf_IntPtr_pbyte_IntPtr_pbyte_IntPtr(JNIEnv.DefineClass), //virtual jclass JNICALL DefineClass(const char *name, jobject loader, const jbyte *buf, jsize len);
			new pf_IntPtr_pbyte(JNIEnv.FindClass), //virtual jclass JNICALL FindClass(const char *name);

			new pf_IntPtr_IntPtr(JNIEnv.FromReflectedMethod), //virtual jmethodID JNICALL FromReflectedMethod(jobject method);
			new pf_IntPtr_IntPtr(JNIEnv.FromReflectedField), //virtual jfieldID JNICALL FromReflectedField(jobject field);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.ToReflectedMethod), //virtual jobject JNICALL ToReflectedMethod(jclass clazz, jmethodID methodID);

			new pf_IntPtr_IntPtr(JNIEnv.GetSuperclass), //virtual jclass JNICALL GetSuperclass(jclass sub);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.IsAssignableFrom), //virtual jboolean JNICALL IsAssignableFrom(jclass sub, jclass sup);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.ToReflectedField), //virtual jobject JNICALL ToReflectedField(jclass clazz, jfieldID fieldID);

			new pf_int_IntPtr(JNIEnv.Throw), //virtual jint JNICALL Throw(jthrowable obj);
			new pf_int_IntPtr_pbyte(JNIEnv.ThrowNew), //virtual jint JNICALL ThrowNew(jclass clazz, const char *msg);
			new pf_IntPtr(JNIEnv.ExceptionOccurred), //virtual jthrowable JNICALL ExceptionOccurred();
			new pf_void(JNIEnv.ExceptionDescribe), //virtual void JNICALL ExceptionDescribe();
			new pf_void(JNIEnv.ExceptionClear), //virtual void JNICALL ExceptionClear();
			new pf_void_pbyte(JNIEnv.FatalError), //virtual void JNICALL FatalError(const char *msg);

			new pf_void(JNIEnv.NotImplemented), //virtual jint JNICALL PushLocalFrame(jint capacity); 
			new pf_void(JNIEnv.NotImplemented), //virtual jobject JNICALL PopLocalFrame(jobject result);

			new pf_IntPtr_IntPtr(JNIEnv.NewGlobalRef), //virtual jobject JNICALL NewGlobalRef(jobject lobj);
			new pf_void_IntPtr(JNIEnv.DeleteGlobalRef), //virtual void JNICALL DeleteGlobalRef(jobject gref);
			new pf_void_IntPtr(JNIEnv.DeleteLocalRef), //virtual void JNICALL DeleteLocalRef(jobject obj);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.IsSameObject), //virtual jboolean JNICALL IsSameObject(jobject obj1, jobject obj2);

			new pf_IntPtr_IntPtr(JNIEnv.NewLocalRef), //virtual jobject JNICALL NewLocalRef(jobject ref);
			new pf_void(JNIEnv.NotImplemented), //virtual jint JNICALL EnsureLocalCapacity(jint capacity);

			new pf_IntPtr_IntPtr(JNIEnv.AllocObject), //virtual jobject JNICALL AllocObject(jclass clazz);
			null, //virtual jobject JNICALL NewObject(jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL NewObjectV(jclass clazz, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.NewObjectA), //virtual jobject JNICALL NewObjectA(jclass clazz, jmethodID methodID, jvalue *args);

			new pf_IntPtr_IntPtr(JNIEnv.GetObjectClass), //virtual jclass JNICALL GetObjectClass(jobject obj);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.IsInstanceOf), //virtual jboolean JNICALL IsInstanceOf(jobject obj, jclass clazz);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetMethodID), //virtual jmethodID JNICALL GetMethodID(jclass clazz, const char *name, const char *sig);

			null, //virtual jobject JNICALL CallObjectMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallObjectMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallObjectMethodA), //virtual jobject JNICALL CallObjectMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jboolean JNICALL CallBooleanMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallBooleanMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallBooleanMethodA), //virtual jboolean JNICALL CallBooleanMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jbyte JNICALL CallByteMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallByteMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallByteMethodA), //virtual jbyte JNICALL CallByteMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallCharMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallCharMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_pjvalue(JNIEnv.CallCharMethodA), //virtual jchar JNICALL CallCharMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallShortMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallShortMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_pjvalue(JNIEnv.CallShortMethodA), //virtual jshort JNICALL CallShortMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallIntMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallIntMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_int_IntPtr_IntPtr_pjvalue(JNIEnv.CallIntMethodA), //virtual jint JNICALL CallIntMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallLongMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallLongMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_long_IntPtr_IntPtr_pjvalue(JNIEnv.CallLongMethodA), //virtual jlong JNICALL CallLongMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallFloatMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallFloatMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_float_IntPtr_IntPtr_pjvalue(JNIEnv.CallFloatMethodA), //virtual jfloat JNICALL CallFloatMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallDoubleMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallDoubleMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_double_IntPtr_IntPtr_pjvalue(JNIEnv.CallDoubleMethodA), //virtual jdouble JNICALL CallDoubleMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallVoidMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual void JNICALL CallVoidMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_void_IntPtr_IntPtr_pjvalue(JNIEnv.CallVoidMethodA), //virtual void JNICALL CallVoidMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jobject JNICALL CallNonvirtualObjectMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallNonvirtualObjectMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualObjectMethodA), //virtual jobject JNICALL CallNonvirtualObjectMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			null, //virtual jboolean JNICALL CallNonvirtualBooleanMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallNonvirtualBooleanMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualBooleanMethodA), //virtual jboolean JNICALL CallNonvirtualBooleanMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			null, //virtual jbyte JNICALL CallNonvirtualByteMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallNonvirtualByteMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualByteMethodA), //virtual jbyte JNICALL CallNonvirtualByteMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallNonvirtualCharMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallNonvirtualCharMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualCharMethodA), //virtual jchar JNICALL CallNonvirtualCharMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallNonvirtualShortMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallNonvirtualShortMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualShortMethodA), //virtual jshort JNICALL CallNonvirtualShortMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallNonvirtualIntMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallNonvirtualIntMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_int_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualIntMethodA), //virtual jint JNICALL CallNonvirtualIntMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallNonvirtualLongMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallNonvirtualLongMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_long_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualLongMethodA), //virtual jlong JNICALL CallNonvirtualLongMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallNonvirtualFloatMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallNonvirtualFloatMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_float_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualFloatMethodA), //virtual jfloat JNICALL CallNonvirtualFloatMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallNonvirtualDoubleMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallNonvirtualDoubleMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_double_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualDoubleMethodA), //virtual jdouble JNICALL CallNonvirtualDoubleMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallNonvirtualVoidMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual void JNICALL CallNonvirtualVoidMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_void_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallNonvirtualVoidMethodA), //virtual void JNICALL CallNonvirtualVoidMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetFieldID), //virtual jfieldID JNICALL GetFieldID(jclass clazz, const char *name, const char *sig);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetObjectField), //virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetBooleanField), //virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetByteField), //virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
			new pf_short_IntPtr_IntPtr(JNIEnv.GetCharField), //virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
			new pf_short_IntPtr_IntPtr(JNIEnv.GetShortField), //virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
			new pf_int_IntPtr_IntPtr(JNIEnv.GetIntField), //virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
			new pf_long_IntPtr_IntPtr(JNIEnv.GetLongField), //virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
			new pf_float_IntPtr_IntPtr(JNIEnv.GetFloatField), //virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
			new pf_double_IntPtr_IntPtr(JNIEnv.GetDoubleField), //virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

			new pf_void_IntPtr_IntPtr_IntPtr(JNIEnv.SetObjectField), //virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetBooleanField), //virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetByteField), //virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
			new pf_void_IntPtr_IntPtr_short(JNIEnv.SetCharField), //virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
			new pf_void_IntPtr_IntPtr_short(JNIEnv.SetShortField), //virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.SetIntField), //virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
			new pf_void_IntPtr_IntPtr_long(JNIEnv.SetLongField), //virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
			new pf_void_IntPtr_IntPtr_float(JNIEnv.SetFloatField), //virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
			new pf_void_IntPtr_IntPtr_double(JNIEnv.SetDoubleField), //virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetStaticMethodID), //virtual jmethodID JNICALL GetStaticMethodID(jclass clazz, const char *name, const char *sig);

			null, //virtual jobject JNICALL CallStaticObjectMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallStaticObjectMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_IntPtr_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticObjectMethodA), //virtual jobject JNICALL CallStaticObjectMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jboolean JNICALL CallStaticBooleanMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallStaticBooleanMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticBooleanMethodA), //virtual jboolean JNICALL CallStaticBooleanMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jbyte JNICALL CallStaticByteMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallStaticByteMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticByteMethodA), //virtual jbyte JNICALL CallStaticByteMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallStaticCharMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallStaticCharMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticCharMethodA), //virtual jchar JNICALL CallStaticCharMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallStaticShortMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallStaticShortMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_short_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticShortMethodA), //virtual jshort JNICALL CallStaticShortMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallStaticIntMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallStaticIntMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_int_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticIntMethodA), //virtual jint JNICALL CallStaticIntMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallStaticLongMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallStaticLongMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_long_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticLongMethodA), //virtual jlong JNICALL CallStaticLongMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallStaticFloatMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallStaticFloatMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_float_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticFloatMethodA), //virtual jfloat JNICALL CallStaticFloatMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallStaticDoubleMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallStaticDoubleMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_double_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticDoubleMethodA), //virtual jdouble JNICALL CallStaticDoubleMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallStaticVoidMethod(jclass cls, jmethodID methodID, ...);
			null, //virtual void JNICALL CallStaticVoidMethodV(jclass cls, jmethodID methodID, va_list args);
			new pf_void_IntPtr_IntPtr_pjvalue(JNIEnv.CallStaticVoidMethodA), //virtual void JNICALL CallStaticVoidMethodA(jclass cls, jmethodID methodID, jvalue * args);

			new pf_IntPtr_IntPtr_pbyte_pbyte(JNIEnv.GetStaticFieldID), //virtual jfieldID JNICALL GetStaticFieldID(jclass clazz, const char *name, const char *sig);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetStaticObjectField), //virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetStaticBooleanField), //virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
			new pf_sbyte_IntPtr_IntPtr(JNIEnv.GetStaticByteField), //virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
			new pf_short_IntPtr_IntPtr(JNIEnv.GetStaticCharField), //virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
			new pf_short_IntPtr_IntPtr(JNIEnv.GetStaticShortField), //virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
			new pf_int_IntPtr_IntPtr(JNIEnv.GetStaticIntField), //virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
			new pf_long_IntPtr_IntPtr(JNIEnv.GetStaticLongField), //virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
			new pf_float_IntPtr_IntPtr(JNIEnv.GetStaticFloatField), //virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
			new pf_double_IntPtr_IntPtr(JNIEnv.GetStaticDoubleField), //virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

			new pf_void_IntPtr_IntPtr_IntPtr(JNIEnv.SetStaticObjectField), //virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetStaticBooleanField), //virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
			new pf_void_IntPtr_IntPtr_sbyte(JNIEnv.SetStaticByteField), //virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
			new pf_void_IntPtr_IntPtr_short(JNIEnv.SetStaticCharField), //virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
			new pf_void_IntPtr_IntPtr_short(JNIEnv.SetStaticShortField), //virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.SetStaticIntField), //virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
			new pf_void_IntPtr_IntPtr_long(JNIEnv.SetStaticLongField), //virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
			new pf_void_IntPtr_IntPtr_float(JNIEnv.SetStaticFloatField), //virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
			new pf_void_IntPtr_IntPtr_double(JNIEnv.SetStaticDoubleField), //virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

			new pf_IntPtr_pchar_int(JNIEnv.NewString), //virtual jstring JNICALL NewString(const jchar *unicode, jsize len);
			new pf_int_IntPtr(JNIEnv.GetStringLength), //virtual jsize JNICALL GetStringLength(jstring str);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetStringChars), //virtual const jchar *JNICALL GetStringChars(jstring str, jboolean *isCopy);
			new pf_void_IntPtr_IntPtr(JNIEnv.ReleaseStringChars), //virtual void JNICALL ReleaseStringChars(jstring str, const jchar *chars);

			new pf_IntPtr_IntPtr(JNIEnv.NewStringUTF), //virtual jstring JNICALL NewStringUTF(const char *utf);
			new pf_int_IntPtr(JNIEnv.GetStringUTFLength), //virtual jsize JNICALL GetStringUTFLength(jstring str);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetStringUTFChars), //virtual const char* JNICALL GetStringUTFChars(jstring str, jboolean *isCopy);
			new pf_void_IntPtr_IntPtr(JNIEnv.ReleaseStringUTFChars), //virtual void JNICALL ReleaseStringUTFChars(jstring str, const char* chars);

			new pf_int_IntPtr(JNIEnv.GetArrayLength), //virtual jsize JNICALL GetArrayLength(jarray array);

			new pf_IntPtr_int_IntPtr_IntPtr(JNIEnv.NewObjectArray), //virtual jobjectArray JNICALL NewObjectArray(jsize len, jclass clazz, jobject init);
			new pf_IntPtr_IntPtr_int(JNIEnv.GetObjectArrayElement), //virtual jobject JNICALL GetObjectArrayElement(jobjectArray array, jsize index);
			new pf_void_IntPtr_int_IntPtr(JNIEnv.SetObjectArrayElement), //virtual void JNICALL SetObjectArrayElement(jobjectArray array, jsize index, jobject val);

			new pf_IntPtr_int(JNIEnv.NewBooleanArray), //virtual jbooleanArray JNICALL NewBooleanArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewByteArray), //virtual jbyteArray JNICALL NewByteArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewCharArray), //virtual jcharArray JNICALL NewCharArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewShortArray), //virtual jshortArray JNICALL NewShortArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewIntArray), //virtual jintArray JNICALL NewIntArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewLongArray), //virtual jlongArray JNICALL NewLongArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewFloatArray), //virtual jfloatArray JNICALL NewFloatArray(jsize len);
			new pf_IntPtr_int(JNIEnv.NewDoubleArray), //virtual jdoubleArray JNICALL NewDoubleArray(jsize len);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetBooleanArrayElements), //virtual jboolean * JNICALL GetBooleanArrayElements(jbooleanArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetByteArrayElements), //virtual jbyte * JNICALL GetByteArrayElements(jbyteArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetCharArrayElements), //virtual jchar * JNICALL GetCharArrayElements(jcharArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetShortArrayElements), //virtual jshort * JNICALL GetShortArrayElements(jshortArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetIntArrayElements), //virtual jint * JNICALL GetIntArrayElements(jintArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetLongArrayElements), //virtual jlong * JNICALL GetLongArrayElements(jlongArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetFloatArrayElements), //virtual jfloat * JNICALL GetFloatArrayElements(jfloatArray array, jboolean *isCopy);
			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetDoubleArrayElements), //virtual jdouble * JNICALL GetDoubleArrayElements(jdoubleArray array, jboolean *isCopy);

			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseBooleanArrayElements), //virtual void JNICALL ReleaseBooleanArrayElements(jbooleanArray array, jboolean *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseByteArrayElements), //virtual void JNICALL ReleaseByteArrayElements(jbyteArray array, jbyte *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseCharArrayElements), //virtual void JNICALL ReleaseCharArrayElements(jcharArray array, jchar *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseShortArrayElements), //virtual void JNICALL ReleaseShortArrayElements(jshortArray array, jshort *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseIntArrayElements), //virtual void JNICALL ReleaseIntArrayElements(jintArray array, jint *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseLongArrayElements), //virtual void JNICALL ReleaseLongArrayElements(jlongArray array, jlong *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseFloatArrayElements), //virtual void JNICALL ReleaseFloatArrayElements(jfloatArray array, jfloat *elems, jint mode);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleaseDoubleArrayElements), //virtual void JNICALL ReleaseDoubleArrayElements(jdoubleArray array, jdouble *elems, jint mode);

			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetBooleanArrayRegion), //virtual void JNICALL GetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetByteArrayRegion), //virtual void JNICALL GetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetCharArrayRegion), //virtual void JNICALL GetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetShortArrayRegion), //virtual void JNICALL GetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetIntArrayRegion), //virtual void JNICALL GetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetLongArrayRegion), //virtual void JNICALL GetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetFloatArrayRegion), //virtual void JNICALL GetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetDoubleArrayRegion), //virtual void JNICALL GetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetBooleanArrayRegion), //virtual void JNICALL SetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetByteArrayRegion), //virtual void JNICALL SetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetCharArrayRegion), //virtual void JNICALL SetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetShortArrayRegion), //virtual void JNICALL SetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetIntArrayRegion), //virtual void JNICALL SetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetLongArrayRegion), //virtual void JNICALL SetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetFloatArrayRegion), //virtual void JNICALL SetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.SetDoubleArrayRegion), //virtual void JNICALL SetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

			new pf_int_IntPtr_pJNINativeMethod_int(JNIEnv.RegisterNatives), //virtual jint JNICALL RegisterNatives(jclass clazz, const JNINativeMethod *methods, jint nMethods);
			new pf_int_IntPtr(JNIEnv.UnregisterNatives), //virtual jint JNICALL UnregisterNatives(jclass clazz);

			new pf_int_IntPtr(JNIEnv.MonitorEnter), //virtual jint JNICALL MonitorEnter(jobject obj);
			new pf_int_IntPtr(JNIEnv.MonitorExit), //virtual jint JNICALL MonitorExit(jobject obj);

			new pf_int_ppJavaVM(JNIEnv.GetJavaVM), //virtual jint JNICALL GetJavaVM(JavaVM **vm);

			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetStringRegion), //virtual void JNICALL GetStringRegion(jstring str, jsize start, jsize len, jchar *buf);
			new pf_void_IntPtr_int_int_IntPtr(JNIEnv.GetStringUTFRegion), //virtual void JNICALL GetStringUTFRegion(jstring str, jsize start, jsize len, char *buf);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetPrimitiveArrayCritical), //virtual void* JNICALL GetPrimitiveArrayCritical(jarray array, jboolean *isCopy);
			new pf_void_IntPtr_IntPtr_int(JNIEnv.ReleasePrimitiveArrayCritical), //virtual void JNICALL ReleasePrimitiveArrayCritical(jarray array, void *carray, jint mode);

			new pf_IntPtr_IntPtr_IntPtr(JNIEnv.GetStringCritical), //virtual const jchar* JNICALL GetStringCritical(jstring string, jboolean *isCopy);
			new pf_void_IntPtr_IntPtr(JNIEnv.ReleaseStringCritical), //virtual void JNICALL ReleaseStringCritical(jstring string, const jchar *cstring);

			new pf_void(JNIEnv.NotImplemented), //virtual jweak JNICALL NewWeakGlobalRef(jobject obj);
			new pf_void(JNIEnv.NotImplemented), //virtual void JNICALL DeleteWeakGlobalRef(jweak ref);

			new pf_sbyte(JNIEnv.ExceptionCheck), //virtual jboolean JNICALL ExceptionCheck();

			new pf_void(JNIEnv.NotImplemented), //virtual jobject JNICALL NewDirectByteBuffer(void* address, jlong capacity);
			new pf_void(JNIEnv.NotImplemented), //virtual void* JNICALL GetDirectBufferAddress(jobject buf);
			new pf_void(JNIEnv.NotImplemented)  //virtual jlong JNICALL GetDirectBufferCapacity(jobject buf);
		};
}

[StructLayout(LayoutKind.Sequential)]
unsafe struct JavaVM
{
	internal static JavaVM* pJavaVM;
	void** vtable;
	void* firstVtableEntry;
	delegate int pf_int(JavaVM* pJVM);
	delegate int pf_int_ppvoid_pvoid(JavaVM* pJVM, void** p1, void* p2);
	delegate int pf_int_ppvoid_int(JavaVM* pJVM, void** p1, int p2);

	static Delegate[] vtableDelegates =
		{
			null,
			null,
			null,
			new pf_int(DestroyJavaVM),
			new pf_int_ppvoid_pvoid(AttachCurrentThread),
			new pf_int(DetachCurrentThread),
			new pf_int_ppvoid_int(GetEnv),
			new pf_int_ppvoid_pvoid(AttachCurrentThreadAsDaemon)
		};

	static JavaVM()
	{
		pJavaVM = (JavaVM*)(void*)Marshal.AllocHGlobal(IntPtr.Size * (1 + vtableDelegates.Length));
#if __MonoCS__ 
		// MONOBUG mcs requires this bogus fixed construct (and Microsoft doesn't allow it)
		fixed(void** p = &pJavaVM->firstVtableEntry) { pJavaVM->vtable = p; }
#else
		pJavaVM->vtable = &pJavaVM->firstVtableEntry;
#endif
		for(int i = 0; i < vtableDelegates.Length; i++)
		{
			pJavaVM->vtable[i] = JniHelper.ikvm_MarshalDelegate(vtableDelegates[i]);
		}
	}

	internal static int DestroyJavaVM(JavaVM* pJVM)
	{
		return JNIEnv.JNI_ERR;
	}

	internal static int AttachCurrentThread(JavaVM* pJVM, void **penv, void *args)
	{
		// TODO do we need a new local ref frame?
		// TODO for now we only support attaching to an existing thread
		// TODO support args (JavaVMAttachArgs)
		JNIEnv* p = TlsHack.pJNIEnv;
		if(p != null)
		{
			*penv = p;
			return JNIEnv.JNI_OK;
		}
		JVM.CriticalFailure("AttachCurrentThread for non-Java threads not implemented", null);
		return JNIEnv.JNI_ERR;
	}

	internal static int DetachCurrentThread(JavaVM* pJVM)
	{
		JVM.CriticalFailure("DetachCurrentThread not implemented", null);
		return JNIEnv.JNI_ERR;
	}

	internal static int GetEnv(JavaVM* pJVM, void **penv, int version)
	{
		// TODO we should check the version
		JNIEnv* p = TlsHack.pJNIEnv;
		if(p != null)
		{
			*penv = p;
			return JNIEnv.JNI_OK;
		}
		return JNIEnv.JNI_EDETACHED;
	}

	internal static int AttachCurrentThreadAsDaemon(JavaVM* pJVM, void **penv, void *args)
	{
		// TODO do we need a new local ref frame?
		// TODO for now we only support attaching to an existing thread
		// TODO support args (JavaVMAttachArgs)
		JNIEnv* p = TlsHack.pJNIEnv;
		if(p != null)
		{
			*penv = p;
			return JNIEnv.JNI_OK;
		}
		JVM.CriticalFailure("AttachCurrentThreadAsDaemon not implemented", null);
		return JNIEnv.JNI_ERR;
	}
}

[StructLayout(LayoutKind.Sequential)]
unsafe struct JNIEnv
{
	internal const int JNI_OK = 0;
	internal const int JNI_ERR = -1;
	internal const int JNI_EDETACHED = -2;
	internal const int JNI_EVERSION = -3;
	internal const int JNI_COMMIT = 1;
	internal const int JNI_ABORT = 2;
	internal const int JNI_VERSION_1_1 = 0x00010001;
	internal const int JNI_VERSION_1_2 = 0x00010002;
	internal const int JNI_VERSION_1_4 = 0x00010004;
	internal const sbyte JNI_TRUE = 1;
	internal const sbyte JNI_FALSE = 0;
	internal void* vtable;
	[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct Union
	{
		[FieldOffset(0)]
		internal JniFrame* activeFrame;
		[FieldOffset(0)]
		internal void* pFrame;
	}
	internal Union u;
	internal GCHandle localRefs;
	internal int localRefSlot;
	internal IntPtr pendingException;

	private static string StringFromUTF8(byte* psz)
	{
		// Sun's modified UTF8 encoding is not compatible with System.Text.Encoding.UTF8,
		// so we need to roll our own
		int len = 0;
		bool hasNonAscii = false;
		while(psz[len] != 0)
		{
			hasNonAscii |= psz[len] >= 128;
			len++;
		}
		if(!hasNonAscii)
		{
			// optimize the common case of 7-bit ASCII
			return new String((sbyte*)psz);
		}
		StringBuilder sb = new StringBuilder(len);
		for(int i = 0; i < len; i++)
		{
			int c = *psz++;
			int char2, char3;
			switch(c >> 4)
			{
				case 12:
				case 13:
					char2 = *psz++;
					i++;
					c = (((c & 0x1F) << 6) | (char2 & 0x3F));
					break;
				case 14:
					char2 = *psz++;
					char3 = *psz++;
					i++;
					i++;
					c = ((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | (char3 & 0x3F);
					break;
			}
			sb.Append((char)c);
		}
		return sb.ToString();
	}

	private static int StringUTF8Length(string s)
	{
		int len = 0;
		for(int i = 0; i < s.Length; i++)
		{
			char ch = s[i];
			if((ch != 0) && (ch <= 0x7F))
			{
				len++;
			}
			else if(ch <= 0x7FF)
			{
				len += 2;
			}
			else
			{
				len += 3;
			}
		}
		return len;
	}

	// this method returns a simplified method argument descriptor.
	// some examples:
	// "()V" -> ""
	// "(ILjava.lang.String;)I" -> "IL"
	// "([Ljava.lang.String;)V" -> "L"
	private static string GetMethodArgList(IntPtr cookie)
	{
		try
		{
			StringBuilder sb = new StringBuilder();
			string s = MethodWrapper.FromCookie(cookie).Signature;
			for(int i = 1;; i++)
			{
				switch(s[i])
				{
					case '[':
						while(s[i] == '[') i++;
						if(s[i] == 'L')
						{
							while(s[i] != ';') i++;
						}
						sb.Append('L');
						break;
					case 'L':
						while(s[i] != ';') i++;
						sb.Append('L');
						break;
					case ')':
						return sb.ToString();
					default:
						sb.Append(s[i]);
						break;
				}
			}
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	internal static int GetMethodArgs(JNIEnv* pEnv, IntPtr method, byte* sig)
	{
		string s = GetMethodArgList(method);
		for(int i = 0; i < s.Length; i++)
		{
			sig[i] = (byte)s[i];
		}
		return s.Length;
	}

	internal static int GetVersion(JNIEnv* pEnv)
	{
		return JNI_VERSION_1_4;
	}

	internal static IntPtr DefineClass(JNIEnv* pEnv, byte* name, IntPtr loader, byte* pbuf, int length)
	{
		byte[] buf = new byte[length];
		Marshal.Copy((IntPtr)(void*)pbuf, buf, 0, length);
		// TODO what should the protection domain be?
		return pEnv->MakeLocalRef(NativeCode.java.lang.VMClassLoader.defineClass(pEnv->UnwrapRef(loader), StringFromUTF8(name), buf, 0, buf.Length, null));
	}

	private static ClassLoaderWrapper FindNativeMethodClassLoader()
	{
		StackTrace st = new StackTrace();
		for(int i = 0; i < st.FrameCount; i++)
		{
			StackFrame frame = st.GetFrame(i);
			Type type = frame.GetMethod().DeclaringType;
			if(type != null)
			{
				// TODO we need a more robust algorithm to find the "caller" (note that in addition to native methods,
				// System.loadLibrary can also trigger executing native code)
				ClassLoaderWrapper loader = ClassLoaderWrapper.GetWrapperFromType(type).GetClassLoader();
				if(loader.GetJavaClassLoader() != null)
				{
					return loader;
				}
			}
		}
		// TODO instead of using the bootstrap class loader, we need to use the system (aka application) class loader
		return ClassLoaderWrapper.GetBootstrapClassLoader();
	}

	internal static IntPtr FindClass(JNIEnv* pEnv, byte* name)
	{
		try
		{
			TypeWrapper wrapper = FindNativeMethodClassLoader().LoadClassByDottedName(StringFromUTF8(name).Replace('/', '.'));
			// TODO is this needed?
			wrapper.Finish();
			return pEnv->MakeLocalRef(NativeCode.java.lang.VMClass.getClassFromWrapper(wrapper));
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, x);
			return IntPtr.Zero;
		}
	}

	internal static IntPtr FromReflectedMethod(JNIEnv* pEnv, IntPtr method)
	{
		object methodObj = pEnv->UnwrapRef(method);
		MethodWrapper mw = (MethodWrapper)methodObj.GetType().GetField("methodCookie", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(methodObj);
		return mw.Cookie;
	}

	internal static IntPtr FromReflectedField(JNIEnv* pEnv, IntPtr field)
	{
		object fieldObj = pEnv->UnwrapRef(field);
		FieldWrapper fw = (FieldWrapper)fieldObj.GetType().GetField("fieldCookie", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(fieldObj);
		return fw.Cookie;
	}

	internal static IntPtr ToReflectedMethod(JNIEnv* pEnv, IntPtr clazz_ignored, IntPtr method)
	{
		MethodWrapper mw = MethodWrapper.FromCookie(method);
		TypeWrapper tw;
		if(mw.Name == "<init>")
		{
			tw = ClassLoaderWrapper.LoadClassCritical("java.lang.reflect.Constructor");
		}
		else
		{
			tw = ClassLoaderWrapper.LoadClassCritical("java.lang.reflect.Method");
		}
		object clazz = NativeCode.java.lang.VMClass.getClassFromWrapper(mw.DeclaringType);
		return pEnv->MakeLocalRef(Activator.CreateInstance(tw.TypeAsTBD, new object[] { clazz, mw }));
	}

	internal static IntPtr GetSuperclass(JNIEnv* pEnv, IntPtr sub)
	{
		TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(sub)).BaseTypeWrapper;
		return pEnv->MakeLocalRef(wrapper == null ? null : NativeCode.java.lang.VMClass.getClassFromWrapper(wrapper));
	}

	internal static sbyte IsAssignableFrom(JNIEnv* pEnv, IntPtr sub, IntPtr super)
	{
		TypeWrapper w1 = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(sub));
		TypeWrapper w2 = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(super));
		return w2.IsAssignableTo(w1) ? JNI_TRUE : JNI_FALSE;
	}

	internal static IntPtr ToReflectedField(JNIEnv* pEnv, IntPtr clazz_ignored, IntPtr field)
	{
		FieldWrapper fw = FieldWrapper.FromCookie(field);
		TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical("java.lang.reflect.Field");
		object clazz = NativeCode.java.lang.VMClass.getClassFromWrapper(fw.DeclaringType);
		return pEnv->MakeLocalRef(Activator.CreateInstance(tw.TypeAsTBD, new object[] { clazz, fw }));
	}

	private static void SetPendingException(JNIEnv* pEnv, Exception x)
	{
		DeleteLocalRef(pEnv, pEnv->pendingException);
		pEnv->pendingException = pEnv->MakeLocalRef(x);
	}

	internal static int Throw(JNIEnv* pEnv, IntPtr throwable)
	{
		DeleteLocalRef(pEnv, pEnv->pendingException);
		pEnv->pendingException = NewLocalRef(pEnv, throwable);
		return JNI_OK;
	}

	internal static int ThrowNew(JNIEnv* pEnv, IntPtr clazz, byte* msg)
	{
		TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
		MethodWrapper mw = wrapper.GetMethodWrapper(new MethodDescriptor("<init>", "(Ljava.lang.String;)V"), false);
		if(mw != null)
		{
			int rc;
			Exception exception;
			try
			{
				wrapper.Finish();
				exception = (Exception)mw.Invoke(null, new object[] { StringFromUTF8(msg) }, false);
				rc = JNI_OK;
			}
			catch(Exception x)
			{
				exception = x;
				rc = JNI_ERR;
			}
			SetPendingException(pEnv, exception);
			return rc;
		}
		else
		{
			SetPendingException(pEnv, JavaException.NoSuchMethodError("<init>(Ljava.lang.String;)V"));
			return JNI_ERR;
		}
	}

	internal static IntPtr ExceptionOccurred(JNIEnv* pEnv)
	{
		return NewLocalRef(pEnv, pEnv->pendingException);
	}

	internal static void ExceptionDescribe(JNIEnv* pEnv)
	{
		Exception x = (Exception)pEnv->UnwrapRef(pEnv->pendingException);
		if(x != null)
		{
			try
			{
				MethodWrapper mw = ClassLoaderWrapper.LoadClassCritical("java.lang.Throwable").GetMethodWrapper(new MethodDescriptor("printStackTrace", "()V"), false);
				mw.Invoke(x, null, false);
			}
			catch(Exception ex)
			{
				Debug.Assert(false, ex.ToString());
			}
		}
	}

	internal static void ExceptionClear(JNIEnv* pEnv)
	{
		DeleteLocalRef(pEnv, pEnv->pendingException);
		pEnv->pendingException = IntPtr.Zero;
	}

	internal static void FatalError(JNIEnv* pEnv, byte* msg)
	{
		JVM.CriticalFailure(StringFromUTF8(msg), null);
	}

	internal static IntPtr NewGlobalRef(JNIEnv* pEnv, IntPtr obj)
	{
		if(obj == IntPtr.Zero)
		{
			return IntPtr.Zero;
		}
		// TODO search for an empty slot before adding it to the end...
		return (IntPtr)(-(GlobalRefs.globalRefs.Add(pEnv->UnwrapRef(obj)) + 1));
	}

	internal static void DeleteGlobalRef(JNIEnv* pEnv, IntPtr obj)
	{
		int i = obj.ToInt32();
		if(i < 0)
		{
			GlobalRefs.globalRefs[(-i) - 1] = null;
			return;
		}
		if(i > 0)
		{
			Debug.Assert(false, "Local ref passed to DeleteGlobalRef");
		}
	}

	internal static void DeleteLocalRef(JNIEnv* pEnv, IntPtr obj)
	{
		int i = obj.ToInt32();
		if(i > 0)
		{
			pEnv->u.activeFrame->localRefs[i >> LocalRefListEntry.LOCAL_REF_SHIFT].DeleteLocalRef(i & LocalRefListEntry.LOCAL_REF_MASK);
			return;
		}
		if(i < 0)
		{
			Debug.Assert(false, "bogus localref in DeleteLocalRef");
		}
	}

	internal static sbyte IsSameObject(JNIEnv* pEnv, IntPtr obj1, IntPtr obj2)
	{
		return pEnv->UnwrapRef(obj1) == pEnv->UnwrapRef(obj2) ? JNI_TRUE : JNI_FALSE;
	}

	internal static IntPtr NewLocalRef(JNIEnv* pEnv, IntPtr obj)
	{
		return pEnv->MakeLocalRef(pEnv->UnwrapRef(obj));
	}

	internal static IntPtr AllocObject(JNIEnv* pEnv, IntPtr clazz)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
			wrapper.Finish();
			// TODO add error handling (e.g. when trying to instantiate an interface or abstract class)
			return pEnv->MakeLocalRef(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType));
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, x);
			return IntPtr.Zero;
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct jvalue
	{
		[FieldOffset(0)]
		public sbyte b;
		[FieldOffset(0)]
		public short s;
		[FieldOffset(0)]
		public int i;
		[FieldOffset(0)]
		public long j;
		[FieldOffset(0)]
		public float f;
		[FieldOffset(0)]
		public double d;
		[FieldOffset(0)]
		public IntPtr l;
	}

	private static object InvokeHelper(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue *args, bool nonVirtual)
	{
		string sig = GetMethodArgList(methodID);
		object[] argarray = new object[sig.Length];
		for(int i = 0; i < sig.Length; i++)
		{
			switch(sig[i])
			{
				case 'Z':
					argarray[i] = args[i].b != 0;
					break;
				case 'B':
					argarray[i] = args[i].b;
					break;
				case 'C':
					argarray[i] = (char)args[i].s;
					break;
				case 'S':
					argarray[i] = args[i].s;
					break;
				case 'I':
					argarray[i] = args[i].i;
					break;
				case 'J':
					argarray[i] = args[i].j;
					break;
				case 'F':
					argarray[i] = args[i].f;
					break;
				case 'D':
					argarray[i] = args[i].d;
					break;
				case 'L':
					argarray[i] = pEnv->UnwrapRef(args[i].l);
					break;
			}
		}
		try
		{
			return MethodWrapper.FromCookie(methodID).Invoke(pEnv->UnwrapRef(obj), argarray, nonVirtual);
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, ExceptionHelper.MapExceptionFast(x));
			return null;
		}
	}

	internal static IntPtr NewObjectA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		return pEnv->MakeLocalRef(InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false));
	}

	internal static IntPtr GetObjectClass(JNIEnv* pEnv, IntPtr obj)
	{
		return pEnv->MakeLocalRef(NativeCode.java.lang.VMClass.getClassFromType(pEnv->UnwrapRef(obj).GetType()));
	}

	internal static sbyte IsInstanceOf(JNIEnv* pEnv, IntPtr obj, IntPtr clazz)
	{
		object objClass = NativeCode.java.lang.VMClass.getClassFromType(pEnv->UnwrapRef(obj).GetType());
		TypeWrapper w1 = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
		TypeWrapper w2 = NativeCode.java.lang.VMClass.getWrapperFromClass(objClass);
		return w2.IsAssignableTo(w1) ? JNI_TRUE : JNI_FALSE;
	}

	private static IntPtr FindMethodID(JNIEnv* pEnv, IntPtr clazz, byte* name, byte* sig, bool isstatic)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
			wrapper.Finish();
			MethodDescriptor md = new MethodDescriptor(StringFromUTF8(name), StringFromUTF8(sig).Replace('/', '.'));
			MethodWrapper mw = wrapper.GetMethodWrapper(md, true);
			if(mw != null)
			{
				if(mw.IsStatic == isstatic)
				{
					mw.Link();
					return mw.Cookie;
				}
			}
			SetPendingException(pEnv, JavaException.NoSuchMethodError("{0}{1}", md.Name, md.Signature));
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, x);
		}
		return IntPtr.Zero;
	}

	internal static IntPtr GetMethodID(JNIEnv* pEnv, IntPtr clazz, byte* name, byte* sig)
	{
		return FindMethodID(pEnv, clazz, name, sig, false);
	}

	internal static IntPtr CallObjectMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		return pEnv->MakeLocalRef(InvokeHelper(pEnv, obj, methodID, args, false));
	}

	internal static sbyte CallBooleanMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return ((bool)o) ? JNI_TRUE : JNI_FALSE;
		}
		return JNI_FALSE;
	}

	internal static sbyte CallByteMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (sbyte)o;
		}
		return 0;
	}

	internal static short CallCharMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (short)(char)o;
		}
		return 0;
	}

	internal static short CallShortMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (short)o;
		}
		return 0;
	}

	internal static int CallIntMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (int)o;
		}
		return 0;
	}

	internal static long CallLongMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (long)o;
		}
		return 0;
	}

	internal static float CallFloatMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (float)o;
		}
		return 0;
	}

	internal static double CallDoubleMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, false);
		if(o != null)
		{
			return (double)o;
		}
		return 0;
	}

	internal static void CallVoidMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr methodID, jvalue*  args)
	{
		InvokeHelper(pEnv, obj, methodID, args, false);
	}

	internal static IntPtr CallNonvirtualObjectMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue*  args)
	{
		return pEnv->MakeLocalRef(InvokeHelper(pEnv, obj, methodID, args, true));
	}

	internal static sbyte CallNonvirtualBooleanMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue*  args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return ((bool)o) ? JNI_TRUE : JNI_FALSE;
		}
		return JNI_FALSE;
	}

	internal static sbyte CallNonvirtualByteMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (sbyte)o;
		}
		return 0;
	}

	internal static short CallNonvirtualCharMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (short)(char)o;
		}
		return 0;
	}

	internal static short CallNonvirtualShortMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (short)o;
		}
		return 0;
	}

	internal static int CallNonvirtualIntMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (int)o;
		}
		return 0;
	}

	internal static long CallNonvirtualLongMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (long)o;
		}
		return 0;
	}

	internal static float CallNonvirtualFloatMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (float)o;
		}
		return 0;
	}

	internal static double CallNonvirtualDoubleMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue* args)
	{
		object o = InvokeHelper(pEnv, obj, methodID, args, true);
		if(o != null)
		{
			return (double)o;
		}
		return 0;
	}

	internal static void CallNonvirtualVoidMethodA(JNIEnv* pEnv, IntPtr obj, IntPtr clazz, IntPtr methodID, jvalue*  args)
	{
		InvokeHelper(pEnv, obj, methodID, args, true);
	}

	private static IntPtr FindFieldID(JNIEnv* pEnv, IntPtr clazz, byte* name, byte* sig, bool isstatic)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
			wrapper.Finish();
			// TODO what about searching the base classes?
			FieldWrapper fw = wrapper.GetFieldWrapper(StringFromUTF8(name), wrapper.GetClassLoader().ExpressionTypeWrapper(StringFromUTF8(sig).Replace('/', '.')));
			if(fw != null)
			{
				if(fw.IsStatic == isstatic)
				{
					// TODO fw.Link()
					return fw.Cookie;
				}
			}
			SetPendingException(pEnv, JavaException.NoSuchFieldError(StringFromUTF8(name)));
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, x);
		}
		return IntPtr.Zero;
	}

	internal static IntPtr GetFieldID(JNIEnv* pEnv, IntPtr clazz, byte* name, byte* sig)
	{
		return FindFieldID(pEnv, clazz, name, sig, false);
	}

	private static void SetFieldValue(IntPtr cookie, object obj, object val)
	{
		try
		{
			FieldWrapper.FromCookie(cookie).SetValue(obj, val);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	private static object GetFieldValue(IntPtr cookie, object obj)
	{
		try
		{
			return FieldWrapper.FromCookie(cookie).GetValue(obj);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	internal static IntPtr GetObjectField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return pEnv->MakeLocalRef(GetFieldValue(fieldID, pEnv->UnwrapRef(obj)));
	}

	internal static sbyte GetBooleanField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return ((bool)GetFieldValue(fieldID, pEnv->UnwrapRef(obj))) ? JNI_TRUE : JNI_FALSE;
	}

	internal static sbyte GetByteField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (sbyte)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static short GetCharField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (short)(char)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static short GetShortField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (short)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static int GetIntField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (int)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static long GetLongField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (long)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static float GetFloatField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (float)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static double GetDoubleField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID)
	{
		return (double)GetFieldValue(fieldID, pEnv->UnwrapRef(obj));
	}

	internal static void SetObjectField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, IntPtr val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), pEnv->UnwrapRef(val));
	}

	internal static void SetBooleanField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, sbyte val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val != JNI_FALSE);
	}

	internal static void SetByteField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, sbyte val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val);
	}

	internal static void SetCharField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, short val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), (char)val);
	}

	internal static void SetShortField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, short val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val);
	}

	internal static void SetIntField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, int val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val);
	}

	internal static void SetLongField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, long val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val);
	}

	internal static void SetFloatField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, float val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val);
	}

	internal static void SetDoubleField(JNIEnv* pEnv, IntPtr obj, IntPtr fieldID, double val)
	{
		SetFieldValue(fieldID, pEnv->UnwrapRef(obj), val);
	}

	internal static IntPtr GetStaticMethodID(JNIEnv* pEnv, IntPtr clazz, byte* name, byte* sig)
	{
		return FindMethodID(pEnv, clazz, name, sig, true);
	}

	internal static IntPtr CallStaticObjectMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		return pEnv->MakeLocalRef(InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false));
	}

	internal static sbyte CallStaticBooleanMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return ((bool)o) ? JNI_TRUE : JNI_FALSE;
		}
		return JNI_FALSE;
	}

	internal static sbyte CallStaticByteMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (sbyte)o;
		}
		return 0;
	}

	internal static short CallStaticCharMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (short)(char)o;
		}
		return 0;
	}

	internal static short CallStaticShortMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (short)o;
		}
		return 0;
	}

	internal static int CallStaticIntMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (int)o;
		}
		return 0;
	}

	internal static long CallStaticLongMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (long)o;
		}
		return 0;
	}

	internal static float CallStaticFloatMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (float)o;
		}
		return 0;
	}

	internal static double CallStaticDoubleMethodA(JNIEnv* pEnv, IntPtr clazz, IntPtr methodID, jvalue *args)
	{
		object o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
		if(o != null)
		{
			return (double)o;
		}
		return 0;
	}

	internal static void CallStaticVoidMethodA(JNIEnv* pEnv, IntPtr cls, IntPtr methodID, jvalue * args)
	{
		InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
	}

	internal static IntPtr GetStaticFieldID(JNIEnv* pEnv, IntPtr clazz, byte* name, byte* sig)
	{
		return FindFieldID(pEnv, clazz, name, sig, true);
	}

	internal static IntPtr GetStaticObjectField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return pEnv->MakeLocalRef(GetFieldValue(fieldID, null));
	}

	internal static sbyte GetStaticBooleanField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return ((bool)GetFieldValue(fieldID, null)) ? JNI_TRUE : JNI_FALSE;
	}

	internal static sbyte GetStaticByteField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (sbyte)GetFieldValue(fieldID, null);
	}

	internal static short GetStaticCharField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (short)(char)GetFieldValue(fieldID, null);
	}

	internal static short GetStaticShortField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (short)GetFieldValue(fieldID, null);
	}

	internal static int GetStaticIntField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (int)GetFieldValue(fieldID, null);
	}

	internal static long GetStaticLongField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (long)GetFieldValue(fieldID, null);
	}

	internal static float GetStaticFloatField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (float)GetFieldValue(fieldID, null);
	}

	internal static double GetStaticDoubleField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID)
	{
		return (double)GetFieldValue(fieldID, null);
	}

	internal static void SetStaticObjectField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, IntPtr val)
	{
		SetFieldValue(fieldID, null, pEnv->UnwrapRef(val));
	}

	internal static void SetStaticBooleanField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, sbyte val)
	{
		SetFieldValue(fieldID, null, val != JNI_FALSE);
	}

	internal static void SetStaticByteField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, sbyte val)
	{
		SetFieldValue(fieldID, null, val);
	}

	internal static void SetStaticCharField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, short val)
	{
		SetFieldValue(fieldID, null, (char)val);
	}

	internal static void SetStaticShortField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, short val)
	{
		SetFieldValue(fieldID, null, val);
	}

	internal static void SetStaticIntField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, int val)
	{
		SetFieldValue(fieldID, null, val);
	}

	internal static void SetStaticLongField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, long val)
	{
		SetFieldValue(fieldID, null, val);
	}

	internal static void SetStaticFloatField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, float val)
	{
		SetFieldValue(fieldID, null, val);
	}

	internal static void SetStaticDoubleField(JNIEnv* pEnv, IntPtr clazz, IntPtr fieldID, double val)
	{
		SetFieldValue(fieldID, null, val);
	}

	internal static IntPtr NewString(JNIEnv* pEnv, char* unicode, int len)
	{
		return pEnv->MakeLocalRef(new String(unicode, 0, len));
	}

	internal static int GetStringLength(JNIEnv* pEnv, IntPtr str)
	{
		return ((string)pEnv->UnwrapRef(str)).Length;
	}

	internal static IntPtr GetStringChars(JNIEnv* pEnv, IntPtr str, IntPtr isCopy)
	{
		string s = (string)pEnv->UnwrapRef(str);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return Marshal.StringToHGlobalUni(s);
	}

	internal static void ReleaseStringChars(JNIEnv* pEnv, IntPtr str, IntPtr chars)
	{
		Marshal.FreeHGlobal(chars);
	}

	internal static IntPtr NewStringUTF(JNIEnv* pEnv, IntPtr psz)
	{
		return pEnv->MakeLocalRef(StringFromUTF8((byte*)(void*)psz));
	}

	internal static int GetStringUTFLength(JNIEnv* pEnv, IntPtr str)
	{
		return StringUTF8Length((string)pEnv->UnwrapRef(str));
	}

	internal static IntPtr GetStringUTFChars(JNIEnv* pEnv, IntPtr str, IntPtr isCopy)
	{
		string s = (string)pEnv->UnwrapRef(str);
		byte* buf = (byte*)Marshal.AllocHGlobal(StringUTF8Length(s) + 1);
		int j = 0;
		for(int i = 0; i < s.Length; i++)
		{
			char ch = s[i];
			if((ch != 0) && (ch <= 0x7F))
			{
				buf[j++] = (byte)ch;
			}
			else if(ch <= 0x7FF)
			{
				buf[j++] = (byte)((ch >> 6) | 0xC0);
				buf[j++] = (byte)((ch & 0x3F) | 0x80);
			}
			else
			{
				buf[j++] = (byte)((ch >> 12) | 0xE0);
				buf[j++] = (byte)(((ch >> 6) & 0x3F) | 0x80);
				buf[j++] = (byte)((ch & 0x3F) | 0x80);
			}
		}
		buf[j] = 0;
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return (IntPtr)(void*)buf;
	}

	internal static void ReleaseStringUTFChars(JNIEnv* pEnv, IntPtr str, IntPtr chars)
	{
		Marshal.FreeHGlobal(chars);
	}

	internal static int GetArrayLength(JNIEnv* pEnv, IntPtr array)
	{
		return ((Array)pEnv->UnwrapRef(array)).Length;
	}

	internal static IntPtr NewObjectArray(JNIEnv* pEnv, int len, IntPtr clazz, IntPtr init)
	{
		// TODO if we want to support (non-primitive) value types we can't use the object[] cast
		object[] array = (object[])Array.CreateInstance(NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz)).TypeAsArrayType, len);
		object o = pEnv->UnwrapRef(init);
		if(o != null)
		{
			for(int i = 0; i < array.Length; i++)
			{
				array[i] = o;
			}
		}
		return pEnv->MakeLocalRef(array);
	}

	internal static IntPtr GetObjectArrayElement(JNIEnv* pEnv, IntPtr array, int index)
	{
		try
		{
			// TODO if we want to support (non-primitive) value types we can't use the object[] cast
			return pEnv->MakeLocalRef(((object[])pEnv->UnwrapRef(array))[index]);
		}
		catch(IndexOutOfRangeException)
		{
			SetPendingException(pEnv, JavaException.ArrayIndexOutOfBoundsException());
			return IntPtr.Zero;
		}
	}

	internal static void SetObjectArrayElement(JNIEnv* pEnv, IntPtr array, int index, IntPtr val)
	{
		try
		{
			// TODO if we want to support (non-primitive) value types we can't use the object[] cast
			((object[])pEnv->UnwrapRef(array))[index] = pEnv->UnwrapRef(val);
		}
		catch(IndexOutOfRangeException)
		{
			SetPendingException(pEnv, JavaException.ArrayIndexOutOfBoundsException());
		}
	}

	internal static IntPtr NewBooleanArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new bool[len]);
	}

	internal static IntPtr NewByteArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new sbyte[len]);
	}

	internal static IntPtr NewCharArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new char[len]);
	}

	internal static IntPtr NewShortArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new short[len]);
	}

	internal static IntPtr NewIntArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new int[len]);
	}

	internal static IntPtr NewLongArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new long[len]);
	}

	internal static IntPtr NewFloatArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new float[len]);
	}

	internal static IntPtr NewDoubleArray(JNIEnv* pEnv, int len)
	{
		return pEnv->MakeLocalRef(new double[len]);
	}

	internal static IntPtr GetBooleanArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		bool[] b = (bool[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 1);
		sbyte* p = (sbyte*)(void*)buf;
		for(int i = 0; i < b.Length; i++)
		{
			*p++ = b[i] ? JNI_TRUE : JNI_FALSE;
		}
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetByteArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		sbyte[] b = (sbyte[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 1);
		sbyte* p = (sbyte*)(void*)buf;
		for(int i = 0; i < b.Length; i++)
		{
			*p++ = b[i];
		}
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetCharArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		char[] b = (char[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 2);
		Marshal.Copy(b, 0, buf, b.Length);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetShortArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		short[] b = (short[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 2);
		Marshal.Copy(b, 0, buf, b.Length);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetIntArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		int[] b = (int[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 4);
		Marshal.Copy(b, 0, buf, b.Length);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetLongArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		long[] b = (long[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 8);
		Marshal.Copy(b, 0, buf, b.Length);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetFloatArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		float[] b = (float[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 4);
		Marshal.Copy(b, 0, buf, b.Length);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static IntPtr GetDoubleArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		double[] b = (double[])pEnv->UnwrapRef(array);
		IntPtr buf = Marshal.AllocHGlobal(b.Length * 8);
		Marshal.Copy(b, 0, buf, b.Length);
		if(isCopy != IntPtr.Zero)
		{
			*((sbyte*)(void*)isCopy) = JNI_TRUE;
		}
		return buf;
	}

	internal static void ReleaseBooleanArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			bool[] b = (bool[])pEnv->UnwrapRef(array);
			sbyte* p = (sbyte*)(void*)elems;
			for(int i = 0; i < b.Length; i++)
			{
				b[i] = *p++ != JNI_FALSE;
			}
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseByteArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			sbyte[] b = (sbyte[])pEnv->UnwrapRef(array);
			sbyte* p = (sbyte*)(void*)elems;
			for(int i = 0; i < b.Length; i++)
			{
				b[i] = *p++;
			}
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseCharArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			char[] b = (char[])pEnv->UnwrapRef(array);
			Marshal.Copy(elems, b, 0, b.Length);
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseShortArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			short[] b = (short[])pEnv->UnwrapRef(array);
			Marshal.Copy(elems, b, 0, b.Length);
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseIntArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			int[] b = (int[])pEnv->UnwrapRef(array);
			Marshal.Copy(elems, b, 0, b.Length);
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseLongArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			long[] b = (long[])pEnv->UnwrapRef(array);
			Marshal.Copy(elems, b, 0, b.Length);
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseFloatArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			float[] b = (float[])pEnv->UnwrapRef(array);
			Marshal.Copy(elems, b, 0, b.Length);
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void ReleaseDoubleArrayElements(JNIEnv* pEnv, IntPtr array, IntPtr elems, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			double[] b = (double[])pEnv->UnwrapRef(array);
			Marshal.Copy(elems, b, 0, b.Length);
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(elems);
		}
	}

	internal static void GetBooleanArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		bool[] b = (bool[])pEnv->UnwrapRef(array);
		sbyte* p = (sbyte*)(void*)buf;
		for(int i = 0; i < len; i++)
		{
			*p++ = b[start + i] ? JNI_TRUE : JNI_FALSE;
		}
	}

	internal static void GetByteArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		sbyte[] b = (sbyte[])pEnv->UnwrapRef(array);
		sbyte* p = (sbyte*)(void*)buf;
		for(int i = 0; i < len; i++)
		{
			*p++ = b[start + i];
		}
	}

	internal static void GetCharArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		char[] b = (char[])pEnv->UnwrapRef(array);
		Marshal.Copy(b, start, buf, len);
	}

	internal static void GetShortArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		short[] b = (short[])pEnv->UnwrapRef(array);
		Marshal.Copy(b, start, buf, len);
	}

	internal static void GetIntArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		int[] b = (int[])pEnv->UnwrapRef(array);
		Marshal.Copy(b, start, buf, len);
	}

	internal static void GetLongArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		long[] b = (long[])pEnv->UnwrapRef(array);
		Marshal.Copy(b, start, buf, len);
	}

	internal static void GetFloatArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		float[] b = (float[])pEnv->UnwrapRef(array);
		Marshal.Copy(b, start, buf, len);
	}

	internal static void GetDoubleArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		double[] b = (double[])pEnv->UnwrapRef(array);
		Marshal.Copy(b, start, buf, len);
	}

	internal static void SetBooleanArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		bool[] b = (bool[])pEnv->UnwrapRef(array);
		sbyte* p = (sbyte*)(void*)buf;
		for(int i = 0; i < len; i++)
		{
			b[start + i] = *p++ != JNI_FALSE;
		}
	}

	internal static void SetByteArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		sbyte[] b = (sbyte[])pEnv->UnwrapRef(array);
		sbyte* p = (sbyte*)(void*)buf;
		for(int i = 0; i < len; i++)
		{
			b[start + i] = *p++;
		}
	}

	internal static void SetCharArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		char[] b = (char[])pEnv->UnwrapRef(array);
		Marshal.Copy(buf, b, start, len);
	}

	internal static void SetShortArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		short[] b = (short[])pEnv->UnwrapRef(array);
		Marshal.Copy(buf, b, start, len);
	}

	internal static void SetIntArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		int[] b = (int[])pEnv->UnwrapRef(array);
		Marshal.Copy(buf, b, start, len);
	}

	internal static void SetLongArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		long[] b = (long[])pEnv->UnwrapRef(array);
		Marshal.Copy(buf, b, start, len);
	}

	internal static void SetFloatArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		float[] b = (float[])pEnv->UnwrapRef(array);
		Marshal.Copy(buf, b, start, len);
	}

	internal static void SetDoubleArrayRegion(JNIEnv* pEnv, IntPtr array, int start, int len, IntPtr buf)
	{
		double[] b = (double[])pEnv->UnwrapRef(array);
		Marshal.Copy(buf, b, start, len);
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct JNINativeMethod
	{
		public byte* name;
		public byte* signature;
		public void* fnPtr;
	}

	internal static int RegisterNatives(JNIEnv* pEnv, IntPtr clazz, JNINativeMethod* methods, int nMethods)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
			wrapper.Finish();
			for(int i = 0; i < nMethods; i++)
			{
				FieldInfo fi = wrapper.TypeAsTBD.GetField("jniptr/" + StringFromUTF8(methods[i].name) + StringFromUTF8(methods[i].signature).Replace('/', '.'), BindingFlags.Static | BindingFlags.NonPublic);
				if(fi == null)
				{
					SetPendingException(pEnv, JavaException.NoSuchMethodError(StringFromUTF8(methods[i].name)));
					return JNI_ERR;
				}
				fi.SetValue(null, (IntPtr)methods[i].fnPtr);
			}
			return JNI_OK;
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, x);
			return JNI_ERR;
		}
	}

	internal static int UnregisterNatives(JNIEnv* pEnv, IntPtr clazz)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(pEnv->UnwrapRef(clazz));
			wrapper.Finish();
			foreach(FieldInfo fi in wrapper.TypeAsTBD.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
			{
				if(fi.Name.StartsWith("jniptr/"))
				{
					fi.SetValue(null, IntPtr.Zero);
				}
			}
			return JNI_OK;
		}
		catch(Exception x)
		{
			SetPendingException(pEnv, x);
			return JNI_ERR;
		}
	}

	internal static int MonitorEnter(JNIEnv* pEnv, IntPtr obj)
	{
		try
		{
			System.Threading.Monitor.Enter(pEnv->UnwrapRef(obj));
			return JNI_OK;
		}
		catch(System.Threading.ThreadInterruptedException)
		{
			SetPendingException(pEnv, JavaException.InterruptedException());
			return JNI_ERR;
		}
	}

	internal static int MonitorExit(JNIEnv* pEnv, IntPtr obj)
	{
		try
		{
			System.Threading.Monitor.Exit(pEnv->UnwrapRef(obj));
			return JNI_OK;
		}
		catch(System.Threading.SynchronizationLockException)
		{
			SetPendingException(pEnv, JavaException.IllegalMonitorStateException());
			return JNI_ERR;
		}
	}

	internal static int GetJavaVM(JNIEnv* pEnv, JavaVM **ppJavaVM)
	{
		*ppJavaVM = JavaVM.pJavaVM;
		return JNI_OK;
	}

	internal static void GetStringRegion(JNIEnv* pEnv, IntPtr str, int start, int len, IntPtr buf)
	{
		string s = (string)pEnv->UnwrapRef(str);
		if(s != null)
		{
			if(start < 0 || start > s.Length || s.Length - start < len)
			{
				SetPendingException(pEnv, JavaException.StringIndexOutOfBoundsException(""));
				return;
			}
			else
			{
				char* p = (char*)(void*)buf;
				// TODO isn't there a managed memcpy?
				for(int i = 0; i < len; i++)
				{
					*p++ = s[start + i];
				}
				return;
			}
		}
		else
		{
			SetPendingException(pEnv, JavaException.NullPointerException());
		}
	}

	internal static void GetStringUTFRegion(JNIEnv* pEnv, IntPtr str, int start, int len, IntPtr buf)
	{
		string s = (string)pEnv->UnwrapRef(str);
		if(s != null)
		{
			if(start < 0 || start > s.Length || s.Length - start < len)
			{
				SetPendingException(pEnv, JavaException.StringIndexOutOfBoundsException(""));
				return;
			}
			else
			{
				byte* p = (byte*)(void*)buf;
				for(int i = 0; i < len; i++)
				{
					char ch = s[start + i];
					if((ch != 0) && (ch <= 0x7F))
					{
						*p++ = (byte)ch;
					}
					else if(ch <= 0x7FF)
					{
						*p++ = (byte)((ch >> 6) | 0xC0);
						*p++ = (byte)((ch & 0x3F) | 0x80);
					}
					else
					{
						*p++ = (byte)((ch >> 12) | 0xE0);
						*p++ = (byte)(((ch >> 6) & 0x3F) | 0x80);
						*p++ = (byte)((ch & 0x3F) | 0x80);
					}
				}
				return;
			}
		}
		else
		{
			SetPendingException(pEnv, JavaException.NullPointerException());
		}
	}

	private static int GetPrimitiveArrayElementSize(Array ar)
	{
		Type type = ar.GetType().GetElementType();
		if(type == typeof(sbyte) || type == typeof(bool))
		{
			return 1;
		}
		else if(type == typeof(short) || type == typeof(char))
		{
			return 2;
		}
		else if(type == typeof(int) || type == typeof(float))
		{
			return 4;
		}
		else if(type == typeof(long) || type == typeof(double))
		{
			return 8;
		}
		else
		{
			JVM.CriticalFailure("invalid array type", null);
			return 0;
		}
	}

	internal static IntPtr GetPrimitiveArrayCritical(JNIEnv* pEnv, IntPtr array, IntPtr isCopy)
	{
		Array ar = (Array)pEnv->UnwrapRef(array);
		int len = ar.Length * GetPrimitiveArrayElementSize(ar);
		GCHandle h = GCHandle.Alloc(ar, GCHandleType.Pinned);
		try
		{
			IntPtr hglobal = Marshal.AllocHGlobal(len);
			byte* pdst = (byte*)(void*)hglobal;
			byte* psrc = (byte*)(void*)h.AddrOfPinnedObject();
			// TODO isn't there a managed memcpy?
			for(int i = 0; i < len; i++)
			{
				*pdst++ = *psrc++;
			}
			if(isCopy != IntPtr.Zero)
			{
				*((sbyte*)(void*)isCopy) = JNI_TRUE;
			}
			return hglobal;
		}
		finally
		{
			h.Free();
		}		
	}

	internal static void ReleasePrimitiveArrayCritical(JNIEnv* pEnv, IntPtr array, IntPtr carray, int mode)
	{
		if(mode == 0 || mode == JNI_COMMIT)
		{
			Array ar = (Array)pEnv->UnwrapRef(array);
			int len = ar.Length * GetPrimitiveArrayElementSize(ar);
			GCHandle h = GCHandle.Alloc(ar, GCHandleType.Pinned);
			try
			{
				byte* pdst = (byte*)(void*)h.AddrOfPinnedObject();
				byte* psrc = (byte*)(void*)carray;
				// TODO isn't there a managed memcpy?
				for(int i = 0; i < len; i++)
				{
					*pdst++ = *psrc++;
				}
			}
			finally
			{
				h.Free();
			}
		}
		if(mode == 0 || mode == JNI_ABORT)
		{
			Marshal.FreeHGlobal(carray);
		}
	}

	internal static IntPtr GetStringCritical(JNIEnv* pEnv, IntPtr str, IntPtr isCopy)
	{
		string s = (string)pEnv->UnwrapRef(str);
		if(s != null)
		{
			if(isCopy != IntPtr.Zero)
			{
				*((sbyte*)(void*)isCopy) = JNI_TRUE;
			}
			return Marshal.StringToHGlobalUni(s);		
		}
		SetPendingException(pEnv, JavaException.NullPointerException());
		return IntPtr.Zero;
	}

	internal static void ReleaseStringCritical(JNIEnv* pEnv, IntPtr str, IntPtr cstring)
	{
		Marshal.FreeHGlobal(cstring);
	}

	internal static sbyte ExceptionCheck(JNIEnv* pEnv)
	{
		return pEnv->UnwrapRef(pEnv->pendingException) != null ? JNI_TRUE : JNI_FALSE;
	}

	internal static void NotImplemented(JNIEnv* pEnv)
	{
		JVM.CriticalFailure("Unimplemented JNIEnv function called", null);
	}

	internal IntPtr MakeLocalRef(object obj)
	{
		return u.activeFrame->MakeLocalRef(obj);
	}

	internal object UnwrapRef(IntPtr o)
	{
		int i = o.ToInt32();
		if(i > 0)
		{
			return u.activeFrame->UnwrapLocalRef(o);
		}
		if(i < 0)
		{
			return GlobalRefs.globalRefs[(-i) - 1];
		}
		return null;
	}
}

unsafe class TlsHack
{
	[ThreadStatic]
	internal static JNIEnv* pJNIEnv;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct JniFrame
{
	private JNIEnv* pJNIEnv;
	private JniFrame* pPrevFrame;
	private LocalRefCache fastlocalrefs;
	internal LocalRefListEntry[] localRefs;
	private RuntimeMethodHandle method;
	// HACK since this isn't a blittable type and C# doesn't allow us to turn the this pointer into a JniFrame*, we need
	// this hack to turn the address of a field into a pointer to the struct (it turns out that on the 1.1 CLR the address
	// of the first field is not equal to the this pointer [for this particular struct])
	private static readonly int jniFramePointerAdjustment = (int)new JNIEnv.Union().activeFrame->GetAddress();

	private byte* GetAddress()
	{
		fixed(void* p = &pJNIEnv)
		{
			return (byte*)p;
		}
	}

	public IntPtr Enter(RuntimeMethodHandle method)
	{
		this.method = method;
		pJNIEnv = TlsHack.pJNIEnv;
		if(pJNIEnv == null)
		{
			// TODO when the thread dies, we're leaking the JNIEnv and the GCHandle
			pJNIEnv = TlsHack.pJNIEnv = (JNIEnv*)Marshal.AllocHGlobal(sizeof(JNIEnv));
			pJNIEnv->vtable = VtableBuilder.vtable;
			pJNIEnv->u.activeFrame = null;
			localRefs = new LocalRefListEntry[32];
			pJNIEnv->localRefs = GCHandle.Alloc(localRefs);
			pJNIEnv->localRefSlot = 0;
			pJNIEnv->pendingException = IntPtr.Zero;
		}
		else
		{
			localRefs = (LocalRefListEntry[])pJNIEnv->localRefs.Target;
		}
		pPrevFrame = pJNIEnv->u.activeFrame;
		pJNIEnv->u.pFrame = GetAddress() - jniFramePointerAdjustment;
		pJNIEnv->localRefSlot++;
		if(pJNIEnv->localRefSlot >= localRefs.Length)
		{
			// TODO instead of bailing out, we should grow the array
			JVM.CriticalFailure("JNI nesting too deep", null);
		}
		fixed(void* p = &pPrevFrame)
		{
			// HACK we assume that the fastlocalrefs struct starts at &pPrevFrame + IntPtr.Size
			localRefs[pJNIEnv->localRefSlot].u.pv = ((byte*)p) + IntPtr.Size;
		}
		return (IntPtr)(void*)pJNIEnv;
	}

	public void Leave()
	{
		Exception x = (Exception)pJNIEnv->UnwrapRef(pJNIEnv->pendingException);
		pJNIEnv->pendingException = IntPtr.Zero;
		pJNIEnv->u.activeFrame = pPrevFrame;
		localRefs[pJNIEnv->localRefSlot].dynamic_list = null;
		// TODO figure out if it is legal to Leave a JNI method while PushLocalFrame is active
		// (i.e. without the corresponding PopLocalFrame)
		pJNIEnv->localRefSlot--;
		if(x != null)
		{
			throw x;
		}
	}

	public static IntPtr GetFuncPtr(RuntimeMethodHandle method, string clazz, string name, string sig)
	{
		MethodBase mb = MethodBase.GetMethodFromHandle(method);
		// MONOBUG Mono 1.0 doesn't implement MethodBase.GetMethodFromHandle
		if(mb == null)
		{
			mb = new StackFrame(1).GetMethod();
		}
		ClassLoaderWrapper loader =	ClassLoaderWrapper.GetWrapperFromType(mb.DeclaringType).GetClassLoader();
		StringBuilder mangledSig = new StringBuilder();
		int sp = 0;
		for(int i = 1; sig[i] != ')'; i++)
		{
			switch(sig[i])
			{
			case '[':
				mangledSig.Append("_3");
				sp += IntPtr.Size;
				while(sig[++i] == '[')
				{
					mangledSig.Append("_3");
				}
				mangledSig.Append(sig[i]);
				if(sig[i] == 'L')
				{
					while(sig[++i] != ';')
					{
						if(sig[i] == '/')
						{
							mangledSig.Append("_");
						}
						else if(sig[i] == '_')
						{
							mangledSig.Append("_1");
						}
						else
						{
							mangledSig.Append(sig[i]);
						}
					}
					mangledSig.Append("_2");
				}
				break;
			case 'L':
				sp += IntPtr.Size;
				mangledSig.Append("L");
				while(sig[++i] != ';')
				{
					if(sig[i] == '/')
					{
						mangledSig.Append("_");
					}
					else if(sig[i] == '_')
					{
						mangledSig.Append("_1");
					}
					else
					{
						mangledSig.Append(sig[i]);
					}
				}
				mangledSig.Append("_2");
				break;
			case 'J':
			case 'D':
				mangledSig.Append(sig[i]);
				sp += 8;
				break;
			case 'F':
			case 'I':
			case 'C':
			case 'Z':
			case 'S':
			case 'B':
				mangledSig.Append(sig[i]);
				sp += 4;
				break;
			default:
				Debug.Assert(false);
				break;
			}
		}
		lock(JniHelper.JniLock)
		{
			string methodName = String.Format("Java_{0}_{1}", clazz.Replace("_", "_1").Replace('/', '_'), name.Replace("_", "_1"));
			foreach(IntPtr p in loader.GetNativeLibraries())
			{
				IntPtr pfunc = JniHelper.ikvm_GetProcAddress(p, methodName, sp + 2 * IntPtr.Size);
				if(pfunc != IntPtr.Zero)
				{
					return pfunc;
				}
			}
			methodName = String.Format("Java_{0}_{1}__{2}", clazz.Replace("_", "_1").Replace('/', '_'), name.Replace("_", "_1"), mangledSig);
			foreach(IntPtr p in loader.GetNativeLibraries())
			{
				IntPtr pfunc = JniHelper.ikvm_GetProcAddress(p, methodName, sp + 2 * IntPtr.Size);
				if(pfunc != IntPtr.Zero)
				{
					return pfunc;
				}
			}
		}
		throw JavaException.UnsatisfiedLinkError("{0}.{1}{2}", clazz, name, sig);
	}

	public IntPtr MakeLocalRef(object obj)
	{
		if(obj == null)
		{
			return IntPtr.Zero;
		}
		int i = localRefs[pJNIEnv->localRefSlot].MakeLocalRef(obj);
		if(i >= 0)
		{
			return (IntPtr)((pJNIEnv->localRefSlot << LocalRefListEntry.LOCAL_REF_SHIFT) + i);
		}
		// TODO consider allocating a new slot (if we do this, the code in
		// PushLocalFrame/PopLocalFrame (and Leave) must be fixed to take this into account)
		JVM.CriticalFailure("Too many JNI local references", null);
		return IntPtr.Zero;
	}

	public object UnwrapLocalRef(IntPtr p)
	{
		int i = p.ToInt32();
		return localRefs[i >> LocalRefListEntry.LOCAL_REF_SHIFT].UnwrapLocalRef(i & LocalRefListEntry.LOCAL_REF_MASK);
	}
}
