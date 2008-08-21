/*
  Copyright (C) 2002, 2004, 2005, 2006, 2007 Jeroen Frijters

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
using IKVM.Attributes;
using IKVM.Internal;

namespace IKVM.NativeCode.java.lang
{
	static class ExceptionHelper
	{
		public static Exception MapExceptionImpl(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			return global::java.lang.Throwable.__mapImpl(x);
#endif
		}

		public static string SafeGetEnvironmentVariable(string name)
		{
			try
			{
				return JVM.SafeGetEnvironmentVariable(name);
			}
			catch(MissingMethodException)
			{
				return null;
			}
		}

		public static Exception getInnerException(Exception t)
		{
			return t.InnerException;
		}

		public static string getMessageFromCliException(Exception t)
		{
			return t.Message;
		}

		public static bool IsNative(MethodBase m)
		{
			object[] methodFlagAttribs = m.GetCustomAttributes(typeof(ModifiersAttribute), false);
			if(methodFlagAttribs.Length == 1)
			{
				ModifiersAttribute modifiersAttrib = (ModifiersAttribute)methodFlagAttribs[0];
				return (modifiersAttrib.Modifiers & Modifiers.Native) != 0;
			}
			return false;
		}

		public static string GetMethodName(MethodBase mb)
		{
			object[] attr = mb.GetCustomAttributes(typeof(NameSigAttribute), false);
			if(attr.Length == 1)
			{
				return ((NameSigAttribute)attr[0]).Name;
			}
			else if(mb.Name == ".ctor")
			{
				return "<init>";
			}
			else if(mb.Name == ".cctor")
			{
				return "<clinit>";
			}
			else if(mb.Name == "ToJava" && typeof(RetargetableJavaException).IsAssignableFrom(mb.DeclaringType))
			{
				// hide this method from the stack trace
				return "__<ToJava>";
			}
			else if(mb.Name == "mapException" && mb.DeclaringType.FullName == "ikvm.runtime.Util")
			{
				// hide this method from the stack trace
				return "__<mapException>";
			}
			else
			{
				return mb.Name;
			}
		}

		public static bool IsHideFromJava(MethodBase mb)
		{
			return NativeCode.sun.reflect.Reflection.IsHideFromJava(mb);
		}

		public static string getClassNameFromType(Type type)
		{
			if(ClassLoaderWrapper.IsRemappedType(type))
			{
				return DotNetTypeWrapper.GetName(type);
			}
			TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
			if(tw != null)
			{
				if(tw.IsPrimitive)
				{
					return DotNetTypeWrapper.GetName(type);
				}
				return tw.Name;
			}
			return type.FullName;
		}

		public static void initThrowable(object throwable, object detailMessage, object cause)
		{
#if !FIRST_PASS
			if(cause == throwable)
			{
				typeof(global::java.lang.Throwable).GetConstructor(new Type[] { typeof(string) }).Invoke(throwable, new object[] { detailMessage });
			}
			else
			{
				typeof(global::java.lang.Throwable).GetConstructor(new Type[] { typeof(string), typeof(Exception) }).Invoke(throwable, new object[] { detailMessage, cause });
			}
#endif
		}

#if !COMPACT_FRAMEWORK
		public static int GetLineNumber(StackFrame frame)
		{
			int ilOffset = frame.GetILOffset();
			if(ilOffset != StackFrame.OFFSET_UNKNOWN)
			{
				MethodBase mb = frame.GetMethod();
				if(mb != null)
				{
					TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(mb.DeclaringType);
					if(tw != null)
					{
						return tw.GetSourceLineNumber(mb, ilOffset);
					}
				}
			}
			return -1;
		}

		public static string GetFileName(StackFrame frame)
		{
			MethodBase mb = frame.GetMethod();
			if(mb != null)
			{
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(mb.DeclaringType);
				if(tw != null)
				{
					return tw.GetSourceFileName();
				}
			}
			return null;
		}
#endif

		public static Type getTypeFromObject(object o)
		{
			return o.GetType();
		}
	}
}
