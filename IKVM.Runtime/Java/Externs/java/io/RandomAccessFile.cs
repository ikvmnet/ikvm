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
using System.Runtime.InteropServices;

namespace IKVM.Java.Externs.java.io
{

    static class RandomAccessFile
    {

        public static void open0(object _this, string name, int mode, [In] global::java.io.FileDescriptor fd, [In] int O_RDWR)
        {
#if !FIRST_PASS
            if ((mode & O_RDWR) == O_RDWR)
            {
                fd.openReadWrite(name);
            }
            else
            {
                fd.openReadOnly(name);
            }
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

        public static void write0(object _this, int b, [In] global::java.io.FileDescriptor fd)
        {
#if !FIRST_PASS
            fd.write(b);
#endif
        }

        public static void writeBytes(object _this, byte[] b, int off, int len, [In] global::java.io.FileDescriptor fd)
        {
#if !FIRST_PASS
            fd.writeBytes(b, off, len);
#endif
        }

        public static long getFilePointer(object _this, [In] global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
        return 0;
#else
            return fd.getFilePointer();
#endif
        }

        public static void seek0(object _this, long pos, [In] global::java.io.FileDescriptor fd)
        {
#if !FIRST_PASS
            fd.seek(pos);
#endif
        }

        public static long length(object _this, [In] global::java.io.FileDescriptor fd)
        {
#if FIRST_PASS
        return 0;
#else
            return fd.length();
#endif
        }

        public static void setLength(object _this, long newLength, [In] global::java.io.FileDescriptor fd)
        {
#if !FIRST_PASS
            fd.setLength(newLength);
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