using System.Net;

namespace IKVM.Runtime.Java.Externs.java.net
{

    /// <summary>
    /// Extension methods for working with Internet Addrsses.
    /// </summary>
    static class InetAddressExtensions
    {

#if !FIRST_PASS

        /// <summary>
        /// Gets a <see cref="IPAddress"/> for the given <see cref="global::java.net.InetAddress"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(this global::java.net.InetAddress self)
        {
            if (self == null)
                return null;
            else if (self is global::java.net.Inet6Address ip6)
                return new IPAddress(self.getAddress(), ip6.getScopeId());
            else
                return new IPAddress(self.getAddress());
        }

        /// <summary>
        /// Gets a <see cref="global::java.net.InetAddress"/> for the given <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static global::java.net.InetAddress ToInetAddress(this IPAddress address)
        {
            return address != null ? address.ToInetAddress(address.ToString()) : null;
        }

        /// <summary>
        /// Gets a <see cref="global::java.net.InetAddress"/> for the given <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="hostname"></param>
        /// <returns></returns>
        public static global::java.net.InetAddress ToInetAddress(this IPAddress address, string hostname)
        {
            if (address == null)
                return null;
            else if (address.IsIPv6LinkLocal || address.IsIPv6SiteLocal)
                return global::java.net.Inet6Address.getByAddress(hostname, address.GetAddressBytes(), (int)address.ScopeId);
            else
                return global::java.net.InetAddress.getByAddress(hostname, address.GetAddressBytes());
        }

        /// <summary>
        /// Gets a <see cref="global::java.net.InetAddress"/> for the given <see cref="IPEndPoint"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static global::java.net.InetAddress ToInetAddress(this IPEndPoint self)
        {
            return self != null ? self.Address.ToInetAddress() : null;
        }

#endif

    }

}
