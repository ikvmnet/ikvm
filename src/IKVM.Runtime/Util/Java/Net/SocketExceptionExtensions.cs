using System;
using System.Net.Sockets;

namespace IKVM.Runtime.Util.Java.Net
{

#if !FIRST_PASS

    /// <summary>
    /// Extensions for working with Sockets.
    /// </summary>
    static class SocketExceptionExtensions
    {

        /// <summary>
        /// Converts the given <see cref="SocketException"/> to a <see cref="global::java.io.IOException"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static global::java.io.IOException ToIOException(this SocketException self)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));

            switch (self.SocketErrorCode)
            {
                case SocketError.AddressAlreadyInUse:
                case SocketError.AddressNotAvailable:
                case SocketError.AccessDenied:
                    return new global::java.net.BindException(self.Message);
                case SocketError.NetworkUnreachable:
                case SocketError.HostUnreachable:
                    return new global::java.net.NoRouteToHostException(self.Message);
                case SocketError.TimedOut:
                    return new global::java.net.SocketTimeoutException(self.Message);
                case SocketError.ConnectionRefused:
                    return new global::java.net.PortUnreachableException(self.Message);
                case SocketError.ConnectionReset:
                    return new global::sun.net.ConnectionResetException(self.Message);
                case SocketError.HostNotFound:
                    return new global::java.net.UnknownHostException(self.Message);
                default:
                    return new global::java.net.SocketException($"{self.Message} [{self.SocketErrorCode}]");
            }
        }

    }

#endif

}
