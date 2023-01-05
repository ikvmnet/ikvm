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
using System.Runtime.InteropServices;

using IKVM.Internal;
using IKVM.Runtime.Text;

namespace IKVM.Runtime.JNI
{

    using jarray = System.IntPtr;
    using jboolean = System.SByte;
    using jbooleanArray = System.IntPtr;
    using jbyte = System.SByte;
    using jbyteArray = System.IntPtr;
    using jchar = System.UInt16;
    using jcharArray = System.IntPtr;
    using jclass = System.IntPtr;
    using jdouble = System.Double;
    using jdoubleArray = System.IntPtr;
    using jfieldID = System.IntPtr;
    using jfloat = System.Single;
    using jfloatArray = System.IntPtr;
    using jint = System.Int32;
    using jintArray = System.IntPtr;
    using jlong = System.Int64;
    using jlongArray = System.IntPtr;
    using jmethodID = System.IntPtr;
    using jobject = System.IntPtr;
    using jshort = System.Int16;
    using jshortArray = System.IntPtr;
    using jsize = System.Int32;
    using jstring = System.IntPtr;
    using jthrowable = System.IntPtr;
    using jweak = System.IntPtr;
    using jobjectArray = System.IntPtr;

    [StructLayout(LayoutKind.Sequential)]
    unsafe partial struct JNIEnv
    {

        internal const int JNI_OK = 0;
        internal const int JNI_ERR = -1;
        internal const int JNI_EDETACHED = -2;
        internal const int JNI_EVERSION = -3;
        internal const int JNI_COMMIT = 1;
        internal const int JNI_ABORT = 2;
        internal const int JNI_VERSION_1_1 = 0x00010001;
        internal const int JNI_VERSION_1_2 = 0x00010002;
        internal const int JNI_VERSION_1_4 = 0x00010004;
        internal const int JNI_VERSION_1_6 = 0x00010006;
        internal const int JNI_VERSION_1_8 = 0x00010008;
        internal const int JNIInvalidRefType = 0;
        internal const int JNILocalRefType = 1;
        internal const int JNIGlobalRefType = 2;
        internal const int JNIWeakGlobalRefType = 3;
        internal const jboolean JNI_TRUE = 1;
        internal const jboolean JNI_FALSE = 0;

        void* vtable;
        GCHandle managedJNIEnv;
        GCHandle* pinHandles;
        int pinHandleMaxCount;
        int pinHandleInUseCount;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JNIEnv()
        {
            // we set the field here so that IKVM.Runtime.dll doesn't have to load us if we're not otherwise needed
            IKVM.Java.Externs.java.lang.SecurityManager.jniAssembly = typeof(JNIEnv).Assembly;
        }

        internal ManagedJNIEnv GetManagedJNIEnv()
        {
            return (ManagedJNIEnv)managedJNIEnv.Target;
        }

        internal sealed class ManagedJNIEnv
        {
            // NOTE the initial bucket size must be a power of two < LOCAL_REF_MAX_BUCKET_SIZE,
            // because each time we grow it, we double the size and it must eventually reach
            // exactly LOCAL_REF_MAX_BUCKET_SIZE
            const int LOCAL_REF_INITIAL_BUCKET_SIZE = 32;
            const int LOCAL_REF_SHIFT = 10;
            const int LOCAL_REF_MAX_BUCKET_SIZE = (1 << LOCAL_REF_SHIFT);
            const int LOCAL_REF_MASK = (LOCAL_REF_MAX_BUCKET_SIZE - 1);

            internal readonly JNIEnv* pJNIEnv;
            internal ClassLoaderWrapper classLoader;
            internal ikvm.@internal.CallerID callerID;

            object[][] localRefs;
            int localRefSlot;
            int localRefIndex;
            object[] active;
            internal Exception pendingException;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            internal ManagedJNIEnv()
            {
                pJNIEnv = (JNIEnv*)JNIMemory.Alloc(sizeof(JNIEnv));
                localRefs = new object[32][];
                active = localRefs[0] = new object[LOCAL_REF_INITIAL_BUCKET_SIZE];
                // stuff something in the first entry to make sure we don't hand out a zero handle
                // (a zero handle corresponds to a null reference)
                active[0] = "";
                localRefIndex = 1;
            }

            ~ManagedJNIEnv()
            {
                // NOTE don't clean up when we're being unloaded (we'll get cleaned up anyway and because
                // of the unorderedness of the finalization process native code could still be run after
                // we run).
                // NOTE when we're not the default AppDomain and we're being unloaded,
                // we're leaking the JNIEnv (but since JNI outside of the default AppDomain isn't currently supported,
                // I can live with that).
                if (!Environment.HasShutdownStarted)
                {
                    if (pJNIEnv->managedJNIEnv.IsAllocated)
                        pJNIEnv->managedJNIEnv.Free();

                    for (int i = 0; i < pJNIEnv->pinHandleMaxCount; i++)
                        if (pJNIEnv->pinHandles[i].IsAllocated)
                            pJNIEnv->pinHandles[i].Free();

                    JNIMemory.Free((nint)(void*)pJNIEnv);
                }

            }

            internal struct FrameState
            {

                internal readonly ikvm.@internal.CallerID callerID;
                internal readonly int localRefSlot;
                internal readonly int localRefIndex;

                internal FrameState(ikvm.@internal.CallerID callerID, int localRefSlot, int localRefIndex)
                {
                    this.callerID = callerID;
                    this.localRefSlot = localRefSlot;
                    this.localRefIndex = localRefIndex;
                }

            }

            internal FrameState Enter(ikvm.@internal.CallerID newCallerID)
            {
                FrameState prev = new FrameState(callerID, localRefSlot, localRefIndex);
                this.callerID = newCallerID;
                localRefSlot++;
                if (localRefSlot >= localRefs.Length)
                {
                    object[][] tmp = new object[localRefs.Length * 2][];
                    Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
                    localRefs = tmp;
                }
                localRefIndex = 0;
                active = localRefs[localRefSlot];
                if (active == null)
                {
                    active = localRefs[localRefSlot] = new object[LOCAL_REF_INITIAL_BUCKET_SIZE];
                }
                return prev;
            }

            internal Exception Leave(FrameState prev)
            {
                // on the current (.NET 2.0 SP2) x86 JIT an explicit for loop is faster than Array.Clear() up to about 100 elements
                for (int i = 0; i < localRefIndex; i++)
                {
                    active[i] = null;
                }

                while (--localRefSlot != prev.localRefSlot)
                {
                    if (localRefs[localRefSlot] != null)
                    {
                        if (localRefs[localRefSlot].Length == LOCAL_REF_MAX_BUCKET_SIZE)
                        {
                            // if the bucket is totally allocated, we're assuming a leaky method so we throw the bucket away
                            localRefs[localRefSlot] = null;
                        }
                        else
                        {
                            Array.Clear(localRefs[localRefSlot], 0, localRefs[localRefSlot].Length);
                        }
                    }
                }
                active = localRefs[localRefSlot];
                this.localRefIndex = prev.localRefIndex;
                this.callerID = prev.callerID;
                Exception x = pendingException;
                pendingException = null;
                return x;
            }

            internal jobject MakeLocalRef(object obj)
            {
                if (obj == null)
                {
                    return IntPtr.Zero;
                }

                int index;
                if (localRefIndex == active.Length)
                {
                    index = FindFreeIndex();
                }
                else
                {
                    index = localRefIndex++;
                }
                active[index] = obj;
                return (IntPtr)((localRefSlot << LOCAL_REF_SHIFT) + index);
            }

            private int FindFreeIndex()
            {
                for (int i = 0; i < active.Length; i++)
                {
                    if (active[i] == null)
                    {
                        while (localRefIndex - 1 > i && active[localRefIndex - 1] == null)
                        {
                            localRefIndex--;
                        }
                        return i;
                    }
                }
                GrowActiveSlot();
                return localRefIndex++;
            }

            private void GrowActiveSlot()
            {
                if (active.Length < LOCAL_REF_MAX_BUCKET_SIZE)
                {
                    object[] tmp = new object[active.Length * 2];
                    Array.Copy(active, tmp, active.Length);
                    active = localRefs[localRefSlot] = tmp;
                    return;
                }
                // if we get here, we're in a native method that most likely is leaking locals refs,
                // so we're going to allocate a new bucket and increment localRefSlot, this means that
                // any slots that become available in the previous bucket are not going to be reused,
                // but since we're assuming that the method is leaking anyway, that isn't a problem
                // (it's never a correctness issue, just a resource consumption issue)
                localRefSlot++;
                localRefIndex = 0;
                if (localRefSlot == localRefs.Length)
                {
                    object[][] tmp = new object[localRefSlot * 2][];
                    Array.Copy(localRefs, 0, tmp, 0, localRefSlot);
                    localRefs = tmp;
                }
                active = localRefs[localRefSlot];
                if (active == null)
                {
                    active = localRefs[localRefSlot] = new object[LOCAL_REF_MAX_BUCKET_SIZE];
                }
            }

            internal object UnwrapLocalRef(nint i)
            {
                return localRefs[i >> LOCAL_REF_SHIFT][i & LOCAL_REF_MASK];
            }

            internal int PushLocalFrame(jint capacity)
            {
                localRefSlot += 2;
                if (localRefSlot >= localRefs.Length)
                {
                    object[][] tmp = new object[localRefs.Length * 2][];
                    Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
                    localRefs = tmp;
                }
                // we use a null slot to mark the fact that we used PushLocalFrame
                localRefs[localRefSlot - 1] = null;
                if (localRefs[localRefSlot] == null)
                {
                    // we can't use capacity directly, because the array length must be a power of two
                    // and it can't be bigger than LOCAL_REF_MAX_BUCKET_SIZE
                    int r = 1;
                    capacity = Math.Min(capacity, LOCAL_REF_MAX_BUCKET_SIZE);
                    while (r < capacity)
                    {
                        r *= 2;
                    }
                    localRefs[localRefSlot] = new object[r];
                }
                localRefIndex = 0;
                active = localRefs[localRefSlot];
                return JNI_OK;
            }

            internal jobject PopLocalFrame(object res)
            {
                while (localRefs[localRefSlot] != null)
                {
                    localRefs[localRefSlot] = null;
                    localRefSlot--;
                }
                localRefSlot--;
                localRefIndex = localRefs[localRefSlot].Length;
                active = localRefs[localRefSlot];
                return MakeLocalRef(res);
            }

            internal void DeleteLocalRef(jobject obj)
            {
                int i = obj.ToInt32();
                if (i > 0)
                {
                    localRefs[i >> LOCAL_REF_SHIFT][i & LOCAL_REF_MASK] = null;
                    return;
                }
                if (i < 0)
                {
                    Debug.Assert(false, "bogus localref in DeleteLocalRef");
                }
            }
        }

        internal static JNIEnv* CreateJNIEnv()
        {
            ManagedJNIEnv env = new ManagedJNIEnv();
            TlsHack.ManagedJNIEnv = env;
            JNIEnv* pJNIEnv = env.pJNIEnv;
            pJNIEnv->vtable = JNINativeInterface.Handle;
            pJNIEnv->managedJNIEnv = GCHandle.Alloc(env, GCHandleType.WeakTrackResurrection);
            pJNIEnv->pinHandles = null;
            pJNIEnv->pinHandleMaxCount = 0;
            pJNIEnv->pinHandleInUseCount = 0;
            return pJNIEnv;
        }

        internal static void FreeJNIEnv()
        {
            TlsHack.ManagedJNIEnv = null;
        }

        /// <summary>
        /// Decodes the NULL terminated modified UTF-8 string pointed to by the given pointer.
        /// </summary>
        /// <param name="psz"></param>
        /// <returns></returns>
        /// <exception cref="java.lang.IllegalArgumentException"></exception>
        internal static string DecodeMUTF8Argument(byte* psz, string arg)
        {
            if (psz is null)
                return null;

            var l = MUTF8Encoding.IndexOfNull(psz);
            if (l < 0)
                throw new java.lang.IllegalArgumentException(arg);

            var v = MUTF8Encoding.MUTF8.GetString(psz, l);
            return v;
        }

        /// <summary>
        /// Decodes the NULL terminated modified UTF-8 string pointed to by the given pointer.
        /// </summary>
        /// <param name="psz"></param>
        /// <returns></returns>
        /// <exception cref="java.lang.IllegalArgumentException"></exception>
        internal static string DecodeMUTF8(byte* psz)
        {
            if (psz is null)
                return null;

            var l = MUTF8Encoding.IndexOfNull(psz);
            if (l < 0)
                throw new java.lang.IllegalArgumentException();

            var v = MUTF8Encoding.MUTF8.GetString(psz, l);
            return v;
        }

        /// <summary>
        /// Outputs an encoded signature of the arguments available to the method.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="method"></param>
        /// <param name="sig"></param>
        /// <returns></returns>
        internal static int GetMethodArgs(JNIEnv* pEnv, nint method, byte* sig)
        {
            var args = MethodWrapper.FromCookie(method).GetParameters();
            for (var i = 0; i < args.Length; i++)
                sig[i] = args[i].IsPrimitive ? (byte)args[i].SigName[0] : (byte)'L';

            return args.Length;
        }

        internal static jint GetVersion(JNIEnv* pEnv)
        {
            return JNI_VERSION_1_8;
        }

        internal static jclass DefineClass(JNIEnv* pEnv, byte* name, jobject loader, jbyte* pbuf, jint length)
        {
            try
            {
                var buf = new byte[length];
                Marshal.Copy((nint)(void*)pbuf, buf, 0, length);
                // TODO what should the protection domain be?
                // NOTE I'm assuming name is platform encoded (as opposed to UTF-8), but the Sun JVM only seems to work for ASCII.
                var classLoader = (java.lang.ClassLoader)pEnv->UnwrapRef(loader);
                return pEnv->MakeLocalRef(IKVM.Java.Externs.java.lang.ClassLoader.defineClass0(classLoader, name != null ? DecodeMUTF8Argument(name, nameof(name)) : null, buf, 0, buf.Length, null));
            }
            catch (Exception e)
            {
                SetPendingException(pEnv, e);
                return IntPtr.Zero;
            }
        }

        static ClassLoaderWrapper FindNativeMethodClassLoader(JNIEnv* pEnv)
        {
            var env = pEnv->GetManagedJNIEnv();
            if (env.callerID != null)
                return ClassLoaderWrapper.FromCallerID(env.callerID);
            else if (env.classLoader != null)
                return env.classLoader;
            else
                return ClassLoaderWrapper.GetClassLoaderWrapper(java.lang.ClassLoader.getSystemClassLoader());
        }

        internal static jclass FindClass(JNIEnv* pEnv, byte* name)
        {
            try
            {
                var n = DecodeMUTF8Argument(name, nameof(name));

                // don't allow dotted names!
                if (n.IndexOf('.') >= 0)
                    throw new java.lang.NoClassDefFoundError(n);

                // spec doesn't say it, but Sun allows signature format class names (but not for primitives)
                if (n.StartsWith("L") && n.EndsWith(";"))
                    n = n.Substring(1, n.Length - 2);

                var w = FindNativeMethodClassLoader(pEnv).LoadClassByDottedNameFast(n.Replace('/', '.'));
                if (w == null)
                    throw new java.lang.NoClassDefFoundError(n);

                w.Finish();
                w.RunClassInit(); // spec doesn't say it, but Sun runs the static initializer
                return pEnv->MakeLocalRef(w.ClassObject);
            }
            catch (Exception e)
            {
                if (e is RetargetableJavaException r)
                    e = r.ToJava();

                SetPendingException(pEnv, e);
                return IntPtr.Zero;
            }
        }

        internal static jmethodID FromReflectedMethod(JNIEnv* pEnv, jobject method)
        {
            return MethodWrapper.FromExecutable((java.lang.reflect.Executable)pEnv->UnwrapRef(method)).Cookie;
        }

        internal static jfieldID FromReflectedField(JNIEnv* pEnv, jobject field)
        {
            return FieldWrapper.FromField((java.lang.reflect.Field)pEnv->UnwrapRef(field)).Cookie;
        }

        internal static jobject ToReflectedMethod(JNIEnv* pEnv, jclass clazz_ignored, jmethodID method, jboolean isStatic)
        {
            return pEnv->MakeLocalRef(MethodWrapper.FromCookie(method).ToMethodOrConstructor(true));
        }

        internal static jclass GetSuperclass(JNIEnv* pEnv, jclass sub)
        {
            TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(sub)).BaseTypeWrapper;
            return pEnv->MakeLocalRef(wrapper == null ? null : wrapper.ClassObject);
        }

        internal static jboolean IsAssignableFrom(JNIEnv* pEnv, jclass sub, jclass super)
        {
            TypeWrapper w1 = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(sub));
            TypeWrapper w2 = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(super));
            return w1.IsAssignableTo(w2) ? JNI_TRUE : JNI_FALSE;
        }

        internal static jobject ToReflectedField(JNIEnv* pEnv, jclass clazz_ignored, jfieldID field, jboolean isStatic)
        {
            return pEnv->MakeLocalRef(FieldWrapper.FromCookie(field).ToField(true));
        }

        private static void SetPendingException(JNIEnv* pEnv, Exception x)
        {
            pEnv->GetManagedJNIEnv().pendingException = ikvm.runtime.Util.mapException(x);
        }

        internal static jint Throw(JNIEnv* pEnv, jthrowable throwable)
        {
            ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
            Exception x = UnwrapRef(env, throwable) as Exception;
            if (x != null)
            {
                env.pendingException = x;
            }
            return JNI_OK;
        }

        internal static jint ThrowNew(JNIEnv* pEnv, jclass clazz, byte* msg)
        {
            var env = pEnv->GetManagedJNIEnv();
            var wrapper = TypeWrapper.FromClass((java.lang.Class)UnwrapRef(env, clazz));
            var mw = wrapper.GetMethodWrapper("<init>", msg == null ? "()V" : "(Ljava.lang.String;)V", false);
            if (mw != null)
            {
                jint rc;
                Exception exception;

                try
                {
                    wrapper.Finish();
                    java.lang.reflect.Constructor cons = (java.lang.reflect.Constructor)mw.ToMethodOrConstructor(false);
                    exception = (Exception)cons.newInstance(msg == null ? new object[0] : new object[] { DecodeMUTF8Argument(msg, nameof(msg)) }, env.callerID);
                    rc = JNI_OK;
                }
                catch (RetargetableJavaException x)
                {
                    exception = x.ToJava();
                    rc = JNI_ERR;
                }
                catch (Exception x)
                {
                    exception = x;
                    rc = JNI_ERR;
                }

                SetPendingException(pEnv, exception);
                return rc;
            }
            else
            {
                SetPendingException(pEnv, new java.lang.NoSuchMethodError("<init>(Ljava.lang.String;)V"));
                return JNI_ERR;
            }
        }

        internal static jthrowable ExceptionOccurred(JNIEnv* pEnv)
        {
            ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
            return pEnv->MakeLocalRef(env.pendingException);
        }

        internal static void ExceptionDescribe(JNIEnv* pEnv)
        {
            ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
            Exception x = env.pendingException;
            if (x != null)
            {
                env.pendingException = null;
                try
                {
                    ikvm.extensions.ExtensionMethods.printStackTrace(x);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.ToString());
                }
            }
        }

        internal static void ExceptionClear(JNIEnv* pEnv)
        {
            var env = pEnv->GetManagedJNIEnv();
            env.pendingException = null;
        }

        internal static void FatalError(JNIEnv* pEnv, byte* msg)
        {
            Console.Error.WriteLine("FATAL ERROR in native method: {0}", msg == null ? "(null)" : DecodeMUTF8Argument(msg, nameof(msg)));
            Console.Error.WriteLine(new StackTrace(1, true));
            Environment.Exit(1);
        }

        /// <summary>
        /// Implements the JNI 'PushLocalFrame' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        internal static jint PushLocalFrame(JNIEnv* pEnv, jint capacity)
        {
            return pEnv->GetManagedJNIEnv().PushLocalFrame(capacity);
        }

        /// <summary>
        /// Implements the JNI 'PopLocalFrame' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        internal static jobject PopLocalFrame(JNIEnv* pEnv, jobject result)
        {
            var env = pEnv->GetManagedJNIEnv();
            return env.PopLocalFrame(UnwrapRef(env, result));
        }

        /// <summary>
        /// Implements the JNI 'NewGlobalRef' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static jobject NewGlobalRef(JNIEnv* pEnv, jobject obj)
        {
            var o = pEnv->UnwrapRef(obj);
            return o == null ? (jobject)0 : JNIGlobalRefTable.AddGlobalRef(o);
        }

        /// <summary>
        /// Implements the JNI 'DeleteGlobalRef' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj"></param>
        internal static void DeleteGlobalRef(JNIEnv* pEnv, jobject obj)
        {
            if (IsGlobalRef(obj))
            {
                JNIGlobalRefTable.DeleteGlobalRef(obj);
                return;
            }

            if (IsLocalRef(obj))
                Debug.Assert(false, "Local ref passed to DeleteGlobalRef");
        }

        /// <summary>
        /// Implements the JNI 'DeleteLocalRef' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj"></param>
        internal static void DeleteLocalRef(JNIEnv* pEnv, jobject obj)
        {
            pEnv->GetManagedJNIEnv().DeleteLocalRef(obj);
        }

        /// <summary>
        /// Implements the JNI 'IsSameObject' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        internal static jboolean IsSameObject(JNIEnv* pEnv, jobject obj1, jobject obj2)
        {
            return pEnv->UnwrapRef(obj1) == pEnv->UnwrapRef(obj2) ? JNI_TRUE : JNI_FALSE;
        }

        /// <summary>
        /// Implements the JNI 'NewLocalRef' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static jobject NewLocalRef(JNIEnv* pEnv, jobject obj)
        {
            return pEnv->MakeLocalRef(pEnv->UnwrapRef(obj));
        }

        /// <summary>
        /// Implements the JNI 'EnsureLocalCapacity' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        internal static jint EnsureLocalCapacity(JNIEnv* pEnv, jint capacity)
        {
            // since we can dynamically grow the local ref table, we'll just return success for any number
            return JNI_OK;
        }

        /// <summary>
        /// Implements the JNI 'AllocObject' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="clazz"></param>
        /// <returns></returns>
        internal static jobject AllocObject(JNIEnv* pEnv, jclass clazz)
        {
            return AllocObjectImpl(pEnv, TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz)));
        }

        static jobject AllocObjectImpl(JNIEnv* pEnv, TypeWrapper wrapper)
        {
            try
            {
                if (wrapper.IsAbstract)
                {
                    SetPendingException(pEnv, new java.lang.InstantiationException(wrapper.Name));
                    return IntPtr.Zero;
                }
                wrapper.Finish();
                return pEnv->MakeLocalRef(System.Runtime.Serialization.FormatterServices.GetUninitializedObject(wrapper.TypeAsBaseType));
            }
            catch (RetargetableJavaException e)
            {
                SetPendingException(pEnv, e.ToJava());
                return IntPtr.Zero;
            }
            catch (Exception e)
            {
                SetPendingException(pEnv, e);
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Invokes the given method, by ID, on the given object handle. Accepts a pointer to a <see cref="jvalue"/> array of arguments.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="objHandle"></param>
        /// <param name="methodID"></param>
        /// <param name="pArgs"></param>
        /// <param name="nonVirtual"></param>
        /// <returns></returns>
        static object InvokeHelper(JNIEnv* pEnv, jobject objHandle, jmethodID methodID, jvalue* pArgs, bool nonVirtual)
        {
            // resolve object who's method is being invoked
            var env = pEnv->GetManagedJNIEnv();
            var obj = UnwrapRef(env, objHandle);

            // resolve the method being invoked on the object
            var mw = MethodWrapper.FromCookie(methodID);
            mw.Link();
            mw.ResolveMethod();

            // assemble arguments into array
            var argTypes = mw.GetParameters();
            var args = new object[argTypes.Length + (mw.HasCallerID ? 1 : 0)];
            for (int i = 0; i < argTypes.Length; i++)
            {
                var type = argTypes[i];
                if (type == PrimitiveTypeWrapper.BOOLEAN)
                    args[i] = pArgs[i].z != JNI_FALSE;
                else if (type == PrimitiveTypeWrapper.BYTE)
                    args[i] = (byte)pArgs[i].b;
                else if (type == PrimitiveTypeWrapper.CHAR)
                    args[i] = (char)pArgs[i].c;
                else if (type == PrimitiveTypeWrapper.SHORT)
                    args[i] = pArgs[i].s;
                else if (type == PrimitiveTypeWrapper.INT)
                    args[i] = pArgs[i].i;
                else if (type == PrimitiveTypeWrapper.LONG)
                    args[i] = pArgs[i].j;
                else if (type == PrimitiveTypeWrapper.FLOAT)
                    args[i] = pArgs[i].f;
                else if (type == PrimitiveTypeWrapper.DOUBLE)
                    args[i] = pArgs[i].d;
                else
                    args[i] = argTypes[i].GhostWrap(UnwrapRef(env, pArgs[i].l));
            }

            // method requires caller
            if (mw.HasCallerID)
                args[args.Length - 1] = env.callerID;

            try
            {
                if (nonVirtual && mw.RequiresNonVirtualDispatcher)
                    return InvokeNonVirtual(env, mw, obj, args);

                if (mw.IsConstructor)
                {
                    if (obj == null)
                    {
                        return mw.CreateInstance(args);
                    }
                    else
                    {
                        var mb = mw.GetMethod();
                        if (mb.IsStatic)
                        {
                            // we're dealing with a constructor on a remapped type, if obj is supplied, it means
                            // that we should call the constructor on an already existing instance, but that isn't
                            // possible with remapped types
                            throw new NotSupportedException($"Remapped type {mw.DeclaringType.Name} doesn't support constructor invocation on an existing instance");
                        }
                        else if (!mb.DeclaringType.IsInstanceOfType(obj))
                        {
                            // we're trying to initialize an existing instance of a remapped type
                            throw new NotSupportedException($"Unable to partially construct object of type {obj.GetType().FullName} to type {mb.DeclaringType.FullName}");
                        }
                    }
                }

                return mw.Invoke(obj, args);
            }
            catch (Exception e)
            {
                SetPendingException(pEnv, ikvm.runtime.Util.mapException(e));
                return null;
            }
        }

        /// <summary>
        /// Invokes the given non-virtual method, by ID, on the given object handle. Accepts a pointer to a <see cref="jvalue"/> array of arguments.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="mw"></param>
        /// <param name="obj"></param>
        /// <param name="argarray"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        static object InvokeNonVirtual(ManagedJNIEnv env, MethodWrapper mw, object obj, object[] argarray)
        {
            if (mw.HasCallerID || mw.IsDynamicOnly)
                throw new NotSupportedException();

            if (mw.DeclaringType.IsRemapped && !mw.DeclaringType.TypeAsBaseType.IsInstanceOfType(obj))
                return mw.InvokeNonvirtualRemapped(obj, argarray);

            var del = (Delegate)Activator.CreateInstance(mw.GetDelegateType(), new object[] { obj, mw.GetMethod().MethodHandle.GetFunctionPointer() });
            try
            {
                return del.DynamicInvoke(argarray);
            }
            catch (TargetInvocationException e)
            {
                throw ikvm.runtime.Util.mapException(e.InnerException);
            }
        }

        public static jobject NewObjectA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
            if (wrapper.IsAbstract == false && wrapper.TypeAsBaseType.IsAbstract)
                return pEnv->MakeLocalRef(InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false)); // static newinstance helper method

            var obj = AllocObjectImpl(pEnv, wrapper);
            if (obj != IntPtr.Zero)
            {
                InvokeHelper(pEnv, obj, methodID, args, false);

                if (ExceptionCheck(pEnv) == JNI_TRUE)
                {
                    DeleteLocalRef(pEnv, obj);
                    obj = IntPtr.Zero;
                }
            }

            return obj;
        }

        internal static jclass GetObjectClass(JNIEnv* pEnv, jobject obj)
        {
            return pEnv->MakeLocalRef(IKVM.Java.Externs.ikvm.runtime.Util.getClassFromObject(pEnv->UnwrapRef(obj)));
        }

        internal static jboolean IsInstanceOf(JNIEnv* pEnv, jobject obj, jclass clazz)
        {
            // NOTE if clazz is an interface, this is still the right thing to do
            // (i.e. if the object implements the interface, we return true)
            var objClass = IKVM.Java.Externs.ikvm.runtime.Util.getClassFromObject(pEnv->UnwrapRef(obj));
            var w1 = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
            var w2 = TypeWrapper.FromClass(objClass);
            return w2.IsAssignableTo(w1) ? JNI_TRUE : JNI_FALSE;
        }

        static MethodWrapper GetMethodImpl(TypeWrapper tw, string name, string sig)
        {
            for (; ; )
            {
                var mw = tw.GetMethodWrapper(name, sig, true);
                if (mw == null || !mw.IsHideFromReflection)
                    return mw;

                tw = mw.DeclaringType.BaseTypeWrapper;
                if (tw == null)
                    return null;
            }
        }

        static void AppendInterfaces(List<TypeWrapper> list, IList<TypeWrapper> add)
        {
            foreach (var iface in add)
                if (list.Contains(iface) == false)
                    list.Add(iface);
        }

        static List<TypeWrapper> TransitiveInterfaces(TypeWrapper tw)
        {
            var list = new List<TypeWrapper>();

            // append interfaces from base type
            if (tw.BaseTypeWrapper != null)
                AppendInterfaces(list, TransitiveInterfaces(tw.BaseTypeWrapper));

            // append transitive interfaces of current type
            foreach (TypeWrapper iface in tw.Interfaces)
                AppendInterfaces(list, TransitiveInterfaces(iface));

            // append interfaces of current type
            AppendInterfaces(list, tw.Interfaces);

            return list;
        }

        static MethodWrapper GetInterfaceMethodImpl(TypeWrapper tw, string name, string sig)
        {
            foreach (var iface in TransitiveInterfaces(tw))
            {
                var mw = iface.GetMethodWrapper(name, sig, false);
                if (mw != null && !mw.IsHideFromReflection)
                    return mw;
            }

            return null;
        }

        static jmethodID FindMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig, bool isStatic)
        {
            try
            {
                var tw = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
                tw.Finish();

                // if name == NULL, the JDK returns the constructor
                var methodname = (IntPtr)name == IntPtr.Zero ? "<init>" : DecodeMUTF8Argument(name, nameof(name));
                var methodsig = DecodeMUTF8Argument(sig, nameof(sig));

                MethodWrapper mw = null;

                // don't allow dotted names!
                if (methodsig.IndexOf('.') < 0)
                {
                    methodsig = methodsig.Replace('/', '.');
                    if (methodname == "<init>" || methodname == "<clinit>")
                        mw = tw.GetMethodWrapper(methodname, methodsig, false);
                    else
                        mw = GetMethodImpl(tw, methodname, methodsig) ?? GetInterfaceMethodImpl(tw, methodname, methodsig);
                }
                if (mw != null && mw.IsStatic == isStatic)
                {
                    mw.Link();
                    return mw.Cookie;
                }

                SetPendingException(pEnv, new java.lang.NoSuchMethodError($"{methodname}{methodsig}"));
            }
            catch (RetargetableJavaException e)
            {
                SetPendingException(pEnv, e.ToJava());
            }
            catch (Exception e)
            {
                SetPendingException(pEnv, e);
            }

            return IntPtr.Zero;
        }

        internal static jmethodID GetMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
        {
            return FindMethodID(pEnv, clazz, name, sig, false);
        }

        internal static jobject CallObjectMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            return pEnv->MakeLocalRef(InvokeHelper(pEnv, obj, methodID, args, false));
        }

        internal static jboolean CallBooleanMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
                return ((bool)o) ? JNI_TRUE : JNI_FALSE;

            return JNI_FALSE;
        }

        internal static jbyte CallByteMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jbyte)(byte)o;
            }
            return 0;
        }

        internal static jchar CallCharMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jchar)(char)o;
            }
            return 0;
        }

        internal static jshort CallShortMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jshort)(short)o;
            }
            return 0;
        }

        internal static jint CallIntMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jint)(int)o;
            }
            return 0;
        }

        internal static jlong CallLongMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jlong)(long)o;
            }
            return 0;
        }

        internal static jfloat CallFloatMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jfloat)(float)o;
            }
            return 0;
        }

        internal static jdouble CallDoubleMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            object o = InvokeHelper(pEnv, obj, methodID, args, false);
            if (o != null)
            {
                return (jdouble)(double)o;
            }
            return 0;
        }

        internal static void CallVoidMethodA(JNIEnv* pEnv, jobject obj, jmethodID methodID, jvalue* args)
        {
            InvokeHelper(pEnv, obj, methodID, args, false);
        }

        internal static jobject CallNonvirtualObjectMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            return pEnv->MakeLocalRef(InvokeHelper(pEnv, obj, methodID, args, true));
        }

        internal static jboolean CallNonvirtualBooleanMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return ((bool)o) ? JNI_TRUE : JNI_FALSE;

            return JNI_FALSE;
        }

        internal static jbyte CallNonvirtualByteMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jbyte)(byte)o;

            return 0;
        }

        internal static jchar CallNonvirtualCharMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jchar)(char)o;

            return 0;
        }

        internal static jshort CallNonvirtualShortMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jshort)(short)o;

            return 0;
        }

        internal static jint CallNonvirtualIntMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jint)(int)o;

            return 0;
        }

        internal static jlong CallNonvirtualLongMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jlong)(long)o;

            return 0;
        }

        internal static jfloat CallNonvirtualFloatMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jfloat)(float)o;

            return 0;
        }

        internal static jdouble CallNonvirtualDoubleMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, obj, methodID, args, true);
            if (o != null)
                return (jdouble)(double)o;

            return 0;
        }

        internal static void CallNonvirtualVoidMethodA(JNIEnv* pEnv, jobject obj, jclass clazz, jmethodID methodID, jvalue* args)
        {
            InvokeHelper(pEnv, obj, methodID, args, true);
        }

        static FieldWrapper GetFieldImpl(TypeWrapper tw, string name, string sig)
        {
            for (; ; )
            {
                var fw = tw.GetFieldWrapper(name, sig);
                if (fw == null || !fw.IsHideFromReflection)
                    return fw;

                tw = fw.DeclaringType.BaseTypeWrapper;
                if (tw == null)
                    return null;
            }
        }

        static jfieldID FindFieldID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig, bool isStatic)
        {
            try
            {
                var n = DecodeMUTF8Argument(name, nameof(name));
                var s = DecodeMUTF8Argument(sig, nameof(sig));

                var tw = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
                tw.Finish();

                // don't allow dotted names!
                if (s.IndexOf('.') < 0)
                {
                    var fw = GetFieldImpl(tw, n, s.Replace('/', '.'));
                    if (fw != null)
                        if (fw.IsStatic == isStatic)
                            return fw.Cookie;
                }

                SetPendingException(pEnv, new java.lang.NoSuchFieldError($"{(isStatic ? "Static" : "Instance")} field '{n}' with signature '{s}' not found in class '{tw.Name}'"));
            }
            catch (RetargetableJavaException x)
            {
                SetPendingException(pEnv, x.ToJava());
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
            }

            return IntPtr.Zero;
        }

        internal static jfieldID GetFieldID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
        {
            return FindFieldID(pEnv, clazz, name, sig, false);
        }

        static global::sun.reflect.FieldAccessor GetFieldAccessor(jfieldID cookie)
        {
            return (sun.reflect.FieldAccessor)FieldWrapper.FromCookie(cookie).GetFieldAccessorJNI();
        }

        internal static jobject GetObjectField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return pEnv->MakeLocalRef(GetFieldAccessor(fieldID).get(pEnv->UnwrapRef(obj)));
        }

        internal static jboolean GetBooleanField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return GetFieldAccessor(fieldID).getBoolean(pEnv->UnwrapRef(obj)) ? JNI_TRUE : JNI_FALSE;
        }

        internal static jbyte GetByteField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jbyte)GetFieldAccessor(fieldID).getByte(pEnv->UnwrapRef(obj));
        }

        internal static jchar GetCharField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jchar)GetFieldAccessor(fieldID).getChar(pEnv->UnwrapRef(obj));
        }

        internal static jshort GetShortField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jshort)GetFieldAccessor(fieldID).getShort(pEnv->UnwrapRef(obj));
        }

        internal static jint GetIntField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jint)GetFieldAccessor(fieldID).getInt(pEnv->UnwrapRef(obj));
        }

        internal static jlong GetLongField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jlong)GetFieldAccessor(fieldID).getLong(pEnv->UnwrapRef(obj));
        }

        internal static jfloat GetFloatField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jfloat)GetFieldAccessor(fieldID).getFloat(pEnv->UnwrapRef(obj));
        }

        internal static jdouble GetDoubleField(JNIEnv* pEnv, jobject obj, jfieldID fieldID)
        {
            return (jdouble)GetFieldAccessor(fieldID).getDouble(pEnv->UnwrapRef(obj));
        }

        internal static void SetObjectField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jobject val)
        {
            GetFieldAccessor(fieldID).set(pEnv->UnwrapRef(obj), pEnv->UnwrapRef(val));
        }

        internal static void SetBooleanField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jboolean val)
        {
            GetFieldAccessor(fieldID).setBoolean(pEnv->UnwrapRef(obj), val != JNI_FALSE);
        }

        internal static void SetByteField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jbyte val)
        {
            GetFieldAccessor(fieldID).setByte(pEnv->UnwrapRef(obj), (byte)val);
        }

        internal static void SetCharField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jchar val)
        {
            GetFieldAccessor(fieldID).setChar(pEnv->UnwrapRef(obj), (char)val);
        }

        internal static void SetShortField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jshort val)
        {
            GetFieldAccessor(fieldID).setShort(pEnv->UnwrapRef(obj), (short)val);
        }

        internal static void SetIntField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jint val)
        {
            GetFieldAccessor(fieldID).setInt(pEnv->UnwrapRef(obj), (int)val);
        }

        internal static void SetLongField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jlong val)
        {
            GetFieldAccessor(fieldID).setLong(pEnv->UnwrapRef(obj), (long)val);
        }

        internal static void SetFloatField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jfloat val)
        {
            GetFieldAccessor(fieldID).setFloat(pEnv->UnwrapRef(obj), (float)val);
        }

        internal static void SetDoubleField(JNIEnv* pEnv, jobject obj, jfieldID fieldID, jdouble val)
        {
            GetFieldAccessor(fieldID).setDouble(pEnv->UnwrapRef(obj), (double)val);
        }

        internal static jmethodID GetStaticMethodID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
        {
            return FindMethodID(pEnv, clazz, name, sig, true);
        }

        internal static jobject CallStaticObjectMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            return pEnv->MakeLocalRef(InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false));
        }

        internal static jboolean CallStaticBooleanMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? ((bool)o) ? JNI_TRUE : JNI_FALSE : JNI_FALSE;
        }

        internal static jbyte CallStaticByteMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jbyte)(byte)o : (sbyte)0;
        }

        internal static jchar CallStaticCharMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jchar)(char)o : (ushort)0;
        }

        internal static jshort CallStaticShortMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jshort)(short)o : (short)0;
        }

        internal static jint CallStaticIntMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jint)(int)o : 0;
        }

        internal static jlong CallStaticLongMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jlong)(long)o : 0;
        }

        internal static jfloat CallStaticFloatMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jfloat)(float)o : 0;
        }

        internal static jdouble CallStaticDoubleMethodA(JNIEnv* pEnv, jclass clazz, jmethodID methodID, jvalue* args)
        {
            var o = InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
            return o != null ? (jdouble)(double)o : 0;
        }

        internal static void CallStaticVoidMethodA(JNIEnv* pEnv, jclass cls, jmethodID methodID, jvalue* args)
        {
            InvokeHelper(pEnv, IntPtr.Zero, methodID, args, false);
        }

        internal static jfieldID GetStaticFieldID(JNIEnv* pEnv, jclass clazz, byte* name, byte* sig)
        {
            return FindFieldID(pEnv, clazz, name, sig, true);
        }

        internal static jobject GetStaticObjectField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return pEnv->MakeLocalRef(GetFieldAccessor(fieldID).get(null));
        }

        internal static jboolean GetStaticBooleanField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return GetFieldAccessor(fieldID).getBoolean(null) ? JNI_TRUE : JNI_FALSE;
        }

        internal static jbyte GetStaticByteField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jbyte)GetFieldAccessor(fieldID).getByte(null);
        }

        internal static jchar GetStaticCharField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jchar)GetFieldAccessor(fieldID).getChar(null);
        }

        internal static jshort GetStaticShortField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jshort)GetFieldAccessor(fieldID).getShort(null);
        }

        internal static jint GetStaticIntField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jint)GetFieldAccessor(fieldID).getInt(null);
        }

        internal static jlong GetStaticLongField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jlong)GetFieldAccessor(fieldID).getLong(null);
        }

        internal static jfloat GetStaticFloatField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jfloat)GetFieldAccessor(fieldID).getFloat(null);
        }

        internal static jdouble GetStaticDoubleField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID)
        {
            return (jdouble)GetFieldAccessor(fieldID).getDouble(null);
        }

        internal static void SetStaticObjectField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jobject val)
        {
            GetFieldAccessor(fieldID).set(null, pEnv->UnwrapRef(val));
        }

        internal static void SetStaticBooleanField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jboolean val)
        {
            GetFieldAccessor(fieldID).setBoolean(null, val != JNI_FALSE);
        }

        internal static void SetStaticByteField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jbyte val)
        {
            GetFieldAccessor(fieldID).setByte(null, (byte)val);
        }

        internal static void SetStaticCharField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jchar val)
        {
            GetFieldAccessor(fieldID).setChar(null, (char)val);
        }

        internal static void SetStaticShortField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jshort val)
        {
            GetFieldAccessor(fieldID).setShort(null, (short)val);
        }

        internal static void SetStaticIntField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jint val)
        {
            GetFieldAccessor(fieldID).setInt(null, (int)val);
        }

        internal static void SetStaticLongField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jlong val)
        {
            GetFieldAccessor(fieldID).setLong(null, (long)val);
        }

        internal static void SetStaticFloatField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jfloat val)
        {
            GetFieldAccessor(fieldID).setFloat(null, (float)val);
        }

        internal static void SetStaticDoubleField(JNIEnv* pEnv, jclass clazz, jfieldID fieldID, jdouble val)
        {
            GetFieldAccessor(fieldID).setDouble(null, (double)val);
        }

        internal static jstring NewString(JNIEnv* pEnv, jchar* unicode, int len)
        {
            return pEnv->MakeLocalRef(new String((char*)unicode, 0, len));
        }

        internal static jint GetStringLength(JNIEnv* pEnv, jstring str)
        {
            return ((string)pEnv->UnwrapRef(str)).Length;
        }

        internal static jchar* GetStringChars(JNIEnv* pEnv, jstring str, jboolean* isCopy)
        {
            var s = (string)pEnv->UnwrapRef(str);
            if (isCopy != null)
                *isCopy = JNI_TRUE;

            return (jchar*)(void*)Marshal.StringToHGlobalUni(s);
        }

        internal static void ReleaseStringChars(JNIEnv* pEnv, jstring str, jchar* chars)
        {
            Marshal.FreeHGlobal((IntPtr)(void*)chars);
        }

        internal static jobject NewStringUTF(JNIEnv* pEnv, byte* bytes)
        {
            // the JNI spec does not explicitly allow a null pointer, but the JDK accepts it
            return bytes == null ? IntPtr.Zero : pEnv->MakeLocalRef(DecodeMUTF8Argument(bytes, nameof(bytes)));
        }

        internal static jint GetStringUTFLength(JNIEnv* pEnv, jstring str)
        {
            return MUTF8Encoding.MUTF8.GetByteCount((string)pEnv->UnwrapRef(str));
        }

        internal static byte* GetStringUTFChars(JNIEnv* pEnv, jstring @string, jboolean* isCopy)
        {
            var s = (string)pEnv->UnwrapRef(@string);
            var buf = (byte*)JNIMemory.Alloc(MUTF8Encoding.MUTF8.GetByteCount(s) + 1);
            int j = 0;

            for (int i = 0; i < s.Length; i++)
            {
                var ch = s[i];
                if ((ch != 0) && (ch <= 0x7F))
                {
                    buf[j++] = (byte)ch;
                }
                else if (ch <= 0x7FF)
                {
                    buf[j++] = (byte)((ch >> 6) | 0xC0);
                    buf[j++] = (byte)((ch & 0x3F) | 0x80);
                }
                else
                {
                    buf[j++] = (byte)((ch >> 12) | 0xE0);
                    buf[j++] = (byte)(((ch >> 6) & 0x3F) | 0x80);
                    buf[j++] = (byte)((ch & 0x3F) | 0x80);
                }
            }

            buf[j] = 0;

            if (isCopy != null)
                *isCopy = JNI_TRUE;

            return buf;
        }

        internal static void ReleaseStringUTFChars(JNIEnv* pEnv, jstring @string, byte* utf)
        {
            JNIMemory.Free((IntPtr)(void*)utf);
        }

        internal static jsize GetArrayLength(JNIEnv* pEnv, jarray array)
        {
            return ((Array)pEnv->UnwrapRef(array)).Length;
        }

        internal static jobjectArray NewObjectArray(JNIEnv* pEnv, jsize len, jclass clazz, jobject init)
        {
            try
            {
                // we want to support (non-primitive) value types so we can't cast to object[]
                Array array = Array.CreateInstance(TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz)).TypeAsArrayType, len);
                object o = pEnv->UnwrapRef(init);
                if (o != null)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        array.SetValue(o, i);
                    }
                }
                return pEnv->MakeLocalRef(array);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.NegativeArraySizeException());
                return IntPtr.Zero;
            }
            catch (Exception e)
            {
                SetPendingException(pEnv, e);
                return IntPtr.Zero;
            }
        }

        internal static jobject GetObjectArrayElement(JNIEnv* pEnv, jarray array, jsize index)
        {
            try
            {
                // we want to support (non-primitive) value types so we can't cast to object[]
                return pEnv->MakeLocalRef(((Array)pEnv->UnwrapRef(array)).GetValue(index));
            }
            catch (IndexOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
                return IntPtr.Zero;
            }
        }

        internal static void SetObjectArrayElement(JNIEnv* pEnv, jarray array, jsize index, jobject val)
        {
            try
            {
                // we want to support (non-primitive) value types so we can't cast to object[]
                ((Array)pEnv->UnwrapRef(array)).SetValue(pEnv->UnwrapRef(val), index);
            }
            catch (IndexOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static jbooleanArray NewBooleanArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new bool[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jbyteArray NewByteArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new byte[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jcharArray NewCharArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new char[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jshortArray NewShortArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new short[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jintArray NewIntArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new int[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jlongArray NewLongArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new long[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jfloatArray NewFloatArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new float[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jdoubleArray NewDoubleArray(JNIEnv* pEnv, jsize len)
        {
            try
            {
                return pEnv->MakeLocalRef(new double[len]);
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return IntPtr.Zero;
            }
        }

        internal static jboolean* GetBooleanArrayElements(JNIEnv* pEnv, jbooleanArray array, jboolean* isCopy)
        {
            var b = (bool[])pEnv->UnwrapRef(array);
            var p = (jboolean*)(void*)JNIMemory.Alloc(b.Length * 1);
            for (int i = 0; i < b.Length; i++)
                p[i] = b[i] ? JNI_TRUE : JNI_FALSE;

            if (isCopy != null)
                *isCopy = JNI_TRUE;

            return p;
        }

        internal static jbyte* GetByteArrayElements(JNIEnv* pEnv, jbyteArray array, jboolean* isCopy)
        {
            byte[] b = (byte[])pEnv->UnwrapRef(array);
            jbyte* p = (jbyte*)(void*)JNIMemory.Alloc(b.Length * 1);
            for (int i = 0; i < b.Length; i++)
                p[i] = (jbyte)b[i];

            if (isCopy != null)
                *isCopy = JNI_TRUE;

            return p;
        }

        internal static jchar* GetCharArrayElements(JNIEnv* pEnv, jcharArray array, jboolean* isCopy)
        {
            char[] b = (char[])pEnv->UnwrapRef(array);
            IntPtr buf = JNIMemory.Alloc(b.Length * 2);
            Marshal.Copy(b, 0, buf, b.Length);
            if (isCopy != null)
            {
                *isCopy = JNI_TRUE;
            }
            return (jchar*)(void*)buf;
        }

        internal static jshort* GetShortArrayElements(JNIEnv* pEnv, jshortArray array, jboolean* isCopy)
        {
            short[] b = (short[])pEnv->UnwrapRef(array);
            IntPtr buf = JNIMemory.Alloc(b.Length * 2);
            Marshal.Copy(b, 0, buf, b.Length);
            if (isCopy != null)
            {
                *isCopy = JNI_TRUE;
            }
            return (jshort*)(void*)buf;
        }

        internal static jint* GetIntArrayElements(JNIEnv* pEnv, jintArray array, jboolean* isCopy)
        {
            int[] b = (int[])pEnv->UnwrapRef(array);
            IntPtr buf = JNIMemory.Alloc(b.Length * 4);
            Marshal.Copy(b, 0, buf, b.Length);
            if (isCopy != null)
            {
                *isCopy = JNI_TRUE;
            }
            return (jint*)(void*)buf;
        }

        internal static jlong* GetLongArrayElements(JNIEnv* pEnv, jlongArray array, jboolean* isCopy)
        {
            long[] b = (long[])pEnv->UnwrapRef(array);
            IntPtr buf = JNIMemory.Alloc(b.Length * 8);
            Marshal.Copy(b, 0, buf, b.Length);
            if (isCopy != null)
            {
                *isCopy = JNI_TRUE;
            }
            return (jlong*)(void*)buf;
        }

        internal static jfloat* GetFloatArrayElements(JNIEnv* pEnv, jfloatArray array, jboolean* isCopy)
        {
            float[] b = (float[])pEnv->UnwrapRef(array);
            IntPtr buf = JNIMemory.Alloc(b.Length * 4);
            Marshal.Copy(b, 0, buf, b.Length);
            if (isCopy != null)
            {
                *isCopy = JNI_TRUE;
            }
            return (jfloat*)(void*)buf;
        }

        internal static jdouble* GetDoubleArrayElements(JNIEnv* pEnv, jdoubleArray array, jboolean* isCopy)
        {
            double[] b = (double[])pEnv->UnwrapRef(array);
            IntPtr buf = JNIMemory.Alloc(b.Length * 8);
            Marshal.Copy(b, 0, buf, b.Length);
            if (isCopy != null)
            {
                *isCopy = JNI_TRUE;
            }
            return (jdouble*)(void*)buf;
        }

        internal static void ReleaseBooleanArrayElements(JNIEnv* pEnv, jbooleanArray array, jboolean* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                bool[] b = (bool[])pEnv->UnwrapRef(array);
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = elems[i] != JNI_FALSE;
                }
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseByteArrayElements(JNIEnv* pEnv, jbyteArray array, jbyte* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                byte[] b = (byte[])pEnv->UnwrapRef(array);
                for (int i = 0; i < b.Length; i++)
                {
                    b[i] = (byte)elems[i];
                }
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseCharArrayElements(JNIEnv* pEnv, jcharArray array, jchar* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                char[] b = (char[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseShortArrayElements(JNIEnv* pEnv, jshortArray array, jshort* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                short[] b = (short[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseIntArrayElements(JNIEnv* pEnv, jintArray array, jint* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                int[] b = (int[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseLongArrayElements(JNIEnv* pEnv, jlongArray array, jlong* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                long[] b = (long[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseFloatArrayElements(JNIEnv* pEnv, jfloatArray array, jfloat* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                float[] b = (float[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void ReleaseDoubleArrayElements(JNIEnv* pEnv, jdoubleArray array, jdouble* elems, jint mode)
        {
            if (mode == 0 || mode == JNI_COMMIT)
            {
                double[] b = (double[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)(void*)elems, b, 0, b.Length);
            }
            if (mode == 0 || mode == JNI_ABORT)
            {
                JNIMemory.Free((IntPtr)(void*)elems);
            }
        }

        internal static void GetBooleanArrayRegion(JNIEnv* pEnv, jbooleanArray array, int start, int len, jboolean* buf)
        {
            try
            {
                bool[] b = (bool[])pEnv->UnwrapRef(array);
                sbyte* p = (sbyte*)(void*)buf;
                for (int i = 0; i < len; i++)
                {
                    *p++ = b[start + i] ? JNI_TRUE : JNI_FALSE;
                }
            }
            catch (IndexOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetByteArrayRegion(JNIEnv* pEnv, jbyteArray array, int start, int len, jbyte* buf)
        {
            try
            {
                byte[] b = (byte[])pEnv->UnwrapRef(array);
                byte* p = (byte*)(void*)buf;
                for (int i = 0; i < len; i++)
                {
                    *p++ = b[start + i];
                }
            }
            catch (IndexOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetCharArrayRegion(JNIEnv* pEnv, jcharArray array, int start, int len, jchar* buf)
        {
            try
            {
                char[] b = (char[])pEnv->UnwrapRef(array);
                Marshal.Copy(b, start, (IntPtr)buf, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetShortArrayRegion(JNIEnv* pEnv, jshortArray array, int start, int len, jshort* buf)
        {
            try
            {
                short[] b = (short[])pEnv->UnwrapRef(array);
                Marshal.Copy(b, start, (IntPtr)buf, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetIntArrayRegion(JNIEnv* pEnv, jintArray array, int start, int len, jint* buf)
        {
            try
            {
                int[] b = (int[])pEnv->UnwrapRef(array);
                Marshal.Copy(b, start, (IntPtr)buf, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetLongArrayRegion(JNIEnv* pEnv, jlongArray array, int start, int len, jlong* buf)
        {
            try
            {
                long[] b = (long[])pEnv->UnwrapRef(array);
                Marshal.Copy(b, start, (IntPtr)buf, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetFloatArrayRegion(JNIEnv* pEnv, jfloatArray array, int start, int len, jfloat* buf)
        {
            try
            {
                float[] b = (float[])pEnv->UnwrapRef(array);
                Marshal.Copy(b, start, (IntPtr)buf, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void GetDoubleArrayRegion(JNIEnv* pEnv, jdoubleArray array, int start, int len, jdouble* buf)
        {
            try
            {
                double[] b = (double[])pEnv->UnwrapRef(array);
                Marshal.Copy(b, start, (IntPtr)buf, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetBooleanArrayRegion(JNIEnv* pEnv, jbooleanArray array, int start, int len, jboolean* buf)
        {
            try
            {
                bool[] b = (bool[])pEnv->UnwrapRef(array);
                sbyte* p = (sbyte*)(void*)buf;
                for (int i = 0; i < len; i++)
                {
                    b[start + i] = *p++ != JNI_FALSE;
                }
            }
            catch (IndexOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetByteArrayRegion(JNIEnv* pEnv, jbyteArray array, int start, int len, jbyte* buf)
        {
            try
            {
                byte[] b = (byte[])pEnv->UnwrapRef(array);
                byte* p = (byte*)(void*)buf;
                for (int i = 0; i < len; i++)
                {
                    b[start + i] = *p++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetCharArrayRegion(JNIEnv* pEnv, jcharArray array, int start, int len, jchar* buf)
        {
            try
            {
                char[] b = (char[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)buf, b, start, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetShortArrayRegion(JNIEnv* pEnv, jshortArray array, int start, int len, jshort* buf)
        {
            try
            {
                short[] b = (short[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)buf, b, start, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetIntArrayRegion(JNIEnv* pEnv, jintArray array, int start, int len, jint* buf)
        {
            try
            {
                int[] b = (int[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)buf, b, start, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetLongArrayRegion(JNIEnv* pEnv, jlongArray array, int start, int len, jlong* buf)
        {
            try
            {
                long[] b = (long[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)buf, b, start, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetFloatArrayRegion(JNIEnv* pEnv, jfloatArray array, int start, int len, jfloat* buf)
        {
            try
            {
                float[] b = (float[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)buf, b, start, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        internal static void SetDoubleArrayRegion(JNIEnv* pEnv, jdoubleArray array, int start, int len, jdouble* buf)
        {
            try
            {
                double[] b = (double[])pEnv->UnwrapRef(array);
                Marshal.Copy((IntPtr)buf, b, start, len);
            }
            catch (ArgumentOutOfRangeException)
            {
                SetPendingException(pEnv, new java.lang.ArrayIndexOutOfBoundsException());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe internal struct JNINativeMethod
        {
            public byte* name;
            public byte* signature;
            public void* fnPtr;
        }

        internal static jint RegisterNatives(JNIEnv* pEnv, jclass clazz, JNINativeMethod* methods, jint nMethods)
        {
            try
            {
                TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
                wrapper.Finish();
                for (int i = 0; i < nMethods; i++)
                {
                    var methodName = DecodeMUTF8(methods[i].name);
                    var methodSig = DecodeMUTF8(methods[i].signature);

                    Tracer.Info(Tracer.Jni, "Registering native method: {0}.{1}{2}, fnPtr = 0x{3:X}", wrapper.Name, methodName, methodSig, ((IntPtr)methods[i].fnPtr).ToInt64());
                    FieldInfo fi = null;

                    // don't allow dotted names!
                    if (methodSig.IndexOf('.') < 0)
                        fi = wrapper.TypeAsTBD.GetField(JNIVM.METHOD_PTR_FIELD_PREFIX + methodName + methodSig, BindingFlags.Static | BindingFlags.NonPublic);

                    if (fi == null)
                    {
                        Tracer.Error(Tracer.Jni, "Failed to register native method: {0}.{1}{2}", wrapper.Name, methodName, methodSig);
                        throw new java.lang.NoSuchMethodError(methodName);
                    }

                    fi.SetValue(null, (IntPtr)methods[i].fnPtr);
                }
                return JNI_OK;
            }
            catch (RetargetableJavaException e)
            {
                SetPendingException(pEnv, e.ToJava());
                return JNI_ERR;
            }
            catch (Exception e)
            {
                SetPendingException(pEnv, e);
                return JNI_ERR;
            }
        }

        internal static jint UnregisterNatives(JNIEnv* pEnv, jclass clazz)
        {
            try
            {
                TypeWrapper wrapper = TypeWrapper.FromClass((java.lang.Class)pEnv->UnwrapRef(clazz));
                wrapper.Finish();
                // TODO this won't work when we're putting the JNI methods in jniproxy.dll
                foreach (FieldInfo fi in wrapper.TypeAsTBD.GetFields(BindingFlags.Static | BindingFlags.NonPublic))
                {
                    string name = fi.Name;
                    if (name.StartsWith(JNIVM.METHOD_PTR_FIELD_PREFIX))
                    {
                        Tracer.Info(Tracer.Jni, "Unregistering native method: {0}.{1}", wrapper.Name, name.Substring(JNIVM.METHOD_PTR_FIELD_PREFIX.Length));
                        fi.SetValue(null, IntPtr.Zero);
                    }
                }
                return JNI_OK;
            }
            catch (RetargetableJavaException x)
            {
                SetPendingException(pEnv, x.ToJava());
                return JNI_ERR;
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return JNI_ERR;
            }
        }

        internal static jint MonitorEnter(JNIEnv* pEnv, jobject obj)
        {
            try
            {
                // on .NET 4.0 Monitor.Enter has been marked obsolete,
                // but in this case the alternative adds no value
#pragma warning disable 618
                System.Threading.Monitor.Enter(pEnv->UnwrapRef(obj));
#pragma warning restore 618
                return JNI_OK;
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return JNI_ERR;
            }
        }

        internal static jint MonitorExit(JNIEnv* pEnv, jobject obj)
        {
            try
            {
                System.Threading.Monitor.Exit(pEnv->UnwrapRef(obj));
                return JNI_OK;
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, x);
                return JNI_ERR;
            }
        }

        internal static jint GetJavaVM(JNIEnv* pEnv, JavaVM** ppJavaVM)
        {
            *ppJavaVM = JavaVM.pJavaVM;
            return JNI_OK;
        }

        internal static void GetStringRegion(JNIEnv* pEnv, jstring str, jsize start, jsize len, jchar* buf)
        {
            string s = (string)pEnv->UnwrapRef(str);
            if (s != null)
            {
                if (start < 0 || start > s.Length || s.Length - start < len)
                {
                    SetPendingException(pEnv, new java.lang.StringIndexOutOfBoundsException());
                    return;
                }
                else
                {
                    char* p = (char*)(void*)buf;
                    // TODO isn't there a managed memcpy?
                    for (int i = 0; i < len; i++)
                    {
                        *p++ = s[start + i];
                    }
                    return;
                }
            }
            else
            {
                SetPendingException(pEnv, new java.lang.NullPointerException());
            }
        }

        internal static void GetStringUTFRegion(JNIEnv* pEnv, jstring str, jsize start, jsize len, byte* buf)
        {
            string s = (string)pEnv->UnwrapRef(str);
            if (s != null)
            {
                if (start < 0 || start > s.Length || s.Length - start < len)
                {
                    SetPendingException(pEnv, new java.lang.StringIndexOutOfBoundsException());
                    return;
                }
                else
                {
                    byte* p = (byte*)(void*)buf;
                    for (int i = 0; i < len; i++)
                    {
                        char ch = s[start + i];
                        if ((ch != 0) && (ch <= 0x7F))
                        {
                            *p++ = (byte)ch;
                        }
                        else if (ch <= 0x7FF)
                        {
                            *p++ = (byte)((ch >> 6) | 0xC0);
                            *p++ = (byte)((ch & 0x3F) | 0x80);
                        }
                        else
                        {
                            *p++ = (byte)((ch >> 12) | 0xE0);
                            *p++ = (byte)(((ch >> 6) & 0x3F) | 0x80);
                            *p++ = (byte)((ch & 0x3F) | 0x80);
                        }
                    }
                    return;
                }
            }
            else
            {
                SetPendingException(pEnv, new java.lang.NullPointerException());
            }
        }

        void* PinObject(object obj)
        {
            if (pinHandleInUseCount == pinHandleMaxCount)
            {
                var newCount = pinHandleMaxCount + 32;
                var pNew = (GCHandle*)JNIMemory.Alloc(sizeof(GCHandle) * newCount);
                for (int i = 0; i < pinHandleMaxCount; i++)
                    pNew[i] = pinHandles[i];

                for (int i = pinHandleMaxCount; i < newCount; i++)
                    pNew[i] = new GCHandle();

                JNIMemory.Free((nint)pinHandles);
                pinHandles = pNew;
                pinHandleMaxCount = newCount;
            }

            var index = pinHandleInUseCount++;
            if (pinHandles[index].IsAllocated == false)
                pinHandles[index] = GCHandle.Alloc(null, GCHandleType.Pinned);

            pinHandles[index].Target = obj;
            return (void*)pinHandles[index].AddrOfPinnedObject();
        }

        void UnpinObject(object obj)
        {
            for (int i = 0; i < pinHandleInUseCount; i++)
            {
                if (pinHandles[i].Target == obj)
                {
                    pinHandles[i].Target = pinHandles[--pinHandleInUseCount].Target;
                    pinHandles[pinHandleInUseCount].Target = null;
                    return;
                }
            }
        }

        internal static void* GetPrimitiveArrayCritical(JNIEnv* pEnv, jarray array, jboolean* isCopy)
        {
            if (isCopy != null)
                *isCopy = JNI_FALSE;

            return pEnv->PinObject(pEnv->UnwrapRef(array));
        }

        internal static void ReleasePrimitiveArrayCritical(JNIEnv* pEnv, jarray array, void* carray, jint mode)
        {
            pEnv->UnpinObject(pEnv->UnwrapRef(array));
        }

        /// <summary>
        /// Implements the JNI 'GetStringCritical' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="str"></param>
        /// <param name="isCopy"></param>
        /// <returns></returns>
        internal static jchar* GetStringCritical(JNIEnv* pEnv, jstring str, jboolean* isCopy)
        {
            if (isCopy != null)
                *isCopy = JNI_FALSE;

            return (jchar*)pEnv->PinObject(pEnv->UnwrapRef(str));
        }

        /// <summary>
        /// Implements the JNI 'ReleaseStringCritical' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="str"></param>
        /// <param name="cstring"></param>
        internal static void ReleaseStringCritical(JNIEnv* pEnv, jstring str, jchar* cstring)
        {
            pEnv->UnpinObject(pEnv->UnwrapRef(str));
        }

        /// <summary>
        /// Implements the JNI 'NewWeakGlobalRef' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static jweak NewWeakGlobalRef(JNIEnv* pEnv, jobject obj)
        {
            var o = pEnv->UnwrapRef(obj);
            return o == null ? IntPtr.Zero : JNIGlobalRefTable.AddWeakGlobalRef(o);
        }

        /// <summary>
        /// Implements the JNI 'DeleteWeakGlobalRef' function.
        /// </summary>
        /// <param name="pEnv"></param>
        /// <param name="obj"></param>
        internal static void DeleteWeakGlobalRef(JNIEnv* pEnv, jweak obj)
        {
            if (IsGlobalRef(obj))
            {
                JNIGlobalRefTable.DeleteWeakGlobalRef(obj);
                return;
            }

            if (IsLocalRef(obj))
                Debug.Assert(false, "Local ref passed to DeleteWeakGlobalRef");
        }

        internal static jboolean ExceptionCheck(JNIEnv* pEnv)
        {
            ManagedJNIEnv env = pEnv->GetManagedJNIEnv();
            return env.pendingException != null ? JNI_TRUE : JNI_FALSE;
        }

        internal static jobject NewDirectByteBuffer(JNIEnv* pEnv, IntPtr address, jlong capacity)
        {
            try
            {
                if (capacity < 0 || capacity > int.MaxValue)
                {
                    SetPendingException(pEnv, new java.lang.IllegalArgumentException("capacity"));
                    return IntPtr.Zero;
                }
                return pEnv->MakeLocalRef(JVM.NewDirectByteBuffer(address.ToInt64(), (int)capacity));
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
                return IntPtr.Zero;
            }
        }

        internal static void* GetDirectBufferAddress(JNIEnv* pEnv, jobject buf)
        {
            try
            {
                return (void*)(IntPtr)((sun.nio.ch.DirectBuffer)pEnv->UnwrapRef(buf)).address();
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
                return (void*)IntPtr.Zero;
            }
        }

        internal static jlong GetDirectBufferCapacity(JNIEnv* pEnv, jobject buf)
        {
            try
            {
                return (jlong)(long)((java.nio.Buffer)pEnv->UnwrapRef(buf)).capacity();
            }
            catch (Exception x)
            {
                SetPendingException(pEnv, ikvm.runtime.Util.mapException(x));
                return 0;
            }
        }

        internal static jobjectRefType GetObjectRefType(JNIEnv* pEnv, jobject obj)
        {
            int i = obj.ToInt32();
            if (i >= 0)
            {
                return jobjectRefType.JNILocalRefType;
            }
            i = -i;
            if ((i & (1 << 30)) != 0)
            {
                return jobjectRefType.JNIWeakGlobalRefType;
            }
            else
            {
                return jobjectRefType.JNIGlobalRefType;
            }
        }

        internal nint MakeLocalRef(object obj)
        {
            return GetManagedJNIEnv().MakeLocalRef(obj);
        }

        internal object UnwrapRef(nint o)
        {
            if (IsLocalRef(o))
                return GetManagedJNIEnv().UnwrapLocalRef(o);
            else if (IsGlobalRef(o))
                return JNIGlobalRefTable.Unwrap(o);
            return null;
        }

        internal static object UnwrapRef(ManagedJNIEnv env, nint o)
        {
            if (IsLocalRef(o))
                return env.UnwrapLocalRef(o);
            else if (IsGlobalRef(o))
                return JNIGlobalRefTable.Unwrap(o);
            else
                return null;
        }

        static bool IsLocalRef(nint o)
        {
            return o > 0;
        }

        static bool IsGlobalRef(nint o)
        {
            return o < 0;
        }

    }

}
