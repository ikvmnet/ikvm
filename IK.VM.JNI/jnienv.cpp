/*
  Copyright (C) 2002 Jeroen Frijters

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

#include "stdafx.h"
#include "jnienv.h"
#include "jni.h"
#include <malloc.h>
#include <stdarg.h>

#define DEBUG
#undef NDEBUG

#include <assert.h>

#define pLocalRefs (LocalRefStruct::Current())

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Reflection;

// this struct exists to ensure the right compile switch is used, if it fails
// to compile, you must compile with /Zc:wchar_t
struct wchar_t_must_be_builtin
{
	void foo(wchar_t t) {}
	void foo(unsigned short t) {}
};

#pragma managed

String* StringFromUTF8(const char* psz)
{
	// Sun's modified UTF8 encoding is not compatible with System::Text::Encoding::UTF8, so
	// we need to roll our own
	int len = 0;
	while(psz[len]) len++;
	System::Text::StringBuilder* sb = new System::Text::StringBuilder(len);
	for(int i = 0; i < len; i++)
	{
		int c = (unsigned char)*psz++;
		int char2, char3;
		switch (c >> 4)
		{
		case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
			// 0xxxxxxx
			break;
		case 12: case 13:
			// 110x xxxx   10xx xxxx
			char2 = *psz++;
			i++;
			c = (((c & 0x1F) << 6) | (char2 & 0x3F));
			break;
		case 14:
			// 1110 xxxx  10xx xxxx  10xx xxxx
			char2 = *psz++;
			char3 = *psz++;
			i++;
			i++;
			c = (((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0));
			break;
		}
		sb->Append((wchar_t)c);
	}
	return sb->ToString();
}

JNIEnv::JNIEnv()
{
}

JNIEnv::~JNIEnv()
{
}

Object* JNIEnv::UnwrapRef(jobject o)
{
	int i = (int)o;
	if(i >= 0)
	{
		return pLocalRefs->UnwrapLocalRef(i);
	}
	return JNI::UnwrapGlobalRef(o);
}

jstring JNIEnv::NewStringUTF(const char *psz)
{
	return (jstring)(void*)pLocalRefs->MakeLocalRef(StringFromUTF8(psz));
}

jstring JNIEnv::NewString(const jchar *unicode, jsize len)
{
	return (jstring)(void*)pLocalRefs->MakeLocalRef(new String((__wchar_t*)unicode, 0, len));
}

const char* JNIEnv::GetStringUTFChars(jstring str, jboolean *isCopy)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	// TODO for now we use the upper limit on the number of possible bytes needed
	char *buf = new char[s->Length * 3 + 1];
	// TODO if memory allocation fails, handle it by "throwing" OutOfMemoryError and returning null
	int j = 0;
	for(int i = 0, e = s->Length; i < e; i++)
	{
		jchar ch = s->Chars[i];
		if ((ch != 0) && (ch <=0x7f))
		{
			buf[j++] = (char)ch;
		}
		else if (ch <= 0x7FF)
		{
			/* 11 bits or less. */
			unsigned char high_five = ch >> 6;
			unsigned char low_six = ch & 0x3F;
			buf[j++] = high_five | 0xC0; /* 110xxxxx */
			buf[j++] = low_six | 0x80;   /* 10xxxxxx */
		}
		else
		{
			/* possibly full 16 bits. */
			char high_four = ch >> 12;
			char mid_six = (ch >> 6) & 0x3F;
			char low_six = ch & 0x3f;
			buf[j++] = high_four | 0xE0; /* 1110xxxx */
			buf[j++] = mid_six | 0x80;   /* 10xxxxxx */
			buf[j++] = low_six | 0x80;   /* 10xxxxxx*/
		}
	}
	buf[j] = 0;
	if(isCopy)
	{
		*isCopy = JNI_TRUE;
	}

	return buf;
}
#pragma unmanaged

void JNIEnv::ReleaseStringUTFChars(jstring str, const char* chars)
{
	delete[] chars;
}

#pragma managed
jint JNIEnv::ThrowNew(jclass clazz, const char *msg)
{
	jstring str = NewStringUTF(msg);
	jmethodID constructor = GetMethodID(clazz, "<init>", "(Ljava/lang/String;)V");
	assert(constructor);
	jobject exc = NewObject(clazz, constructor, str);
	DeleteLocalRef(str);
	Throw((jthrowable)exc);
	DeleteLocalRef(exc);
	return JNI_OK;
}

jint JNICALL JNIEnv::Throw(jthrowable obj)
{
	pLocalRefs->PendingException = __try_cast<Exception*>(UnwrapRef(obj));
	return JNI_OK;
}

jthrowable JNICALL JNIEnv::ExceptionOccurred()
{
	return (jthrowable)(void*)pLocalRefs->MakeLocalRef(pLocalRefs->PendingException);
}

void JNICALL JNIEnv::ExceptionDescribe()
{
	if(pLocalRefs->PendingException)
	{
		// when calling JNI methods there cannot be an exception pending, so we clear the exception
		// temporarily, while we print it
		jobject exception = ExceptionOccurred();
		Exception* pException = pLocalRefs->PendingException;
		pLocalRefs->PendingException = 0;
		jclass cls = FindClass("java/lang/Throwable");
		if(cls)
		{
			jmethodID mid = GetMethodID(cls, "printStackTrace", "()V");
			if(mid)
			{
				DeleteLocalRef(cls);
				CallVoidMethod(exception, mid);
			}
			else
			{
				Console::Error->WriteLine(S"JNI internal error: printStackTrace method not found in java.lang.Throwable");
			}
		}
		else
		{
			Console::Error->WriteLine(S"JNI internal error: java.lang.Throwable not found");
		}
		pLocalRefs->DeleteLocalRef(exception);
		pLocalRefs->PendingException = pException;
	}
}

void JNICALL JNIEnv::ExceptionClear()
{
	pLocalRefs->PendingException = 0;
}

#pragma unmanaged
jclass JNICALL JNIEnv::DefineClass(const char *name, jobject loader, const jbyte *buf, jsize len)
{
	assert(false);
	_asm int 3
}

#pragma managed
jclass JNIEnv::FindClass(const char *utf)
{
	return (jclass)(void*)pLocalRefs->MakeLocalRef(VM::FindClass(StringFromUTF8(utf)));
}
#pragma unmanaged

jobject JNIEnv::AllocObject(jclass cls)
{
	// wicked, I just realized that serialization should have a facility to construct uninitialized objects
	// this can be implemented using FormatterServices.GetUninitializedObject, the only hitch is that this
	// won't supports strings, so we may have to figure out a workaround for that, or decide just not to support it
	assert(false);
	_asm int 3
}

#pragma managed

jmethodID JNIEnv::FindMethodID(jclass cls, const char* name, const char* sig, bool isstatic)
{
	jmethodID mid = (jmethodID)(void*)VM::GetMethodCookie(UnwrapRef(cls), StringFromUTF8(name), StringFromUTF8(sig), isstatic);
	if(!mid)
	{
		//Console::WriteLine("Method not found: {0}{1} (static = {2})", StringFromUTF8(name), StringFromUTF8(sig), __box(isstatic));
		// TODO set the exception message
		ThrowNew(FindClass("java/lang/NoSuchMethodError"), "");
		return 0;
	}
	return mid;
}

jfieldID JNIEnv::FindFieldID(jclass cls, const char* name, const char* sig, bool isstatic)
{
	jfieldID fid = (jfieldID)(void*)VM::GetFieldCookie(UnwrapRef(cls), StringFromUTF8(name), StringFromUTF8(sig), isstatic);
	if(!fid)
	{
		// TODO set the exception message
		ThrowNew(FindClass("java/lang/NoSuchFieldError"), "");
		return 0;
	}
	return fid;
}

jmethodID JNIEnv::GetStaticMethodID(jclass cls, const char *name, const char *sig)
{
	return FindMethodID(cls, name, sig, true);
}

static int GetMethodArgs(jmethodID methodID, char* sig)
{
	int count = 0;
	String* s = VM::GetMethodArgList(methodID);
	for(int i = 0; i < s->Length; i++)
	{
		*sig++ = (char)s->get_Chars(i);
		count++;
	}
	*sig = 0;
	return count;
}

Object* JNIEnv::InvokeHelper(jobject object, jmethodID methodID, jvalue* args)
{
	assert(!pLocalRefs->PendingException);
	assert(methodID);

	char sig[257];
	int argc = GetMethodArgs(methodID, sig);
	Object* argarray __gc[] = new Object*[argc];
	for(int i = 0; i < argc; i++)
	{
		switch(sig[i])
		{
		case 'Z':
			argarray[i] = __box(args[i].z != JNI_FALSE);
			break;
		case 'B':
			argarray[i] = __box((char)args[i].b);
			break;
		case 'C':
			argarray[i] = __box((__wchar_t)args[i].c);
			break;
		case 'S':
			argarray[i] = __box((short)args[i].s);
			break;
		case 'I':
			argarray[i] = __box((int)args[i].i);
			break;
		case 'J':
			argarray[i] = __box((__int64)args[i].j);
			break;
		case 'F':
			argarray[i] = __box((float)args[i].f);
			break;
		case 'D':
			argarray[i] = __box((double)args[i].d);
			break;
		case 'L':
			argarray[i] = UnwrapRef(args[i].l);
			break;
		}
	}
	try
	{
		return VM::InvokeMethod(methodID, UnwrapRef(object), argarray, false);
	}
	catch(Exception* x)
	{
		pLocalRefs->PendingException = x;
		return 0;
	}
/*
	MethodBase* m = __try_cast<MethodBase*>(GCHandle::op_Explicit((IntPtr)methodID->method).Target);
	ParameterInfo* p __gc[] = m->GetParameters();
	Object* argarray __gc[] = new Object*[p->Length];
	for(int i = 0; i < p->Length; i++)
	{
		if(p[i]->ParameterType == __typeof(bool))
		{
			argarray[i] = __box(args[i].z != JNI_FALSE);
		}
		else if(p[i]->ParameterType == __typeof(char))
		{
			argarray[i] = __box((char)args[i].b);
		}
		else if(p[i]->ParameterType == __typeof(__wchar_t))
		{
			argarray[i] = __box((__wchar_t)args[i].c);
		}
		else if(p[i]->ParameterType == __typeof(short))
		{
			argarray[i] = __box((short)args[i].s);
		}
		else if(p[i]->ParameterType == __typeof(int))
		{
			argarray[i] = __box((int)args[i].i);
		}
		else if(p[i]->ParameterType == __typeof(__int64))
		{
			argarray[i] = __box((__int64)args[i].j);
		}
		else if(p[i]->ParameterType == __typeof(float))
		{
			argarray[i] = __box((float)args[i].f);
		}
		else if(p[i]->ParameterType == __typeof(double))
		{
			argarray[i] = __box((double)args[i].d);
		}
		else if(!p[i]->ParameterType->IsValueType)
		{
			// If we have an object specified but the method is static, we have been redirected to
			// a static helper, so we have to adjust
			if(i == 0 && object && m->IsStatic)
			{
				argarray[i] = UnwrapRef(object);
				object = 0;
				// HACK fix up the args ptr to correct for the missing first argument
				args--;
			}
			else
			{
				argarray[i] = UnwrapRef(args[i].l);
			}
		}
		else
		{
			// this can't happen, so it probably should be an assertion
			assert(false);
			throw new NotImplementedException(p[i]->ParameterType->FullName);
		}
	}
	try
	{
		Object* obj = 0;
		if(object)
		{
			obj = UnwrapRef(object);
		}
		if(m->IsConstructor)
		{
			return __try_cast<ConstructorInfo*>(m)->Invoke(argarray);
		}
		return m->Invoke(obj, argarray);
	}
	catch(TargetInvocationException* x)
	{
		// TODO remove this
		Console::WriteLine(S"InvokeHelper: {0}", x);
		// TODO retain stack trace information
		//Console::WriteLine(S"LocalRefs = ", __box((int)pLocalRefs));
		pLocalRefs->PendingException = x->InnerException;
		return 0;
	}
*/
}

void JNICALL JNIEnv::CallStaticVoidMethodA(jclass cls, jmethodID methodID, jvalue* args)
{
	InvokeHelper(0, methodID, args);
}
#pragma unmanaged

void JNICALL JNIEnv::CallStaticVoidMethodV(jclass clazz, jmethodID methodID, va_list args)
{
	char arglist[257];
	int argc = GetMethodArgs(methodID, arglist);
	jvalue* argarray = (jvalue*)_alloca(argc * sizeof(jvalue));
	for(int i = 0; i < argc; i++)
	{
		switch(arglist[i])
		{
		case 'Z':
		case 'B':
		case 'S':
		case 'C':
		case 'I':
			argarray[i].i = va_arg(args, int);
			break;
		case 'J':
			argarray[i].j = va_arg(args, __int64);
			break;
		case 'L':
			argarray[i].l = va_arg(args, jobject);
			break;
		case 'D':
			argarray[i].d = va_arg(args, double);
			break;
		case 'F':
			argarray[i].f = (float)va_arg(args, double);
			break;
		}
	}
	CallStaticVoidMethodA(clazz, methodID, argarray);
}

void JNIEnv::CallStaticVoidMethod(jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	CallStaticVoidMethodV(clazz, methodID, args);
	va_end(args);
}

#define STATIC_METHOD_IMPL(Type,type) \
type JNICALL JNIEnv::CallStatic##Type##MethodV(jclass clazz, jmethodID methodID, va_list args)\
{\
	char sig[257];\
	int argc = GetMethodArgs(methodID, sig);\
	jvalue* argarray = (jvalue*)_alloca(argc * sizeof(jvalue));\
	for(int i = 0; i < argc; i++)\
	{\
		switch(sig[i])\
		{\
		case 'Z':\
		case 'B':\
		case 'S':\
		case 'C':\
		case 'I':\
			argarray[i].i = va_arg(args, int);\
			break;\
		case 'J':\
			argarray[i].j = va_arg(args, __int64);\
			break;\
		case 'L':\
			argarray[i].l = va_arg(args, jobject);\
			break;\
		case 'D':\
			argarray[i].d = va_arg(args, double);\
			break;\
		case 'F':\
			argarray[i].f = (float)va_arg(args, double);\
			break;\
		}\
	}\
	return CallStatic##Type##MethodA(clazz, methodID, argarray);\
}\
type JNIEnv::CallStatic##Type##Method(jclass clazz, jmethodID methodID, ...)\
{\
	va_list args;\
	va_start(args, methodID);\
	type ret = CallStatic##Type##MethodV(clazz, methodID, args);\
	va_end(args);\
	return ret;\
}
#define STATIC_METHOD_IMPL_MANAGED(Type,type,cpptype) \
type JNICALL JNIEnv::CallStatic##Type##MethodA(jclass cls, jmethodID methodID, jvalue* args)\
{\
	Object* ret = InvokeHelper(0, methodID, args);\
	if(ret)	return __unbox<cpptype>(ret);\
	return 0;\
}

STATIC_METHOD_IMPL(Object,jobject)
STATIC_METHOD_IMPL(Boolean,jboolean)
STATIC_METHOD_IMPL(Byte,jbyte)
STATIC_METHOD_IMPL(Char,jchar)
STATIC_METHOD_IMPL(Short,jshort)
STATIC_METHOD_IMPL(Int,jint)
STATIC_METHOD_IMPL(Long,jlong)
STATIC_METHOD_IMPL(Float,jfloat)
STATIC_METHOD_IMPL(Double,jdouble)
#pragma managed
STATIC_METHOD_IMPL_MANAGED(Boolean,jboolean,bool)
STATIC_METHOD_IMPL_MANAGED(Byte,jbyte,System::SByte)
STATIC_METHOD_IMPL_MANAGED(Char,jchar,wchar_t)
STATIC_METHOD_IMPL_MANAGED(Short,jshort,short)
STATIC_METHOD_IMPL_MANAGED(Int,jint,int)
STATIC_METHOD_IMPL_MANAGED(Long,jlong,__int64)
STATIC_METHOD_IMPL_MANAGED(Float,jfloat,float)
STATIC_METHOD_IMPL_MANAGED(Double,jdouble,double)

// special case for Object
jobject JNICALL JNIEnv::CallStaticObjectMethodA(jclass cls, jmethodID methodID, jvalue* args)
{
	return (jobject)(void*)pLocalRefs->MakeLocalRef(InvokeHelper(0, methodID, args));
}

#pragma unmanaged
jmethodID JNIEnv::GetMethodID(jclass cls, const char *name, const char *sig)
{
	return FindMethodID(cls, name, sig, false);
}

jfieldID JNICALL JNIEnv::GetFieldID(jclass cls, const char *name, const char *sig)
{
	return FindFieldID(cls, name, sig, false);
}

jfieldID JNICALL JNIEnv::GetStaticFieldID(jclass cls, const char *name, const char *sig)
{
	return FindFieldID(cls, name, sig, true);
}

#pragma managed
#define GET_SET_FIELD(Type,type,cpptype) \
void JNICALL JNIEnv::Set##Type##Field(jobject obj, jfieldID fieldID, type val)\
{\
	VM::SetFieldValue((IntPtr)fieldID, UnwrapRef(obj), __box((cpptype)val));\
}\
type JNICALL JNIEnv::Get##Type##Field(jobject obj, jfieldID fieldID)\
{\
	return __unbox<cpptype>(VM::GetFieldValue((IntPtr)fieldID, UnwrapRef(obj)));\
}

GET_SET_FIELD(Boolean,jboolean,bool)
GET_SET_FIELD(Byte,jbyte,System::SByte)
GET_SET_FIELD(Char,jchar,wchar_t)
GET_SET_FIELD(Short,jshort,short)
GET_SET_FIELD(Int,jint,int)
GET_SET_FIELD(Long,jlong,__int64)
GET_SET_FIELD(Float,jfloat,float)
GET_SET_FIELD(Double,jdouble,double)

void JNICALL JNIEnv::SetObjectField(jobject obj, jfieldID fieldID, jobject val)
{
	VM::SetFieldValue((IntPtr)fieldID, UnwrapRef(obj), UnwrapRef(val));
}

jobject JNICALL JNIEnv::GetObjectField(jobject obj, jfieldID fieldID)
{
	return (jobject)(void*)pLocalRefs->MakeLocalRef(VM::GetFieldValue((IntPtr)fieldID, UnwrapRef(obj)));
}
#pragma unmanaged

void JNICALL JNIEnv::SetStaticObjectField(jclass cls, jfieldID fieldID, jobject value)
{
	assert(false);
	_asm int 3
}

jobject JNICALL JNIEnv::GetStaticObjectField(jclass clazz, jfieldID fieldID)
{
	assert(false);
	_asm int 3
}

jlong JNICALL JNIEnv::GetStaticLongField(jclass clazz, jfieldID fieldID)
{
	assert(false);
	_asm int 3
}

#define METHOD_IMPL(Type,type) \
type JNIEnv::Call##Type##Method(jobject obj, jmethodID methodID, ...) \
{\
	va_list args;\
	va_start(args, methodID);\
	type ret = Call##Type##MethodV(obj, methodID, args);\
	va_end(args);\
	return ret;\
}\
type JNICALL JNIEnv::Call##Type##MethodV(jobject obj, jmethodID methodID, va_list args)\
{\
	char sig[257];\
	int argc = GetMethodArgs(methodID, sig);\
	jvalue* argarray = (jvalue*)_alloca(argc * sizeof(jvalue));\
	for(int i = 0; i < argc; i++)\
	{\
		switch(sig[i])\
		{\
		case 'Z':\
		case 'B':\
		case 'S':\
		case 'C':\
		case 'I':\
			argarray[i].i = va_arg(args, int);\
			break;\
		case 'J':\
			argarray[i].j = va_arg(args, __int64);\
			break;\
		case 'L':\
			argarray[i].l = va_arg(args, jobject);\
			break;\
		case 'D':\
			argarray[i].d = va_arg(args, double);\
			break;\
		case 'F':\
			argarray[i].f = (float)va_arg(args, double);\
			break;\
		}\
	}\
	return Call##Type##MethodA(obj, methodID, argarray);\
}

#define METHOD_IMPL_MANAGED(Type,type,cpptype) \
type JNICALL JNIEnv::Call##Type##MethodA(jobject obj, jmethodID methodID, jvalue* args)\
{\
	Object* ret = InvokeHelper(obj, methodID, args);\
	if(ret)	return __unbox<cpptype>(ret);\
	return 0;\
}

#pragma unmanaged
METHOD_IMPL(Object,jobject)
METHOD_IMPL(Boolean,jboolean)
METHOD_IMPL(Byte,jbyte)
METHOD_IMPL(Char,jchar)
METHOD_IMPL(Short,jshort)
METHOD_IMPL(Int,jint)
METHOD_IMPL(Long,jlong)
METHOD_IMPL(Float,jfloat)
METHOD_IMPL(Double,jdouble)
#pragma managed
METHOD_IMPL_MANAGED(Boolean,jboolean,bool)
METHOD_IMPL_MANAGED(Byte,jbyte,System::SByte)
METHOD_IMPL_MANAGED(Char,jchar,wchar_t)
METHOD_IMPL_MANAGED(Short,jshort,short)
METHOD_IMPL_MANAGED(Int,jint,int)
METHOD_IMPL_MANAGED(Long,jlong,__int64)
METHOD_IMPL_MANAGED(Float,jfloat,float)
METHOD_IMPL_MANAGED(Double,jdouble,double)

// special case for Object, because we need to convert the reference to a localref
jobject JNICALL JNIEnv::CallObjectMethodA(jobject obj, jmethodID methodID, jvalue* args)
{
	return (jobject)(void*)pLocalRefs->MakeLocalRef(InvokeHelper(obj, methodID, args));
}
#pragma unmanaged

void JNIEnv::CallVoidMethod(jobject obj, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	CallVoidMethodV(obj, methodID, args);
	va_end(args);
}

void JNICALL JNIEnv::CallVoidMethodV(jobject obj, jmethodID methodID, va_list args)
{
	char sig[257];
	int argc = GetMethodArgs(methodID, sig);
	jvalue* argarray = (jvalue*)_alloca(argc * sizeof(jvalue));
	for(int i = 0; i < argc; i++)
	{
		switch(sig[i])
		{
		case 'Z':
		case 'B':
		case 'S':
		case 'C':
		case 'I':
			argarray[i].i = va_arg(args, int);
			break;
		case 'J':
			argarray[i].j = va_arg(args, __int64);
			break;
		case 'L':
			argarray[i].l = va_arg(args, jobject);
			break;
		case 'D':
			argarray[i].d = va_arg(args, double);
			break;
		case 'F':
			argarray[i].f = (float)va_arg(args, double);
			break;
		}
	}
	CallVoidMethodA(obj, methodID, argarray);
}

#pragma managed
void JNICALL JNIEnv::CallVoidMethodA(jobject obj, jmethodID methodID, jvalue* args)
{
	InvokeHelper(obj, methodID, args);
}
#pragma unmanaged

void JNICALL JNIEnv::CallNonvirtualVoidMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args)
{
	assert(false);
	_asm int 3
}

void JNICALL JNIEnv::CallNonvirtualVoidMethod(jobject obj, jclass clazz, jmethodID methodID, ...)
{
	assert(false);
	_asm int 3
}

#pragma managed
jsize JNIEnv::GetStringLength(jstring str)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	return s->Length;
}

const jchar* JNIEnv::GetStringChars(jstring str, jboolean *isCopy)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	jchar* p = new jchar[s->Length];
	for(int i = 0; i < s->Length; i++)
	{
		p[i] = s->Chars[i];
	}
	if(isCopy)
	{
		*isCopy = JNI_TRUE;
	}
	return p;
}

void JNIEnv::ReleaseStringChars(jstring str, const jchar *chars)
{
	delete[] chars;
}

jsize JNIEnv::GetArrayLength(jarray array)
{
	Array* ar = __try_cast<Array*>(UnwrapRef(array));
	return ar->Length;
}
#pragma unmanaged

jobjectArray JNIEnv::NewObjectArray(jsize len, jclass clazz, jobject init)
{
	assert(false);
	_asm int 3
}

jcharArray JNICALL JNIEnv::NewCharArray(jsize len)
{
	assert(false);
	_asm int 3
}

jbyteArray JNICALL JNIEnv::NewByteArray(jsize len)
{
	assert(false);
	_asm int 3
}

#pragma managed
jintArray JNICALL JNIEnv::NewIntArray(jsize len)
{
	return (jintArray)(void*)pLocalRefs->MakeLocalRef(new int __gc[len]);
}

#pragma unmanaged
void JNIEnv::SetObjectArrayElement(jobjectArray array, jsize index, jobject val)
{
	assert(false);
	_asm int 3
}

#pragma managed
jobject JNIEnv::GetObjectArrayElement(jobjectArray array, jsize index)
{
	Object* ar __gc[] = __try_cast<Object* __gc[]>(UnwrapRef(array));
	if(index >= ar->Length)
	{
		// TODO handle error
		assert(false);
	}
	return (jobject)(void*)pLocalRefs->MakeLocalRef(ar[index]);
}

#define GET_SET_ARRAY_ELEMENTS(Type,type,cpptype) \
type* JNIEnv::Get##Type##ArrayElements(type##Array array, jboolean *isCopy)\
{\
	cpptype ar __gc[] = __try_cast<cpptype __gc[]>(UnwrapRef(array));\
	type* p = new type[ar->Length];\
	for(int i = 0; i < ar->Length; i++)\
	{\
		p[i] = ar[i];\
	}\
	if(isCopy)\
	{\
		*isCopy = JNI_TRUE;\
	}\
	return p;\
}\
void JNIEnv::Release##Type##ArrayElements(type##Array array, type *elems, jint mode)\
{\
	if(mode == 0)\
	{\
		cpptype ar __gc[] = __try_cast<cpptype __gc[]>(UnwrapRef(array));\
		for(int i = 0; i < ar->Length; i++)\
		{\
			ar[i] = elems[i];\
		}\
		delete[] elems;\
	}\
	else if(mode == JNI_COMMIT)\
	{\
		cpptype ar __gc[] = __try_cast<cpptype __gc[]>(UnwrapRef(array));\
		for(int i = 0; i < ar->Length; i++)\
		{\
			ar[i] = elems[i];\
		}\
	}\
	else if(mode == JNI_ABORT)\
	{\
		delete[] elems;\
	}\
}

GET_SET_ARRAY_ELEMENTS(Boolean,jboolean,bool)
GET_SET_ARRAY_ELEMENTS(Byte,jbyte,System::SByte)
GET_SET_ARRAY_ELEMENTS(Char,jchar,wchar_t)
GET_SET_ARRAY_ELEMENTS(Short,jshort,short)
GET_SET_ARRAY_ELEMENTS(Int,jint,int)
GET_SET_ARRAY_ELEMENTS(Long,jlong,__int64)
GET_SET_ARRAY_ELEMENTS(Float,jfloat,float)
GET_SET_ARRAY_ELEMENTS(Double,jdouble,double)

#pragma unmanaged
jobject JNICALL JNIEnv::NewObject(jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	jobject o = NewObjectV(clazz, methodID, args);
	va_end(args);
	return o;
}

jobject JNICALL JNIEnv::NewObjectV(jclass clazz, jmethodID methodID, va_list args)
{
	char sig[257];
	int argc = GetMethodArgs(methodID, sig);
	jvalue* argarray = (jvalue*)_alloca(argc * sizeof(jvalue));
	for(int i = 0; i < argc; i++)
	{
		switch(sig[i])
		{
		case 'Z':
		case 'B':
		case 'S':
		case 'C':
		case 'I':
			argarray[i].i = va_arg(args, int);
			break;
		case 'J':
			argarray[i].j = va_arg(args, __int64);
			break;
		case 'L':
			argarray[i].l = va_arg(args, jobject);
			break;
		case 'D':
			argarray[i].d = va_arg(args, double);
			break;
		case 'F':
			argarray[i].f = (float)va_arg(args, double);
			break;
		}
	}
	return NewObjectA(clazz, methodID, argarray);
}

#pragma managed
jobject JNICALL JNIEnv::NewObjectA(jclass clazz, jmethodID methodID, jvalue *args)
{
	return (jobject)(void*)pLocalRefs->MakeLocalRef(InvokeHelper(0, methodID, args));
}

jclass JNICALL JNIEnv::GetObjectClass(jobject obj)
{
	if(obj == 0)
	{
		// TODO throw nullpointerexception
		assert(false);
	}
	return (jclass)(void*)pLocalRefs->MakeLocalRef(VM::GetClassFromType(UnwrapRef(obj)->GetType()));
}
#pragma unmanaged

jboolean JNICALL JNIEnv::IsInstanceOf(jobject obj, jclass clazz)
{
	assert(false);
	_asm int 3
}

jboolean JNICALL JNIEnv::IsAssignableFrom(jclass sub, jclass sup)
{
	assert(false);
	_asm int 3
}

#pragma managed
jobject JNICALL JNIEnv::NewGlobalRef(jobject lobj)
{
	return JNI::MakeGlobalRef(UnwrapRef(lobj));
}

void JNICALL JNIEnv::DeleteGlobalRef(jobject gref)
{
	JNI::DeleteGlobalRef(gref);
}

void JNICALL JNIEnv::DeleteLocalRef(jobject obj)
{
	pLocalRefs->DeleteLocalRef(obj);
}
#pragma unmanaged

jboolean JNICALL JNIEnv::IsSameObject(jobject obj1, jobject obj2)
{
	assert(false);
	_asm int 3
}

jclass JNICALL JNIEnv::GetSuperclass(jclass sub)
{
	assert(false);
	_asm int 3
}

jint JNICALL JNIEnv::MonitorEnter(jobject obj)
{
	assert(false);
	_asm int 3
}

jint JNICALL JNIEnv::MonitorExit(jobject obj)
{
	assert(false);
	_asm int 3
}

jint JNICALL JNIEnv::RegisterNatives(jclass clazz, const JNINativeMethod *methods, jint nMethods)
{
	assert(false);
	_asm int 3
}

jint JNICALL JNIEnv::UnregisterNatives(jclass clazz)
{
	assert(false);
	_asm int 3
}

jint JNICALL JNIEnv::GetJavaVM(JavaVM **vm)
{
	static JavaVM theVM;
	*vm = &theVM;
	return 0;
}

void JavaVM::reserved0() { assert(false); _asm int 3}
void JavaVM::reserved1() { assert(false); _asm int 3}
void JavaVM::reserved2() { assert(false); _asm int 3}

jint JavaVM::DestroyJavaVM()
{
	assert(false);
	_asm int 3
}

jint JavaVM::AttachCurrentThread(void **penv, void *args)
{
	assert(false);
	_asm int 3
}

jint JavaVM::DetachCurrentThread()
{
	assert(false);
	_asm int 3
}

#pragma managed
jint JavaVM::GetEnv(void **penv, jint version)
{
	// TODO we should check the version
	JNIEnv* p = LocalRefStruct::GetEnv();
	if(p)
	{
		*penv = p;
		return JNI_OK;
	}
	return JNI_EDETACHED;
}
#pragma unmanaged

jint JavaVM::AttachCurrentThreadAsDaemon(void **penv, void *args)
{
	assert(false);
	_asm int 3
}

////////////////////////////////////////////////////////////////////////////
#pragma warning (disable : 4035)

void JNIEnv::reserved0() { assert(false); _asm int 3}
void JNIEnv::reserved1() { assert(false); _asm int 3}
void JNIEnv::reserved2() { assert(false); _asm int 3}

void JNIEnv::reserved3() { assert(false); _asm int 3}
jint JNICALL JNIEnv::GetVersion() { assert(false); _asm int 3}

void JNIEnv::reserved4() { assert(false); _asm int 3}
void JNIEnv::reserved5() { assert(false); _asm int 3}
void JNIEnv::reserved6() { assert(false); _asm int 3}

void JNIEnv::reserved7() { assert(false); _asm int 3}

void JNICALL JNIEnv::FatalError(const char *msg) { assert(false); _asm int 3}
void JNIEnv::reserved8() { assert(false); _asm int 3}
void JNIEnv::reserved9() { assert(false); _asm int 3}

void JNIEnv::reserved10() { assert(false); _asm int 3}
void JNIEnv::reserved11() { assert(false); _asm int 3}

jobject JNICALL JNIEnv::CallNonvirtualObjectMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jobject JNICALL JNIEnv::CallNonvirtualObjectMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jobject JNICALL JNIEnv::CallNonvirtualObjectMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args) { assert(false); _asm int 3}

jboolean JNICALL JNIEnv::CallNonvirtualBooleanMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jboolean JNICALL JNIEnv::CallNonvirtualBooleanMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jboolean JNICALL JNIEnv::CallNonvirtualBooleanMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args) { assert(false); _asm int 3}

jbyte JNICALL JNIEnv::CallNonvirtualByteMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jbyte JNICALL JNIEnv::CallNonvirtualByteMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jbyte JNICALL JNIEnv::CallNonvirtualByteMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

jchar JNICALL JNIEnv::CallNonvirtualCharMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jchar JNICALL JNIEnv::CallNonvirtualCharMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jchar JNICALL JNIEnv::CallNonvirtualCharMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

jshort JNICALL JNIEnv::CallNonvirtualShortMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jshort JNICALL JNIEnv::CallNonvirtualShortMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jshort JNICALL JNIEnv::CallNonvirtualShortMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

jint JNICALL JNIEnv::CallNonvirtualIntMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jint JNICALL JNIEnv::CallNonvirtualIntMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jint JNICALL JNIEnv::CallNonvirtualIntMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

jlong JNICALL JNIEnv::CallNonvirtualLongMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jlong JNICALL JNIEnv::CallNonvirtualLongMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jlong JNICALL JNIEnv::CallNonvirtualLongMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

jfloat JNICALL JNIEnv::CallNonvirtualFloatMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jfloat JNICALL JNIEnv::CallNonvirtualFloatMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jfloat JNICALL JNIEnv::CallNonvirtualFloatMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

jdouble JNICALL JNIEnv::CallNonvirtualDoubleMethod(jobject obj, jclass clazz, jmethodID methodID, ...) { assert(false); _asm int 3}
jdouble JNICALL JNIEnv::CallNonvirtualDoubleMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args) { assert(false); _asm int 3}
jdouble JNICALL JNIEnv::CallNonvirtualDoubleMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue *args) { assert(false); _asm int 3}

void JNICALL JNIEnv::CallNonvirtualVoidMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue * args) { assert(false); _asm int 3}

jboolean JNICALL JNIEnv::GetStaticBooleanField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}
jbyte JNICALL JNIEnv::GetStaticByteField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}
jchar JNICALL JNIEnv::GetStaticCharField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}
jshort JNICALL JNIEnv::GetStaticShortField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}
jint JNICALL JNIEnv::GetStaticIntField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}
jfloat JNICALL JNIEnv::GetStaticFloatField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}
jdouble JNICALL JNIEnv::GetStaticDoubleField(jclass clazz, jfieldID fieldID) { assert(false); _asm int 3}

void JNICALL JNIEnv::SetStaticBooleanField(jclass clazz, jfieldID fieldID, jboolean value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticByteField(jclass clazz, jfieldID fieldID, jbyte value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticCharField(jclass clazz, jfieldID fieldID, jchar value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticShortField(jclass clazz, jfieldID fieldID, jshort value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticIntField(jclass clazz, jfieldID fieldID, jint value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticLongField(jclass clazz, jfieldID fieldID, jlong value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticFloatField(jclass clazz, jfieldID fieldID, jfloat value) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetStaticDoubleField(jclass clazz, jfieldID fieldID, jdouble value) { assert(false); _asm int 3}

jsize JNICALL JNIEnv::GetStringUTFLength(jstring str) { assert(false); _asm int 3}

jbooleanArray JNICALL JNIEnv::NewBooleanArray(jsize len) { assert(false); _asm int 3}
jshortArray JNICALL JNIEnv::NewShortArray(jsize len) { assert(false); _asm int 3}
jlongArray JNICALL JNIEnv::NewLongArray(jsize len) { assert(false); _asm int 3}
jfloatArray JNICALL JNIEnv::NewFloatArray(jsize len) { assert(false); _asm int 3}
jdoubleArray JNICALL JNIEnv::NewDoubleArray(jsize len) { assert(false); _asm int 3}

void JNICALL JNIEnv::GetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::GetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf) { assert(false); _asm int 3}

void JNICALL JNIEnv::SetBooleanArrayRegion(jbooleanArray array, jsize start, jsize l, jboolean *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetByteArrayRegion(jbyteArray array, jsize start, jsize len, jbyte *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetCharArrayRegion(jcharArray array, jsize start, jsize len, jchar *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetShortArrayRegion(jshortArray array, jsize start, jsize len, jshort *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetIntArrayRegion(jintArray array, jsize start, jsize len, jint *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetLongArrayRegion(jlongArray array, jsize start, jsize len, jlong *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetFloatArrayRegion(jfloatArray array, jsize start, jsize len, jfloat *buf) { assert(false); _asm int 3}
void JNICALL JNIEnv::SetDoubleArrayRegion(jdoubleArray array, jsize start, jsize len, jdouble *buf) { assert(false); _asm int 3}
