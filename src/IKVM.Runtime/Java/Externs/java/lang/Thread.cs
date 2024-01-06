/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Util.Java.Security;

namespace IKVM.Java.Externs.java.lang
{

    /// <summary>
    /// Implements the native mthods of 'java.lang.Thread'.
    /// </summary>
    static class Thread
    {

#if FIRST_PASS == false

        static AccessControllerAccessor accessControllerAccessor;
        static ThreadAccessor threadAccessor;

        static AccessControllerAccessor AccessControllerAccessor => JVM.Internal.BaseAccessors.Get(ref accessControllerAccessor);

        static ThreadAccessor ThreadAccessor => JVM.Internal.BaseAccessors.Get(ref threadAccessor);

#endif

        /// <summary>
        /// Implements the native method 'getMainThreadGroup'.
        /// </summary>
        /// <returns></returns>
        public static object getMainThreadGroup()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.MainThreadGroup;
#endif
        }

        // this is called from JniInterface.cs
        internal static void WaitUntilLastJniThread()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            int count = ThreadAccessor.InvokeIsDaemon(ThreadAccessor.InvokeCurrentThread()) ? 0 : 1;
            while (Interlocked.CompareExchange(ref global::java.lang.Thread.nonDaemonCount[0], 0, 0) > count)
                global::System.Threading.Thread.Sleep(1);
#endif
        }

        // this is called from JniInterface.cs
        internal static void AttachThreadFromJni(object threadGroup)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (ThreadAccessor.GetCurrent() == null)
                ThreadAccessor.Init(threadGroup ?? JVM.MainThreadGroup);
#endif
        }

        /// <summary>
        /// Implements the native method 'getStackTrace'.
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public static global::java.lang.StackTraceElement[] getStackTrace(StackTrace stack)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stackTrace = new List<global::java.lang.StackTraceElement>();
            ExceptionHelper.ExceptionInfoHelper.Append(JVM.Context.ExceptionHelper, stackTrace, stack, 0, true);
            return stackTrace.ToArray();
#endif
        }

        /// <summary>
        /// Implements the native method 'getThreads'.
        /// </summary>
        /// <returns></returns>
        public static object getThreads()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return AccessControllerAccessor.InvokeDoPrivileged(new FuncPrivilegedAction<object[]>(() =>
            {
                var root = (global::java.lang.ThreadGroup)JVM.MainThreadGroup;
                while (true)
                {
                    var threads = new global::java.lang.Thread[root.activeCount()];
                    if (root.enumerate(threads) == threads.Length)
                        return threads;
                }
            }));
#endif
        }

    }

}
