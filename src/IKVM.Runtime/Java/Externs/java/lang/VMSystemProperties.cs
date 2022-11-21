/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using IKVM.Internal;
using IKVM.Runtime.Vfs;

#if NETCOREAPP
using Mono.Unix.Native;
#endif

namespace IKVM.Java.Externs.java.lang
{

    static class VMSystemProperties
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct OSVERSIONINFOEXW
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public ushort wServicePackMajor;
            public ushort wServicePackMinor;
            public ushort wSuiteMask;
            public VER_NT wProductType;
            public byte wReserved;
        }

        /// <summary>
        /// VER_NT_* values.
        /// </summary>
        enum VER_NT : byte
        {

            VER_NT_DOMAIN_CONTROLLER = 0x0000002,
            VER_NT_SERVER = 0x0000003,
            VER_NT_WORKSTATION = 0x0000001,

        }

        /// <summary>
        /// Set of properties to initially import upon startup.
        /// </summary>
        public static IDictionary ImportProperties { get; set; }

        /// <summary>
        /// Gets the <see cref="Assembly"/> of the runtime.
        /// </summary>
        /// <returns></returns>
        public static Assembly getRuntimeAssembly()
        {
            return typeof(VMSystemProperties).Assembly;
        }

        /// <summary>
        /// Gets the RID architecture.
        /// </summary>
        /// <returns></returns>
        static string GetRuntimeIdentifierArchitecture() => RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x64",
            Architecture.Arm => "arm",
            Architecture.Arm64 => "arm64",
            _ => throw new NotSupportedException(),
        };

        /// <summary>
        /// Returns the architecture name of the ikvm.home directory to use for this run.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetIkvmHomeArchsEnumerator()
        {
            var arch = GetRuntimeIdentifierArchitecture();
            if (arch == null)
                yield break;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var v = Environment.OSVersion.Version;

                // Windows 10
                if (v.Major > 10 || (v.Major == 10 && v.Minor >= 0))
                    yield return $"win10-{arch}";

                // Windows 8.1
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 3))
                    yield return $"win81-{arch}";

                // Windows 7
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 1))
                    yield return $"win7-{arch}";

                // fallback
                yield return $"win-{arch}";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return $"linux-{arch}";
            }
        }

        /// <summary>
        /// Returns the architecture name of the ikvm.home directory to use for this run.
        /// </summary>
        /// <returns></returns>
        public static string[] getIkvmHomeArchs()
        {
            return GetIkvmHomeArchsEnumerator().ToArray();
        }

        /// <summary>
        /// Gets the path to the root of the VFS.
        /// </summary>
        /// <returns></returns>
        public static string getVirtualFileSystemRoot()
        {
            return VfsTable.RootPath;
        }

        public static string getBootClassPath()
        {
            return VfsTable.Default.GetAssemblyClassesPath(JVM.CoreAssembly);
        }

        public static string getStdoutEncoding()
        {
            return IsWindowsConsole(true) ? GetConsoleEncoding() : null;
        }

        public static string getStderrEncoding()
        {
            return IsWindowsConsole(false) ? GetConsoleEncoding() : null;
        }

        public static FileVersionInfo getKernel32FileVersionInfo()
        {
            try
            {
                foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                    if (string.Compare(module.ModuleName, "kernel32.dll", StringComparison.OrdinalIgnoreCase) == 0)
                        return module.FileVersionInfo;
            }
            catch
            {

            }

            return null;
        }

        static bool IsWindowsConsole(bool stdout)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;
            else
                return stdout ? !Console.IsOutputRedirected : !Console.IsErrorRedirected;
        }

        static string GetConsoleEncoding()
        {
            var codepage = Console.InputEncoding.CodePage;
            return codepage is >= 847 and <= 950 ? $"ms{codepage}" : $"cp{codepage}";
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int RtlGetVersion(ref OSVERSIONINFOEXW versionInfo);

        /// <summary>
        /// Gets the Windows ProductType.
        /// </summary>
        /// <returns></returns>
        public static byte getWindowsProductType()
        {
            var osvi = default(OSVERSIONINFOEXW);
            osvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEXW));
            var result = RtlGetVersion(ref osvi);
            if (result != 0)
            {
                return 0;
            }
            else
            {
                return (byte)osvi.wProductType;
            }
        }

        /// <summary>
        /// Gets the 'sysname' on Unix.
        /// </summary>
        /// <returns></returns>
        public static string[] getLinuxSysnameAndRelease()
        {
#if NETFRAMEWORK
            return null;
#else
            Syscall.uname(out var utsname);
            return new[] { utsname.sysname, utsname.release };
#endif
        }

    }

}
