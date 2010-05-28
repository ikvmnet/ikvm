/*
  Copyright (C) 2002-2010 Jeroen Frijters

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
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using IKVM.Attributes;
using IKVM.Internal;
using IDictionary = System.Collections.IDictionary;
using Interlocked = System.Threading.Interlocked;
using MethodBase = System.Reflection.MethodBase;
using ObjectInputStream = java.io.ObjectInputStream;
using ObjectOutputStream = java.io.ObjectOutputStream;
using ObjectStreamField = java.io.ObjectStreamField;
using StackTraceElement = java.lang.StackTraceElement;
#if !FIRST_PASS
using Throwable = java.lang.Throwable;
#endif

namespace IKVM.Internal
{
	static class ExceptionHelper
	{
		private static readonly Dictionary<string, string> failedTypes = new Dictionary<string, string>();
		private static readonly Key EXCEPTION_DATA_KEY = new Key();
		private static readonly Exception NOT_REMAPPED = new Exception();
		private static readonly bool cleanStackTrace = JVM.SafeGetEnvironmentVariable("IKVM_DISABLE_STACKTRACE_CLEANING") == null;
		private static readonly Type System_Exception = typeof(Exception);
#if !FIRST_PASS
		private static readonly ikvm.@internal.WeakIdentityMap exceptions = new ikvm.@internal.WeakIdentityMap();

		static ExceptionHelper()
		{
			// make sure the exceptions map continues to work during AppDomain finalization
			GC.SuppressFinalize(exceptions);
		}

		[Serializable]
		internal sealed class ExceptionInfoHelper
		{
			[NonSerialized]
			private StackTrace tracePart1;
			[NonSerialized]
			private StackTrace tracePart2;
			private StackTraceElement[] stackTrace;

			internal ExceptionInfoHelper(StackTraceElement[] stackTrace)
			{
				this.stackTrace = stackTrace;
			}

			internal ExceptionInfoHelper(StackTrace tracePart1, StackTrace tracePart2)
			{
				this.tracePart1 = tracePart1;
				this.tracePart2 = tracePart2;
			}

			internal ExceptionInfoHelper(Exception x, bool captureAdditionalStackTrace)
			{
				tracePart1 = new StackTrace(x, true);
				if (captureAdditionalStackTrace)
				{
					tracePart2 = new StackTrace(true);
				}
			}

			[OnSerializing]
			private void OnSerializing(StreamingContext context)
			{
				// make sure the stack trace is computed before serializing
				get_StackTrace(null);
			}

			private static bool IsPrivateScope(MethodBase mb)
			{
				return (mb.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.PrivateScope;
			}

			internal StackTraceElement[] get_StackTrace(Exception t)
			{
				lock (this)
				{
					if (stackTrace == null)
					{
						List<StackTraceElement> list = new List<StackTraceElement>();
						if (tracePart1 != null)
						{
							int skip1 = 0;
							if (cleanStackTrace && t is java.lang.NullPointerException && tracePart1.FrameCount > 0)
							{
								// HACK if a NullPointerException originated inside an instancehelper method,
								// we assume that the reference the method was called on was really the one that was null,
								// so we filter it.
								if (tracePart1.GetFrame(0).GetMethod().Name.StartsWith("instancehelper_") &&
									!GetMethodName(tracePart1.GetFrame(0).GetMethod()).StartsWith("instancehelper_"))
								{
									skip1 = 1;
								}
							}
							Append(list, tracePart1, skip1);
						}
						if (tracePart2 != null && tracePart2.FrameCount > 0)
						{
							int skip = 0;
							if (cleanStackTrace)
							{
								// If fillInStackTrace was called (either directly or from the constructor),
								// filter out fillInStackTrace and the following constructor frames.
								if (tracePart1 == null)
								{
									MethodBase mb = tracePart2.GetFrame(skip).GetMethod();
									if (mb.DeclaringType == typeof(Throwable) && mb.Name.EndsWith("fillInStackTrace", StringComparison.Ordinal))
									{
										while (tracePart2.FrameCount > skip)
										{
											mb = tracePart2.GetFrame(skip).GetMethod();
											if (mb.DeclaringType != typeof(Throwable) || !mb.Name.EndsWith("fillInStackTrace", StringComparison.Ordinal))
											{
												break;
											}
											skip++;
										}
										while (tracePart2.FrameCount > skip)
										{
											mb = tracePart2.GetFrame(skip).GetMethod();
											if (mb.Name != ".ctor" || !mb.DeclaringType.IsInstanceOfType(t))
											{
												break;
											}
											skip++;
										}
									}
								}
								else
								{
									// Skip java.lang.Throwable.__<map> and other mapping methods, because we need to be able to remove the frame
									// that called map (if it is the same as where the exception was caught).
									while (tracePart2.FrameCount > skip && IsHideFromJava(tracePart2.GetFrame(skip).GetMethod()))
									{
										skip++;
									}
									if (tracePart1.FrameCount > 0 &&
										tracePart2.FrameCount > skip &&
										tracePart1.GetFrame(tracePart1.FrameCount - 1).GetMethod() == tracePart2.GetFrame(skip).GetMethod())
									{
										// skip the caller of the map method
										skip++;
									}
								}
							}
							Append(list, tracePart2, skip);
						}
						if (cleanStackTrace && list.Count > 0)
						{
							StackTraceElement elem = list[list.Count - 1];
							if (elem.getClassName() == "java.lang.reflect.Method")
							{
								list.RemoveAt(list.Count - 1);
							}
						}
						tracePart1 = null;
						tracePart2 = null;
						this.stackTrace = list.ToArray();
					}
				}
				return (StackTraceElement[])stackTrace.Clone();
			}

			internal static void Append(List<StackTraceElement> stackTrace, StackTrace st, int skip)
			{
				for (int i = skip; i < st.FrameCount; i++)
				{
					StackFrame frame = st.GetFrame(i);
					MethodBase m = frame.GetMethod();
					if (m == null || m.DeclaringType == null)
					{
						continue;
					}
					Type type = m.DeclaringType;
					if (cleanStackTrace &&
						(typeof(MethodBase).IsAssignableFrom(type)
						|| type == typeof(RuntimeMethodHandle)
						|| (type == typeof(Throwable) && m.Name == "instancehelper_fillInStackTrace")
						|| (m.Name == "ToJava" && typeof(RetargetableJavaException).IsAssignableFrom(type))
						|| IsHideFromJava(m)
						|| IsPrivateScope(m))) // NOTE we assume that privatescope methods are always stubs that we should exclude
					{
						continue;
					}
					int lineNumber = frame.GetFileLineNumber();
					if (lineNumber == 0)
					{
						lineNumber = GetLineNumber(frame);
					}
					string fileName = frame.GetFileName();
					if (fileName != null)
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
					if (fileName == null)
					{
						fileName = GetFileName(frame);
					}
					stackTrace.Add(new StackTraceElement(getClassNameFromType(type), GetMethodName(m), fileName, IsNative(m) ? -2 : lineNumber));
				}
			}
		}
#endif

		[Serializable]
		private sealed class Key : ISerializable
		{
			[Serializable]
			private sealed class Helper : IObjectReference
			{
				[SecurityCritical]
				public Object GetRealObject(StreamingContext context)
				{
					return EXCEPTION_DATA_KEY;
				}
			}

			[SecurityCritical]
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.SetType(typeof(Helper));
			}
		}

		private static bool IsNative(MethodBase m)
		{
			object[] methodFlagAttribs = m.GetCustomAttributes(typeof(ModifiersAttribute), false);
			if(methodFlagAttribs.Length == 1)
			{
				ModifiersAttribute modifiersAttrib = (ModifiersAttribute)methodFlagAttribs[0];
				return (modifiersAttrib.Modifiers & Modifiers.Native) != 0;
			}
			return false;
		}

		private static string GetMethodName(MethodBase mb)
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
			else
			{
				return mb.Name;
			}
		}

		private static bool IsHideFromJava(MethodBase mb)
		{
			return NativeCode.sun.reflect.Reflection.IsHideFromJava(mb) || (mb.DeclaringType == typeof(ikvm.runtime.Util) && mb.Name == "mapException");
		}

		private static string getClassNameFromType(Type type)
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
				typeof(Throwable).GetConstructor(new Type[] { typeof(string) }).Invoke(throwable, new object[] { detailMessage });
			}
			else
			{
				typeof(Throwable).GetConstructor(new Type[] { typeof(string), typeof(Exception) }).Invoke(throwable, new object[] { detailMessage, cause });
			}
#endif
		}

		private static int GetLineNumber(StackFrame frame)
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

		private static string GetFileName(StackFrame frame)
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

		// called from map.xml
		internal static ObjectStreamField[] getPersistentFields()
		{
#if FIRST_PASS
			return null;
#else
			return new ObjectStreamField[] {
				new ObjectStreamField("detailMessage", typeof(global::java.lang.String)),
				new ObjectStreamField("cause", typeof(global::java.lang.Throwable)),
				new ObjectStreamField("stackTrace", typeof(global::java.lang.StackTraceElement[]))
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
			Throwable.instancehelper_printStackTrace(x, java.lang.System.err);
#endif
		}

		internal static void printStackTrace(Exception x, java.io.PrintStream printStream)
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

		internal static void printStackTrace(Exception x, java.io.PrintWriter printWriter)
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
			return java.lang.Object.instancehelper_toString(cause);
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
				return java.lang.Object.instancehelper_getClass(x).getName();
			}
			return java.lang.Object.instancehelper_getClass(x).getName() + ": " + message;
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
				throw new java.lang.IllegalStateException("Can't overwrite cause");
			}
			if (cause == _this)
			{
				throw new java.lang.IllegalArgumentException("Self-causation not permitted");
			}
#endif
		}

		internal static StackTraceElement[] computeStackTrace(Exception x, StackTrace part1, StackTrace part2)
		{
#if FIRST_PASS
			return null;
#else
			ExceptionInfoHelper eih = new ExceptionInfoHelper(part1, part2);
			return eih.get_StackTrace(x);
#endif
		}

		// this method is *only* for .NET exceptions (i.e. types not derived from java.lang.Throwable)
		internal static StackTraceElement[] getStackTrace(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			lock (x)
			{
				ExceptionInfoHelper eih = null;
				IDictionary data = x.Data;
				if (data != null && !data.IsReadOnly)
				{
					lock (data.SyncRoot)
					{
						eih = (ExceptionInfoHelper)data[EXCEPTION_DATA_KEY];
					}
				}
				if (eih == null)
				{
					return new StackTraceElement[0];
				}
				return eih.get_StackTrace(x);
			}
#endif
		}

		internal static StackTraceElement[] checkStackTrace(StackTraceElement[] original)
		{
#if FIRST_PASS
			return null;
#else
			StackTraceElement[] copy = (StackTraceElement[])original.Clone();
			for (int i = 0; i < copy.Length; i++)
			{
				if (copy[i] == null)
				{
					throw new java.lang.NullPointerException();
				}
			}
			return copy;
#endif
		}

		// this method is *only* for .NET exceptions (i.e. types not derived from java.lang.Throwable)
		internal static void setStackTrace(Exception x, StackTraceElement[] stackTrace)
		{
#if !FIRST_PASS
			ExceptionInfoHelper eih = new ExceptionInfoHelper(checkStackTrace(stackTrace));
			IDictionary data = x.Data;
			if (data != null && !data.IsReadOnly)
			{
				lock (data.SyncRoot)
				{
					data[EXCEPTION_DATA_KEY] = eih;
				}
			}
#endif
		}

		// this method is *only* for .NET exceptions (i.e. types not derived from java.lang.Throwable)
		[HideFromJava]
		internal static void fillInStackTrace(Exception x)
		{
#if !FIRST_PASS
			lock (x)
			{
				ExceptionInfoHelper eih = new ExceptionInfoHelper(null, new StackTrace(true));
				IDictionary data = x.Data;
				if (data != null && !data.IsReadOnly)
				{
					lock (data.SyncRoot)
					{
						data[EXCEPTION_DATA_KEY] = eih;
					}
				}
			}
#endif
		}

		// this method is *only* for .NET exceptions (i.e. types not derived from java.lang.Throwable)
		internal static void FixateException(Exception x)
		{
#if !FIRST_PASS
			exceptions.put(x, NOT_REMAPPED);
#endif
		}

		internal static Exception UnmapException(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			if (x is Throwable)
			{
				Exception org = Interlocked.Exchange(ref ((Throwable)x).original, null);
				if (org != null)
				{
					exceptions.put(org, x);
					x = org;
				}
			}
			return x;
#endif
		}

		[HideFromJava]
		internal static Exception MapExceptionFast(Exception x, bool remap)
		{
#if FIRST_PASS
			return null;
#else
			return MapException(x, null, remap);
#endif
		}

		[HideFromJava]
		private static Exception MapTypeInitializeException(TypeInitializationException t, Type handler)
		{
#if FIRST_PASS
			return null;
#else
			bool wrapped = false;
			Exception r = MapExceptionFast(t.InnerException, true);
			if (!(r is java.lang.Error))
			{
				r = new java.lang.ExceptionInInitializerError(r);
				wrapped = true;
			}
			string type = t.TypeName;
			if (failedTypes.ContainsKey(type))
			{
				r = new java.lang.NoClassDefFoundError(type).initCause(r);
				wrapped = true;
			}
			if (handler != null && !handler.IsInstanceOfType(r))
			{
				return null;
			}
			failedTypes[type] = type;
			if (wrapped)
			{
				// transplant the stack trace
				((Throwable)r).setStackTrace(new ExceptionInfoHelper(t, true).get_StackTrace(t));
			}
			return r;
#endif
		}

		private static bool isInstanceOfType(Exception t, Type type, bool remap)
		{
#if FIRST_PASS
			return false;
#else
			if (!remap && type == typeof(Exception))
			{
				return !(t is Throwable);
			}
			return type.IsInstanceOfType(t);
#endif
		}

		[HideFromJava]
		internal static Exception MapException(Exception x, Type handler, bool remap)
		{
#if FIRST_PASS
			return null;
#else
			Exception org = x;
			bool nonJavaException = !(x is Throwable);
			if (nonJavaException && remap)
			{
				if (x is TypeInitializationException)
				{
					return MapTypeInitializeException((TypeInitializationException)x, handler);
				}
				object obj = exceptions.get(x);
				Exception remapped = (Exception)obj;
				if (remapped == null)
				{
					remapped = Throwable.__mapImpl(x);
					if (remapped == x)
					{
						exceptions.put(x, NOT_REMAPPED);
					}
					else
					{
						exceptions.put(x, remapped);
						x = remapped;
					}
				}
				else if (remapped != NOT_REMAPPED)
				{
					x = remapped;
				}
			}

			if (handler == null || isInstanceOfType(x, handler, remap))
			{
				if (!(x is Throwable))
				{
					IDictionary data = x.Data;
					if (data != null && !data.IsReadOnly)
					{
						lock (data.SyncRoot)
						{
							if (!data.Contains(EXCEPTION_DATA_KEY))
							{
								data.Add(EXCEPTION_DATA_KEY, new ExceptionInfoHelper(x, true));
							}
						}
					}
				}
				else
				{
					if (needStackTraceInfo((Throwable)x))
					{
						StackTrace tracePart1 = new StackTrace(org, true);
						StackTrace tracePart2 = new StackTrace(true);
						setStackTraceInfo((Throwable)x, tracePart1, tracePart2);
					}
				}

				if (nonJavaException && !remap)
				{
					exceptions.put(x, NOT_REMAPPED);
				}

				if (x != org)
				{
					((Throwable)x).original = org;
					exceptions.remove(org);
				}
				return x;
			}
			return null;
#endif
		}

#if !FIRST_PASS
	    private static bool needStackTraceInfo(Throwable t)
		{
			return t.tracePart1 == null && t.tracePart2 == null && t.stackTrace == null;
		}

		private static void setStackTraceInfo(Throwable t, StackTrace part1, StackTrace part2)
		{
			t.tracePart1 = part1;
			t.tracePart2 = part2;
		}
#endif
	}
}
