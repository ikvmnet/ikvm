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
using System.Diagnostics;
using System.Text;
using System.Collections;
using IKVM.Attributes;
using NetSystem = System;

namespace IKVM.NativeCode.java.lang
{
	public class ExceptionHelper
	{
		public static bool IsNative(MethodBase m)
		{
			if(m.IsDefined(typeof(ModifiersAttribute), false))
			{
				object[] methodFlagAttribs = m.GetCustomAttributes(typeof(ModifiersAttribute), false);
				if(methodFlagAttribs.Length == 1)
				{
					ModifiersAttribute modifiersAttrib = (ModifiersAttribute)methodFlagAttribs[0];
					if((modifiersAttrib.Modifiers & Modifiers.Native) != 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string getClassNameFromType(Type type)
		{
			return TypeWrapper.GetNameFromType(type);
		}

		public static void initThrowable(object throwable, object detailMessage, object cause)
		{
			if(cause == throwable)
			{
				MethodWrapper mw = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper(new MethodDescriptor("<init>", "(Ljava.lang.String;)V"), false);
				mw.Invoke(throwable, new object[] { detailMessage }, true);
			}
			else
			{
				MethodWrapper mw = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper(new MethodDescriptor("<init>", "(Ljava.lang.String;Ljava.lang.Throwable;)V"), false);
				mw.Invoke(throwable, new object[] { detailMessage, cause }, true);
			}
		}
	}
}

class ExceptionHelper
{
	static MethodInfo mapExceptionFastMethod;
	static MethodInfo printStackTraceMethod;

	static ExceptionHelper()
	{
		TypeWrapper tw = ClassLoaderWrapper.LoadClassCritical("java.lang.ExceptionHelper");
		tw.Finish();
		Type exceptionHelper = tw.TypeAsTBD;
		mapExceptionFastMethod = exceptionHelper.GetMethod("MapExceptionFast");
		printStackTraceMethod = exceptionHelper.GetMethod("printStackTrace", new Type[] { typeof(Exception) });
	}

	internal static void printStackTrace(Exception x)
	{
		try
		{
			printStackTraceMethod.Invoke(null, new object[] { x });
		}
		catch(TargetInvocationException t)
		{
			throw t.InnerException;
		}
	}

	// HACK this is used in ClassFile.cs and MemberWrapper.cs
	internal static Exception MapExceptionFast(Exception x)
	{
		try
		{
			return (Exception)mapExceptionFastMethod.Invoke(null, new object[] { x });
		}
		catch(TargetInvocationException t)
		{
			throw t.InnerException;
		}
	}
}
