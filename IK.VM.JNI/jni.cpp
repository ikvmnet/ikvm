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
