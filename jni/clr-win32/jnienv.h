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

#include <vcclr.h>

using namespace System;
using namespace System::Runtime::InteropServices;

#pragma unmanaged

#define JNI_TRUE 1
#define JNI_FALSE 0

typedef struct {
	char *name;
	char *signature;
	void *fnPtr;
} JNINativeMethod;

#define JNI_VERSION_1_1 0x00010001
#define JNI_VERSION_1_2 0x00010002
#define JNI_VERSION_1_4 0x00010004

/*
 * possible return values for JNI functions.
 */

#define JNI_OK			0
#define JNI_ERR			(-1)
#define JNI_EDETACHED	(-2)
#define JNI_EVERSION	(-3)
/*
 * used in ReleaseScalarArrayElements
 */
  
#define JNI_COMMIT 1
#define JNI_ABORT 2

class JavaVM;

#define JNIEXPORT
#define JNICALL __stdcall

#ifndef _HELPERTYPES_
#define _HELPERTYPES_
class _jobject {};
class _jclass : public _jobject {};
class _jstring : public _jobject {};
class _jthrowable : public _jobject {};
class Array$ : public _jobject {};
class ObjectArray$ : public Array$ {};
class BooleanArray$ : public Array$ {};
class ByteArray$ : public Array$ {};
class CharArray$ : public Array$ {};
class ShortArray$ : public Array$ {};
class IntArray$ : public Array$ {};
class LongArray$ : public Array$ {};
class FloatArray$ : public Array$ {};
class DoubleArray$ : public Array$ {};
#endif //_HELPERTYPES_

typedef class _jclass* jclass;
typedef class _jobject* jobject;
typedef jobject jweak;
typedef class _jstring* jstring;
typedef class _jthrowable* jthrowable;
typedef class Array$* jarray;
typedef class ObjectArray$* jobjectArray;
typedef class BooleanArray$* jbooleanArray;
typedef class ByteArray$* jbyteArray;
typedef class CharArray$* jcharArray;
typedef class ShortArray$* jshortArray;
typedef class IntArray$* jintArray;
typedef class LongArray$* jlongArray;
typedef class FloatArray$* jfloatArray;
typedef class DoubleArray$* jdoubleArray;
typedef struct _jmethodID* jmethodID;
typedef struct _jfieldID* jfieldID;

struct _jmethodID
{
};

struct _jfieldID
{
};

/*
 * JNI Types
 */

typedef unsigned char   jboolean;
typedef unsigned short  jchar;
typedef short           jshort;
typedef float           jfloat;
typedef double          jdouble;

typedef long            jint;
typedef __int64         jlong;
typedef signed char     jbyte;

typedef jint            jsize;

typedef union jvalue {
	jboolean z;
	jbyte    b;
	jchar    c;
	jshort   s;
	jint     i;
	jlong    j;
	jfloat   f;
	jdouble  d;
	jobject  l;
} jvalue;

//public __value class LocalRefStruct;

#pragma managed

//[StructLayout(LayoutKind::Sequential)]
//__value struct LocalRefCache
//{
//	Object* loc1;
//	Object* loc2;
//	Object* loc3;
//	Object* loc4;
//	Object* loc5;
//	Object* loc6;
//	Object* loc7;
//	Object* loc8;
//	Object* loc9;
//	Object* loc10;
//};
//
//#define STATIC_LIST_SIZE 10
//#define BUCKET_SIZE (1 << LOCAL_REF_SHIFT)
//#define LOCAL_REF_SHIFT 10
//#define LOCAL_REF_MASK (BUCKET_SIZE - 1)
//
//__value struct LocalRefListEntry
//{
//	Object* __nogc* static_list;
//	Object* dynamic_list __gc[];
//
//	int MakeLocalRef(Object* o)
//	{
//		Object** p = static_list;
//		for(int i = 0; i < STATIC_LIST_SIZE; i++)
//		{
//			if(p[i] == 0)
//			{
//				p[i] = o;
//				return i;
//			}
//		}
//		if(!dynamic_list)
//		{
//			dynamic_list = new Object* __gc[32 - STATIC_LIST_SIZE];
//		}
//		for(int i = 0; i < dynamic_list->Length; i++)
//		{
//			if(dynamic_list[i] == 0)
//			{
//				dynamic_list[i] = o;
//				return i + STATIC_LIST_SIZE;
//			}
//		}
//		int newsize = (dynamic_list->Length + STATIC_LIST_SIZE) * 2 - STATIC_LIST_SIZE;
//		if(newsize > BUCKET_SIZE)
//		{
//			return -1;
//		}
//		Object* tmp __gc[] = dynamic_list;
//		dynamic_list = new Object* __gc[newsize];
//		Array::Copy(tmp, 0, dynamic_list, 0, tmp->Length);
//		dynamic_list[tmp->Length] = o;
//		return tmp->Length + STATIC_LIST_SIZE;
//	}
//
//	void DeleteLocalRef(unsigned int i)
//	{
//		if(i < STATIC_LIST_SIZE)
//		{
//			static_list[i] = 0;
//		}
//		else
//		{
//			dynamic_list[i - STATIC_LIST_SIZE] = 0;
//		}
//	}
//
//	Object* UnwrapLocalRef(unsigned int i)
//	{
//		if(i < STATIC_LIST_SIZE)
//		{
//			return static_list[i];
//		}
//		else
//		{
//			return dynamic_list[i - STATIC_LIST_SIZE];
//		}
//	}
//};
//
//__gc struct GlobalRefs
//{
//	static System::Collections::ArrayList* globalRefs = new System::Collections::ArrayList();
//};
//
//class JNIEnv;
//
//public __value class LocalRefStruct
//{
//	JNIEnv* pJNIEnv;
//	LocalRefStruct __nogc* pPrevLocalRefCache;
//	LocalRefCache fastlocalrefs;
//	LocalRefListEntry localRefs __gc[];
//
//public:
//	static JNIEnv* GetEnv();
//
//	IntPtr Enter();
//	void Leave();
//
//	IntPtr MakeLocalRef(Object* o);
//	Object* UnwrapLocalRef(IntPtr p);
//};

class JNIEnv
{
public:
	jobject MakeLocalRef(System::Object* obj);
	//{
	//	return (jobject)(void*)pActiveLocalRefCache->MakeLocalRef(obj);
	//}

	Object* UnwrapRef(jobject o);
	//{
	//	int i = (int)o;
	//	if(i > 0)
	//	{
	//		return pActiveLocalRefCache->UnwrapLocalRef((void*)o);
	//	}
	//	if(i < 0)
	//	{
	//		return GlobalRefs::globalRefs->Item[(-i) - 1];
	//	}
	//	return 0;
	//}

	jmethodID FindMethodID(jclass cls, const char* name, const char* sig, bool isstatic);
	Object* InvokeHelper(jobject object, jmethodID methodID, jvalue* args, bool nonVirtual);
	jfieldID FindFieldID(jclass cls, const char* name, const char* sig, bool isstatic);

	//int localRefSlot;
	//LocalRefStruct __nogc* pActiveLocalRefCache;
	//gcroot<LocalRefListEntry __gc[]> localRefs;
	//jthrowable pendingException;

	JNIEnv();
	~JNIEnv();

	virtual void JNICALL reserved0();
	virtual void JNICALL reserved1();
	virtual void JNICALL reserved2();
	virtual void JNICALL reserved3();

	virtual jint JNICALL GetVersion();

	virtual jclass JNICALL DefineClass(const char *name, jobject loader, const jbyte *buf, jsize len);
	virtual jclass JNICALL FindClass(const char *name);

	virtual jmethodID JNICALL FromReflectedMethod(jobject method);
	virtual jfieldID JNICALL FromReflectedField(jobject field);
	virtual jobject JNICALL ToReflectedMethod(jclass clazz, jmethodID methodID);

	virtual jclass JNICALL GetSuperclass(jclass sub);
	virtual jboolean JNICALL IsAssignableFrom(jclass sub, jclass sup);

	virtual jobject JNICALL ToReflectedField(jclass clazz, jfieldID fieldID);

	virtual jint JNICALL Throw(jthrowable obj);
	virtual jint JNICALL ThrowNew(jclass clazz, const char *msg);
	virtual jthrowable JNICALL ExceptionOccurred();
	virtual void JNICALL ExceptionDescribe();
	virtual void JNICALL ExceptionClear();
	virtual void JNICALL FatalError(const char *msg);

	virtual jint JNICALL PushLocalFrame(jint capacity); 
	virtual jobject JNICALL PopLocalFrame(jobject result);

	virtual jobject JNICALL NewGlobalRef(jobject lobj);
	virtual void JNICALL DeleteGlobalRef(jobject gref);
	virtual void JNICALL DeleteLocalRef(jobject obj);
	virtual jboolean JNICALL IsSameObject(jobject obj1, jobject obj2);

	virtual jobject JNICALL NewLocalRef(jobject ref);
	virtual jint JNICALL EnsureLocalCapacity(jint capacity); 

	virtual jobject JNICALL AllocObject(jclass clazz);
	virtual jobject JNICALL NewObject(jclass clazz, jmethodID methodID, ...);
	virtual jobject JNICALL NewObjectV(jclass clazz, jmethodID methodID, va_list args);
	virtual jobject JNICALL NewObjectA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jclass JNICALL GetObjectClass(jobject obj);
	virtual jboolean JNICALL IsInstanceOf(jobject obj, jclass clazz);

	virtual jmethodID JNICALL GetMethodID(jclass clazz, const char *name, const char *sig);

	virtual jobject JNICALL CallObjectMethod(jobject obj, jmethodID methodID, ...);
	virtual jobject JNICALL CallObjectMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jobject JNICALL CallObjectMethodA(jobject obj, jmethodID methodID, jvalue * args);

	virtual jboolean JNICALL CallBooleanMethod(jobject obj, jmethodID methodID, ...);
	virtual jboolean JNICALL CallBooleanMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jboolean JNICALL CallBooleanMethodA(jobject obj, jmethodID methodID, jvalue * args);

	virtual jbyte JNICALL CallByteMethod(jobject obj, jmethodID methodID, ...);
	virtual jbyte JNICALL CallByteMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jbyte JNICALL CallByteMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual jchar JNICALL CallCharMethod(jobject obj, jmethodID methodID, ...);
	virtual jchar JNICALL CallCharMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jchar JNICALL CallCharMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual jshort JNICALL CallShortMethod(jobject obj, jmethodID methodID, ...);
	virtual jshort JNICALL CallShortMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jshort JNICALL CallShortMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual jint JNICALL CallIntMethod(jobject obj, jmethodID methodID, ...);
	virtual jint JNICALL CallIntMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jint JNICALL CallIntMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual jlong JNICALL CallLongMethod(jobject obj, jmethodID methodID, ...);
	virtual jlong JNICALL CallLongMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jlong JNICALL CallLongMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual jfloat JNICALL CallFloatMethod(jobject obj, jmethodID methodID, ...);
	virtual jfloat JNICALL CallFloatMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jfloat JNICALL CallFloatMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual jdouble JNICALL CallDoubleMethod(jobject obj, jmethodID methodID, ...);
	virtual jdouble JNICALL CallDoubleMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual jdouble JNICALL CallDoubleMethodA(jobject obj, jmethodID methodID, jvalue *args);

	virtual void JNICALL CallVoidMethod(jobject obj, jmethodID methodID, ...);
	virtual void JNICALL CallVoidMethodV(jobject obj, jmethodID methodID, va_list args);
	virtual void JNICALL CallVoidMethodA(jobject obj, jmethodID methodID, jvalue * args);

	virtual jobject JNICALL CallNonvirtualObjectMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jobject JNICALL CallNonvirtualObjectMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jobject JNICALL CallNonvirtualObjectMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

	virtual jboolean JNICALL CallNonvirtualBooleanMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jboolean JNICALL CallNonvirtualBooleanMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jboolean JNICALL CallNonvirtualBooleanMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

	virtual jbyte JNICALL CallNonvirtualByteMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jbyte JNICALL CallNonvirtualByteMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jbyte JNICALL CallNonvirtualByteMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual jchar JNICALL CallNonvirtualCharMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jchar JNICALL CallNonvirtualCharMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jchar JNICALL CallNonvirtualCharMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual jshort JNICALL CallNonvirtualShortMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jshort JNICALL CallNonvirtualShortMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jshort JNICALL CallNonvirtualShortMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual jint JNICALL CallNonvirtualIntMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jint JNICALL CallNonvirtualIntMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jint JNICALL CallNonvirtualIntMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual jlong JNICALL CallNonvirtualLongMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jlong JNICALL CallNonvirtualLongMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jlong JNICALL CallNonvirtualLongMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual jfloat JNICALL CallNonvirtualFloatMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jfloat JNICALL CallNonvirtualFloatMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jfloat JNICALL CallNonvirtualFloatMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual jdouble JNICALL CallNonvirtualDoubleMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual jdouble JNICALL CallNonvirtualDoubleMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual jdouble JNICALL CallNonvirtualDoubleMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args);

	virtual void JNICALL CallNonvirtualVoidMethod(jobject obj, jclass clazz, jmethodID methodID, ...);
	virtual void JNICALL CallNonvirtualVoidMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args);
	virtual void JNICALL CallNonvirtualVoidMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args);

	virtual jfieldID JNICALL GetFieldID(jclass clazz, const char *name, const char *sig);

	virtual jobject JNICALL GetObjectField(jobject obj, jfieldID fieldID);
	virtual jboolean JNICALL GetBooleanField(jobject obj, jfieldID fieldID);
	virtual jbyte JNICALL GetByteField(jobject obj, jfieldID fieldID);
	virtual jchar JNICALL GetCharField(jobject obj, jfieldID fieldID);
	virtual jshort JNICALL GetShortField(jobject obj, jfieldID fieldID);
	virtual jint JNICALL GetIntField(jobject obj, jfieldID fieldID);
	virtual jlong JNICALL GetLongField(jobject obj, jfieldID fieldID);
	virtual jfloat JNICALL GetFloatField(jobject obj, jfieldID fieldID);
	virtual jdouble JNICALL GetDoubleField(jobject obj, jfieldID fieldID);

	virtual void JNICALL SetObjectField(jobject obj, jfieldID fieldID, jobject val);
	virtual void JNICALL SetBooleanField(jobject obj, jfieldID fieldID, jboolean val);
	virtual void JNICALL SetByteField(jobject obj, jfieldID fieldID, jbyte val);
	virtual void JNICALL SetCharField(jobject obj, jfieldID fieldID, jchar val);
	virtual void JNICALL SetShortField(jobject obj, jfieldID fieldID, jshort val);
	virtual void JNICALL SetIntField(jobject obj, jfieldID fieldID, jint val);
	virtual void JNICALL SetLongField(jobject obj, jfieldID fieldID, jlong val);
	virtual void JNICALL SetFloatField(jobject obj, jfieldID fieldID, jfloat val);
	virtual void JNICALL SetDoubleField(jobject obj, jfieldID fieldID, jdouble val);

	virtual jmethodID JNICALL GetStaticMethodID(jclass clazz, const char *name, const char *sig);

	virtual jobject JNICALL CallStaticObjectMethod(jclass clazz, jmethodID methodID, ...);
	virtual jobject JNICALL CallStaticObjectMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jobject JNICALL CallStaticObjectMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jboolean JNICALL CallStaticBooleanMethod(jclass clazz, jmethodID methodID, ...);
	virtual jboolean JNICALL CallStaticBooleanMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jboolean JNICALL CallStaticBooleanMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jbyte JNICALL CallStaticByteMethod(jclass clazz, jmethodID methodID, ...);
	virtual jbyte JNICALL CallStaticByteMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jbyte JNICALL CallStaticByteMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jchar JNICALL CallStaticCharMethod(jclass clazz, jmethodID methodID, ...);
	virtual jchar JNICALL CallStaticCharMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jchar JNICALL CallStaticCharMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jshort JNICALL CallStaticShortMethod(jclass clazz, jmethodID methodID, ...);
	virtual jshort JNICALL CallStaticShortMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jshort JNICALL CallStaticShortMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jint JNICALL CallStaticIntMethod(jclass clazz, jmethodID methodID, ...);
	virtual jint JNICALL CallStaticIntMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jint JNICALL CallStaticIntMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jlong JNICALL CallStaticLongMethod(jclass clazz, jmethodID methodID, ...);
	virtual jlong JNICALL CallStaticLongMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jlong JNICALL CallStaticLongMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jfloat JNICALL CallStaticFloatMethod(jclass clazz, jmethodID methodID, ...);
	virtual jfloat JNICALL CallStaticFloatMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jfloat JNICALL CallStaticFloatMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual jdouble JNICALL CallStaticDoubleMethod(jclass clazz, jmethodID methodID, ...);
	virtual jdouble JNICALL CallStaticDoubleMethodV(jclass clazz, jmethodID methodID, va_list args);
	virtual jdouble JNICALL CallStaticDoubleMethodA(jclass clazz, jmethodID methodID, jvalue *args);

	virtual void JNICALL CallStaticVoidMethod(jclass cls, jmethodID methodID, ...);
	virtual void JNICALL CallStaticVoidMethodV(jclass cls, jmethodID methodID, va_list args);
	virtual void JNICALL CallStaticVoidMethodA(jclass cls, jmethodID methodID, jvalue * args);

	virtual jfieldID JNICALL GetStaticFieldID(jclass clazz, const char *name, const char *sig);
	virtual jobject JNICALL GetStaticObjectField(jclass clazz, jfieldID fieldID);
	virtual jboolean JNICALL GetStaticBooleanField(jclass clazz, jfieldID fieldID);
	virtual jbyte JNICALL GetStaticByteField(jclass clazz, jfieldID fieldID);
	virtual jchar JNICALL GetStaticCharField(jclass clazz, jfieldID fieldID);
	virtual jshort JNICALL GetStaticShortField(jclass clazz, jfieldID fieldID);
	virtual jint JNICALL GetStaticIntField(jclass clazz, jfieldID fieldID);
	virtual jlong JNICALL GetStaticLongField(jclass clazz, jfieldID fieldID);
	virtual jfloat JNICALL GetStaticFloatField(jclass clazz, jfieldID fieldID);
	virtual jdouble JNICALL GetStaticDoubleField(jclass clazz, jfieldID fieldID);

	virtual void JNICALL SetStaticObjectField(jclass clazz, jfieldID fieldID, jobject value);
	virtual void JNICALL SetStaticBooleanField(jclass clazz, jfieldID fieldID, jboolean value);
	virtual void JNICALL SetStaticByteField(jclass clazz, jfieldID fieldID, jbyte value);
	virtual void JNICALL SetStaticCharField(jclass clazz, jfieldID fieldID, jchar value);
	virtual void JNICALL SetStaticShortField(jclass clazz, jfieldID fieldID, jshort value);
	virtual void JNICALL SetStaticIntField(jclass clazz, jfieldID fieldID, jint value);
	virtual void JNICALL SetStaticLongField(jclass clazz, jfieldID fieldID, jlong value);
	virtual void JNICALL SetStaticFloatField(jclass clazz, jfieldID fieldID, jfloat value);
	virtual void JNICALL SetStaticDoubleField(jclass clazz, jfieldID fieldID, jdouble value);

	virtual jstring JNICALL NewString(const jchar *unicode, jsize len);
	virtual jsize JNICALL GetStringLength(jstring str);
	virtual const jchar *JNICALL GetStringChars(jstring str, jboolean *isCopy);
	virtual void JNICALL ReleaseStringChars(jstring str, const jchar *chars);

	virtual jstring JNICALL NewStringUTF(const char *utf);
	virtual jsize JNICALL GetStringUTFLength(jstring str);
	virtual const char* JNICALL GetStringUTFChars(jstring str, jboolean *isCopy);
	virtual void JNICALL ReleaseStringUTFChars(jstring str, const char* chars);

	virtual jsize JNICALL GetArrayLength(jarray array);

	virtual jobjectArray JNICALL NewObjectArray(jsize len, jclass clazz, jobject init);
	virtual jobject JNICALL GetObjectArrayElement(jobjectArray array, jsize index);
	virtual void JNICALL SetObjectArrayElement(jobjectArray array, jsize index, jobject val);

	virtual jbooleanArray JNICALL NewBooleanArray(jsize len);
	virtual jbyteArray JNICALL NewByteArray(jsize len);
	virtual jcharArray JNICALL NewCharArray(jsize len);
	virtual jshortArray JNICALL NewShortArray(jsize len);
	virtual jintArray JNICALL NewIntArray(jsize len);
	virtual jlongArray JNICALL NewLongArray(jsize len);
	virtual jfloatArray JNICALL NewFloatArray(jsize len);
	virtual jdoubleArray JNICALL NewDoubleArray(jsize len);

	virtual jboolean * JNICALL GetBooleanArrayElements(jbooleanArray array, jboolean *isCopy);
	virtual jbyte * JNICALL GetByteArrayElements(jbyteArray array, jboolean *isCopy);
	virtual jchar * JNICALL GetCharArrayElements(jcharArray array, jboolean *isCopy);
	virtual jshort * JNICALL GetShortArrayElements(jshortArray array, jboolean *isCopy);
	virtual jint * JNICALL GetIntArrayElements(jintArray array, jboolean *isCopy);
	virtual jlong * JNICALL GetLongArrayElements(jlongArray array, jboolean *isCopy);
	virtual jfloat * JNICALL GetFloatArrayElements(jfloatArray array, jboolean *isCopy);
	virtual jdouble * JNICALL GetDoubleArrayElements(jdoubleArray array, jboolean *isCopy);

	virtual void JNICALL ReleaseBooleanArrayElements(jbooleanArray array, jboolean *elems, jint mode);
	virtual void JNICALL ReleaseByteArrayElements(jbyteArray array, jbyte *elems, jint mode);
	virtual void JNICALL ReleaseCharArrayElements(jcharArray array, jchar *elems, jint mode);
	virtual void JNICALL ReleaseShortArrayElements(jshortArray array, jshort *elems, jint mode);
	virtual void JNICALL ReleaseIntArrayElements(jintArray array, jint *elems, jint mode);
	virtual void JNICALL ReleaseLongArrayElements(jlongArray array, jlong *elems, jint mode);
	virtual void JNICALL ReleaseFloatArrayElements(jfloatArray array, jfloat *elems, jint mode);
	virtual void JNICALL ReleaseDoubleArrayElements(jdoubleArray array, jdouble *elems, jint mode);

	virtual void JNICALL GetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
	virtual void JNICALL GetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
	virtual void JNICALL GetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
	virtual void JNICALL GetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
	virtual void JNICALL GetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
	virtual void JNICALL GetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
	virtual void JNICALL GetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
	virtual void JNICALL GetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

	virtual void JNICALL SetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf);
	virtual void JNICALL SetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf);
	virtual void JNICALL SetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf);
	virtual void JNICALL SetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf);
	virtual void JNICALL SetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf);
	virtual void JNICALL SetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf);
	virtual void JNICALL SetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf);
	virtual void JNICALL SetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf);

	virtual jint JNICALL RegisterNatives(jclass clazz, const JNINativeMethod *methods, jint nMethods);
	virtual jint JNICALL UnregisterNatives(jclass clazz);

	virtual jint JNICALL MonitorEnter(jobject obj);
	virtual jint JNICALL MonitorExit(jobject obj);

	virtual jint JNICALL GetJavaVM(JavaVM **vm);

	virtual void JNICALL GetStringRegion(jstring str, jsize start, jsize len, jchar *buf);
	virtual void JNICALL GetStringUTFRegion(jstring str, jsize start, jsize len, char *buf);

	virtual void* JNICALL GetPrimitiveArrayCritical(jarray array, jboolean *isCopy);
	virtual void JNICALL ReleasePrimitiveArrayCritical(jarray array, void *carray, jint mode);

	virtual const jchar* JNICALL GetStringCritical(jstring string, jboolean *isCopy);
	virtual void JNICALL ReleaseStringCritical(jstring string, const jchar *cstring);

	virtual jweak JNICALL NewWeakGlobalRef(jobject obj);
	virtual void JNICALL DeleteWeakGlobalRef(jweak ref);

	virtual jboolean JNICALL ExceptionCheck();

	virtual jobject JNICALL NewDirectByteBuffer(void* address, jlong capacity);
	virtual void* JNICALL GetDirectBufferAddress(jobject buf);
	virtual jlong JNICALL GetDirectBufferCapacity(jobject buf);
};

class JavaVM
{
public:
	virtual void JNICALL reserved0();
	virtual void JNICALL reserved1();
	virtual void JNICALL reserved2();
	virtual jint JNICALL DestroyJavaVM();
	virtual jint JNICALL AttachCurrentThread(void **penv, void *args);
	virtual jint JNICALL DetachCurrentThread();
	virtual jint JNICALL GetEnv(void **penv, jint version);
	virtual jint JNICALL AttachCurrentThreadAsDaemon(void **penv, void *args);
};
