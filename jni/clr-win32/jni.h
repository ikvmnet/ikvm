/*
  Copyright (C) 2002, 2004 Jeroen Frijters

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

#pragma once

using namespace System;
using namespace System::Collections;
using namespace System::Runtime::InteropServices;
using namespace System::Reflection;

class JNIEnv;

#pragma managed

template<class T> T __unbox(Object* o)
{
	// HACK the MC++ compiler has a bug when unboxing, the static_cast<> is to work around that bug
	return *(static_cast<T __gc*>(__try_cast<__box T*>(o)));
}

public __gc class JNI : public IJniProvider
{
	static ArrayList* libs = new ArrayList();
	[DllImport("kernel32")]
	[System::Security::SuppressUnmanagedCodeSecurityAttribute]
	static HMODULE LoadLibrary(String* lpLibFileName);
	[DllImport("kernel32")]
	[System::Security::SuppressUnmanagedCodeSecurityAttribute]
	static void* GetProcAddress(HMODULE hMod, String* lpProcName);
public:
	int LoadNativeLibrary(String* name);
	Type* GetLocalRefStructType();
	MethodInfo* GetJniFuncPtrMethod();

	void** GetVtable()
	{
		JNIEnv p;
		return *(void***)&p;
	}

	IntPtr LoadLibraryFoo(String* name)
	{
		return (IntPtr)(void*)LoadLibrary(name);
	}

	void FreeLibraryFoo(IntPtr p)
	{
		FreeLibrary((HMODULE)(void*)p);
	}

	IntPtr GetProcAddress(IntPtr library, String* name, int argcount)
	{
		String* n = String::Format(S"_{0}@{1}", name, __box(argcount));
		return (IntPtr)GetProcAddress((HMODULE)(void*)library, n);
	}

	IntPtr GetJavaVM()
	{
		JavaVM* pvm;
		((JNIEnv*)0)->JNIEnv::GetJavaVM(&pvm);
		return pvm;
	}

	typedef JNIEXPORT jint (JNICALL *PJNI_ONLOAD)(JavaVM* vm, void* reserved);

	int CallOnLoad(IntPtr pFunc, IntPtr javavm, IntPtr reserved)
	{
		return ((PJNI_ONLOAD)(void*)pFunc)((JavaVM*)(void*)javavm, (void*)reserved);
	}

	static IntPtr GetJniFuncPtr(String* name, String* sig, String* clazz);
};

public __gc class VM
{
public:
	static IntPtr GetMethodCookie(Object* clazz, String* name, String* sig, bool isStatic)
	{
		return JniHelper::GetMethodCookie(clazz, name, sig, isStatic);
	}
	static String* GetMethodArgList(IntPtr cookie)
	{
		return JniHelper::GetMethodArgList(cookie);
	}
	static Object* InvokeMethod(IntPtr cookie, Object* obj, Object* args[], bool nonVirtual)
	{
		return JniHelper::InvokeMethod(cookie, obj, args, nonVirtual);
	}
	static IntPtr GetFieldCookie(Object* clazz, String* name, String* sig, bool isStatic)
	{
		return JniHelper::GetFieldCookie(clazz, name, sig, isStatic);
	}
	static Object* GetFieldValue(IntPtr cookie, Object* obj)
	{
		return JniHelper::GetFieldValue(cookie, obj);
	}
	static void SetFieldValue(IntPtr cookie, Object* obj, Object* value)
	{
		JniHelper::SetFieldValue(cookie, obj, value);
	}
	static Object* FindClass(String* javaName)
	{
		return JniHelper::FindClass(javaName);
	}
	static Exception* UnsatisfiedLinkError(String* msg)
	{
		return JniHelper::UnsatisfiedLinkError(msg);
	}
	static Object* GetObjectClass(Object* o)
	{
		return JniHelper::GetObjectClass(o);
	}
	static bool IsInstanceOf(Object* o, Object* clazz)
	{
		return JniHelper::IsInstanceOf(o, clazz);
	}
	static bool IsAssignableFrom(Object* sub, Object* sup)
	{
		return JniHelper::IsAssignableFrom(sub, sup);
	}
	static Object* GetSuperclass(Object* sub)
	{
		return JniHelper::GetSuperclass(sub);
	}
	static Object* AllocObject(Object* clazz)
	{
		return JniHelper::AllocObject(clazz);
	}
	static IntPtr MethodToCookie(Object* method)
	{
		return JniHelper::MethodToCookie(method);
	}
	static IntPtr FieldToCookie(Object* field)
	{
		return JniHelper::FieldToCookie(field);
	}
	static Object* CookieToMethod(IntPtr method)
	{
		return JniHelper::CookieToMethod(method);
	}
	static Object* CookieToField(IntPtr field)
	{
		return JniHelper::CookieToField(field);
	}
	static void FatalError(String* msg)
	{
		Console::Error->WriteLine(S"Fatal Error in JNI: {0}", msg);
		JniHelper::FatalError(msg);
	}
	static Object* DefineClass(String* name, Object* loader, System::Byte array __gc[])
	{
		return JniHelper::DefineClass(name, loader, array);
	}
	static bool SetNativeMethodPointer(Object* clazz, String* name, String* signature, IntPtr methodPtr)
	{
		return JniHelper::SetNativeMethodPointer(clazz, name, signature, methodPtr);
	}
	static void ResetNativeMethodPointers(Object* clazz)
	{
		return JniHelper::ResetNativeMethodPointers(clazz);
	}

	static JNIEnv* GetEnv()
	{
		return (JNIEnv*)(void*)JniHelper::GetEnv();
	}
	static jobject MakeLocalRef(JNIEnv* pEnv, Object* obj)
	{
		return (jobject)(void*)JniHelper::MakeLocalRef(pEnv, obj);
	}
	static Object* UnwrapRef(JNIEnv* pEnv, jobject obj)
	{
		return JniHelper::UnwrapRef(pEnv, obj);
	}
};
