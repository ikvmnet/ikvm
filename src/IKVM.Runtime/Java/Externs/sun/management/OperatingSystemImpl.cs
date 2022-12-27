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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IKVM.Java.Externs.sun.management
{

    static class OperatingSystemImpl
    {

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESS_MEMORY_COUNTERS
        {

            public uint cb;
            public uint PageFaultCount;
            public UIntPtr PeakWorkingSetSize;
            public UIntPtr WorkingSetSize;
            public UIntPtr QuotaPeakPagedPoolUsage;
            public UIntPtr QuotaPagedPoolUsage;
            public UIntPtr QuotaPeakNonPagedPoolUsage;
            public UIntPtr QuotaNonPagedPoolUsage;
            public UIntPtr PagefileUsage;
            public UIntPtr PeakPagefileUsage;

        }

        struct MEMORYSTATUSEX
        {

            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct PERFORMANCE_INFORMATION
        {

            public uint cb;
            public UIntPtr CommitTotal;
            public UIntPtr CommitLimit;
            public UIntPtr CommitPeak;
            public UIntPtr PhysicalTotal;
            public UIntPtr PhysicalAvailable;
            public UIntPtr SystemCache;
            public UIntPtr KernelTotal;
            public UIntPtr KernelPaged;
            public UIntPtr KernelNonpaged;
            public UIntPtr PageSize;
            public uint HandleCount;
            public uint ProcessCount;
            public uint ThreadCount;

        }

        static OSPlatform platform;

        [DllImport("psapi", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetProcessMemoryInfo(IntPtr Process, out PROCESS_MEMORY_COUNTERS ppsmemCounters, uint cb);

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

        [DllImport("psapi", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPerformanceInfo(ref PERFORMANCE_INFORMATION pPerformanceInformation, uint size);

        /// <summary>
        /// Initializes the static information.
        /// </summary>
        public static void initialize()
        {
            // determine OS platform up front
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                platform = OSPlatform.Windows;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                platform = OSPlatform.Linux;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                platform = OSPlatform.OSX;
        }

        public static long getCommittedVirtualMemorySize0(object _this)
        {
            return Process.GetCurrentProcess().PagedMemorySize64;
        }

        public static long getTotalSwapSpaceSize(object _this)
        {
            if (platform == OSPlatform.Windows)
            {
                var r = -1L;
                var p = new MEMORYSTATUSEX();
                p.dwLength = (uint)Marshal.SizeOf(p);
                if (GlobalMemoryStatusEx(ref p))
                    r = (long)p.ullTotalPageFile;

                return r;
            }
            else
            {
                return -1;
            }
        }

        public static long getFreeSwapSpaceSize(object _this)
        {
            if (platform == OSPlatform.Windows)
            {
                var r = -1L;
                var p = new MEMORYSTATUSEX();
                p.dwLength = (uint)Marshal.SizeOf(p);
                if (GlobalMemoryStatusEx(ref p))
                    r = (long)p.ullAvailPageFile;

                return r;
            }
            else
            {
                return -1;
            }
        }

        public static long getFreePhysicalMemorySize(object _this)
        {
            if (platform == OSPlatform.Windows)
            {
                var m = -1L;
                var p = new PERFORMANCE_INFORMATION();
                if (GetPerformanceInfo(ref p, (uint)Marshal.SizeOf(p)))
                    m = (long)(ulong)p.PhysicalAvailable;

                return m;
            }
            else
            {
                return -1;
            }
        }

        public static long getTotalPhysicalMemorySize(object _this)
        {
            if (platform == OSPlatform.Windows)
            {
                var m = -1L;
                var p = new PERFORMANCE_INFORMATION();
                if (GetPerformanceInfo(ref p, (uint)Marshal.SizeOf(p)))
                    m = (long)(ulong)p.PhysicalTotal;

                return m;
            }
            else
            {
                return -1;
            }
        }

        public static long getProcessCpuTime(object _this)
        {
            return Process.GetCurrentProcess().TotalProcessorTime.Ticks * 100;
        }

        public static double getSystemCpuLoad(object _this)
        {
            return -1;
        }

        public static double getProcessCpuLoad(object _this)
        {
            return -1;
        }

    }

}