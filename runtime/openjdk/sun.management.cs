/*
  Copyright (C) 2011 Jeroen Frijters

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
#if !FIRST_PASS
using java.lang.management;
#endif

static class Java_sun_management_ClassLoadingImpl
{
	public static void setVerboseClass(bool value)
	{
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

static class Java_sun_management_ThreadImpl
{
	public static object getThreads()
	{
		throw new System.NotImplementedException();
	}

	public static void getThreadInfo1(long[] ids, int maxDepth, object result)
	{
		throw new System.NotImplementedException();
	}

	public static long getThreadTotalCpuTime0(long id)
	{
		throw new System.NotImplementedException();
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
		throw new System.NotImplementedException();
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
