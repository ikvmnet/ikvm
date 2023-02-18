using System;
using System.Security.Cryptography;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the external methods for <see cref="global::sun.nio.ch.IOUtil"/>.
    /// </summary>
    static class IOUtil
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Implements the native method 'initIDs'.
        /// </summary>
        public static void initIDs()
        {

        }

        /// <summary>
        /// Implements the native method 'randomBytes'.
        /// </summary>
        public static bool randomBytes(byte[] someBytes)
        {
            rng.GetBytes(someBytes);
            return true;
        }

        /// <summary>
        /// Implements the native method 'makePipe'.
        /// </summary>
        public static long makePipe(bool blocking)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implements the native method 'drain'.
        /// </summary>
        public static bool drain(int fd)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Implements the native method 'configureBlocking'.
        /// </summary>
        public static void configureBlocking(object fd, bool blocking)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(fd);
            if (socket == null)
                throw new global::java.io.IOException("Socket closed.");

            socket.Blocking = blocking;
#endif
        }

        /// <summary>
        /// Implements the native method 'fdVal'.
        /// </summary>
        public static int fdVal(object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return FileDescriptorAccessor.GetFd(fd);
#endif
        }

        /// <summary>
        /// Implements the native method 'setfdVal'.
        /// </summary>
        public static void setfdVal(global::java.io.FileDescriptor fd, int value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Implements the native method 'fdLimit'.
        /// </summary>
        public static int fdLimit()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Implements the native method 'iovMax'.
        /// </summary>
        public static int iovMax()
        {
            return 16;
        }

        /// <summary>
        /// Implements the native method 'load'.
        /// </summary>
        public static void load()
        {

        }

    }

}