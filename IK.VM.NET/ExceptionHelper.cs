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
using java.lang;
using OpenSystem.Java;
using ClassHelper = NativeCode.java.lang.VMClass;

public class ExceptionHelper
{
	// the contents of the NULL_STRING should be empty (because when the exception propagates to other .NET code
	// it will return that text as the Message property), but it *must* be a copy, because we need to be
	// able to distinguish it from a user specified blank string
	private static readonly string NULL_STRING = string.Copy("");

	private class ExceptionInfoHelper
	{
		private static readonly Exception CAUSE_NOT_SET = new Exception();
		private StackTrace tracePart1;
		private StackTrace tracePart2;
		private ArrayList stackTrace;
		private Exception cause;

		[StackTraceInfo(Hidden = true)]
		internal ExceptionInfoHelper(Exception x)
		{
			Profiler.Enter("new ExceptionInfoHelper");
			try
			{
				tracePart1 = new StackTrace(x, true);
				tracePart2 = new StackTrace(true);
				cause = x.InnerException;
				if(cause == null)
				{
					cause = CAUSE_NOT_SET;
				}
			}
			finally
			{
				Profiler.Leave("new ExceptionInfoHelper");
			}	
		}

		internal Exception Cause
		{
			get
			{
				return cause == CAUSE_NOT_SET ? null : cause;
			}
			set
			{
				if(cause == CAUSE_NOT_SET)
				{
					cause = value;
				}
				else
				{
					throw JavaException.IllegalStateException("Throwable cause already initialized");
				}
			}
		}

		internal void ResetStackTrace()
		{
			stackTrace = null;
			tracePart1 = new StackTrace(true);
			tracePart2 = null;
		}

		private static bool IsPrivateScope(MethodBase mb)
		{
			// HACK shouldn't there be a better way to determine whether a method is privatescope?
			return !mb.IsPrivate && !mb.IsFamily && !mb.IsFamilyAndAssembly && !mb.IsFamilyOrAssembly && !mb.IsPublic;
		}

		private void Append(StackTrace st)
		{
			Profiler.Enter("ExceptionInfoHelper.Append");
			try
			{
				if(st.FrameCount > 0)
				{
					int baseSize = stackTrace.Count;
					for(int i = 0; i < st.FrameCount; i++)
					{
						StackFrame frame = st.GetFrame(i);
						MethodBase m = frame.GetMethod();
						// TODO I may need more safety checks like these
						if(m == null || m.DeclaringType == null || m.ReflectedType == null)
						{
							continue;
						}
						if(JVM.CleanStackTraces && (m.DeclaringType == typeof(System.Runtime.CompilerServices.RuntimeHelpers)
							|| m.DeclaringType.IsSubclassOf(typeof(System.Reflection.MethodInfo))
							|| IsPrivateScope(m))) // NOTE we assume that privatescope methods are always stubs that we should exclude
						{
							continue;
						}
						string methodName = frame.GetMethod().Name;
						if(methodName == ".ctor")
						{
							methodName = "<init>";
						}
						else if(methodName == ".cctor")
						{
							methodName = "<clinit>";
						}
						int lineNumber = frame.GetFileLineNumber();
						if(lineNumber == 0)
						{
							lineNumber = -1;
						}
						string fileName = frame.GetFileName();
						if(fileName != null)
						{
							try
							{
								fileName = new System.IO.FileInfo(fileName).Name;
							}
							catch
							{
								// Mono returns "<unknown>" for frame.GetFileName() and the FileInfo constructor
								// doesn't like that
								fileName = null;
							}
						}
						string className = ClassHelper.getName(frame.GetMethod().ReflectedType);
						bool native = false;
						if(m.IsDefined(typeof(ModifiersAttribute), false))
						{
							object[] methodFlagAttribs = m.GetCustomAttributes(typeof(ModifiersAttribute), false);
							if(methodFlagAttribs.Length == 1)
							{
								ModifiersAttribute modifiersAttrib = (ModifiersAttribute)methodFlagAttribs[0];
								if((modifiersAttrib.Modifiers & Modifiers.Native) != 0)
								{
									native = true;
								}
							}
						}
						if(JVM.CleanStackTraces)
						{
							if(AttributeHelper.IsHideFromReflection(m))
							{
								continue;
							}
							object[] attribs = m.DeclaringType.GetCustomAttributes(typeof(StackTraceInfoAttribute), false);
							if(attribs.Length == 1)
							{
								StackTraceInfoAttribute sta = (StackTraceInfoAttribute)attribs[0];
								if(sta.EatFrames > 0)
								{
									stackTrace.RemoveRange(stackTrace.Count - sta.EatFrames, sta.EatFrames);
								}
								if(sta.Hidden)
								{
									continue;
								}
								if(sta.Truncate)
								{
									stackTrace.RemoveRange(baseSize, stackTrace.Count - baseSize);
									continue;
								}
								if(sta.Class != null)
								{
									className = sta.Class;
								}
							}
							attribs = m.GetCustomAttributes(typeof(StackTraceInfoAttribute), false);
							if(attribs.Length == 1)
							{
								StackTraceInfoAttribute sta = (StackTraceInfoAttribute)attribs[0];
								if(sta.EatFrames > 0)
								{
									int eat = Math.Min(stackTrace.Count, sta.EatFrames);
									stackTrace.RemoveRange(stackTrace.Count - eat, eat);
								}
								if(sta.Hidden)
								{
									continue;
								}
								if(sta.Truncate)
								{
									stackTrace.RemoveRange(baseSize, stackTrace.Count - baseSize);
									continue;
								}
								if(sta.Class != null)
								{
									className = sta.Class;
								}
							}
						}
						stackTrace.Add(new StackTraceElement(fileName, lineNumber, className, methodName, native));
					}
					if(JVM.CleanStackTraces)
					{
						int chop = 0;
						for(int i = stackTrace.Count - 1; i >= 0; i--)
						{
							StackTraceElement ste = (StackTraceElement)stackTrace[i];
							if(ste.getClassName() == "System.Reflection.MethodBase")
							{
								// skip method invocation by reflection, if it is at the top of the stack
								chop++;
							}
							else
							{
								break;
							}
						}
						stackTrace.RemoveRange(stackTrace.Count - chop, chop);
					}
				}
			}
			finally
			{
				Profiler.Leave("ExceptionInfoHelper.Append");
			}
		}

		internal StackTraceElement[] StackTrace
		{
			get
			{
				if(stackTrace == null)
				{
					stackTrace = new ArrayList();
					Append(tracePart1);
					if(tracePart2 != null)
					{
						// don't have a clue why this was here, but it doesn't work for jni.class
						//bool chopFirst = stackTrace.Count != 0;
						Append(tracePart2);
						//if(chopFirst && stackTrace.Count > 0 && JVM.CleanStackTraces)
						//{
						//	stackTrace.RemoveAt(0);
						//}
					}
					tracePart1 = null;
					tracePart2 = null;
				}
				return (StackTraceElement[])stackTrace.ToArray(typeof(StackTraceElement));
			}
			set
			{
				stackTrace = new ArrayList(value);
				tracePart1 = null;
				tracePart2 = null;
			}
		}
	}

	// TODO this should be an "identity" hashtable instead of "equality"
	private static WeakHashtable exceptions = new WeakHashtable();

	public static void printStackTrace(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		Type type = ClassLoaderWrapper.GetType("java.lang.System");
		object err = type.GetProperty("err").GetValue(null, null);
		printStackTrace(x, err);
	}

	public static void printStackTrace(Exception x, object printStreamOrWriter)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		StringBuilder sb = new StringBuilder();
		sb.Append(toString_Virtual(x)).Append(Environment.NewLine);
		StackTraceElement[] stack = getStackTrace_Virtual(x);
		for(int i = 0; i < stack .Length; i++)
		{
			sb.Append("\tat ").Append(stack[i]).Append(Environment.NewLine);
		}
		Exception cause = getCause_Virtual(x);
		while(cause != null)
		{
			sb.Append("Caused by: ").Append(toString_Virtual(cause)).Append(Environment.NewLine);

			// Cause stacktrace
			StackTraceElement[] parentStack = stack;
			stack = getStackTrace_Virtual(cause);
			bool equal = false; // Is rest of stack equal to parent frame?
			for(int i = 0; i < stack.Length && !equal; i++)
			{
				// Check if we already printed the rest of the stack
				// since it was the tail of the parent stack
				int remaining = stack.Length - i;
				int element = i;
				int parentElement = parentStack.Length - remaining;
				equal = parentElement >= 0
					&& parentElement < parentStack.Length; // be optimistic
				while(equal && element < stack.Length)
				{
					if(stack[element].Equals(parentStack[parentElement]))
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
				if(!equal)
				{
					sb.Append("\tat ").Append(stack[i]).Append(Environment.NewLine);
				}
				else
				{
					sb.Append("\t... ").Append(remaining).Append(" more").Append(Environment.NewLine);
					break; // from stack printing for loop
				}
			}
			cause = getCause_Virtual(cause);
		}
		// NOTE since we use reflection to lookup the print method each time, we can use this one method for both
		// the printStackTrace(..., PrintStream) & printStackTrace(..., PrintWriter) versions
		MethodInfo write = printStreamOrWriter.GetType().GetMethod("print", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, new Type[] { typeof(string) }, null);
		write.Invoke(printStreamOrWriter, new object[] { sb.ToString() });
	}

	public static Exception initCause(Exception x, Exception cause)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		if(cause == x)
		{
			throw JavaException.IllegalArgumentException("Cause cannot be self");
		}
		ExceptionInfoHelper eih = (ExceptionInfoHelper)exceptions[x];
		if(eih == null)
		{
			eih = new ExceptionInfoHelper(x);
			exceptions[x] = eih;
		}
		eih.Cause = cause;
		return x;
	}

	public static Exception getCause(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		ExceptionInfoHelper eih = (ExceptionInfoHelper)exceptions[x];
		if(eih == null)
		{
			return x.InnerException;
		}
		return eih.Cause;
	}

	public static StackTraceElement[] getStackTrace(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		ExceptionInfoHelper ei = (ExceptionInfoHelper)exceptions[x];
		if(ei == null)
		{
			return new StackTraceElement[0];
		}
		return ei.StackTrace;
	}

	public static void setStackTrace(Exception x, StackTraceElement[] stackTrace)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		for(int i = 0; i < stackTrace.Length; i++)
		{
			if(stackTrace[i] == null)
			{
				throw new NullReferenceException();
			}
		}
		ExceptionInfoHelper ei = (ExceptionInfoHelper)exceptions[x];
		if(ei == null)
		{
			ei = new ExceptionInfoHelper(x);
			exceptions[x] = ei;
		}
		ei.StackTrace = stackTrace;
	}

	public static string NullString
	{
		get
		{
			return NULL_STRING;
		}
	}

	public static string FilterMessage(string message)
	{
		if(message == null)
		{
			message = NULL_STRING;
		}
		return message;
	}

	public static string GetMessageFromCause(Exception cause)
	{
		if(cause == null)
		{
			return NULL_STRING;
		}
		return toString_Virtual(cause);
	}

	public static string getMessage(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		string message = x.Message;
		if(message == NULL_STRING)
		{
			message = null;
		}
		return message;
	}

	public static string getLocalizedMessage(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		return getMessage_Virtual(x);
	}

	[StackTraceInfo(Hidden = true)]
	public static Exception fillInStackTrace(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		ExceptionInfoHelper eih = (ExceptionInfoHelper)exceptions[x];
		if(eih == null)
		{
			eih = new ExceptionInfoHelper(x);
			exceptions[x] = eih;
		}
		else
		{
			eih.ResetStackTrace();
		}
		return x;
	}

	[StackTraceInfo(Hidden = true)]
	public static Exception MapExceptionFast(Exception t)
	{
		if(exceptions.ContainsKey(t))
		{
			return t;
		}
		return MapException(t, typeof(Exception));
	}

	[StackTraceInfo(Truncate = true)]
	public static Exception MapException(Exception t, Type handler)
	{
		Profiler.Enter("MapException");
		try
		{
			//Console.WriteLine("MapException: {0}, {1}", t, handler);
			//Console.WriteLine(new StackTrace(t));
			Exception org = t;
			Type type = t.GetType();
			// TODO don't remap if the exception already has associated ExceptionInfoHelper object (this means
			// that the .NET exception was thrown from Java code, explicitly).
			if(type == typeof(NullReferenceException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.NullPointerException"));
			}
			// HACK many of the String and Object methods throw ArgumentNullException where Java throws an NPE
			else if(type == typeof(ArgumentNullException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.NullPointerException"));
			}
			else if(type == typeof(IndexOutOfRangeException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.ArrayIndexOutOfBoundsException"));
			}
			// HACK for String methods, we remap ArgumentOutOfRangeException to StringIndexOutOfBoundsException
			else if(type == typeof(ArgumentOutOfRangeException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.StringIndexOutOfBoundsException"));
			}
			else if(type == typeof(InvalidCastException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.ClassCastException"));
			}
			else if(type == typeof(TypeInitializationException))
			{
				t = (Exception)MapExceptionFast(t.InnerException);
				if(!ClassLoaderWrapper.GetType("java.lang.Error").IsInstanceOfType(t))
				{
					ConstructorInfo constructor = ClassLoaderWrapper.GetType("java.lang.ExceptionInInitializerError").GetConstructor(new Type[] { typeof(Exception) });
					t = (Exception)constructor.Invoke(new object[] { t });
				}
			}
			else if(type == typeof(System.Threading.SynchronizationLockException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.IllegalMonitorStateException"));
			}
			else if(type == typeof(System.Threading.ThreadInterruptedException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.InterruptedException"));
			}
			else if(type == typeof(OutOfMemoryException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.OutOfMemoryError"));
			}
			else if(type == typeof(DivideByZeroException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.ArithmeticException"), new object[] { "/ by zero" });
			}
			else if(type == typeof(ArrayTypeMismatchException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.ArrayStoreException"));
			}
			else if(type == typeof(StackOverflowException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.StackOverflowError"));
			}
			else if(type == typeof(System.Security.VerificationException))
			{
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.VerifyError"));
			}
			else if(type == typeof(System.Threading.ThreadAbortException))
			{
				System.Threading.ThreadAbortException abort = (System.Threading.ThreadAbortException)t;
				if(abort.ExceptionState is Exception)
				{
					t = (Exception)abort.ExceptionState;
				}
				else
				{
					t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.ThreadDeath"));
				}
				System.Threading.Thread.ResetAbort();
			}
			else if(type == typeof(OverflowException))
			{
				// TODO make sure the originating method was from an IK.VM.NET generated assembly, because if it was
				// generated by non-Java code, this remapping is obviously bogus.
				t = (Exception)Activator.CreateInstance(ClassLoaderWrapper.GetType("java.lang.NegativeArraySizeException"));
			}
			else if(type.FullName.StartsWith("System.") && type != typeof(TargetInvocationException))
			{
				if(handler != typeof(Exception) && handler.FullName.StartsWith("System."))
				{
					// this is Java code explicitly handling a .NET exception (e.g. System.IO.IOException in java.io.FileDescriptor)
					// so we don't need to print this
				}
				else
				{
					// TODO this is just for debugging
					Console.WriteLine("caught: {0}, handler: {1}", t.GetType().FullName, handler.FullName);
					Console.WriteLine(t);
				}
			}
			if(!exceptions.ContainsKey(t))
			{
				exceptions.Add(t, new ExceptionInfoHelper(org));
				Exception inner = org.InnerException;
				if(inner != null && !exceptions.ContainsKey(inner))
				{
					exceptions.Add(inner, new ExceptionInfoHelper(inner));
				}
			}
			return handler.IsInstanceOfType(t) ? t : null;
		}
		finally
		{
			Profiler.Leave("MapException");
		}
	}

	public static string toString(Exception x)
	{
		if(x == null)
		{
			throw new NullReferenceException();
		}
		string message = getLocalizedMessage_Virtual(x);
		if(message == null)
		{
			return ClassHelper.getName(x.GetType());
		}
		return ClassHelper.getName(x.GetType()) + ": " + message;
	}

	// below are some helper methods to support calling virtual methods on Throwable
	private static readonly object[] noargs = new object[0];
	private static TypeWrapper throwableWrapper;
	private static MethodWrapper toString_methodWrapper;
	private static MethodWrapper getMessage_methodWrapper;
	private static MethodWrapper getStackTrace_methodWrapper;
	private static MethodWrapper getCause_methodWrapper;
	private static MethodWrapper getLocalizedMessage_methodWrapper;

	private static MethodWrapper GetMethod(string name, string sig)
	{
		if(throwableWrapper == null)
		{
			throwableWrapper = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Throwable");
		}
		return throwableWrapper.GetMethodWrapper(new MethodDescriptor(ClassLoaderWrapper.GetBootstrapClassLoader(), name, sig), true);
	}

	private static string toString_Virtual(Exception x)
	{
		if(toString_methodWrapper == null)
		{
			toString_methodWrapper = GetMethod("toString", "()Ljava.lang.String;");
		}
		return (string)toString_methodWrapper.Invoke(x, noargs, false);
	}

	private static string getMessage_Virtual(Exception x)
	{
		if(getMessage_methodWrapper == null)
		{
			getMessage_methodWrapper = GetMethod("getMessage", "()Ljava.lang.String;");
		}
		return (string)getMessage_methodWrapper.Invoke(x, noargs, false);
	}

	private static StackTraceElement[] getStackTrace_Virtual(Exception x)
	{
		if(getStackTrace_methodWrapper == null)
		{
			getStackTrace_methodWrapper = GetMethod("getStackTrace", "()[Ljava.lang.StackTraceElement;");
		}
		return (StackTraceElement[])getStackTrace_methodWrapper.Invoke(x, noargs, false);
	}

	private static Exception getCause_Virtual(Exception x)
	{
		if(getCause_methodWrapper == null)
		{
			getCause_methodWrapper = GetMethod("getCause", "()Ljava.lang.Throwable;");
		}
		return (Exception)getCause_methodWrapper.Invoke(x, noargs, false);
	}

	private static string getLocalizedMessage_Virtual(Exception x)
	{
		if(getLocalizedMessage_methodWrapper == null)
		{
			getLocalizedMessage_methodWrapper = GetMethod("getLocalizedMessage", "()Ljava.lang.String;");
		}
		return (string)getLocalizedMessage_methodWrapper.Invoke(x, noargs, false);
	}

	[StackTraceInfo(Hidden = true)]
	public static void ThrowHack(Exception x)
	{
		throw x;
	}
}
