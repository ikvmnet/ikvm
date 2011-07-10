/*
  Copyright (C) 2010 Jeroen Frijters

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

package com.sun.management;

import ikvm.internal.NotYetImplementedError;
import cli.System.Activator;
import cli.System.Diagnostics.Process;
import cli.System.Reflection.Assembly;
import cli.System.Reflection.PropertyInfo;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import cli.System.Runtime.InteropServices.LayoutKind;
import cli.System.Runtime.InteropServices.StructLayoutAttribute;
import cli.System.Security.SecuritySafeCriticalAttribute;
import cli.System.Type;
import sun.management.OperatingSystemImpl;
import sun.management.VMManagement;

class OperatingSystem extends OperatingSystemImpl implements OperatingSystemMXBean
{
    OperatingSystem(VMManagement vmm)
    {
        super(vmm);
    }

    public long getProcessCpuTime()
    {
        return Process.GetCurrentProcess().get_TotalProcessorTime().get_Ticks() * 100;
    }

    public long getCommittedVirtualMemorySize()
    {
        return Process.GetCurrentProcess().get_PagedMemorySize64();
    }

    public long getTotalPhysicalMemorySize()
    {
        long value = get("TotalPhysicalMemory");
        if (value != -1)
        {
            return value;
        }
        return GetMemoryStatusEx().ullTotalPhys;
    }

    public long getFreePhysicalMemorySize()
    {
        long value = get("AvailablePhysicalMemory");
        if (value != -1)
        {
            return value;
        }
        return GetMemoryStatusEx().ullAvailPhys;
    }
    
    public long getTotalSwapSpaceSize()
    {
        return GetMemoryStatusEx().ullTotalPageFile;
    }

    public long getFreeSwapSpaceSize()
    {
        return GetMemoryStatusEx().ullAvailPageFile;
    }
    
    public /*native*/ double getSystemCpuLoad()
    {
    	throw new NotYetImplementedError(); //TODO JDK7
    }
    
    public /*native*/ double getProcessCpuLoad()
    {
    	throw new NotYetImplementedError(); //TODO JDK7
    }

    private static long get(String propertyName)
    {
        Assembly asm = Assembly.LoadWithPartialName("Microsoft.VisualBasic");
        if (asm != null)
        {
            Type type = asm.GetType("Microsoft.VisualBasic.Devices.ComputerInfo");
            if (type != null)
            {
                PropertyInfo property = type.GetProperty(propertyName);
                if (property != null)
                {
                    Object obj = Activator.CreateInstance(type);
                    try
                    {
                        if (false) throw new cli.System.NotImplementedException();
                        return ikvm.lang.CIL.unbox_ulong((cli.System.UInt64)property.GetValue(obj, null));
                    }
                    catch (cli.System.NotImplementedException _)
                    {
                        // Mono doesn't implement this property
                    }
                }
            }
        }
        return -1;
    }
    
    @SecuritySafeCriticalAttribute.Annotation
    private static MEMORYSTATUSEX GetMemoryStatusEx()
    {
        if (ikvm.internal.Util.WINDOWS)
        {
            MEMORYSTATUSEX mem = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(mem))
            {
                return mem;
            }
        }
        throw new InternalError("Unsupported Platform");
    }

    @DllImportAttribute.Annotation("kernel32")    
    private static native boolean GlobalMemoryStatusEx(MEMORYSTATUSEX lpBuffer);
}

@StructLayoutAttribute.Annotation(LayoutKind.__Enum.Sequential)
final class MEMORYSTATUSEX extends cli.System.Object
{
    int dwLength = 64;
    int dwMemoryLoad;
    long ullTotalPhys;
    long ullAvailPhys;
    long ullTotalPageFile;
    long ullAvailPageFile;
    long ullTotalVirtual;
    long ullAvailVirtual;
    long ullAvailExtendedVirtual;
}
