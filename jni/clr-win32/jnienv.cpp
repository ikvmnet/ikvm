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

#include "stdafx.h"
#include "jnienv.h"
#include "jni.h"
#include <malloc.h>
#include <stdarg.h>
#include <vcclr.h>

#define DEBUG
#undef NDEBUG

#include <assert.h>

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

jobject JNIEnv::MakeLocalRef(System::Object* obj)
{
	return VM::MakeLocalRef(this, obj);
}

Object* JNIEnv::UnwrapRef(jobject o)
{
	return VM::UnwrapRef(this, o);
}


JNIEnv::JNIEnv()
{
}

JNIEnv::~JNIEnv()
{
}

void JNIEnv::reserved0()
{
	VM::FatalError("JNIEnv::reserved0");
}

void JNIEnv::reserved1()
{
	VM::FatalError("JNIEnv::reserved1");
}

void JNIEnv::reserved2()
{
	VM::FatalError("JNIEnv::reserved2");
}

void JNIEnv::reserved3()
{
	VM::FatalError("JNIEnv::reserved3");
}

jstring JNIEnv::NewStringUTF(const char *psz)
{
	return (jstring)MakeLocalRef(StringFromUTF8(psz));
}

jstring JNIEnv::NewString(const jchar *unicode, jsize len)
{
	return (jstring)MakeLocalRef(new String((__wchar_t*)unicode, 0, len));
}

static jsize StringUTFLength(String* s)
{
	jsize len = 0;
	for(int i = 0; i < s->Length; i++)
	{
		jchar ch = s->Chars[i];
		if ((ch != 0) && (ch <=0x7f))
		{
			len++;
		}
		else if (ch <= 0x7FF)
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

jsize JNICALL JNIEnv::GetStringUTFLength(jstring str)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	if(s)
	{
		return StringUTFLength(s);
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return 0;
}

const char* JNIEnv::GetStringUTFChars(jstring str, jboolean *isCopy)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	if(s)
	{
		// TODO handle out of memory (what does the unmanaged new do?)
		char *buf = new char[StringUTFLength(s) + 1];
		int j = 0;
		for(int i = 0; i < s->Length; i++)
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
	else
	{
		ThrowNew(FindClass("java/lang/NullPointerException"), "");
		return 0;
	}
}

void JNICALL JNIEnv::GetStringRegion(jstring str, jsize start, jsize len, jchar *buf)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	if(s)
	{
		if(start > s->Length || s->Length - start < len)
		{
			ThrowNew(FindClass("java/lang/StringIndexOutOfBoundsException"), "");
			return;
		}
		else
		{
			const wchar_t __pin* p = PtrToStringChars(s);
			memcpy(buf, p, len);
			return;
		}
	}
	else
	{
		ThrowNew(FindClass("java/lang/NullPointerException"), "");
	}
}

void JNICALL JNIEnv::GetStringUTFRegion(jstring str, jsize start, jsize len, char *buf)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	if(s)
	{
		if(start > s->Length || s->Length - start < len)
		{
			ThrowNew(FindClass("java/lang/StringIndexOutOfBoundsException"), "");
			return;
		}
		else
		{
			int j = 0;
			for(jsize i = 0; i < len; i++)
			{
				jchar ch = s->Chars[start + i];
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
		}
	}
	else
	{
		ThrowNew(FindClass("java/lang/NullPointerException"), "");
	}
}

const jchar* JNICALL JNIEnv::GetStringCritical(jstring str, jboolean *isCopy)
{
	String* s = __try_cast<String*>(UnwrapRef(str));
	if(s)
	{
		// TODO handle out of memory (what does the unmanaged new do?)
		jchar* cstring = new jchar[s->Length];
		const wchar_t __pin* p = PtrToStringChars(s);
		memcpy(cstring, p, s->Length * 2);
		if(isCopy)
		{
			*isCopy = JNI_TRUE;
		}
		return cstring;		
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return 0;
}

#pragma unmanaged
void JNIEnv::ReleaseStringUTFChars(jstring str, const char* chars)
{
	delete[] chars;
}

void JNICALL JNIEnv::ReleaseStringCritical(jstring string, const jchar* cstring)
{
	delete[] cstring;
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
	//// TODO once we implement PopLocalFrame, we need to make sure that pendingException isn't in the popped local frame
	//pendingException = (jthrowable)NewLocalRef(obj);
	//return JNI_OK;
	throw new InvalidOperationException();
}

jthrowable JNICALL JNIEnv::ExceptionOccurred()
{
	//return (jthrowable)NewLocalRef(pendingException);
	throw new InvalidOperationException();
}

void JNICALL JNIEnv::ExceptionDescribe()
{
	//if(pendingException)
	//{
	//	// when calling JNI methods there cannot be an exception pending, so we clear the exception
	//	// temporarily, while we print it
	//	jthrowable exception = pendingException;
	//	pendingException = 0;
	//	jclass cls = FindClass("java/lang/Throwable");
	//	if(cls)
	//	{
	//		jmethodID mid = GetMethodID(cls, "printStackTrace", "()V");
	//		DeleteLocalRef(cls);
	//		if(mid)
	//		{
	//			CallVoidMethod(exception, mid);
	//		}
	//		else
	//		{
	//			Console::Error->WriteLine(S"JNI internal error: printStackTrace method not found in java.lang.Throwable");
	//		}
	//	}
	//	else
	//	{
	//		Console::Error->WriteLine(S"JNI internal error: java.lang.Throwable not found");
	//	}
	//	pendingException = exception;
	//}
	throw new InvalidOperationException();
}

void JNICALL JNIEnv::ExceptionClear()
{
	//DeleteLocalRef(pendingException);
	//pendingException = 0;
	throw new InvalidOperationException();
}

jclass JNIEnv::FindClass(const char *utf)
{
	try
	{
		return (jclass)MakeLocalRef(VM::FindClass(StringFromUTF8(utf)));
	}
	catch(Exception* x)
	{
		jobject o = MakeLocalRef(x);
		Throw((jthrowable)o);
		DeleteLocalRef(o);
		return 0;
	}
}

jobject JNIEnv::AllocObject(jclass cls)
{
	return MakeLocalRef(VM::AllocObject(UnwrapRef(cls)));
}

jmethodID JNIEnv::FindMethodID(jclass cls, const char* name, const char* sig, bool isstatic)
{
	jmethodID mid = (jmethodID)(void*)VM::GetMethodCookie(UnwrapRef(cls), StringFromUTF8(name), StringFromUTF8(sig), isstatic);
	if(!mid)
	{
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

Object* JNIEnv::InvokeHelper(jobject object, jmethodID methodID, jvalue* args, bool nonVirtual)
{
	DebugBreak();
	return 0;
	////assert(!pendingException);
	//assert(methodID);

	//char sig[257];
	//int argc = GetMethodArgs(methodID, sig);
	//Object* argarray __gc[] = new Object*[argc];
	//for(int i = 0; i < argc; i++)
	//{
	//	switch(sig[i])
	//	{
	//	case 'Z':
	//		argarray[i] = __box(args[i].z != JNI_FALSE);
	//		break;
	//	case 'B':
	//		argarray[i] = __box((char)args[i].b);
	//		break;
	//	case 'C':
	//		argarray[i] = __box((__wchar_t)args[i].c);
	//		break;
	//	case 'S':
	//		argarray[i] = __box((short)args[i].s);
	//		break;
	//	case 'I':
	//		argarray[i] = __box((int)args[i].i);
	//		break;
	//	case 'J':
	//		argarray[i] = __box((__int64)args[i].j);
	//		break;
	//	case 'F':
	//		argarray[i] = __box((float)args[i].f);
	//		break;
	//	case 'D':
	//		argarray[i] = __box((double)args[i].d);
	//		break;
	//	case 'L':
	//		argarray[i] = UnwrapRef(args[i].l);
	//		break;
	//	}
	//}
	//try
	//{
	//	return VM::InvokeMethod(methodID, UnwrapRef(object), argarray, nonVirtual);
	//}
	//catch(Exception* x)
	//{
	//	jobject o = MakeLocalRef(x);
	//	Throw((jthrowable)o);
	//	DeleteLocalRef(o);
	//	return 0;
	//}
}

void JNICALL JNIEnv::CallStaticVoidMethodA(jclass cls, jmethodID methodID, jvalue* args)
{
	InvokeHelper(0, methodID, args, false);
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
	Object* ret = InvokeHelper(0, methodID, args, false);\
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
	return MakeLocalRef(InvokeHelper(0, methodID, args, false));
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
}\
void JNICALL JNIEnv::SetStatic##Type##Field(jclass clazz, jfieldID fieldID, type val)\
{\
	/* // TODO consider checking that clazz is the right object */ \
	VM::SetFieldValue((IntPtr)fieldID, 0, __box((cpptype)val));\
}\
type JNICALL JNIEnv::GetStatic##Type##Field(jclass clazz, jfieldID fieldID)\
{\
	/* // TODO consider checking that clazz is the right object */ \
	return __unbox<cpptype>(VM::GetFieldValue((IntPtr)fieldID, 0));\
}

#pragma warning (push)
// stop the compiler from wanking about "forcing value to bool 'true' or 'false' (performance warning)"
#pragma warning (disable : 4800)
GET_SET_FIELD(Boolean,jboolean,bool)
#pragma warning (pop)
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
	return MakeLocalRef(VM::GetFieldValue((IntPtr)fieldID, UnwrapRef(obj)));
}

void JNICALL JNIEnv::SetStaticObjectField(jclass clazz, jfieldID fieldID, jobject val)
{
	VM::SetFieldValue((IntPtr)fieldID, 0, UnwrapRef(val));
}

jobject JNICALL JNIEnv::GetStaticObjectField(jclass clazz, jfieldID fieldID)
{
	return MakeLocalRef(VM::GetFieldValue((IntPtr)fieldID, 0));
}

#pragma unmanaged

#define METHOD_IMPL(Type,type) \
type JNIEnv::Call##Type##Method(jobject obj, jmethodID methodID, ...) \
{\
	va_list args;\
	va_start(args, methodID);\
	type ret = Call##Type##MethodV(obj, methodID, args);\
	va_end(args);\
	return ret;\
}\
type JNIEnv::CallNonvirtual##Type##Method(jobject obj, jclass clazz, jmethodID methodID, ...) \
{\
	va_list args;\
	va_start(args, methodID);\
	type ret = CallNonvirtual##Type##MethodV(obj, clazz, methodID, args);\
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
}\
type JNICALL JNIEnv::CallNonvirtual##Type##MethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args)\
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
	return CallNonvirtual##Type##MethodA(obj, clazz, methodID, argarray);\
}

#define METHOD_IMPL_MANAGED(Type,type,cpptype) \
type JNICALL JNIEnv::Call##Type##MethodA(jobject obj, jmethodID methodID, jvalue* args)\
{\
	Object* ret = InvokeHelper(obj, methodID, args, false);\
	if(ret)	return __unbox<cpptype>(ret);\
	return 0;\
}\
type JNICALL JNIEnv::CallNonvirtual##Type##MethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue* args)\
{\
	Object* ret = InvokeHelper(obj, methodID, args, true);\
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
	return MakeLocalRef(InvokeHelper(obj, methodID, args, false));
}
jobject JNICALL JNIEnv::CallNonvirtualObjectMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
{
	return MakeLocalRef(InvokeHelper(obj, methodID, args, true));
}
#pragma unmanaged

void JNIEnv::CallVoidMethod(jobject obj, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	CallVoidMethodV(obj, methodID, args);
	va_end(args);
}

void JNIEnv::CallNonvirtualVoidMethod(jobject obj, jclass clazz, jmethodID methodID, ...)
{
	va_list args;
	va_start(args, methodID);
	CallNonvirtualVoidMethodV(obj, clazz, methodID, args);
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

void JNICALL JNIEnv::CallNonvirtualVoidMethodV(jobject obj, jclass clazz, jmethodID methodID, va_list args)
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
	CallNonvirtualVoidMethodA(obj, clazz, methodID, argarray);
}

#pragma managed
void JNICALL JNIEnv::CallVoidMethodA(jobject obj, jmethodID methodID, jvalue* args)
{
	InvokeHelper(obj, methodID, args, false);
}

void JNICALL JNIEnv::CallNonvirtualVoidMethodA(jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
{
	InvokeHelper(obj, methodID, args, true);
}

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

#define NEW_ARRAY(Type,type,cpptype) \
type##Array JNIEnv::New##Type##Array(jsize len)\
{\
	try\
	{\
		return (type##Array)MakeLocalRef(new cpptype __gc[len]);\
	}\
	catch(OutOfMemoryException*)\
	{\
		ThrowNew(FindClass("java/lang/OutOfMemoryError"), "");\
		return 0;\
	}\
}

#pragma warning (push)
// stop the compiler from wanking about "forcing value to bool 'true' or 'false' (performance warning)"
#pragma warning (disable : 4800)
NEW_ARRAY(Boolean, jboolean, bool)
#pragma warning (pop)
NEW_ARRAY(Byte, jbyte, System::SByte)
NEW_ARRAY(Char, jchar, wchar_t)
NEW_ARRAY(Short, jshort, short)
NEW_ARRAY(Int, jint, int)
NEW_ARRAY(Long, jlong, __int64)
NEW_ARRAY(Float, jfloat, float)
NEW_ARRAY(Double, jdouble, double)

jobjectArray JNIEnv::NewObjectArray(jsize len, jclass clazz, jobject init)
{
	try
	{
		Object* ar __gc[] = new Object* __gc[len];
		Object* o = UnwrapRef(init);
		if(o)
		{
			for(jsize i = 0; i < len; i++)
			{
				ar[i] = o;
			}
		}
		return (jobjectArray)MakeLocalRef(ar);
	}
	catch(OutOfMemoryException*)
	{
		ThrowNew(FindClass("java/lang/OutOfMemoryError"), "");
		return 0;
	}
}

void JNIEnv::SetObjectArrayElement(jobjectArray array, jsize index, jobject val)
{
	Object* ar __gc[] = __try_cast<Object* __gc[]>(UnwrapRef(array));
	if(index >= ar->Length)
	{
		// TODO handle error
		assert(false);
	}
	ar[index] = UnwrapRef(val);
}

jobject JNIEnv::GetObjectArrayElement(jobjectArray array, jsize index)
{
	Object* ar __gc[] = __try_cast<Object* __gc[]>(UnwrapRef(array));
	if(index >= ar->Length)
	{
		// TODO handle error
		assert(false);
	}
	return MakeLocalRef(ar[index]);
}

#define GET_SET_ARRAY_REGION(Name, JavaType, ClrType) \
void JNICALL JNIEnv::Get##Name##ArrayRegion(JavaType##Array array, jsize start, jsize l, JavaType *buf) \
{ \
	ClrType ar __gc[] = __try_cast<ClrType __gc[]>(UnwrapRef(array)); \
	for(; l != 0; l--) \
	{ \
		*buf++ = ar[start++]; \
	} \
} \
void JNICALL JNIEnv::Set##Name##ArrayRegion(JavaType##Array array, jsize start, jsize l, JavaType *buf) \
{ \
	ClrType ar __gc[] = __try_cast<ClrType __gc[]>(UnwrapRef(array)); \
	for(; l != 0; l--) \
	{ \
		ar[start++] = *buf++; \
	} \
}

#pragma warning (push)
// stop the compiler from wanking about "forcing value to bool 'true' or 'false' (performance warning)"
#pragma warning (disable : 4800)
GET_SET_ARRAY_REGION(Boolean, jboolean, bool)
#pragma warning (pop)
GET_SET_ARRAY_REGION(Byte, jbyte, System::SByte)
GET_SET_ARRAY_REGION(Char, jchar, wchar_t)
GET_SET_ARRAY_REGION(Short, jshort, short)
GET_SET_ARRAY_REGION(Int, jint, int)
GET_SET_ARRAY_REGION(Long, jlong, __int64)
GET_SET_ARRAY_REGION(Float, jfloat, float)
GET_SET_ARRAY_REGION(Double, jdouble, double)

#define GET_SET_ARRAY_ELEMENTS(Type,type,cpptype) \
type* JNIEnv::Get##Type##ArrayElements(type##Array array, jboolean *isCopy)\
{\
	cpptype ar __gc[] = __try_cast<cpptype __gc[]>(UnwrapRef(array));\
	type* p = new type[ar->Length];\
	if(ar->Length)\
	{\
		cpptype __pin* par = &ar[0];\
		memcpy(p, par, ar->Length * sizeof(cpptype));\
	}\
	if(isCopy)\
	{\
		*isCopy = JNI_TRUE;\
	}\
	return p;\
}\
void JNIEnv::Release##Type##ArrayElements(type##Array array, type *elems, jint mode)\
{\
	if(mode == 0 || mode == JNI_COMMIT)\
	{\
		cpptype ar __gc[] = __try_cast<cpptype __gc[]>(UnwrapRef(array));\
		if(ar->Length)\
		{\
			cpptype __pin* p = &ar[0];\
			memcpy(p, elems, ar->Length * sizeof(cpptype));\
		}\
	}\
	if(mode == 0 || mode == JNI_ABORT)\
	{\
		delete[] elems;\
	}\
}

#pragma warning (push)
// stop the compiler from wanking about "forcing value to bool 'true' or 'false' (performance warning)"
#pragma warning (disable : 4800)
GET_SET_ARRAY_ELEMENTS(Boolean,jboolean,bool)
#pragma warning (pop)
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
	return MakeLocalRef(InvokeHelper(0, methodID, args, false));
}

jclass JNICALL JNIEnv::GetObjectClass(jobject obj)
{
	if(obj)
	{
		return (jclass)MakeLocalRef(VM::GetObjectClass(UnwrapRef(obj)));
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return 0;
}

jboolean JNICALL JNIEnv::IsInstanceOf(jobject obj, jclass clazz)
{
	if(clazz)
	{
		return obj && VM::IsInstanceOf(UnwrapRef(obj), UnwrapRef(clazz));
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return JNI_FALSE;
}

jboolean JNICALL JNIEnv::IsAssignableFrom(jclass sub, jclass sup)
{
	if(sub && sup)
	{
		return VM::IsAssignableFrom(UnwrapRef(sub), UnwrapRef(sup));
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return JNI_FALSE;
}

jclass JNICALL JNIEnv::GetSuperclass(jclass sub)
{
	if(sub)
	{
		return (jclass)MakeLocalRef(VM::GetSuperclass(UnwrapRef(sub)));
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return 0;
}

jobject JNICALL JNIEnv::NewLocalRef(jobject ref)
{
	return MakeLocalRef(UnwrapRef(ref));
}

jobject JNICALL JNIEnv::NewGlobalRef(jobject obj)
{
	//if(!obj)
	//{
	//	return 0;
	//}
	//// TODO search for an empty slot before adding it to the end...
	//return (jobject)-(GlobalRefs::globalRefs->Add(UnwrapRef(obj)) + 1);
	throw new InvalidOperationException();
}

void JNICALL JNIEnv::DeleteGlobalRef(jobject gref)
{
	//int i = int(gref);
	//if(i < 0)
	//{
	//	GlobalRefs::globalRefs->Item[(-i) - 1] = 0;
	//	return;
	//}
	//if(i > 0)
	//{
	//	DebugBreak();
	//}
	throw new InvalidOperationException();
}

void JNICALL JNIEnv::DeleteLocalRef(jobject obj)
{
	//int i = (int)obj;
	//if(i > 0)
	//{
	//	localRefs[i >> LOCAL_REF_SHIFT].DeleteLocalRef(i & LOCAL_REF_MASK);
	//	return;
	//}
	//if(i < 0)
	//{
	//	Console::WriteLine("bogus localref in DeleteLocalRef");
	//	DebugBreak();
	//}
	throw new InvalidOperationException();
}

jboolean JNICALL JNIEnv::IsSameObject(jobject obj1, jobject obj2)
{
	return UnwrapRef(obj1) == UnwrapRef(obj2);
}

jint JNICALL JNIEnv::MonitorEnter(jobject obj)
{
	Object* o = UnwrapRef(obj);
	if(o)
	{
		try
		{
			System::Threading::Monitor::Enter(o);
			return JNI_OK;
		}
		catch(System::Threading::ThreadInterruptedException*)
		{
			ThrowNew(FindClass("java/lang/InterruptedException"), "");
			return JNI_ERR;
		}
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return JNI_ERR;
}

jint JNICALL JNIEnv::MonitorExit(jobject obj)
{
	Object* o = UnwrapRef(obj);
	if(o)
	{
		try
		{
			System::Threading::Monitor::Exit(o);
			return JNI_OK;
		}
		catch(System::Threading::SynchronizationLockException*)
		{
			ThrowNew(FindClass("java/lang/IllegalMonitorStateException"), "");
			return JNI_ERR;
		}
	}
	ThrowNew(FindClass("java/lang/NullPointerException"), "");
	return JNI_ERR;
}

#pragma unmanaged
jint JNICALL JNIEnv::GetJavaVM(JavaVM **vm)
{
	static JavaVM theVM;
	*vm = &theVM;
	return 0;
}

#pragma managed
static jsize GetPrimitiveArrayElementSize(Array* ar)
{
	Type* type = ar->GetType()->GetElementType();
	if(type == __typeof(System::SByte) || type == __typeof(System::Boolean))
	{
		return 1;
	}
	else if(type == __typeof(System::Int16) || type == __typeof(System::Char))
	{
		return 2;
	}
	else if(type == __typeof(System::Int32) || type == __typeof(System::Single))
	{
		return 4;
	}
	else if(type == __typeof(System::Int64) || type == __typeof(System::Double))
	{
		return 8;
	}
	else
	{
		assert(false);
		return 1;
	}
}

void* JNICALL JNIEnv::GetPrimitiveArrayCritical(jarray array, jboolean *isCopy)
{
	Array* ar = __try_cast<Array*>(UnwrapRef(array));
	int len = ar->Length * GetPrimitiveArrayElementSize(ar);
	GCHandle h = GCHandle::Alloc(ar, GCHandleType::Pinned);
	try
	{
		char* buf = new char[len];
		if(!buf)
		{
			// TODO throw OutOfMemoryError
			assert(false);
			return 0;
		}
		memcpy(buf, (void*)h.AddrOfPinnedObject(), len);
		if(isCopy)
		{
			*isCopy = JNI_TRUE;
		}
		return buf;
	}
	__finally
	{
		h.Free();
	}
}

void JNICALL JNIEnv::ReleasePrimitiveArrayCritical(jarray array, void *carray, jint mode)
{
	if(mode == 0 || mode == JNI_COMMIT)
	{
		Array* ar = __try_cast<Array*>(UnwrapRef(array));
		int len = ar->Length * GetPrimitiveArrayElementSize(ar);
		GCHandle h = GCHandle::Alloc(ar, GCHandleType::Pinned);
		try
		{
			memcpy((void*)h.AddrOfPinnedObject(), carray, len);
		}
		__finally
		{
			h.Free();
		}
	}
	if(mode == 0 || mode == JNI_ABORT)
	{
		delete[] (char*)carray;
	}
}

jint JNICALL JNIEnv::GetVersion()
{
	// We implement (part of) JNI version 1.4
	return JNI_VERSION_1_4;
}

jboolean JNICALL JNIEnv::ExceptionCheck()
{
	//return pendingException != 0;
	throw new InvalidOperationException();
}

jmethodID JNICALL JNIEnv::FromReflectedMethod(jobject method)
{
	return (jmethodID)(void*)VM::MethodToCookie(UnwrapRef(method));
}

jfieldID JNICALL JNIEnv::FromReflectedField(jobject field)
{
	return (jfieldID)(void*)VM::FieldToCookie(UnwrapRef(field));
}

jobject JNICALL JNIEnv::ToReflectedMethod(jclass clazz, jmethodID methodID)
{
	return MakeLocalRef(VM::CookieToMethod(methodID));
}

jobject JNICALL JNIEnv::ToReflectedField(jclass clazz, jfieldID fieldID)
{
	return MakeLocalRef(VM::CookieToField(fieldID));
}

void JNICALL JNIEnv::FatalError(const char *msg)
{
	VM::FatalError(StringFromUTF8(msg));
}

jclass JNICALL JNIEnv::DefineClass(const char *name, jobject loader, const jbyte *buf, jsize len)
{
	System::Byte array __gc[] = new System::Byte __gc[len];
	Marshal::Copy((void*)buf, array, 0, len);
	return (jclass)MakeLocalRef(VM::DefineClass(StringFromUTF8(name), UnwrapRef(loader), array));
}

void JavaVM::reserved0()
{
	VM::FatalError("JavaVM::reserved0");
}

void JavaVM::reserved1()
{
	VM::FatalError("JavaVM::reserved1");
}

void JavaVM::reserved2()
{
	VM::FatalError("JavaVM::reserved2");
}

jint JavaVM::DestroyJavaVM()
{
	return JNI_ERR;
}

#pragma unmanaged
static void AttachCurrentThread_NotImplemented()
{
	assert(false);
	_asm int 3
}

#pragma managed
jint JavaVM::AttachCurrentThread(void **penv, void *args)
{
	// TODO do we need a new local ref frame?
	// TODO for now we only support attaching to an existing thread
	// TODO support args (JavaVMAttachArgs)
	JNIEnv* p = VM::GetEnv();
	if(p)
	{
		*penv = p;
		return JNI_OK;
	}
	AttachCurrentThread_NotImplemented();
	return JNI_ERR;
}
#pragma unmanaged

jint JavaVM::DetachCurrentThread()
{
	assert(false);
	_asm int 3
}

#pragma managed
jint JavaVM::GetEnv(void **penv, jint version)
{
	// TODO we should check the version
	JNIEnv* p = VM::GetEnv();
	if(p)
	{
		*penv = p;
		return JNI_OK;
	}
	return JNI_EDETACHED;
}

jint JNICALL JNIEnv::RegisterNatives(jclass clazz, const JNINativeMethod *methods, jint nMethods)
{
	Object* pclass = UnwrapRef(clazz);
	for(int i = 0; i < nMethods; i++)
	{
		if(!VM::SetNativeMethodPointer(pclass, StringFromUTF8(methods[i].name), StringFromUTF8(methods[i].signature), methods[i].fnPtr))
		{
			// TODO set the exception message
			ThrowNew(FindClass("java/lang/NoSuchMethodError"), "");
			return JNI_ERR;
		}
	}
	return JNI_OK;
}

jint JNICALL JNIEnv::UnregisterNatives(jclass clazz)
{
	VM::ResetNativeMethodPointers(UnwrapRef(clazz));
	return JNI_OK;
}

#pragma unmanaged

jint JavaVM::AttachCurrentThreadAsDaemon(void **penv, void *args)
{
	assert(false);
	_asm int 3
}

////////////////////////////////////////////////////////////////////////////
#pragma warning (disable : 4035)

jint JNICALL JNIEnv::PushLocalFrame(jint capacity) { assert(false); _asm int 3} 
jobject JNICALL JNIEnv::PopLocalFrame(jobject result) { assert(false); _asm int 3}
jint JNICALL JNIEnv::EnsureLocalCapacity(jint capacity) { assert(false); _asm int 3} 

jweak JNICALL JNIEnv::NewWeakGlobalRef(jobject obj) { assert(false); _asm int 3}
void JNICALL JNIEnv::DeleteWeakGlobalRef(jweak ref) { assert(false); _asm int 3}

jobject JNICALL JNIEnv::NewDirectByteBuffer(void* address, jlong capacity) { assert(false); _asm int 3}
void* JNICALL JNIEnv::GetDirectBufferAddress(jobject buf) { assert(false); _asm int 3}
jlong JNICALL JNIEnv::GetDirectBufferCapacity(jobject buf) { assert(false); _asm int 3}
