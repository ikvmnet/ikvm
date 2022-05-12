/*
  Copyright (C) 2011-2014 Jeroen Frijters

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
#if !FIRST_PASS
using java.lang.management;
#endif

static class Java_sun_management_ClassLoadingImpl
{
	public static void setVerboseClass(bool value)
	{
	}
}

static class Java_sun_management_FileSystemImpl
{
    public static void init0()
	{
	}

	public static bool isSecuritySupported0(string path)
	{
		throw new NotSupportedException();
	}

	public static bool isAccessUserOnly0(string path)
	{
		throw new NotSupportedException();
	}
}

static class Java_sun_management_GcInfoBuilder
{
	public static int getNumGcExtAttributes(object _this, object gc)
	{
		throw new NotSupportedException();
	}

	public static void fillGcAttributeInfo(object _this, object gc, int numAttributes, string[] attributeNames, char[] types, string[] descriptions)
	{
		throw new NotSupportedException();
	}

	public static object getLastGcInfo0(object _this, object gc, int numExtAtts, object[] extAttValues, char[] extAttTypes, object[] before, object[] after)
	{
		throw new NotSupportedException();
	}
}

static class Java_sun_management_MemoryImpl
{
	public static object getMemoryPools0()
	{
#if FIRST_PASS
		return null;
#else
		return new MemoryPoolMXBean[0];
#endif
	}

	public static object getMemoryManagers0()
	{
#if FIRST_PASS
		return null;
#else
		return new MemoryManagerMXBean[0];
#endif
	}

	public static object getMemoryUsage0(object impl, bool heap)
	{
#if FIRST_PASS
		return null;
#else
		long mem = System.GC.GetTotalMemory(false);
		return new MemoryUsage(-1, mem, mem, -1);
#endif
	}

	public static void setVerboseGC(object impl, bool value)
	{
	}
}

static class Java_sun_management_OperatingSystemImpl
{
	private static long getComputerInfo(string property){
#pragma warning disable 618
        Assembly asm = Assembly.LoadWithPartialName("Microsoft.VisualBasic");
#pragma warning restore 618
        if (asm != null)
        {
            Type type = asm.GetType("Microsoft.VisualBasic.Devices.ComputerInfo");
            if (type != null)
            {
                try
                {
                    ulong result = (ulong)type.GetProperty(property).GetValue(Activator.CreateInstance(type), null);
                    return (long)result;
                }
                catch (TargetInvocationException)
                {
                    // Mono does not implement this property
                }
            }
        }
		throw new System.NotImplementedException();
	}

	public static long getCommittedVirtualMemorySize0(object _this)
	{
		throw new System.NotImplementedException();
	}

	public static long getTotalSwapSpaceSize(object _this)
	{
		throw new System.NotImplementedException();
	}

	public static long getFreeSwapSpaceSize(object _this)
	{
		throw new System.NotImplementedException();
	}

	public static long getProcessCpuTime(object _this)
	{
		throw new System.NotImplementedException();
	}

	public static long getFreePhysicalMemorySize(object _this)
	{
		return getComputerInfo("AvailablePhysicalMemory");
	}

	public static long getTotalPhysicalMemorySize(object _this)
	{
	    return getComputerInfo("TotalPhysicalMemory");
	}

	public static double getSystemCpuLoad(object _this)
	{
		throw new System.NotImplementedException();
	}

	public static double getProcessCpuLoad(object _this)
	{
		throw new System.NotImplementedException();
	}

	public static void initialize()
	{
	}
}

static class Java_sun_management_ThreadImpl
{
	public static object getThreads()
	{
        return Java_java_lang_Thread.getThreads();
	}

    private const int JVMTI_THREAD_STATE_ALIVE = 0x0001;
    private const int JVMTI_THREAD_STATE_TERMINATED = 0x0002;
    private const int JVMTI_THREAD_STATE_RUNNABLE = 0x0004;
    private const int JVMTI_THREAD_STATE_BLOCKED_ON_MONITOR_ENTER = 0x0400;
    private const int JVMTI_THREAD_STATE_WAITING_INDEFINITELY = 0x0010;
    private const int JVMTI_THREAD_STATE_WAITING_WITH_TIMEOUT = 0x0020;
    private const int JMM_THREAD_STATE_FLAG_SUSPENDED = 0x00100000;
    private const int JMM_THREAD_STATE_FLAG_NATIVE = 0x00400000;

	public static void getThreadInfo1(long[] ids, int maxDepth, object result)
	{
#if !FIRST_PASS

        System.Reflection.ConstructorInfo[] constructors = typeof(java.lang.management.ThreadInfo).GetConstructors( System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        foreach (System.Reflection.ConstructorInfo constructor in constructors)
        {
            if (constructor.GetParameters().Length == 9)
            {
                java.lang.Thread[] threads = (java.lang.Thread[])getThreads();
                java.lang.management.ThreadInfo[] threadInfos = (java.lang.management.ThreadInfo[])result;
                for (int i = 0; i < ids.Length; i++)
                {
                    long id = ids[i];
                    for (int t = 0; t < threads.Length; t++)
                    {
                        if (threads[t].getId() == id)
                        {
                            java.lang.Thread thread = threads[t];

                            int state;
                            // invers to sun.misc.VM.toThreadState
                            switch(thread.getState().ordinal())
                            {
                                case (int)java.lang.Thread.State.__Enum.RUNNABLE:
                                    state = JVMTI_THREAD_STATE_RUNNABLE;
                                    break;
                                case (int)java.lang.Thread.State.__Enum.BLOCKED:
                                    state = JVMTI_THREAD_STATE_BLOCKED_ON_MONITOR_ENTER;
                                    break;
                                case (int)java.lang.Thread.State.__Enum.WAITING:
                                    state = JVMTI_THREAD_STATE_WAITING_INDEFINITELY;
                                    break;
                                case (int)java.lang.Thread.State.__Enum.TIMED_WAITING:
                                    state = JVMTI_THREAD_STATE_WAITING_WITH_TIMEOUT;
                                    break;
                                case (int)java.lang.Thread.State.__Enum.TERMINATED:
                                    state = JVMTI_THREAD_STATE_TERMINATED;
                                    break;
                                case (int)java.lang.Thread.State.__Enum.NEW:
                                    state = JVMTI_THREAD_STATE_ALIVE;
                                    break;
                                default:
                                    state = 0;
                                    break;
                            }
                            //TODO set in state JMM_THREAD_STATE_FLAG_SUSPENDED if the thread is suspended

                            java.lang.StackTraceElement[] stacktrace = thread.getStackTrace();
                            if (maxDepth >= 0 && maxDepth < stacktrace.Length)
                            {
                                java.lang.StackTraceElement[] temp = new java.lang.StackTraceElement[maxDepth];
                                System.Array.Copy(stacktrace, temp, temp.Length);
                                stacktrace = temp;
                            }

                            object[] parameters = new object[9];
                            parameters[0] = thread;                     // thread
                            parameters[1] = state;                      // state
                                                                        // lockObj
                                                                        // lockOwner
                            parameters[4] = 0;                          // blockedCount
                            parameters[5] = 0;                          // blockedTime
                            parameters[6] = -1;                         // waitedCount
                            parameters[7] = 0;                          // waitedTime
                            parameters[8] = stacktrace;                 // stackTrace
                            threadInfos[i] = (java.lang.management.ThreadInfo)constructor.Invoke(parameters);
                            break;
                        }
                    }
                }
                return;
            }
        }
        throw new java.lang.InternalError("Constructor for java.lang.management.ThreadInfo not find.");
#endif
    }

	private static int GetCurrentThreadId()
	{
#pragma warning disable 618
		// On the CLR and Mono on Windows this is the (obsolete) equivalent of kernel32!GetCurrentThreadId
		return System.AppDomain.GetCurrentThreadId();
#pragma warning restore 618
	}

	public static long getThreadTotalCpuTime0(long id)
	{
        if (id == 0) {
            int currentId = GetCurrentThreadId();
            System.Diagnostics.ProcessThreadCollection threads = System.Diagnostics.Process.GetCurrentProcess().Threads;
            foreach (System.Diagnostics.ProcessThread t in threads) {
                if (t.Id == currentId) {
                    return (long)(t.TotalProcessorTime.Ticks * 100);
                }
            }
            return 0;
        } else {
            throw new System.NotImplementedException("Only current Thread is supported.");
        }
	}

	public static void getThreadTotalCpuTime1(long[] ids, long[] result)
	{
		throw new System.NotImplementedException();
	}

	public static long getThreadUserCpuTime0(long id)
	{
		throw new System.NotImplementedException();
	}

	public static void getThreadUserCpuTime1(long[] ids, long[] result)
	{
		throw new System.NotImplementedException();
	}

	public static void getThreadAllocatedMemory1(long[] ids, long[] result)
	{
		throw new System.NotImplementedException();
	}

	public static void setThreadCpuTimeEnabled0(bool enable)
	{
		//ignoring, we need nothing to enable
	}

	public static void setThreadAllocatedMemoryEnabled0(bool enable)
	{
		throw new System.NotImplementedException();
	}

	public static void setThreadContentionMonitoringEnabled0(bool enable)
	{
		throw new System.NotImplementedException();
	}

	public static object findMonitorDeadlockedThreads0()
	{
		throw new System.NotImplementedException();
	}

	public static object findDeadlockedThreads0()
	{
		throw new System.NotImplementedException();
	}

	public static void resetPeakThreadCount0()
	{
		throw new System.NotImplementedException();
	}

	public static object dumpThreads0(long[] ids, bool lockedMonitors, bool lockedSynchronizers)
	{
		throw new System.NotImplementedException();
	}

	public static void resetContentionTimes0(long tid)
	{
		throw new System.NotImplementedException();
	}
}
