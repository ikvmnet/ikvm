using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Util.Java.Net;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for 'ServerSocketChannelImpl'.
    /// </summary>
    static class ServerSocketChannelImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

#endif

        [Flags]
        enum HANDLE_FLAGS
        {

            NONE = 0,
            INHERIT = 0x00000001,
            PROTECT_FROM_CLOSE = 0x00000002,

        }

#if NETFRAMEWORK

        /// <summary>
        /// Invokes the Win32 SetHandleInformation function.
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="dwMask"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetHandleInformation(IntPtr hObject, HANDLE_FLAGS dwMask, HANDLE_FLAGS dwFlags);
#else

        /// <summary>
        /// Invokes the Win32 SetHandleInformation function.
        /// </summary>
        /// <param name="hObject"></param>
        /// <param name="dwMask"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetHandleInformation(SafeHandle hObject, HANDLE_FLAGS dwMask, HANDLE_FLAGS dwFlags);

#endif


        /// <summary>
        /// Implements the native method 'accept0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="ssfd"></param>
        /// <param name="newfd"></param>
        /// <param name="isaa"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static int accept0(object self, object ssfd, object newfd, object isaa)
        {
#if FIRST_PASS
            throw new NotSupportedException();
#else
            var socket = FileDescriptorAccessor.GetSocket(ssfd);
            if (socket == null)
                throw new global::java.io.IOException("Socket closed.");

            try
            {
                if (socket.Blocking || socket.Poll(0, SelectMode.SelectRead))
                {
                    var newSocket = socket.Accept();
                    if (newSocket == null)
                        throw new global::java.net.SocketException("Invalid socket.");

                    FileDescriptorAccessor.SetSocket(newfd, newSocket);
                    var ep = (System.Net.IPEndPoint)newSocket.RemoteEndPoint;
                    ((object[])isaa)[0] = ep.ToInetSocketAddress();
                    return 1;
                }
                else
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
            }
            catch (SocketException e)
            {
                throw e.ToIOException();
            }
            catch (ObjectDisposedException)
            {
                throw new global::java.net.SocketException("Socket closed.");
            }
            catch (Exception e)
            {
                throw new global::java.io.IOException(e);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'initIDs'.
        /// </summary>
        public static void initIDs()
        {

        }

    }

}
