using System;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Internal;
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
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        [DllImport("kernel32")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        /// Implements the native method 'getHandle0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int getFd(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
            {
                if (FileDescriptorAccessor.GetStream(self) is FileStream fs)
                {
                    return fs.SafeFileHandle.DangerousGetHandle().ToInt32();
                }
                else if (self == FileDescriptorAccessor.GetIn())
                {
                    return GetStdHandle(-10).ToInt32();
                }
                else if (self == FileDescriptorAccessor.GetOut())
                {
                    return GetStdHandle(-11).ToInt32();
                }
                else if (self == FileDescriptorAccessor.GetErr())
                {
                    return GetStdHandle(-12).ToInt32();
                }
            }
            else
            {

                if (FileDescriptorAccessor.GetStream(self) is FileStream fs)
                {
                    return fs.SafeFileHandle.DangerousGetHandle().ToInt32();
                }
                else if (self == FileDescriptorAccessor.GetIn())
                {
                    return 0;
                }
                else if (self == FileDescriptorAccessor.GetOut())
                {
                    return 1;
                }
                else if (self == FileDescriptorAccessor.GetErr())
                {
                    return 2;
                }
            }

            return -1;
#endif
        }

        /// <summary>
        /// Implements the native method 'getHandle0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getHandle(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (RuntimeUtil.IsWindows)
            {
                if (FileDescriptorAccessor.GetStream(self) is FileStream fs)
                {
                    return fs.SafeFileHandle.DangerousGetHandle().ToInt64();
                }
                else if (self == FileDescriptorAccessor.GetIn())
                {
                    return GetStdHandle(-10).ToInt64();
                }
                else if (self == FileDescriptorAccessor.GetOut())
                {
                    return GetStdHandle(-11).ToInt64();
                }
                else if (self == FileDescriptorAccessor.GetErr())
                {
                    return GetStdHandle(-12).ToInt64();
                }
            }

            return -1;
#endif
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
            return FileDescriptorAccessor.FromStream(fd switch
            {
                0 => System.Console.OpenStandardInput(),
                1 => System.Console.OpenStandardOutput(),
                2 => System.Console.OpenStandardError(),
                _ => throw new NotImplementedException(),
            });
#endif
        }

        /// <summary>
        /// Implements the native method 'sync'.
        /// </summary>
        /// <param name="self"></param>
        /// <exception cref="global::java.io.SyncFailedException"></exception>
        public static void sync(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stream = FileDescriptorAccessor.GetStream(self);
            if (stream == null)
                throw new global::java.io.SyncFailedException("Sync failed.");
            if (stream.CanWrite == false)
                return;

            try
            {
                stream.Flush();
            }
            catch (IOException e)
            {
                throw new global::java.io.SyncFailedException(e.Message);
            }
#endif
        }

    }

}