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
#include <malloc.h>
#include "jnienv.h"
#include "jni.h"

#pragma managed

#include <stdio.h>

using namespace System;
using namespace System::Collections;
using namespace System::Runtime::InteropServices;
using namespace System::Reflection;

int JNI::LoadNativeLibrary(String* name)
{
	HMODULE hMod = LoadLibrary(name);
	if(!hMod)
	{
		return 0;
	}
	libs->Add(__box((int)hMod));
	return 1;
}

Type* JNI::GetLocalRefStructType()
{
	return __typeof(LocalRefStruct);
}

MethodInfo* JNI::GetJniFuncPtrMethod()
{
	return __typeof(JNI)->GetMethod("GetJniFuncPtr");
}

jobject JNI::MakeGlobalRef(Object* o)
{
	if(!o)
	{
		return 0;
	}
	// TODO search for an empty slot before adding it to the end...
	return (jobject)-(globalRefs->Add(o) + 1);
}

void JNI::DeleteGlobalRef(jobject o)
{
	if(o)
	{
		int i = int(o);
		if(i < 0)
		{
			globalRefs->Item[(-i) - 1] = 0;
			return;
		}
		DebugBreak();
	}
}

Object* JNI::UnwrapGlobalRef(jobject o)
{
	if(!o)
	{
		return 0;
	}
	int i = int(o);
	if(i < 0)
	{
		return globalRefs->Item[(-i) - 1];
	}
	DebugBreak();
	return 0;
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

// NOTE we have only one global JNIEnv*, because allocating the JNIEnv is TLS proved too much of a headache,
// we only keep the pointer to the current frame (LocalRefStruct) in a TLS variable
static JNIEnv jniEnv;

// If we put the ThreadStatic in LocalRefStruct we get an ExecutionEngineException (?!)
__gc class TlsHack
{
public:
	[ThreadStatic]
	static void* currentLocalRefStruct;
};

JNIEnv* LocalRefStruct::GetEnv()
{
	if(TlsHack::currentLocalRefStruct)
	{
		return &jniEnv;
	}
	return 0;
}

LocalRefStruct* LocalRefStruct::Current()
{
	return (LocalRefStruct*)TlsHack::currentLocalRefStruct;
}

IntPtr LocalRefStruct::Enter()
{
	pPrevLocalRefCache = TlsHack::currentLocalRefStruct;
	// NOTE since this __value type can (should) only be allocated on the stack,
	// it is "safe" to store the this pointer in a __nogc*, but the compiler
	// doesn't know this, so we have to use a __pin* to bypass its checks.
	LocalRefStruct __pin* pPinHack = this;
	TlsHack::currentLocalRefStruct = pPinHack;
	return (IntPtr)&jniEnv;
}

void LocalRefStruct::Leave()
{
	TlsHack::currentLocalRefStruct = pPrevLocalRefCache;
	if(pendingException)
	{
		// TODO retain the stack trace of the exception object
		throw pendingException;
	}
}

Exception* LocalRefStruct::get_PendingException()
{
	return pendingException;
}

void LocalRefStruct::set_PendingException(Exception* exception)
{
	pendingException = exception;
}

IntPtr LocalRefStruct::MakeLocalRef(Object* o)
{
	if(o == 0)
	{
		return 0;
	}
	Object** p = &cache.loc1;
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

void LocalRefStruct::DeleteLocalRef(jobject o)
{
	int i = (int)o;
	if(i < 0)
	{
		Console::WriteLine("bogus localref in DeleteLocalRef");
		DebugBreak();
	}
	if(i > 0)
	{
		if(i <= 10)
		{
			Object** p = &cache.loc1;
			p[i - 1] = 0;
		}
		else
		{
			overflow[i - 11] = 0;
		}
	}
}

Object* LocalRefStruct::UnwrapLocalRef(IntPtr localref)
{
	if(localref == 0)
	{
		return 0;
	}
	if(int(localref) < 0)
	{
		Console::WriteLine("bogus localref in UnwrapLocalRef");
		DebugBreak();
	}
	if(int(localref) <= 10)
	{
		Object** p = &cache.loc1;
		return p[int(localref) - 1];
	}
	else
	{
		return overflow[int(localref) - 11];
	}
}
