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

public __value class LocalRefStruct;

// NOTE this __value type needs to have the *exact* same layout as the unmanaged JNIENv class
__value struct JNIEnvContainer
{
	void* pVtable;
	LocalRefStruct __nogc* pLocalRefStruct;
};

public __value class LocalRefStruct
{
	[ThreadStatic]
	static JNIEnvContainer jniEnv;
	LocalRefStruct __nogc* pPrevLocalRefCache;
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
	Object* overflow __gc[];
	Exception* pendingException;
public:

	static JNIEnv* GetEnv()
	{
		if(jniEnv.pVtable != 0)
		{
			return (JNIEnv*)(void*)&jniEnv;
		}
		return 0;
	}

	IntPtr Enter()
	{
		if(jniEnv.pVtable == 0)
		{
			// HACK initialize the vtable ptr
			JNIEnv env;
			jniEnv.pVtable = *((void**)&env);
		}
		pPrevLocalRefCache = jniEnv.pLocalRefStruct;
		// NOTE since this __value type can (should) only be allocated on the stack,
		// it is "safe" to store the this pointer in a __nogc*, but the compiler
		// doesn't know this, so we have to use a __pin* to bypass its checks.
		LocalRefStruct __pin* pPinHack = this;
		jniEnv.pLocalRefStruct = pPinHack;
		return (IntPtr)&jniEnv;
	}

	void Leave()
	{
		jniEnv.pLocalRefStruct = pPrevLocalRefCache;
		if(pendingException)
		{
			// TODO retain the stack trace of the exception object
			throw pendingException;
		}
	}

	__property Exception* get_PendingException()
	{
		return pendingException;
	}

	__property void set_PendingException(Exception* exception)
	{
		pendingException = exception;
	}

	IntPtr MakeLocalRef(Object* o)
	{
		if(o == 0)
		{
			return 0;
		}
		Object** p = &loc1;
		for(int i = 0; i < 10; i++)
		{
			if(p[i] == 0)
			{
				p[i] = o;
				return i + 1;
			}
		}
		if(!overflow)
		{
			// HACK we use a very large dynamic table size, because we don't yet support growing it
			overflow = new Object* __gc[256];
		}
		for(int i = 0; i < overflow->Length; i++)
		{
			if(overflow[i] == 0)
			{
				overflow[i] = o;
				return i + 11;
			}
		}
		throw new NotImplementedException(S"Growing the localref table is not implemented");
	}

	void DeleteLocalRef(jobject o)
	{
		int i = (int)o;
		if(i < 0)
		{
			DebugBreak();
		}
		if(i > 0)
		{
			if(i <= 10)
			{
				Object** p = &loc1;
				p[i - 1] = 0;
			}
			else
			{
				overflow[i - 11] = 0;
			}
		}
	}

	Object* UnwrapLocalRef(IntPtr localref)
	{
		if(localref == 0)
		{
			return 0;
		}
		if(int(localref) <= 10)
		{
			Object** p = &loc1;
			return p[int(localref) - 1];
		}
		else
		{
			return overflow[int(localref) - 11];
		}
	}
};

public __gc class JNI
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
	static int LoadNativeLibrary(String* name);
	static IntPtr GetJniFuncPtr(String* method, String* sig, String* clazz);
	static jobject MakeGlobalRef(Object* o);
	static void DeleteGlobalRef(jobject o);
	static Object* UnwrapGlobalRef(jobject o);
};

public __gc interface IJniHack
{
public:
	virtual MethodBase* GetMethod(Object* clazz, String* name, String* sig, bool isStatic)=0;
	virtual FieldInfo* GetField(Object* clazz, String* name, String* sig, bool isStatic)=0;
	virtual Object* FindClass(String* name)=0;
	virtual Exception* UnsatisfiedLinkError(String* msg)=0;
	virtual Object* GetClassFromType(Type* type)=0;
};

public __gc class VM
{
private:
	static IJniHack* pJniHack = 0;
	static VM()
	{
		pJniHack = __try_cast<IJniHack*>(Activator::CreateInstance(Assembly::Load(S"ik.vm.net")->GetType(S"JniHack")));
	}
public:
	// NOTE we use this nasty construct to avoid a compile time dependency on JVM.DLL (which already has a compile time
	// dependency on us)
	static MethodBase* GetMethod(Object* clazz, String* name, String* sig, bool isStatic)
	{
		return pJniHack->GetMethod(clazz, name, sig, isStatic);
	}
	static FieldInfo* GetField(Object* clazz, String* name, String* sig, bool isStatic)
	{
		return pJniHack->GetField(clazz, name, sig, isStatic);
	}
	static Object* FindClass(String* javaName)
	{
		return pJniHack->FindClass(javaName);
	}
	static Exception* UnsatisfiedLinkError(String* msg)
	{
		return pJniHack->UnsatisfiedLinkError(msg);
	}
	static Object* GetClassFromType(Type* type)
	{
		return pJniHack->GetClassFromType(type);
	}
};

template<class T> T __unbox(Object* o)
{
	// HACK the MC++ compiler has a bug when unboxing, the static_cast<> is to work around that bug
	return *(static_cast<T __gc*>(__try_cast<__box T*>(o)));
}
