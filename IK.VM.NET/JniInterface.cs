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
using System.Reflection;

public sealed class JniHelper
{
	public static System.Reflection.MethodBase GetMethod(object clazz, string name, string sig, bool isStatic)
	{
		// TODO this is totally broken, because JNI needs to support redirection
		TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(NativeCode.java.lang.Class.getType(clazz));
		wrapper.Finish();
		MethodDescriptor md = new MethodDescriptor(wrapper.GetClassLoader(), name, sig);
		BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic;
		if(isStatic)
		{
			bindings |= BindingFlags.Static;
		}
		else
		{
			bindings |= BindingFlags.Instance;
		}
		if(name == "<init>")
		{
			return wrapper.Type.GetConstructor(bindings, null, md.ArgTypes, null);
		}
		Type type = wrapper.Type;
		while(type != null)
		{
			MethodInfo m = type.GetMethod(name, bindings, null, CallingConventions.Standard, md.ArgTypes, null);
			if(m != null)
			{
				return m;
			}
			type = type.BaseType;
		}
		return null;
	}

	public static System.Reflection.FieldInfo GetField(object clazz, string name, string sig, bool isStatic)
	{
		// TODO this is totally broken, because JNI needs to support redirection
		TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromType(NativeCode.java.lang.Class.getType(clazz));
		wrapper.Finish();
		MethodDescriptor md = new MethodDescriptor(wrapper.GetClassLoader(), name, sig);
		BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic;
		if(isStatic)
		{
			bindings |= BindingFlags.Static;
		}
		else
		{
			bindings |= BindingFlags.Instance;
		}
		return wrapper.Type.GetField(name, bindings);
	}

	public static object FindClass(string javaName)
	{
		TypeWrapper wrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName(javaName);
		wrapper.Finish();
		return NativeCode.java.lang.Class.getClassFromWrapper(wrapper);
	}

	public static Exception UnsatisfiedLinkError(string msg)
	{
		return JavaException.UnsatisfiedLinkError(msg);
	}

	public static object GetClassFromType(Type type)
	{
		return NativeCode.java.lang.Class.getClassFromType(type);
	}
}

public interface IJniProvider
{
	int LoadNativeLibrary(string filename);
	Type GetLocalRefStructType();
	MethodInfo GetJniFuncPtrMethod();
}
