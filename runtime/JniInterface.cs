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
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

public sealed class JniHelper
{
	// NOTE sig contains slashed class names
	public static IntPtr GetMethodCookie(object clazz, string name, string sig, bool isStatic)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
			MethodWrapper mw = wrapper.GetMethodWrapper(new MethodDescriptor(name, sig.Replace('/', '.')), true);
			if(mw != null)
			{
				if(mw.IsStatic == isStatic)
				{
					mw.Link();
					return mw.Cookie;
				}
			}
			return (IntPtr)0;
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	// this method returns a simplified method argument descriptor.
	// some examples:
	// "()V" -> ""
	// "(ILjava.lang.String;)I" -> "IL"
	// "([Ljava.lang.String;)V" -> "L"
	public static string GetMethodArgList(IntPtr cookie)
	{
		try
		{
			StringBuilder sb = new StringBuilder();
			string s = MethodWrapper.FromCookie(cookie).Signature;
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
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object InvokeMethod(IntPtr cookie, object obj, object[] args, bool nonVirtual)
	{
		try
		{
			return MethodWrapper.FromCookie(cookie).Invoke(obj, args, nonVirtual);
		}
		catch(Exception x)
		{
			throw ExceptionHelper.MapExceptionFast(x);
		}
	}

	// NOTE sig contains slashed class names
	public static IntPtr GetFieldCookie(object clazz, string name, string sig, bool isStatic)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
			wrapper.Finish();
			// TODO what about searching the base classes?
			FieldWrapper fw = wrapper.GetFieldWrapper(name, wrapper.GetClassLoader().ExpressionTypeWrapper(sig.Replace('/', '.')));
			if(fw != null)
			{
				if(fw.IsStatic == isStatic)
				{
					return fw.Cookie;
				}
			}
			return (IntPtr)0;
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static void SetFieldValue(IntPtr cookie, object obj, object val)
	{
		try
		{
			FieldWrapper.FromCookie(cookie).SetValue(obj, val);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object GetFieldValue(IntPtr cookie, object obj)
	{
		try
		{
			return FieldWrapper.FromCookie(cookie).GetValue(obj);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object FindClass(string javaName)
	{
		try
		{
			// TODO instead of using the bootstrap class loader, we need to use the system (aka application) class loader
			TypeWrapper wrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(javaName.Replace('/', '.'));
			wrapper.Finish();
			return NativeCode.java.lang.VMClass.getClassFromWrapper(wrapper);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static Exception UnsatisfiedLinkError(string msg)
	{
		try
		{
			return JavaException.UnsatisfiedLinkError(msg);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	[Obsolete]
	public static object GetClassFromType(Type type)
	{
		try
		{
			return NativeCode.java.lang.VMClass.getClassFromType(type);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object GetObjectClass(object o)
	{
		try
		{
			return NativeCode.java.lang.VMClass.getClassFromType(o.GetType());
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static bool IsInstanceOf(object o, object clazz)
	{
		try
		{
			return IsAssignableFrom(clazz, GetObjectClass(o));
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static bool IsAssignableFrom(object sub, object sup)
	{
		try
		{
			TypeWrapper w1 = NativeCode.java.lang.VMClass.getWrapperFromClass(sub);
			TypeWrapper w2 = NativeCode.java.lang.VMClass.getWrapperFromClass(sup);
			return w2.IsAssignableTo(w1);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}
	
	public static object GetSuperclass(object clazz)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz).BaseTypeWrapper;
			return wrapper == null ? null : NativeCode.java.lang.VMClass.getClassFromWrapper(wrapper);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object AllocObject(object clazz)
	{
		try
		{
			TypeWrapper wrapper = NativeCode.java.lang.VMClass.getWrapperFromClass(clazz);
			wrapper.Finish();
			// TODO add error handling (e.g. when trying to instantiate an interface or abstract class)
			return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType);
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static IntPtr MethodToCookie(object method)
	{
		try
		{
			MethodWrapper mw = (MethodWrapper)method.GetType().GetField("methodCookie", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(method);
			return mw.Cookie;
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static IntPtr FieldToCookie(object field)
	{
		try
		{
			FieldWrapper fw = (FieldWrapper)field.GetType().GetField("fieldCookie", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(field);
			return fw.Cookie;
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object CookieToMethod(IntPtr method)
	{
		try
		{
			MethodWrapper mw = MethodWrapper.FromCookie(method);
			TypeWrapper tw;
			if(mw.Name == "<init>")
			{
				tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.reflect.Constructor");
			}
			else
			{
				tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.reflect.Method");
			}
			object clazz = NativeCode.java.lang.VMClass.getClassFromWrapper(mw.DeclaringType);
			return Activator.CreateInstance(tw.TypeAsTBD, new object[] { clazz, mw });
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static object CookieToField(IntPtr field)
	{
		try
		{
			FieldWrapper fw = FieldWrapper.FromCookie(field);
			TypeWrapper tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.reflect.Field");
			object clazz = NativeCode.java.lang.VMClass.getClassFromWrapper(fw.DeclaringType);
			return Activator.CreateInstance(tw.TypeAsTBD, new object[] { clazz, fw });
		}
		catch
		{
			Debug.Assert(false);
			throw;
		}
	}

	public static void FatalError(string msg)
	{
		JVM.CriticalFailure(msg, null);
	}

	public static object DefineClass(string name, object classLoader, byte[] buf)
	{
		// TODO what should the protection domain be?
		return NativeCode.java.lang.VMClassLoader.defineClass(classLoader, name, buf, 0, buf.Length, null);
	}
}

public interface IJniProvider
{
	int LoadNativeLibrary(string filename);
	Type GetLocalRefStructType();
	// NOTE the signature of the GetJniFuncPtr method is:
	//  IntPtr GetJniFuncPtr(String method, String sig, String clazz)
	// sig & clazz are contain slashed class names
	MethodInfo GetJniFuncPtrMethod();
}
