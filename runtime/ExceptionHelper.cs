/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
		private static readonly Exception[] EMPTY_THROWABLE_ARRAY = new Exception[0];
		private static readonly bool cleanStackTrace = JVM.SafeGetEnvironmentVariable("IKVM_DISABLE_STACKTRACE_CLEANING") == null;
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

			[HideFromJava]
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
							Append(list, tracePart1, skip1, false);
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
										skip++;
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
							Append(list, tracePart2, skip, true);
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

			internal static void Append(List<StackTraceElement> stackTrace, StackTrace st, int skip, bool isLast)
			{
				for (int i = skip; i < st.FrameCount; i++)
				{
					StackFrame frame = st.GetFrame(i);
					MethodBase m = frame.GetMethod();
					if (m == null)
					{
						continue;
					}
					Type type = m.DeclaringType;
					if (cleanStackTrace &&
						(type == null
						|| typeof(MethodBase).IsAssignableFrom(type)
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
				if (cleanStackTrace && isLast)
				{
					while (stackTrace.Count > 0 && stackTrace[stackTrace.Count - 1].getClassName().StartsWith("cli.System.Threading.", StringComparison.Ordinal))
					{
						stackTrace.RemoveAt(stackTrace.Count - 1);
					}
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
			else if(mb.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal))
			{
				return mb.Name.Substring(NamePrefix.DefaultMethod.Length);
			}
			else if(mb.Name.StartsWith(NamePrefix.Bridge, StringComparison.Ordinal))
			{
				return mb.Name.Substring(NamePrefix.Bridge.Length);
			}
			else if(mb.IsSpecialName)
			{
				return UnicodeUtil.UnescapeInvalidSurrogates(mb.Name);
			}
			else
			{
				return mb.Name;
			}
		}

		private static bool IsHideFromJava(MethodBase mb)
		{
#if FIRST_PASS
			return false;
#else
			return (Java_sun_reflect_Reflection.GetHideFromJavaFlags(mb) & HideFromJavaFlags.StackTrace) != 0
				|| (mb.DeclaringType == typeof(ikvm.runtime.Util) && mb.Name == "mapException");
#endif
		}

		private static string getClassNameFromType(Type type)
		{
			if(type == null)
			{
				return "<Module>";
			}
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
#if !FIRST_PASS
				if(tw.IsUnsafeAnonymous)
				{
					return tw.ClassObject.getName();
				}
#endif
				return tw.Name;
			}
			return type.FullName;
		}

		private static int GetLineNumber(StackFrame frame)
		{
			int ilOffset = frame.GetILOffset();
			if(ilOffset != StackFrame.OFFSET_UNKNOWN)
			{
				MethodBase mb = frame.GetMethod();
				if(mb != null && mb.DeclaringType != null)
				{
					if(ClassLoaderWrapper.IsRemappedType(mb.DeclaringType))
					{
						return -1;
					}
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
			if(mb != null && mb.DeclaringType != null)
			{
				if(ClassLoaderWrapper.IsRemappedType(mb.DeclaringType))
				{
					return null;
				}
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
				new ObjectStreamField("stackTrace", typeof(global::java.lang.StackTraceElement[])),
				new ObjectStreamField("suppressedExceptions", typeof(global::java.util.List))
			};
#endif
		}

		internal static void writeObject(Exception x, ObjectOutputStream s)
		{
#if !FIRST_PASS
			lock (x)
			{
				ObjectOutputStream.PutField fields = s.putFields();
				Throwable _thisJava = x as Throwable;
				if (_thisJava == null)
				{
					fields.put("detailMessage", x.Message);
					fields.put("cause", x.InnerException);
					// suppressed exceptions are not supported on CLR exceptions
					fields.put("suppressedExceptions", null);
					fields.put("stackTrace", getOurStackTrace(x));
				}
				else
				{
					fields.put("detailMessage", _thisJava.detailMessage);
					fields.put("cause", _thisJava.cause);
					fields.put("suppressedExceptions", _thisJava.suppressedExceptions);
					getOurStackTrace(x);
					fields.put("stackTrace", _thisJava.stackTrace ?? java.lang.ThrowableHelper.SentinelHolder.STACK_TRACE_SENTINEL);
				}
				s.writeFields();
			}
#endif
		}

		internal static void readObject(Exception x, ObjectInputStream s)
		{
#if !FIRST_PASS
			lock (x)
			{
				// when you serialize a .NET exception it gets replaced by a com.sun.xml.internal.ws.developer.ServerSideException,
				// so we know that Exception is always a Throwable
				Throwable _this = (Throwable)x;

				// this the equivalent of s.defaultReadObject();
				ObjectInputStream.GetField fields = s.readFields();
				object detailMessage = fields.get("detailMessage", null);
				object cause = fields.get("cause", null);
				ConstructorInfo ctor = typeof(Throwable).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(Exception), typeof(bool), typeof(bool) }, null);
				if (cause == _this)
				{
					ctor.Invoke(_this, new object[] { detailMessage, null, false, false });
					_this.cause = _this;
				}
				else
				{
					ctor.Invoke(_this, new object[] { detailMessage, cause, false, false });
				}
				_this.stackTrace = (StackTraceElement[])fields.get("stackTrace", null);
				_this.suppressedExceptions = (java.util.List)fields.get("suppressedExceptions", null);

				// this is where the rest of the Throwable.readObject() code starts
				if (_this.suppressedExceptions != null)
				{
					java.util.List suppressed = null;
					if (_this.suppressedExceptions.isEmpty())
					{
						suppressed = Throwable.SUPPRESSED_SENTINEL;
					}
					else
					{
						suppressed = new java.util.ArrayList(1);
						for (int i = 0; i < _this.suppressedExceptions.size(); i++)
						{
							Exception entry = (Exception)_this.suppressedExceptions.get(i);
							if (entry == null)
							{
								throw new java.lang.NullPointerException("Cannot suppress a null exception.");
							}
							if (entry == _this)
							{
								throw new java.lang.IllegalArgumentException("Self-suppression not permitted");
							}
							suppressed.add(entry);
						}
					}
					_this.suppressedExceptions = suppressed;
				}

				if (_this.stackTrace != null)
				{
					if (_this.stackTrace.Length == 0)
					{
						_this.stackTrace = new StackTraceElement[0];
					}
					else if (_this.stackTrace.Length == 1
						&& java.lang.ThrowableHelper.SentinelHolder.STACK_TRACE_ELEMENT_SENTINEL.equals(_this.stackTrace[0]))
					{
						_this.stackTrace = null;
					}
					else
					{
						foreach (StackTraceElement elem in _this.stackTrace)
						{
							if (elem == null)
							{
								throw new java.lang.NullPointerException("null StackTraceElement in serial stream. ");
							}
						}
					}
				}
				else
				{
					_this.stackTrace = new StackTraceElement[0];
				}
			}
#endif
		}

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
			return ikvm.extensions.ExtensionMethods.toString(cause);
#endif
		}

		internal static string getLocalizedMessage(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			return ikvm.extensions.ExtensionMethods.getMessage(x);
#endif
		}

		internal static string toString(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			string message = ikvm.extensions.ExtensionMethods.getLocalizedMessage(x);
			if (message == null)
			{
				return ikvm.extensions.ExtensionMethods.getClass(x).getName();
			}
			return ikvm.extensions.ExtensionMethods.getClass(x).getName() + ": " + message;
#endif
		}

		internal static Exception getCause(Exception _this)
		{
#if FIRST_PASS
			return null;
#else
			lock (_this)
			{
				Exception cause = ((Throwable)_this).cause;
				return cause == _this ? null : cause;
			}
#endif
		}

		internal static void checkInitCause(Exception _this, Exception _this_cause, Exception cause)
		{
#if !FIRST_PASS
			if (_this_cause != _this)
			{
				throw new java.lang.IllegalStateException("Can't overwrite cause with " + java.util.Objects.toString(cause, "a null"), _this);
			}
			if (cause == _this)
			{
				throw new java.lang.IllegalArgumentException("Self-causation not permitted", _this);
			}
#endif
		}

		internal static void addSuppressed(Exception _this, Exception x)
		{
#if !FIRST_PASS
			lock (_this)
			{
				if (_this == x)
				{
					throw new java.lang.IllegalArgumentException("Self-suppression not permitted", x);
				}
				if (x == null)
				{
					throw new java.lang.NullPointerException("Cannot suppress a null exception.");
				}
				Throwable _thisJava = _this as Throwable;
				if (_thisJava == null)
				{
					// we ignore suppressed exceptions for non-Java exceptions
				}
				else
				{
					if (_thisJava.suppressedExceptions == null)
					{
						return;
					}
					if (_thisJava.suppressedExceptions == Throwable.SUPPRESSED_SENTINEL)
					{
						_thisJava.suppressedExceptions = new java.util.ArrayList();
					}
					_thisJava.suppressedExceptions.add(x);
				}
			}
#endif
		}

		internal static Exception[] getSuppressed(Exception _this)
		{
#if FIRST_PASS
			return null;
#else
			lock (_this)
			{
				Throwable _thisJava = _this as Throwable;
				if (_thisJava == null)
				{
					// we ignore suppressed exceptions for non-Java exceptions
					return EMPTY_THROWABLE_ARRAY;
				}
				else
				{
					if (_thisJava.suppressedExceptions == Throwable.SUPPRESSED_SENTINEL
						|| _thisJava.suppressedExceptions == null)
					{
						return EMPTY_THROWABLE_ARRAY;
					}
					return (Exception[])_thisJava.suppressedExceptions.toArray(EMPTY_THROWABLE_ARRAY);
				}
			}
#endif
		}

		internal static int getStackTraceDepth(Exception _this)
		{
			return getOurStackTrace(_this).Length;
		}

		internal static StackTraceElement getStackTraceElement(Exception _this, int index)
		{
			return getOurStackTrace(_this)[index];
		}

		internal static StackTraceElement[] getOurStackTrace(Exception x)
		{
#if FIRST_PASS
			return null;
#else
			Throwable _this = x as Throwable;
			if (_this == null)
			{
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
						return Throwable.UNASSIGNED_STACK;
					}
					return eih.get_StackTrace(x);
				}
			}
			else
			{
				lock (_this)
				{
					if (_this.stackTrace == Throwable.UNASSIGNED_STACK
						|| (_this.stackTrace == null && (_this.tracePart1 != null || _this.tracePart2 != null)))
					{
						ExceptionInfoHelper eih = new ExceptionInfoHelper(_this.tracePart1, _this.tracePart2);
						_this.stackTrace = eih.get_StackTrace(x);
						_this.tracePart1 = null;
						_this.tracePart2 = null;
					}
				}
				return _this.stackTrace ?? Throwable.UNASSIGNED_STACK;
			}
#endif
		}

		internal static void setStackTrace(Exception x, StackTraceElement[] stackTrace)
		{
#if !FIRST_PASS
			StackTraceElement[] copy = (StackTraceElement[])stackTrace.Clone();
			for (int i = 0; i < copy.Length; i++)
			{
				if (copy[i] == null)
				{
					throw new java.lang.NullPointerException();
				}
			}
			SetStackTraceImpl(x, copy);
#endif
		}

		private static void SetStackTraceImpl(Exception x, StackTraceElement[] stackTrace)
		{
#if !FIRST_PASS
			Throwable _this = x as Throwable;
			if (_this == null)
			{
				ExceptionInfoHelper eih = new ExceptionInfoHelper(stackTrace);
				IDictionary data = x.Data;
				if (data != null && !data.IsReadOnly)
				{
					lock (data.SyncRoot)
					{
						data[EXCEPTION_DATA_KEY] = eih;
					}
				}
			}
			else
			{
				lock (_this)
				{
					if (_this.stackTrace == null && _this.tracePart1 == null && _this.tracePart2 == null)
					{
						return;
					}
					_this.stackTrace = stackTrace;
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
		private static Exception MapTypeInitializeException(TypeInitializationException t, Type handler)
		{
#if FIRST_PASS
			return null;
#else
			bool wrapped = false;
			Exception r = MapException<Exception>(t.InnerException, true, false);
			if (!(r is java.lang.Error))
			{
				r = new java.lang.ExceptionInInitializerError(r);
				wrapped = true;
			}
			string type = t.TypeName;
			if (failedTypes.ContainsKey(type))
			{
				r = new java.lang.NoClassDefFoundError("Could not initialize class " + type);
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

		private static bool IsInstanceOfType<T>(Exception t, bool remap)
			where T : Exception
		{
#if FIRST_PASS
			return false;
#else
			if (!remap && typeof(T) == typeof(Exception))
			{
				return !(t is Throwable);
			}
			return t is T;
#endif
		}

		[HideFromJava]
		internal static T MapException<T>(Exception x, bool remap, bool unused)
			where T : Exception
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
					return (T)MapTypeInitializeException((TypeInitializationException)x, typeof(T));
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

			if (IsInstanceOfType<T>(x, remap))
			{
				Throwable t = x as Throwable;
				if (t != null)
				{
					if (!unused && t.tracePart1 == null && t.tracePart2 == null && t.stackTrace == Throwable.UNASSIGNED_STACK)
					{
						t.tracePart1 = new StackTrace(org, true);
						t.tracePart2 = new StackTrace(true);
					}
					if (t != org)
					{
						t.original = org;
						exceptions.remove(org);
					}
				}
				else
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

				if (nonJavaException && !remap)
				{
					exceptions.put(x, NOT_REMAPPED);
				}
				return (T)x;
			}
			return null;
#endif
		}
	}
}
