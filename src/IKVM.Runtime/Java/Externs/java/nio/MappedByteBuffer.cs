/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System.Security;

using IKVM.Runtime;

using Mono.Unix.Native;

namespace IKVM.Java.Externs.java.nio
{

    static class MappedByteBuffer
    {

        static volatile int bogusField;

        public static bool isLoaded0(object self, long address, long length, int pageCount)
        {
            // on Windows, JDK simply returns false, so we can get away with that too.
            return false;
        }

        [SecuritySafeCritical]
        public static void load0(object self, long address, long length)
        {
            int bogus = bogusField;
            while (length > 0)
            {
                // touch a byte in every page
                bogus += Marshal.ReadByte((IntPtr)address);
                length -= 4096;
                address += 4096;
            }

            // do a volatile store of the sum of the bytes to make sure the reads don't get optimized out
            bogusField = bogus;
            GC.KeepAlive(self);
        }

        [SecuritySafeCritical]
        public static void force0(object self, object fd, long address, long length)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsLinux || RuntimeUtil.IsOSX)
            {
                Syscall.msync((IntPtr)address, (ulong)length, MsyncFlags.MS_SYNC);
                GC.KeepAlive(self);
            }
            else
            {
                // according to the JDK sources, FlushViewOfFile can fail with an ERROR_LOCK_VIOLATION error,
                // so like the JDK, we retry up to three times if that happens.
                for (int i = 0; i < 3; i++)
                {
                    if (FlushViewOfFile((IntPtr)address, (IntPtr)length) != 0)
                    {
                        GC.KeepAlive(self);
                        return;
                    }

                    const int ERROR_LOCK_VIOLATION = 33;
                    if (Marshal.GetLastWin32Error() != ERROR_LOCK_VIOLATION)
                        break;
                }

                throw new global::java.io.IOException("Flush failed");
            }
#endif
        }

        [DllImport("kernel32", SetLastError = true)]
        static extern int FlushViewOfFile(IntPtr lpBaseAddress, IntPtr dwNumberOfBytesToFlush);

    }

}
