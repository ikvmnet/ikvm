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

public __value class LocalRefStruct;

// NOTE this __value type needs to have the *exact* same layout as the unmanaged JNIENv class
//__nogc struct JNIEnvContainer
//{
//	void* pVtable;
//	LocalRefStruct __nogc* pLocalRefStruct;
//};

[StructLayout(LayoutKind::Sequential)]
__value struct LocalRefCache
{
	Object* loc1;
	Object* loc2;
	Object* loc3;
	Object* loc4;
	Object* loc5;
	Object* loc6;
	Object* loc7;
	Object* loc8;
	Object* loc9;
	Object* loc10;
};

public __value class LocalRefStruct
{
	void* pPrevLocalRefCache;
	int cookie;
	LocalRefCache cache;
	Object* overflow __gc[];
	Exception* pendingException;
public:

	static JNIEnv* GetEnv();

	static LocalRefStruct* Current();

	IntPtr Enter();
	void Leave();

	__property Exception* get_PendingException();
	__property void set_PendingException(Exception* exception);

	IntPtr MakeLocalRef(Object* o);
	void DeleteLocalRef(jobject o);
	Object* UnwrapLocalRef(IntPtr localref);
};

public __gc class JNI : public IJniProvider
{
	static ArrayList* libs = new ArrayList();
	static ArrayList* globalRefs = new ArrayList();
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

	static IntPtr GetJniFuncPtr(String* name, String* sig, String* clazz);
	static jobject MakeGlobalRef(Object* o);
	static void DeleteGlobalRef(jobject o);
	static Object* UnwrapGlobalRef(jobject o);
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
	[StackTraceInfo(Hidden = true)]
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
	static Object* GetClassFromType(Type* type)
	{
		return JniHelper::GetClassFromType(type);
	}
};
