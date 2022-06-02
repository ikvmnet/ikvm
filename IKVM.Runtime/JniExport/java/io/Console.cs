/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Runtime.InteropServices;

namespace IKVM.Runtime.JniExport.java.io
{

    static class Console
    {

        public static string encoding()
        {
            int cp = 437;
            try
            {
                cp = System.Console.InputEncoding.CodePage;
            }
            catch
            {

            }

            if (cp >= 874 && cp <= 950)
                return "ms" + cp;

            return "cp" + cp;
        }

        private const int STD_INPUT_HANDLE = -10;
        private const int ENABLE_ECHO_INPUT = 0x0004;

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32")]
        private static extern int GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("kernel32")]
        private static extern int SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        public static bool echo(bool on)
        {
#if !FIRST_PASS
            // HACK the only way to get this to work is by p/invoking the Win32 APIs
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                IntPtr hStdIn = GetStdHandle(STD_INPUT_HANDLE);
                if (hStdIn.ToInt64() == 0 || hStdIn.ToInt64() == -1)
                {
                    throw new global::java.io.IOException("The handle is invalid");
                }
                int fdwMode;
                if (GetConsoleMode(hStdIn, out fdwMode) == 0)
                {
                    throw new global::java.io.IOException("GetConsoleMode failed");
                }
                bool old = (fdwMode & ENABLE_ECHO_INPUT) != 0;
                if (on)
                {
                    fdwMode |= ENABLE_ECHO_INPUT;
                }
                else
                {
                    fdwMode &= ~ENABLE_ECHO_INPUT;
                }
                if (SetConsoleMode(hStdIn, fdwMode) == 0)
                {
                    throw new global::java.io.IOException("SetConsoleMode failed");
                }
                return old;
            }
#endif
            return true;
        }

        public static bool istty()
        {
            // The JDK returns false here if stdin or stdout (not stderr) is redirected to a file
            // or if there is no console associated with the current process.
            // The best we can do is to look at the KeyAvailable property, which
            // will throw an InvalidOperationException if stdin is redirected or not available
            try
            {
                return System.Console.KeyAvailable || true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

    }

}
