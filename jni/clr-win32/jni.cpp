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

#include "stdafx.h"
#include <malloc.h>
#include "jnienv.h"
#include "jni.h"

#pragma managed

#include <stdio.h>

using namespace System;
using namespace System::Collections;
using namespace System::Runtime::InteropServices;
using namespace System::Reflection;

typedef JNIEXPORT jint (JNICALL *PJNI_ONLOAD)(JavaVM* vm, void* reserved);

int JNI::LoadNativeLibrary(String* name)
{
	HMODULE hMod = LoadLibrary(name);
	if(!hMod)
	{
		return 0;
	}
	//void* pOnLoad = GetProcAddress(hMod, S"_JNI_OnLoad@8");
	//if(pOnLoad)
	//{
	//	JavaVM* pVM;
	//	((JNIEnv*)0)->JNIEnv::GetJavaVM(&pVM);
	//	LocalRefStruct loc;
	//	loc.Enter();
	//	jint version = ((PJNI_ONLOAD)pOnLoad)(pVM, 0);
	//	try
	//	{
	//		loc.Leave();
	//	}
	//	catch(...)
	//	{
	//		// If the JNI_OnLoad procedure threw an exception, we have to unload the library
	//		FreeLibrary(hMod);
	//		throw;
	//	}
	//	if(version != JNI_VERSION_1_1 && version != JNI_VERSION_1_2 && version != JNI_VERSION_1_4)
	//	{
	//		// NOTE we don't call JNI_OnUnload when the version doesn't match!
	//		FreeLibrary(hMod);
	//		throw VM::UnsatisfiedLinkError(String::Format(S"Unsupported JNI version 0x{0:X} required by {1}", __box(version), name));
	//	}
	//}
	libs->Add(__box((int)hMod));
	return 1;
}

Type* JNI::GetLocalRefStructType()
{
	return 0;
	//return __typeof(LocalRefStruct);
}

MethodInfo* JNI::GetJniFuncPtrMethod()
{
	return __typeof(JNI)->GetMethod("GetJniFuncPtr");
}

IntPtr JNI::GetJniFuncPtr(String* method, String* sig, String* clazz)
{
	System::Text::StringBuilder* mangledSig = new System::Text::StringBuilder();
	int sp = 0;
	for(int i = 1; sig->Chars[i] != ')'; i++)
	{
		switch(sig->Chars[i])
		{
		case '[':
			mangledSig->Append(S"_3");
			sp += 4;
			while(sig->Chars[++i] == '[')
			{
				mangledSig->Append(S"_3");
			}
			mangledSig->Append(sig->Chars[i]);
			if(sig->Chars[i] == 'L')
			{
				while(sig->Chars[++i] != ';')
				{
					if(sig->Chars[i] == '/')
					{
						mangledSig->Append(S"_");
					}
					else if(sig->Chars[i] == '_')
					{
						mangledSig->Append(S"_1");
					}
					else
					{
						mangledSig->Append(sig->Chars[i]);
					}
				}
				mangledSig->Append(S"_2");
			}
			break;
		case 'L':
			sp += 4;
			mangledSig->Append(S"L");
			while(sig->Chars[++i] != ';')
			{
				if(sig->Chars[i] == '/')
				{
					mangledSig->Append(S"_");
				}
				else if(sig->Chars[i] == '_')
				{
					mangledSig->Append(S"_1");
				}
				else
				{
					mangledSig->Append(sig->Chars[i]);
				}
			}
			mangledSig->Append(S"_2");
			break;
		case 'J':
		case 'D':
			mangledSig->Append(sig->Chars[i]);
			sp += 8;
			break;
		case 'F':
		case 'I':
		case 'C':
		case 'Z':
		case 'S':
		case 'B':
			mangledSig->Append(sig->Chars[i]);
			sp += 4;
			break;
		default:
			DebugBreak();
			throw new NotImplementedException();
		}
	}
	void* func = 0;
	// TODO implement this correctly
	String* methodName = String::Format(S"_Java_{0}_{1}@{2}", clazz->Replace(S"_", S"_1")->Replace('/', '_'), method->Replace(S"_", S"_1"), __box(sp + 8));
	for(int i = 0; i < libs->Count; i++)
	{
		HMODULE hMod = (HMODULE)__unbox<int>(libs->Item[i]);
		func = GetProcAddress(hMod, methodName);
		if(func)
		{
			return (IntPtr)func;
		}
	}
	methodName = String::Concat(String::Format(S"_Java_{0}_{1}__{2}@", clazz->Replace(S"_", S"_1")->Replace('/', '_'), method->Replace(S"_", S"_1"), mangledSig), __box(sp + 8));
	for(int i = 0; i < libs->Count; i++)
	{
		HMODULE hMod = (HMODULE)__unbox<int>(libs->Item[i]);
		func = GetProcAddress(hMod, methodName);
		if(func)
		{
			return (IntPtr)func;
		}
	}
	throw VM::UnsatisfiedLinkError(methodName);
}

//// If we put the ThreadStatic in LocalRefStruct we get an ExecutionEngineException (?!)
//__gc class TlsHack
//{
//public:
//	[ThreadStatic]
//	static JNIEnv* pJNIEnv;
//};
//
//JNIEnv* LocalRefStruct::GetEnv()
//{
//	return TlsHack::pJNIEnv;
//}
//
//IntPtr LocalRefStruct::Enter()
//{
//	pJNIEnv = TlsHack::pJNIEnv;
//	if(!pJNIEnv)
//	{
//		// TODO we should create a managed helper object that deletes JNIEnv when the thread dies
//		pJNIEnv = TlsHack::pJNIEnv = new JNIEnv();
//		pJNIEnv->pendingException = 0;
//		pJNIEnv->localRefSlot = 0;
//		localRefs = pJNIEnv->localRefs = new LocalRefListEntry __gc[32];
//	}
//	else
//	{
//		pPrevLocalRefCache = pJNIEnv->pActiveLocalRefCache;
//		localRefs = pJNIEnv->localRefs;
//	}
//
//	pJNIEnv->localRefSlot++;
//	if(pJNIEnv->localRefSlot >= 32)
//	{
//		// TODO instead of bailing out, we should grow the array
//		VM::FatalError("JNI nesting too deep");
//	}
//
//	// NOTE since this __value type can (should) only be allocated on the stack,
//	// it is "safe" to store the this pointer in a __nogc*, but the compiler
//	// doesn't know this, so we have to use a __pin* to bypass its checks.
//	LocalRefStruct __pin* pPinHack = this;
//	pJNIEnv->pActiveLocalRefCache = pPinHack;
//	localRefs[pJNIEnv->localRefSlot].static_list = &pPinHack->fastlocalrefs.loc1;
//	return (IntPtr)pJNIEnv;
//}
//
//void LocalRefStruct::Leave()
//{
//	Object* x = pJNIEnv->UnwrapRef(pJNIEnv->pendingException);
//	pJNIEnv->pendingException = 0;
//	pJNIEnv->pActiveLocalRefCache = pPrevLocalRefCache;
//	localRefs[pJNIEnv->localRefSlot].dynamic_list = 0;
//	// TODO figure out if it is legal to Leave a JNI method while PushLocalFrame is active (i.e. without the corresponding PopLocalFrame)
//	pJNIEnv->localRefSlot--;
//	if(x)
//	{
//		throw x;
//	}
//}
//
//IntPtr LocalRefStruct::MakeLocalRef(Object* obj)
//{
//	if(obj == 0)
//	{
//		return 0;
//	}
//	int i = localRefs[pJNIEnv->localRefSlot].MakeLocalRef(obj);
//	if(i >= 0)
//	{
//		return (pJNIEnv->localRefSlot << LOCAL_REF_SHIFT) + i;
//	}
//	// TODO consider allocating a new slot (if we do this, the code in
//	// PushLocalFrame/PopLocalFrame (and Leave) must be fixed to take this into account)
//	VM::FatalError("Too many JNI local references");
//	return 0;
//}
//
//Object* LocalRefStruct::UnwrapLocalRef(IntPtr localref)
//{
//	int i = (int)localref;
//	return localRefs[i >> LOCAL_REF_SHIFT].UnwrapLocalRef(i & LOCAL_REF_MASK);
//}
