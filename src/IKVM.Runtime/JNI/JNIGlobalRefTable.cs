using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI
{

    /// <summary>
    /// Maintains a set of global refereces to instances.
    /// </summary>
    static class JNIGlobalRefTable
    {

        readonly static List<object> globalRefs = new List<object>();
        internal static readonly object weakRefLock = new object();
        internal static GCHandle[] weakRefs = new GCHandle[16];

        /// <summary>
        /// Returns the underlying object instance for the specified global reference.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static object Unwrap(nint z)
        {
            var i = -(int)z;
            if ((i & (1 << 30)) != 0)
                lock (weakRefLock)
                    return weakRefs[i - (1 << 30)].Target;
            else
                lock (globalRefs)
                    return globalRefs[i - 1];
        }

        /// <summary>
        /// Adds a new global ref to the table.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static nint AddGlobalRef(object o)
        {
            lock (globalRefs)
            {
                int index = globalRefs.IndexOf(null);
                if (index >= 0)
                {
                    globalRefs[index] = o;
                }
                else
                {
                    index = globalRefs.Count;
                    globalRefs.Add(o);
                }

                return -(index + 1);
            }
        }

        /// <summary>
        /// Deletes a global ref from the table.
        /// </summary>
        /// <param name="o"></param>
        public static void DeleteGlobalRef(nint o)
        {
            lock (globalRefs)
                globalRefs[(-(int)o) - 1] = null;
        }

        /// <summary>
        /// Adds a new global ref to the table.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static nint AddWeakGlobalRef(object o)
        {
            lock (weakRefLock)
            {
                for (int i = 0; i < weakRefs.Length; i++)
                {
                    if (weakRefs[i].IsAllocated == false)
                    {
                        weakRefs[i] = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);
                        return -(i | (1 << 30));
                    }
                }

                var len = weakRefs.Length;
                var tmp = new GCHandle[len * 2];
                Array.Copy(weakRefs, 0, tmp, 0, len);
                tmp[len] = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);
                weakRefs = tmp;
                return -(len | (1 << 30));
            }
        }


        /// <summary>
        /// Deletes a weak global ref from the table.
        /// </summary>
        /// <param name="o"></param>
        public static void DeleteWeakGlobalRef(nint o)
        {
            lock (weakRefLock)
                weakRefs[-o - (1 << 30)].Free();
        }

    }

}
