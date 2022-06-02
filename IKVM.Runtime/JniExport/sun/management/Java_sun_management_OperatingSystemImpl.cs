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
#if !FIRST_PASS
#endif

static class Java_sun_management_OperatingSystemImpl
{

    static long getComputerInfo(string property)
    {
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
