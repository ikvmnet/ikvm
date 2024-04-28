using System;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;

namespace IKVM.Java.Externs.java.io
{

    /// <summary>
    /// Implements the native methods for 'FileDescriptor'.
    /// </summary>
    static class FileDescriptor
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.Internal.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        /// <summary>
        /// Sets the underlying pointer object. Disposes any associated stream.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ptr"></param>
        static void SetPtr(object self, long ptr)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (self)
            {
                var p = FileDescriptorAccessor.GetPtr(self);
                if (p != ptr || ptr == -1)
                {
                    var stream = (IDisposable)FileDescriptorAccessor.GetStream(self);
                    if (stream != null)
                    {
                        stream.Dispose();
                        FileDescriptorAccessor.SetStream(self, null);
                    }
                }

                FileDescriptorAccessor.SetPtr(self, ptr);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'getFd'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int getFd(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (self)
                return (int)FileDescriptorAccessor.GetPtr(self);
#endif
        }

        /// <summary>
        /// Implements the native method 'setFd'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void setFd(object self, int value)
        {
            SetPtr(self, value);
        }

        /// <summary>
        /// Implements the native method 'getHandle'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getHandle(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            lock (self)
                return FileDescriptorAccessor.GetPtr(self);
#endif
        }

        /// <summary>
        /// Implements the native method 'setHandle'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void setHandle(object self, long value)
        {
            SetPtr(self, value);
        }

        /// <summary>
        /// Implements the native method 'standardStream'.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static object standardStream(int fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fdo = FileDescriptorAccessor.Init();
            FileDescriptorAccessor.SetPtr(fdo, GetStandardHandle(fd));
            return fdo;
#endif
        }

        [DllImport("Kernel32")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        /// Gets the standard handle for the given fd index.
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        static long GetStandardHandle(int fd)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (fd == 0)
                    return (long)GetStdHandle(-10);
                else if (fd == 1)
                    return (long)GetStdHandle(-11);
                else if (fd == 2)
                    return (long)GetStdHandle(-12);
            }

            return fd;
        }

    }
}