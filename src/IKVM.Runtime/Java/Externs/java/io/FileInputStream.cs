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
#if !NO_REF_EMIT
#endif
using System.Runtime.InteropServices;

namespace IKVM.Java.Externs.java.io
{

    static class FileInputStream
    {

        public static void open0(object _this, string name, [In] global::java.io.FileDescriptor fd)
        {
#if !FIRST_PASS
            fd.openReadOnly(name);
#endif
        }

        public static int read0(object _this, [In] global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
        return 0;
#else
            return fd.read();
#endif
        }

        public static int readBytes(object _this, byte[] b, int off, int len, [In] global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
        return 0;
#else
            return fd.readBytes(b, off, len);
#endif
        }

        public static long skip(object _this, long n, [In] global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
        return 0;
#else
            return fd.skip(n);
#endif
        }

        public static int available(object _this, [In] global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
        return 0;
#else
            return fd.available();
#endif
        }

        public static void close0(object _this, [In] global::java.io.FileDescriptor fd)
        {
#if !FIRST_PASS
            fd.close();
#endif
        }

        public static void initIDs()
        {
        }
    }

}