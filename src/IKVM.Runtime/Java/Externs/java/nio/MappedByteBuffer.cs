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

using IKVM.Internal;

namespace IKVM.Java.Externs.java.nio
{

    static class MappedByteBuffer
    {

        private static volatile int bogusField;

        public static bool isLoaded0(object thisMappedByteBuffer, long address, long length, int pageCount)
        {
            // on Windows, JDK simply returns false, so we can get away with that too.
            return false;
        }

        [SecuritySafeCritical]
        public static void load0(object thisMappedByteBuffer, long address, long length)
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
            GC.KeepAlive(thisMappedByteBuffer);
        }

        [SecuritySafeCritical]
        public static void force0(object thisMappedByteBuffer, object fd, long address, long length)
        {
            if (JVM.IsUnix)
            {
                ikvm_msync((IntPtr)address, (int)length);
                GC.KeepAlive(thisMappedByteBuffer);
            }
            else
            {
                // according to the JDK sources, FlushViewOfFile can fail with an ERROR_LOCK_VIOLATION error,
                // so like the JDK, we retry up to three times if that happens.
                for (int i = 0; i < 3; i++)
                {
                    if (FlushViewOfFile((IntPtr)address, (IntPtr)length) != 0)
                    {
                        GC.KeepAlive(thisMappedByteBuffer);
                        return;
                    }
                    const int ERROR_LOCK_VIOLATION = 33;
                    if (Marshal.GetLastWin32Error() != ERROR_LOCK_VIOLATION)
                    {
                        break;
                    }
                }

#if !FIRST_PASS
                throw new global::java.io.IOException("Flush failed");
#endif
            }
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern int FlushViewOfFile(IntPtr lpBaseAddress, IntPtr dwNumberOfBytesToFlush);

        private static int ikvm_msync(IntPtr address, int size) => msync(address, size, 0x4);

        [DllImport("libc", EntryPoint = "msync")]
        private static extern int msync(IntPtr address, int size, int flags);

    }

}
