using System.Net;

namespace IKVM.Runtime.Util.Java.Net
{

    /// <summary>
    /// Extension methods for working with Internet Socket Addresses.
    /// </summary>
    static class InetSocketAddressExtensions
    {

#if FIRST_PASS == false

        /// <summary>
        /// Gets a <see cref="IPEndPoint"/> for the given <see cref="global::java.net.InetSocketAddress"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IPEndPoint ToIPEndpoint(this global::java.net.InetSocketAddress self)
        {
            if (self == null)
                return null;
            else
                return new IPEndPoint(self.getAddress().ToIPAddress(), self.getPort());
        }

        /// <summary>
        /// Gets a <see cref="global::java.net.InetSocketAddress"/> for the given <see cref="IPEndPoint"/>.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static global::java.net.InetSocketAddress ToInetSocketAddress(this IPEndPoint endpoint)
        {
            return endpoint != null ? new java.net.InetSocketAddress(endpoint.Address.ToInetAddress(endpoint.Address.ToString()), endpoint.Port) : null;
        }

#endif

    }

}
