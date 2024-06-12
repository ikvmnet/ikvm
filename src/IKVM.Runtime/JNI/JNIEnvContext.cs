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
using System.Diagnostics;

namespace IKVM.Runtime.JNI
{
    using jboolean = System.SByte;
    using jint = System.Int32;
    using jobject = System.IntPtr;

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    /// <summary>
    /// Associated class that holds JNIEnv information for the current thread.
    /// </summary>
    internal sealed unsafe class JNIEnvContext
    {

        [ThreadStatic]
        static JNIEnvContext current;

        /// <summary>
        /// Gets the current <see cref="JNIEnvContext"/> for this thread.
        /// </summary>
        internal static JNIEnvContext Current
        {
            get => current;
            set => current = value;
        }

        internal const int JNI_OK = 0;
        internal const int JNI_ERR = -1;
        internal const int JNI_EDETACHED = -2;
        internal const int JNI_EVERSION = -3;
        internal const int JNI_COMMIT = 1;
        internal const int JNI_ABORT = 2;
        internal const jboolean JNI_TRUE = 1;
        internal const jboolean JNI_FALSE = 0;

        // NOTE the initial bucket size must be a power of two < LOCAL_REF_MAX_BUCKET_SIZE,
        // because each time we grow it, we double the size and it must eventually reach
        // exactly LOCAL_REF_MAX_BUCKET_SIZE
        const int LOCAL_REF_INITIAL_BUCKET_SIZE = 32;
        const int LOCAL_REF_SHIFT = 10;
        const int LOCAL_REF_MAX_BUCKET_SIZE = (1 << LOCAL_REF_SHIFT);
        const int LOCAL_REF_MASK = (LOCAL_REF_MAX_BUCKET_SIZE - 1);

        internal readonly RuntimeContext context;
        internal readonly JNIEnv* pJNIEnv;
        internal RuntimeClassLoader classLoader;
        internal ikvm.@internal.CallerID callerID;

        object[][] localRefs;
        int localRefSlot;
        int localRefIndex;
        object[] active;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal JNIEnvContext(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            pJNIEnv = (JNIEnv*)JNIMemory.Alloc(sizeof(JNIEnv));
            localRefs = new object[32][];
            active = localRefs[0] = new object[LOCAL_REF_INITIAL_BUCKET_SIZE];
            // stuff something in the first entry to make sure we don't hand out a zero handle
            // (a zero handle corresponds to a null reference)
            active[0] = "";
            localRefIndex = 1;
        }

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~JNIEnvContext()
        {
            if (Environment.HasShutdownStarted == false)
            {
                if (pJNIEnv->context.IsAllocated)
                    pJNIEnv->context.Free();

                for (int i = 0; i < pJNIEnv->pinHandleMaxCount; i++)
                    if (pJNIEnv->pinHandles[i].IsAllocated)
                        pJNIEnv->pinHandles[i].Free();

                JNIMemory.Free((nint)(void*)pJNIEnv);
            }
        }

        internal readonly struct FrameState
        {

            internal readonly ikvm.@internal.CallerID callerID;
            internal readonly int localRefSlot;
            internal readonly int localRefIndex;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="callerID"></param>
            /// <param name="localRefSlot"></param>
            /// <param name="localRefIndex"></param>
            internal FrameState(ikvm.@internal.CallerID callerID, int localRefSlot, int localRefIndex)
            {
                this.callerID = callerID;
                this.localRefSlot = localRefSlot;
                this.localRefIndex = localRefIndex;
            }

        }

        internal FrameState Enter(ikvm.@internal.CallerID newCallerID)
        {
            var prev = new FrameState(callerID, localRefSlot, localRefIndex);
            callerID = newCallerID;
            localRefSlot++;
            if (localRefSlot >= localRefs.Length)
            {
                var tmp = new object[localRefs.Length * 2][];
                Array.Copy(localRefs, 0, tmp, 0, localRefs.Length);
                localRefs = tmp;
            }

            localRefIndex = 0;
            active = localRefs[localRefSlot];
            active ??= localRefs[localRefSlot] = new object[LOCAL_REF_INITIAL_BUCKET_SIZE];

            return prev;
        }

        internal void Leave(FrameState prev)
        {
            for (int i = 0; i < localRefIndex; i++)
                active[i] = null;

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
            localRefIndex = prev.localRefIndex;
            callerID = prev.callerID;
        }

        internal nint MakeLocalRef(object obj)
        {
            if (obj == null)
                return 0;

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
            return (nint)((localRefSlot << LOCAL_REF_SHIFT) + index);
        }

        int FindFreeIndex()
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

        void GrowActiveSlot()
        {
            if (active.Length < LOCAL_REF_MAX_BUCKET_SIZE)
            {
                var tmp = new object[active.Length * 2];
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
                var tmp = new object[localRefSlot * 2][];
                Array.Copy(localRefs, 0, tmp, 0, localRefSlot);
                localRefs = tmp;
            }

            active = localRefs[localRefSlot];
            active ??= localRefs[localRefSlot] = new object[LOCAL_REF_MAX_BUCKET_SIZE];
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
                var tmp = new object[localRefs.Length * 2][];
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
                    r *= 2;

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

#endif

}
