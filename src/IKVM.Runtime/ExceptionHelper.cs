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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

using IKVM.Attributes;

using Interlocked = System.Threading.Interlocked;
using MethodBase = System.Reflection.MethodBase;

using ObjectInputStream = java.io.ObjectInputStream;
using ObjectOutputStream = java.io.ObjectOutputStream;
using ObjectStreamField = java.io.ObjectStreamField;
using StackTraceElement = java.lang.StackTraceElement;
using Throwable = java.lang.Throwable;

namespace IKVM.Runtime
{

    class ExceptionHelper
    {

        static readonly Key EXCEPTION_DATA_KEY = new Key();
        static readonly Exception NOT_REMAPPED = new Exception();
        static readonly Exception[] EMPTY_THROWABLE_ARRAY = new Exception[0];
        static readonly bool cleanStackTrace = JVM.SafeGetEnvironmentVariable("IKVM_DISABLE_STACKTRACE_CLEANING") == null;

        readonly RuntimeContext context;
        readonly Dictionary<string, string> failedTypes = new Dictionary<string, string>();

#if FIRST_PASS == false
        readonly ConditionalWeakTable<Exception, Exception> exceptions = new();
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ExceptionHelper(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

#if FIRST_PASS == false
            // make sure the exceptions map continues to work during AppDomain finalization
            GC.SuppressFinalize(exceptions);
#endif
        }

#if !FIRST_PASS

        [Serializable]
        internal sealed class ExceptionInfoHelper
        {

            readonly ExceptionHelper exceptionHelper;

            [NonSerialized]
            StackTrace tracePart1;
            [NonSerialized]
            StackTrace tracePart2;
            StackTraceElement[] stackTrace;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="exceptionHelper"></param>
            /// <param name="stackTrace"></param>
            internal ExceptionInfoHelper(ExceptionHelper exceptionHelper, StackTraceElement[] stackTrace)
            {
                this.exceptionHelper = exceptionHelper;
                this.stackTrace = stackTrace;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="exceptionHelper"></param>
            /// <param name="tracePart1"></param>
            /// <param name="tracePart2"></param>
            internal ExceptionInfoHelper(ExceptionHelper exceptionHelper, StackTrace tracePart1, StackTrace tracePart2)
            {
                this.exceptionHelper = exceptionHelper;
                this.tracePart1 = tracePart1;
                this.tracePart2 = tracePart2;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="exceptionHelper"></param>
            /// <param name="x"></param>
            /// <param name="captureAdditionalStackTrace"></param>
            [HideFromJava]
            internal ExceptionInfoHelper(ExceptionHelper exceptionHelper, Exception x, bool captureAdditionalStackTrace)
            {
                this.exceptionHelper = exceptionHelper;
                tracePart1 = new StackTrace(x, true);
                if (captureAdditionalStackTrace)
                    tracePart2 = new StackTrace(true);
            }

            [OnSerializing]
            void OnSerializing(StreamingContext context)
            {
                // make sure the stack trace is computed before serializing
                get_StackTrace(null);
            }

            static bool IsPrivateScope(MethodBase mb)
            {
                return (mb.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.PrivateScope;
            }

            internal StackTraceElement[] get_StackTrace(Exception t)
            {
                lock (this)
                {
                    if (stackTrace == null)
                    {
                        var list = new List<StackTraceElement>();

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

                            Append(exceptionHelper, list, tracePart1, skip1, false);
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
                                    var mb = tracePart2.GetFrame(skip).GetMethod();
                                    if (mb.DeclaringType == typeof(Throwable) && mb.Name.EndsWith("fillInStackTrace", StringComparison.Ordinal))
                                    {
                                        skip++;

                                        while (tracePart2.FrameCount > skip)
                                        {
                                            mb = tracePart2.GetFrame(skip).GetMethod();
                                            if (mb.DeclaringType != typeof(Throwable) || !mb.Name.EndsWith("fillInStackTrace", StringComparison.Ordinal))
                                                break;

                                            skip++;
                                        }

                                        while (tracePart2.FrameCount > skip)
                                        {
                                            mb = tracePart2.GetFrame(skip).GetMethod();
                                            if (mb.Name != ".ctor" || !mb.DeclaringType.IsInstanceOfType(t))
                                                break;

                                            skip++;
                                        }
                                    }
                                }
                                else
                                {
                                    // Skip java.lang.Throwable.__<map> and other mapping methods, because we need to be able to remove the frame
                                    // that called map (if it is the same as where the exception was caught).
                                    while (tracePart2.FrameCount > skip && IsHideFromJava(tracePart2.GetFrame(skip).GetMethod()))
                                        skip++;

                                    if (tracePart1.FrameCount > 0 &&
                                        tracePart2.FrameCount > skip &&
                                        tracePart1.GetFrame(tracePart1.FrameCount - 1).GetMethod() == tracePart2.GetFrame(skip).GetMethod())
                                    {
                                        // skip the caller of the map method
                                        skip++;
                                    }
                                }
                            }

                            Append(exceptionHelper, list, tracePart2, skip, true);
                        }

                        if (cleanStackTrace && list.Count > 0)
                        {
                            var elem = list[list.Count - 1];
                            if (elem.getClassName() == "java.lang.reflect.Method")
                                list.RemoveAt(list.Count - 1);
                        }

                        tracePart1 = null;
                        tracePart2 = null;
                        this.stackTrace = list.ToArray();
                    }
                }

                return (StackTraceElement[])stackTrace.Clone();
            }

            internal static void Append(ExceptionHelper exceptionHelper, List<StackTraceElement> stackTrace, StackTrace st, int skip, bool isLast)
            {
                for (int i = skip; i < st.FrameCount; i++)
                {
                    var frame = st.GetFrame(i);
                    var m = frame.GetMethod();
                    if (m == null)
                        continue;

                    var type = m.DeclaringType;
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

                    var lineNumber = frame.GetFileLineNumber();
                    if (lineNumber == 0)
                        lineNumber = exceptionHelper.GetLineNumber(frame);

                    var fileName = frame.GetFileName();
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

                    fileName ??= exceptionHelper.GetFileName(frame);
                    stackTrace.Add(new StackTraceElement(exceptionHelper.GetClassNameFromType(type), GetMethodName(m), fileName, IsNative(m) ? -2 : lineNumber));
                }

                if (cleanStackTrace && isLast)
                    while (stackTrace.Count > 0 && stackTrace[stackTrace.Count - 1].getClassName().StartsWith("cli.System.Threading.", StringComparison.Ordinal))
                        stackTrace.RemoveAt(stackTrace.Count - 1);
            }
        }

#endif

        [Serializable]
        sealed class Key : ISerializable
        {

            [Serializable]
            sealed class Helper : IObjectReference
            {

                [SecurityCritical]
                public object GetRealObject(StreamingContext context)
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

        static bool IsNative(MethodBase m)
        {
            var methodFlagAttribs = m.GetCustomAttributes(typeof(ModifiersAttribute), false);
            if (methodFlagAttribs.Length == 1)
            {
                var modifiersAttrib = (ModifiersAttribute)methodFlagAttribs[0];
                return (modifiersAttrib.Modifiers & Modifiers.Native) != 0;
            }

            return false;
        }

        static string GetMethodName(MethodBase mb)
        {
            var attr = mb.GetCustomAttributes(typeof(NameSigAttribute), false);
            if (attr.Length == 1)
            {
                return ((NameSigAttribute)attr[0]).Name;
            }
            else if (mb.Name == ".ctor")
            {
                return "<init>";
            }
            else if (mb.Name == ".cctor")
            {
                return "<clinit>";
            }
            else if (mb.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal))
            {
                return mb.Name.Substring(NamePrefix.DefaultMethod.Length);
            }
            else if (mb.Name.StartsWith(NamePrefix.Bridge, StringComparison.Ordinal))
            {
                return mb.Name.Substring(NamePrefix.Bridge.Length);
            }
            else if (mb.IsSpecialName)
            {
                return UnicodeUtil.UnescapeInvalidSurrogates(mb.Name);
            }
            else
            {
                return mb.Name;
            }
        }

        static bool IsHideFromJava(MethodBase mb)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return (IKVM.Java.Externs.sun.reflect.Reflection.GetHideFromJavaFlags(mb) & HideFromJavaFlags.StackTrace) != 0 || (mb.DeclaringType == typeof(ikvm.runtime.Util) && mb.Name == "mapException");
#endif
        }

        string GetClassNameFromType(Type type)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (type == null)
                return "<Module>";

            if (context.ClassLoaderFactory.IsRemappedType(type))
                return RuntimeManagedJavaType.GetName(context, type);

            var tw = context.ClassLoaderFactory.GetJavaTypeFromType(type);
            if (tw != null)
            {
                if (tw.IsPrimitive)
                    return RuntimeManagedJavaType.GetName(context, type);
                if (tw.IsUnsafeAnonymous)
                    return tw.ClassObject.getName();

                return tw.Name;
            }

            return type.FullName;
#endif
        }

        int GetLineNumber(StackFrame frame)
        {
            var ilOffset = frame.GetILOffset();
            if (ilOffset != StackFrame.OFFSET_UNKNOWN)
            {
                var mb = frame.GetMethod();
                if (mb != null && mb.DeclaringType != null)
                {
                    if (context.ClassLoaderFactory.IsRemappedType(mb.DeclaringType))
                        return -1;

                    var tw = context.ClassLoaderFactory.GetJavaTypeFromType(mb.DeclaringType);
                    if (tw != null)
                        return tw.GetSourceLineNumber(mb, ilOffset);
                }
            }

            return -1;
        }

        string GetFileName(StackFrame frame)
        {
            var mb = frame.GetMethod();
            if (mb != null && mb.DeclaringType != null)
            {
                if (context.ClassLoaderFactory.IsRemappedType(mb.DeclaringType))
                    return null;

                var tw = context.ClassLoaderFactory.GetJavaTypeFromType(mb.DeclaringType);
                if (tw != null)
                    return tw.GetSourceFileName();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Called from map.xml for IKVM.Java.
        /// </remarks>
        /// <returns></returns>
        internal static object[] GetPersistentFields()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new ObjectStreamField[]
            {
                new ObjectStreamField("detailMessage", typeof(global::java.lang.String)),
                new ObjectStreamField("cause", typeof(global::java.lang.Throwable)),
                new ObjectStreamField("stackTrace", typeof(global::java.lang.StackTraceElement[])),
                new ObjectStreamField("suppressedExceptions", typeof(global::java.util.List))
            };
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Called from map.xml for IKVM.Java.
        /// </remarks>
        /// <param name="e"></param>
        /// <param name="stream"></param>
        internal static void WriteObject(Exception e, object stream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (e)
            {
                var fields = ((ObjectOutputStream)stream).putFields();
                if (e is not Throwable t)
                {
                    fields.put("detailMessage", e.Message);
                    fields.put("cause", e.InnerException);
                    // suppressed exceptions are not supported on CLR exceptions
                    fields.put("suppressedExceptions", null);
                    fields.put("stackTrace", GetOurStackTrace(e));
                }
                else
                {
                    fields.put("detailMessage", t.detailMessage);
                    fields.put("cause", t.cause);
                    fields.put("suppressedExceptions", t.suppressedExceptions);
                    GetOurStackTrace(e);
                    fields.put("stackTrace", t.stackTrace ?? java.lang.ThrowableHelper.SentinelHolder.STACK_TRACE_SENTINEL);
                }

                ((ObjectOutputStream)stream).writeFields();
            }
#endif
        }

        internal static void ReadObject(Exception e, object stream)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (e)
            {
                // when you serialize a .NET exception it gets replaced by a com.sun.xml.internal.ws.developer.ServerSideException,
                // so we know that Exception is always a Throwable
                var _this = (Throwable)e;

                // this the equivalent of s.defaultReadObject();
                var fields = ((ObjectInputStream)stream).readFields();
                var detailMessage = fields.get("detailMessage", null);
                var cause = fields.get("cause", null);
                var ctor = typeof(Throwable).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(Exception), typeof(bool), typeof(bool) }, null);
                if (cause == _this)
                {
                    ctor.Invoke(_this, [detailMessage, null, false, false]);
                    _this.cause = _this;
                }
                else
                {
                    ctor.Invoke(_this, [detailMessage, cause, false, false]);
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
                            var entry = (Exception)_this.suppressedExceptions.get(i);
                            if (entry == null)
                                throw new java.lang.NullPointerException("Cannot suppress a null exception.");
                            if (entry == _this)
                                throw new java.lang.IllegalArgumentException("Self-suppression not permitted");

                            suppressed.add(entry);
                        }
                    }

                    _this.suppressedExceptions = suppressed;
                }

                if (_this.stackTrace != null)
                {
                    if (_this.stackTrace.Length == 0)
                    {
                        _this.stackTrace = [];
                    }
                    else if (_this.stackTrace.Length == 1 && java.lang.ThrowableHelper.SentinelHolder.STACK_TRACE_ELEMENT_SENTINEL.equals(_this.stackTrace[0]))
                    {
                        _this.stackTrace = null;
                    }
                    else
                    {
                        foreach (var elem in _this.stackTrace)
                            if (elem == null)
                                throw new java.lang.NullPointerException("null StackTraceElement in serial stream. ");
                    }
                }
                else
                {
                    _this.stackTrace = [];
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
            throw new NotImplementedException();
#else
            return cause != null ? ikvm.extensions.ExtensionMethods.toString(cause) : "";
#endif
        }

        internal static string GetLocalizedMessage(Exception x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ikvm.extensions.ExtensionMethods.getMessage(x);
#endif
        }

        internal static string ToString(Exception x)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var message = ikvm.extensions.ExtensionMethods.getLocalizedMessage(x);
            if (message == null)
                return ikvm.extensions.ExtensionMethods.getClass(x).getName();
            else
                return ikvm.extensions.ExtensionMethods.getClass(x).getName() + ": " + message;
#endif
        }

        internal static Exception GetCause(Exception _this)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (_this)
            {
                var cause = ((Throwable)_this).cause;
                return cause == _this ? null : cause;
            }
#endif
        }

        internal static void CheckInitCause(Exception self, Exception self_cause, Exception cause)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (self_cause != self)
                throw new java.lang.IllegalStateException("Can't overwrite cause with " + java.util.Objects.toString(cause, "a null"), self);

            if (cause == self)
                throw new java.lang.IllegalArgumentException("Self-causation not permitted", self);
#endif
        }

        internal static void AddSuppressed(Exception self, Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (self)
            {
                if (self == e)
                    throw new java.lang.IllegalArgumentException("Self-suppression not permitted", e);
                if (e == null)
                    throw new java.lang.NullPointerException("Cannot suppress a null exception.");

                if (self is not Throwable _thisJava)
                {
                    // we ignore suppressed exceptions for non-Java exceptions
                }
                else
                {
                    if (_thisJava.suppressedExceptions == null)
                        return;

                    if (_thisJava.suppressedExceptions == Throwable.SUPPRESSED_SENTINEL)
                        _thisJava.suppressedExceptions = new java.util.ArrayList();

                    _thisJava.suppressedExceptions.add(e);
                }
            }
#endif
        }

        internal static Exception[] GetSuppressed(Exception self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (self)
            {
                if (self is not Throwable t)
                {
                    // we ignore suppressed exceptions for non-Java exceptions
                    return EMPTY_THROWABLE_ARRAY;
                }
                else
                {
                    if (t.suppressedExceptions == Throwable.SUPPRESSED_SENTINEL || t.suppressedExceptions == null)
                        return EMPTY_THROWABLE_ARRAY;
                    else
                        return (Exception[])t.suppressedExceptions.toArray(EMPTY_THROWABLE_ARRAY);
                }
            }
#endif
        }

        internal static int GetStackTraceDepth(Exception self)
        {
            return GetOurStackTrace(self).Length;
        }

        internal static object GetStackTraceElement(Exception self, int index)
        {
            return GetOurStackTrace(self)[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Called from map.xml for IKVM.Java.
        /// </remarks>
        /// <returns></returns>
        internal static object[] GetOurStackTrace(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (e is not Throwable self)
            {
                lock (e)
                {
                    ExceptionInfoHelper eih = null;
                    var data = e.Data;
                    if (data != null && !data.IsReadOnly)
                        lock (data.SyncRoot)
                            eih = (ExceptionInfoHelper)data[EXCEPTION_DATA_KEY];

                    if (eih == null)
                        return Throwable.UNASSIGNED_STACK;

                    return eih.get_StackTrace(e);
                }
            }
            else
            {
                lock (self)
                {
                    if (self.stackTrace == Throwable.UNASSIGNED_STACK || (self.stackTrace == null && (self.tracePart1 != null || self.tracePart2 != null)))
                    {
                        var eih = new ExceptionInfoHelper(JVM.Context.ExceptionHelper, self.tracePart1, self.tracePart2);
                        self.stackTrace = eih.get_StackTrace(e);
                        self.tracePart1 = null;
                        self.tracePart2 = null;
                    }
                }

                return self.stackTrace ?? Throwable.UNASSIGNED_STACK;
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Called from map.xml for IKVM.Java.
        /// </remarks>
        /// <returns></returns>
        internal static void SetStackTrace(Exception e, object[] stackTrace)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stackTrace_ = (StackTraceElement[])stackTrace;
            var copy = (StackTraceElement[])stackTrace_.Clone();
            for (int i = 0; i < copy.Length; i++)
                if (copy[i] == null)
                    throw new java.lang.NullPointerException();

            JVM.Context.ExceptionHelper.SetStackTraceImpl(e, copy);
#endif
        }

        void SetStackTraceImpl(Exception e, StackTraceElement[] stackTrace)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (e is not Throwable t)
            {
                var eih = new ExceptionInfoHelper(this, stackTrace);
                var data = e.Data;
                if (data != null && !data.IsReadOnly)
                    lock (data.SyncRoot)
                        data[EXCEPTION_DATA_KEY] = eih;
            }
            else
            {
                lock (t)
                {
                    if (t.stackTrace == null && t.tracePart1 == null && t.tracePart2 == null)
                        return;

                    t.stackTrace = stackTrace;
                }
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This method is *only* for .NET exceptions (i.e. types not derived from java.lang.Throwable).
        /// </remarks>
        /// <param name="e"></param>
        [HideFromJava]
        internal static void FillInStackTrace(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (e)
            {
                var eih = new ExceptionInfoHelper(JVM.Context.ExceptionHelper, null, new StackTrace(true));
                var data = e.Data;
                if (data != null && !data.IsReadOnly)
                    lock (data.SyncRoot)
                        data[EXCEPTION_DATA_KEY] = eih;
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This method is *only* for .NET exceptions (i.e. types not derived from java.lang.Throwable).
        /// </remarks>
        /// <param name="e"></param>
        internal static void FixateException(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            JVM.Context.ExceptionHelper.FixateExceptionImpl(e);
#endif
        }
        void FixateExceptionImpl(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            exceptions.Add(e, NOT_REMAPPED);
#endif
        }

        internal static Exception UnmapException(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Context.ExceptionHelper.UnmapExceptionImpl(e);
#endif
        }

        Exception UnmapExceptionImpl(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (e is Throwable t)
            {
                var org = Interlocked.Exchange(ref t.original, null);
                if (org != null)
                {
                    exceptions.Add(org, e);
                    e = org;
                }
            }

            return e;
#endif
        }

        [HideFromJava]
        Exception MapTypeInitializeException(TypeInitializationException t, Type handler)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var wrapped = false;
            var r = MapException<Exception>(t.InnerException, true, false);
            if (r is not java.lang.Error)
            {
                // Forwarding "r" as cause only doesn't make it available in the debugger details
                // of current versions of VS, so at least provide a text representation instead.
                // Not wrapping at all might be the even better approach, but it was introduced for
                // some reason I guess.
                r = new java.lang.ExceptionInInitializerError(r.ToString());
                wrapped = true;
            }

            var type = t.TypeName;
            if (failedTypes.ContainsKey(type))
            {
                r = new java.lang.NoClassDefFoundError("Could not initialize class " + type);
                wrapped = true;
            }

            if (handler != null && !handler.IsInstanceOfType(r))
                return null;

            failedTypes[type] = type;
            if (wrapped)
            {
                // transplant the stack trace
                ((Throwable)r).setStackTrace(new ExceptionInfoHelper(this, t, true).get_StackTrace(t));
            }

            return r;
#endif
        }

        bool IsInstanceOfType<T>(Exception t, bool remap)
            where T : Exception
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return remap || typeof(T) != typeof(Exception) ? t is T : t is not Throwable;
#endif
        }

        /// <summary>
        /// Maps the given exception to a form suitable for exposure to Java code.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="e"></param>
        /// <param name="remap"></param>
        /// <param name="unused"></param>
        /// <returns></returns>[HideFromJava]
        internal T MapException<T>(Exception e, bool remap, bool unused)
            where T : Exception
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var org = e;
            var nonJavaException = e is not Throwable;
            if (nonJavaException && remap)
            {
                if (e is TypeInitializationException tie)
                    return (T)MapTypeInitializeException(tie, typeof(T));

                exceptions.TryGetValue(e, out var obj);
                var remapped = obj;
                if (remapped == null)
                {
                    remapped = Throwable.__mapImpl(e);
                    if (remapped == e)
                        exceptions.Add(e, NOT_REMAPPED);
                    else
                        exceptions.Add(e, remapped);

                    e = remapped;
                }
                else if (remapped != NOT_REMAPPED)
                {
                    e = remapped;
                }
            }

            if (IsInstanceOfType<T>(e, remap))
            {
                if (e is Throwable t)
                {
                    if (!unused && t.tracePart1 == null && t.tracePart2 == null && t.stackTrace == Throwable.UNASSIGNED_STACK)
                    {
                        t.tracePart1 = new StackTrace(org, true);
                        t.tracePart2 = new StackTrace(true);
                    }
                    if (t != org)
                    {
                        t.original = org;
                        exceptions.Remove(org);
                    }
                }
                else
                {
                    var data = e.Data;
                    if (data != null && !data.IsReadOnly)
                    {
                        lock (data.SyncRoot)
                        {
                            if (!data.Contains(EXCEPTION_DATA_KEY))
                            {
                                data.Add(EXCEPTION_DATA_KEY, new ExceptionInfoHelper(this, e, true));
                            }
                        }
                    }
                }

                if (nonJavaException && !remap)
                    exceptions.Add(e, NOT_REMAPPED);

                return (T)e;
            }

            return null;
#endif
        }

    }

}
