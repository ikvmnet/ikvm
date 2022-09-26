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

        /// <summary>
        /// Gets the <see cref="Assembly"/> of the runtime.
        /// </summary>
        /// <returns></returns>
        public static Assembly getRuntimeAssembly()
        {
            return typeof(VMSystemProperties).Assembly;
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


    }

}
