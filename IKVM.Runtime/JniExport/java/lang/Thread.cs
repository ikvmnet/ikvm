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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using IKVM.Internal;

namespace IKVM.Runtime.JniExport.java.lang
{

    static class Thread
    {

        private static readonly object mainThreadGroup;

#if !FIRST_PASS
        static Thread()
        {
            mainThreadGroup = new global::java.lang.ThreadGroup(global::java.lang.ThreadGroup.createRootGroup(), "main");
        }
#endif

        public static object getMainThreadGroup()
        {
            return mainThreadGroup;
        }

        // this is called from JniInterface.cs
        internal static void WaitUntilLastJniThread()
        {
#if !FIRST_PASS
            int count = global::java.lang.Thread.currentThread().isDaemon() ? 0 : 1;
            while (Interlocked.CompareExchange(ref global::java.lang.Thread.nonDaemonCount[0], 0, 0) > count)
            {
                global::System.Threading.Thread.Sleep(1);
            }
#endif
        }

        // this is called from JniInterface.cs
        internal static void AttachThreadFromJni(object threadGroup)
        {
#if !FIRST_PASS
            if (threadGroup == null)
            {
                threadGroup = mainThreadGroup;
            }
            if (global::java.lang.Thread.current == null)
            {
                new global::java.lang.Thread((global::java.lang.ThreadGroup)threadGroup);
            }
#endif
        }

        public static global::java.lang.StackTraceElement[] getStackTrace(StackTrace stack)
        {
#if FIRST_PASS
            return null;
#else
            List<global::java.lang.StackTraceElement> stackTrace = new List<global::java.lang.StackTraceElement>();
            ExceptionHelper.ExceptionInfoHelper.Append(stackTrace, stack, 0, true);
            return stackTrace.ToArray();
#endif
        }

        public static object getThreads()
        {
#if FIRST_PASS
            return null;
#else
            return global::java.security.AccessController.doPrivileged(global::ikvm.runtime.Delegates.toPrivilegedAction(delegate
            {
                global::java.lang.ThreadGroup root = (global::java.lang.ThreadGroup)mainThreadGroup;
                for (; ; )
                {
                    global::java.lang.Thread[] threads = new global::java.lang.Thread[root.activeCount()];
                    if (root.enumerate(threads) == threads.Length)
                    {
                        return threads;
                    }
                }
            }));
#endif
        }

    }

}
