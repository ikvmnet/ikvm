using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Mono.Unix.Native;

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
        /// Describes the Linux 'sysinfo' structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        unsafe struct sysinfo_t_x64
        {

            /// <summary>
            /// Seconds since boot
            /// </summary>
            public long uptime;

            /// <summary>
            /// 1, 5, and 15 minute load averages
            /// </summary>
            public fixed ulong loads[3];

            /// <summary>
            /// Total usable main memory size
            /// </summary>
            public ulong totalram;

            /// <summary>
            /// Available memory size
            /// </summary>
            public ulong freeram;

            /// <summary>
            /// Amount of shared memory
            /// </summary>
            public ulong sharedram;

            /// <summary>
            /// Memory used by buffers
            /// </summary>
            public ulong bufferram;

            /// <summary>
            /// Total swap space size.
            /// </summary>
            public uint totalswap;

            /// <summary>
            /// swap space still available
            /// </summary>
            public uint freeswap;

            /// <summary>
            /// Number of current processes
            /// </summary>
            public ushort procs;

            /// <summary>
            /// Explicit padding for m68k.
            /// </summary>
            public ushort pad;

            /// <summary>
            /// Total high memory size.
            /// </summary>
            public ulong totalhigh;

            /// <summary>
            /// Available high memory size.
            /// </summary>
            public ulong freehigh;

            /// <summary>
            /// Memory unit size in bytes.
            /// </summary>
            public uint mem_unit;

        }

        /// <summary>
        /// Invokes the native linux method 'sysinfo' for a x64 platform.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("lib6", EntryPoint = "sysinfo")]
        static extern int sysinfo_x64(ref sysinfo_t_x64 info);

        /// <summary>
        /// Initializes the static information.
        /// </summary>
        public static void initialize()
        {

        }

        /// <summary>
        /// Implements the native method 'getCommittedVirtualMemorySize0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getCommittedVirtualMemorySize0(object self)
        {
            return Process.GetCurrentProcess().PagedMemorySize64;
        }

        /// <summary>
        /// Implements the native method 'getTotalSwapSpaceSize'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.InternalError"></exception>
        public static long getTotalSwapSpaceSize(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var r = -1L;
                var p = new MEMORYSTATUSEX();
                p.dwLength = (uint)Marshal.SizeOf(p);
                if (GlobalMemoryStatusEx(ref p))
                    r = (long)p.ullTotalPageFile;

                return r;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && IntPtr.Size == 8)
            {
                var si = new sysinfo_t_x64();
                if (sysinfo_x64(ref si) != 0)
                    throw new global::java.lang.InternalError("sysinfo failed to get swap size");

                return (long)si.totalswap * si.mem_unit;
            }
            else
            {
                return -1;
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getFreeSwapSpaceSize'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.InternalError"></exception>
        public static long getFreeSwapSpaceSize(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var r = -1L;
                var p = new MEMORYSTATUSEX();
                p.dwLength = (uint)Marshal.SizeOf(p);
                if (GlobalMemoryStatusEx(ref p))
                    r = (long)p.ullAvailPageFile;

                return r;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && IntPtr.Size == 8)
            {
                var si = new sysinfo_t_x64();
                if (sysinfo_x64(ref si) != 0)
                    throw new global::java.lang.InternalError("sysinfo failed to get swap size");

                return (long)si.freeswap * si.mem_unit;
            }
            else
            {
                return -1;
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getFreePhysicalMemorySize'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getFreePhysicalMemorySize(object self)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var m = -1L;
                var p = new PERFORMANCE_INFORMATION();
                if (GetPerformanceInfo(ref p, (uint)Marshal.SizeOf(p)))
                    m = (long)(ulong)p.PhysicalAvailable;

                return m;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Syscall.sysconf(SysconfName._SC_AVPHYS_PAGES) * Syscall.sysconf(SysconfName._SC_PAGESIZE);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Implements the native method 'getTotalPhysicalMemorySize'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getTotalPhysicalMemorySize(object self)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var m = -1L;
                var p = new PERFORMANCE_INFORMATION();
                if (GetPerformanceInfo(ref p, (uint)Marshal.SizeOf(p)))
                    m = (long)(ulong)p.PhysicalTotal;

                return m;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Syscall.sysconf(SysconfName._SC_PHYS_PAGES);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Implements the native method 'getProcessCpuTime'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getProcessCpuTime(object self)
        {
            return Process.GetCurrentProcess().TotalProcessorTime.Ticks * 100;
        }

        /// <summary>
        /// Implements the native method 'getSystemCpuLoad'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double getSystemCpuLoad(object self)
        {
            return -1;
        }

        /// <summary>
        /// Implements the native method 'getProcessCpuLoad'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static double getProcessCpuLoad(object self)
        {
            return -1;
        }

    }

}