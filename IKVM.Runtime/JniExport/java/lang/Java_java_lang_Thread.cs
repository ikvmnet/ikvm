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

static class Java_java_lang_Thread
{
    private static readonly object mainThreadGroup;

#if !FIRST_PASS
	static Java_java_lang_Thread()
	{
		mainThreadGroup = new java.lang.ThreadGroup(java.lang.ThreadGroup.createRootGroup(), "main");
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
		int count = java.lang.Thread.currentThread().isDaemon() ? 0 : 1;
		while (Interlocked.CompareExchange(ref java.lang.Thread.nonDaemonCount[0], 0, 0) > count)
		{
			Thread.Sleep(1);
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
		if (java.lang.Thread.current == null)
		{
			new java.lang.Thread((java.lang.ThreadGroup)threadGroup);
		}
#endif
    }

    public static java.lang.StackTraceElement[] getStackTrace(StackTrace stack)
    {
#if FIRST_PASS
        return null;
#else
		List<java.lang.StackTraceElement> stackTrace = new List<java.lang.StackTraceElement>();
		ExceptionHelper.ExceptionInfoHelper.Append(stackTrace, stack, 0, true);
		return stackTrace.ToArray();
#endif
    }

    public static object getThreads()
    {
#if FIRST_PASS
        return null;
#else
		return java.security.AccessController.doPrivileged(ikvm.runtime.Delegates.toPrivilegedAction(delegate
		{
			java.lang.ThreadGroup root = (java.lang.ThreadGroup)mainThreadGroup;
			for (; ; )
			{
				java.lang.Thread[] threads = new java.lang.Thread[root.activeCount()];
				if (root.enumerate(threads) == threads.Length)
				{
					return threads;
				}
			}
		}));
#endif
    }
}
