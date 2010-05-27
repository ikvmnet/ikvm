/*
  Copyright (C) 2002, 2004-2007, 2010 Jeroen Frijters

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
using ObjectInputStream = java.io.ObjectInputStream;
using ObjectOutputStream = java.io.ObjectOutputStream;
using StackTraceElement = java.lang.StackTraceElement;
#if !FIRST_PASS
using Throwable = java.lang.Throwable;
using System.Collections.Generic;
#endif

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
			return JVM.SafeGetEnvironmentVariable(name);
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

		private static void initThrowable(object throwable, object detailMessage, object cause)
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

		public static Type getTypeFromObject(object o)
		{
			return o.GetType();
		}

		// called from map.xml
		internal static global::java.io.ObjectStreamField[] getPersistentFields()
		{
#if FIRST_PASS
			return null;
#else
			return new global::java.io.ObjectStreamField[] {
				new global::java.io.ObjectStreamField("detailMessage", typeof(global::java.lang.String)),
				new global::java.io.ObjectStreamField("cause", typeof(global::java.lang.Throwable)),
				new global::java.io.ObjectStreamField("stackTrace", typeof(global::java.lang.StackTraceElement[]))
			};
#endif
		}

		internal static void writeObject(Exception x, ObjectOutputStream s)
		{
#if !FIRST_PASS
			lock (x)
			{
				ObjectOutputStream.PutField fields = s.putFields();
				fields.put("detailMessage", Throwable.instancehelper_getMessage(x));
				Exception cause = Throwable.instancehelper_getCause(x);
				if (cause == null && x is Throwable)
				{
					cause = ((Throwable)x).cause;
				}
				fields.put("cause", cause);
				fields.put("stackTrace", Throwable.instancehelper_getStackTrace(x));
				s.writeFields();
			}
#endif
		}

		internal static void readObject(Exception x, ObjectInputStream s)
		{
#if !FIRST_PASS
			ObjectInputStream.GetField fields = s.readFields();
			initThrowable(x, fields.get("detailMessage", null), fields.get("cause", null));
			StackTraceElement[] stackTrace = (StackTraceElement[])fields.get("stackTrace", null);
			Throwable.instancehelper_setStackTrace(x, stackTrace == null ? new StackTraceElement[0] : stackTrace);
#endif
		}

		internal static void printStackTrace(Exception x)
		{
#if !FIRST_PASS
			Throwable.instancehelper_printStackTrace(x, global::java.lang.System.err);
#endif
		}

		internal static void printStackTrace(Exception x, global::java.io.PrintStream printStream)
		{
#if !FIRST_PASS
			lock (printStream)
			{
				foreach (string line in BuildStackTrace(x))
				{
					printStream.println(line);
				}
			}
#endif
		}

		internal static void printStackTrace(Exception x, global::java.io.PrintWriter printWriter)
		{
#if !FIRST_PASS
			lock (printWriter)
			{
				foreach (string line in BuildStackTrace(x))
				{
					printWriter.println(line);
				}
			}
#endif
		}

#if !FIRST_PASS
		private static List<String> BuildStackTrace(Exception x)
		{
			List<String> list = new List<String>();
			list.Add(x.ToString());
			StackTraceElement[] stack = Throwable.instancehelper_getStackTrace(x);
			for (int i = 0; i < stack.Length; i++)
			{
				list.Add("\tat " + stack[i]);
			}
			Exception cause = Throwable.instancehelper_getCause(x);
			while (cause != null)
			{
				list.Add("Caused by: " + cause);

				// Cause stacktrace
				StackTraceElement[] parentStack = stack;
				stack = Throwable.instancehelper_getStackTrace(cause);
				bool equal = false; // Is rest of stack equal to parent frame?
				for (int i = 0; i < stack.Length && !equal; i++)
				{
					// Check if we already printed the rest of the stack
					// since it was the tail of the parent stack
					int remaining = stack.Length - i;
					int element = i;
					int parentElement = parentStack.Length - remaining;
					equal = parentElement >= 0 && parentElement < parentStack.Length;
					while (equal && element < stack.Length)
					{
						if (stack[element].equals(parentStack[parentElement]))
						{
							element++;
							parentElement++;
						}
						else
						{
							equal = false;
						}
					}
					// Print stacktrace element or indicate the rest is equal 
					if (!equal)
					{
						list.Add("\tat " + stack[i]);
					}
					else
					{
						list.Add("\t... " + remaining + " more");
						break; // from stack printing for loop
					}
				}
				cause = Throwable.instancehelper_getCause(cause);
			}
			return list;
		}
#endif

		internal static string FilterMessage(string message)
		{
			return message ?? "";
		}

		internal static string GetMessageFromCause(Exception cause)
		{
#if FIRST_PASS
			return null;
#else
			if (cause == null)
			{
				return "";
			}
			return global::java.lang.Object.instancehelper_toString(cause);
#endif
		}

		internal static string getLocalizedMessage(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			return Throwable.instancehelper_getMessage(x);
#endif
		}

		internal static string toString(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			string message = Throwable.instancehelper_getLocalizedMessage(x);
			if (message == null)
			{
				return global::java.lang.Object.instancehelper_getClass(x).getName();
			}
			return global::java.lang.Object.instancehelper_getClass(x).getName() + ": " + message;
#endif
		}

		internal static Exception getCause(Exception _this, Exception cause)
		{
			return cause == _this ? null : cause;
		}

		internal static void checkInitCause(Exception _this, Exception _this_cause, Exception cause)
		{
#if !FIRST_PASS
			if (_this_cause != _this)
			{
				throw new global::java.lang.IllegalStateException("Can't overwrite cause");
			}
			if (cause == _this)
			{
				throw new global::java.lang.IllegalArgumentException("Self-causation not permitted");
			}
#endif
		}

		internal static StackTraceElement[] computeStackTrace(Exception x, StackTrace part1, StackTrace part2)
		{
#if FIRST_PASS
			return null;
#else
			global::java.lang.ExceptionHelper.ExceptionInfoHelper eih = new global::java.lang.ExceptionHelper.ExceptionInfoHelper(part1, part2);
			return eih.get_StackTrace(x);
#endif
		}
	}
}
