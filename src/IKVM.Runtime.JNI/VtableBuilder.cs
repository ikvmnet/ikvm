/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Populates the JNIEnv table of methods with implementations that call back into managed code.
    /// </summary>
    unsafe class VtableBuilder
    {

        delegate int GetMethodArgs(JNIEnv* pEnv, nint p1, byte* p2);

        delegate int pf_int_nint(JNIEnv* pEnv, nint p);
        delegate nint pf_nint_nint(JNIEnv* pEnv, nint p);
        delegate void pf_void_nint(JNIEnv* pEnv, nint p);
        delegate nint pf_nint(JNIEnv* pEnv);
        delegate void pf_void(JNIEnv* pEnv);
        delegate sbyte pf_sbyte(JNIEnv* pEnv);
        delegate nint pf_nint_pbyte(JNIEnv* pEnv, byte* p);
        delegate int pf_int(JNIEnv* pEnv);
        delegate nint pf_nint_pbyte_nint_psbyte_nint(JNIEnv* pEnv, byte* p1, nint p2, sbyte* p3, int p4);
        delegate nint pf_nint_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate jchar* pf_pjchar_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate void pf_void_nint_pvoid_int(JNIEnv* pEnv, nint p1, void* p2, int p3);
        delegate void* pf_pvoid_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate int pf_int_nint_pbyte(JNIEnv* pEnv, nint p1, byte* p2);
        delegate void pf_void_pbyte(JNIEnv* pEnv, byte* p1);
        delegate nint pf_nint_nint_pbyte_pbyte(JNIEnv* pEnv, nint p1, byte* p2, byte* p3);
        delegate int pf_int_nint_pJNINativeMethod_int(JNIEnv* pEnv, nint p1, JNIEnv.JNINativeMethod* p2, int p3);
        delegate int pf_int_ppJavaVM(JNIEnv* pEnv, JavaVM** ppJavaVM);
        delegate sbyte pf_sbyte_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate short pf_short_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate ushort pf_ushort_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate int pf_int_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate long pf_long_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate float pf_float_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate double pf_double_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate void pf_void_nint_nint_nint(JNIEnv* pEnv, nint p1, nint p2, nint p3);
        delegate void pf_void_nint_nint_sbyte(JNIEnv* pEnv, nint p1, nint p2, sbyte p3);
        delegate void pf_void_nint_nint_short(JNIEnv* pEnv, nint p1, nint p2, short p3);
        delegate void pf_void_nint_nint_ushort(JNIEnv* pEnv, nint p1, nint p2, ushort p3);
        delegate void pf_void_nint_nint_int(JNIEnv* pEnv, nint p1, nint p2, int p3);
        delegate void pf_void_nint_nint_long(JNIEnv* pEnv, nint p1, nint p2, long p3);
        delegate void pf_void_nint_nint_float(JNIEnv* pEnv, nint p1, nint p2, float p3);
        delegate void pf_void_nint_nint_double(JNIEnv* pEnv, nint p1, nint p2, double p3);
        delegate nint pf_nint_pjchar_int(JNIEnv* pEnv, jchar* p1, int p2);
        delegate void pf_void_nint_nint(JNIEnv* pEnv, nint p1, nint p2);
        delegate void pf_void_nint_pjchar(JNIEnv* pEnv, nint p1, jchar* p2);
        delegate nint pf_nint_int_nint_nint(JNIEnv* pEnv, int p1, nint p2, nint p3);
        delegate nint pf_nint_nint_int(JNIEnv* pEnv, nint p1, int p2);
        delegate void pf_void_nint_int_nint(JNIEnv* pEnv, nint p1, int p2, nint p3);
        delegate nint pf_nint_int(JNIEnv* pEnv, int p1);
        delegate void pf_void_nint_int_int_nint(JNIEnv* pEnv, nint p1, int p2, int p3, nint p4);
        delegate nint pf_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate sbyte pf_sbyte_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate short pf_short_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate ushort pf_ushort_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate int pf_int_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate long pf_long_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate float pf_float_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate double pf_double_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate void pf_void_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, JNIEnv.jvalue* p3);
        delegate nint pf_nint_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate sbyte pf_sbyte_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate ushort pf_ushort_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate short pf_short_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate int pf_int_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate long pf_long_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate float pf_float_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate double pf_double_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate void pf_void_nint_nint_nint_pjvalue(JNIEnv* pEnv, nint p1, nint p2, nint p3, JNIEnv.jvalue* p4);
        delegate byte* pf_pbyte_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate void pf_void_nint_pbyte(JNIEnv* pEnv, nint p1, byte* p2);
        delegate jboolean* pf_pjboolean_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate jbyte* pf_pjbyte_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate jshort* pf_pjshort_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate jint* pf_pjint_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate jlong* pf_pjlong_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate jfloat* pf_pjfloat_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate jdouble* pf_pjdouble_nint_pjboolean(JNIEnv* pEnv, nint p1, jboolean* p2);
        delegate void pf_void_nint_pjboolean_int(JNIEnv* pEnv, nint p1, jboolean* p2, int p3);
        delegate void pf_void_nint_pjbyte_int(JNIEnv* pEnv, nint p1, jbyte* p2, int p3);
        delegate void pf_void_nint_pjchar_int(JNIEnv* pEnv, nint p1, jchar* p2, int p3);
        delegate void pf_void_nint_pjshort_int(JNIEnv* pEnv, nint p1, jshort* p2, int p3);
        delegate void pf_void_nint_pjint_int(JNIEnv* pEnv, nint p1, jint* p2, int p3);
        delegate void pf_void_nint_pjlong_int(JNIEnv* pEnv, nint p1, jlong* p2, int p3);
        delegate void pf_void_nint_pjfloat_int(JNIEnv* pEnv, nint p1, jfloat* p2, int p3);
        delegate void pf_void_nint_pjdouble_int(JNIEnv* pEnv, nint p1, jdouble* p2, int p3);
        delegate int pf_int_int(JNIEnv* pEnv, int p1);
        delegate nint pf_nint_nint_long(JNIEnv* pEnv, nint p1, long p2);
        delegate long pf_long_nint(JNIEnv* pEnv, nint p1);
        delegate nint pf_nint_nint_nint_sbyte(JNIEnv* pEnv, nint p1, nint p2, sbyte p3);

        internal static void* vtable;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static VtableBuilder()
        {
            JNI.jvmCreated = true;

            // native library provides an initial template with functions for variadic implementations
            var pmcpp = Native.ikvm_GetJNIEnvVTable();
            var p = (void**)JniMemory.Alloc(sizeof(nint) * delegates.Length);
            for (int i = 0; i < delegates.Length; i++)
                p[i] = delegates[i] != null ? (void*)Marshal.GetFunctionPointerForDelegate(delegates[i]) : pmcpp[i];

            vtable = p;
        }

        /// <summary>
        /// Set of delegates for the JNIEnv method table.
        /// </summary>
        static Delegate[] delegates =
        {
            new GetMethodArgs(JNIEnv.GetMethodArgs),
			null,
			null,
			null,

			new pf_int(JNIEnv.GetVersion), //virtual jint JNICALL GetVersion();

			new pf_nint_pbyte_nint_psbyte_nint(JNIEnv.DefineClass), //virtual jclass JNICALL DefineClass(const char *name, jobject loader, const jbyte *buf, jsize len);
			new pf_nint_pbyte(JNIEnv.FindClass), //virtual jclass JNICALL FindClass(const char *name);

			new pf_nint_nint(JNIEnv.FromReflectedMethod), //virtual jmethodID JNICALL FromReflectedMethod(jobject method);
			new pf_nint_nint(JNIEnv.FromReflectedField), //virtual jfieldID JNICALL FromReflectedField(jobject field);
			new pf_nint_nint_nint_sbyte(JNIEnv.ToReflectedMethod), //virtual jobject JNICALL ToReflectedMethod(jclass clazz, jmethodID methodID, jboolean isStatic);

			new pf_nint_nint(JNIEnv.GetSuperclass), //virtual jclass JNICALL GetSuperclass(jclass sub);
			new pf_sbyte_nint_nint(JNIEnv.IsAssignableFrom), //virtual jboolean JNICALL IsAssignableFrom(jclass sub, jclass sup);

			new pf_nint_nint_nint_sbyte(JNIEnv.ToReflectedField), //virtual jobject JNICALL ToReflectedField(jclass clazz, jfieldID fieldID, jboolean isStatic);

			new pf_int_nint(JNIEnv.Throw), //virtual jint JNICALL Throw(jthrowable obj);
			new pf_int_nint_pbyte(JNIEnv.ThrowNew), //virtual jint JNICALL ThrowNew(jclass clazz, const char *msg);
			new pf_nint(JNIEnv.ExceptionOccurred), //virtual jthrowable JNICALL ExceptionOccurred();
			new pf_void(JNIEnv.ExceptionDescribe), //virtual void JNICALL ExceptionDescribe();
			new pf_void(JNIEnv.ExceptionClear), //virtual void JNICALL ExceptionClear();
			new pf_void_pbyte(JNIEnv.FatalError), //virtual void JNICALL FatalError(const char *msg);

			new pf_int_int(JNIEnv.PushLocalFrame), //virtual jint JNICALL PushLocalFrame(jint capacity); 
			new pf_nint_nint(JNIEnv.PopLocalFrame), //virtual jobject JNICALL PopLocalFrame(jobject result);

			new pf_nint_nint(JNIEnv.NewGlobalRef), //virtual jobject JNICALL NewGlobalRef(jobject lobj);
			new pf_void_nint(JNIEnv.DeleteGlobalRef), //virtual void JNICALL DeleteGlobalRef(jobject gref);
			new pf_void_nint(JNIEnv.DeleteLocalRef), //virtual void JNICALL DeleteLocalRef(jobject obj);
			new pf_sbyte_nint_nint(JNIEnv.IsSameObject), //virtual jboolean JNICALL IsSameObject(jobject obj1, jobject obj2);

			new pf_nint_nint(JNIEnv.NewLocalRef), //virtual jobject JNICALL NewLocalRef(jobject ref);
			new pf_int_int(JNIEnv.EnsureLocalCapacity), //virtual jint JNICALL EnsureLocalCapacity(jint capacity);

			new pf_nint_nint(JNIEnv.AllocObject), //virtual jobject JNICALL AllocObject(jclass clazz);
			null, //virtual jobject JNICALL NewObject(jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL NewObjectV(jclass clazz, jmethodID methodID, va_list args);
			new pf_nint_nint_nint_pjvalue(JNIEnv.NewObjectA), //virtual jobject JNICALL NewObjectA(jclass clazz, jmethodID methodID, jvalue *args);

			new pf_nint_nint(JNIEnv.GetObjectClass), //virtual jclass JNICALL GetObjectClass(jobject obj);
			new pf_sbyte_nint_nint(JNIEnv.IsInstanceOf), //virtual jboolean JNICALL IsInstanceOf(jobject obj, jclass clazz);

			new pf_nint_nint_pbyte_pbyte(JNIEnv.GetMethodID), //virtual jmethodID JNICALL GetMethodID(jclass clazz, const char *name, const char *sig);

			null, //virtual jobject JNICALL CallObjectMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallObjectMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_nint_nint_nint_pjvalue(JNIEnv.CallObjectMethodA), //virtual jobject JNICALL CallObjectMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jboolean JNICALL CallBooleanMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallBooleanMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_sbyte_nint_nint_pjvalue(JNIEnv.CallBooleanMethodA), //virtual jboolean JNICALL CallBooleanMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jbyte JNICALL CallByteMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallByteMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_sbyte_nint_nint_pjvalue(JNIEnv.CallByteMethodA), //virtual jbyte JNICALL CallByteMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallCharMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallCharMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_ushort_nint_nint_pjvalue(JNIEnv.CallCharMethodA), //virtual jchar JNICALL CallCharMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallShortMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallShortMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_short_nint_nint_pjvalue(JNIEnv.CallShortMethodA), //virtual jshort JNICALL CallShortMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallIntMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallIntMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_int_nint_nint_pjvalue(JNIEnv.CallIntMethodA), //virtual jint JNICALL CallIntMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallLongMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallLongMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_long_nint_nint_pjvalue(JNIEnv.CallLongMethodA), //virtual jlong JNICALL CallLongMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallFloatMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallFloatMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_float_nint_nint_pjvalue(JNIEnv.CallFloatMethodA), //virtual jfloat JNICALL CallFloatMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallDoubleMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallDoubleMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_double_nint_nint_pjvalue(JNIEnv.CallDoubleMethodA), //virtual jdouble JNICALL CallDoubleMethodA(jobject obj, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallVoidMethod(jobject obj, jmethodID methodID, ...);
			null, //virtual void JNICALL CallVoidMethodV(jobject obj, jmethodID methodID, va_list args);
			new pf_void_nint_nint_pjvalue(JNIEnv.CallVoidMethodA), //virtual void JNICALL CallVoidMethodA(jobject obj, jmethodID methodID, jvalue * args);

			null, //virtual jobject JNICALL CallNonvirtualObjectMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallNonvirtualObjectMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_nint_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualObjectMethodA), //virtual jobject JNICALL CallNonvirtualObjectMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			null, //virtual jboolean JNICALL CallNonvirtualBooleanMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallNonvirtualBooleanMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualBooleanMethodA), //virtual jboolean JNICALL CallNonvirtualBooleanMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			null, //virtual jbyte JNICALL CallNonvirtualByteMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallNonvirtualByteMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualByteMethodA), //virtual jbyte JNICALL CallNonvirtualByteMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallNonvirtualCharMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallNonvirtualCharMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_ushort_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualCharMethodA), //virtual jchar JNICALL CallNonvirtualCharMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallNonvirtualShortMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallNonvirtualShortMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_short_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualShortMethodA), //virtual jshort JNICALL CallNonvirtualShortMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallNonvirtualIntMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallNonvirtualIntMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_int_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualIntMethodA), //virtual jint JNICALL CallNonvirtualIntMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallNonvirtualLongMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallNonvirtualLongMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_long_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualLongMethodA), //virtual jlong JNICALL CallNonvirtualLongMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallNonvirtualFloatMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallNonvirtualFloatMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_float_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualFloatMethodA), //virtual jfloat JNICALL CallNonvirtualFloatMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallNonvirtualDoubleMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallNonvirtualDoubleMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_double_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualDoubleMethodA), //virtual jdouble JNICALL CallNonvirtualDoubleMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallNonvirtualVoidMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
			null, //virtual void JNICALL CallNonvirtualVoidMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
			new pf_void_nint_nint_nint_pjvalue(JNIEnv.CallNonvirtualVoidMethodA), //virtual void JNICALL CallNonvirtualVoidMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

			new pf_nint_nint_pbyte_pbyte(JNIEnv.GetFieldID), //virtual jfieldID JNICALL GetFieldID(jclass clazz, const char *name, const char *sig);

			new pf_nint_nint_nint(JNIEnv.GetObjectField), //virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
			new pf_sbyte_nint_nint(JNIEnv.GetBooleanField), //virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
			new pf_sbyte_nint_nint(JNIEnv.GetByteField), //virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
			new pf_ushort_nint_nint(JNIEnv.GetCharField), //virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
			new pf_short_nint_nint(JNIEnv.GetShortField), //virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
			new pf_int_nint_nint(JNIEnv.GetIntField), //virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
			new pf_long_nint_nint(JNIEnv.GetLongField), //virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
			new pf_float_nint_nint(JNIEnv.GetFloatField), //virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
			new pf_double_nint_nint(JNIEnv.GetDoubleField), //virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

			new pf_void_nint_nint_nint(JNIEnv.SetObjectField), //virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
			new pf_void_nint_nint_sbyte(JNIEnv.SetBooleanField), //virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
			new pf_void_nint_nint_sbyte(JNIEnv.SetByteField), //virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
			new pf_void_nint_nint_ushort(JNIEnv.SetCharField), //virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
			new pf_void_nint_nint_short(JNIEnv.SetShortField), //virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
			new pf_void_nint_nint_int(JNIEnv.SetIntField), //virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
			new pf_void_nint_nint_long(JNIEnv.SetLongField), //virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
			new pf_void_nint_nint_float(JNIEnv.SetFloatField), //virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
			new pf_void_nint_nint_double(JNIEnv.SetDoubleField), //virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

			new pf_nint_nint_pbyte_pbyte(JNIEnv.GetStaticMethodID), //virtual jmethodID JNICALL GetStaticMethodID(jclass clazz, const char *name, const char *sig);

			null, //virtual jobject JNICALL CallStaticObjectMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jobject JNICALL CallStaticObjectMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_nint_nint_nint_pjvalue(JNIEnv.CallStaticObjectMethodA), //virtual jobject JNICALL CallStaticObjectMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jboolean JNICALL CallStaticBooleanMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jboolean JNICALL CallStaticBooleanMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_nint_nint_pjvalue(JNIEnv.CallStaticBooleanMethodA), //virtual jboolean JNICALL CallStaticBooleanMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jbyte JNICALL CallStaticByteMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jbyte JNICALL CallStaticByteMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_sbyte_nint_nint_pjvalue(JNIEnv.CallStaticByteMethodA), //virtual jbyte JNICALL CallStaticByteMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jchar JNICALL CallStaticCharMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jchar JNICALL CallStaticCharMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_ushort_nint_nint_pjvalue(JNIEnv.CallStaticCharMethodA), //virtual jchar JNICALL CallStaticCharMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jshort JNICALL CallStaticShortMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jshort JNICALL CallStaticShortMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_short_nint_nint_pjvalue(JNIEnv.CallStaticShortMethodA), //virtual jshort JNICALL CallStaticShortMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jint JNICALL CallStaticIntMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jint JNICALL CallStaticIntMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_int_nint_nint_pjvalue(JNIEnv.CallStaticIntMethodA), //virtual jint JNICALL CallStaticIntMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jlong JNICALL CallStaticLongMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jlong JNICALL CallStaticLongMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_long_nint_nint_pjvalue(JNIEnv.CallStaticLongMethodA), //virtual jlong JNICALL CallStaticLongMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jfloat JNICALL CallStaticFloatMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jfloat JNICALL CallStaticFloatMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_float_nint_nint_pjvalue(JNIEnv.CallStaticFloatMethodA), //virtual jfloat JNICALL CallStaticFloatMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual jdouble JNICALL CallStaticDoubleMethod(jclass clazz, jmethodID methodID, ...);
			null, //virtual jdouble JNICALL CallStaticDoubleMethodV(jclass clazz, jmethodID methodID, va_list args);
			new pf_double_nint_nint_pjvalue(JNIEnv.CallStaticDoubleMethodA), //virtual jdouble JNICALL CallStaticDoubleMethodA(jclass clazz, jmethodID methodID, jvalue *args);

			null, //virtual void JNICALL CallStaticVoidMethod(jclass cls, jmethodID methodID, ...);
			null, //virtual void JNICALL CallStaticVoidMethodV(jclass cls, jmethodID methodID, va_list args);
			new pf_void_nint_nint_pjvalue(JNIEnv.CallStaticVoidMethodA), //virtual void JNICALL CallStaticVoidMethodA(jclass cls, jmethodID methodID, jvalue * args);

			new pf_nint_nint_pbyte_pbyte(JNIEnv.GetStaticFieldID), //virtual jfieldID JNICALL GetStaticFieldID(jclass clazz, const char *name, const char *sig);

			new pf_nint_nint_nint(JNIEnv.GetStaticObjectField), //virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
			new pf_sbyte_nint_nint(JNIEnv.GetStaticBooleanField), //virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
			new pf_sbyte_nint_nint(JNIEnv.GetStaticByteField), //virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
			new pf_ushort_nint_nint(JNIEnv.GetStaticCharField), //virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
			new pf_short_nint_nint(JNIEnv.GetStaticShortField), //virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
			new pf_int_nint_nint(JNIEnv.GetStaticIntField), //virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
			new pf_long_nint_nint(JNIEnv.GetStaticLongField), //virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
			new pf_float_nint_nint(JNIEnv.GetStaticFloatField), //virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
			new pf_double_nint_nint(JNIEnv.GetStaticDoubleField), //virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

			new pf_void_nint_nint_nint(JNIEnv.SetStaticObjectField), //virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
			new pf_void_nint_nint_sbyte(JNIEnv.SetStaticBooleanField), //virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
			new pf_void_nint_nint_sbyte(JNIEnv.SetStaticByteField), //virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
			new pf_void_nint_nint_ushort(JNIEnv.SetStaticCharField), //virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
			new pf_void_nint_nint_short(JNIEnv.SetStaticShortField), //virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
			new pf_void_nint_nint_int(JNIEnv.SetStaticIntField), //virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
			new pf_void_nint_nint_long(JNIEnv.SetStaticLongField), //virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
			new pf_void_nint_nint_float(JNIEnv.SetStaticFloatField), //virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
			new pf_void_nint_nint_double(JNIEnv.SetStaticDoubleField), //virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

			new pf_nint_pjchar_int(JNIEnv.NewString), //virtual jstring JNICALL NewString(const jchar *unicode, jsize len);
			new pf_int_nint(JNIEnv.GetStringLength), //virtual jsize JNICALL GetStringLength(jstring str);
			new pf_pjchar_nint_pjboolean(JNIEnv.GetStringChars), //virtual const jchar *JNICALL GetStringChars(jstring str, jboolean *isCopy);
			new pf_void_nint_pjchar(JNIEnv.ReleaseStringChars), //virtual void JNICALL ReleaseStringChars(jstring str, const jchar *chars);

			new pf_nint_pbyte(JNIEnv.NewStringUTF), //virtual jstring JNICALL NewStringUTF(const char *utf);
			new pf_int_nint(JNIEnv.GetStringUTFLength), //virtual jsize JNICALL GetStringUTFLength(jstring str);
			new pf_pbyte_nint_pjboolean(JNIEnv.GetStringUTFChars), //virtual const char* JNICALL GetStringUTFChars(jstring str, jboolean *isCopy);
			new pf_void_nint_pbyte(JNIEnv.ReleaseStringUTFChars), //virtual void JNICALL ReleaseStringUTFChars(jstring str, const char* chars);

			new pf_int_nint(JNIEnv.GetArrayLength), //virtual jsize JNICALL GetArrayLength(jarray array);

			new pf_nint_int_nint_nint(JNIEnv.NewObjectArray), //virtual jobjectArray JNICALL NewObjectArray(jsize len, jclass clazz, jobject init);
			new pf_nint_nint_int(JNIEnv.GetObjectArrayElement), //virtual jobject JNICALL GetObjectArrayElement(jobjectArray array, jsize index);
			new pf_void_nint_int_nint(JNIEnv.SetObjectArrayElement), //virtual void JNICALL SetObjectArrayElement(jobjectArray array, jsize index, jobject val);

			new pf_nint_int(JNIEnv.NewBooleanArray), //virtual jbooleanArray JNICALL NewBooleanArray(jsize len);
			new pf_nint_int(JNIEnv.NewByteArray), //virtual jbyteArray JNICALL NewByteArray(jsize len);
			new pf_nint_int(JNIEnv.NewCharArray), //virtual jcharArray JNICALL NewCharArray(jsize len);
			new pf_nint_int(JNIEnv.NewShortArray), //virtual jshortArray JNICALL NewShortArray(jsize len);
			new pf_nint_int(JNIEnv.NewIntArray), //virtual jintArray JNICALL NewIntArray(jsize len);
			new pf_nint_int(JNIEnv.NewLongArray), //virtual jlongArray JNICALL NewLongArray(jsize len);
			new pf_nint_int(JNIEnv.NewFloatArray), //virtual jfloatArray JNICALL NewFloatArray(jsize len);
			new pf_nint_int(JNIEnv.NewDoubleArray), //virtual jdoubleArray JNICALL NewDoubleArray(jsize len);

			new pf_pjboolean_nint_pjboolean(JNIEnv.GetBooleanArrayElements), //virtual jboolean * JNICALL GetBooleanArrayElements(jbooleanArray array, jboolean *isCopy);
			new pf_pjbyte_nint_pjboolean(JNIEnv.GetByteArrayElements), //virtual jbyte * JNICALL GetByteArrayElements(jbyteArray array, jboolean *isCopy);
			new pf_pjchar_nint_pjboolean(JNIEnv.GetCharArrayElements), //virtual jchar * JNICALL GetCharArrayElements(jcharArray array, jboolean *isCopy);
			new pf_pjshort_nint_pjboolean(JNIEnv.GetShortArrayElements), //virtual jshort * JNICALL GetShortArrayElements(jshortArray array, jboolean *isCopy);
			new pf_pjint_nint_pjboolean(JNIEnv.GetIntArrayElements), //virtual jint * JNICALL GetIntArrayElements(jintArray array, jboolean *isCopy);
			new pf_pjlong_nint_pjboolean(JNIEnv.GetLongArrayElements), //virtual jlong * JNICALL GetLongArrayElements(jlongArray array, jboolean *isCopy);
			new pf_pjfloat_nint_pjboolean(JNIEnv.GetFloatArrayElements), //virtual jfloat * JNICALL GetFloatArrayElements(jfloatArray array, jboolean *isCopy);
			new pf_pjdouble_nint_pjboolean(JNIEnv.GetDoubleArrayElements), //virtual jdouble * JNICALL GetDoubleArrayElements(jdoubleArray array, jboolean *isCopy);

			new pf_void_nint_pjboolean_int(JNIEnv.ReleaseBooleanArrayElements), //virtual void JNICALL ReleaseBooleanArrayElements(jbooleanArray array, jboolean *elems, jint mode);
			new pf_void_nint_pjbyte_int(JNIEnv.ReleaseByteArrayElements), //virtual void JNICALL ReleaseByteArrayElements(jbyteArray array, jbyte *elems, jint mode);
			new pf_void_nint_pjchar_int(JNIEnv.ReleaseCharArrayElements), //virtual void JNICALL ReleaseCharArrayElements(jcharArray array, jchar *elems, jint mode);
			new pf_void_nint_pjshort_int(JNIEnv.ReleaseShortArrayElements), //virtual void JNICALL ReleaseShortArrayElements(jshortArray array, jshort *elems, jint mode);
			new pf_void_nint_pjint_int(JNIEnv.ReleaseIntArrayElements), //virtual void JNICALL ReleaseIntArrayElements(jintArray array, jint *elems, jint mode);
			new pf_void_nint_pjlong_int(JNIEnv.ReleaseLongArrayElements), //virtual void JNICALL ReleaseLongArrayElements(jlongArray array, jlong *elems, jint mode);
			new pf_void_nint_pjfloat_int(JNIEnv.ReleaseFloatArrayElements), //virtual void JNICALL ReleaseFloatArrayElements(jfloatArray array, jfloat *elems, jint mode);
			new pf_void_nint_pjdouble_int(JNIEnv.ReleaseDoubleArrayElements), //virtual void JNICALL ReleaseDoubleArrayElements(jdoubleArray array, jdouble *elems, jint mode);

			new pf_void_nint_int_int_nint(JNIEnv.GetBooleanArrayRegion), //virtual void JNICALL GetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetByteArrayRegion), //virtual void JNICALL GetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetCharArrayRegion), //virtual void JNICALL GetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetShortArrayRegion), //virtual void JNICALL GetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetIntArrayRegion), //virtual void JNICALL GetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetLongArrayRegion), //virtual void JNICALL GetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetFloatArrayRegion), //virtual void JNICALL GetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetDoubleArrayRegion), //virtual void JNICALL GetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

			new pf_void_nint_int_int_nint(JNIEnv.SetBooleanArrayRegion), //virtual void JNICALL SetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetByteArrayRegion), //virtual void JNICALL SetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetCharArrayRegion), //virtual void JNICALL SetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetShortArrayRegion), //virtual void JNICALL SetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetIntArrayRegion), //virtual void JNICALL SetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetLongArrayRegion), //virtual void JNICALL SetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetFloatArrayRegion), //virtual void JNICALL SetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
			new pf_void_nint_int_int_nint(JNIEnv.SetDoubleArrayRegion), //virtual void JNICALL SetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

			new pf_int_nint_pJNINativeMethod_int(JNIEnv.RegisterNatives), //virtual jint JNICALL RegisterNatives(jclass clazz, const JNINativeMethod *methods, jint nMethods);
			new pf_int_nint(JNIEnv.UnregisterNatives), //virtual jint JNICALL UnregisterNatives(jclass clazz);

			new pf_int_nint(JNIEnv.MonitorEnter), //virtual jint JNICALL MonitorEnter(jobject obj);
			new pf_int_nint(JNIEnv.MonitorExit), //virtual jint JNICALL MonitorExit(jobject obj);

			new pf_int_ppJavaVM(JNIEnv.GetJavaVM), //virtual jint JNICALL GetJavaVM(JavaVM **vm);

			new pf_void_nint_int_int_nint(JNIEnv.GetStringRegion), //virtual void JNICALL GetStringRegion(jstring str, jsize start, jsize len, jchar *buf);
			new pf_void_nint_int_int_nint(JNIEnv.GetStringUTFRegion), //virtual void JNICALL GetStringUTFRegion(jstring str, jsize start, jsize len, char *buf);

			new pf_pvoid_nint_pjboolean(JNIEnv.GetPrimitiveArrayCritical), //virtual void* JNICALL GetPrimitiveArrayCritical(jarray array, jboolean *isCopy);
			new pf_void_nint_pvoid_int(JNIEnv.ReleasePrimitiveArrayCritical), //virtual void JNICALL ReleasePrimitiveArrayCritical(jarray array, void *carray, jint mode);

			new pf_pjchar_nint_pjboolean(JNIEnv.GetStringCritical), //virtual const jchar* JNICALL GetStringCritical(jstring string, jboolean *isCopy);
			new pf_void_nint_pjchar(JNIEnv.ReleaseStringCritical), //virtual void JNICALL ReleaseStringCritical(jstring string, const jchar *cstring);

			new pf_nint_nint(JNIEnv.NewWeakGlobalRef), //virtual jweak JNICALL NewWeakGlobalRef(jobject obj);
			new pf_void_nint(JNIEnv.DeleteWeakGlobalRef), //virtual void JNICALL DeleteWeakGlobalRef(jweak ref);

			new pf_sbyte(JNIEnv.ExceptionCheck), //virtual jboolean JNICALL ExceptionCheck();

			new pf_nint_nint_long(JNIEnv.NewDirectByteBuffer), //virtual jobject JNICALL NewDirectByteBuffer(void* address, jlong capacity);
			new pf_nint_nint(JNIEnv.GetDirectBufferAddress), //virtual void* JNICALL GetDirectBufferAddress(jobject buf);
			new pf_long_nint(JNIEnv.GetDirectBufferCapacity), //virtual jlong JNICALL GetDirectBufferCapacity(jobject buf);

			new pf_int_nint(JNIEnv.GetObjectRefType) // virtual jobjectRefType GetObjectRefType(jobject obj);
		};

    }

}
