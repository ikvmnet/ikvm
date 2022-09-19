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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using IKVM.Internal;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.lang
{

    static class VMSystemProperties
    {

        /// <summary>
        /// Set of properties to initially import upon startup.
        /// </summary>
        public static IDictionary ImportProperties { get; set; }

        public static string getVirtualFileSystemRoot()
        {
            return VfsTable.HomePath;
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

            // these properties are available starting with .NET 4.5
            var pi = typeof(Console).GetProperty(stdout ? "IsOutputRedirected" : "IsErrorRedirected");
            if (pi != null)
                return !(bool)pi.GetValue(null, null);

            const int STD_OUTPUT_HANDLE = -11;
            const int STD_ERROR_HANDLE = -12;
            var handle = GetStdHandle(stdout ? STD_OUTPUT_HANDLE : STD_ERROR_HANDLE);
            if (handle == IntPtr.Zero)
                return false;

            const int FILE_TYPE_CHAR = 2;
            return GetFileType(handle) == FILE_TYPE_CHAR;
        }

        static string GetConsoleEncoding()
        {
            int codepage = Console.InputEncoding.CodePage;
            return codepage >= 847 && codepage <= 950
                ? "ms" + codepage
                : "cp" + codepage;
        }

        [DllImport("kernel32")]
        static extern int GetFileType(IntPtr hFile);

        [DllImport("kernel32")]
        static extern IntPtr GetStdHandle(int nStdHandle);

    }

}
