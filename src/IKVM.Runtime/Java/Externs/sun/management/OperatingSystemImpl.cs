using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
            /// Seconds since boot.
            /// </summary>
            public long uptime;

            /// <summary>
            /// 1, 5, and 15 minute load averages.
            /// </summary>
            public fixed ulong loads[3];

            /// <summary>
            /// Total usable main memory size.
            /// </summary>
            public ulong totalram;

            /// <summary>
            /// Available memory size.
            /// </summary>
            public ulong freeram;

            /// <summary>
            /// Amount of shared memory.
            /// </summary>
            public ulong sharedram;

            /// <summary>
            /// Memory used by buffers.
            /// </summary>
            public ulong bufferram;

            /// <summary>
            /// Total swap space size.
            /// </summary>
            public ulong totalswap;

            /// <summary>
            /// swap space still available.
            /// </summary>
            public ulong freeswap;

            /// <summary>
            /// Number of current processes.
            /// </summary>
            public ushort procs;

            /// <summary>
            /// Number of current processes.
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

            /// <summary>po;llokp
            /// Memory unit size in bytes.
            /// </summary>
            public uint mem_unit;

        }

        /// <summary>
        /// Invokes the native linux method 'sysinfo' for a x64 platform.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "sysinfo")]
        static extern int sysinfo_x64(ref sysinfo_t_x64 info);

        /// <summary>
        /// Describes the rlimit structure for a x64 Linux platform.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct rlimit_t_x64
        {

            public ulong rlim_cur;
            public ulong rlim_max;

        }

        enum RLIMIT
        {
            FSIZE = 0,
            NOFILE = 1,
            CORE = 2,
            CPU = 3,
            DATA = 4,
            STACK = 5,
            AS = 6,
        }

        /// <summary>
        /// Invokes the native linux method 'getrlimit' for a x64 platform.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("libc", EntryPoint = "getrlimit")]
        static extern int getrlimit_x64(RLIMIT resource, ref rlimit_t_x64 info);

        /// <summary>
        /// Regular expression for the Linux /proc/pid/stat file.
        /// </summary>
        static readonly Regex LinuxProcStatRegex = new Regex(@"^\d+ \(.+\) [A-Za-z] \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ \d+ (\d+)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

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
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Process.GetCurrentProcess().PagedMemorySize64;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                try
                {
                    using var s = File.OpenRead("/proc/self/stat");
                    using var r = new StreamReader(s);

                    var l = r.ReadLine();
                    if (l != null && LinuxProcStatRegex.Match(l) is Match m && m.Groups.Count >= 2)
                        return (long)ulong.Parse(m.Groups[1].Value);

                    throw new global::java.lang.InternalError("Unable to get virtual memory usage");
                }
                catch (IOException e)
                {
                    throw new global::java.lang.InternalError("Unable to open or read /proc/self/stat", e);
                }
                catch (Exception e)
                {
                    throw new global::java.lang.InternalError("Unable to get virtual memory usage", e);
                }
            }
            else
            {
                return -1;
            }
#endif
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

        /// <summary>
        /// Implements the native method 'getOpenFileDescriptorCount'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getOpenFileDescriptorCount(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                try
                {
                    return Directory.GetFiles("/proc/self/fd").Count();
                }
                catch (Exception e)
                {
                    throw new global::java.lang.InternalError(e);
                }
            }
            else
            {
                return -1;
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getMaxFileDescriptorCount'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getMaxFileDescriptorCount(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && IntPtr.Size == 8)
            {
                var rlp = new rlimit_t_x64();
                if (getrlimit_x64(RLIMIT.NOFILE, ref rlp) == -1)
                    throw new global::java.lang.InternalError("getrlimit failed");

                return (long)rlp.rlim_cur;
            }
            else
            {
                return -1;
            }
#endif
        }

    }

}