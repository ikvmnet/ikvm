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
using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

public sealed class JniHelper
{
	public static IntPtr GetMethodCookie(object clazz, string name, string sig, bool isStatic)
	{
		TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(NativeCode.java.lang.VMClass.getType(clazz));
		wrapper.Finish();
		MethodWrapper mw = wrapper.GetMethodWrapper(new MethodDescriptor(wrapper.GetClassLoader(), name, sig), true);
		if(mw != null)
		{
			if(mw.IsStatic == isStatic)
			{
				return mw.Cookie;
			}
		}
		return (IntPtr)0;
	}

	// this method returns a simplified method argument descriptor.
	// some examples:
	// "()V" -> ""
	// "(ILjava/lang/String;)I" -> "IL"
	// "([Ljava/lang/String;)V" -> "L"
	public static string GetMethodArgList(IntPtr cookie)
	{
		StringBuilder sb = new StringBuilder();
		string s = MethodWrapper.FromCookie(cookie).Descriptor.Signature;
		for(int i = 1;; i++)
		{
			switch(s[i])
			{
				case '[':
					while(s[i] == '[') i++;
					if(s[i] == 'L')
					{
						while(s[i] != ';') i++;
					}
					sb.Append('L');
					break;
				case 'L':
					while(s[i] != ';') i++;
					sb.Append('L');
					break;
				case ')':
					return sb.ToString();
				default:
					sb.Append(s[i]);
					break;
			}
		}
	}

	[StackTraceInfo(Hidden = true)]
	public static object InvokeMethod(IntPtr cookie, object obj, object[] args, bool nonVirtual)
	{
		return MethodWrapper.FromCookie(cookie).Invoke(obj, args, nonVirtual);
	}

	public static IntPtr GetFieldCookie(object clazz, string name, string sig, bool isStatic)
	{
		TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(NativeCode.java.lang.VMClass.getType(clazz));
		wrapper.Finish();
		// TODO GetFieldWrapper should take sig (what about searching the base classes?)
		FieldWrapper fw = wrapper.GetFieldWrapper(name);
		if(fw != null)
		{
			if(fw.IsStatic == isStatic)
			{
				return fw.Cookie;
			}
		}
		return (IntPtr)0;
	}

	public static void SetFieldValue(IntPtr cookie, object obj, object val)
	{
		FieldWrapper.FromCookie(cookie).SetValue(obj, val);
	}

	public static object GetFieldValue(IntPtr cookie, object obj)
	{
		return FieldWrapper.FromCookie(cookie).GetValue(obj);
	}

	public static object FindClass(string javaName)
	{
		TypeWrapper wrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName(javaName);
		wrapper.Finish();
		return NativeCode.java.lang.VMClass.getClassFromWrapper(wrapper);
	}

	public static Exception UnsatisfiedLinkError(string msg)
	{
		return JavaException.UnsatisfiedLinkError(msg);
	}

	public static object GetClassFromType(Type type)
	{
		return NativeCode.java.lang.VMClass.getClassFromType(type);
	}
}

public interface IJniProvider
{
	int LoadNativeLibrary(string filename);
	Type GetLocalRefStructType();
	MethodInfo GetJniFuncPtrMethod();
}
