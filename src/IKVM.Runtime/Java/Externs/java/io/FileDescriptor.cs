using System;
using System.IO;
using System.Net.Sockets;

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

        /// <summary>
        /// Implements the native method 'getStream'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Stream getStream(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return FileDescriptorAccessor.GetStream(self);
#endif
        }

        /// <summary>
        /// Implements the native method 'getSocket'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Socket getSocket(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return FileDescriptorAccessor.GetSocket(self);
#endif
        }

        /// <summary>
        /// Sets the underlying pointer object. If the pointer value changes, any existing cached object is removed.
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
                    var obj = (IDisposable)FileDescriptorAccessor.GetObj(self);
                    if (obj != null)
                    {
                        obj.Dispose();
                        FileDescriptorAccessor.SetObj(self, null);
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
            var stream = fd switch
            {
                0 => System.Console.OpenStandardInput(),
                1 => System.Console.OpenStandardOutput(),
                2 => System.Console.OpenStandardError(),
                _ => throw new NotImplementedException(),
            };

            var fdo = FileDescriptorAccessor.Init();
            FileDescriptorAccessor.SetPtr(fdo, fd);
            FileDescriptorAccessor.SetObj(fdo, stream);
            return fdo;
#endif
        }

    }

}
