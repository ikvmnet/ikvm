using System.Net.Sockets;

namespace IKVM.Runtime.Util.Java.Net
{

#if !FIRST_PASS

    /// <summary>
    /// Provides a map of Java socket information to .NET socket information.
    /// </summary>
    static class SocketOptionMap
    {

        /// <summary>
        /// Describes a mapping between a <see cref="global::java.net.SocketOptions"/> and it's associated .NET information.
        /// </summary>
        struct SocketOptionMapItem
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="option"></param>
            /// <param name="level"></param>
            /// <param name="name"></param>
            public SocketOptionMapItem(int option, SocketOptionLevel level, SocketOptionName name)
            {
                Option = option;
                Level = level;
                Name = name;
            }

            /// <summary>
            /// Gets the option.
            /// </summary>
            public int Option { get; set; }

            /// <summary>
            /// Gets the <see cref="SocketOptionLevel"/>.
            /// </summary>
            public SocketOptionLevel Level { get; set; }

            /// <summary>
            /// Gets the <see cref="SocketOptionName"/>.
            /// </summary>
            public SocketOptionName Name { get; set; }

        }

        readonly static SocketOptionMapItem[] map = new[]
        {
            new SocketOptionMapItem(global::java.net.SocketOptions.TCP_NODELAY,         SocketOptionLevel.Tcp,      SocketOptionName.NoDelay),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_OOBINLINE,        SocketOptionLevel.Socket,   SocketOptionName.OutOfBandInline),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_LINGER,           SocketOptionLevel.Socket,   SocketOptionName.Linger),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_SNDBUF,           SocketOptionLevel.Socket,   SocketOptionName.SendBuffer),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_RCVBUF,           SocketOptionLevel.Socket,   SocketOptionName.ReceiveBuffer),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_KEEPALIVE,        SocketOptionLevel.Socket,   SocketOptionName.KeepAlive),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_REUSEADDR,        SocketOptionLevel.Socket,   SocketOptionName.ReuseAddress),
            new SocketOptionMapItem(global::java.net.SocketOptions.SO_BROADCAST,        SocketOptionLevel.Socket,   SocketOptionName.Broadcast),
            new SocketOptionMapItem(global::java.net.SocketOptions.IP_MULTICAST_IF,     SocketOptionLevel.IP,       SocketOptionName.MulticastInterface),
            new SocketOptionMapItem(global::java.net.SocketOptions.IP_MULTICAST_LOOP,   SocketOptionLevel.IP,       SocketOptionName.MulticastLoopback),
            new SocketOptionMapItem(global::java.net.SocketOptions.IP_TOS,              SocketOptionLevel.IP,       SocketOptionName.TypeOfService),
        };

        /// <summary>
        /// Gets the socket option map item from the given option.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool TryGetDotNetSocketOption(int option, out (SocketOptionLevel Level, SocketOptionName Name) options)
        {
            foreach (var o in map)
            {
                if (o.Option == option)
                {
                    options = (o.Level, o.Name);
                    return true;
                }
            }

            options = default;
            return false;
        }

        /// <summary>
        /// Gets the socket option map item from the given option.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="name"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool TryGetJavaSocketOptions(SocketOptionLevel level, SocketOptionName name, out int option)
        {
            foreach (var o in map)
            {
                if (o.Level == level && o.Name == name)
                {
                    option = o.Option;
                    return true;
                }
            }

            option = default;
            return false;
        }

    }

#endif

}
