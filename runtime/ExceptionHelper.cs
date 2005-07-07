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
using IKVM.Internal;
using NetSystem = System;

namespace IKVM.NativeCode.java.lang
{
	public class ExceptionHelper
	{
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

		public static string GetMethodName(MethodBase mb)
		{
			if(mb.IsDefined(typeof(NameSigAttribute), false))
			{
				object[] attr = mb.GetCustomAttributes(typeof(NameSigAttribute), false);
				if(attr.Length == 1)
				{
					return ((NameSigAttribute)attr[0]).Name;
				}
				return mb.Name;
			}
			else if(mb.Name == ".ctor")
			{
				return "<init>";
			}
			else if(mb.Name == ".cctor")
			{
				return "<clinit>";
			}
			else
			{
				return mb.Name;
			}
		}

		public static bool IsHideFromJava(MethodBase mb)
		{
			return mb.IsDefined(typeof(HideFromJavaAttribute), false);
		}

		public static string getClassNameFromType(Type type)
		{
			if(ClassLoaderWrapper.IsRemappedType(type))
			{
				return DotNetTypeWrapper.GetName(type);
			}
			return TypeWrapper.GetNameFromType(type);
		}

		public static void initThrowable(object throwable, object detailMessage, object cause)
		{
			if(cause == throwable)
			{
				MethodWrapper mw = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
				mw.Invoke(throwable, new object[] { detailMessage }, true);
			}
			else
			{
				MethodWrapper mw = CoreClasses.java.lang.Throwable.Wrapper.GetMethodWrapper("<init>", "(Ljava.lang.String;Ljava.lang.Throwable;)V", false);
				mw.Invoke(throwable, new object[] { detailMessage, cause }, true);
			}
		}

		public static int GetLineNumber(StackFrame frame)
		{
			int ilOffset = frame.GetILOffset();
			if(ilOffset != StackFrame.OFFSET_UNKNOWN)
			{
				MethodBase mb = frame.GetMethod();
				if(mb != null)
				{
					object[] attr = mb.GetCustomAttributes(typeof(LineNumberTableAttribute), false);
					if(attr.Length == 1)
					{
						return ((LineNumberTableAttribute)attr[0]).GetLineNumber(ilOffset);
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
				object[] attr = mb.DeclaringType.GetCustomAttributes(typeof(SourceFileAttribute), false);
				if(attr.Length == 1)
				{
					return ((SourceFileAttribute)attr[0]).SourceFile;
				}
				if(mb.DeclaringType.Module.IsDefined(typeof(SourceFileAttribute), false))
				{
					return mb.DeclaringType.Name + ".java";
				}
			}
			return null;
		}

		public static Type getTypeFromObject(object o)
		{
			return o.GetType();
		}
	}
}
